using KlitechHF.Interfaces;
using KlitechHF.Services;
using System.Diagnostics;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;

namespace KlitechHF.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

            // app title
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = false;
            ApplicationView.GetForCurrentView().Title = "Szotar";
        }




        private async void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            var yandexService = new YandexApiService();
            ITranslateService translateService = yandexService;
            ISynonymService synonymService = yandexService;



            var translations = await translateService.Translate("economy", "en", "de");
            foreach (var translation in translations)
            {
                Debug.Write(translation + " ");
            }

            Debug.WriteLine("");

            var synonyms = await synonymService.GetSynonyms("Wirtschaft");
            foreach (var syn in synonyms)
            {
                Debug.Write(syn + " ");
            }
        }



        private void UpdateLanguageFrom()
        {
            TranslateLanguageFromDropdown.Items.Clear();
            var languages = ViewModel.GetSupportedLanguageFrom();
            foreach (var lang in languages)
            {
                TranslateLanguageFromDropdown.Items.Add(new ComboBoxItem
                {
                    Content = lang,
                });
            }

            if (0 < TranslateLanguageFromDropdown.Items.Count)
            {
                TranslateLanguageFromDropdown.SelectedItem = TranslateLanguageFromDropdown.Items[0];
            }
        }



        private void UpdateLanguageTo(string languageFrom = "en")
        {
            TranslateLanguageToDropdown.Items.Clear();
            var languages = ViewModel.GetSupportedLanguageTo(languageFrom);
            foreach (var lang in languages)
            {
                TranslateLanguageToDropdown.Items.Add(new ComboBoxItem
                {
                    Content = lang,
                });
            }
            if (0 < TranslateLanguageToDropdown.Items.Count)
            {
                TranslateLanguageToDropdown.SelectedItem = TranslateLanguageToDropdown.Items[0];
            }
        }



        public void LanguageFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox?.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                string selectedLanguageFrom = selectedItem.Content.ToString();
                UpdateLanguageTo(selectedLanguageFrom);
            }
        }



        public async void TranslateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(TranslateLanguageFromDropdown?.SelectedItem is ComboBoxItem languageFrom))
            {
                Debug.WriteLine("Could not translate. languageFrom is null");
                return;
            }
            if (!(TranslateLanguageToDropdown?.SelectedItem is ComboBoxItem languageTo))
            {
                Debug.WriteLine("Could not translate. languageTo is null");
                return;
            }

            await ViewModel.Translate(languageFrom.Content.ToString(), languageTo.Content.ToString());
        }




        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.SetSupportedLanguagesAsync();  // TODO check if successful
            UpdateLanguageFrom();
            UpdateLanguageTo();
        }



    }
}
