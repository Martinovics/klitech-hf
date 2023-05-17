using System.Collections.Generic;
using System.Threading.Tasks;




namespace KlitechHF.Interfaces
{
    public interface ITranslateService
    {
        Task<ICollection<string>> GetSupportedLanguagePairsAsync();
        Task<ICollection<string>> GetTranslationAsync(string word, string fromLanguage, string toLanguage);
    }
}
