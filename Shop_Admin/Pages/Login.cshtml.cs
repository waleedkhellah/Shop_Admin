using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop_Admin.Data;

namespace Shop_Admin.Pages;

public class LoginModel : PageModel
{
    private readonly AppDbContext _context;

    public LoginModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string Username { get; set; } = "";

    [BindProperty]
    public string Password { get; set; } = "";

    public string ErrorMessage { get; set; } = "";

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var admin = await _context.AdminUsers
            .FirstOrDefaultAsync(x =>
                x.Username == Username &&
                x.Password == Password &&
                x.Status == "Active");

        if (admin == null)
        {
            ErrorMessage = "Invalid username or password, or account is inactive.";
            return Page();
        }

        return RedirectToPage("/Dashboard");
    }
}