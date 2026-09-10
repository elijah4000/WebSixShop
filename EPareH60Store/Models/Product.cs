using System;
using System.Collections.Generic;

namespace EPareH60Store.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int ProdCatId { get; set; }

    public string? Description { get; set; }

    public string? Manufacturer { get; set; }

    public int Stock { get; set; }

    public decimal? BuyPrice { get; set; }

    public decimal? SellPrice { get; set; }

    public virtual ProductCategory ProdCat { get; set; } = null!;
}
