using RiodeApp.Models;
using RiodeApp.ViewModels.CategoryVMs;

namespace RiodeApp.Services.Interfaces;

public interface ICategoryService
{
    Task<ICollection<Category>> GetAll(bool takeAll);
    IQueryable<Category> GetTable { get; }
    Task<Category> GetById(int? id, bool takeAll = false);
    Task Create(CreateCategoryVM vm);
    Task Update(int? id, UpdateCategoryVm vm);
    Task<bool> IsExist(int id);
    Task<bool> IsAllExist(List<int> ids);
    Task Delete(int? id);
    Task SoftDelete(int? id);
}
