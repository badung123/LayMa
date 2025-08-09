using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LayMa.Tests
{
    public abstract class TestBase : IDisposable
    {
        protected readonly LayMaContext _context;
        protected readonly UserManager<AppUser> _userManager;
        protected readonly RoleManager<IdentityRole<Guid>> _roleManager;
        protected readonly IServiceProvider _serviceProvider;

        protected TestBase()
        {
            var services = new ServiceCollection();

            // Setup in-memory database
            services.AddDbContext<LayMaContext>(options =>
                options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));

            // Setup Identity
            services.AddIdentity<AppUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<LayMaContext>()
                .AddDefaultTokenProviders();

            // Add other required services
            services.AddLogging();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            _serviceProvider = services.BuildServiceProvider();
            _context = _serviceProvider.GetRequiredService<LayMaContext>();
            _userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            _roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            // Initialize database
            InitializeDatabase().Wait();
        }

        private async Task InitializeDatabase()
        {
            // Create roles if needed
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));
            }
            // Add other initialization as needed
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
} 