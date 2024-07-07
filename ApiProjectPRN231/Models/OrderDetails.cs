using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiProjectPRN231.Models
{
    public class OrderDetails
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        [Range(0, 100)]
        public int? Discount { get; set; } = 0;

        //--------------------------//
        public virtual Product? Product { get; set; }
        public virtual Order? Order { get; set; }

    }
}
