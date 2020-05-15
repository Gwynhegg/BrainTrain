using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BrainTrain.Components.Attention
{
    //Задание на цветовое внимание. Перед игроком будет показываться текст, окрашенный в определенный цвет. Его цель - указать цвет, название которого указано в текстовом сообщении.
    //Класс наследывается от абстрактного класса Exercise
    public abstract class ColorExercise : Exercise
    {
        //Установим лист ссответствий между названием цвета и цветом.
        protected List<Tuple<string, Color>> text_color;

        //Конструктор класса. С помощью ключевого слова base обращаемся к классу-предку.
        public ColorExercise(ref Grid grid) : base(ref grid)
        {
            text_color = new List<Tuple<string, Color>>();
        }

        //Метод, отвечающий за создание и добавление "цветовой" таблицы.
        public void setColorTable()
        {
            text_color.Add(new Tuple<string,Color>("Красный", Color.Red));
            text_color.Add(new Tuple<string, Color>("Зеленый", Color.Green));
            text_color.Add(new Tuple<string, Color>("Черный", Color.Black));
            text_color.Add(new Tuple<string, Color>("Синий", Color.Blue));
            text_color.Add(new Tuple<string, Color>("Желтый", Color.Yellow));
            text_color.Add(new Tuple<string, Color>("Фиолетовый", Color.Purple));
        }
    }

    //Конечный класс упражнения, который наследует добавленные методы и переменные ColorExercise и методы и переменные Exercise (исключительно в рамках интереса).
    class colorAttention : ColorExercise
    {

        //переменная, хранящая индекс правильного ответа.
        private int answer_index;

        //Конструктор класса. С помощью ключевого слова base обращаемся к классу-предку.
        public colorAttention(ref Grid grid) : base(ref grid)
        {
            //Устанавливаем описание задания для отображения на форме.
            description = "На экране будут отображаться названия цветов. Цвет надписи и название не совпадают. \n Ваша задача: правильно нажать на кнопку с цветом, совпадающим с цветом, указанным текстом.";
        }

        //Метод, отвечающий за первоначаную задачу параметров и старт уровня.
        public override void startLevel()
        {
            //В данном методе задается интервал, в течение которого будет выполняться задание.
            end_time = DateTime.Now.AddMinutes(1);
            //Создаем цветовую таблицу.
            setColorTable();
            //Запускается главный таймер и генерируется первый уровень.
            task_timer.Enabled = true;
            general_timer.Enabled = true; general_timer.Start();
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        //Метод, отвечающий за генерацию нового уровня.
        public override void generateLevel()
        {
            //Отображаем измененные компоненты формы.
            displayComponents();
            //Получаем случайный цвет из таблицы.
            answer_index = rnd.Next(0, text_color.Count);
            //Отображаем название данного цвета на форме.
            ((Label)content_grid.FindByName("txt_tasktext")).Text = text_color[answer_index].Item1;
            //Присваиваем отображенному тексту случайный цвет.
            ((Label)content_grid.FindByName("txt_tasktext")).TextColor = text_color[rnd.Next(0, text_color.Count)].Item2;
            progress = 1;
            //Запускаем уровень.
            task_timer.Enabled = true; task_timer.Start();
        }

        //Метод проверки правильности ответа. Дополнительный параметр содержит цвет, который указал пользователь.
        public void checkLevel(Color color)
        {
            //Если указанный цвет совпадает с цветом, название которого отображено на форме, то...
            if (color == text_color[answer_index].Item2)
            {
                //Прибавляем полученные очки и меняем сложность в сторону повышения.
                general_points += difficulty;
                current_mistakes = 0; 
                if (++current_positives > 2) raiseDifficulty();
            }
            else
            {
                //Понижаем сложность.
                current_positives = 0;
                if (++current_mistakes > 2) lowerDifficulty();
            }
            //Собираем мусор и генерируем новый уровень.
            GC.Collect();
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        //Метод, отвечающий за понижение сложности. Переопределение метода позволяет задать формулу изменения сложности индивидуально для каждого задания.
        public override void lowerDifficulty()
        {
            difficulty -= current_mistakes / 2;
            if (difficulty < 1) difficulty = 1;
        }
        //Метод, отвечающий за повышение сложности.
        public override void raiseDifficulty()
        {
            difficulty += current_positives;
        }

        //Поскольку данная задача имеет параметр в методе проверки уровня, метод перегружен, а его основоположник переопределен "вхолостую".
        public override void checkLevel() { return; }

    }
}
