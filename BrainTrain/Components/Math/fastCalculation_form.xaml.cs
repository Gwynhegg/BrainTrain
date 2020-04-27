using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Components.Math
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class fastCalculation_form : ContentPage
    {
        public fastCalculation_form()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            fastCalculating new_task = new fastCalculating(this);
        }

        private void clickNumber(object sender, EventArgs e)
        {
            txt_answer.Text += ((Button)sender).Text;
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new Forms.CategoryPage();
            return true;
        }
    }
}