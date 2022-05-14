using MovieFav.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MovieFav.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string movieCode;
        private string name;
        private string description;
        private string type;


        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(movieCode)
                && !String.IsNullOrWhiteSpace(Name)
                && !String.IsNullOrWhiteSpace(description)
                && !String.IsNullOrWhiteSpace(type);
        }

        public string MovieCode
        {
            get => movieCode;
            set => SetProperty(ref movieCode, value);
        }
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public string Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Movie newItem = new Movie()
            {
                MovieCode = this.MovieCode,
                Name = this.Name,
                Description = this.Description,
                Type = this.Type,
                URL = $"https://www.youtube.com/watch?v=" + MovieCode
            };
            await DataStore.AddItemAsync(newItem);
            await Shell.Current.GoToAsync("..");
        }
    }
}
