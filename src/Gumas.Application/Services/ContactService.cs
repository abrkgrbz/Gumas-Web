using AutoMapper;
using Gumas.Application.DTOs;
using Gumas.Domain.Entities;
using Gumas.Domain.Enums;
using Gumas.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gumas.Application.Services;

public interface IContactService
{
    Task<ContactMessage> SubmitMessageAsync(ContactMessageDto dto, string? ipAddress, string? userAgent);
    Task<List<ContactMessage>> GetMessagesAsync(MessageStatus? status = null);
    Task<ContactMessage?> GetMessageByIdAsync(int id);
    Task MarkAsReadAsync(int id);
    Task UpdateStatusAsync(int id, MessageStatus status, string? adminNotes = null);
}

public class ContactService : IContactService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ContactService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ContactMessage> SubmitMessageAsync(ContactMessageDto dto, string? ipAddress, string? userAgent)
    {
        var message = _mapper.Map<ContactMessage>(dto);
        message.IpAddress = ipAddress;
        message.UserAgent = userAgent;
        message.Status = MessageStatus.New;

        await _unitOfWork.ContactMessages.AddAsync(message);
        await _unitOfWork.SaveChangesAsync();

        return message;
    }

    public async Task<List<ContactMessage>> GetMessagesAsync(MessageStatus? status = null)
    {
        var query = _unitOfWork.ContactMessages.Query()
            .Include(m => m.Product)
            .Where(m => !m.IsDeleted);

        if (status.HasValue)
            query = query.Where(m => m.Status == status.Value);

        return await query
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<ContactMessage?> GetMessageByIdAsync(int id)
    {
        return await _unitOfWork.ContactMessages.Query()
            .Include(m => m.Product)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task MarkAsReadAsync(int id)
    {
        var message = await _unitOfWork.ContactMessages.GetByIdAsync(id);
        if (message != null && message.Status == MessageStatus.New)
        {
            message.Status = MessageStatus.Read;
            message.ReadAt = DateTime.UtcNow;
            _unitOfWork.ContactMessages.Update(message);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task UpdateStatusAsync(int id, MessageStatus status, string? adminNotes = null)
    {
        var message = await _unitOfWork.ContactMessages.GetByIdAsync(id)
            ?? throw new Exception("Message not found");

        message.Status = status;
        if (!string.IsNullOrEmpty(adminNotes))
            message.AdminNotes = adminNotes;

        _unitOfWork.ContactMessages.Update(message);
        await _unitOfWork.SaveChangesAsync();
    }
}
