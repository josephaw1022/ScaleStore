namespace ScaleStoreWebUI.Services
{
    public class ProjectStateService
    {
        public string SelectedProject { get; set; } = "Test Project";

        public event Action OnChange;

        public void SetSelectedProject(string project)
        {
            SelectedProject = project;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

}
