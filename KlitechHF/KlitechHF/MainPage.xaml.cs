using KlitechHF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;




namespace KlitechHF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            PagePerson = new Person() { Name = "John Wick", Age = 44 };

            People = new ObservableCollection<Person>()
            {
             new Person() { Name = "Peter Griffin", Age = 40 },
             new Person() { Name = "Homer Simpson", Age = 42 },
            };

            DataContext = this;
        }

        public Person PagePerson { get; set; }
        
        public ObservableCollection<Person> People { get; set; }


        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            var person = new Person() { Name = PagePerson.Name, Age = PagePerson.Age };
            People.Add(person);
        }


        private void DecreaseButton_Click(object Sender, RoutedEventArgs e)
        {
            PagePerson.Age -= 1;
        }


        private void IncreaseButton_Click(object Sender, RoutedEventArgs e)
        {
            PagePerson.Age += 1;
        }


        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var person = (Person)e.ClickedItem;
            new MessageDialog($"Name={person.Name} Age={person.Age}").ShowAsync();
        }

    }
}
