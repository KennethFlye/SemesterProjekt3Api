namespace SemesterProjekt3Api.Service
{
    public interface IServiceConneciton
    {
        public HttpClient HttpEnabler { get; init; }

        Task<HttpResponseMessage?> CallServicePost(HttpRequestMessage postRequest);
    }
}
