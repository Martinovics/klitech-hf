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
    public class YandexApiService : ITranslateService
    {
        private readonly string baseUrl = "https://dictionary.yandex.net/api/v1/dicservice.json";




        /// <summary>
        /// Gets the available translation language pairs (en-de, de-es ...)
        /// </summary>
        /// <returns>Collection of strings</returns>
        public async Task<ICollection<string>> GetSupportedLanguagePairsAsync()
        {
            var endpoint = new Uri($"{baseUrl}/getLangs");
            var query = new Dictionary<string, string>()
            {
                ["key"] = Secret.yandexApiKey,
            };

            var supportedLanguagePairs = new List<string>();  // en-de, en-es...
            try
            {
                supportedLanguagePairs = await HttpService.GetTAsync<List<string>>(endpoint, query);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return supportedLanguagePairs;
        }


        

        /// <summary>
        /// Gets the translations for the given word
        /// </summary>
        /// <param name="word"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <returns>Collection of strings (translated words)</returns>
        public async Task<ICollection<string>> GetTranslationAsync(string word, string fromLanguage, string toLanguage)
        {
            var translations = new List<string>();


            YandexApiLookupModel resp;
            try
            {
                resp = await GetTranslationResponseAsync(word, fromLanguage, toLanguage);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                return translations;
            }


            foreach (var def in resp.Def)
            {
                foreach (var tr in def.Tr)
                {
                    translations.Add(tr.Text);  // TODO azt is hozza lehetne venni, hogy mit jelent
                }
            }

            return translations;
        }




        /// <summary>
        /// Helper method for sending the translation request
        /// </summary>
        /// <param name="word"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <returns>T</returns>
        private async Task<YandexApiLookupModel> GetTranslationResponseAsync(string word, string fromLanguage, string toLanguage)
        {
            var endpoint = new Uri($"{baseUrl}/lookup");
            var query = new Dictionary<string, string>()
            {
                ["key"] = Secret.yandexApiKey,
                ["lang"] = $"{fromLanguage}-{toLanguage}",
                ["text"] = word,
            };
            
            return await HttpService.GetTAsync<YandexApiLookupModel>(endpoint, query); ;
        }
    }
}
