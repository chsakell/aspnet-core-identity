using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers
{
    public class ClientAppSettings : Controller
    {
        public IActionResult Index()
        {
            var settings = new
            {
                stsServer = "http://localhost:5005",
                redirect_url = "http://localhost:5000",
                client_id = "angularclient",
                response_type = "code",
                scope = "openid profile SocialAPI",
                post_logout_redirect_uri = "http://localhost:5000",
                start_checksession = true,
                silent_renew = true,
                post_login_route = "/home",
                forbidden_route = "/forbidden",
                unauthorized_route = "/unauthorized",
                log_console_warning_active = true,
                log_console_debug_active = true,
                max_id_token_iat_offset_allowed_in_seconds = 10,
                apiServer = "http://localhost:5010"
            };

            return Ok(settings);
        }
    }
}
