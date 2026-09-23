using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPareH60Store.Models
{
    [Table("ProductCategory")]
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int CategoryId { get; set; }

        [NotMapped]
        public int CategoryID { get => CategoryId; set => CategoryId = value; }

        [Required]
        [StringLength(60)]
        public string ProdCat { get; set; } = null!;

        [InverseProperty("Category")]
        public virtual ICollection<Product> Products { get; set; }
    }
}