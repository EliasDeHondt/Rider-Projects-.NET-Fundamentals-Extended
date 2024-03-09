/***************************************
 *                                     *
 *   Created by Elias De Hondt         *
 *   Visit https://eliasdh.com         *
 *                                     *
 ***************************************/

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PadelClubManagement.BL;
using PadelClubManagement.BL.Domain;
using PadelClubManagement.DAL.EF;
using PadelClubManagement.UI.Web.Controllers;
using Tests.Config;

namespace Tests.IntegrationTests;

public class PlayerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    
    public PlayerTests(CustomWebApplicationFactory<Program> factory) // Constructor
    {
        _factory = factory; // Create a new web application factory
    }
    
    [Fact]                                                     // [Authorize] + [HttpPost]
    public void Add_ReturnRedirectToAction_GivenValidUser() // Method: public IActionResult Add(Player player);
    {
        // Arrange
        using IServiceScope scope = _factory.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        IManager manager = services.GetRequiredService<IManager>();
        PlayerController controller = new PlayerController(manager);
        
        Player player = manager.GetPlayer(1); // Get a valid player from the database
        
        // Simulate an authenticated user
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new (ClaimTypes.Email, "user1@eliasdh.com") }, "mock"));

        // Set the HttpContext user to the simulated user
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        
        // Act
        IActionResult result = controller.Add(player);
        
        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Detail", redirectToActionResult.ActionName);
        Assert.Equal(player.PlayerNumber, redirectToActionResult.RouteValues?["playerNumber"]);
    }

    [Fact]                                                     // [Authorize] + [HttpPost]
    public void Add_ReturnRedirectToAction_GivenInvalidUser() // Method: public IActionResult Add(Player player);
    {
        // Arrange
        using IServiceScope scope = _factory.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        IManager manager = services.GetRequiredService<IManager>();
        PlayerController controller = new PlayerController(manager);
        
        Player player = manager.GetPlayer(1); // Get a valid player from the database
        
        // Simulate an unauthenticated user
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        
        // Set the HttpContext user to the simulated user
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        
        // Act and Assert
        Assert.Throws<ValidationException>(() =>
        {
            IActionResult result = controller.Add(player);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Detail", ((RedirectToActionResult)result).ActionName);
            Assert.Equal(player.PlayerNumber, ((RedirectToActionResult)result).RouteValues?["playerNumber"]);
        });
    }
}