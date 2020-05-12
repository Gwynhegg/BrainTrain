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
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new InterfacePage();
            return true;
        }

        protected override void OnAppearing()
        {

        }

        private async void toSum(object sender, EventArgs e)
        {
            List < Components.fastSummarize> temp = await App.Database.getSum();
            Application.Current.MainPage = new StatisticPage(temp.ToList<Components.ExerciseResults>());
        }

        private async void toArifm(object sender, EventArgs e)
        {
            List<Components.fastCalculating> temp = await App.Database.getCalc();
            Application.Current.MainPage = new StatisticPage(temp.ToList<Components.ExerciseResults>());
        }

        private async void toMem(object sender, EventArgs e)
        {
            List<Components.memoryTable> temp = await App.Database.getMem();
            Application.Current.MainPage = new StatisticPage(temp.ToList<Components.ExerciseResults>());
        }

        private async void toText(object sender, EventArgs e)
        {
            List<Components.memoryText> temp = await App.Database.getText();
            Application.Current.MainPage = new StatisticPage(temp.ToList<Components.ExerciseResults>());
        }

        private async void toColor(object sender, EventArgs e)
        {
            List<Components.colorAttention> temp = await App.Database.getColor();
            Application.Current.MainPage = new StatisticPage(temp.ToList<Components.ExerciseResults>());
        }

        private async void toAtt(object sender, EventArgs e)
        {
            List<Components.attentionSwitch> temp = await App.Database.getAtt();
            Application.Current.MainPage = new StatisticPage(temp.ToList<Components.ExerciseResults>());
        }
    }
}