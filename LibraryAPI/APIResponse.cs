namespace LibraryAPI
{
    using System.Net;
    using System;
    public class APIResponse
    {
        public HttpStatusCode StatusCode {  get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public  object Result { get; set; }
    }
}
