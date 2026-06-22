using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaturalezaViva.Domain.Entities;
using NaturalezaViva.Domain.Enums;

namespace NaturalezaViva.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(255);

            email.HasIndex(e => e.Value)
                 .IsUnique()
                 .HasDatabaseName("IX_Users_Email");
        });

        builder.OwnsOne(u => u.Document, doc =>
        {
            doc.Property(d => d.Type)
                .HasColumnName("DocumentType")
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            doc.Property(d => d.Number)
                .HasColumnName("DocumentNumber")
                .IsRequired()
                .HasMaxLength(20);

            doc.HasIndex(d => new { d.Type, d.Number })
               .IsUnique()
               .HasDatabaseName("IX_Users_Document");
        });

        builder.OwnsOne(u => u.PasswordHash, pass =>
        {
            pass.Property(p => p.Value)
                .HasColumnName("PasswordHash")
                .IsRequired()
                .HasMaxLength(500);
        });

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt);
    }
}