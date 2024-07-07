using Microsoft.AspNetCore.Identity;

namespace ApiProjectPRN231.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int StatusId { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
        //----------------------//
        public virtual OrderStatus? Status { get; set; }
        public virtual UserApp? User { get; set; }
        public virtual ICollection<OrderDetails>? Details { get; set; }
    }
}
