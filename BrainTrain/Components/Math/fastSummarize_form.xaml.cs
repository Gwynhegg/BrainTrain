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
    public partial class fastSummarize_form : ContentPage
    {
        public fastSummarize_form()
        {
            InitializeComponent();
        }

        private fastSummarize new_task;

        protected override void OnAppearing()
        {
            new_task = new fastSummarize(ref content_grid);
            txt_description.Text = new_task.showDescription();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new Forms.CategoryPage();
            return true;
        }

        private void startSummarize(object sender, EventArgs e)
        {
            txt_description.IsVisible = false; ((Button)sender).IsVisible = false;
            new_task.startLevel();
        }
    }
}