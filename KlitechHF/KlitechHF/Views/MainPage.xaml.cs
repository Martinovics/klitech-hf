using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace KlitechHF.Views
{
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




        /// <summary>
        /// Updates the 'languages from' UI dropdown
        /// </summary>
        private void UpdateLanguageFrom()
        {
            TranslateLanguageFromDropdown.Items.Clear();
            foreach (var lang in ViewModel.SupportedLanguages.Keys)
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




        /// <summary>
        /// Updates the 'languages to' UI dropdown
        /// </summary>
        /// <param name="languageFrom"></param>
        private void UpdateLanguageTo(string languageFrom = "en")
        {
            TranslateLanguageToDropdown.Items.Clear();
            
            try
            {

            }
            catch (KeyNotFoundException)
            {
                Debug.WriteLine($"Could not update language to (invalid language from: {languageFrom})");
                return;
            }

            foreach (var lang in ViewModel.SupportedLanguages[languageFrom])
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




        /// <summary>
        /// 'Language from' dropdown click listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LanguageFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedLanguageFrom = selectedItem.Content.ToString();
                UpdateLanguageTo(selectedLanguageFrom);
            }
        }




        /// <summary>
        /// Gets the translations for the input word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            await ViewModel.GetTranslationAsync(languageFrom.Content.ToString(), languageTo.Content.ToString());
        }




        /// <summary>
        /// Gets the synonyms for the given word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void SynonymButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(TranslateLanguageFromDropdown?.SelectedItem is ComboBoxItem languageFrom))
            {
                Debug.WriteLine("Could not get synonyms. languageFrom is null");
                return;
            }

            await ViewModel.GetSynonymsAsync(languageFrom.Content.ToString());
        }




        /// <summary>
        /// Fires when the view is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            int maxTries = 10;
            for (int i = 1; i <= maxTries; i++)
            {
                if (await ViewModel.SetSupportedLanguagesAsync())
                    break;
                Debug.WriteLine($"Could not get supported languages. Try {i}/{maxTries}");
                Thread.Sleep(10000);
            }

            UpdateLanguageFrom();
            UpdateLanguageTo();
        }
    }
}
