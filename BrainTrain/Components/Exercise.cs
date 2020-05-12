using Android.Content.Res;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;

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
        protected double progress = 1;
    
        public Exercise(ref Grid grid)
        {
            rnd = new Random();
            content_grid = grid;
            current_mistakes = 0; current_positives = 0; general_points = 0; difficulty = 1;
            general_timer = new System.Timers.Timer() { Enabled = false, Interval=100};
            general_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            task_timer = new System.Timers.Timer() { Enabled = false, Interval = 10 };
            task_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTaskEvent);
            end_time = new DateTime();
        }
        protected void OnTaskEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            progress -= (double)(difficulty) / 1000;
            if (progress < 0)
            {
                task_timer.Stop();
                current_positives = 0;
                if (++current_mistakes > 2) lowerDifficulty();
                Device.BeginInvokeOnMainThread(() => generateLevel());
            }
            Device.BeginInvokeOnMainThread(() => ((ProgressBar)content_grid.FindByName("current_time")).Progress = progress);
        }
        protected void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan cur = end_time - e.SignalTime;
            if (cur.TotalMilliseconds <= 0)
            {
                general_timer.Dispose();
                Console.WriteLine(this.GetType());
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage = new Forms.ResultPage(this.GetType().ToString(),general_points));
            }
            Device.BeginInvokeOnMainThread(() => ((Label)content_grid.FindByName("txt_timer")).Text = cur.TotalSeconds.ToString("F1"));
        }

        public void timerAnnulate()
        {
            general_timer.Dispose();
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
        protected void displayComponents() 
        {
            ((Label)content_grid.FindByName("txt_difficulty")).Text = "Сложность: \n" + difficulty.ToString();
            ((Label)content_grid.FindByName("txt_points")).Text = "Очки: \n" + general_points.ToString();
        }
    }
}
