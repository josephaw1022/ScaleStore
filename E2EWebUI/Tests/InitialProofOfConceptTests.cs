using E2EWebUI.Pages;

namespace E2EWebUI.Tests;

[TestFixture]
internal class InitialProofOfConceptTests : PageTest
{


    private async Task<ProjectsListPage> GenerateProjectsListPage ()
    {
        var projectsListPage = new ProjectsListPage(Page);
        await projectsListPage.GoToAsync();
        return projectsListPage;
    }

    [Test]
    public async Task TestProjectsPageLoadsCorrectly()
    {
        var projectsPage = await GenerateProjectsListPage();
        var isLoaded = await projectsPage.IsLoadedAsync();
        isLoaded.Should().BeTrue("because the projects page should load correctly");
    }

    [Test]
    public async Task TestProjectsPageShowsCreateButton()
    {
        var projectsPage = await GenerateProjectsListPage();
        var isLoaded = await projectsPage.IsLoadedAsync();
        isLoaded.Should().BeTrue("because the projects page should load correctly");

        var isVisible = await projectsPage.CreateButton.IsVisibleAsync();
        isVisible.Should().BeTrue("because the create button should be visible on the projects page");
    }

    [Test]
    public async Task TestProjectsPageUrlIsCorrect()
    {
        var projectsPage = new ProjectsListPage(Page);
        await projectsPage.GoToAsync();
        var isAtCorrectUrl = await projectsPage.IsAtAsync();
        isAtCorrectUrl.Should().BeTrue("because the URL should be correct for the projects page");
    }

    [Test]
    public async Task TestCorrectNumberOfProjectsLoaded()
    {
        var projectsPage = new ProjectsListPage(Page);
        await projectsPage.GoToAsync();
        var rowCount = await projectsPage.GetProjectRowCountAsync();
        var expectedRowCount = 2;
        rowCount.Should().Be(expectedRowCount, "because that is the expected number of projects");
    }

}
