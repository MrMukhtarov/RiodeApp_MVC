namespace RiodeApp.Models;

public class UserToken : BaseEntity
{
    public string AppUserId { get; set; }
    public string Key { get; set; }
    public DateTime SendDate { get; set; }
    public AppUser AppUser { get; set; }
}
