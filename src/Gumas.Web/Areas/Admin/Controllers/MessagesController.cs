using Gumas.Domain.Interfaces;
using Gumas.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Gumas.Web.Areas.Admin.Controllers;

public class MessagesController : BaseAdminController
{
    private readonly IUnitOfWork _unitOfWork;

    public MessagesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index(MessageStatus? status, string? search, int page = 1)
    {
        const int pageSize = 20;

        var messages = await _unitOfWork.ContactMessages.GetAllAsync(
            filter: m => (!status.HasValue || m.Status == status)
                      && (string.IsNullOrEmpty(search) || m.FullName.Contains(search) || m.Email.Contains(search) || m.Subject.Contains(search)),
            orderBy: q => q.OrderByDescending(m => m.CreatedAt),
            skip: (page - 1) * pageSize,
            take: pageSize);

        var totalCount = await _unitOfWork.ContactMessages.CountAsync(
            m => (!status.HasValue || m.Status == status)
              && (string.IsNullOrEmpty(search) || m.FullName.Contains(search) || m.Email.Contains(search) || m.Subject.Contains(search)));

        var unreadCount = await _unitOfWork.ContactMessages.CountAsync(m => m.Status == MessageStatus.New);

        ViewBag.Search = search;
        ViewBag.Status = status;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ViewBag.UnreadCount = unreadCount;

        return View(messages);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var message = await _unitOfWork.ContactMessages.GetByIdAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        if (message.Status == MessageStatus.New)
        {
            message.Status = MessageStatus.Read;
            message.ReadAt = DateTime.UtcNow;
            _unitOfWork.ContactMessages.Update(message);
            await _unitOfWork.SaveChangesAsync();
        }

        return View(message);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var message = await _unitOfWork.ContactMessages.GetByIdAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        message.Status = MessageStatus.Read;
        message.ReadAt = DateTime.UtcNow;
        _unitOfWork.ContactMessages.Update(message);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsUnread(int id)
    {
        var message = await _unitOfWork.ContactMessages.GetByIdAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        message.Status = MessageStatus.New;
        message.ReadAt = null;
        _unitOfWork.ContactMessages.Update(message);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var message = await _unitOfWork.ContactMessages.GetByIdAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        _unitOfWork.ContactMessages.Remove(message);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Mesaj başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var unreadMessages = await _unitOfWork.ContactMessages.GetAllAsync(filter: m => m.Status == MessageStatus.New);

        foreach (var message in unreadMessages)
        {
            message.Status = MessageStatus.Read;
            message.ReadAt = DateTime.UtcNow;
            _unitOfWork.ContactMessages.Update(message);
        }

        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Tüm mesajlar okundu olarak işaretlendi.";
        return RedirectToAction(nameof(Index));
    }
}
