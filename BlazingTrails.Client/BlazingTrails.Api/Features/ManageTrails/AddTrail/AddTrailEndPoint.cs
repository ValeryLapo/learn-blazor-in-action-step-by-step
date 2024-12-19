using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Api.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using Microsoft.AspNetCore.Mvc;

namespace BlazingTrails.Api.Features.ManageTrails.AddTrail
{
    public class AddTrailEndPoint : BaseAsyncEndpoint.WithRequest<AddTrailRequest>.WithResponse<int>
    {
        private readonly BlazingTrailsContext _database;
        public AddTrailEndPoint(BlazingTrailsContext database)
        {
            _database = database;
        }

        [HttpPost(AddTrailRequest.RouteTemplate)]
        public override async Task<ActionResult<int>> HandleAsync(AddTrailRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var trail = new Trail
            {
                Name = request.Trail.Name,
                Description = request.Trail.Description,
                Location = request.Trail.Location,
                TimeInMinutes = request.Trail.TimeInMinutes,
                Length = request.Trail.Length
            };
            await _database.Trails.AddAsync(trail, cancellationToken);

            var routeInstructions = request.Trail.Waypoints
                .Select(wp => new Waypoint
                {
                    Latitude = wp.Latitude,
                    Longitude = wp.Longitude,
                    Trail = trail
                });

            await _database.Waypoints.AddRangeAsync(routeInstructions, cancellationToken);
            await _database.SaveChangesAsync(cancellationToken);

            return Ok(trail.Id);
        }
    }
}
