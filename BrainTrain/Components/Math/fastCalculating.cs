using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Components.Math
{

    //Статический класс, отвечающий за все математические взаимодействия на форме.
    public static class MathInterface
    {
        //Метод, отвечающий за вычисления. Получает два числа, оператор и сравнивает результат с указанным овтетом.
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

        //Перегрузка метода doMath, возвращающая ответ в соответствии с указанными параметрами - двумя числами и операндом.
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

        //Метод, отвечающий за генерацию операнда.
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

    //Задание на быструю арифметику. Игроку будут показаны два числа и математический оператор. Цель игрока - правильно указать ответ.
    //Класс наследывается от абстрактного класса Exercise
    public class fastCalculating :  Exercise
    {
        //Переменные овтечающие за операнд, первое, второе число и результат вычислений.
        private string operand;
        private int first_number, second_number, answer;

        //Конструктор класса. С помощью ключевого слова base обращаемся к классу-предку.
        public fastCalculating(ref Grid grid) : base(ref grid)
        {
            //Устанавливаем описание задания для отображения на форме.
            description = "На вашем экране будут появляться числа и арифметические операции. \n Ваша задача: набрать как можно больше очков, давая правильный ответ.";
        }

        //Метод, отвечающий за первоначаную задачу параметров и старт уровня.
        public override void startLevel()
        {

            //В данном методе задается интервал, в течение которого будет выполняться задание.
            end_time = DateTime.Now.AddMinutes(1);
            //Запускается главный таймер и генерируется первый уровень.
            task_timer.Enabled = true;
            general_timer.Enabled = true;  general_timer.Start();
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        //Метод, отвечающий за генерацию нового уровня.
        public override void generateLevel()
        {
            //Получение операнда с помощью статического класса.
            operand = MathInterface.createOperand(rnd.Next(4));
            if (operand == "*")
            {
                //Получение чисел для конкретной операции - умножения.
                first_number = rnd.Next(1, difficulty+10);
                if (first_number > 10) second_number = rnd.Next(1, first_number / 10 + 1); else second_number = rnd.Next(1, difficulty * 10);
                
                //Получение чисел для операции деления.
            } else if (operand == "/")
            {
                first_number = rnd.Next(1, difficulty * 10);
                answer = rnd.Next(1, difficulty+5);
                second_number = answer * first_number;
                if (second_number > first_number) Swap(ref first_number,ref  second_number);
            } else
            {
                //Получение чисел для сложения и вычитания.
                first_number = rnd.Next(1, difficulty * 10);
                second_number = rnd.Next(1, difficulty * 10);
            }

            //Высчитывание результата для сложившейся задачи.
            answer = MathInterface.doMath(first_number, second_number, operand);
            //Отображение компонентов на форме.
            displayComponents();
            }

        //Метод, возвращающий длину ответа для получения маски ввода.
        public int getAnswerLength()
        {
            return answer.ToString().Length;
        }

        //Метод проверки правильности ответа. Параметр представляет собой введенный пользователем ответ..
        public void checkLevel(string answer)
        {
            //Если ответ пользователя равен настоящему ответу, то..
            if (this.answer.ToString() == answer)
            {
                //Прибавляем очки и увеличиваем сложность.
                general_points += difficulty;
                current_mistakes = 0;
                if (++current_positives > 2) raiseDifficulty();
            } else
            {
                //Уменьшаем сложность.
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
            difficulty += current_positives / 3;
        }

        //Переопределение отображения компонентов.
        new public void displayComponents()
        {
            //Отображение интерфейса и чисел.
            progress = 1;
            task_timer.Start();
            ((Label)content_grid.FindByName("txt_difficulty")).Text = "Сложность: \n" + difficulty.ToString();
            ((Label)content_grid.FindByName("txt_points")).Text = "Очки: \n" + general_points.ToString();
            ((Label)content_grid.FindByName("txt_first")).Text = first_number.ToString();
            ((Label)content_grid.FindByName("txt_operand")).Text = operand;
            ((Label)content_grid.FindByName("txt_second")).Text = second_number.ToString();
            if (answer<0) ((Label)content_grid.FindByName("txt_answer")).Text = "-"; else ((Label)content_grid.FindByName("txt_answer")).Text = "";
        }


        //Вспомогательный метод, меняющий значения двух чисел.
        private void Swap(ref int first,ref int second)
        {
            int temp = first;
            first = second;
            second = temp;
        }

        //Поскольку данная задача имеет параметр в методе проверки уровня, метод перегружен, а его основоположник переопределен "вхолостую".
        public override void checkLevel(){return;}
    }
}
