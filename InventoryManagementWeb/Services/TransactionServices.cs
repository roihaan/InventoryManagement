using InventoryManagementWeb.Contracts;
using InventoryManagementWeb.Models;
using InventoryManagementWeb.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementWeb.Services
{
    public class TransactionService : ITransaction
    {
        private readonly InventoryDBContext _inventory;

        public TransactionService(InventoryDBContext inventoryDbContext)
        {
            _inventory = inventoryDbContext ?? throw new ArgumentNullException(nameof(inventoryDbContext));
        }

        public Transaction Add(Transaction entity)
        {
            try
            {
                var product = _inventory.Products.FirstOrDefault(p => p.ProductId == entity.ProductId);

                if (product == null)
                {
                    throw new ArgumentException("Product not found");
                }

                _inventory.Transactions.Add(entity);
                _inventory.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error adding transaction: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            var transaction = _inventory.Transactions.Find(id);
            if (transaction == null)
            {
                throw new ArgumentException("Transaction not found");
            }

            _inventory.Transactions.Remove(transaction);
            _inventory.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _inventory.Transactions.Any(t => t.TransactionId == id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _inventory.Transactions.ToList();
        }

        public Transaction GetById(int id)
        {
            var transaction = _inventory.Transactions.Find(id);
            if (transaction == null)
            {
                throw new ArgumentException("Transaction not found");
            }
            return transaction;
        }

        public ViewModel GetByIdJoin(int id)
        {
            var result = _inventory.Transactions
                .Where(t => t.TransactionId == id)
                .Include(t => t.Product)
                .FirstOrDefault();

            if (result == null)
            {
                throw new ArgumentException("Transaction not found");
            }

            var viewModel = new ViewModel
            {
                TransactionID = result.TransactionId,
                ProductID = result.ProductId,
                ProductName = result.Product.Name,
                TransactionType = result.TransactionType,
                Quantity = result.Quantity,
                Date = result.Date
            };

            return viewModel;
        }

        public IEnumerable<ViewModel> GetProductTransactions()
        {
            var results = _inventory.Transactions
                .Include(t => t.Product)
                .OrderByDescending(t => t.TransactionId)
                .Select(t => new ViewModel
                {
                    TransactionID = t.TransactionId,
                    ProductID = t.ProductId,
                    ProductName = t.Product.Name,
                    TransactionType = t.TransactionType,
                    Quantity = t.Quantity,
                    Date = t.Date
                })
                .ToList();

            return results;
        }

        public IEnumerable<ViewModel> GetTransactionsByProductName(string productName)
        {
            var results = _inventory.Transactions
                .Where(t => t.Product.Name.Contains(productName))
                .Include(t => t.Product)
                .OrderByDescending(t => t.Date)
                .Select(t => new ViewModel
                {
                    TransactionID = t.TransactionId,
                    ProductID = t.ProductId,
                    ProductName = t.Product.Name,
                    TransactionType = t.TransactionType,
                    Quantity = t.Quantity,
                    Date = t.Date
                })
                .ToList();

            return results;
        }

        public Transaction Update(Transaction entity)
        {
            var existingTransaction = _inventory.Transactions.Find(entity.TransactionId);
            if (existingTransaction == null)
            {
                throw new ArgumentException("Transaction not found");
            }

            // Update existingTransaction properties
            existingTransaction.ProductId = entity.ProductId;
            existingTransaction.TransactionType = entity.TransactionType;
            existingTransaction.Quantity = entity.Quantity;
            existingTransaction.Date = entity.Date;

            _inventory.SaveChanges();
            return existingTransaction;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _inventory.Products.ToList();
        }
    }
}
