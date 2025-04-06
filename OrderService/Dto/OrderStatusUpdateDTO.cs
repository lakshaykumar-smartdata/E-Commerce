using System.ComponentModel.DataAnnotations;

namespace OrderService.Dto
{
    public class OrderStatusUpdateDTO
    {
        [Required]
        public string Status { get; set; } = string.Empty;
        [Required]
        public string CustomerEmail { get; set; } = string.Empty;
    }
}