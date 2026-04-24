using Gumas.Domain.Entities;
using Gumas.Domain.Interfaces;
using Gumas.Infrastructure.Data;
using Gumas.Infrastructure.Repositories;

namespace Gumas.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly GumasDbContext _context;
    private bool _disposed;

    private IRepository<Brand>? _brands;
    private IRepository<Category>? _categories;
    private IRepository<Product>? _products;
    private IRepository<ProductImage>? _productImages;
    private IRepository<ProductDocument>? _productDocuments;
    private IRepository<Slider>? _sliders;
    private IRepository<News>? _news;
    private IRepository<ContactMessage>? _contactMessages;
    private IRepository<Certificate>? _certificates;
    private IRepository<TeamMember>? _teamMembers;
    private IRepository<Setting>? _settings;

    public UnitOfWork(GumasDbContext context)
    {
        _context = context;
    }

    public IRepository<Brand> Brands => _brands ??= new Repository<Brand>(_context);
    public IRepository<Category> Categories => _categories ??= new Repository<Category>(_context);
    public IRepository<Product> Products => _products ??= new Repository<Product>(_context);
    public IRepository<ProductImage> ProductImages => _productImages ??= new Repository<ProductImage>(_context);
    public IRepository<ProductDocument> ProductDocuments => _productDocuments ??= new Repository<ProductDocument>(_context);
    public IRepository<Slider> Sliders => _sliders ??= new Repository<Slider>(_context);
    public IRepository<News> News => _news ??= new Repository<News>(_context);
    public IRepository<ContactMessage> ContactMessages => _contactMessages ??= new Repository<ContactMessage>(_context);
    public IRepository<Certificate> Certificates => _certificates ??= new Repository<Certificate>(_context);
    public IRepository<TeamMember> TeamMembers => _teamMembers ??= new Repository<TeamMember>(_context);
    public IRepository<Setting> Settings => _settings ??= new Repository<Setting>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
