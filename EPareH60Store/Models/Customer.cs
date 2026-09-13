using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPareH60Store.Models
{
	public class Customer
	{
		[Key]
		public int CustomerId { get; set; }

		[Required, StringLength(20)]
		public string FirstName { get; set; } = string.Empty;

		[Required, StringLength(30)]
		public string LastName { get; set; } = string.Empty;

		[Required, StringLength(30), EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required, StringLength(10)]
		public string PhoneNumber { get; set; } = string.Empty;

		[Required, StringLength(2)]
		public string Province { get; set; } = string.Empty;

		[StringLength(16)]
		public string? CreditCard { get; set; }

		public virtual ShoppingCart? ShoppingCart { get; set; }

		public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
	}
}