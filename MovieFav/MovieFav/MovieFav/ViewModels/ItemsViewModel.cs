using MovieFav.Models;
using MovieFav.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MovieFav.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public Command<Movie> ShareTapped { get; }

        private Movie _selectedItem;

        public ObservableCollection<Movie> Movies { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Movie> ItemTapped { get; }

        public Command<Movie> UpdateTapped { get; set; }
        public Command<Movie> DeleteTapped { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Movies = new ObservableCollection<Movie>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Movie>(OnOpemMovie);

            AddItemCommand = new Command(OnAddItem);
            ShareTapped = new Command<Movie>(OnShareMovie);

            UpdateTapped = new Command<Movie>(OnUpdateMovie);
            DeleteTapped = new Command<Movie>(OnDeleteMovie);
        }

        

        private async void OnShareMovie(Movie obj)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = $"{obj.Name} : {obj.URL}",
                Title = "Share Movie URL"
            });
        }

        private async void OnOpemMovie(Movie item)
        {
            await Browser.OpenAsync(item.URL);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Movies.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Movies.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Movie SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Movie item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.MovieId)}={item.MovieId}");
        }

        private async void OnUpdateMovie(Movie obj)
        {
            if (obj == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.MovieId)}={obj.MovieId}");
        }

        private async void OnDeleteMovie(Movie obj)
        {
            if (obj == null)
                return;
            bool answer =
                await Application.Current
                .MainPage.DisplayAlert("ยืนยัน > ลบเพลง ?",
                obj.Name, "ลบ", "ยกเลิก");
            if(answer)
            {
                bool isDelete = await DataStore.DeleteItemAsync(obj.MovieId.ToString());
                if (isDelete)
                    Movies.Remove(obj);
            }
        }
    }
}