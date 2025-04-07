using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public record OrderCreated
    {
        public int OrderId { get; init; }
        public string CustomerEmail { get; init; } = default!;
        public string SellerEmail { get; init; } = default!;

    }
}
