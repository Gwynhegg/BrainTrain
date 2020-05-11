using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Android.Content.Res;


namespace BrainTrain.Components.Attention
{

    public static class IFigure
    {
        public static ImageSource getRandomPic(Image img)
        {
            Random rnd = new Random();
            Image temp = new Image();
            if (rnd.Next(1, 6) == 1 && img.Source!=null) return img.Source; else
            {
                ImageSource source =  ImageSource.FromResource("BrainTrain.Images.img"+rnd.Next(0,9)+".png");
                return source;
            }
        }
    }

    class attentionSwitch : Exercise
    {
        private ImageSource  previous_image;
        double progress = 1;

        public void setPrevImage(ImageSource img)
        {
            previous_image = img;
        }
        public attentionSwitch(ref Grid grid) : base(ref grid)
        {
            description = "На экране последовательно будут появляться фигуры. \n Ваша задача: определить, сопадает ли текущая фигура с предыдущей.";
        }

        public void checkLevel(bool ans)
        {
            if ((((Image)content_grid.FindByName("img_task")).Source==previous_image && ans) || (((Image)content_grid.FindByName("img_task")).Source != previous_image && !ans))
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
            setPrevImage(((Image)content_grid.FindByName("img_task")).Source);
            Device.BeginInvokeOnMainThread(async () => generateLevel());
        }

        public override void displayComponents()
        {
            ((Label)content_grid.FindByName("txt_difficulty")).Text = "Сложность: \n" + difficulty.ToString();
            ((Label)content_grid.FindByName("txt_points")).Text = "Очки: \n" + general_points.ToString();
        }

        public override void generateLevel()
        {
            displayComponents();
            Device.BeginInvokeOnMainThread(async () => ((Image)content_grid.FindByName("img_task")).Source = IFigure.getRandomPic(((Image)content_grid.FindByName("img_task"))));
            progress = 1;
            task_timer.Start();
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

        public override void startLevel()
        {
            general_timer.Enabled = true; task_timer.Enabled = true;
            general_timer.Interval = 100; task_timer.Interval = 10;
            task_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTaskEvent);
            general_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            end_time = DateTime.Now.AddMinutes(1);
            previous_image = ((Image)content_grid.FindByName("img_task")).Source;
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

        public override void checkLevel() { return;  }

    }
}
