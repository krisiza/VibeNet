using Microsoft.AspNetCore.Mvc;

namespace VibeNet.Components
{
    public class SearchComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult<IViewComponentResult>(View());
        }
    }
}
