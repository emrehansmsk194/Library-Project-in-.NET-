using AutoMapper;
using LibraryAPI_Utility;
using LibraryWeb.Models;
using LibraryWeb.Models.DTO;
using LibraryWeb.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LibraryWeb.Controllers
{
    public class EventController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;
        public EventController(ICategoryService categoryService, IMapper mapper, IEventService eventService) : base(categoryService) 
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _eventService = eventService;
        }
        [HttpGet]
        public async Task<IActionResult> IndexEvent()
        {
            List<EventDTO> events = new List<EventDTO>();
            var response = await  _eventService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                events = JsonConvert.DeserializeObject<List<EventDTO>>(Convert.ToString(response.Result));
            }
            return View(events);
        }
        [Authorize(Roles ="admin")]
        [HttpGet]
        public async Task<IActionResult> CreateEvent()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateEvent(EventCreateDTO model)
        {
        
            
                var response = await _eventService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if(response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexEvent));
                }
            
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ShowEvents()
        {
            List<EventDTO> events = new List<EventDTO>();
            var response = await _eventService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if(response != null && response.IsSuccess)
            {
                events = JsonConvert.DeserializeObject<List<EventDTO>>(Convert.ToString(response.Result));
            }
            return View(events);
        }
    }
}
