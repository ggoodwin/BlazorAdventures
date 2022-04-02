using Shared.Settings;

namespace Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        //Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}
