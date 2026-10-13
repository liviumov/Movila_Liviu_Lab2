using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Movila_Liviu_Lab2.Models;

namespace Movila_Liviu_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Movila_Liviu_Lab2Context _context;

    public IndexModel(Movila_Liviu_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Book != null)
        {
            Book = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.Author)
                .ToListAsync();
        }
    }
} 
