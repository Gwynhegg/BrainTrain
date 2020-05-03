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
        private fastCalculating new_task;
        public fastCalculation_form()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            new_task = new fastCalculating(ref content_grid);
            setNumPanel();
            txt_description.Text = new_task.showDescription();
        }

        private void clickNumber(object sender, EventArgs e)
        {
            txt_answer.Text += ((Button)sender).Text;
            if (txt_answer.Text.Length == new_task.getAnswerLength())
            {
                new_task.checkLevel(txt_answer.Text);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new Forms.CategoryPage();
            return true;
        }

        private void startMath(object sender, EventArgs e)
        {
            txt_description.IsVisible = false; ((Button)start).IsVisible = false;
            setNumPanel();
            txt_line.IsVisible = true;
            new_task.startLevel();
        }

        private void setNumPanel()
        {
            if (num0.IsVisible) 
                for (int i = 0; i < 10; i++) 
                    ((Button)FindByName("num" + i)).IsVisible = false; 
            else 
                for (int i = 0; i < 10; i++) 
                    ((Button)FindByName("num" + i)).IsVisible = true;
        }
    }
}