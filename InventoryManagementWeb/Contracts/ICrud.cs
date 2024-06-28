namespace InventoryManagementWeb.Contracts
{
    public class ICrud<T> where T : class
    {
        IEnumerable<T> GetAll();
    }
}
