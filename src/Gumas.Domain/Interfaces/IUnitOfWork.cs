using Gumas.Domain.Entities;

namespace Gumas.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Brand> Brands { get; }
    IRepository<Category> Categories { get; }
    IRepository<Product> Products { get; }
    IRepository<ProductImage> ProductImages { get; }
    IRepository<ProductDocument> ProductDocuments { get; }
    IRepository<Slider> Sliders { get; }
    IRepository<News> News { get; }
    IRepository<ContactMessage> ContactMessages { get; }
    IRepository<Certificate> Certificates { get; }
    IRepository<TeamMember> TeamMembers { get; }
    IRepository<Setting> Settings { get; }

    Task<int> SaveChangesAsync();
}
