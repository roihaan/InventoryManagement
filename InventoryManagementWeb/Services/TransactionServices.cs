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
            _inventory = inventoryDbContext;
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
                throw new Exception(ex.Message);
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
            var results = from t in _inventory.Transactions
                          join p in _inventory.Products
                          on t.ProductId equals p.ProductId
                          orderby t.TransactionId descending
                          select new ViewModel
                          {
                              TransactionID = t.TransactionId,
                              ProductID = t.ProductId,
                              ProductName = p.Name,
                              TransactionType = t.TransactionType,
                              Quantity = t.Quantity,
                              Date = t.Date
                          };
            return results.ToList();
        }

        public IEnumerable<ViewModel> GetTransactionsByProductName(string productName)
        {
            var results = from t in _inventory.Transactions
                          join p in _inventory.Products on t.ProductId equals p.ProductId
                          where p.Name.Contains(productName)
                          orderby t.Date descending
                          select new ViewModel
                          {
                              TransactionID = t.TransactionId,
                              ProductID = t.ProductId,
                              TransactionType = t.TransactionType,
                              Quantity = t.Quantity,
                              Date = t.Date,
                              ProductName = p.Name
                          };

            return results.ToList();
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
    }
}
