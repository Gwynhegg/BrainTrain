using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BrainTrain.Components.Attention
{

    public abstract class ColorExercise : Exercise
    {
        protected List<Tuple<string, Color>> text_color;
        public ColorExercise(ref Grid grid) : base(ref grid)
        {
            text_color = new List<Tuple<string, Color>>();
        }

        public void setColorTable()
        {
            text_color.Add(new Tuple<string,Color>("Красный", Color.Red));
            text_color.Add(new Tuple<string, Color>("Зеленый", Color.Green));
            text_color.Add(new Tuple<string, Color>("Черный", Color.Black));
            text_color.Add(new Tuple<string, Color>("Синий", Color.Blue));
            text_color.Add(new Tuple<string, Color>("Желтый", Color.Yellow));
            text_color.Add(new Tuple<string, Color>("Фиолетовый", Color.Purple));
        }
    }
    class colorAttention : ColorExercise
    {

        double progress = 1;
        private int answer_index;

        public colorAttention(ref Grid grid) : base(ref grid)
        {
            description = "На экране будут отображаться названия цветов. Цвет надписи и название не совпадают. \n Ваша задача: правильно нажать на кнопку с цветом, совпадающим с цветом, указанным текстом.";
        }

        public override void startLevel()
        {
            general_timer.Enabled = true; task_timer.Enabled = true;
            general_timer.Interval = 100; task_timer.Interval = 10;
            task_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTaskEvent);
            general_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            end_time = DateTime.Now.AddMinutes(1);
            setColorTable();
            general_timer.Start();
            Device.BeginInvokeOnMainThread(async () => generateLevel());
        }

        private void OnTaskEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            progress -= (double)(difficulty) / 1000;
            if (progress < 0)
            {
                task_timer.Enabled = false;
                current_positives = 0;
                if (++current_mistakes > 2) lowerDifficulty();
                Device.BeginInvokeOnMainThread(async () => generateLevel());
            }
            Device.BeginInvokeOnMainThread(async () => ((ProgressBar)content_grid.FindByName("current_time")).Progress = progress);
        }
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan cur = end_time - e.SignalTime;
            if (cur.TotalMilliseconds <= 0)
            {
                general_timer.Enabled = false;
                Application.Current.MainPage = new Forms.ResultPage();
            }
            Device.BeginInvokeOnMainThread(async () => ((Label)content_grid.FindByName("txt_timer")).Text = cur.TotalSeconds.ToString("F1"));
        }

        public override void generateLevel()
        {
            displayComponents();
            answer_index = rnd.Next(0, text_color.Count);
            ((Label)content_grid.FindByName("txt_tasktext")).Text = text_color[answer_index].Item1;
            ((Label)content_grid.FindByName("txt_tasktext")).TextColor = text_color[rnd.Next(0, text_color.Count)].Item2;
            progress = 1;
            task_timer.Enabled = true; task_timer.Start();
        }

        public void checkLevel(Color color)
        {
            if (color == text_color[answer_index].Item2)
            {
                general_points += difficulty;
                current_mistakes = 0; 
                if (++current_positives > 2) raiseDifficulty();
            }
            else
            {
                current_positives = 0;
                if (++current_mistakes > 2) lowerDifficulty();
            }
            generateLevel();
        }        

        public override void displayComponents()
        {
            ((Label)content_grid.FindByName("txt_difficulty")).Text = "Сложность: \n" + difficulty.ToString();
            ((Label)content_grid.FindByName("txt_points")).Text = "Очки: \n" + general_points.ToString();
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

        public override void checkLevel() { return; }

    }
}
