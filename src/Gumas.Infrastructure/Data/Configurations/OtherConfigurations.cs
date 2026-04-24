using Gumas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gumas.Infrastructure.Data.Configurations;

public class SliderConfiguration : IEntityTypeConfiguration<Slider>
{
    public void Configure(EntityTypeBuilder<Slider> builder)
    {
        builder.ToTable("Sliders");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.NameTr).HasMaxLength(200).IsRequired();
        builder.Property(s => s.NameEn).HasMaxLength(200).IsRequired();
        builder.Property(s => s.SubtitleTr).HasMaxLength(500);
        builder.Property(s => s.SubtitleEn).HasMaxLength(500);
        builder.Property(s => s.ImageUrl).HasMaxLength(500).IsRequired();
        builder.Property(s => s.MobileImageUrl).HasMaxLength(500);
        builder.Property(s => s.VideoUrl).HasMaxLength(500);
        builder.Property(s => s.ButtonTextTr).HasMaxLength(100);
        builder.Property(s => s.ButtonTextEn).HasMaxLength(100);
        builder.Property(s => s.ButtonUrl).HasMaxLength(500);
        builder.Property(s => s.DescriptionTr).HasMaxLength(1000);
        builder.Property(s => s.DescriptionEn).HasMaxLength(1000);

        builder.HasIndex(s => s.Type);
        builder.HasIndex(s => s.IsActive);
    }
}

public class NewsConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.ToTable("News");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.NameTr).HasMaxLength(300).IsRequired();
        builder.Property(n => n.NameEn).HasMaxLength(300).IsRequired();
        builder.Property(n => n.Slug).HasMaxLength(300).IsRequired();
        builder.Property(n => n.ImageUrl).HasMaxLength(500);
        builder.Property(n => n.ThumbnailUrl).HasMaxLength(500);
        builder.Property(n => n.SummaryTr).HasMaxLength(1000);
        builder.Property(n => n.SummaryEn).HasMaxLength(1000);
        builder.Property(n => n.MetaTitleTr).HasMaxLength(100);
        builder.Property(n => n.MetaTitleEn).HasMaxLength(100);
        builder.Property(n => n.MetaDescriptionTr).HasMaxLength(300);
        builder.Property(n => n.MetaDescriptionEn).HasMaxLength(300);

        builder.HasIndex(n => n.Slug).IsUnique();
        builder.HasIndex(n => n.PublishDate);
        builder.HasIndex(n => n.IsFeatured);
    }
}

public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        builder.ToTable("ContactMessages");

        builder.HasKey(cm => cm.Id);

        builder.Property(cm => cm.FullName).HasMaxLength(200).IsRequired();
        builder.Property(cm => cm.Email).HasMaxLength(200).IsRequired();
        builder.Property(cm => cm.Phone).HasMaxLength(50);
        builder.Property(cm => cm.Company).HasMaxLength(200);
        builder.Property(cm => cm.Subject).HasMaxLength(300).IsRequired();
        builder.Property(cm => cm.Message).HasMaxLength(4000).IsRequired();
        builder.Property(cm => cm.IpAddress).HasMaxLength(50);
        builder.Property(cm => cm.UserAgent).HasMaxLength(500);
        builder.Property(cm => cm.AdminNotes).HasMaxLength(2000);

        builder.HasOne(cm => cm.Product)
            .WithMany()
            .HasForeignKey(cm => cm.ProductId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(cm => cm.Status);
        builder.HasIndex(cm => cm.CreatedAt);
    }
}

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.ToTable("Certificates");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.NameTr).HasMaxLength(200).IsRequired();
        builder.Property(c => c.NameEn).HasMaxLength(200).IsRequired();
        builder.Property(c => c.ImageUrl).HasMaxLength(500);
        builder.Property(c => c.FileUrl).HasMaxLength(500);
        builder.Property(c => c.IssuingOrganization).HasMaxLength(200);
        builder.Property(c => c.DescriptionTr).HasMaxLength(1000);
        builder.Property(c => c.DescriptionEn).HasMaxLength(1000);
    }
}

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("TeamMembers");

        builder.HasKey(tm => tm.Id);

        builder.Property(tm => tm.NameTr).HasMaxLength(200).IsRequired();
        builder.Property(tm => tm.NameEn).HasMaxLength(200).IsRequired();
        builder.Property(tm => tm.TitleTr).HasMaxLength(200);
        builder.Property(tm => tm.TitleEn).HasMaxLength(200);
        builder.Property(tm => tm.PhotoUrl).HasMaxLength(500);
        builder.Property(tm => tm.Email).HasMaxLength(200);
        builder.Property(tm => tm.Phone).HasMaxLength(50);
        builder.Property(tm => tm.LinkedInUrl).HasMaxLength(300);
        builder.Property(tm => tm.DescriptionTr).HasMaxLength(2000);
        builder.Property(tm => tm.DescriptionEn).HasMaxLength(2000);
    }
}

public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("Settings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Key).HasMaxLength(100).IsRequired();
        builder.Property(s => s.ValueTr).HasMaxLength(4000);
        builder.Property(s => s.ValueEn).HasMaxLength(4000);
        builder.Property(s => s.GroupName).HasMaxLength(100);

        builder.HasIndex(s => s.Key).IsUnique();
        builder.HasIndex(s => s.GroupName);
    }
}
