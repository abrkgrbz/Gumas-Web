using Gumas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gumas.Infrastructure.Data.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.NameTr).HasMaxLength(200).IsRequired();
        builder.Property(b => b.NameEn).HasMaxLength(200).IsRequired();
        builder.Property(b => b.Slug).HasMaxLength(200).IsRequired();
        builder.Property(b => b.LogoUrl).HasMaxLength(500);
        builder.Property(b => b.BannerUrl).HasMaxLength(500);
        builder.Property(b => b.Website).HasMaxLength(200);
        builder.Property(b => b.DescriptionTr).HasMaxLength(2000);
        builder.Property(b => b.DescriptionEn).HasMaxLength(2000);
        builder.Property(b => b.MetaTitleTr).HasMaxLength(100);
        builder.Property(b => b.MetaTitleEn).HasMaxLength(100);
        builder.Property(b => b.MetaDescriptionTr).HasMaxLength(300);
        builder.Property(b => b.MetaDescriptionEn).HasMaxLength(300);

        builder.HasIndex(b => b.Slug).IsUnique();
        builder.HasIndex(b => b.IsActive);
    }
}

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.NameTr).HasMaxLength(200).IsRequired();
        builder.Property(c => c.NameEn).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Slug).HasMaxLength(200).IsRequired();
        builder.Property(c => c.ImageUrl).HasMaxLength(500);
        builder.Property(c => c.IconClass).HasMaxLength(100);
        builder.Property(c => c.DescriptionTr).HasMaxLength(2000);
        builder.Property(c => c.DescriptionEn).HasMaxLength(2000);
        builder.Property(c => c.MetaTitleTr).HasMaxLength(100);
        builder.Property(c => c.MetaTitleEn).HasMaxLength(100);
        builder.Property(c => c.MetaDescriptionTr).HasMaxLength(300);
        builder.Property(c => c.MetaDescriptionEn).HasMaxLength(300);

        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.Slug).IsUnique();
        builder.HasIndex(c => c.ParentId);
    }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.NameTr).HasMaxLength(300).IsRequired();
        builder.Property(p => p.NameEn).HasMaxLength(300).IsRequired();
        builder.Property(p => p.Slug).HasMaxLength(300).IsRequired();
        builder.Property(p => p.ProductCode).HasMaxLength(100);
        builder.Property(p => p.OemNumber).HasMaxLength(200);
        builder.Property(p => p.CrossReference).HasMaxLength(500);
        builder.Property(p => p.DescriptionTr).HasMaxLength(4000);
        builder.Property(p => p.DescriptionEn).HasMaxLength(4000);
        builder.Property(p => p.SpecificationsTr).HasMaxLength(4000);
        builder.Property(p => p.SpecificationsEn).HasMaxLength(4000);
        builder.Property(p => p.TechnicalDetailsTr).HasMaxLength(4000);
        builder.Property(p => p.TechnicalDetailsEn).HasMaxLength(4000);
        builder.Property(p => p.MetaTitleTr).HasMaxLength(100);
        builder.Property(p => p.MetaTitleEn).HasMaxLength(100);
        builder.Property(p => p.MetaDescriptionTr).HasMaxLength(300);
        builder.Property(p => p.MetaDescriptionEn).HasMaxLength(300);

        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.Slug).IsUnique();
        builder.HasIndex(p => p.BrandId);
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.ProductCode);
        builder.HasIndex(p => p.OemNumber);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.IsFeatured);
    }
}

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages");

        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.ImageUrl).HasMaxLength(500).IsRequired();
        builder.Property(pi => pi.ThumbnailUrl).HasMaxLength(500);
        builder.Property(pi => pi.AltText).HasMaxLength(200);

        builder.HasOne(pi => pi.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pi => pi.ProductId);
    }
}

public class ProductDocumentConfiguration : IEntityTypeConfiguration<ProductDocument>
{
    public void Configure(EntityTypeBuilder<ProductDocument> builder)
    {
        builder.ToTable("ProductDocuments");

        builder.HasKey(pd => pd.Id);

        builder.Property(pd => pd.TitleTr).HasMaxLength(200).IsRequired();
        builder.Property(pd => pd.TitleEn).HasMaxLength(200).IsRequired();
        builder.Property(pd => pd.FileUrl).HasMaxLength(500).IsRequired();
        builder.Property(pd => pd.FileType).HasMaxLength(50);

        builder.HasOne(pd => pd.Product)
            .WithMany(p => p.Documents)
            .HasForeignKey(pd => pd.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pd => pd.ProductId);
    }
}
