using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BrainTrain.Components.Memory
{

    //Задание на память. Перед игроком будет показана таблица с подсвеченными ячейками. Задача игрока - повторить последовательность после скрытия ячеек.
    //Класс наследывается от абстрактного класса Exercise
    class memoryTable : Exercise
    {
        //Переменная, отвечающая за возможное количество ошибок.
        private int possible_mistakes=10;
        //Логическая переменная для отслеживания состояния.
        private bool game_condition = true;
        //Grid, в котором будет отображаться таблица с игровыми ячейками
        private Grid table_grid;
        //Таблица, содержащая подсвеченные и неподсвеченные ячейки в формате True/False
        private bool[,] aux_table;

        //Конструктор класса. С помощью ключевого слова base обращаемся к классу-предку.
        public memoryTable(ref Grid grid) :base(ref grid)
        {
            //Устанавливаем описание задания для отображения на форме.
            description = "На экране будет отображена таблица. Некоторые ячейки будут подсвечены на непродолжительное время. \n Ваша задача: запомнить ячейки и повторить их расположение.";
            this.task_timer.Elapsed -=base.OnTaskEvent;
            this.task_timer.Elapsed += this.OnTaskEvent;
        }

        //Метод, отвечающий за первоначаную задачу параметров и старт уровня.
        public override void startLevel()
        {
            //Отображаются главные компоненты и запускается генерация уровня.
            displayComponents();
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        //Метод, отвеачющий за генерацию уровня.
        public override void generateLevel()
        {
            createTable();
        }

        //Метод, отвечающий за генерацию нового уровня.
        public override void checkLevel()
        {
            //Создаем таблицу валидации для проверку значений.
            int marker = aux_table.GetLength(0);
            bool[,] validation_table = new bool[marker, marker];
            //Заполняем таблицу валидации - если ячейка помечена красным, то считаем ее значение за True;
            for (int i = 0; i < table_grid.Children.Count; i++) if (table_grid.Children[i].BackgroundColor == Color.Red) validation_table[i / marker, i % marker] = true;

            for (int i = 0; i < marker; i++)
            {
                //Сравниваем таблцу валидации и таблицу ответов. Правильный ответ - если закрашенные ячейки совпадают. Если происходит несовпадение - это считается за ошибку.
                for (int j = 0; j < marker; j++) if (aux_table[i, j] && validation_table[i, j]) current_positives++; else if (aux_table[i, j] || validation_table[i, j]) current_mistakes++;
            }
            //Если количество правильных ответов преобладает над количеством отрицательным - повышаем количество очков и повышаем сложность.
            if (current_positives >= current_mistakes)
            {
                raiseDifficulty();
                general_points += (current_positives - current_mistakes);
        }
            //Иначе понижаем сложность
        else lowerDifficulty();
            //Если количество ошибок превысило количество допустимых - то заканчиваем игру.
            if (current_mistakes >= possible_mistakes) Device.BeginInvokeOnMainThread(() => Application.Current.MainPage = new Forms.ResultPage(this.GetType().ToString(), general_points));
            //Отображаем компоненты, собираем мусор и генерируем новый уровень.
            displayComponents();
            content_grid.Children.RemoveAt(content_grid.Children.Count-1);
            GC.Collect();
            generateLevel();
        }

        //Отображение главных компонентов.
        new public void displayComponents()
        {
            ((Label)content_grid.FindByName("txt_mistakes")).Text = "Осталось попыток: \n" + (possible_mistakes - current_mistakes);
            ((Label)content_grid.FindByName("txt_difficulty")).Text = "Сложность: \n" + difficulty.ToString();
            ((Label)content_grid.FindByName("txt_points")).Text = "Очки: \n" + general_points.ToString();
        }

        //Метод, отвечающий за понижение сложности. Переопределение метода позволяет задать формулу изменения сложности индивидуально для каждого задания.
        public override void lowerDifficulty()
        {
            difficulty = difficulty + current_positives / (current_mistakes + 1);
        }

        //Метод, отвечающий за повышение сложности.
        public override void raiseDifficulty()
        {
           difficulty = difficulty + (current_positives-current_mistakes)/10;
        }

        //Метод создания и отображения таблицы.
        private void createTable()
        {
            //Задание базового размера таблицы и создание необходимых компонентов.
            int size = difficulty+3;
            table_grid = new Grid();
            aux_table = new bool[size, size];
            List<Tuple<int, int>> answers = new List<Tuple<int, int>>();
            Tuple<int, int> tuple;
            //УСтанавливаем количество закрашенных ячеек с помощью формулы.
            int answers_count = rnd.Next(size * size / 2 - size, size * (size - 1));
            int temp = 0;
            //Генерируем пары координат, которые будут закрашены в нашей таблице.
            while (temp!=answers_count)
            {
                tuple = (rnd.Next(0, size), rnd.Next(0, size)).ToTuple<int,int>();
                if (!answers.Contains(tuple)) 
                { 
                    answers.Add(tuple);
                    aux_table[tuple.Item1, tuple.Item2] = true;
                    temp++;
                }
            }

            //Заполняем таблицу.
            for (int i = 0; i < size; i++)
            {
                table_grid.RowDefinitions.Add(new RowDefinition());
                table_grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            //Добавляем компоненты Button в каждую ячейку.
            Button new_button;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    //Меняем цвет ячеек, указанных в ответах. Добавляем обработчик событий для каждой кнопки и добавляем в таблицу.
                    if (answers.Contains((i,j).ToTuple<int,int>())) new_button = new Button {BackgroundColor = Color.Red}; else new_button = new Button { BackgroundColor = Color.White };
                    new_button.Clicked += changeColor;
                    table_grid.Children.Add(new_button, j, i);
                }
            }

            //Добавляем компонент на форму и запускаем уровень.
            content_grid.Children.Add(table_grid, 0, 1);
            Grid.SetColumnSpan(table_grid, 3); Grid.SetRowSpan(table_grid, 2);
            GC.Collect();
            table_grid.IsEnabled = false;
            task_timer.Enabled = true;
            progress = 1;
            task_timer.Start();
        }

        //Переопределение события тика таймера уровня.
       new private void OnTaskEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            progress -= (double)(difficulty) / 1000;
            if (progress < 0)
            {
                //Если игроку была показана таблица с ответами и таймер вышел, то...
                if (game_condition)
                {
                    task_timer.Enabled = false;
                    //очищаем таблицу и делаем ее доступной. Перезапускаем таймер.
                    clearBoard();
                    game_condition = false;
                    table_grid.IsEnabled = true;
                    progress = 1;
                    task_timer.Enabled = true;
                    task_timer.Start();
                }
                else
                {
                    task_timer.Enabled = false;
                    //Иначе запускаем процесс проверки уровня.
                    game_condition = true;
                    table_grid.IsEnabled = false;
                    progress = 1;
                    task_timer.Enabled = true;
                    Device.BeginInvokeOnMainThread(() =>  checkLevel());
                }
            }
            Device.BeginInvokeOnMainThread(() => ((ProgressBar)content_grid.FindByName("current_time")).Progress = progress);
        }

        //Обработчик нажатия на кнопку, содержащуюся в таблице.
        private void changeColor(object sender, EventArgs e)
        {
            //При нажатии кнопка меняет свой цвет на красный и наоборот.
            if (((Button)sender).BackgroundColor == Color.White) ((Button)sender).BackgroundColor = Color.Red; else ((Button)sender).BackgroundColor = Color.White;
        }

        //Метод, отвечающий за очищение таблицы.
        private void clearBoard()
        {
            foreach (object e in table_grid.Children) ((Button)e).BackgroundColor = Color.White;
        }

    }
}
