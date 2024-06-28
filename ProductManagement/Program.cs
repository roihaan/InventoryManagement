using System;
using System.Linq;

using ProductManagement;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new InventoryContext())
        {
            while (true)
            {
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Update Product");
                Console.WriteLine("3. List Products");
                Console.WriteLine("4. Exit");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProduct(context);
                        break;
                    case "2":
                        UpdateProduct(context);
                        break;
                    case "3":
                        ListProducts(context);
                        break;
                    case "4":
                        return;
                }
            }
        }
    }

    static void AddProduct(InventoryContext context)
    {
        Console.Write("Enter product name: ");
        var name = Console.ReadLine();
        Console.Write("Enter initial stock level: ");
        var stockLevel = int.Parse(Console.ReadLine());

        var product = new Product { Name = name, StockLevel = stockLevel };
        context.Products.Add(product);
        context.SaveChanges();
        Console.WriteLine("Product added successfully.");
    }

    static void UpdateProduct(InventoryContext context)
    {
        Console.Write("Enter product ID to update: ");
        var id = int.Parse(Console.ReadLine());
        var product = context.Products.Find(id);

        if (product != null)
        {
            Console.Write("Enter new name: ");
            product.Name = Console.ReadLine();
            Console.Write("Enter new stock level: ");
            product.StockLevel = int.Parse(Console.ReadLine());

            context.SaveChanges();
            Console.WriteLine("Product updated successfully.");
        }
        else
        {
            Console.WriteLine("Product not found.");
        }
    }

    static void ListProducts(InventoryContext context)
    {
        var products = context.Products.ToList();
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.ProductID}, Name: {product.Name}, Stock Level: {product.StockLevel}");
        }
    }
}