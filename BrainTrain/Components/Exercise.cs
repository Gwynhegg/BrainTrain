using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BrainTrain.Components
{
    public abstract class Exercise
    {
        protected int value, general_points, difficulty, current_mistakes, current_positives;
        protected DateTime start_time, end_time;
        protected Random rnd;
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
            rnd = new Random();
            value = 0; current_mistakes = 0; current_positives = 0; general_points = 0; difficulty = 1;
            general_timer = new System.Timers.Timer();
            task_timer = new System.Timers.Timer();
        }

        public void setStartingDiff(int diff)
        {
            this.difficulty = diff;
        }
        public abstract void startLevel();

        public abstract string showDescription();
        public abstract void raiseDifficulty();
        public abstract void lowerDifficulty();
        public abstract void generateLevel();
        public abstract void checkLevel();
    }


}
