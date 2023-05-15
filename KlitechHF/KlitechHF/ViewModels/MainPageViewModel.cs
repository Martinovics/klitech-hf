using KlitechHF.Interfaces;
using KlitechHF.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KlitechHF.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _output = "";
        private ITranslateService _translateService;
        private ISynonymService _synonymService;
        private List<string> _supportedLanguages = new List<string>();
        public event PropertyChangedEventHandler PropertyChanged;



        public MainPageViewModel()
        {
            // cserelheto az implementacio
            var yandexService = new YandexApiService();
            _translateService = yandexService;
            _synonymService = yandexService;
        }


        public string TextToTranslate { get; set; }

        public string Output
        {
            get { return _output; }
            set
            {
                if (_output != value)
                {
                    _output = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Output)));
                }
            }
        }


        public async Task SetSupportedLanguagesAsync()
        {
            var languages = await _translateService.GetSupportedLanguages();
            foreach (var lang in languages)
            {
                if (lang.Count(c => c == '-') == 1)
                {
                    _supportedLanguages.Add(lang);
                }
            }
            Debug.WriteLine($"Set supported languages. Language-pair count={_supportedLanguages.Count}");
        }


        public HashSet<string> GetSupportedLanguageFrom()
        {
            var languages = new HashSet<string>();
            foreach (var lang in _supportedLanguages)
            {
                languages.Add(lang.Split("-")[0]);
            }
            Debug.WriteLine($"GetSupportedLanguageFrom");
            return languages; //.OrderBy(x => x).ToHashSet();
        }


        public HashSet<string> GetSupportedLanguageTo(string languageTo)
        {
            var languages = new HashSet<string>();
            foreach (var lang in _supportedLanguages)
            {
                var s = lang.Split("-");
                if (s[0] == languageTo)
                {
                    languages.Add(s[1]);
                }
            }
            Debug.WriteLine($"GetSupportedLanguageTo");
            return languages;
        }



        public async Task Translate(string languageFrom, string languageTo)
        {
            Debug.WriteLine($"languageFrom={languageFrom} languageTo={languageTo} TextToTranslate={TextToTranslate}");
            string output = "";
            var translated = await _translateService.Translate(TextToTranslate, languageFrom, languageTo);
            foreach (var tr in translated)
            {
                output += tr + " ";
            }
            Output = output;
        }

    }
}
