using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BrainTrain.Components.Math
{

    public class fastSummarize : Exercise
    {
        private int first_number, second_number, answer;
        private Grid content_grid, table_grid;
        public fastSummarize(ref Grid grid)
        {
            content_grid = grid;
            description = "На экране будет представлена таблица с числами. \n Ваша задача: найти те числа, которые в сумме дают указанные сверху числа."; 
        }

        public override void startLevel()
        {
            table_grid = createTable(3, difficulty);
            content_grid.Children.Add(table_grid, 0, 2);
            Grid.SetColumnSpan(table_grid, 3);
            createAnswer();
        }

        public override void checkLevel()
        {
        }

        public override void generateLevel()
        {
        }

        public override void lowerDifficulty()
        {
        }

        public override void raiseDifficulty()
        {
        }

        public override void displayComponents()
        {
            ((Label)content_grid.FindByName("txt_difficulty")).Text = "Сложность: \n" + difficulty.ToString();
            ((Label)content_grid.FindByName("txt_points")).Text = "Очки: \n" + general_points.ToString();
            
        }

        private Grid createTable(int size, int difficulty)
        {
            Grid elements = new Grid();
            Random rnd = new Random();
            for (int i = 0; i < size; i++)
            {
                elements.RowDefinitions.Add(new RowDefinition());
                elements.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Button new_button = new Button { Text = rnd.Next(1, difficulty * 10).ToString() };
                    new_button.Clicked += addValue;
                    elements.Children.Add(new_button, i, j);
                }
            }
            return elements;
        }

        private void addValue(object sender, EventArgs e)
        {
            if (first_number == 0) first_number = Int32.Parse(((Button)sender).Text); else second_number = Int32.Parse(((Button)sender).Text);
        }

        private string createAnswer()
        {
            Console.WriteLine(table_grid.Children[0]);
            return "";
        }

    }
}
