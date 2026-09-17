using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop_Admin.Data;
using Shop_Admin.Models;

namespace Shop_Admin.Pages;

public class TeachersModel : PageModel
{
    private readonly AppDbContext _context;

    public TeachersModel(AppDbContext context)
    {
        _context = context;
    }

    public List<Teacher> Teachers { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string Search { get; set; } = "";

    [BindProperty]
    public string FirstName { get; set; } = "";

    [BindProperty]
    public string LastName { get; set; } = "";

    [BindProperty]
    public string Email { get; set; } = "";

    [BindProperty]
    public string Phone { get; set; } = "";

    [BindProperty]
    public string Gender { get; set; } = "Male";

    public async Task OnGetAsync()
    {
        await LoadTeachersAsync();
    }

    private async Task LoadTeachersAsync()
    {
        var query = _context.Teachers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            query = query.Where(x =>
                x.FirstName.Contains(Search) ||
                x.LastName.Contains(Search) ||
                x.Email.Contains(Search) ||
                x.Phone.Contains(Search));
        }

        Teachers = await query
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        ModelState.Clear();

        if (string.IsNullOrWhiteSpace(FirstName))
            ModelState.AddModelError("", "First Name is required.");

        if (string.IsNullOrWhiteSpace(LastName))
            ModelState.AddModelError("", "Last Name is required.");

        if (string.IsNullOrWhiteSpace(Email))
            ModelState.AddModelError("", "Email is required.");

        if (string.IsNullOrWhiteSpace(Phone))
            ModelState.AddModelError("", "Phone is required.");

        if (!string.IsNullOrWhiteSpace(Email))
        {
            bool emailExists = await _context.Teachers
                .AnyAsync(x => x.Email == Email.Trim());

            if (emailExists)
            {
                ModelState.AddModelError(
                    "",
                    "This email already exists."
                );
            }
        }

        if (!ModelState.IsValid)
        {
            await LoadTeachersAsync();
            return Page();
        }

        var teacher = new Teacher
        {
            FirstName = FirstName.Trim(),
            LastName = LastName.Trim(),
            Email = Email.Trim(),
            Phone = Phone.Trim(),
            Gender = string.IsNullOrWhiteSpace(Gender)
                ? "Male"
                : Gender,
            CreatedAt = DateTime.Now,
            Status = "Active"
        };

        _context.Teachers.Add(teacher);

        await _context.SaveChangesAsync();

        return RedirectToPage("/Teachers");
    }

    public async Task<IActionResult> OnPostToggleStatusAsync(int ToggleId)
    {
        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(x => x.Id == ToggleId);

        if (teacher == null)
            return NotFound();

        teacher.Status =
            teacher.Status == "Active"
                ? "Inactive"
                : "Active";

        await _context.SaveChangesAsync();

        return RedirectToPage("/Teachers");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int DeleteId)
    {
        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(x => x.Id == DeleteId);

        if (teacher == null)
            return NotFound();

        _context.Teachers.Remove(teacher);

        await _context.SaveChangesAsync();

        return RedirectToPage("/Teachers");
    }
}