using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : TabbedPage
    {
        public CategoryPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new InterfacePage();
            return true;
        }

        private void ToFastMath(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Components.Math.fastCalculation_form();
        }

        private void ToFastSum(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Components.Math.fastSummarize_form();
        }
    }
    }
