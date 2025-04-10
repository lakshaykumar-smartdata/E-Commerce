﻿using System.ComponentModel.DataAnnotations;

namespace OrderService.Dto
{
    public class OrderRequestDTO
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
        
        [Required] // Ensures CustomerId cannot be null
        public int CustomerId { get; set; }
        [Required] 
        public string CustomerEmail { get; set; } = string.Empty;
    }
}
