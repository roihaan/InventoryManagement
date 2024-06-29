using Microsoft.AspNetCore.Mvc;
using InventoryManagementWeb.Contracts;
using InventoryManagementWeb.ViewModels;
using InventoryManagementWeb.Models; // Ensure to include the correct namespace
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;

namespace InventoryManagementWeb.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransaction _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransaction transactionService, ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
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

        [HttpGet] // Specify HTTP method explicitly for clarity
        public IActionResult Create()
        {
            var viewModel = new ViewModel();
            viewModel.Products = _transactionService.GetProducts().ToList();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ViewModel viewModel)
        {
            try
            {
                _logger.LogInformation("Create action called");

                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Model state is valid");

                    // Ensure to use the correct Transaction class from your project's namespace
                    var transaction = new InventoryManagementWeb.Models.Transaction
                    {
                        ProductId = viewModel.ProductID.GetValueOrDefault(),
                        TransactionType = viewModel.TransactionType.GetValueOrDefault(),
                        Quantity = viewModel.Quantity.GetValueOrDefault(),
                        Date = viewModel.Date.GetValueOrDefault()
                    };

                    _transactionService.Add(transaction);
                    _logger.LogInformation("Transaction successfully added");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogWarning("Model state is invalid");
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            _logger.LogWarning($"Property: {state.Key}, Error: {error.ErrorMessage}");
                        }
                    }
                }

                // If the model state is invalid, repopulate the products list
                viewModel.Products = _transactionService.GetProducts().ToList();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating transaction");
                ViewBag.ErrorMessage = $"Error creating transaction: {ex.Message}";
                return View(viewModel);
            }
        }
    }
}
