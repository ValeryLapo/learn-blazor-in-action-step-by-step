using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared.Features.Home.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlazingTrails.Api.Features.Home;

public class GetTrailsEndpoint: BaseAsyncEndpoint.WithRequest<int>.WithResponse<GetTrailsRequest.Response>
{
    private readonly BlazingTrailsContext _database;
    public GetTrailsEndpoint(BlazingTrailsContext database)
    {
        _database = database;
    }
    [HttpGet(GetTrailsRequest.RouteTemplate)]
    public override async Task<ActionResult<GetTrailsRequest.Response>> HandleAsync(int trailId, CancellationToken cancellationToken = new CancellationToken())
    {
        var trails = await _database.Trails
            .Include(t => t.Waypoints)
            .ToListAsync(cancellationToken);

        var response = new GetTrailsRequest.Response(trails.Select(t => new GetTrailsRequest.Trail(
            t.Id,
            t.Name,
            t.Image,
            t.Location,
            t.TimeInMinutes,
            t.Length,
            t.Description,
            t.Owner,
            t.Waypoints.Select(wp => new GetTrailsRequest.Waypoint(wp.Latitude, wp.Longitude)).ToList())));

        return Ok(response);
    }
}