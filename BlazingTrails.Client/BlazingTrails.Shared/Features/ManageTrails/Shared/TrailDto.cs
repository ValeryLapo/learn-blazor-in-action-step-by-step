using FluentValidation;
using static BlazingTrails.Shared.Features.ManageTrails.Shared.TrailDto;

namespace BlazingTrails.Shared.Features.ManageTrails.Shared;

public enum ImageAction
{
    None, Add, Remove
}
public class TrailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Location { get; set; } = "";
    public int TimeInMinutes { get; set; }
    public int Length { get; set; }
    public string? Image { get; set; }
    public ImageAction ImageAction { get; set; }
    public List<RouteInstruction> Route { get; set; } = new();

    public class RouteInstruction
    {
        public int Stage { get; set; }
        public string Description { get; set; } = "";
    }
}

public class TrailValidator : AbstractValidator<TrailDto>
{
    public TrailValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter a name");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter a description");
        RuleFor(x => x.Location).NotEmpty().WithMessage("Please enter a location");
        RuleFor(x => x.Length).GreaterThan(0).WithMessage("Please enter a name");
        RuleFor(x => x.Route).NotEmpty().WithMessage("Please add a route instruction");
        RuleFor(x => x.TimeInMinutes).GreaterThan(0).WithMessage("Please enter a time");
    }
}

public class RouteInstructionValidator : AbstractValidator<RouteInstruction>
{
    public RouteInstructionValidator()
    {
        RuleFor(x => x.Stage).NotEmpty().WithMessage("Please enter a stage");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter a description");
    }
}