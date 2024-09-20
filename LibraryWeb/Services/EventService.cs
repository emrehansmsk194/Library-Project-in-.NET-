using LibraryAPI_Utility;
using LibraryWeb.Models;
using LibraryWeb.Models.DTO;
using LibraryWeb.Services.IServices;

namespace LibraryWeb.Services
{
    public class EventService : BaseService, IEventService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string eventUrl;
        public EventService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
            eventUrl = configuration.GetValue<string>("ServiceUrls:LibraryAPI");
            
        }

        public Task<T> CreateAsync<T>(EventCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = eventUrl + "/api/EventAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType= SD.ApiType.DELETE,
                Url = eventUrl + "/api/EventAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType= SD.ApiType.GET,
                Url = eventUrl + "/api/EventAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = eventUrl + "/api/EventAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(EventUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = eventUrl + "/api/EventAPI/" + dto.EventId,
                Token = token
            });
        }
    }
}
