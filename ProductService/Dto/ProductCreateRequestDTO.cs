using System.ComponentModel.DataAnnotations;

namespace ProductService.Dto
{
    public class ProductCreateRequestDTO
    {
        public Guid ProductId { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 10000)]
        public int Stock { get; set; }

        [Required]
        public int SellerId { get; set; }
    }
}
