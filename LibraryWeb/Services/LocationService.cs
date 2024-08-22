using LibraryAPI_Utility;
using LibraryWeb.Models;
using LibraryWeb.Models.DTO;
using LibraryWeb.Services.IServices;

namespace LibraryWeb.Services
{
    public class LocationService : BaseService, ILocationService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string locationUrl;

        public LocationService(IHttpClientFactory clientFactory, IConfiguration configuration):base(clientFactory)
        {
            _clientFactory = clientFactory;
            locationUrl = configuration.GetValue<string>("ServiceUrls:LibraryAPI");
        }
        public Task<T> CreateAsync<T>(LocationCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = locationUrl + "/api/LocationAPI",
                Token = token

            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType= SD.ApiType.DELETE,
                Url = locationUrl + "/api/LocationAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = locationUrl + "/api/LocationAPI",
                Token = token
            });

        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = locationUrl + "/api/LocationAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(LocationUpdateDTO dTO, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dTO,
                Url = locationUrl + "/api/LocationAPI/" + dTO.LocationId,
                Token = token

            });
        }
    }
}
