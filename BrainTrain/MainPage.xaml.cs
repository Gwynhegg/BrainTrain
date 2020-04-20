using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BrainTrain
{

    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private bool is_authorized = false;
        public MainPage()
        {
            InitializeComponent();
            if (!is_authorized) to_prime.IsVisible = true;
    }

       private async void SendToPrime(object sender, EventArgs e)
        {
            if (!is_authorized)
            {
                ((Button)sender).Text = "Перейти в задания";
                is_authorized = true;
            }
            else
            {
                await Navigation.PushModalAsync(new PrimePage());
            }
        }

        

    }
}
