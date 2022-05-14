using MovieFav.Models;
using MovieFav.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieFav.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }

        private void pickerX_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtType.Text = pickerX.SelectedItem.ToString();
        }
    }
}