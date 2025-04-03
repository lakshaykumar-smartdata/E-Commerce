using System.ComponentModel.DataAnnotations;

namespace OrderService.Dto
{
    public class OrderStatusUpdateDTO
    {
        [Required]
        public string Status { get; set; }
    }
}