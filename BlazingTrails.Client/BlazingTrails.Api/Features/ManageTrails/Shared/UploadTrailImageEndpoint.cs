﻿using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared.Features.ManageTrails.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BlazingTrails.Api.Features.ManageTrails.Shared
{
    public class UploadTrailImageEndpoint : BaseAsyncEndpoint.WithRequest<int>.WithResponse<string>
    {
        private readonly BlazingTrailsContext _database;
        public UploadTrailImageEndpoint(BlazingTrailsContext database)
        {
            _database = database;
        }

        [HttpPost(UploadTrailImageRequest.RouteTemplate)]
        public override async Task<ActionResult<string>> HandleAsync(int trailId, CancellationToken cancellationToken = new CancellationToken())
        {
            var trail = await _database.Trails.SingleOrDefaultAsync(x => x.Id == trailId, cancellationToken);

            if (trail is null)
            {
                return BadRequest("Trail does not exist");
            }

            var file = Request.Form.Files[0];
            if (file.Length == 0)
            {
                return BadRequest("No image found");
            }

            var fileName = $"{Guid.NewGuid()}.jpg";
            var saveLocation = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

            var resizeOptions = new ResizeOptions
            {
                Mode = ResizeMode.Pad,
                Size = new Size(640, 426)
            };

            using var image = Image.Load(file.OpenReadStream());
            image.Mutate(x => x.Resize(resizeOptions));

            await image.SaveAsJpegAsync(saveLocation, cancellationToken);

            if (!string.IsNullOrWhiteSpace(trail.Image))
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Images", trail.Image));
            }
            trail.Image = fileName;
            await _database.SaveChangesAsync(cancellationToken);

            return Ok(trail.Image);
        }
    }
}
