using AutoMapper;
using LibraryAPI.Models;
using LibraryAPI.Models.DTO;
using LibraryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Text.Json;


namespace LibraryAPI.Controllers
{
    [Route("api/LibraryBookAPI")]
    [ApiController]
    public class LibraryAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public LibraryAPIController(IBookRepository bookRepository, IMapper mapper)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
            this._response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllBooks([FromQuery] string? search)
        {
            try
            {
                IEnumerable<Book> bookList = await _bookRepository.GetAllAsync();
                if (!string.IsNullOrEmpty(search))
                {
                    bookList = bookList.Where(b => b.Name.ToLower().Contains(search));
                }
                if(!User.IsInRole("admin"))
                {
                    bookList = bookList.Where(b => !b.IsAdminOnly);
                }
                _response.Result = _mapper.Map<List<BookDTO>>(bookList);
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
        [HttpGet("{id:int}",Name ="GetBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBook(int id)
        {
            try
            {
                if(id == 0)
                {
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var book = await _bookRepository.GetAsync(u => u.BookId == id);
                if (book == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                if (book.IsAdminOnly && !User.IsInRole("admin"))
                {
                    _response.IsSuccess = false;
                    return Forbid(); 
                }
                _response.Result = _mapper.Map<BookDTO>(book);
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

        public async Task<ActionResult<APIResponse>> CreateBook([FromBody] BookCreateDTO bookDTO)
        {
            try
            {
                if (await _bookRepository.GetAsync(u => u.Name.ToLower() == bookDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("CustomError", "Book Already Exists!");
                    return BadRequest(ModelState);
                }
                if (bookDTO == null)
                {
                    _response.IsSuccess = false;
                    return BadRequest(bookDTO);
                }

                Book model = _mapper.Map<Book>(bookDTO);


                //if (await _categoryRepository.GetAsync(u => u.Id == bookDTO.CategoryId) == null)
                //{
                //    ModelState.AddModelError("CustomError", "Invalid CategoryId!");
                //    return BadRequest(ModelState);
                //}
                await _bookRepository.CreateAsync(model);
                await _bookRepository.SaveAsync();
                _response.Result = _mapper.Map<BookDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetBook", new { id = model.BookId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;



        }
        [HttpDelete("{id:int}", Name = "DeleteBook")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteBook(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    return BadRequest();
                }
                var book = await _bookRepository.GetAsync(u => u.BookId == id);

                if (book == null)
                {
                    _response.IsSuccess = false;
                    return NotFound();
                }
                await _bookRepository.RemoveAsync(book);
                await _bookRepository.SaveAsync();
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
        [HttpPut("{id:int}", Name = "UpdateBook")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "admin")]



        public async Task<ActionResult<APIResponse>> UpdateBook(int id, [FromBody] BookUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.BookId)
                {
                    return BadRequest();
                }
                Book model = _mapper.Map<Book>(updateDTO);
                await _bookRepository.UpdateAsync(model);
                await _bookRepository.SaveAsync();
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
        [HttpGet("category = {CategoryId:int}",Name ="GetBookByCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBookByCategory(int CategoryId)
        {
            try
            {
                if(CategoryId == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var book =  await _bookRepository.GetAllAsync(u => u.CategoryId == CategoryId);
                if(book.Count == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                if(!User.IsInRole("admin"))
                {
                    book = book.Where(b => !b.IsAdminOnly).ToList();
                }
                _response.Result = _mapper.Map<List<BookDTO>>(book);
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
        [HttpPost("{id:int}",Name ="BorrowBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<ActionResult<APIResponse>> BorrowBook(int id)
        {
            try
            {
                if(id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var book = await _bookRepository.GetAsync(u => u.BookId == id);
                if(book == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                if (book.IsBorrowed)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Book is already borrowed." };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "User ID could not be found." };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                book.IsBorrowed = true;
                book.BorrowedDate = DateTime.Now;
                book.ReturnDate = DateTime.Now.AddDays(7);
                book.UserId = userId;

                await _bookRepository.UpdateAsync(book);
                await _bookRepository.SaveAsync();
                _response.Result = _mapper.Map<BookDTO>(book);
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
        //GetBookByLocation 



    }
}
