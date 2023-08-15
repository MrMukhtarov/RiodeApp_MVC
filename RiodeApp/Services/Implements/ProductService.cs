using Microsoft.EntityFrameworkCore;
using RiodeApp.DataAccess;
using RiodeApp.ExtensionServices.Interfaces;
using RiodeApp.Models;
using RiodeApp.Services.Interfaces;
using RiodeApp.ViewModels.ProductVMs;

namespace RiodeApp.Services.Implements;

public class ProductService : IProductService
{
    readonly RiodeDbContext _context;
    readonly IFileService _fileService;
    readonly ICategoryService _categoryService;

    public ProductService(RiodeDbContext context, IFileService fileService, ICategoryService categoryService)
    {
        _context = context;
        _fileService = fileService;
        _categoryService = categoryService;
    }
    public IQueryable<Product> GetTable { get => _context.Set<Product>(); }
    public async Task<ICollection<Product>> GetAll(bool TakeAll)
    {
        if (TakeAll)
        {
            return await _context.Products.ToListAsync();
        }
        return await _context.Products.Where(p => p.IsDeleted == false).ToListAsync();
    }

    public async Task<Product> GetById(int? id, bool takeAll = false)
    {
        if (id == null || id <= 0) throw new ArgumentNullException();
        Product? entity;
        if (takeAll)
        {
            entity = await _context.Products.FindAsync(id);
        }
        else
        {
            entity = await _context.Products.SingleOrDefaultAsync(p => p.IsDeleted == false && p.Id == id);
        }
        if (entity == null) throw new ArgumentNullException();
        return entity;
    }
    public async Task Create(CreateProductVM vm)
    {
        if(vm.CategoryIds.Count > 4)
        {
            throw new Exception();
        }
        if (!await _categoryService.IsAllExist(vm.CategoryIds))
        {
            throw new ArgumentException();
        }
        List<ProductCategory> prodCategories = new List<ProductCategory>();
        foreach (var id in vm.CategoryIds)
        {
            prodCategories.Add(new ProductCategory
            {
                CategoryId = id,
            });
        }
        Product entity = new Product()
        {
            Name = vm.Name,
            Description = vm.Description,
            Price = vm.Price,
            Discount = vm.Discount,
            Rating = vm.Rating,
            SalesCount = vm.SalesCount,
            StockCount = vm.StockCount,
            MainImage = await _fileService.UploadAsync(vm.MainImageFile, Path.Combine("Assets", "imgs", "Products")),
            ProductCategories = prodCategories
        };
        if (vm.ImageFiles != null)
        {
            List<ProductImage> images = new List<ProductImage>();
            foreach (var item in vm.ImageFiles)
            {
                string filename = await _fileService.UploadAsync(item, Path.Combine("Assets", "imgs", "Products"));

                images.Add(new ProductImage
                {
                    Name = filename
                });
            }
            entity.ProductImages = images;
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Update(UpdateProductVM vm, int? id)
    {
        if(vm.CategoryIds.Count > 4)
        {
            throw new Exception();
        }
        if(!await _categoryService.IsAllExist(vm.CategoryIds))
        {
            throw new ArgumentException();
        }
        List<ProductCategory> productCategories = new List<ProductCategory>();

        foreach (var category in vm.CategoryIds)
        {
            productCategories.Add(new ProductCategory
            {
                CategoryId = category
            });
        }
        var entity = await _context.Products.Include(p => p.ProductCategories).
            SingleOrDefaultAsync(pc => pc.Id == id);
        if(entity == null)
        {
            throw new NullReferenceException();
        }
        if(entity.ProductCategories != null)
        {
            entity.ProductCategories.Clear();
        }

        entity.Name = vm.Name;
        entity.Description = vm.Description;
        entity.Price = vm.Price;
        entity.Discount = vm.Discount;
        entity.Rating = vm.Rating;
        entity.StockCount = vm.StockCount;
        entity.ProductCategories = productCategories;
        if (vm.MainImageFile != null)
        {
            _fileService.Delete(entity.MainImage);
            entity.MainImage = await _fileService.UploadAsync(vm.MainImageFile,
                Path.Combine("Assets", "imgs", "Products"));
        }
        if (vm.ProductImageFiles != null)
        {
            if (entity.ProductImages == null) entity.ProductImages = new List<ProductImage>();
            foreach (var item in vm.ProductImageFiles)
            {
                ProductImage prodImf = new ProductImage
                {
                    Name = await _fileService.UploadAsync(item, Path.Combine("Assets", "imgs", "Products"))
                };
                entity.ProductImages.Add(prodImf);
            }
        }
        await _context.SaveChangesAsync();
    }
    public async Task DeleteImage(int? id)
    {
        if (id == null || id <= 0) throw new NullReferenceException();
        var entity = await _context.ProductImages.FindAsync(id);
        if (entity == null) throw new NullReferenceException();
        _fileService.Delete(entity.Name);
        _context.ProductImages.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int? id)
    {
        var entity = await GetById(id, true);
        _fileService.Delete(entity.MainImage);
        if (entity.ProductImages != null)
        {
            foreach (var item in entity.ProductImages)
            {
                _fileService.Delete(item.Name);
            }
        }
        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDelete(int? id)
    {
        var entity = await GetById(id, true);
        entity.IsDeleted = !entity.IsDeleted;
        await _context.SaveChangesAsync();
    }
}
