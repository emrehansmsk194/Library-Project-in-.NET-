using AutoMapper;
using LibraryAPI_Utility;
using LibraryWeb.Models;
using LibraryWeb.Models.DTO;
using LibraryWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LibraryWeb.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;
        public BookController(IBookService bookService, IMapper mapper, ILocationService locationService)
        {
            _mapper = mapper;
            _bookService = bookService;
            _locationService = locationService;
        }
        public IActionResult IndexBook()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> BookByLocation(int locationId)
        {
            List<BookDTO> books = new();
            var response = await _bookService.GetByLocationAsync<APIResponse>(locationId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                books = JsonConvert.DeserializeObject<List<BookDTO>>(JsonConvert.SerializeObject(response.Result));
            }
            return View(books);

        }
        [HttpGet]
        public async Task<IActionResult> Shelves()
        {
            List<LocationDTO> locations = new();
            var locationResponse = await _locationService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (locationResponse != null && locationResponse.IsSuccess)
            {
                locations = JsonConvert.DeserializeObject<List<LocationDTO>>(JsonConvert.SerializeObject(locationResponse.Result));
            }
            List<ShelfViewModel> shelves = new List<ShelfViewModel>();
            foreach (var location in locations)
            {
                var books = await _bookService.GetByLocationAsync<APIResponse>(location.LocationId, HttpContext.Session.GetString(SD.SessionToken));
                if (books != null && books.IsSuccess)
                {
                    shelves.Add(new ShelfViewModel
                    {
                        ShelfCode = $"{location.Floor}{location.Shelf}",
                        Books = JsonConvert.DeserializeObject<List<BookDTO>>(JsonConvert.SerializeObject(books.Result))
                    });
                }
            }
			if (shelves.Count == 0)
			{
				ViewBag.Message = "No shelves or books found.";
			}
			return View(shelves);
        }
    }
}
