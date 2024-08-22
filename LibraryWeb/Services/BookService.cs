using LibraryAPI_Utility;
using LibraryWeb.Models;
using LibraryWeb.Models.DTO;
using LibraryWeb.Services.IServices;

namespace LibraryWeb.Services
{
    public class BookService : BaseService, IBookService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string bookUrl;
        public BookService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            bookUrl = configuration.GetValue<string>("ServiceUrls:LibraryAPI");
        }
        public Task<T> CreateAsync<T>(BookCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = bookUrl + "/api/LibraryBookAPI",
                Token = token

            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = bookUrl + "/api/LibraryBookAPI/" + id,
                Token = token

            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = bookUrl + "/api/LibraryBookAPI",
                Token = token

            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = bookUrl + "/api/LibraryBookAPI/" + id,
                Token = token

            });
        }

        public Task<T> GetByCategoryAsync<T>(int categoryId, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = bookUrl + "/api/LibraryBookAPI/category=" + categoryId,
                Token = token

            });
        }

        public Task<T> GetByLocationAsync<T>(int locationId, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = bookUrl + "/api/LibraryBookAPI/location=" + locationId,
                Token = token

            });

        }

        public Task<T> UpdateAsync<T>(BookUpdateDTO dTO, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dTO,
                Url = bookUrl + "/api/LibraryBookAPI/" + dTO.BookId,
                Token = token

            });
        }
    }
}
