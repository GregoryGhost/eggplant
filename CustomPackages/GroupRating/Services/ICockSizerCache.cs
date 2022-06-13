namespace GroupRating.Services
{
    using GroupRating.Models;

    public interface ICockSizerCache
    {
        IEnumerable<UserCockSize> GetCheckedUsers();
    }
}