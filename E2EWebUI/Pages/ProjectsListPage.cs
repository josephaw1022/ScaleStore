namespace E2EWebUI.Pages;

public class ProjectsListPage
{
    private readonly IPage _page;
    private readonly ILocator _createButton;
    private readonly ILocator _projectRows;
    private readonly ILocator _loadingElement;
    private readonly ILocator _nameInput;

    public ProjectsListPage(IPage page)
    {
        _page = page;
        _createButton = page.Locator("button.btn-primary");
        _projectRows = page.Locator("table.table tbody tr");
        _loadingElement = page.Locator("p:contains('Loading')");
        _nameInput = page.Locator("input[name='Name']"); // Adjust the selector as per the actual attribute
    }

    public async Task GoToAsync()
    {
        await _page.GotoAsync($"{ScaleStoreWebUIPage.BaseUrl}/projects");
    }

    public async Task<bool> IsLoadedAsync()
    {
        try
        {
            return !(await _loadingElement.IsVisibleAsync());
        }
        catch (PlaywrightException)
        {
            return true; // If the element is not found, the page is considered loaded
        }
    }

    public async Task ClickCreateAsync()
    {
        await _createButton.ClickAsync();
    }

    public async Task<List<string>> GetProjectNamesAsync()
    {
        var projectNames = new List<string>();
        var count = await _projectRows.CountAsync();
        for (int i = 0; i < count; i++)
        {
            var name = await _projectRows.Nth(i).Locator("td:first-child").TextContentAsync();
            projectNames.Add(name);
        }
        return projectNames;
    }

    public async Task<bool> IsAtAsync()
    {
        return _page.Url.ToString() == $"{ScaleStoreWebUIPage.BaseUrl}/projects";
    }

    public ILocator CreateButton => _createButton; // Expose the create button

    public async Task EnterNameAsync(string name)
    {
        await _nameInput.FillAsync(name);
    }


    public async Task<int> GetProjectRowCountAsync()
    {
        return await _page.Locator("[data-test-id^='project-row-']").CountAsync();
    }

}
