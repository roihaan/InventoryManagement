using InventoryManagementWeb.Contracts;
using InventoryManagementWeb.Models;
using InventoryManagementWeb.ViewModels;
using System;

namespace InventoryManagementWeb.Services
{
    public class ProductService : IProduct
    {
        private readonly InventoryDBContext _inventory;
        public ProductService(InventoryDBContext inventoryDbContext)
        {
            _inventory = inventoryDbContext;
        }

        public Product Add(Product entity)
        {
            try
            {
                _inventory.Products.Add(entity);
                _inventory.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var deleteProduct = GetById(id);
                if (deleteProduct != null)
                {
                    _inventory.Products.Remove(deleteProduct);
                    _inventory.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Category not found");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            var results = _inventory.Products.ToList();
            return results;
        }

        public Product GetById(int id)
        {
            var result = _inventory.Products.Where(c => c.ProductId == id).FirstOrDefault();
            if (result == null)
            {
                throw new ArgumentException("Product not found");
            }
            return result;
        }

        public Product Update(Product entity)
        {
            try
            {
                var updateProduct = GetById(entity.ProductId);
                if (updateProduct != null)
                {
                    updateProduct.Name = entity.Name;
                    updateProduct.StockLevel = entity.StockLevel;
                    _inventory.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Product not found");
                }
                return updateProduct;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public IEnumerable<Product> GetProductsByName(string productName)
        {
            var results = (from i in _inventory.Products
                           where i.Name.Contains(productName)
                           select i).ToList();
            return results;
        }
    }
}
