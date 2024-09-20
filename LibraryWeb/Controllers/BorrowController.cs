using AutoMapper;
using LibraryAPI_Utility;
using LibraryWeb.Models;
using LibraryWeb.Models.DTO;
using LibraryWeb.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Security.Claims;

namespace LibraryWeb.Controllers
{
	public class BorrowController : BaseController
	{
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;
		private readonly IBookService _bookService;
		private readonly IAuthService _authService;
	
        public BorrowController(ICategoryService categoryService, IMapper mapper,
			IBookService bookService, IAuthService authService) : base(categoryService)
        {
            _categoryService = categoryService;
			_mapper = mapper;
			_bookService = bookService;
			_authService = authService;
        }
		[HttpGet]
        public async Task<IActionResult> AssignToUser(int id)
		{
			List<UserDTO> users = new();
			var response = await _authService.GetAllUsersAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if(response != null && response.IsSuccess)
			{
				users = JsonConvert.DeserializeObject<List<UserDTO>>(JsonConvert.SerializeObject(response.Result));
				var bookResponse = await _bookService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
				if (bookResponse != null && bookResponse.IsSuccess)
				{
					var book = JsonConvert.DeserializeObject<BookDTO>(Convert.ToString(bookResponse.Result));
					string BookImageUrl = book.CoverImageUrl;
					ViewBag.BookImageUrl = BookImageUrl;
				}
				
			}
			
			ViewBag.BookId = id;
			return View(users);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles ="admin")]
		public async Task<IActionResult> AssignToUser(int id, string userId)
		{
			var response = await _bookService.BorrowBookForUserAsync<APIResponse>(id, userId, HttpContext.Session.GetString(SD.SessionToken));
			if(response != null && response.IsSuccess)
			{
				TempData["success"] = "Book successfully borrowed for the user.";
				return RedirectToAction("Index", "Home");
			}
			else
			{
				TempData["error"] = "Error occurred while borrowing the book.";
				return View("Error");
			}
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Borrowed()
		{
			List<BookDTO> books = new();
			var response = await _bookService.GetBorrowedBookAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if(response != null && response.IsSuccess)
			{
				books = JsonConvert.DeserializeObject<List<BookDTO>>(JsonConvert.SerializeObject(response.Result));
			}
			return View(books);

			
		}
	

		}
	}

