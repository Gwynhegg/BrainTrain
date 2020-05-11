using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.IO;
using Android.Content.Res;
using System.Reflection;
using System.Linq;
using Syncfusion.ListView.XForms;

namespace BrainTrain.Components.Memory
{
    public static class ITextHandler
    {
        public static string getTextFromFile()
        {
            Random rnd = new Random();
            
            AssetManager asset = Android.App.Application.Context.Assets;
            return new StreamReader(asset.Open("CommonFiles/Texts/TextFile"+rnd.Next(1,asset.List("CommonFiles/Texts").Length)+".txt")).ReadToEnd();
        }

        public static string selectTextPart(string all_text)
        {
            return all_text.Substring(0, all_text.IndexOf("<mark>"));
        }

        public static List<string> getKeys(string all_text, int diff)
        {
            Random rnd = new Random();
            string[] temp = all_text.Substring(all_text.IndexOf("<mark>") + 8).Split('\n');
            List<string> ret_keys = new List<string>();
            int marker = rnd.Next(0, System.Math.Min(temp.Length, rnd.Next(2,diff+2)));
            for (int i = marker; i < temp.Length; i++) ret_keys.Add(temp[i]);
            return ret_keys;
        }
    }
    class memoryText : Exercise
    {
        List<string> keys;
        List<string> validate_table;
        bool phase = false;

        public memoryText(ref Grid grid) : base(ref grid)
        {
            description = "На экране будет отображен текст. Постарайтесь запомнить как можно больше. Как будете готовы, нажмите Далее и восстановите структуру.";
        }

        public override void startLevel()
        {
            general_timer.Enabled = true;
            general_timer.Interval = 100;
            general_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            end_time = DateTime.Now.AddMinutes(2);
            generateLevel();
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

        public override void checkLevel()
        {
            for (int i = 0; i < keys.Count; i++) if (keys[i] == validate_table[i]) current_positives++; else current_mistakes++;
            if (current_positives >= current_mistakes) raiseDifficulty(); else lowerDifficulty();
            general_points += current_positives;
            current_positives = 0;current_mistakes = 0;
            GC.Collect();
            generateLevel();
        }

        public override void generateLevel()
        {
            displayComponents();
            keys = new List<string>(); validate_table = new List<string>();
            string all_text = ITextHandler.getTextFromFile();
            string text_part = ITextHandler.selectTextPart(all_text);
            keys = ITextHandler.getKeys(all_text, difficulty);
            ((Label)content_grid.FindByName("txt_text")).Text = text_part;
        }

        public override void displayComponents()
        {
            ((Label)content_grid.FindByName("txt_difficulty")).Text = "Сложность: \n" + difficulty.ToString();
            ((Label)content_grid.FindByName("txt_points")).Text = "Очки: \n" + general_points.ToString();
        }

        public override void lowerDifficulty()
        {
        }

        public override void raiseDifficulty()
        {
        }

        public void Submit()
        {
            if (!phase)
            {
                ((Label)content_grid.FindByName("txt_text")).IsVisible = false;
                ((SfListView)content_grid.Children[content_grid.Children.Count-1]).IsVisible = true;
                validate_table = keys.OrderBy(x => rnd.Next()).ToList();
                ((SfListView)content_grid.Children[content_grid.Children.Count - 1]).ItemDragging += MemoryText_ItemDragging;
                ((SfListView)content_grid.Children[content_grid.Children.Count - 1]).ItemsSource = validate_table;
                Console.WriteLine("s");
                phase = true;
            } else
            {
                phase = false;
                ((SfListView)content_grid.Children[content_grid.Children.Count - 1]).IsVisible = false;
                ((Label)content_grid.FindByName("txt_text")).IsVisible = true;
                checkLevel();
            }
        }

        private void MemoryText_ItemDragging(object sender, ItemDraggingEventArgs e)
        {
            if (e.Action == DragAction.Drop)
            {
                var temp = validate_table[e.OldIndex];
                validate_table.RemoveAt(e.OldIndex);
                validate_table.Insert(e.NewIndex, temp);
            }
        }
    }
}
