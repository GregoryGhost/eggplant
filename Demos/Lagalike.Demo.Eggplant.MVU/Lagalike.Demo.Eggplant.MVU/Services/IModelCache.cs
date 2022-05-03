namespace Lagalike.Demo.Eggplant.MVU.Services
{
    using GroupRating.Services;

    using Lagalike.Demo.Eggplant.MVU.Models;

    using PatrickStar.MVU;

    public interface IModelCache: IModelCache<Model>, ICockSizerCache
    {
        
    }
}