using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private System.Timers.Timer general_timer;
        DateTime end_time;
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            general_timer = new System.Timers.Timer();
            general_timer.Enabled = true;
            general_timer.Interval = 100;
            end_time = DateTime.Now.AddSeconds(3);
            general_timer.Elapsed+= new System.Timers.ElapsedEventHandler(OnTimedEvent);
            general_timer.Start();
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan cur = end_time - e.SignalTime;
            if (cur.TotalMilliseconds <= 0)
            {
                general_timer.Enabled = false;
                Device.BeginInvokeOnMainThread(async() => Application.Current.MainPage = new Forms.InterfacePage());
            }
        }
        private void sendToPrime(object sender, EventArgs e)
        {

            Application.Current.MainPage = new InterfacePage();
        }
    }
}