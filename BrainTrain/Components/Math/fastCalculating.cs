using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Components.Math
{

    public abstract class MathInterface : Exercise
    {
        public int doMath(int first_number, int second_number, string operand)
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
    }


    public class fastCalculating :  MathInterface
    {
        fastCalculation_form display;
        public fastCalculating(fastCalculation_form form)
        {
            display = form;
        }
        //Панель, на которой будут отображаться компоненты
        //Запуск уровня и установление экрана компонентов
        public override void startLevel()
        {
        }
        public override string showDescription()
        {
            return "На вашем экране будут появляться числа и арифметические операции. \n Ваша задача: набрать как можно больше очков, давая правильный ответ.";
        }
        public override void generateLevel()
        {
            
        }

        public override void changeLevel()
        {
        }

        public override void checkLevel()
        {
        }

        public override void lowerDifficulty()
        {
        }

        public override void raiseDifficulty()
        {
        }

        public override void setStartingDiff(int diff)
        {
        }

        private void displayComponents()
        {

        }
    }
}
