﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BrainTrain.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : TabbedPage
    {
        public CategoryPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new InterfacePage();
            return true;
        }

        //Методы-обработчики нажатия на определенную кнопку и отсылающие к необходимому заданию.
        private void ToFastMath(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Components.Math.fastCalculation_form();
        }

        private void ToFastSum(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Components.Math.fastSummarize_form();
        }

        private void ToMemTable(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Components.Memory.memoryTable_form();
        }

        private void ToMemText(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Components.Memory.memoryText_form();
        }

        private void ToColoredText(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Components.Attention.colorAttention_form();
        }

        private void ToAttSwitch(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Components.Attention.attentionSwitch_form();
        }
    }
    }
