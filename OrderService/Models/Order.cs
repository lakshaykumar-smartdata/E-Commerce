using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment for MSSQL
        public int OrderId { get; set; }

        [Required] // Ensures ProductId cannot be null
        public Guid ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Processing";

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required] // Ensures CustomerId cannot be null
        public int CustomerId { get; set; }
    }
}
