using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Components.Memory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class memoryTable_form : ContentPage
    {

        memoryTable new_task;
        public memoryTable_form()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            new_task = new memoryTable(ref content_grid);
            txt_description.Text = new_task.showDescription();
        }

        protected override void OnDisappearing()
        {
            new_task = null;
            GC.Collect();
        }
        private void startMemTable(object sender, EventArgs e)
        {
            txt_description.IsVisible = false; ((Button)sender).IsVisible = false; current_time.IsVisible = true;
            new_task.startLevel();
        }

        protected override bool OnBackButtonPressed()
        {
            new_task.timerAnnulate();
            Application.Current.MainPage = new Forms.CategoryPage();
            return true;
        }
    }
}