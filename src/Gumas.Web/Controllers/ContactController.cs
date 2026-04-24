using Microsoft.AspNetCore.Mvc;
using Gumas.Domain.Entities;
using Gumas.Domain.Enums;
using Gumas.Infrastructure.Data;
using Gumas.Web.ViewModels;

namespace Gumas.Web.Controllers;

public class ContactController : Controller
{
    private readonly GumasDbContext _context;

    public ContactController(GumasDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(new ContactViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var message = new ContactMessage
        {
            FullName = model.FullName,
            Email = model.Email,
            Phone = model.Phone,
            Subject = model.Subject,
            Message = model.Message,
            Status = MessageStatus.New,
            CreatedAt = DateTime.UtcNow
        };

        _context.ContactMessages.Add(message);
        await _context.SaveChangesAsync();

        model.IsSuccess = true;
        model.SuccessMessage = "Mesajınız başarıyla gönderildi. En kısa sürede size dönüş yapacağız.";
        ModelState.Clear();

        return View(new ContactViewModel
        {
            IsSuccess = true,
            SuccessMessage = model.SuccessMessage
        });
    }
}
