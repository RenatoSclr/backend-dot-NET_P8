namespace GpsUtil.Location;

public class Locations
{
    public double Longitude { get; }
    public double Latitude { get; }

    public Locations(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
