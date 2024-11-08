using GpsUtil.Location;
using System.Collections.Concurrent;
using TourGuide.LibrairiesWrappers.Interfaces;
using TourGuide.Services.Interfaces;
using TourGuide.Users;

namespace TourGuide.Services;

public class RewardsService : IRewardsService
{
    private const double StatuteMilesPerNauticalMile = 1.15077945;
    private readonly int _defaultProximityBuffer = 10;
    private int _proximityBuffer;
    private readonly int _attractionProximityRange = 200;
    private readonly IGpsUtil _gpsUtil;
    private readonly IRewardCentral _rewardsCentral;

    public RewardsService(IGpsUtil gpsUtil, IRewardCentral rewardCentral)
    {
        _gpsUtil = gpsUtil;
        _rewardsCentral =rewardCentral;
        _proximityBuffer = _defaultProximityBuffer;
    }

    public void SetProximityBuffer(int proximityBuffer)
    {
        _proximityBuffer = proximityBuffer;
    }

    public void SetDefaultProximityBuffer()
    {
        _proximityBuffer = _defaultProximityBuffer;
    }

    public async Task CalculateRewards(User user)
    {
        List<Attraction> attractions = _gpsUtil.GetAttractions();

        var rewardedAttractions = new HashSet<string>(user.UserRewards.Select(r => r.Attraction.AttractionName));
        var rewardsToAdd = new ConcurrentBag<UserReward>();
        List<Task> tasks = new List<Task>();

        foreach (var attraction in attractions)
        {
            tasks.Add(Task.Run(() =>
            {
                if (!rewardedAttractions.Contains(attraction.AttractionName))
                {
                    var visitedLocation = user.VisitedLocations
                        .FirstOrDefault(location => NearAttraction(location, attraction));

                    if (visitedLocation != null)
                    {
                        var rewardPoints = GetRewardPoints(attraction, user);
                        rewardsToAdd.Add(new UserReward(visitedLocation, attraction, rewardPoints));
                    }
                }
            }));

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine($"Handled exception: {innerException.Message}");
                }
            }

            foreach (var reward in rewardsToAdd)
            {
                user.AddUserReward(reward);
            }
        }
    }

    public bool IsWithinAttractionProximity(Attraction attraction, Locations location)
    {
        Console.WriteLine(GetDistance(attraction, location));
        return GetDistance(attraction, location) <= _attractionProximityRange;
    }

    private bool NearAttraction(VisitedLocation visitedLocation, Attraction attraction)
    {
        return GetDistance(attraction, visitedLocation.Location) <= _proximityBuffer;
    }

    private int GetRewardPoints(Attraction attraction, User user)
    {
        return _rewardsCentral.GetAttractionRewardPoints(attraction.AttractionId, user.UserId);
    }

    public double GetDistance(Locations loc1, Locations loc2)
    {
        double lat1 = Math.PI * loc1.Latitude / 180.0;
        double lon1 = Math.PI * loc1.Longitude / 180.0;
        double lat2 = Math.PI * loc2.Latitude / 180.0;
        double lon2 = Math.PI * loc2.Longitude / 180.0;

        double angle = Math.Acos(Math.Sin(lat1) * Math.Sin(lat2)
                                + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lon1 - lon2));

        double nauticalMiles = 60.0 * angle * 180.0 / Math.PI;
        return StatuteMilesPerNauticalMile * nauticalMiles;
    }
}
