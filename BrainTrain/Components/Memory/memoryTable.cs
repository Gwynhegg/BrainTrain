using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BrainTrain.Components.Memory
{
    class memoryTable : Exercise
    {
        private double progress = 1;
        private int possible_mistakes=10;
        private bool game_condition = true;
        private Grid table_grid;
        private bool[,] aux_table;
        public memoryTable(ref Grid grid) :base(ref grid)
        {
            description = "На экране будет отображена таблица. Некоторые ячейки будут подсвечены на непродолжительное время. \n Ваша задача: запомнить ячейки и повторить их расположение.";
        }
        public override void startLevel()
        {
            task_timer.Interval = 10;
            task_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTaskEvent);
            displayComponents();
            generateLevel();
        }

        public override void generateLevel()
        {
            createTable();
        }

        public override void checkLevel()
        {
            int marker = aux_table.GetLength(0);
            bool[,] validation_table = new bool[marker, marker];
            for (int i = 0; i < table_grid.Children.Count; i++) if (table_grid.Children[i].BackgroundColor == Color.Red) validation_table[i / marker, i % marker] = true;

            for (int i = 0; i < marker; i++)
            {
                for (int j = 0; j < marker; j++) if (aux_table[i, j] && validation_table[i, j]) current_positives++; else if (aux_table[i, j] || validation_table[i, j]) current_mistakes++;
            }
            if (current_positives >= current_mistakes)
            {
                raiseDifficulty();
                general_points += (current_positives - current_mistakes);
        }
        else lowerDifficulty();
            if (current_mistakes>=possible_mistakes) Application.Current.MainPage = new Forms.ResultPage();
            displayComponents();
            content_grid.Children.RemoveAt(content_grid.Children.Count-1);
            generateLevel();
        }

        public override void displayComponents()
        {
            ((Label)content_grid.FindByName("txt_mistakes")).Text = "Осталось попыток: \n" + (possible_mistakes - current_mistakes);
            ((Label)content_grid.FindByName("txt_difficulty")).Text = "Сложность: \n" + difficulty.ToString();
            ((Label)content_grid.FindByName("txt_points")).Text = "Очки: \n" + general_points.ToString();
        }

        public override void lowerDifficulty()
        {
            //ДОДУМОТЬ
            //difficulty = difficulty * current_positives / (current_mistakes + 1);
        }

        public override void raiseDifficulty()
        {
            //ДОДУМОТЬ
           // difficulty = difficulty * current_positives / (current_mistakes + 1);
        }

        private void createTable()
        {
            //ДОУДМОТЬ
            int size = 4;
            table_grid = new Grid();
            aux_table = new bool[size, size];
            List<Tuple<int, int>> answers = new List<Tuple<int, int>>();
            Tuple<int, int> tuple;
            int answers_count = rnd.Next(size * size / 2 - size, size * (size - 1));
            int temp = 0;
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
                    if (answers.Contains((i,j).ToTuple<int,int>())) new_button = new Button {BackgroundColor = Color.Red}; else new_button = new Button { BackgroundColor = Color.White };
                    new_button.Clicked += changeColor;
                    table_grid.Children.Add(new_button, j, i);
                }
            }

            content_grid.Children.Add(table_grid, 0, 1);
            Grid.SetColumnSpan(table_grid, 3); Grid.SetRowSpan(table_grid, 2);
            GC.Collect();
            table_grid.IsEnabled = false;
            task_timer.Enabled = true;
            progress = 1;
            task_timer.Start();
        }

        private void OnTaskEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            progress -= (double)(difficulty) / 1000;
            if (progress < 0)
            {
                task_timer.Enabled = false;
                if (game_condition)
                {
                    clearBoard();
                    game_condition = false;
                    table_grid.IsEnabled = true;
                    progress = 1;
                    task_timer.Enabled = true;
                    task_timer.Start();
                }
                else
                {
                    game_condition = true;
                    table_grid.IsEnabled = false;
                    progress = 1;
                 Device.BeginInvokeOnMainThread(async () =>  checkLevel());
                }
            }
            Device.BeginInvokeOnMainThread(async () => ((ProgressBar)content_grid.FindByName("current_time")).Progress = progress);
        }

        private void changeColor(object sender, EventArgs e)
        {
            if (((Button)sender).BackgroundColor == Color.White) ((Button)sender).BackgroundColor = Color.Red; else ((Button)sender).BackgroundColor = Color.White;
        }

        private void clearBoard()
        {
            foreach (object e in table_grid.Children) ((Button)e).BackgroundColor = Color.White;
        }

    }
}
