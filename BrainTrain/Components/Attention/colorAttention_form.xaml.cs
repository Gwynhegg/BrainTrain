using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Components.Attention
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class colorAttention_form : ContentPage
    {
        colorAttention new_task;
        public colorAttention_form()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            //При появлении формы создается новый экзэмпляр класса соответствующего задания.
            new_task = new colorAttention(ref content_grid);
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

        //Метод, вызываемый при нажание на кнопку Начать.
        private void startColor(object sender, EventArgs e)
        {
            //Отображение цветовой панели.
            showColorPanel();
            //Скрытие ненужных элементов формы.
            txt_description.IsVisible = false; start_button.IsVisible = false; current_time.IsVisible = true;
            //Запуск текущего задания.
            new_task.startLevel();
        }

        //Метод, обрабатывающий нажатие на любую кнопку цветовой панели.
        private void sendColor(object sender, EventArgs e)
        {
            //При нажатии отсылает цвет нажатой кнопки в соответствующий метод класса.
            new_task.checkLevel(((Button)sender).BackgroundColor);
        }

        //Метод, отвечающий за отображение цветовой панели (которые представляют собой набор кнопок).
        private void showColorPanel()
        {
            if (col1.IsVisible)
                for (int i = 1; i < 7; i++)
                    ((Button)FindByName("col" + i)).IsVisible = false;
            else
                for (int i = 1; i < 7; i++)
                    ((Button)FindByName("col" + i)).IsVisible = true;
        }
    }
}