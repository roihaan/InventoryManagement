using InventoryManagementWeb.Models;
using InventoryManagementWeb.ViewModels;

namespace InventoryManagementWeb.Contracts
{
    public interface IProduct : ICrud<Product>
    {
        IEnumerable<Product> GetProductsByName(string productName);
    }
}
