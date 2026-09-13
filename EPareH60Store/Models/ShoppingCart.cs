using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPareH60Store.Models
{
    public class ShoppingCart
    {
        [Key]
        public int CartId { get; set; }

        public int CustomerId { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateCreated { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;

        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}