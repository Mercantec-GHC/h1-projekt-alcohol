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
    private readonly List<ProductModel> listings = new List<ProductModel>();

    public void AddListing(ProductModel item)
    {
        listings.Add(item);
    }

}
