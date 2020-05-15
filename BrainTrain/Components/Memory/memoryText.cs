using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.IO;
using Android.Content.Res;
using System.Reflection;
using System.Linq;
using Syncfusion.ListView.XForms;

namespace BrainTrain.Components.Memory
{
    //Вспомогательный статический класс, служащий для обработки текстовой информации.
    public static class ITextHandler
    {

        //Метод, загружающий текстовый ресурс из ассетов приложения.
        public static string getTextFromFile()
        {
            Random rnd = new Random();
            
            //С помощью менеджера ассетов считываем случайный текстовый файл из папки Assets.
            AssetManager asset = Android.App.Application.Context.Assets;
            return new StreamReader(asset.Open("CommonFiles/Texts/TextFile"+rnd.Next(1,asset.List("CommonFiles/Texts").Length)+".txt")).ReadToEnd();
        }

        //Метод, избирающий часть, содержащую текст для запоминания.
        public static string selectTextPart(string all_text)
        {
            return all_text.Substring(0, all_text.IndexOf("<mark>"));
        }

        //Метод, избирающий ключи для запоминания, находящиеся в тексте после тега <mark>
        public static List<string> getKeys(string all_text, int diff)
        {
            Random rnd = new Random();
            string[] temp = all_text.Substring(all_text.IndexOf("<mark>") + 8).Split('\n');
            List<string> ret_keys = new List<string>();
            int marker = rnd.Next(0, System.Math.Min(temp.Length, rnd.Next(2,diff+2)));
            for (int i = marker; i < temp.Length; i++) ret_keys.Add(temp[i]);
            return ret_keys;
        }
    }

    //Задание на запоминание текстов. Перед игроком будет показан текст. Его задача - запомнить его и воссоздать события, ориентируясь по кодовым словам.
    //Класс наследывается от абстрактного класса Exercise
    class memoryText : Exercise
    {
        //Создаем два списка для хранения ключей и таблицы валидации.
        List<string> keys;
        List<string> validate_table;
        //Логическая переменная, отвечающая за индикатор текущей фазы.
        bool phase = false;

        //Конструктор класса. С помощью ключевого слова base обращаемся к классу-предку.
        public memoryText(ref Grid grid) : base(ref grid)
        {
            //Устанавливаем описание задания для отображения на форме.
            description = "На экране будет отображен текст. Постарайтесь запомнить как можно больше. Как будете готовы, нажмите Далее и восстановите структуру.";
        }

        //Метод, отвечающий за первоначаную задачу параметров и старт уровня.
        public override void startLevel()
        {
            //На данное задание выделяется две минуты вместо одной.
            end_time = DateTime.Now.AddMinutes(2);
            //Запускается главный таймер и генерируется первый уровень.
            general_timer.Enabled = true; general_timer.Start();
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        //Метод проверки правильности ответа. 
        public override void checkLevel()
        {
            //Если значения ключей совпадают с соответсвующими значениями таблицы валидации - то ответ корректен. Иначе - нет.
            for (int i = 0; i < keys.Count; i++) if (keys[i] == validate_table[i]) current_positives++; else current_mistakes++;
            //Если количество правильных ответов больше, чем неправильных, то повышаем сложность упражнения. В противном случае - понижаем.
            if (current_positives >= current_mistakes) raiseDifficulty(); else lowerDifficulty();
            general_points += current_positives;
            //Обнуляем значения и генерируем новый уровень.
            current_positives = 0;current_mistakes = 0;
            GC.Collect();
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        //Метод, отвечающий за генерацию нового уровня.
        public override void generateLevel()
        {
            //Отображаем интерфейс, получаем необходимые данные - текст, ключи. Выводим необходимые компоненты на форму.
            displayComponents();
            keys = new List<string>(); validate_table = new List<string>();
            string all_text = ITextHandler.getTextFromFile();
            string text_part = ITextHandler.selectTextPart(all_text);
            keys = ITextHandler.getKeys(all_text, difficulty);
            ((Label)content_grid.FindByName("txt_text")).Text = text_part;
        }

        //Метод, отвечающий за понижение сложности. Переопределение метода позволяет задать формулу изменения сложности индивидуально для каждого задания.
        public override void lowerDifficulty()
        {
            difficulty--;
            if (difficulty < 1) difficulty = 1;
        }

        //Метод, отвечающий за повышение сложности.
        public override void raiseDifficulty()
        {
            difficulty++;
        }

        //Событие, возникающее при нажатии кнопки на главной странице пользовательского интерфейса после показа текста.
        public void Submit()
        {
            if (!phase)
            {
                //Скрываем ненужные компоненты.
                ((Label)content_grid.FindByName("txt_text")).IsVisible = false;
                //Устанавливаем кастомный компонент.
                ((SfListView)content_grid.Children[content_grid.Children.Count-1]).IsVisible = true;
                //Составляем таблицу валидации, представляющую собой перемешанный список ключей.
                validate_table = keys.OrderBy(x => rnd.Next()).ToList();
                //Добавляем событие0обработчик на перетаскивание предмета кастомного компонента.
                ((SfListView)content_grid.Children[content_grid.Children.Count - 1]).ItemDragging += MemoryText_ItemDragging;
                //Указываем источник значений компонента.
                ((SfListView)content_grid.Children[content_grid.Children.Count - 1]).ItemsSource = validate_table;
                Console.WriteLine("s");
                phase = true;
            } else
            {
                //Скрываем ненужные компоненты интерфейса и проверяем результаты.
                phase = false;
                ((SfListView)content_grid.Children[content_grid.Children.Count - 1]).IsVisible = false;
                ((Label)content_grid.FindByName("txt_text")).IsVisible = true;
                checkLevel();
            }
        }

        //Обработчик перетаскивания. При перетаскивании заменяем две строки друг с другом.
        private void MemoryText_ItemDragging(object sender, ItemDraggingEventArgs e)
        {
            if (e.Action == DragAction.Drop)
            {
                var temp = validate_table[e.OldIndex];
                validate_table.RemoveAt(e.OldIndex);
                validate_table.Insert(e.NewIndex, temp);
            }
        }
    }
}
