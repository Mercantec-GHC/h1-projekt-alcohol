using System.Collections.Generic;

public class ListingService
{
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
