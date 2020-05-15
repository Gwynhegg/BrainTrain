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
    public partial class TrainerPage : ContentPage
    {
        List<List<Components.ExerciseResults>> res;
        public TrainerPage()
        {
            res = new List<List<Components.ExerciseResults>>();
            fillList(res);
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new InterfacePage();
            return true;
        }

        private void toLowest(object sender, EventArgs e)
        {
            goToForm(getLowest(res));
        }

        private void toAverage(object sender, EventArgs e)
        {
            goToForm(getAverage(res));
        }

        private void toDate(object sender, EventArgs e)
        {
            goToForm(getDate(res));
        }

        private void createTrainer(object sender, EventArgs e)
        {
            to_average.IsVisible = true; to_lowest.IsVisible = true; to_date.IsVisible = true; lbl_average.IsVisible = true;
            create_trainer.IsVisible = false;
            lbl_low.Text = "Улучшите показатели!"; lbl_average.Text = "Не останавливаться!"; lbl_date.Text = "Давно не виделись!";
        }
        private async void fillList(List<List<Components.ExerciseResults>> res)
        {
            List<Components.fastSummarize> sum = await App.Database.getSum(); List<Components.fastCalculating> calc = await App.Database.getCalc(); List<Components.memoryTable> tab = await App.Database.getMem();
            List<Components.memoryText> text = await App.Database.getText(); List<Components.colorAttention> col = await App.Database.getColor(); List<Components.attentionSwitch> att = await App.Database.getAtt();
            res.Add(sum.ToList<Components.ExerciseResults>()); res.Add(calc.ToList<Components.ExerciseResults>()); res.Add(tab.ToList<Components.ExerciseResults>());
            res.Add(text.ToList<Components.ExerciseResults>()); res.Add(col.ToList<Components.ExerciseResults>()); res.Add(att.ToList<Components.ExerciseResults>());
        }

        private int getDate(List<List<Components.ExerciseResults>> res)
        {
            DateTime[] date = new DateTime[res.Count];
            DateTime curr_time = DateTime.Now;
            int iter=0;
            for (int i = 0; i < res.Count; i++)
            {
                try
                {
                    if (res[i].Last().date < curr_time) { curr_time = res[i].Last().date; iter = i; }
                }
                catch
                {
                    iter = i;
                }
            }
            return iter;
        }

        private int getAverage(List<List<Components.ExerciseResults>> res)
        {
            double[] growth = new double[res.Count];
            for (int i = 0; i < res.Count; i++)
            {
                double sum_of_temps = 0;
                for (int j = 1; j < res[i].Count; j++) sum_of_temps += (double)res[i][j].points / res[i][j - 1].points;
                growth[i] = sum_of_temps / res[i].Count;
            }
            return Array.IndexOf(growth, growth.Min());
        }
        private int getLowest(List<List<Components.ExerciseResults>> res)
        {
            double[] averages = new double[res.Count];
            Type[] types = new Type[res.Count];
            for (int i = 0; i < res.Count; i++)
            {
                int sum = 0;
                foreach (Components.ExerciseResults e in res[i]) sum += e.points;
                if (res[i].Count != 0) averages[i] = (double)(sum) / res[i].Count; else averages[i] = 0;
            }

            return Array.IndexOf(averages, averages.Min());
        }


        private void goToForm(int num)
        {
            switch (num)
            {
                case 0: Application.Current.MainPage = new Components.Math.fastSummarize_form(); break;
                case 1: Application.Current.MainPage = new Components.Math.fastCalculation_form(); break;
                case 2: Application.Current.MainPage = new Components.Memory.memoryTable_form(); break;
                case 3: Application.Current.MainPage = new Components.Memory.memoryText_form(); break;
                case 4: Application.Current.MainPage = new Components.Attention.colorAttention_form(); break;
                case 5: Application.Current.MainPage = new Components.Attention.attentionSwitch_form(); break;
            }
        }
    }
}