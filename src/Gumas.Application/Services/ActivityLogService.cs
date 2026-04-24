using Gumas.Application.ViewModels;
using Gumas.Domain.Entities;
using Gumas.Domain.Interfaces;

namespace Gumas.Application.Services;

public class ActivityLogService : IActivityLogService
{
    private readonly IRepository<ActivityLog> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivityLogService(IRepository<ActivityLog> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task LogActivityAsync(string action, string entityType, int? entityId, string entityName, string details, string userName, string ipAddress)
    {
        var log = new ActivityLog
        {
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            EntityName = entityName,
            Details = details,
            UserName = userName,
            IpAddress = ipAddress,
            Timestamp = DateTime.UtcNow
        };

        await _repository.AddAsync(log);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedResult<ActivityLogDto>> GetActivityLogsAsync(int page, int pageSize, string? entityType = null, string? action = null)
    {
        var query = await _repository.GetAllAsync();

        if (!string.IsNullOrEmpty(entityType))
        {
            query = query.Where(l => l.EntityType == entityType);
        }

        if (!string.IsNullOrEmpty(action))
        {
            query = query.Where(l => l.Action == action);
        }

        var totalCount = query.Count();
        var items = query
            .OrderByDescending(l => l.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new ActivityLogDto
            {
                Id = l.Id,
                Action = l.Action,
                EntityType = l.EntityType,
                EntityId = l.EntityId,
                EntityName = l.EntityName,
                Details = l.Details,
                UserName = l.UserName,
                IpAddress = l.IpAddress,
                Timestamp = l.Timestamp
            })
            .ToList();

        return new PagedResult<ActivityLogDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<List<ActivityLogDto>> GetRecentActivitiesAsync(int count = 10)
    {
        var query = await _repository.GetAllAsync();

        return query
            .OrderByDescending(l => l.Timestamp)
            .Take(count)
            .Select(l => new ActivityLogDto
            {
                Id = l.Id,
                Action = l.Action,
                EntityType = l.EntityType,
                EntityId = l.EntityId,
                EntityName = l.EntityName,
                Details = l.Details,
                UserName = l.UserName,
                IpAddress = l.IpAddress,
                Timestamp = l.Timestamp
            })
            .ToList();
    }
}
