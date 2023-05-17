using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlitechHF.Interfaces
{
    public interface ISynonymService
    {
        Task<ICollection<string>> GetSynonymsAsync(string word, string language);
    }
}
