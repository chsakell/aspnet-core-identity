namespace AspNetCoreIdentity.ViewModels
{
    public class UserStateVM
    {
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public string AuthenticationMethod { get; set; }
    }
}
