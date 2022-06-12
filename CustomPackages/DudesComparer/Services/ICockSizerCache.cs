namespace DudesComparer.Services
{
    using DudesComparer.Models;

    public interface ICockSizerCache
    {
        CheckedDude? GetCheckedDude(string userName);

        UserCockSize? GetCheckedUser(long userId);
    }
}