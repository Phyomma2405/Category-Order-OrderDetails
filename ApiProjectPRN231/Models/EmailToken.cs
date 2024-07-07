using Microsoft.AspNetCore.Identity;

namespace ApiProjectPRN231.Models
{
    public class EmailToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiredTime { get; set; }

        public bool Deleted { get; set; }

        public virtual UserApp User { get; set; }
    }
}
