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
    public class YandexApiService : ITranslateService, ISynonymService
    {
        private readonly string baseUrl = "https://dictionary.yandex.net/api/v1/dicservice.json";

        private string _lastLookedUpWord = "";
        private YandexApiLookupModel _lastLookedUpWordModel = new YandexApiLookupModel();


        public async Task<IEnumerable<string>> GetSupportedLanguages()
        {
            var endpoint = new Uri($"{baseUrl}/getLangs");
            var query = new Dictionary<string, string>()
            {
                ["key"] = Secret.yandexApiKey,
            };

            var resp = await HttpService.GetTAsync<List<string>>(endpoint, query);
            // todo handle error

            return resp;
        }


        public async Task<IEnumerable<string>> GetSynonyms(string word)
        {
            var synonyms = new List<string>();

            if (_lastLookedUpWord == "")
            {
                Debug.WriteLine($"Nem sikerult lekerni a szinonimakat: {word}");
                return synonyms;
            }


            foreach (var def in _lastLookedUpWordModel.Def)
            {
                foreach (var tr in def.Tr)
                {
                    if (tr.Syn != null && tr.Text == word)
                    {
                        foreach (var syn in tr.Syn)
                        {
                            synonyms.Add(syn.Text);
                        }
                    }
                }
            }

            return synonyms;
        }

        
        public async Task<IEnumerable<string>> Translate(string word, string fromLanguage, string toLanguage)
        {
            var translations = new List<string>();

            bool success = await Lookup(word, fromLanguage, toLanguage);
            if (!success)
            {
                Debug.WriteLine($"Nem sikerult leforditani: {word}");
                return translations;
            }


            foreach (var def in _lastLookedUpWordModel.Def)
            {
                foreach (var tr in def.Tr)
                {
                    translations.Add(tr.Text);  // TODO azt is hozza lehetne venni, hogy mit jelent
                }
            }

            return translations;
        }


        private async Task<bool> Lookup(string word, string fromLanguage, string toLanguage)
        {
            if (word == _lastLookedUpWord)  // mar megneztuk es eltaroltuk ezt a szot legutobb
                return true;

            var endpoint = new Uri($"{baseUrl}/lookup");
            var query = new Dictionary<string, string>()
            {
                ["key"] = Secret.yandexApiKey,
                ["lang"] = $"{fromLanguage}-{toLanguage}",
                ["text"] = word,
            };


            YandexApiLookupModel lookupModel;
            try
            {
                lookupModel = await HttpService.GetTAsync<YandexApiLookupModel>(endpoint, query);
            }
            catch (Exception ex)  // TODO: specifikusabb exception
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            
            _lastLookedUpWord = word;
            _lastLookedUpWordModel = lookupModel;
            return true;
        }
    }
}
