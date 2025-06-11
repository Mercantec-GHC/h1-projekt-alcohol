namespace Blazor_Markedsplads.Models
{
    public class CartItem
{

        public int ID { get; set; }
        public int Quantity { get; set; } = 1;
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public string? SellerName { get; set; }
        public int? SellerPhone { get; set; }
        public ListingModel Listing { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}
