namespace GroupRating.Services
{
    using System.Collections.Generic;

    using GroupRating.Models;

    public interface ICockSizerCache
    {
        IEnumerable<UserCockSize> GetCheckedUsers();
    }
}