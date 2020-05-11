using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Components.Memory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class memoryText_form : ContentPage
    {
        bool is_started = false;
        memoryText new_task;
        public memoryText_form()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            new_task = new memoryText(ref content_grid);
            txt_description.Text = new_task.showDescription();
            SfListView keys = new SfListView();
            keys.DragStartMode = DragStartMode.OnHold | DragStartMode.OnDragIndicator;
            content_grid.Children.Add(keys, 0, 1);
            Grid.SetColumnSpan(keys, 3);
            keys.IsVisible = false;
        }

        private void submitTask(object sender, EventArgs e)
        {
            if (!is_started) { new_task.startLevel(); txt_description.IsVisible = false; is_started = true;  } else new_task.Submit();
        }


        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new Forms.CategoryPage();
            return true;
        }
    }
}