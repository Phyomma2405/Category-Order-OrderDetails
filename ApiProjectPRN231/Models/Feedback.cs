using System.ComponentModel.DataAnnotations;

namespace ApiProjectPRN231.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }

        [MinLength(20)]
        public string Message { get; set; }

        public virtual Product? Product { get; set; }
        public virtual UserApp? User {  get; set; }
    }
}
