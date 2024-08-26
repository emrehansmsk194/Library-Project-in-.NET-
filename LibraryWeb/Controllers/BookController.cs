using AutoMapper;
using LibraryAPI_Utility;
using LibraryWeb.Models;
using LibraryWeb.Models.DTO;
using LibraryWeb.Models.VM;
using LibraryWeb.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LibraryWeb.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ILocationService _locationService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public BookController(IBookService bookService, IMapper mapper, ILocationService locationService,
        ICategoryService categoryService)
        {
            _mapper = mapper;
            _bookService = bookService;
            _locationService = locationService;
            _categoryService = categoryService;
        }
       
        

        public async Task<IActionResult> IndexBook()
        {
            List<BookDTO> bookList = new List<BookDTO>();
            var response = await _bookService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)
            {
                bookList = JsonConvert.DeserializeObject<List<BookDTO>>(Convert.ToString(response.Result));

                var categoryResponse = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (categoryResponse != null && categoryResponse.IsSuccess)
                {
                    var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result));

                    foreach (var book in bookList)
                    {
                        var category = categories.FirstOrDefault(c => c.CategoryId == book.CategoryId);
                        if (category != null)
                        {
                            book.CategoryName = category.CategoryName;
                        }
                    }
                }
            }

            return View(bookList);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateBook()
        {
            BookCreateVm bookCreateVM = new();
            List<BookDTO> bookList = new();
            var response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                bookCreateVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result)).Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.CategoryId.ToString()
                });
            }
            return View(bookCreateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateBook(BookCreateVm model)
        {
            if (ModelState.IsValid)
            {
                var response = await _bookService.CreateAsync<APIResponse>(model.Book, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Book created successfully.";
                    return RedirectToAction(nameof(IndexBook));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            var resp = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (resp != null && resp.IsSuccess)
            {
                model.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(resp.Result)).Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.CategoryId.ToString()
                });
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateBook(int id)
        {
            BookUpdateVm bookVM = new();
            var response = await _bookService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                BookDTO model = JsonConvert.DeserializeObject<BookDTO>(Convert.ToString(response.Result));
                bookVM.Book = _mapper.Map<BookUpdateDTO>(model);
            }
            response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                bookVM.CategoryList = JsonConvert.DeserializeObject<List<BookDTO>>(Convert.ToString(response.Result)).Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.CategoryId.ToString()
                });
                return View(bookVM);
            }
            return NotFound();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateBook(BookUpdateVm model)
        {
            if (ModelState.IsValid)
            {
                var response = await _bookService.UpdateAsync<APIResponse>(model.Book, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Book updated successfully.";
                    return RedirectToAction(nameof(IndexBook));
                }
                else
                {
                    if(response.ErrorMessages.Count >0)
                    {
                        ModelState.AddModelError("ErrorMessages",response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            var resp = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if(resp != null && resp.IsSuccess)
            {
                model.CategoryList = JsonConvert.DeserializeObject<List<BookDTO>>(Convert.ToString(resp.Result)).Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.CategoryId.ToString()
                });
            }
            TempData["error"] = "Error encountered.";
            return View(model);
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
        public async Task<IActionResult> Shelves(string value = "") 
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
            if(!string.IsNullOrEmpty(value))
            {
                shelves = shelves.Where(u => u.ShelfCode.ToLower().Contains(value.ToLower())).ToList();
            }
			if (shelves.Count == 0)
			{
				ViewBag.Message = "No shelves or books found.";
			}
			return View(shelves);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShelvesPost(string value) // select elemanındaki name ile bu fonksiyonun parametre ismi ayni olmali.
        {
            return RedirectToAction("Shelves", new { value = value });
        }
    }
   
}
