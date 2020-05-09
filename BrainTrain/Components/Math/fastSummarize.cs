using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BrainTrain.Components.Math
{

    public class fastSummarize : Exercise
    {
        private int first_number, second_number, answer_count=0;
        int[,] aux_table;
        int[] answers;
        private Grid table_grid;
        public fastSummarize(ref Grid grid) :base(ref grid)
        {
            description = "На экране будет представлена таблица с числами. \n Ваша задача: найти те числа, которые в сумме дают указанные сверху числа."; 
        }

        public override void startLevel()
        {
            general_timer.Enabled = true;
            general_timer.Interval = 100;
            general_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            end_time = DateTime.Now.AddMinutes(1);
            general_timer.Start();
            displayComponents();
            generateLevel();
        }


        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan cur = end_time - e.SignalTime;
            if (cur.TotalMilliseconds <= 0)
            {
                Application.Current.MainPage = new Forms.ResultPage();
            }
            Device.BeginInvokeOnMainThread(async () => ((Label)content_grid.FindByName("txt_timer")).Text = cur.TotalSeconds.ToString("F1"));
        }

        public override void checkLevel()
        {
            if (first_number + second_number == answers[answer_count])
            {
                general_points += difficulty;
                current_positives++;
            }
            else current_mistakes++;
            if (++answer_count == answers.Length)
            {
                if (current_positives >= current_mistakes) raiseDifficulty(); else lowerDifficulty();
                current_positives = 0; current_mistakes = 0; answer_count = 0;
                content_grid.Children.RemoveAt(content_grid.Children.Count-1);
                generateLevel();
            }
            else 
            {
                ((Label)content_grid.FindByName("curr_number")).Text = ((Label)content_grid.FindByName("curr_number")).Text.Remove(0, ((Label)content_grid.FindByName("curr_number")).Text.IndexOf(" ")+1); 
            }
            first_number = 0; second_number = 0;
            displayComponents();
        }

        public override void generateLevel()
        {
            createTable();
            createAnswer(difficulty / 5 + 5);
            ((Label)content_grid.FindByName("curr_number")).Text = String.Join(" ", answers);
        }

        public override void lowerDifficulty()
        {
            //ПОДУМОТЬ
            difficulty = difficulty * current_positives / (current_mistakes + 1) + 1;
        }

        public override void raiseDifficulty()
        {
            //ПОДУМОТЬ
             difficulty = difficulty * current_positives / (current_mistakes + 1) + 1;
        }

        public override void displayComponents()
        {
            
            ((Label)content_grid.FindByName("txt_difficulty")).Text = "Сложность: \n" + difficulty.ToString();
            ((Label)content_grid.FindByName("txt_points")).Text = "Очки: \n" + general_points.ToString();
            
        }

        private void createTable()
        {
            int size = difficulty / 3 + 3;
            table_grid = new Grid();
            rnd = new Random();
            aux_table = new int[size, size];
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
                    aux_table[i, j] = rnd.Next(1, difficulty * 10);
                    new_button = new Button { Text = aux_table[i,j].ToString()};
                    new_button.Clicked += addValue;
                    table_grid.Children.Add(new_button,j,i);
                }
            }
            content_grid.Children.Add(table_grid, 0, 2);
            Grid.SetColumnSpan(table_grid, 3);
        }

        private void addValue(object sender, EventArgs e)
        {
            if (first_number == 0) first_number = Int32.Parse(((Button)sender).Text); else 
            { 
                second_number = Int32.Parse(((Button)sender).Text);
                checkLevel();
            }
        }

        private void createAnswer(int count)
        {
            answers = new int[count];
            for (int i = 0; i < count; i++) answers[i] = aux_table[rnd.Next(0, aux_table.GetLength(0)), rnd.Next(0, aux_table.GetLength(0))] + aux_table[rnd.Next(0, aux_table.GetLength(0)), rnd.Next(0, aux_table.GetLength(0))];
        }

    }
}
