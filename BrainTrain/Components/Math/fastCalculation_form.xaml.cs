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
    public partial class fastCalculation_form : ContentPage
    {
        private fastCalculating new_task;
        public fastCalculation_form()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            //При появлении формы создается новый экзэмпляр класса соответствующего задания.
            new_task = new fastCalculating(ref content_grid);
            //Скрывается цифровая панель.
            setNumPanel();
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

        //Метод, обрабатывающий нажатие на кнопку цифровой панели.
        private void clickNumber(object sender, EventArgs e)
        {
            //Нажатая цифра вводится в графу Ответ.
            txt_answer.Text += ((Button)sender).Text;
            //Если введенное значение совпадает по длине с реальным ответом, то..
            if (txt_answer.Text.Length == new_task.getAnswerLength())
            {
                //Класс проверяет введенное значение.
                new_task.checkLevel(txt_answer.Text);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            //При нажатии кнопки Назад происходит переход на форму выбора заданий.
            new_task.timerAnnulate();
            Application.Current.MainPage = new Forms.CategoryPage();
            return true;
        }
        //Метод, обрабатывающий нажатие на кнопку Начать.
        private void startMath(object sender, EventArgs e)
        {
            //Ненужные компоненты скрываются, отображается числовая панель и отображаются необходимые компоненты.
            txt_description.IsVisible = false; ((Button)start).IsVisible = false;
            setNumPanel();
            txt_line.IsVisible = true; current_time.IsVisible = true;
            //Упражнение запускает уровень.
            new_task.startLevel();
        }

        //Метод, меняющий видимость числовой панели.
        private void setNumPanel()
        {
            if (num0.IsVisible) 
                for (int i = 0; i < 10; i++) 
                    ((Button)FindByName("num" + i)).IsVisible = false; 
            else 
                for (int i = 0; i < 10; i++) 
                    ((Button)FindByName("num" + i)).IsVisible = true;
        }
    }
}