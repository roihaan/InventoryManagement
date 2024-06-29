using InventoryManagementWeb.Models;

namespace InventoryManagementWeb.ViewModels
{
    public class ViewModel
    {
        public int TransactionID { get; set; }
        public int? ProductID { get; set; }
        public string? ProductName { get; set; }
        public int? TransactionType { get; set; }
        public int? Quantity { get; set; }
        public DateTime? Date { get; set; }
        public List<Product>? Products { get; set; } // Add this property

        public Transaction Transaction { get; set; }
    }
}
