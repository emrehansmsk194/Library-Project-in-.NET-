using LibraryAPI_Utility;
using LibraryWeb.Models;
using LibraryWeb.Models.DTO;
using LibraryWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace LibraryWeb.Controllers
{
	public class BaseController : Controller
	{
		private readonly ICategoryService _categoryService;
        public BaseController(ICategoryService categoryService)
        {
           _categoryService = categoryService;
        }
		public override async void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);
            var response = _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken)).GetAwaiter().GetResult();
            if (response != null && response.IsSuccess)
			{
				var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));
				ViewData["Categories"] = categories;
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Failed to load categories from API.");
			}
		}
	}
}
