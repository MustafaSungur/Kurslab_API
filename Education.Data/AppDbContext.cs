using Education.Entity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Education.Data
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		// Diğer DbSet tanımlamaları
		public DbSet<Category> Categories { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<CommentLike> CommentLikes { get; set; }
		public DbSet<Content> Contents { get; set; }
		public DbSet<ContentTag> ContentTags { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public DbSet<Tag> Tags { get; set; }
	

		// ContentUser DbSet tanımlaması
		public DbSet<ContentUser> ContentUsers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Tüm string tiplerini PostgreSQL için 'text' olarak ayarla
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				var properties = entity.GetProperties()
					.Where(p => p.ClrType == typeof(string));

				foreach (var property in properties)
				{
					property.SetColumnType("text");  // nvarchar yerine text kullanıyoruz
				}
			}

			// Fluent API ile ilişkiler
			modelBuilder.Entity<ContentTag>()
				.HasKey(ct => new { ct.ContentId, ct.TagId });

			// ContentUser için ilişkileri tanımlıyoruz
			modelBuilder.Entity<ContentUser>()
				.HasKey(cu => new { cu.UserId, cu.ContentId }); // Composite key (Birleşik anahtar)

			modelBuilder.Entity<ContentUser>()
				.HasOne(cu => cu.User)
				.WithMany(u => u.ViewedContents)
				.HasForeignKey(cu => cu.UserId)
				.OnDelete(DeleteBehavior.Cascade); // Kullanıcı silindiğinde izleme geçmişi de silinir

			modelBuilder.Entity<ContentUser>()
				.HasOne(cu => cu.Content)
				.WithMany(c => c.ViewedUsers)
				.HasForeignKey(cu => cu.ContentId)
				.OnDelete(DeleteBehavior.Cascade); // İçerik silindiğinde izleme geçmişi de silinir

			// Diğer ilişkiler
			modelBuilder.Entity<Content>()
				.HasMany(c => c.Comments)
				.WithOne(co => co.Content)
				.HasForeignKey(co => co.ContentId)
				.OnDelete(DeleteBehavior.Cascade);


			modelBuilder.Entity<Comment>()
				.HasOne(c => c.Content)
				.WithMany(co => co.Comments)
				.HasForeignKey(c => c.ContentId)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Rating>()
			   .HasOne(r => r.Content)
			   .WithMany(c => c.Ratings)
			   .HasForeignKey(r => r.ContentId)
			   .OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<CommentLike>()
			  .HasOne(cl => cl.Comment)
			  .WithMany(c => c.Likes)
			  .HasForeignKey(cl => cl.CommentId)
			  .OnDelete(DeleteBehavior.NoAction);
		}
	}
}
