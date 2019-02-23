using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Contact>> Get()
        {
            return new List<Contact>
            {
                new Contact
                {
                    Name = "Francesca Fenton",
                    Username = "Fenton25",
                    Email = "francesca@example.com"
                },
                new Contact {
                    Name = "Pierce North",
                    Username = "Pierce",
                    Email = "pierce@example.com"
                },
                new Contact {
                    Name = "Marta Grimes",
                    Username = "GrimesX",
                    Email = "marta@example.com"
                },
                new Contact{
                    Name = "Margie Kearney",
                    Username = "Kearney20",
                    Email = "margie@example.com"
                }
            };
        }
    }

    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public Contact()
        {
            Id = Guid.NewGuid();
        }
    }
}
