using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public record OrderShipped
    {
        public int OrderId { get; init; }
        public DateTime ShippedAt { get; init; }
        public string TrackingNumber { get; init; } = default!;
        public string CustomerEmail { get; init; } = default!;
    }
}
