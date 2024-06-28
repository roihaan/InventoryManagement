using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryWeb.Models;

namespace InventoryWeb
{
    public class IndexModel : PageModel
    {
        private readonly InventoryWeb.Models.InventoryDBContext _context;

        public IndexModel(InventoryWeb.Models.InventoryDBContext context)
        {
            _context = context;
        }

        public IList<Transaction> Transaction { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Transaction = await _context.Transactions
                .Include(t => t.Product).ToListAsync();
        }
    }
}
