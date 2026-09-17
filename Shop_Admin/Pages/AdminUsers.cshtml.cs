using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop_Admin.Data;
using Shop_Admin.Models;

namespace Shop_Admin.Pages;

public class AdminUsersModel : PageModel
{
    private readonly AppDbContext _context;

    public AdminUsersModel(AppDbContext context)
    {
        _context = context;
    }

    public List<AdminUser> AdminUsers { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string Search { get; set; } = "";

    [BindProperty]
    public string FirstName { get; set; } = "";

    [BindProperty]
    public string LastName { get; set; } = "";

    [BindProperty]
    public string Username { get; set; } = "";

    [BindProperty]
    public string Email { get; set; } = "";

    [BindProperty]
    public string Password { get; set; } = "";

    [BindProperty]
    public string Gender { get; set; } = "Male";


    public async Task OnGetAsync()
    {
        await LoadAdminsAsync();
    }


    private async Task LoadAdminsAsync()
    {
        var query = _context.AdminUsers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            query = query.Where(x =>
                x.FirstName.Contains(Search) ||
                x.LastName.Contains(Search) ||
                x.Username.Contains(Search) ||
                x.Email.Contains(Search));
        }

        AdminUsers = await query
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }


    // =========================
    // ADD ADMIN
    // =========================

    public async Task<IActionResult> OnPostAddAsync()
    {
        // Remove validation errors that are not related
        // to the Add form itself.
        ModelState.Clear();

        if (string.IsNullOrWhiteSpace(FirstName))
        {
            ModelState.AddModelError("", "First Name is required.");
        }

        if (string.IsNullOrWhiteSpace(LastName))
        {
            ModelState.AddModelError("", "Last Name is required.");
        }

        if (string.IsNullOrWhiteSpace(Username))
        {
            ModelState.AddModelError("", "Username is required.");
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            ModelState.AddModelError("", "Email is required.");
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            ModelState.AddModelError("", "Password is required.");
        }

        if (!string.IsNullOrWhiteSpace(Username))
        {
            bool usernameExists = await _context.AdminUsers
                .AnyAsync(x => x.Username == Username.Trim());

            if (usernameExists)
            {
                ModelState.AddModelError(
                    "",
                    "This username already exists."
                );
            }
        }

        if (!string.IsNullOrWhiteSpace(Email))
        {
            bool emailExists = await _context.AdminUsers
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
            await LoadAdminsAsync();
            return Page();
        }


        try
        {
            var newAdmin = new AdminUser
            {
                FirstName = FirstName.Trim(),
                LastName = LastName.Trim(),
                Username = Username.Trim(),
                Email = Email.Trim(),
                Password = Password,
                Gender = string.IsNullOrWhiteSpace(Gender)
                    ? "Male"
                    : Gender,
                CreatedAt = DateTime.Now,
                Status = "Active"
            };

            _context.AdminUsers.Add(newAdmin);

            await _context.SaveChangesAsync();

            return RedirectToPage("/AdminUsers");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                "",
                "Database Error: " + ex.InnerException?.Message ?? ex.Message
            );

            await LoadAdminsAsync();

            return Page();
        }
    }


    // =========================
    // TOGGLE STATUS
    // =========================

    public async Task<IActionResult> OnPostToggleStatusAsync(int ToggleId)
    {
        var admin = await _context.AdminUsers
            .FirstOrDefaultAsync(x => x.Id == ToggleId);

        if (admin == null)
        {
            return NotFound();
        }

        admin.Status =
            admin.Status == "Active"
                ? "Inactive"
                : "Active";

        await _context.SaveChangesAsync();

        return RedirectToPage("/AdminUsers");
    }


    // =========================
    // DELETE
    // =========================

    public async Task<IActionResult> OnPostDeleteAsync(int DeleteId)
    {
        var admin = await _context.AdminUsers
            .FirstOrDefaultAsync(x => x.Id == DeleteId);

        if (admin == null)
        {
            return NotFound();
        }

        _context.AdminUsers.Remove(admin);

        await _context.SaveChangesAsync();

        return RedirectToPage("/AdminUsers");
    }
}