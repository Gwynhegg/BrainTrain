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

        //Методы-обработчики нажатия на кнопки навигационного меню.
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

        protected override bool OnBackButtonPressed()
        {
            System.Environment.Exit(0);
            return true;
        }
    }
}