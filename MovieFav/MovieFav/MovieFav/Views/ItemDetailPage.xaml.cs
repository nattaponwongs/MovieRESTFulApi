using MovieFav.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MovieFav.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}