using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Components.Math
{

    public static class MathInterface
    {
        public static bool doMath(int first_number, int second_number, string operand, int answer)
        {
            int temporal=0;
            switch (operand)
            {
                case "+": temporal = first_number + second_number; break;
                case "-": temporal = first_number - second_number; break;
                case "*": temporal = first_number * second_number; break;
                case "/": temporal = (int)(first_number / second_number); break;
            }
            if (temporal == answer) return true; else return false;
        }

        public static int doMath(int first_number, int second_number, string operand)
        {
            switch (operand)
            {
                case "+": return first_number + second_number;
                case "-": return first_number - second_number; 
                case "*": return first_number * second_number; 
                case "/": return (int)(first_number / second_number); 
            }
            return 0;
        }

        public static string createOperand(int num)
        {
            switch (num)
            {
                case 0: return "+";
                case 1: return "-";
                case 2: return "*";
                case 3: return "/";
            }
            return "";
        }
    }


    public class fastCalculating :  Exercise
    {
        private string operand;
        private int first_number, second_number, answer;
        public fastCalculating(ref Grid grid) : base(ref grid)
        {
            description = "На вашем экране будут появляться числа и арифметические операции. \n Ваша задача: набрать как можно больше очков, давая правильный ответ.";
        }

        public override void startLevel()
        {
            end_time = DateTime.Now.AddMinutes(1);
            task_timer.Enabled = true;
            general_timer.Enabled = true;  general_timer.Start();
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        public override void generateLevel()
        {
            operand = MathInterface.createOperand(rnd.Next(4));
            if (operand == "*")
            {
                //ПОЛУЧИТЬ НОРМАЛЬНУЮ ФОРМУЛУ, КОРРЕЛИРУЮЩУЮ ОТ СЛОЖНОСТИ
                first_number = rnd.Next(1, difficulty * 10);
                if (first_number > 10) second_number = rnd.Next(1, first_number / 15 + 1); else second_number = rnd.Next(1, difficulty * 10);
                
            } else if (operand == "/")
            {
                first_number = rnd.Next(1, difficulty * 10);
                answer = rnd.Next(1, difficulty+5);
                second_number = answer * first_number;
                if (second_number > first_number) Swap(ref first_number,ref  second_number);
            } else
            {
                first_number = rnd.Next(1, difficulty * 10);
                second_number = rnd.Next(1, difficulty * 10);
            }
            answer = MathInterface.doMath(first_number, second_number, operand);
            displayComponents();
            }

        public int getAnswerLength()
        {
            return answer.ToString().Length;
        }
        public void checkLevel(string answer)
        {
            if (this.answer.ToString() == answer)
            {
                general_points += difficulty;
                current_mistakes = 0;
                if (++current_positives > 2) raiseDifficulty();
            } else
            {
                current_positives = 0;
                if (++current_mistakes > 2) lowerDifficulty();
            }
            GC.Collect();
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        public override void lowerDifficulty()
        {
            difficulty -= current_mistakes / 2;
            if (difficulty < 1) difficulty = 1;
        }

        public override void raiseDifficulty()
        {
            difficulty += current_positives / 3;
        }

        new public void displayComponents()
        {
            progress = 1;
            task_timer.Start();
            ((Label)content_grid.FindByName("txt_difficulty")).Text = "Сложность: \n" + difficulty.ToString();
            ((Label)content_grid.FindByName("txt_points")).Text = "Очки: \n" + general_points.ToString();
            ((Label)content_grid.FindByName("txt_first")).Text = first_number.ToString();
            ((Label)content_grid.FindByName("txt_operand")).Text = operand;
            ((Label)content_grid.FindByName("txt_second")).Text = second_number.ToString();
            if (answer<0) ((Label)content_grid.FindByName("txt_answer")).Text = "-"; else ((Label)content_grid.FindByName("txt_answer")).Text = "";
        }

        private void Swap(ref int first,ref int second)
        {
            int temp = first;
            first = second;
            second = temp;
        }

        public override void checkLevel(){return;}
    }
}
