using DevStore.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevStore.WebApp.MVC.Extensions
{
    public class PagingViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPagedList pagingModel)
        {
            return View(pagingModel);
        }
    }
}