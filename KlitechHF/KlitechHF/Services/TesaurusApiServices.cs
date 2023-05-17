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
        /// Gets the synonyms for a given word in a given language
        /// </summary>
        /// <param name="word"></param>
        /// <param name="language"></param>
        /// <returns>Collection of strings</returns>
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




        /// <summary>
        /// Corrects the input language to a valid form which the api can understand (en -> en_US, de -> de_DE ...)
        /// </summary>
        /// <param name="language"></param>
        /// <returns>Corrected language string</returns>
        private string CorrectLanguage(string language)
        {
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




        /// <summary>
        /// Helper method for sending the request
        /// </summary>
        /// <param name="word"></param>
        /// <param name="language"></param>
        /// <returns>T</returns>
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
