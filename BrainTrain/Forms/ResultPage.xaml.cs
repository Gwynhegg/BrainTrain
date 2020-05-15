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
    public partial class ResultPage : ContentPage
    {
        //Здесь происходит вывод полученных очков на экран, а также запись данных в конкретную таблицу БД.
        public ResultPage(string type, int gen_points)
        {
            InitializeComponent();
            switch (type)
            {
                case "BrainTrain.Components.Math.fastCalculating": App.Database.SaveNoteAsync(new Components.fastCalculating() {points = gen_points, date = DateTime.Now}); points.Text = "Очки:" + gen_points.ToString(); break;
                case "BrainTrain.Components.Math.fastSummarize": App.Database.SaveNoteAsync(new Components.fastSummarize() { points = gen_points, date = DateTime.Now }); points.Text = "Очки:" + gen_points.ToString(); break;
                case "BrainTrain.Components.Attention.attentionSwitch": App.Database.SaveNoteAsync(new Components.attentionSwitch() { points = gen_points, date = DateTime.Now }); points.Text = "Очки:" + gen_points.ToString(); break;
                case "BrainTrain.Components.Attention.colorAttention": App.Database.SaveNoteAsync(new Components.colorAttention() { points = gen_points, date = DateTime.Now }); points.Text = "Очки:" + gen_points.ToString(); break;
                case "BrainTrain.Components.Memory.memoryTable": App.Database.SaveNoteAsync(new Components.memoryTable() { points = gen_points, date = DateTime.Now }); points.Text = "Очки:" + gen_points.ToString(); break;
                case "BrainTrain.Components.Memory.memoryText": App.Database.SaveNoteAsync(new Components.memoryText() { points = gen_points, date = DateTime.Now }); points.Text = "Очки:" + gen_points.ToString(); break;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new CategoryPage();
            return true;
        }
    }
}