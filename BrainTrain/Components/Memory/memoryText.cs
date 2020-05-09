using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.IO;

namespace BrainTrain.Components.Memory
{
    public static class ITextHandler
    {
        public static string getTextFromFile()
        {
          StreamReader txt_reader = new StreamReader(BrainTrain.CommonFiles.Texts.text1.txt);
            return "";
        }
    }
    class memoryText : Exercise
    {



        public memoryText(ref Grid grid) : base(ref grid)
        {

        }

        public override void startLevel()
        {

        }

        public override void checkLevel()
        {
        }

        public override void generateLevel()
        {
        }

        public override void displayComponents()
        {
        }

        public override void lowerDifficulty()
        {
        }

        public override void raiseDifficulty()
        {
        }
    }
}
