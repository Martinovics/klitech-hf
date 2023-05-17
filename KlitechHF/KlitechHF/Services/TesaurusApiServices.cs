using KlitechHF.Exceptions;
using KlitechHF.Interfaces;
using KlitechHF.Models;
using KlitechHF.Secrets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;




namespace KlitechHF.Services
{
    public class TesaurusApiServices : ISynonymService
    {
        private readonly string _baseUrl = "http://thesaurus.altervista.org/thesaurus/v1";




        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        /// <exception cref="InvalidLanguageException"></exception>
        public async Task<ICollection<string>> GetSynonymsAsync(string word, string language)
        {
            language = CorrectLanguage(language);
            if (language == "")
                throw new InvalidLanguageException();


            var synonyms = new List<string>();


            Synonyms resp;
            try
            {
                resp = await GetSynonymsResponseAsync(word, language);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                return synonyms;
            }


            foreach (var r in resp.Response)
            {
                synonyms.Add(r.List.Synonyms);  // TODO: meg kell szurni a szinonimakat
            }

            return synonyms;
        }




        private string CorrectLanguage(string language)
        {
            // vagy 2 betus nyelvet varunk es akkor kijavitjuk | en --> en_US
            // vagy eleve jo

            string[] validLanguages = { "cs_CZ", "da_DK", "de_CH", "de_DE", "en_US", "el_GR", "es_ES", "fr_FR", "hu_HU", "it_IT", "no_NO", "pl_PL", "pt_PT", "ro_RO", "ru_RU", "sk_SK" };
            foreach (var lang in validLanguages)
            {
                if (lang.StartsWith(language))
                {
                    return lang;
                }
            }
            
            return "";
        }




        private async Task<Synonyms> GetSynonymsResponseAsync(string word, string language)
        {
            var endpoint = new Uri($"{_baseUrl}");
            var queryParams = new Dictionary<string, string>()
            {
                ["key"] = Secret.thesaurusApiKey,
                ["word"] = word,
                ["language"] = language,
                ["output"] = "json",
            };

            return await HttpService.GetTAsync<Synonyms>(endpoint, queryParams);
        }

    }
}
