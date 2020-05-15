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
    public partial class attentionSwitch_form : ContentPage
    {
        //Переменная, показывающая, запущен ли уровень.
        bool is_started = false;

        attentionSwitch new_task;
        public attentionSwitch_form()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            //При появлении формы создается новый экзэмпляр класса соответствующего задания.
            new_task = new attentionSwitch(ref content_grid);
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

        //Метод, отвечающий за обработку нажатия на кнопку Нет. Указывает на то, что данная фигура не совпадает с предыдущей.
        private void Decline(object sender, EventArgs e)
        {
            //Проверяем ответ, выраженный в логическом значении.
            new_task.checkLevel(false);
        }

        //Метод, отвечающий за обработку нажатия на кнопку Нет. Указывает на то, что данная фигура совпадает с предыдущей.
        private void Accept(object sender, EventArgs e)
        {
            new_task.checkLevel(true);
        }

        //Метод, обрабатывающий нажатие на кнопку Начать.
        private void startLevel(object sender, EventArgs e)
        {
            //При первом нажатии скрывается определение, показывается первая фигура, переменная is_sterted меняет значение для повторного нажатия.
            if (!is_started)
            {
                txt_description.IsVisible = false;
                img_task.Source = IFigure.getRandomPic(img_task);
                is_started = true;
            } else
            {
                //При повторном нажатии отображается интерфейс и упражнение запускает генерацию уровень и отсчет.
                accept.IsVisible = true; decline.IsVisible = true; current_time.IsVisible = true; start.IsVisible = false; current_time.IsVisible = true;
                new_task.startLevel();
            }
        }
    }
}