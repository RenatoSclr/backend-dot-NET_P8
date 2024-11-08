using GpsUtil.Location;
using Microsoft.AspNetCore.Mvc;
using TourGuide.Services.Interfaces;
using TourGuide.Users;
using TripPricer;

namespace TourGuide.Controllers;

[ApiController]
[Route("[controller]")]
public class TourGuideController : ControllerBase
{
    private readonly ITourGuideService _tourGuideService;

    public TourGuideController(ITourGuideService tourGuideService)
    {
        _tourGuideService = tourGuideService;
    }

    [HttpGet("getLocation")]
    public async Task<ActionResult<VisitedLocation>> GetLocation([FromQuery] string userName)
    {
        var location = await _tourGuideService.GetUserLocation(GetUser(userName));
        return Ok(location);
    }

    [HttpGet("getNearbyAttractions")]
    public async Task<ActionResult<List<Attraction>>> GetNearbyAttractions([FromQuery] string userName)
    {
        var visitedLocation = await _tourGuideService.GetUserLocation(GetUser(userName));
        var attractions = await _tourGuideService.GetNearByAttractions(visitedLocation);
        return Ok(attractions);
    }

    [HttpGet("getRewards")]
    public ActionResult<List<UserReward>> GetRewards([FromQuery] string userName)
    {
        var rewards = _tourGuideService.GetUserRewards(GetUser(userName));
        return Ok(rewards);
    }

    [HttpGet("getTripDeals")]
    public ActionResult<List<Provider>> GetTripDeals([FromQuery] string userName)
    {
        var deals = _tourGuideService.GetTripDeals(GetUser(userName));
        return Ok(deals);
    }

    private User GetUser(string userName)
    {
        return _tourGuideService.GetUser(userName);
    }
}
