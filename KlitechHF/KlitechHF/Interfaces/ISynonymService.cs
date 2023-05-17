using System.Collections.Generic;
using System.Threading.Tasks;




namespace KlitechHF.Interfaces
{
    public interface ISynonymService
    {
        Task<ICollection<string>> GetSynonymsAsync(string word, string language);
    }
}
