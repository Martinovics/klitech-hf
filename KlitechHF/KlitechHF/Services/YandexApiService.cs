using KlitechHF.Interfaces;
using KlitechHF.Models;
using KlitechHF.Secrets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Media.Casting;

namespace KlitechHF.Services
{
    public class YandexApiService : ITranslateService
    {
        private readonly string baseUrl = "https://dictionary.yandex.net/api/v1/dicservice.json";




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
