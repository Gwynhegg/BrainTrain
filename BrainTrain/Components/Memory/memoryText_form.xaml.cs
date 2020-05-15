    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Components.Memory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class memoryText_form : ContentPage
    {
        //Переменная, показывающая, запущен ли уровень.
        bool is_started = false;
        memoryText new_task;
        public memoryText_form()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            //При появлении формы создается новый экзэмпляр класса соответствующего задания.
            new_task = new memoryText(ref content_grid);
            //Отображается описание задания.
            txt_description.Text = new_task.showDescription();
            //Создаем новый экземпляр SfListView для вывода ключей с возможностью перетаскивания и реордеринга компонентов.
            SfListView keys = new SfListView() {BackgroundColor=Color.White};
            keys.DragStartMode = DragStartMode.OnHold | DragStartMode.OnDragIndicator;
            content_grid.Children.Add(keys, 0, 1);
            Grid.SetColumnSpan(keys, 3);
            keys.IsVisible = false;
        }

        protected override void OnDisappearing()
        {
            //При сворачивании формы весь мусор собирается.
            new_task.timerAnnulate();
            new_task = null;
            GC.Collect();
        }

        //Обработчик нажатия на кнопку Начать.
        private void submitTask(object sender, EventArgs e)
        {
            //Если программа только запущена, то начинаем упражнение и выводим текст, полученный из файла. При повторном нажатии происходит проверка значений.
            if (!is_started) { new_task.startLevel(); txt_description.IsVisible = false; is_started = true;  } else new_task.Submit();
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