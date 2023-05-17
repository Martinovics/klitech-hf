using KlitechHF.Exceptions;
using KlitechHF.Interfaces;
using KlitechHF.Models;
using KlitechHF.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Globalization;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KlitechHF.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _output = "";
        private string _outputTitle = "Translations and synonyms will be shown here.";
        private string _outputMessage = "";
        private ITranslateService _translateService;
        private ISynonymService _synonymService;
        public event PropertyChangedEventHandler PropertyChanged;




        public MainPageViewModel()
        {
            // cserelheto az implementacio
            _translateService = new YandexApiService();
            _synonymService = new TesaurusApiServices();
        }




        public Dictionary<string, List<string>> SupportedLanguages { get; private set; } = new Dictionary<string, List<string>>();
        public string InputWord { get; set; }
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

        public string OutputTitle
        {
            get { return _outputTitle; } 
            set
            {
                if (value != _outputTitle)
                {
                    _outputTitle = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputTitle)));
                }
            } 
        }

        public string OutputMessage
        {
            get { return _outputMessage; }
            set
            {
                if (value != _outputMessage)
                {
                    _outputMessage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputMessage)));
                }
            }
        }




        public async Task SetSupportedLanguagesAsync()
        {
            var languages = await _translateService.GetSupportedLanguagePairsAsync();
            foreach (var langPair in languages)
            {
                if (langPair.Count(c => c == '-') == 1)
                {
                    string[] splitted = langPair.Split('-');
                    string languageFrom = splitted[0];
                    string languageTo = splitted[1];
                    if (!SupportedLanguages.ContainsKey(languageFrom))
                    {
                        SupportedLanguages[languageFrom] = new List<string>() { languageTo };
                    }
                    else
                    {
                        if (!SupportedLanguages[languageFrom].Contains(languageTo))
                        {
                            SupportedLanguages[languageFrom].Add(languageTo);
                        }
                    }
                }
            }
            Debug.WriteLine($"Set supported languages. LanguageFrom count={SupportedLanguages.Count}");
        }




        public async Task GetTranslationAsync(string languageFrom, string languageTo)
        {
            if (!isWordValid())
                return;


            int c = 1;
            var output = new List<string>();
            var translated = await _translateService.GetTranslationAsync(InputWord, languageFrom, languageTo);
            foreach (var tr in translated)
            {
                output.Add($"{c++}. {tr}");
            }
            
            OutputTitle = $"Translations for '{InputWord}':";
            OutputMessage = string.Join("\n", output);
        }




        public async Task GetSynonymsAsync(string languageFrom)
        {
            if (!isWordValid())
                return;


            ICollection<string> synonyms;
            try
            {
                synonyms = await _synonymService.GetSynonymsAsync(InputWord, languageFrom);
            }
            catch (InvalidLanguageException ex)
            {
                OutputTitle = $"Can't get synonyms from language '{languageFrom}'";
                OutputMessage = "";
                Debug.WriteLine(ex.Message);
                return;
            }


            int c = 1;
            var output = new List<string>();
            foreach (var s in synonyms)
            {
                output.Add($"{c++}. {s}");
            }
            
            OutputTitle = $"Synonyms for '{InputWord}':";
            OutputMessage = string.Join("\n", output);
        }


        private bool isWordValid()
        {
            if (string.IsNullOrEmpty(InputWord))
            {
                OutputTitle = "Please, provide a word!";
                OutputMessage = "";
                return false;
            }

            return true;
        }

    }
}
