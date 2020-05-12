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
    public partial class attentionSwitch_form : ContentPage
    {

        bool is_started = false;

        attentionSwitch new_task;
        public attentionSwitch_form()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            new_task = new attentionSwitch(ref content_grid);
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

        private void Decline(object sender, EventArgs e)
        {
            new_task.checkLevel(false);
        }

        private void Accept(object sender, EventArgs e)
        {
            new_task.checkLevel(true);
        }

        private void startLevel(object sender, EventArgs e)
        {
            if (!is_started)
            {
                txt_description.IsVisible = false;
                img_task.Source = IFigure.getRandomPic(img_task);
                is_started = true;
            } else
            {
                accept.IsVisible = true; decline.IsVisible = true; current_time.IsVisible = true; start.IsVisible = false; current_time.IsVisible = true;
                new_task.startLevel();
            }
        }
    }
}