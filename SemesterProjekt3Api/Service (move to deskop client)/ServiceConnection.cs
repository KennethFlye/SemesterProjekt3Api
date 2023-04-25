namespace SemesterProjekt3Api.Service
{
    public class ServiceConnection : IServiceConneciton;
    {
        public async Task<HttpResponseMessage?> 
            CallServicePost(HttpRequestMessage postRequest) 
        {
            HttpResponseMessage? hrm = null; 
            if (UseUrl != null) 
            { 
                hrm = await HttpEnabler.SendAsync(postRequest); 
            } 
           return hrm; 
        }
    }
}
