using Bunit;
using Bunit.TestDoubles;
using NUnit.Framework;
using NSubstitute;
using Microsoft.Extensions.Logging;
using ScaleStoreWebUI.Services;
using ScaleStoreWebUI.Components.Pages;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace UnitTestsWebUI
{
    public class CreateProjectTests : BunitTestContext
    {


        [SetUp]
        public void Setup()
        {
            // Mock ScaleStoreApiService with its dependencies
            var apiServiceMock = Substitute.For<ScaleStoreApiService>(Substitute.For<HttpClient>());
            TestContext.Services.AddSingleton<ScaleStoreApiService>(apiServiceMock);

            // Mock ILogger
            var loggerMock = Substitute.For<ILogger<ProjectsCreate>>();
            TestContext.Services.AddSingleton<ILogger<ProjectsCreate>>(loggerMock);
        }

        [Test]
        public void FormSubmission_CallsCreateProjectServiceMethod()
        {
            var apiServiceMock = Substitute.For<ScaleStoreApiService>(Substitute.For<HttpClient>());
            TestContext.Services.AddSingleton(apiServiceMock);

            var fakeNavigation = TestContext.Services.GetService<FakeNavigationManager>();

            var cut = RenderComponent<ProjectsCreate>();
            cut.Find("form").Submit();

            apiServiceMock.Received().CreateProject(Arg.Any<string>());
        }

        [Test]
        public async Task SuccessfulFormSubmission_NavigatesToProjectsPage()
        {
            var apiServiceMock = Substitute.For<ScaleStoreApiService>(Substitute.For<HttpClient>());
            
            TestContext.Services.AddSingleton(apiServiceMock);

            var fakeNavigation = TestContext.Services.GetService<FakeNavigationManager>();

            var cut = RenderComponent<ProjectsCreate>();
            cut.Find("form").Submit();

            Assert.IsTrue(fakeNavigation.Uri.EndsWith("/projects"));
        }

        [Test]
        public void GoBackButton_Click_NavigatesToProjectsPage()
        {
            var navigationManagerMock = Services.GetRequiredService<FakeNavigationManager>();

            var cut = RenderComponent<ProjectsCreate>();
            cut.Find("#GoBack").Click();

            Assert.IsTrue(navigationManagerMock.Uri.Contains("/projects"));

        }

        [Test]
        public void ComponentRenders_GoBackButton()
        {
            var cut = RenderComponent<ProjectsCreate>();
            var goBackButton = cut.Find("#GoBack");
            Assert.IsNotNull(goBackButton);
        }

        [Test]
        public void ComponentRenders_CreateProjectHeader()
        {
            var cut = RenderComponent<ProjectsCreate>();
            var header = cut.Find("h2");
            Assert.IsTrue(header.TextContent.Contains("Create Project"));
        }
    }
}
