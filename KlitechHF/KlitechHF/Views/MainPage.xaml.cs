using KlitechHF.Interfaces;
using KlitechHF.Services;
using System.Diagnostics;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using System.Collections.Generic;

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



        public void LanguageFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
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

            await ViewModel.GetTranslationAsync(languageFrom.Content.ToString(), languageTo.Content.ToString());
        }




        public async void SynonymButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(TranslateLanguageFromDropdown?.SelectedItem is ComboBoxItem languageFrom))
            {
                Debug.WriteLine("Could not get synonyms. languageFrom is null");
                return;
            }

            await ViewModel.GetSynonymsAsync(languageFrom.Content.ToString());
        }




        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.SetSupportedLanguagesAsync();  // TODO check if successful
            UpdateLanguageFrom();
            UpdateLanguageTo();
        }



    }
}
