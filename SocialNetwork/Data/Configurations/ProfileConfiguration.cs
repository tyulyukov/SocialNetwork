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
                .HasMany(p => p.Followers)
                .WithMany(f => f.Following)
                .UsingEntity(j => j.ToTable("Follows"));

            builder
                .HasMany(p => p.Following)
                .WithMany(f => f.Followers)
                .UsingEntity(j => j.ToTable("Follows"));
        }
    }
}
