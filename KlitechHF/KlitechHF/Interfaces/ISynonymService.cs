using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlitechHF.Interfaces
{
    public interface ISynonymService
    {
        Task<IEnumerable<string>> GetSynonyms(string word);
    }
}
