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
                ImageSource source =  ImageSource.FromResource("BrainTrain.Images.img"+rnd.Next(0,8)+".png");
                return source;
            }
        }
    }

    class attentionSwitch : Exercise
    {
        private ImageSource  previous_image;

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
            GC.Collect();
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        public override void generateLevel()
        {
            displayComponents();
            Device.BeginInvokeOnMainThread(() => ((Image)content_grid.FindByName("img_task")).Source = IFigure.getRandomPic(((Image)content_grid.FindByName("img_task"))));
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
           
            end_time = DateTime.Now.AddMinutes(1);
            previous_image = ((Image)content_grid.FindByName("img_task")).Source;
            task_timer.Enabled = true;
            general_timer.Enabled = true; general_timer.Start();
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        public override void checkLevel() { return;  }

    }
}
