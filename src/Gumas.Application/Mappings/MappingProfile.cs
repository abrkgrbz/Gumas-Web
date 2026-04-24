using AutoMapper;
using Gumas.Application.DTOs;
using Gumas.Domain.Entities;

namespace Gumas.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Brand mappings
        CreateMap<Brand, BrandDto>()
            .ForMember(d => d.Name, opt => opt.Ignore())
            .ForMember(d => d.Description, opt => opt.Ignore())
            .ForMember(d => d.MetaTitle, opt => opt.Ignore())
            .ForMember(d => d.MetaDescription, opt => opt.Ignore())
            .ForMember(d => d.ProductCount, opt => opt.MapFrom(s => s.Products.Count));

        CreateMap<Brand, BrandListDto>()
            .ForMember(d => d.Name, opt => opt.Ignore())
            .ForMember(d => d.ProductCount, opt => opt.MapFrom(s => s.Products.Count));

        CreateMap<BrandCreateDto, Brand>();
        CreateMap<Brand, BrandCreateDto>();

        // Category mappings
        CreateMap<Category, CategoryDto>()
            .ForMember(d => d.Name, opt => opt.Ignore())
            .ForMember(d => d.Description, opt => opt.Ignore())
            .ForMember(d => d.MetaTitle, opt => opt.Ignore())
            .ForMember(d => d.MetaDescription, opt => opt.Ignore())
            .ForMember(d => d.ParentName, opt => opt.MapFrom(s => s.Parent != null ? s.Parent.NameTr : null))
            .ForMember(d => d.ProductCount, opt => opt.MapFrom(s => s.Products.Count))
            .ForMember(d => d.Children, opt => opt.MapFrom(s => s.Children));

        CreateMap<Category, CategoryListDto>()
            .ForMember(d => d.Name, opt => opt.Ignore())
            .ForMember(d => d.ProductCount, opt => opt.MapFrom(s => s.Products.Count));

        CreateMap<CategoryCreateDto, Category>();
        CreateMap<Category, CategoryCreateDto>();

        // Product mappings
        CreateMap<Product, ProductDto>()
            .ForMember(d => d.Name, opt => opt.Ignore())
            .ForMember(d => d.Description, opt => opt.Ignore())
            .ForMember(d => d.Specifications, opt => opt.Ignore())
            .ForMember(d => d.TechnicalDetails, opt => opt.Ignore())
            .ForMember(d => d.MetaTitle, opt => opt.Ignore())
            .ForMember(d => d.MetaDescription, opt => opt.Ignore())
            .ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.Brand.NameTr))
            .ForMember(d => d.BrandLogo, opt => opt.MapFrom(s => s.Brand.LogoUrl))
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.NameTr))
            .ForMember(d => d.MainImageUrl, opt => opt.MapFrom(s => s.Images.FirstOrDefault(i => i.IsMain) != null
                ? s.Images.First(i => i.IsMain).ImageUrl
                : s.Images.FirstOrDefault() != null ? s.Images.First().ImageUrl : null));

        CreateMap<Product, ProductListDto>()
            .ForMember(d => d.Name, opt => opt.Ignore())
            .ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.Brand.NameTr))
            .ForMember(d => d.BrandLogo, opt => opt.MapFrom(s => s.Brand.LogoUrl))
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.NameTr))
            .ForMember(d => d.MainImageUrl, opt => opt.MapFrom(s => s.Images.FirstOrDefault(i => i.IsMain) != null
                ? s.Images.First(i => i.IsMain).ImageUrl
                : s.Images.FirstOrDefault() != null ? s.Images.First().ImageUrl : null));

        CreateMap<ProductCreateDto, Product>();
        CreateMap<Product, ProductCreateDto>();

        // Product Image & Document mappings
        CreateMap<ProductImage, ProductImageDto>();
        CreateMap<ProductDocument, ProductDocumentDto>()
            .ForMember(d => d.Title, opt => opt.Ignore());

        // Slider mappings
        CreateMap<Slider, SliderDto>()
            .ForMember(d => d.Name, opt => opt.Ignore())
            .ForMember(d => d.Subtitle, opt => opt.Ignore())
            .ForMember(d => d.Description, opt => opt.Ignore())
            .ForMember(d => d.ButtonText, opt => opt.Ignore());

        // News mappings
        CreateMap<News, NewsDto>()
            .ForMember(d => d.Name, opt => opt.Ignore())
            .ForMember(d => d.Content, opt => opt.Ignore())
            .ForMember(d => d.Summary, opt => opt.Ignore())
            .ForMember(d => d.MetaTitle, opt => opt.Ignore())
            .ForMember(d => d.MetaDescription, opt => opt.Ignore());

        CreateMap<News, NewsListDto>()
            .ForMember(d => d.Name, opt => opt.Ignore())
            .ForMember(d => d.Summary, opt => opt.Ignore());

        // Contact Message mappings
        CreateMap<ContactMessageDto, ContactMessage>();

        // Certificate mappings
        CreateMap<Certificate, CertificateDto>()
            .ForMember(d => d.Name, opt => opt.Ignore())
            .ForMember(d => d.Description, opt => opt.Ignore());

        // Team Member mappings
        CreateMap<TeamMember, TeamMemberDto>()
            .ForMember(d => d.Name, opt => opt.Ignore())
            .ForMember(d => d.Title, opt => opt.Ignore())
            .ForMember(d => d.Description, opt => opt.Ignore());

        // Setting mappings
        CreateMap<Setting, SettingDto>()
            .ForMember(d => d.Value, opt => opt.Ignore());
    }
}
