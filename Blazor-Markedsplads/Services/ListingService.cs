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
    private readonly List<Listing> listings = new List<Listing>();

    public void AddListing(Listing item)
    {
        listings.Add(item);
    }

    public IEnumerable<Listing> GetAllListings()
    {
        return listings;
    }
}
