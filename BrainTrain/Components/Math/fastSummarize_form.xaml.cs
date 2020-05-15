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
    public partial class fastSummarize_form : ContentPage
    {
        public fastSummarize_form()
        {
            InitializeComponent();
        }

        private fastSummarize new_task;

        protected override void OnAppearing()
        {
            //При появлении формы создается новый экзэмпляр класса соответствующего задания.
            new_task = new fastSummarize(ref content_grid);
            //Отображается описание задания.
            txt_description.Text = new_task.showDescription();
        }

        protected override void OnDisappearing()
        {
            //При сворачивании формы весь мусор собирается.
            new_task.timerAnnulate();
            new_task = null;
            GC.Collect();
        }
        protected override bool OnBackButtonPressed()
        {
            //При нажатии кнопки Назад происходит переход на форму выбора заданий.
            new_task.timerAnnulate();
            Application.Current.MainPage = new Forms.CategoryPage();
            return true;
        }

        //Метод, обрабатывающий нажатие на кнопку Начать
        private void startSummarize(object sender, EventArgs e)
        {
            //Скрываем все ненужные компоненты интерфейса и запускает экземпляр упражнения.
            txt_description.IsVisible = false; ((Button)sender).IsVisible = false;
            new_task.startLevel();
        }
    }
}