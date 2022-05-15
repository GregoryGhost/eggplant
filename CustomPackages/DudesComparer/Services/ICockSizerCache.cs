namespace DudesComparer.Services
{
    using System.Collections.Generic;

    using DudesComparer.Models;

    public interface ICockSizerCache
    {
        UserCockSize? GetCheckedUser(long userId);
    }
}