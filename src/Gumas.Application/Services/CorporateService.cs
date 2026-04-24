using AutoMapper;
using Gumas.Application.DTOs;
using Gumas.Application.ViewModels;
using Gumas.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gumas.Application.Services;

public interface ICorporateService
{
    Task<List<TeamMemberDto>> GetTeamMembersAsync(string culture);
    Task<List<CertificateDto>> GetCertificatesAsync(string culture);
    Task<CorporateViewModel> GetCorporateDataAsync(string culture);
}

public class CorporateService : ICorporateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHomeService _homeService;

    public CorporateService(IUnitOfWork unitOfWork, IMapper mapper, IHomeService homeService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _homeService = homeService;
    }

    public async Task<List<TeamMemberDto>> GetTeamMembersAsync(string culture)
    {
        var members = await _unitOfWork.TeamMembers.Query()
            .Where(m => m.IsActive)
            .OrderBy(m => m.DisplayOrder)
            .ToListAsync();

        return members.Select(m => new TeamMemberDto
        {
            Id = m.Id,
            Name = m.GetName(culture),
            Title = m.GetTitle(culture),
            Description = m.GetDescription(culture),
            PhotoUrl = m.PhotoUrl,
            Email = m.Email,
            Phone = m.Phone,
            LinkedInUrl = m.LinkedInUrl
        }).ToList();
    }

    public async Task<List<CertificateDto>> GetCertificatesAsync(string culture)
    {
        var certificates = await _unitOfWork.Certificates.Query()
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return certificates.Select(c => new CertificateDto
        {
            Id = c.Id,
            Name = c.GetName(culture),
            Description = c.GetDescription(culture),
            ImageUrl = c.ImageUrl,
            FileUrl = c.FileUrl,
            IssuingOrganization = c.IssuingOrganization,
            IssueDate = c.IssueDate
        }).ToList();
    }

    public async Task<CorporateViewModel> GetCorporateDataAsync(string culture)
    {
        return new CorporateViewModel
        {
            TeamMembers = await GetTeamMembersAsync(culture),
            Certificates = await GetCertificatesAsync(culture),
            Settings = await _homeService.GetSettingsAsync("General", culture)
        };
    }
}
