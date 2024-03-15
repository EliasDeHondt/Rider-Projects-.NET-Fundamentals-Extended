/***************************************
 *                                     *
 *   Created by Elias De Hondt         *
 *   Visit https://eliasdh.com         *
 *                                     *
 ***************************************/

using System.Net;
using Microsoft.Extensions.DependencyInjection;
using PadelClubManagement.BL;
using Tests.Config;
using Xunit;

namespace Tests.IntegrationTests;

public class ClubsControllerTests : IClassFixture<ExtendedWebApplicationFactoryWithMockAuth<Program>>
{
    private readonly ExtendedWebApplicationFactoryWithMockAuth<Program> _factory;
    
    public ClubsControllerTests(ExtendedWebApplicationFactoryWithMockAuth<Program> factory) // Constructor
    {
        _factory = factory; // Create a new web application factory
    }
    
    [Fact]                                                     // [HttpGet("/api/clubs")]
    public void GetAllClubs_Return200_GivenValidEndpointData() // Method: public IActionResult GetAllClubs();
    {
        // Arrange
        var client = _factory.CreateClient(); // Create an HTTP client
        
        // Act
        var response = client.GetAsync("/api/clubs").Result; // Send a GET request to the specified URI
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Expected: 200
        Assert.NotNull(response.Content); // Expected: Not null
        Assert.True(response.Content.Headers.ContentLength > 0); // Expected: True
    }
    
    [Fact]                                                       // [HttpGet("/api/clubs")]
    public void GetAllClubs_Return204_GivenInValidEndpointData() // Method: public IActionResult GetAllClubs();
    {
        // Arrange
        DropDatabase(); // Drop the database
        var client = _factory.CreateClient(); // Create an HTTP client
        
        // Act
        var response = client.GetAsync("/api/clubs").Result; // Send a GET request to the specified URI
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);  // Expected: 204
    }
    
    private void DropDatabase()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        IManager manager = services.GetRequiredService<IManager>();
        manager.DeleteAllClubs(); // Delete all clubs
    }
}