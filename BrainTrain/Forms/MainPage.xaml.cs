using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                Components.User user = await App.Database.GetUserAsync();
            }
            catch
            {
            //    await App.Database.InsertUserAsync(new Components.User());
            }
        }

        private async void sendToPrime(object sender, EventArgs e)
        {
            try
            {
             Components.User user =  await App.Database.GetUserAsync();
                user.ID = 12;
               Console.WriteLine(user.ID);
            }
            catch
            {
                Console.WriteLine("you");
            }
            Application.Current.MainPage = new InterfacePage();
        }
    }
}