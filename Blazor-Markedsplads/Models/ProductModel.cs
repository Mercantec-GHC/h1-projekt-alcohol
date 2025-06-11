using System.ComponentModel.DataAnnotations;

public class ProductModel
{
    public int ID { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title can be up to 100 characters")]
    public string ProductName { get; set; } = string.Empty; //title

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 1000000, ErrorMessage = "Price must be between 0.01 and 1000000")]
    public decimal Price { get; set; }

    [StringLength(500, ErrorMessage = "Description can be up to 500 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public string ProductType { get; set; } = string.Empty; //Category

    [Required(ErrorMessage = "Image URL is required")]
    [StringLength(2083, ErrorMessage = "URL is too long")]  // 2083 is a common max URL length
    public string ImageUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Percentage is required")]
    [Range(0, 100, ErrorMessage = "The percentage must be between 0% and 100%")]
    public decimal Percent { get; set; }
    public string? Nationality { get; set; }
    public int? Age { get; set; }
    public int QuantityToAdd { get; set; } = 1;

    //public int? AlcoholId { get; set; } //do we need alcohol ID?
    public int CustomerID { get; set; }
    public string SellerName { get; set; } = string.Empty;
}
