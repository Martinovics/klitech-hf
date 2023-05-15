using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlitechHF.Interfaces
{
    public interface ITranslateService
    {
        Task<IEnumerable<string>> GetSupportedLanguages();
        Task<IEnumerable<string>> Translate(string word, string fromLanguage, string toLanguage);
    }
}
