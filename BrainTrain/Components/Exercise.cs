using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BrainTrain.Components
{
    public abstract class Exercise
    {
        protected int general_points, difficulty, current_mistakes, current_positives;
        protected string description;
        protected Random rnd;
        protected System.Timers.Timer general_timer, task_timer;
        protected DateTime end_time;
        protected Grid content_grid;
    
        public Exercise(ref Grid grid)
        {
            rnd = new Random();
            current_mistakes = 0; current_positives = 0; general_points = 0; difficulty = 1;
            general_timer = new System.Timers.Timer();
            task_timer = new System.Timers.Timer();
            end_time = new DateTime();
            content_grid = grid;
        }

        public void setStartingDiff(int diff)
        {
            this.difficulty = diff;
        }
        public abstract void startLevel();
        public string showDescription()
        {
            return description;
        }
        public abstract void raiseDifficulty();
        public abstract void lowerDifficulty();
        public abstract void generateLevel();
        public abstract void checkLevel();
        public abstract void displayComponents();
    }
}
