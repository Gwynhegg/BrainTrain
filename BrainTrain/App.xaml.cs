using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace BrainTrain
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Forms.MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
