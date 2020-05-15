using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BrainTrain.Components.Math
{

    //Задание на быстрое суммирование. Игрок должен выбирать числа, которые в сумме дают числа, указанные выше.
    //Класс наследывается от абстрактного класса Exercise
    public class fastSummarize : Exercise
    {
        //Переменные, отвечающие за числа, появляющиеся в строке сверху.
        private int first_number, second_number, answer_count=0;
        //массив, содержащий числа, из которых планируется составлять строку ответов.
        int[,] aux_table;
        //массив, содержащий ответы текущего уровня.
        int[] answers;
        //Сделать будет храниться ссылка на компонент, в котором будет отображаться таблица.
        private Grid table_grid;

        //Конструктор класса. С помощью ключевого слова base обращаемся к классу-предку.
        public fastSummarize(ref Grid grid) :base(ref grid)
        {
            //Устанавливаем описание задания для отображения на форме.
            description = "На экране будет представлена таблица с числами. \n Ваша задача: найти те числа, которые в сумме дают указанные сверху числа."; 
        }

        //Метод, отвечающий за первоначаную задачу параметров и старт уровня.
        public override void startLevel()
        {
            //В данном методе задается интервал, в течение которого будет выполняться задание.
            end_time = DateTime.Now.AddMinutes(1);
            //Запускается главный таймер и генерируется первый уровень
            general_timer.Enabled = true;  general_timer.Start();
            displayComponents();
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        //Определение абстрактного метода проверки уровня.
        public override void checkLevel()
        {
            //Если цифры, нажатые на клавиатуре, совпадает с текущим ответом, то...
            if (first_number + second_number == answers[answer_count])
            {
                //Прибавляем очки и меняем сложность в сторону повышения.
                general_points += difficulty;
                current_positives++;
            }
            //Меняем сложность в обратную сторону.
            else current_mistakes++;

            //Если количество текущих овтетов равно их общему количеству, то...
            if (++answer_count == answers.Length)
            {
                //Если правильных ответов было больше, чем негативных, то повышаем сложность, иначе понижаем.
                if (current_positives >= current_mistakes) raiseDifficulty(); else lowerDifficulty();
                //Обнуляем все переменные.
                current_positives = 0; current_mistakes = 0; answer_count = 0;
                content_grid.Children.RemoveAt(content_grid.Children.Count-1);
                //Собираем мусор и генерируем новый уровень.
                GC.Collect();
                Device.BeginInvokeOnMainThread(() => generateLevel());

            }
            else 
            {
                //Иначе удаляем уже сформированную пару и убираем первое число из списка с числами.
                ((Label)content_grid.FindByName("curr_number")).Text = ((Label)content_grid.FindByName("curr_number")).Text.Remove(0, ((Label)content_grid.FindByName("curr_number")).Text.IndexOf(" ")+1); 
            }
            first_number = 0; second_number = 0;
            displayComponents();
        }

        //Метод, отвечающий за генерацию нового уровня.
        public override void generateLevel()
        {
            //Создаем таблицу и ответы к ней. Отображаем все на форме.
            createTable();
            createAnswer(difficulty / 5 + 5);
            ((Label)content_grid.FindByName("curr_number")).Text = String.Join(" ", answers);
        }

        //Метод, отвечающий за понижение сложности. Переопределение метода позволяет задать формулу изменения сложности индивидуально для каждого задания.
        public override void lowerDifficulty()
        {
            difficulty = difficulty * current_positives / (current_mistakes + 1) + 1;
        }

        //Метод, отвечающий за повышение сложности.
        public override void raiseDifficulty()
        {
             difficulty = difficulty + (int)(current_positives / (current_mistakes + 1));
        }

        //Метод, отвечающий за создание таблицы с числами.
        private void createTable()
        {
            int size = difficulty / 3 + 3;
            table_grid = new Grid();
            rnd = new Random();
            aux_table = new int[size, size];
            //Числа будут отображаться в программно созданном Grid.
            for (int i = 0; i < size; i++)
            {
                table_grid.RowDefinitions.Add(new RowDefinition());
                table_grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            Button new_button;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    //Заполняем таблицу со значениями случайными числами, создаем кнопки с тем же текстом, и помещаем в Grid.
                    aux_table[i, j] = rnd.Next(1, difficulty * 5);
                    new_button = new Button { Text = aux_table[i,j].ToString()};
                    //Добавляем обработчик нажатия на кнопки и добавляем в Grid.
                    new_button.Clicked += addValue;
                    table_grid.Children.Add(new_button,j,i);
                }
            }
            //Отображаем и расстягиваем Grid.
            content_grid.Children.Add(table_grid, 0, 2);
            Grid.SetColumnSpan(table_grid, 3);
        }

        //Метод, обрабатывающий нажатие на любую из кнопок в созданном Grid.
        private void addValue(object sender, EventArgs e)
        {
            //Заполняем свободные переменные значениями, указанными в кнопках.
            if (first_number == 0) first_number = Int32.Parse(((Button)sender).Text); else 
            { 
                second_number = Int32.Parse(((Button)sender).Text);
                //Если заполнены обе переменные, то проверяем значение на корректность.
                checkLevel();
            }
        }

        //Метод, отвечающий за генерацию строки ответа. Берутся случайные ячейки таблицы с числами и суммируются.
        private void createAnswer(int count)
        {
            answers = new int[count];
            for (int i = 0; i < count; i++) answers[i] = aux_table[rnd.Next(0, aux_table.GetLength(0)), rnd.Next(0, aux_table.GetLength(0))] + aux_table[rnd.Next(0, aux_table.GetLength(0)), rnd.Next(0, aux_table.GetLength(0))];
        }

    }
}
