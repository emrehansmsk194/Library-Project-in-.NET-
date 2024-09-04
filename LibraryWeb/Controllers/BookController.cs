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
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;
        private readonly ILocationService _locationService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public BookController(IBookService bookService, IMapper mapper, ILocationService locationService,
        ICategoryService categoryService) :base(categoryService)
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
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            var resp = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (resp != null && resp.IsSuccess)
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
     public async Task<IActionResult> BookPage(int bookId)
        {
            BookDTO book = new();
            var response =await _bookService.GetAsync<APIResponse>(bookId, HttpContext.Session.GetString(SD.SessionToken));
            if(response != null && response.IsSuccess)
            {
                book = JsonConvert.DeserializeObject<BookDTO>(JsonConvert.SerializeObject(response.Result));
                var categoryResponse = await _categoryService.GetAsync<APIResponse>(book.CategoryId,HttpContext.Session.GetString(SD.SessionToken));
                if (categoryResponse != null && categoryResponse.IsSuccess)
                {
                    var category = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(categoryResponse.Result));
                    book.CategoryName = category.CategoryName;
                }
            }

            return View(book);
        }
       
        [HttpGet]
        public async Task<IActionResult> BookByCategory(int categoryId)
        {
            List<BookDTO> books = new();
            var response = await _bookService.GetByCategoryAsync<APIResponse>(categoryId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                books = JsonConvert.DeserializeObject<List<BookDTO>>(JsonConvert.SerializeObject(response.Result));
				var locationResponse = await _locationService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
				if (locationResponse != null && locationResponse.IsSuccess)
				{
					var locations = JsonConvert.DeserializeObject<List<LocationDTO>>(JsonConvert.SerializeObject(locationResponse.Result));
					foreach (var book in books)
					{
						var location = locations.FirstOrDefault(u => u.LocationId == book.LocationId);
						if (location != null)
						{
							book.LocationFloor = location.Floor;
							book.LocationShelf = location.Shelf;
						}
					}
				}
				var categoryResponse = await _categoryService.GetAsync<APIResponse>(categoryId, HttpContext.Session.GetString(SD.SessionToken));
				if (categoryResponse != null && categoryResponse.IsSuccess)
				{
					var category = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(categoryResponse.Result));
					ViewBag.CategoryName = category.CategoryName;
				}
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
            if (!string.IsNullOrEmpty(value))
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
        [HttpGet]
        public async Task<IActionResult> SearchBook(string search = "")
        {
            List<BookDTO> bookList = new();
            var response = await _bookService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                bookList = JsonConvert.DeserializeObject<List<BookDTO>>(Convert.ToString(response.Result));
                var categoryResponse = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if (categoryResponse != null && categoryResponse.IsSuccess)
                {
                    var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(JsonConvert.SerializeObject(categoryResponse.Result));
                    foreach (var book in bookList)
                    {
                        var category = categories.FirstOrDefault(u => u.CategoryId == book.CategoryId);
                        if(category != null)
                        {
                            book.CategoryName = category.CategoryName;
                        }
                }
                }
                var locationResponse = await _locationService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if(locationResponse != null && locationResponse.IsSuccess )
                {
                    var locations = JsonConvert.DeserializeObject<List<LocationDTO>>(JsonConvert.SerializeObject(locationResponse.Result));
                    foreach(var book in bookList)
                    {
                        var location = locations.FirstOrDefault(u => u.LocationId == book.LocationId); 
                        if(location != null)
                        {
                            book.LocationFloor = location.Floor;
                            book.LocationShelf = location.Shelf;
                        }
                    }
                }
              if(!string.IsNullOrEmpty(search))
                {
                    bookList = bookList.Where(u => u.Name.ToLower().Contains(search.ToLower())).ToList(); 
                }

            }
          
            return View(bookList);
        }
        [HttpPost]
        public async Task<IActionResult> SearchBookPost(string search)
        {
            return RedirectToAction("SearchBook","Book", new { search = search });
        }
    }
}
