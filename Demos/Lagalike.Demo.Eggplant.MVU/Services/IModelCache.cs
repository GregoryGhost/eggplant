namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using Lagalike.Demo.Eggplant.MVU.Models;

    public interface IModelCache : IModelCache<Model>,
        GroupRating.Services.ICockSizerCache,
        DudesComparer.Services.ICockSizerCache
    {
    }
}