using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BrainTrain.Components
{
    public abstract class Exercise
    {
        protected int value, general_points, difficulty, current_mistakes, current_positives;
        public int getGeneralPoints()
        {
            return general_points;
        }

        public int displayDifficulty()
        {
            return difficulty;
        }

      

        protected System.Timers.Timer general_timer, task_timer;

        public Exercise()
        {
            value = 0; current_mistakes = 0; current_positives = 0; general_points = 0; difficulty = 0;
            general_timer = new System.Timers.Timer();
            task_timer = new System.Timers.Timer();
        }

        public abstract void setStartingDiff(int diff);
        public abstract void startLevel();

        public abstract string showDescription();
       // Метод "Показать картинку"
        public abstract void raiseDifficulty();
        public abstract void lowerDifficulty();
        public abstract void generateLevel();
        public abstract void checkLevel();
        public abstract void changeLevel();
    }


}
