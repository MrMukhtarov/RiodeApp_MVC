using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeApp.DataAccess;
using RiodeApp.Models;

namespace RiodeApp.Controllers;

public class AuthController : Controller
{
    readonly UserManager<AppUser> _userManager;
    readonly SignInManager<AppUser> _signInManager;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly IEmailService _emailService;
    readonly RiodeDbContext _context;

    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, RiodeDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailService = emailService;
        _context = context;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid) return View();
        AppUser user = new AppUser
        {
            FullName = vm.Name + " " + vm.Surname,
            Email = vm.Email,
            UserName = vm.Username
        };
        var result = await _userManager.CreateAsync(user, vm.Password);
        if (!result.Succeeded)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            return View();
        }

        var res = await _userManager.AddToRoleAsync(user, "Member");



        if (!res.Succeeded)
        {
            foreach (var item in res.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            return View();
        }

        return RedirectToAction(nameof(ConfirmationEmailSent), new { Username = user.UserName });
    }


    public async Task<IActionResult> EmailConfirmation(string token, string user)
    {
        if (String.IsNullOrWhiteSpace(token) || String.IsNullOrWhiteSpace(user)) return BadRequest();
        var us = await _userManager.FindByNameAsync(user);
        await _userManager.ConfirmEmailAsync(us, token);
        _context.UserTokens.Remove(await _context.UserTokens.SingleOrDefaultAsync(ut => ut.AppUserId == us.Id &&
        ut.Key == "EmailConfirmation"));
        _context.SaveChangesAsync();
        return Content("Email Confirmed");
    }

    public async Task<IActionResult> ConfirmationEmailSent(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return BadRequest();
        var utoken = await _context.UserTokens.SingleOrDefaultAsync(ut => ut.AppUserId == user.Id && ut.Key == "EmailConfirmation");

        if (utoken?.SendDate.AddHours(24) > DateTime.UtcNow)
        {
            return Content("Mail already sent.If you dont see in your inbox.Please check your spams.You can send confirmation email after" + utoken.SendDate.AddHours(24));
        }
        else if (utoken?.SendDate.AddHours(24) < DateTime.UtcNow)
        {
            _context.Remove(utoken);
        }

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string link = Url.Action("EmailConfirmation", "Auth", new { token = token, user = user.UserName },
            Request.Scheme);
        string text = await System.IO.File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory()
            , "wwwroot", "EmailConfirmation.html"));
        text = text.Replace("[[username]]", user.FullName);
        text = text.Replace("[[link]]", link);

        _emailService.Send(user.Email, "Email Confirmation", text, true);

        await _context.UserTokens.AddAsync(new UserToken
        {
            AppUser = user,
            Key = "EmailConfirmation",
            SendDate = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        return Content("Confirmation email send succesfully!");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string? ReturnUrl, LoginVM vm)
    {
        if (!ModelState.IsValid) return View();

        var user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "Username,email or password is wrong");
                return View();
            }
        }
        var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);
        if (result.IsLockedOut)
        {
            ModelState.AddModelError("", "Wait until " + 
                user.LockoutEnd.Value.AddHours(4).ToString("HH:mm:ss"));
            return View();
        }
        if (!user.EmailConfirmed)
        {
            var utoken = await _context.UserTokens.SingleOrDefaultAsync
                (ut => ut.AppUserId == user.Id && ut.Key == "EmailConfirmation");

            if (utoken?.SendDate.AddHours(24) > DateTime.UtcNow)
            {
                ModelState.AddModelError("", "Mail already sent.If you dont see in your inbox.Please check your spams.You can send confirmation email after" + utoken.SendDate.AddHours(24));
            }
            else
            {
                ViewBag.ConfirmationMessage = $"Please confirm your account <a href='/Auth/ConfirmationEmailSent?username={user.UserName}'>Send Email </a>";
                return View();
            }
            return View();
        }
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Username,email or password is wrong");
            return View();
        }
        if (ReturnUrl == null)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return Redirect(ReturnUrl);
        }
    }   
    public async Task<IActionResult> Signout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
    //public async Task CreateRoles()
    //{
    //    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
    //    await _roleManager.CreateAsync(new IdentityRole { Name = "Editor" });
    //    await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
    //}
}