using AutoMapper;
using Azure;
using LibraryAPI.Models;
using LibraryAPI.Models.DTO;
using LibraryAPI.Repository;
using LibraryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        public LocationAPIController(ILocationRepository locationRepository, IMapper mapper)
        {
           _mapper = mapper;
            _locationRepository = locationRepository;
            this._response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        public async Task<ActionResult<APIResponse>> GetAllLocations()
        {
            try
            {
                IEnumerable<Location> locationList = await _locationRepository.GetAllAsync();
                _response.Result = _mapper.Map<List<LocationDTO>>(locationList);
                _response.StatusCode =HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("{id:int}",Name ="GetLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetLocation(int id)
        {
            try
            {
                if(id == 0)
                {
                    _response.IsSuccess=false;
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var location = await _locationRepository.GetAsync(u => u.LocationId == id);
                if(location == null)
                {
                    _response.IsSuccess=false;
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
        public async Task<ActionResult<APIResponse>> CreateLocation([FromBody] LocationCreateDTO locationDTO)
        {
            try
            {
                if(locationDTO == null)
                {
                    _response.IsSuccess = false;
                    return BadRequest(locationDTO);
                }
                Location model = _mapper.Map<Location>(locationDTO);
                await _locationRepository.CreateAsync(model);
                await _locationRepository.SaveAsync();
                _response.Result = _mapper.Map<LocationDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetLocation", new { id = model.LocationId }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpDelete("{id:int}", Name = "DeleteLocation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteLocation(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    return BadRequest();
                }
                var location = await _locationRepository.GetAsync(u => u.LocationId == id);
                if (location == null)
                {
                    _response.IsSuccess = false;
                    return NotFound();
                }
                await _locationRepository.RemoveAsync(location);
                await _locationRepository.SaveAsync();
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
        [HttpPut("{id:int}", Name = "UpdateLocation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateLocation(int id, [FromBody] LocationUpdateDTO locationDTO)
        {
            try
            {

                if (locationDTO == null || id != locationDTO.LocationId)
                {
                    return BadRequest();
                }
                Location model = _mapper.Map<Location>(locationDTO);
                await _locationRepository.UpdateAsync(model);
                await _locationRepository.SaveAsync();
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
