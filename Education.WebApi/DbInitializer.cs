using Education.Data;
using Education.Entity.Enums;
using Education.Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Education.WebApi
{
	public static class DbInitializer
	{
		public static async Task SeedData(IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.CreateScope())
			{
				var scopedServiceProvider = scope.ServiceProvider;
				var context = scopedServiceProvider.GetRequiredService<AppDbContext>();
				var roleManager = scopedServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = scopedServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

				await context.Database.MigrateAsync();

				// Rolleri oluştur
				string[] roleNames = ["Admin", "User", "UserAndInstructor"];
				foreach (var roleName in roleNames)
				{
					if (!await roleManager.RoleExistsAsync(roleName))
					{
						await roleManager.CreateAsync(new IdentityRole(roleName));
					}
				}

				// Admin kullanıcı oluştur
				var adminEmail = "admin@admin.com";
				var adminUser = await userManager.FindByEmailAsync(adminEmail);
				if (adminUser == null)
				{
					adminUser = new ApplicationUser
					{
						UserName = "admin",
						Email = adminEmail,
						FirstName = "Admin",
						LastName = "User",
						EmailConfirmed = true,
						BirthDate = new DateTime(1999, 1, 12).ToUniversalTime(),
						Role = UserRole.Admin,
						State = State.Active
					};

					string adminPassword = "Admin123!";
					var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword);

					if (createAdminResult.Succeeded)
					{
						await userManager.AddToRoleAsync(adminUser, "Admin");
					}
					else
					{
						var errors = string.Join(", ", createAdminResult.Errors.Select(e => e.Description));
						throw new Exception($"Admin kullanıcı oluşturulamadı: {errors}");
					}
				}


				// Kategorileri ve Alt Kategorileri oluştur
				if (!await context.Categories.AnyAsync())
				{
					// Ana kategorileri ekleyelim
					var categories = new List<Category>
					{
						new Category { Name = "Programlama" },
						new Category { Name = "Veri Bilimi" },
						new Category { Name = "Siber Güvenlik" },
						new Category { Name = "İş ve Yönetim" },
						new Category { Name = "Grafik Tasarım" }
					};

					await context.Categories.AddRangeAsync(categories);
					await context.SaveChangesAsync();

					// Alt kategorileri ekleyelim (ParentId belirterek hiyerarşi oluşturuyoruz)
					var subCategories = new List<Category>
					{
						new Category { Name = "Web Geliştirme", ParentId = categories[0].Id },
						new Category { Name = "Mobil Geliştirme", ParentId = categories[0].Id },
						new Category { Name = "Makine Öğrenmesi", ParentId = categories[1].Id },
						new Category { Name = "Veri Analizi", ParentId = categories[1].Id },
						new Category { Name = "Ağ Güvenliği", ParentId = categories[2].Id },
						new Category { Name = "Etik Hacking", ParentId = categories[2].Id },
						new Category { Name = "Proje Yönetimi", ParentId = categories[3].Id },
						new Category { Name = "Liderlik ve Yönetim", ParentId = categories[3].Id },
						new Category { Name = "Grafik Araçları", ParentId = categories[4].Id },
						new Category { Name = "3D Tasarım ve Animasyon", ParentId = categories[4].Id }
					};

					await context.Categories.AddRangeAsync(subCategories);
					await context.SaveChangesAsync();

					// Etiketleri oluştur
					if (!await context.Tags.AnyAsync())
					{
						var tags = new List<Tag>
						{
							new Tag { Name = "HTML" },
							new Tag { Name = "CSS" },
							new Tag { Name = "JavaScript" },
							new Tag { Name = "Python" },
							new Tag { Name = "Flutter" },
							new Tag { Name = "React Native" },
							new Tag { Name = "Makine Öğrenimi" },
							new Tag { Name = "Veri Analizi" },
							new Tag { Name = "Ağ Güvenliği" },
							new Tag { Name = "Proje Yönetimi" },
							new Tag { Name = "UI/UX Tasarımı" },
							new Tag { Name = "Derin Öğrenme" },
							new Tag { Name = "TensorFlow" },
							new Tag { Name = "Keras" },
							new Tag { Name = "Pandas" },
							new Tag { Name = "Numpy" },
							new Tag { Name = "SQL" },
							new Tag { Name = "NoSQL" },
							new Tag { Name = "MongoDB" },
							new Tag { Name = "Firebase" },
							new Tag { Name = "AWS" },
							new Tag { Name = "Azure" },
							new Tag { Name = "Docker" },
							new Tag { Name = "Kubernetes" },
							new Tag { Name = "Linux" },
							new Tag { Name = "Git" },
							new Tag { Name = "GitHub" },
							new Tag { Name = "CI/CD" },
							new Tag { Name = "Agile" },
							new Tag { Name = "Scrum" },
							new Tag { Name = "Penetrasyon Testi" },
							new Tag { Name = "Kali Linux" },
							new Tag { Name = "Metasploit" },
							new Tag { Name = "Veri Görselleştirme" },
							new Tag { Name = "Matplotlib" },
							new Tag { Name = "Seaborn" },
							new Tag { Name = "DataFrame" },
							new Tag { Name = "Statistik" },
							new Tag { Name = "Big Data" },
							new Tag { Name = "Apache Hadoop" },
							new Tag { Name = "Apache Spark" },
							new Tag { Name = "Machine Learning Ops (MLOps)" },
							new Tag { Name = "C#" },
							new Tag { Name = "Java" },
							new Tag { Name = "TypeScript" },
							new Tag { Name = "React.js" },
							new Tag { Name = "Vue.js" },
							new Tag { Name = "Angular" },
							new Tag { Name = "Node.js" },
							new Tag { Name = "Express.js" },
							new Tag { Name = "REST API" },
							new Tag { Name = "GraphQL" },
							new Tag { Name = "DevOps" },
							new Tag { Name = "CI/CD" },
							new Tag { Name = "Automation" },
							new Tag { Name = "Blockchain" },
							new Tag { Name = "Cryptocurrency" },
							new Tag { Name = "NFT" },
							new Tag { Name = "Adobe Photoshop" },
							new Tag { Name = "Adobe Illustrator" },
							new Tag { Name = "Blender" },
							new Tag { Name = "3D Modelleme" },
							new Tag { Name = "Oyun Geliştirme" },
							new Tag { Name = "Unity" },
							new Tag { Name = "Unreal Engine" },
							new Tag { Name = "C++" },
							new Tag { Name = "Siber Güvenlik" },
							new Tag { Name = "Ethical Hacking" }
						};


						await context.Tags.AddRangeAsync(tags);
						await context.SaveChangesAsync();
					}

				}
			}
		}
	}
}