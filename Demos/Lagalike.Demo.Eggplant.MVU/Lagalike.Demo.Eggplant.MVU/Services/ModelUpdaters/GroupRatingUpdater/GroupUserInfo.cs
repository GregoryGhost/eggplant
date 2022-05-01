namespace Lagalike.Demo.Eggplant.MVU.Services.ModelUpdaters
{
    using Lagalike.Demo.Eggplant.MVU.Services.Domain;

    public record GroupUserInfo
    {
        public string FullName { get; init; } = null!;

        public CockSize CockSize { get; init; } = null!;
    }
}