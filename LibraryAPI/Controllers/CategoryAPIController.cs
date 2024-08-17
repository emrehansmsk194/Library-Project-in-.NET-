using AutoMapper;
using LibraryAPI.Models.DTO;
using LibraryAPI.Models;
using LibraryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        public CategoryAPIController(IMapper mapper, ICategoryRepository categoryRepo)
        {
            _mapper = mapper;
            _categoryRepo = categoryRepo;
            this._response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
      
        public async Task<ActionResult<APIResponse>> GetCategories([FromQuery] string? search)
        {
            try
            {
                IEnumerable<Category> categoryList = await _categoryRepo.GetAllAsync();
                if (!string.IsNullOrEmpty(search))
                {
                    categoryList = categoryList.Where(u => u.CategoryName.ToLower().Contains(search));
                }
              
                _response.Result = _mapper.Map<List<CategoryDTO>>(categoryList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<String> { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCategory(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var Category = await _categoryRepo.GetAsync(u => u.CategoryId == id);
                if (Category == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<CategoryDTO>(Category);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<String> { ex.ToString() };
            }
            return _response;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles ="admin")]
  
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            try
            {
                if (await _categoryRepo.GetAsync(u => u.CategoryName.ToLower() == categoryCreateDTO.CategoryName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Category already exists!");
                    return BadRequest(ModelState);
                }
                if (categoryCreateDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                Category category = _mapper.Map<Category>(categoryCreateDTO);
                await _categoryRepo.CreateAsync(category);
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<CategoryDTO>(category);
                return CreatedAtRoute("GetCategory", new { id = category.CategoryId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<String> { ex.ToString() };
            }
            return _response;


        }
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<APIResponse>> DeleteCategory(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var Category = await _categoryRepo.GetAsync(u => u.CategoryId == id);
                if (Category == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                await _categoryRepo.RemoveAsync(Category);
                await _categoryRepo.SaveAsync();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<String> { ex.ToString() };
            }
            return _response;


        }
        [HttpPut("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateCategory(int id, CategoryUpdateDTO categoryUpdateDTO)
        {
            try
            {
                if (id != categoryUpdateDTO.CategoryId || categoryUpdateDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Category Category = _mapper.Map<Category>(categoryUpdateDTO);
                await _categoryRepo.UpdateAsync(Category);
                await _categoryRepo.SaveAsync();
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<String> { ex.ToString() };
            }
            return _response;
        }
    }
}
