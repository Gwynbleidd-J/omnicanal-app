﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace LoginForms
{
    public partial class DashboardAgente : Form
    {
        public DashboardAgente()
        {
            InitializeComponent();
            SetSolidGauge();
        }

        double dummy = 50;


        public void SetSolidGauge()
        {
            solidGauge1.Uses360Mode = true;
            solidGauge1.From = 0;
            solidGauge1.To = 100;
            solidGauge1.Value = dummy;

            solidGauge1.Base.GaugeRenderTransform = new TransformGroup
            {
                Children = new TransformCollection
                {
                    new RotateTransform(90),
                    //new ScaleTransform {ScaleX = -1}
                }
            };

            solidGauge1.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(240, 53, 13));
            solidGauge1.LabelFormatter = val => val.ToString() + "%";

            solidGauge1.FontWeight = FontWeights.Light;
            solidGauge1.FontSize = 4;
            solidGauge1.FontStyle = FontStyles.Normal;
            solidGauge1.FontStretch = FontStretches.Condensed;
            solidGauge1.HighFontSize = 12;
            solidGauge1.FontFamily = System.Windows.SystemFonts.SmallCaptionFontFamily;

            Font fontM = new Font("Times New Roman", 12.0f);
            solidGauge1.Font = fontM;


            solidGauge2.Uses360Mode = true;
            solidGauge2.From = 0;
            solidGauge2.To = 10000;
            solidGauge2.Value = 2823;
            //solidGauge2.HighFontSize = 30;
            //solidGauge2.Base.Foreground = System.Windows.Media.Brushes.White;
            solidGauge2.InnerRadius = 45;
            //solidGauge2.GaugeBackground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(71, 128, 181));
            solidGauge2.Base.GaugeRenderTransform = new TransformGroup
            {
                Children = new TransformCollection
                {
                    new RotateTransform(90),
                    //new ScaleTransform {ScaleX = -1}
                }
            };
            //solidGauge2.FromColor = Colors.Blue;
            //solidGauge2.ToColor = Colors.Black;
            solidGauge2.LabelFormatter = val => "$" + val.ToString();
            //solidGauge2.HighFontSize = 14;


            solidGauge3.Uses360Mode = true;
            solidGauge3.From = 0;
            solidGauge3.To = 850;
            solidGauge3.Value = 823;
            solidGauge3.InnerRadius = 45;
            solidGauge3.Base.GaugeRenderTransform = new TransformGroup
            {
                Children = new TransformCollection
                {
                    new RotateTransform(90),
                }
            };
            solidGauge3.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(240, 53, 13));
            solidGauge3.LabelFormatter = val => "$" + val.ToString();
            solidGauge3.HighFontSize = 14;



            solidGauge4.Uses360Mode = true;
            solidGauge4.From = 0;
            solidGauge4.To = 32000;
            solidGauge4.Value = 24276;
            solidGauge4.InnerRadius = 40;
            solidGauge4.Base.GaugeRenderTransform = new TransformGroup
            {
                Children = new TransformCollection
                {
                    new RotateTransform(90),
                }
            };
            solidGauge4.LabelFormatter = val => "$" + val.ToString();
            solidGauge4.HighFontSize = 12;

        }

    }
}
