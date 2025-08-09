using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Core.Interface;
using LayMa.Core.Model.ShortLink;
using LayMa.WebAPI.Controllers.AdminApi;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace LayMa.Tests.Controllers
{
    public class UserControllerTests : TestBase
    {
        private readonly UserController _controller;
        private readonly IUnitOfWork _unitOfWork;

        public UserControllerTests() : base()
        {
            // Setup UnitOfWork
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.ViewDetails.GetTopUsersByClicks(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .ReturnsAsync((DateTime start, DateTime end, int top) => 
                {
                    var users = _context.Users.ToList();
                    var viewDetails = _context.ViewDetails
                        .Where(x => x.DateCreated >= start && x.DateCreated <= end)
                        .ToList();

                    var userClickCounts = viewDetails
                        .GroupBy(x => x.UserId)
                        .Select(g => new
                        {
                            UserId = g.Key,
                            ClickCount = g.GroupBy(x => new { x.CampainId, x.ShortLinkId, x.Device, x.IPAddress }).Count()
                        })
                        .OrderByDescending(x => x.ClickCount)
                        .Take(top)
                        .ToList();

                    return userClickCounts.Select(tu => new ThongKeViewClickByUser
                    {
                        Id = tu.UserId,
                        UserName = users.FirstOrDefault(u => u.Id == tu.UserId)?.UserName ?? "Unknown",
                        Click = tu.ClickCount,
                        View = 0
                    }).ToList();
                });

            _unitOfWork = unitOfWorkMock.Object;

            // Create controller
            _controller = new UserController(
                _userManager,
                _serviceProvider.GetRequiredService<SignInManager<AppUser>>(),
                _unitOfWork);
        }

        [Fact]
        public async Task GetTopUsersByClicks_ReturnsTop4UsersThisWeek()
        {
            // Arrange
            var user1 = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "user1",
                Email = "user1@example.com"
            };
            var user2 = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "user2",
                Email = "user2@example.com"
            };
            var user3 = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "user3",
                Email = "user3@example.com"
            };
            var user4 = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "user4",
                Email = "user4@example.com"
            };

            await _userManager.CreateAsync(user1, "Test123!");
            await _userManager.CreateAsync(user2, "Test123!");
            await _userManager.CreateAsync(user3, "Test123!");
            await _userManager.CreateAsync(user4, "Test123!");

            // Create clicks for this week
            var today = DateTime.Now.Date;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            
            var clicks = new List<ViewDetail>
            {
                // User1: 10 clicks
                new ViewDetail { Id = Guid.NewGuid(), UserId = user1.Id, DateCreated = startOfWeek.AddDays(1), CampainId = Guid.NewGuid(), ShortLinkId = Guid.NewGuid(), Device = "Desktop", IPAddress = "192.168.1.1" },
                new ViewDetail { Id = Guid.NewGuid(), UserId = user1.Id, DateCreated = startOfWeek.AddDays(1), CampainId = Guid.NewGuid(), ShortLinkId = Guid.NewGuid(), Device = "Desktop", IPAddress = "192.168.1.2" },
                new ViewDetail { Id = Guid.NewGuid(), UserId = user1.Id, DateCreated = startOfWeek.AddDays(2), CampainId = Guid.NewGuid(), ShortLinkId = Guid.NewGuid(), Device = "Mobile", IPAddress = "192.168.1.3" },
                
                // User2: 8 clicks
                new ViewDetail { Id = Guid.NewGuid(), UserId = user2.Id, DateCreated = startOfWeek.AddDays(1), CampainId = Guid.NewGuid(), ShortLinkId = Guid.NewGuid(), Device = "Desktop", IPAddress = "192.168.1.4" },
                new ViewDetail { Id = Guid.NewGuid(), UserId = user2.Id, DateCreated = startOfWeek.AddDays(2), CampainId = Guid.NewGuid(), ShortLinkId = Guid.NewGuid(), Device = "Mobile", IPAddress = "192.168.1.5" },
                
                // User3: 5 clicks
                new ViewDetail { Id = Guid.NewGuid(), UserId = user3.Id, DateCreated = startOfWeek.AddDays(3), CampainId = Guid.NewGuid(), ShortLinkId = Guid.NewGuid(), Device = "Desktop", IPAddress = "192.168.1.6" },
                
                // User4: 3 clicks
                new ViewDetail { Id = Guid.NewGuid(), UserId = user4.Id, DateCreated = startOfWeek.AddDays(4), CampainId = Guid.NewGuid(), ShortLinkId = Guid.NewGuid(), Device = "Desktop", IPAddress = "192.168.1.7" }
            };

            await _context.ViewDetails.AddRangeAsync(clicks);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetTopUsersByClicks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var topUsers = Assert.IsType<List<ThongKeViewClickByUser>>(okResult.Value);
            
            Assert.Equal(4, topUsers.Count);
            
            // Check order (should be sorted by clicks descending)
            Assert.Equal("user1", topUsers[0].UserName);
            Assert.Equal(3, topUsers[0].Click); // 3 unique clicks (different IP/Device combinations)
            
            Assert.Equal("user2", topUsers[1].UserName);
            Assert.Equal(2, topUsers[1].Click);
            
            Assert.Equal("user3", topUsers[2].UserName);
            Assert.Equal(1, topUsers[2].Click);
            
            Assert.Equal("user4", topUsers[3].UserName);
            Assert.Equal(1, topUsers[3].Click);
        }

        [Fact]
        public async Task GetTopUsersByClicks_WithNoClicksThisWeek_ReturnsEmptyList()
        {
            // Arrange - Create users but no clicks for this week
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "testuser",
                Email = "test@example.com"
            };
            await _userManager.CreateAsync(user, "Test123!");

            // Create clicks for last week (outside current week)
            var lastWeek = DateTime.Now.AddDays(-7);
            var clicks = new List<ViewDetail>
            {
                new ViewDetail { Id = Guid.NewGuid(), UserId = user.Id, DateCreated = lastWeek, CampainId = Guid.NewGuid(), ShortLinkId = Guid.NewGuid(), Device = "Desktop", IPAddress = "192.168.1.1" }
            };

            await _context.ViewDetails.AddRangeAsync(clicks);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetTopUsersByClicks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var topUsers = Assert.IsType<List<ThongKeViewClickByUser>>(okResult.Value);
            
            Assert.Empty(topUsers);
        }
    }
} 