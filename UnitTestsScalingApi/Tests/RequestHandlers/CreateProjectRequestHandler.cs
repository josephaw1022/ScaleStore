using ScaleStoreHttpApi.Requests;

using ServiceScalingDb.ScalingDb;

namespace UnitTestsScalingApi.Tests.RequestHandlers;

[TestFixture]
public class CreateProjectRequestHandlerTests
{
	private CreateProjectRequestHandler _handler;
	private IScalingDbContext _dbContext;

	[SetUp]
	public void SetUp()
	{
		_dbContext = Substitute.For<IScalingDbContext>();
		_handler = new(_dbContext);
	}

	[Test]
	public async Task Handle_ShouldCreateProjectAndReturnCorrectResponse()
	{
		// Arrange
		var request = new CreateProjectRequest { Name = "Test Project" };
		_dbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

		// Act
		var response = await _handler.Handle(request, new CancellationToken());

		// Assert
		response.Should().NotBeNull();
		response.Name.Should().Be(request.Name);

		_dbContext.Projects.Received(1).Add(Arg.Is<Project>(p => p.ProjectName == request.Name));
		await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
	}

	// Additional tests as necessary...
}