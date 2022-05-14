using MovieFav.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MovieFav.ViewModels
{
    [QueryProperty(nameof(MovieId), nameof(MovieId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private int movieId;
        private string movieCode;
        private string url;
        private string name;
        private string description;
        private Movie movieEdit;

        public Command<Movie> SaveCommand { get; }
        public Command CancelCommand { get; }

        public ItemDetailViewModel()
        {
            SaveCommand = new Command<Movie>(OnSave);
            CancelCommand = new Command(OnCancel);
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave(Movie obj)
        {
            Movie newItem = new Movie()
            {
                MovieId = obj.MovieId,
                MovieCode = obj.MovieCode,
                Name = Name,
                Description = Description,
                Type = obj.Type,
                URL = obj.URL
            };
            await DataStore.UpdateItemAsync(newItem);
            await Shell.Current.GoToAsync("..");
        }
        public Movie MovieEdit
        {
            get => movieEdit;
            set => SetProperty(ref movieEdit, value);
        }
        public string MovieCode
        {
            get => movieCode;
            set => SetProperty(ref movieCode, value);
        }

        public string URL
        {
            get => url;
            set => SetProperty(ref url, value);
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

        public int MovieId
        {
            get
            {
                return movieId;
            }
            set
            {
                movieId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(int itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId.ToString());
                //MovieId = item.MovieId;
                movieCode = item.MovieCode;
                Name = item.Name;
                Description = item.Description;
                URL = item.URL;
                MovieEdit = item;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
