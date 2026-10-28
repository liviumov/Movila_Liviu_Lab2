using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movila_Liviu_Lab2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movila_Liviu_Lab2.Pages.BookPages
{
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Movila_Liviu_Lab2Context _context;

        public CreateModel(Movila_Liviu_Lab2Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");

            var authorList = _context.Author.Select(x => new
            {
                x.ID,
                FullName = x.FirstName + " " + x.LastName
            });
            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName");

            var book = new Book();
            book.BookCategories = new List<BookCategory>();
            PopulateAssignedCategoryData(_context, book);

            return Page();
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            if (selectedCategories != null)
            {
                Book.BookCategories = new List<BookCategory>();
                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    };
                    Book.BookCategories.Add(catToAdd);
                }
            }

            ModelState.Remove("Book.Publisher");
            ModelState.Remove("Book.Author");
            ModelState.Remove("Book.BookCategories");

            if (ModelState.IsValid)
            {
                _context.Book.Add(Book);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index"); 
            }

            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");
            var authorList = _context.Author.Select(x => new
            {
                x.ID,
                FullName = x.FirstName + " " + x.LastName
            });
            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName");

            PopulateAssignedCategoryData(_context, Book);

            return Page();
        }
    }
}