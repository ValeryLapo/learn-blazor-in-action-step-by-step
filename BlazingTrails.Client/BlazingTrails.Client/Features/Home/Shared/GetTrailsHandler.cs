using System.Net.Http.Json;
using BlazingTrails.Shared.Features.Home.Shared;
using MediatR;

namespace BlazingTrails.Client.Features.Home.Shared;

public class GetTrailsHandler(HttpClient client): IRequestHandler<GetTrailsRequest, GetTrailsRequest.Response>
{
    public async Task<GetTrailsRequest.Response?> Handle(GetTrailsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await client.GetFromJsonAsync<GetTrailsRequest.Response>(GetTrailsRequest.RouteTemplate, cancellationToken: cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            return default!;
        }
    }
}