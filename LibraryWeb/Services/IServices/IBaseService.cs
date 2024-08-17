using LibraryWeb.Models;

namespace LibraryWeb.Services.IServices 
{
    public interface IBaseService {
        APIResponse ResponseModel {get;set;}
        Task<T>SendAsync<T>(APIRequest apiRequest);
    }
}