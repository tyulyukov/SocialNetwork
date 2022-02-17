using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using SocialNetwork.Data.Entities;

namespace SocialNetwork.Data.Configurations
{
    internal class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder
                .Property(p => p.RegistrationDate)
                .HasDefaultValueSql("getdate()");

            builder
                .Property(p => p.AvatarImageUrl)
                .HasDefaultValue("/storage/avatars/default-avatar.png");

            builder
                .HasMany(p => p.Posts)
                .WithOne(pp => pp.Author)
                .HasForeignKey(pp => pp.Author.Id);

            builder
                .HasMany(p => p.Likes)
                .WithOne(pl => pl.Author)
                .HasForeignKey(pl => pl.Author.Id);

            builder
                .HasMany(p => p.Comments)
                .WithOne(pc => pc.Author)
                .HasForeignKey(pc => pc.Author.Id);

            builder
                .HasMany(c => c.Followers)
                .WithOne(s => s.)
                .UsingEntity<Enrollment>(
                   j => j
                    .HasOne(pt => pt.Student)
                    .WithMany(t => t.Enrollments)
                    .HasForeignKey(pt => pt.StudentId),
                j => j
                    .HasOne(pt => pt.Course)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(pt => pt.CourseId),
                j =>
                {
                    j.Property(pt => pt.EnrollmentDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                    j.Property(pt => pt.Mark).HasDefaultValue(3);
                    j.HasKey(t => new { t.CourseId, t.StudentId });
                    j.ToTable("Enrollments");
                });
        }
    }
}
