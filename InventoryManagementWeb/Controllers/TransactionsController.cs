using Microsoft.AspNetCore.Mvc;
using InventoryManagementWeb.Contracts;
using InventoryManagementWeb.ViewModels;
using System;
using InventoryManagementWeb.Models;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementWeb.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransaction _transactionService;

        public TransactionsController(ITransaction transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: Transactions/Index
        public IActionResult Index()
        {
            try
            {
                var transactions = _transactionService.GetProductTransactions();

                var viewModels = transactions.Select(t => new ViewModel
                {
                    TransactionID = t.TransactionID,
                    ProductID = t.ProductID,
                    ProductName = t.ProductName,
                    TransactionType = t.TransactionType,
                    Quantity = t.Quantity,
                    Date = t.Date
                }).ToList();

                return View(viewModels);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving transactions: {ex.Message}";
                return View(new List<ViewModel>());
            }
        }

        // GET: Transactions/Details/{id}
        public IActionResult Details(int id)
        {
            try
            {
                var viewModel = _transactionService.GetByIdJoin(id);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving transaction details: {ex.Message}";
                return View();
            }
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Transaction transaction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _transactionService.Add(transaction);
                    return RedirectToAction(nameof(Index));
                }
                return View(transaction);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error creating transaction: {ex.Message}";
                return View(transaction);
            }
        }

        // GET: Transactions/Edit/{id}
        public IActionResult Edit(int id)
        {
            try
            {
                var transaction = _transactionService.GetById(id);
                if (transaction == null)
                {
                    return NotFound();
                }
                return View(transaction);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving transaction for editing: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Transactions/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Transaction transaction)
        {
            try
            {
                if (id != transaction.TransactionId)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    _transactionService.Update(transaction);
                    return RedirectToAction(nameof(Index));
                }

                return View(transaction);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error updating transaction: {ex.Message}";
                return View(transaction);
            }
        }

        // GET: Transactions/Delete/{id}
        public IActionResult Delete(int id)
        {
            try
            {
                var transaction = _transactionService.GetById(id);
                if (transaction == null)
                {
                    return NotFound();
                }
                return View(transaction);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving transaction for deletion: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Transactions/DeleteConfirmed/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _transactionService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error deleting transaction: {ex.Message}";
                return RedirectToAction(nameof(Delete), new { id = id });
            }
        }
    }
}
