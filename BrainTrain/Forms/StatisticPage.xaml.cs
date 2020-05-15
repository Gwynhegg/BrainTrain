using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.SfChart.XForms;
namespace BrainTrain.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticPage : ContentPage
    {

        List<Components.ExerciseResults> local_data;
        int maximal_value=0;
        public StatisticPage(List<Components.ExerciseResults> task)
        {
            if (task.Count == 0)
            {
                Label lbl = new Label() { Text = "Вы не завершили ни одного задания. Время начать!" , VerticalOptions=LayoutOptions.CenterAndExpand, HorizontalOptions=LayoutOptions.CenterAndExpand, TextColor=Color.White };
                InitializeComponent();
                month.IsVisible = false; week.IsVisible = false; all_time.IsVisible = false;
                content_grid.Children.Add(lbl, 0, 1);
                Grid.SetColumnSpan(lbl, 3);
                Button btn = new Button();
                btn.Clicked +=  (sender, args) =>  Application.Current.MainPage = new CategoryPage();
                content_grid.Children.Add(btn, 0, 2);
                Grid.SetColumnSpan(btn, 3);
            }
            else
            {
                InitializeComponent();
                createTable(task,7);
                max_value.Text = "Максимальное значение: " + maximal_value.ToString();
                createGraph(task,7);
                local_data = task;
            }
        }

        //Метод, отвечающий за создание таблицы с результатами пользователя и отображении ее на дисплее.
        private void createTable(List<Components.ExerciseResults> task, int days)
        {
            data_table.RowDefinitions.Add(new RowDefinition());
            data_table.Children.Add(new Label() { Text = "Дата", HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.White }, 0, 0);
            data_table.Children.Add(new Label() { Text = "Очки", HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.White }, 0, 1);
            task.Reverse();
            for (int i = 0; i < Math.Min(task.Count, days); i++)
            {
                data_table.ColumnDefinitions.Add(new ColumnDefinition());
                data_table.Children.Add(new Label() { Text = task[i].date.ToShortDateString(), HorizontalOptions=LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, TextColor=Color.White }, i+1, 0);
                data_table.Children.Add(new Label() { Text = task[i].points.ToString(), HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.White }, i+1, 1);
                setMaximal(task[i].points);
            }
        }

        //Метод, отвечающий за отрисовку графиков успехов пользователя.
        private void createGraph(List<Components.ExerciseResults> task, int days)
        {
            //Используем сторонную библиотеку SyncFusion для отрисовки кастомных компонентов.
            //Устанавливаем значения осей и данные, являющиеся объектами отрисовки графика.
            SfChart chart = new SfChart();
            CategoryAxis primary_axis = new CategoryAxis();
            chart.PrimaryAxis = primary_axis;
            NumericalAxis secondary_axis = new NumericalAxis();
            chart.SecondaryAxis = secondary_axis;
            LineSeries series = new LineSeries();
            task.Reverse();
            series.ItemsSource = task.Take(Math.Min(task.Count, days));
            series.XBindingPath = "date";
            series.YBindingPath = "points";
            chart.Series.Add(series);
            content_grid.Children.Add(chart, 0, 2);
            Grid.SetColumnSpan(chart, 3);

        }
        private void setMaximal(int value)
        {
            if (value > maximal_value) maximal_value = value;
        }

        protected override bool OnBackButtonPressed()
        {               
            Application.Current.MainPage = new UserPage();
            return true;
        }


       //Выбор диапозона, из которого происходит выбора результатов.
        private void Weekly(object sender, EventArgs e)
        {
            data_table = new Grid();
            content_grid.Children.RemoveAt(content_grid.Children.Count - 1);
            createTable(local_data, 10);
            createGraph(local_data,10);

        }

        private void Monthly(object sender, EventArgs e)
        {
            data_table = new Grid();
            content_grid.Children.RemoveAt(content_grid.Children.Count - 1);
            createTable(local_data, 30);
            createGraph(local_data, 30);

        }

        private void allTime(object sender, EventArgs e)
        {
            data_table = new Grid();
            content_grid.Children.RemoveAt(content_grid.Children.Count - 1);
            createTable(local_data, 999999);
            createGraph(local_data, 999999);
        }
    }
}