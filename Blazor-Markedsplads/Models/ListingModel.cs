using Microsoft.AspNetCore.SignalR;

public class ListingModel

{
    public string Title { get; set; } = string.Empty;       
   // public string? Description { get; set; }                  
    public decimal Price { get; set; }                        
    public string Category { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    public int QuantityToAdd { get; set; } = 1;
    public string? Nationality { get; set; }
     public decimal Percent { get; set; }
    public int? Age { get; set; }
    public int ProductId { get; set; }                      
    public int CustomerId { get; set; }                       
}