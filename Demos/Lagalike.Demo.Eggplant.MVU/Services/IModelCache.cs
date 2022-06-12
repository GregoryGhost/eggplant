namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using Lagalike.Demo.Eggplant.MVU.Models;

    using PatrickStar.MVU;

    public interface IModelCache: IModelCache<Model>, GroupRating.Services.ICockSizerCache, DudesComparer.Services.ICockSizerCache
    {
    }
}