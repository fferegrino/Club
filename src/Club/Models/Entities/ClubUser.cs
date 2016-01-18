using Microsoft.AspNet.Identity.EntityFramework;

namespace Club.Models.Entities
{
    public class ClubUser : IdentityUser
    {
        public bool Approved { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
}
