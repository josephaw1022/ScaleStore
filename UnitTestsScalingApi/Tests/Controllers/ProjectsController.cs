using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ScaleStoreHttpApi.Controllers;
using ScaleStoreHttpApi.Requests;
using ServiceScalingDTO;
using ServiceScalingWebApi.Requests;

namespace ScaleStoreHttpApi.Tests.Controllers;

[TestFixture]
public class ProjectsControllerTests
{
    private ProjectsController _controller;
    private IMediator _mediator;

    [SetUp]
    public void SetUp()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new ProjectsController(_mediator);
    }
    [Test]
    public async Task GetProjects_ReturnsListOfProjects()
    {
        // Arrange
        var mockResponse = new List<ProjectTableViewRequestResponse>
        {
            new() { Id = 1, Name = "Test Project" }
        };

        _mediator.Send(Arg.Any<ProjectsTableViewRequest>()).Returns(mockResponse);

        // Act
        var result = await _controller.GetProjects();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(mockResponse);
    }

    [Test]
    public async Task GetProjectNames_ReturnsListOfProjectNames()
    {
        // Arrange
        var mockResponse = new List<ProjectGetManyNamesResponseItem>
    {
        new ProjectGetManyNamesResponseItem { Id = 1, Name = "Test Project" }
    };
        _mediator.Send(Arg.Any<ProjectGetManyNamesRequest>()).Returns(mockResponse);

        // Act
        var result = await _controller.GetProjectNames();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(mockResponse);
    }

    [Test]
    public async Task GetProject_ReturnsProject_WhenProjectExists()
    {
        // Arrange
        var mockResponse = new ProjectsGetOneRequestResponse { Id = 1, Name = "Test Project" };
        _mediator.Send(Arg.Any<ProjectsGetOneRequest>()).Returns(mockResponse);

        // Act
        var result = await _controller.GetProject(1);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(mockResponse);
    }

    [Test]
    public async Task GetProject_ReturnsNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        _mediator.Send(Arg.Any<ProjectsGetOneRequest>()).Returns((ProjectsGetOneRequestResponse)null);

        // Act
        var result = await _controller.GetProject(1);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task PutProject_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var updateProject = new ProjectUpdateDTO { Id = 1, Name = "Updated Project" };
        
        var mockResponse = new UpdateProjectResponse
        {
            Id = 1,
            Name = "Updated Project",
            IsSuccess = true
        };

        _mediator.Send(Arg.Is<UpdateProjectRequest>(req => req.Id == 1 && req.Name == "Updated Project")).Returns(mockResponse);

        // Act
        var result = await _controller.PutProject(1, updateProject);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task PutProject_ReturnsBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange
        var updateProject = new ProjectUpdateDTO { Id = 2, Name = "Updated Project" };

        // Act
        var result = await _controller.PutProject(1, updateProject);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task PutProject_ReturnsNotFound_WhenUpdateFails()
    {
        // Arrange
        var updateProject = new ProjectUpdateDTO { Id = 1, Name = "Updated Project" };
        var mockResponse = new UpdateProjectResponse
        {
            Id = updateProject.Id,
            Name = updateProject.Name,
            IsSuccess = false
        };

        _mediator.Send(Arg.Any<UpdateProjectRequest>()).Returns(mockResponse);

        // Act
        var result = await _controller.PutProject(1, updateProject);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task PostProject_ReturnsCreatedAtAction_WhenCreationIsSuccessful()
    {
        // Arrange
        var createProjectDto = new ProjectCreateDTO { Name = "New Project" };
        var mockResponse = new CreateProjectResponse { Id = 1, Name = "New Project" };
        _mediator.Send(Arg.Is<CreateProjectRequest>(req => req.Name == "New Project")).Returns(mockResponse);

        // Act
        var result = await _controller.PostProject(createProjectDto);

        // Assert
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        createdAtActionResult.Should().NotBeNull();
        createdAtActionResult.ActionName.Should().Be("GetProject");
        createdAtActionResult.RouteValues["id"].Should().Be(1);
        createdAtActionResult.Value.Should().BeEquivalentTo(mockResponse);
    }

    [Test]
    public async Task DeleteProject_ThrowsNotImplementedException()
    {
        // Arrange & Act & Assert
        Assert.ThrowsAsync<NotImplementedException>(() => _controller.DeleteProject(1));
    }


}
