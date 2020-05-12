using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Components.Attention
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class colorAttention_form : ContentPage
    {
        colorAttention new_task;
        public colorAttention_form()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            new_task = new colorAttention(ref content_grid);
            txt_description.Text = new_task.showDescription();
        }

        protected override void OnDisappearing()
        {
            new_task = null;
            GC.Collect();
        }
        protected override bool OnBackButtonPressed()
        {
            new_task.timerAnnulate();
            Application.Current.MainPage = new Forms.CategoryPage();

            return true;
        }

        private void startColor(object sender, EventArgs e)
        {
            showColorPanel();
            txt_description.IsVisible = false; start_button.IsVisible = false; current_time.IsVisible = true;
            new_task.startLevel();
        }
        private void sendColor(object sender, EventArgs e)
        {
            new_task.checkLevel(((Button)sender).BackgroundColor);
        }

        private void showColorPanel()
        {
            if (col1.IsVisible)
                for (int i = 1; i < 7; i++)
                    ((Button)FindByName("col" + i)).IsVisible = false;
            else
                for (int i = 1; i < 7; i++)
                    ((Button)FindByName("col" + i)).IsVisible = true;
        }
    }
}