using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogApp.Infrastructure.Persistence
{
    // Temel veri ekleme işlemleri için kullanılan sınıf
    public class DataSeeder(
        AppDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager)
    {

        public async Task SeedAsync()
        {
            if (await context.Users.AnyAsync())
            {
                return;
            }

            // 1. Rolleri oluştur
            await SeedRolesAsync();

            // 2. Kullanıcıları oluştur
            var adminUser = await SeedAdminUserAsync();
            await SeedRegularUserAsync();

            // 3. Diğer verileri oluştur (Kategoriler, Post'lar, Yorumlar)
            await SeedSampleDataAsync(adminUser);
        }

        private async Task SeedRolesAsync()
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole<int>("User"));
            }
        }

        private async Task<User> SeedAdminUserAsync()
        {
            var adminUser = new User
            {
                UserName = "admin",
                Email = "admin@blogapp.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            return adminUser;
        }

        private async Task SeedRegularUserAsync()
        {
            var regularUser = new User
            {
                UserName = "testuser",
                Email = "user@blogapp.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(regularUser, "User123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(regularUser, "User");
            }
        }

        private async Task SeedSampleDataAsync(User adminUser)
        {
            // 5 Kategori oluştur
            var categories = new List<Category>
            {
                new("Teknoloji"),
                new("Yazılım Geliştirme"),
                new("Seyahat"),
                new("Sağlıklı Yaşam"),
                new("Finans")
            };
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // 10 Post oluştur (hepsi admin tarafından)
            var posts = new List<Post>();
            var random = new Random();
            for (int i = 1; i <= 10; i++)
            {
                posts.Add(new Post(
                    $"Örnek Post Başlığı {i}", 
                    $"Bu, {i}. örnek postun içeriğidir. Lorem ipsum dolor sit amet...",
                    adminUser.Id,
                    categories[random.Next(categories.Count)].Id));
            }
            await context.Posts.AddRangeAsync(posts);

            // Veritabanı ID'lerinin oluşması için değişiklikleri burada kaydedelim.
            // Bu sayede yorumları post'lara bağlayabiliriz.
            await context.SaveChangesAsync();

            // 20 Yorum oluştur (hepsi admin tarafından, rastgele post'lara)
            var comments = new List<Comment>();
            for (int i = 1; i <= 20; i++)
            {
                comments.Add(new Comment($"Bu, {i}. harika bir yorumdur!",
                    adminUser.Id,
                    posts[random.Next(posts.Count)].Id));
            }
            await context.Comments.AddRangeAsync(comments);

            // Son olarak tüm değişiklikleri veritabanına kaydet
            await context.SaveChangesAsync();
        }
    }
}