using AutoMapper;
using LibraryAPI.Models.DTO;
using LibraryAPI.Models;
using LibraryAPI.Repository;
using LibraryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        public EventAPIController(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            this._response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllEvents()
        {
            try
            {
                IEnumerable<Event> eventList = await _eventRepository.GetAllAsync();
                _response.Result = _mapper.Map<List<EventDTO>>(eventList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("{id:int}", Name = "GetEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEvent(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var location = await _eventRepository.GetAsync(u => u.EventId == id);
                if (location == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                _response.Result = _mapper.Map<LocationDTO>(location);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> CreateEvent([FromBody] EventCreateDTO eventDTO)
        {
            try
            {
                if (await _eventRepository.GetAsync(u => u.EventName.ToLower() == eventDTO.EventName.ToLower()) != null)
                {
                    _response.IsSuccess = false;
                    ModelState.AddModelError("CustomError", "Event already exists !");
                    return BadRequest(ModelState);
                }
                if (eventDTO == null)
                {
                    _response.IsSuccess = false;
                    return BadRequest(eventDTO);
                }
                Event model = _mapper.Map<Event>(eventDTO);
                await _eventRepository.CreateAsync(model);
                await _eventRepository.SaveAsync();
                _response.Result = _mapper.Map<EventDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetEvent", new { id = model.EventId }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpDelete("{id:int}", Name = "DeleteEvent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteEvent(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    return BadRequest();
                }
                var location = await _eventRepository.GetAsync(u => u.EventId == id);
                if (location == null)
                {
                    _response.IsSuccess = false;
                    return NotFound();
                }
                await _eventRepository.RemoveAsync(location);
                await _eventRepository.SaveAsync();
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpPut("{id:int}", Name = "UpdateEvent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateEvent(int id, [FromBody] EventUpdateDTO eventDTO)
        {
            try
            {

                if (eventDTO == null || id != eventDTO.EventId)
                {
                    return BadRequest();
                }
                Event model = _mapper.Map<Event>(eventDTO);
                await _eventRepository.UpdateAsync(model);
                await _eventRepository.SaveAsync();
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }

    }
}
