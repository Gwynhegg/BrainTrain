using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InterfacePage : ContentPage
    {
        public InterfacePage()
        {
            InitializeComponent();
        }

        private void sendToTasks(object sender, EventArgs e)
        {
            Application.Current.MainPage = new CategoryPage();
        }

        private void sendToUser(object sender, EventArgs e)
        {
            Application.Current.MainPage = new UserPage();
        }

        private void sendToTrainer(object sender, EventArgs e)
        {
            Application.Current.MainPage = new TrainerPage();
        }

        private void sendToStats(object sender, EventArgs e)
        {
           // Application.Current.MainPage = new StatsPage();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new MainPage();
            return true;
        }
    }
}