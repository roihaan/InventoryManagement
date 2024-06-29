using InventoryManagementWeb.Models;
using InventoryManagementWeb.ViewModels;

namespace InventoryManagementWeb.Contracts
{
    public interface ITransaction : ICrud<Transaction>
    {
        IEnumerable<ViewModel> GetProductTransactions();
        IEnumerable<ViewModel> GetTransactionsByProductName(string transactionName);
        ViewModel GetByIdJoin(int id);
        bool Exists(int id);
    }
}
