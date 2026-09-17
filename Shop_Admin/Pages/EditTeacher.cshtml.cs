using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop_Admin.Data;
using Shop_Admin.Models;

namespace Shop_Admin.Pages;

public class EditTeacherModel : PageModel
{
    private readonly AppDbContext _context;

    public EditTeacherModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Teacher Teacher { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(x => x.Id == id);

        if (teacher == null)
        {
            return NotFound();
        }

        Teacher = teacher;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(x => x.Id == Teacher.Id);

        if (teacher == null)
        {
            return NotFound();
        }

        bool emailExists = await _context.Teachers
            .AnyAsync(x =>
                x.Email == Teacher.Email &&
                x.Id != Teacher.Id);

        if (emailExists)
        {
            ModelState.AddModelError(
                "",
                "This email already exists."
            );

            return Page();
        }

        teacher.FirstName = Teacher.FirstName.Trim();
        teacher.LastName = Teacher.LastName.Trim();
        teacher.Email = Teacher.Email.Trim();
        teacher.Phone = Teacher.Phone.Trim();
        teacher.Gender = Teacher.Gender;
        teacher.Status = Teacher.Status;

        await _context.SaveChangesAsync();

        return RedirectToPage("/Teachers");
    }
}