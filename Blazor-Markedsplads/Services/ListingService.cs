using System.Collections.Generic;
using Npgsql;
using Blazor_Markedsplads;

public class ListingService
{
    private readonly string _connectionString;

    public ListingService(IConfiguration configuration)
    {
         _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    private readonly List<Product> listings = new List<Product>();

    public void AddListing(Product item)
    {
        listings.Add(item);
    }

}
