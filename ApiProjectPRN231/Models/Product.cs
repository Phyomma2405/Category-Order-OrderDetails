namespace ApiProjectPRN231.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Url { get; set; }

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Feedback>? Feedbacks { get; set; }
    }
}
