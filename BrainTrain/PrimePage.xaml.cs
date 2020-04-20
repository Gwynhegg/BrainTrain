using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrimePage : TabbedPage
    {
        public PrimePage()
        {
            InitializeComponent();
             
        }


        private  async void ShowPopUpMenu(object sender, EventArgs e)
        {
            Application.Current.MainPage = new TaskPage();
        }
    }
}