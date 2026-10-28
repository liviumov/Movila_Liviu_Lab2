using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Movila_Liviu_Lab2.Models;

namespace Movila_Liviu_Lab2.Pages.BookPages
{
    public class IndexModel : PageModel
    {
        private readonly Movila_Liviu_Lab2Context _context;

        public IndexModel(Movila_Liviu_Lab2Context context)
        {
            _context = context;
        }

        public IList<Book> Book { get; set; } = default!;

        public BookData BookD { get; set; } = new BookData();
        public int BookID { get; set; }
        public int CategoryID { get; set; }

        public string TitleSort { get; set; }

        public string AuthorSort { get; set; }

        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string searchString)
        {
            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            AuthorSort = sortOrder == "author" ? "author_desc" : "author";

            CurrentFilter = searchString;

            BookD.Books = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.Author)
                .Include(b => b.BookCategories)
                    .ThenInclude(b => b.Category)
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                BookD.Books = BookD.Books.Where(s => s.Author.FirstName.Contains(searchString) || s.Author.LastName.Contains(searchString)  || s.Title.Contains(searchString));
            }

            if (id != null)
            {
                BookID = id.Value;
                Book book = BookD.Books.Where(i => i.Id == id.Value).Single();
                BookD.Categories = book.BookCategories.Select(s => s.Category);
            }
            switch (sortOrder)
            {
                case "title_desc":
                    BookD.Books = BookD.Books.OrderByDescending(b => b.Title).ToList();
                    break;
                case "author":
                    BookD.Books = BookD.Books.OrderBy(b => b.Author.LastName).ToList();
                    break;
                case "author_desc":
                    BookD.Books = BookD.Books.OrderByDescending(b => b.Author.LastName).ToList();
                    break;
                default:
                    BookD.Books = BookD.Books.OrderBy(b => b.Title).ToList();
                    break;
            }
        }
    }
}