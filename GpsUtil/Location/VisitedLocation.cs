namespace GpsUtil.Location;

public class VisitedLocation
{
    public Guid UserId { get; }
    public Locations Location { get; }
    public DateTime TimeVisited { get; }

    public VisitedLocation(Guid userId, Locations location, DateTime timeVisited)
    {
        UserId = userId;
        Location = location;
        TimeVisited = timeVisited;
    }
}
