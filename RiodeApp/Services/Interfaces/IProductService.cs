using RiodeApp.Models;
using RiodeApp.ViewModels.ProductVMs;

namespace RiodeApp.Services.Interfaces;

public interface IProductService
{
    IQueryable<Product> GetTable { get; }
    Task<ICollection<Product>> GetAll(bool TakeAll);
    Task<Product> GetById(int? id, bool takeAll = false);
    Task Create(CreateProductVM vm);
    Task Update(UpdateProductVM vm,int? id);
    Task DeleteImage(int? id);
    Task Delete(int? id);
    Task SoftDelete(int? id);
}
