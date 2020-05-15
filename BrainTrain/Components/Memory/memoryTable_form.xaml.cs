using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Components.Memory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class memoryTable_form : ContentPage
    {

        memoryTable new_task;
        public memoryTable_form()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            //При появлении формы создается новый экзэмпляр класса соответствующего задания.
            new_task = new memoryTable(ref content_grid);
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

        //Метод, обрабатывающий нажатие на кнопку Начать.
        private void startMemTable(object sender, EventArgs e)
        {
            //При повторном нажатии скрывается ненужный интерфейс отображается необходимый, и упражнение запускает генерацию уровень и отсчет.
            txt_description.IsVisible = false; ((Button)sender).IsVisible = false; current_time.IsVisible = true;
            new_task.startLevel();
        }

        protected override bool OnBackButtonPressed()
        {
            //При нажатии кнопки Назад происходит переход на форму выбора заданий.
            new_task.timerAnnulate();
            Application.Current.MainPage = new Forms.CategoryPage();
            return true;
        }
    }
}