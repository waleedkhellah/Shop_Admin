using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop_Admin.Data;
using Shop_Admin.Models;

namespace Shop_Admin.Pages;

public class EditAdminModel : PageModel
{
    private readonly AppDbContext _context;

    public EditAdminModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public AdminUser Admin { get; set; } = new();

    [BindProperty]
    public string NewPassword { get; set; } = "";


    // =========================
    // OPEN EDIT PAGE
    // =========================

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var admin = await _context.AdminUsers
            .FirstOrDefaultAsync(x => x.Id == id);

        if (admin == null)
        {
            return NotFound();
        }

        Admin = admin;

        return Page();
    }


    // =========================
    // SAVE EDIT
    // =========================

    public async Task<IActionResult> OnPostAsync()
    {
        var admin = await _context.AdminUsers
            .FirstOrDefaultAsync(x => x.Id == Admin.Id);

        if (admin == null)
        {
            return NotFound();
        }


        // Check username

        bool usernameExists = await _context.AdminUsers
            .AnyAsync(x =>
                x.Username == Admin.Username &&
                x.Id != Admin.Id);

        if (usernameExists)
        {
            ModelState.AddModelError(
                "",
                "This username already exists."
            );

            return Page();
        }


        // Check email

        bool emailExists = await _context.AdminUsers
            .AnyAsync(x =>
                x.Email == Admin.Email &&
                x.Id != Admin.Id);

        if (emailExists)
        {
            ModelState.AddModelError(
                "",
                "This email already exists."
            );

            return Page();
        }


        // Update

        admin.FirstName = Admin.FirstName.Trim();

        admin.LastName = Admin.LastName.Trim();

        admin.Username = Admin.Username.Trim();

        admin.Email = Admin.Email.Trim();

        admin.Gender = Admin.Gender;

        admin.Status = Admin.Status;


        // Change password only if entered

        if (!string.IsNullOrWhiteSpace(NewPassword))
        {
            admin.Password = NewPassword;
        }


        await _context.SaveChangesAsync();


        return RedirectToPage("/AdminUsers");
    }
}