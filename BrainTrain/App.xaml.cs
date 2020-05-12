using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace BrainTrain
{
    public partial class App : Application
    {
        static DataBase.Database database;

        public static DataBase.Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new DataBase.Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "stats.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Forms.MainPage());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
