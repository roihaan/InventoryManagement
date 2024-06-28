using Microsoft.AspNetCore.Mvc;
using System.Linq;
using InventoryWeb.Models;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly InventoryDBContext _context;

    public TransactionsController(InventoryDBContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult CreateTransaction([FromBody] Transaction transaction)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Transactions.Add(transaction);
        _context.SaveChanges();

        return Ok(transaction);
    }

    [HttpGet]
    public IActionResult GetTransactions()
    {
        var transactions = _context.Transactions.ToList();
        return Ok(transactions);
    }
}
