using LiveCharts;
using LiveCharts.Wpf;
using LoginForms.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
            getChatsAsync();
            getCallsAsync();
            getStatusAsync();

            btnRecargar.BackColor = ColorTranslator.FromHtml("#e2e0e0");

        }

        RestHelper rh = new RestHelper();

        public async Task getCallsAsync()
        {
            try
            {
                var userId = GlobalSocket.currentUser.ID;
                var data = await rh.getTotalCalls(userId);
                var cleanData = (JObject)JsonConvert.DeserializeObject(data);
                var algo = cleanData["data"].Children();

                int contadorLlamadas = 0;
                int contadorLlamadasEntrantes = 0;
                int contadorLlamadasSalientes = 0;
                int contadorLlamadasTransferidas = 0;

                bool dataLlamadasEntrantes = false;
                bool dataLlamadasSalientes = false;
                bool dataLlamadasTransferidas = false;


                foreach (var item in algo)
                {
                    if (item["tipoLlamada"].Value<string>() == "1") { contadorLlamadasEntrantes++; dataLlamadasEntrantes = true; }
                    if (item["tipoLlamada"].Value<string>() == "2") { contadorLlamadasSalientes++; dataLlamadasSalientes = true; }
                    if (item["tipoLlamada"].Value<string>() == "3") { contadorLlamadasTransferidas++; dataLlamadasTransferidas = true; }

                    contadorLlamadas++;
                }


                lblLlamadasTotales.Text = contadorLlamadas.ToString();

                pieLlamadas.Series = new SeriesCollection
            {
                new PieSeries
                {
                    StrokeThickness = 0,
                    Title = "Entrantes",
                    Values = new ChartValues<double> {contadorLlamadasEntrantes},
                    PushOut = 5,
                    DataLabels = dataLlamadasEntrantes,
                    FontWeight = FontWeights.Normal,
                    FontSize = double.Parse("14")
                },
                new PieSeries
                {
                    StrokeThickness = 0,
                    PushOut = 5,
                    Title = "Salida",
                    Values = new ChartValues<double> {contadorLlamadasSalientes},
                    DataLabels = dataLlamadasSalientes,
                    FontWeight = FontWeights.Normal,
                },
                new PieSeries
                {
                    StrokeThickness = 0,
                    PushOut = 5,
                    Title = "Transferidas",
                    Values = new ChartValues<double> {contadorLlamadasTransferidas},
                    DataLabels = dataLlamadasTransferidas,
                    FontWeight = FontWeights.Normal,
                }
            };

                //pieLlamadas.LegendLocation = LegendLocation.Bottom;

            }
            catch (Exception _e)
            {
                throw _e;
            }
        }

        public async Task getChatsAsync()
        {
            try
            {
                var userId = GlobalSocket.currentUser.ID;
                var data = await rh.getUserChats(userId);
                var cleanData = (JObject)JsonConvert.DeserializeObject(data);
                var algo = cleanData["data"].Children();

                var contadorChats = 0;
                int whatsApp = 0;
                int telegram = 0;
                int chatWeb = 0;

                bool dataWhatsApp = false;
                bool dataTelegram = false;
                bool dataChatWeb = false;

                foreach (var item in algo)
                {
                    if (item["platformIdentifier"].Value<string>() == "w") { whatsApp++; dataWhatsApp = true; }
                    if (item["platformIdentifier"].Value<string>() == "t") { telegram++; dataTelegram = true; }
                    if (item["platformIdentifier"].Value<string>() == "c") { chatWeb++; dataChatWeb = true; }

                    contadorChats++;
                }

                lblChatsTotales.Text = contadorChats.ToString();


                Func<ChartPoint, string> labelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

                pieChats.Series = new SeriesCollection
            {
                new PieSeries
                {
                    //Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0)),
                    StrokeThickness = 0,
                    Title = "WhatsApp",
                    Values = new ChartValues<double> {whatsApp},
                    PushOut = 5,
                    DataLabels = dataWhatsApp,
                    //LabelPoint = labelPoint,
                    FontWeight = FontWeights.Normal,
                    FontSize = double.Parse("14")
                },
                new PieSeries
                {
                    //Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0)),
                    StrokeThickness = 0,
                    PushOut = 5,
                    Title = "Telegram",
                    Values = new ChartValues<double> {telegram},
                    DataLabels = dataTelegram,
                    FontWeight = FontWeights.Normal,
                    //LabelPoint = labelPoint
                },
                new PieSeries
                {
                    //Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0)),
                    StrokeThickness = 0,
                    PushOut = 5,
                    Title = "Chat Web",
                    Values = new ChartValues<double> {chatWeb},
                    DataLabels = dataChatWeb,
                    FontWeight = FontWeights.Normal,
                    //LabelPoint = labelPoint
                }
            };

                //pieChats.LegendLocation = LegendLocation.Bottom;

            }
            catch (Exception _e)
            {
                throw _e;
            }

        }

        public async Task getStatusAsync()
        {
            try
            {
                var userId = GlobalSocket.currentUser.ID;
                var data = await rh.GetUserStates(userId);
                var cleanData = (JObject)JsonConvert.DeserializeObject(data);
                var algo = cleanData["data"];

                var Disponible = algo["Disponible"].Value<int>();
                var NoDisponible = algo["NoDisponible"].Value<int>();
                var ACW = algo["ACW"].Value<int>();
                var Capacitacion = algo["Capacitacion"].Value<int>();
                var Calidad = algo["Calidad"].Value<int>();
                var Sanitario = algo["Sanitario"].Value<int>();
                var Comida = algo["Comida"].Value<int>();
                var Break = algo["Break"].Value<int>();


                bool dataDisponible = false;
                bool dataNoDisponible = false;
                bool dataACW = false;
                bool dataCapacitacion = false;
                bool dataCalidad = false;
                bool dataSanitario = false;
                bool dataComida = false;
                bool dataBreak = false;



                foreach (var item in algo)
                {
                    if (Disponible != 0) { dataDisponible = true; }
                    if (NoDisponible != 0) { dataNoDisponible = true; }
                    if (ACW != 0) { dataACW = true; }
                    if (Capacitacion != 0) { dataCapacitacion = true; }
                    if (Calidad != 0) { dataCalidad = true; }
                    if (Sanitario != 0) { dataSanitario = true; }
                    if (Comida != 0) { dataComida = true; }
                    if (Break != 0) { dataBreak = true; }

                }

                pieStatus.Series = new SeriesCollection
            {
                new PieSeries
                {
                    StrokeThickness = 0,
                    Title = "Disponible",
                    Values = new ChartValues<double> {Disponible},
                    PushOut = 5,
                    DataLabels = dataDisponible,
                    FontWeight = FontWeights.Normal,
                    FontSize = double.Parse("14"),
                    LabelPoint = val => val.Y +" "

                },
                new PieSeries
                {
                    StrokeThickness = 0,
                    PushOut = 5,
                    Title = "No Disponible",
                    Values = new ChartValues<double> {NoDisponible},
                    DataLabels = dataNoDisponible,
                    FontWeight = FontWeights.Normal,
                    LabelPoint = val => val.Y +" "

                },
                new PieSeries
                {
                    StrokeThickness = 0,
                    PushOut = 5,
                    Title = "ACW",
                    Values = new ChartValues<double> {ACW},
                    DataLabels = dataACW,
                    FontWeight = FontWeights.Normal,
                    LabelPoint = val => val.Y +" "
                },
                new PieSeries
                {
                    StrokeThickness = 0,
                    PushOut = 5,
                    Title = "Capacitacion",
                    Values = new ChartValues<double> {Capacitacion},
                    DataLabels = dataCapacitacion,
                    FontWeight = FontWeights.Normal,
                    LabelPoint = val => val.Y +" "
                },
                new PieSeries
                {
                    StrokeThickness = 0,
                    PushOut = 5,
                    Title = "Calidad",
                    Values = new ChartValues<double> {Calidad},
                    DataLabels = dataCalidad,
                    FontWeight = FontWeights.Normal,
                    LabelPoint = val => val.Y +" "
                },
                new PieSeries
                {
                    StrokeThickness = 0,
                    PushOut = 5,
                    Title = "Sanitario",
                    Values = new ChartValues<double> {Sanitario},
                    DataLabels = dataSanitario,
                    FontWeight = FontWeights.Normal,
                    LabelPoint = val => val.Y +" "
                },
                new PieSeries
                {
                    StrokeThickness = 0,
                    PushOut = 5,
                    Title = "Comida",
                    Values = new ChartValues<double> {Comida},
                    DataLabels = dataComida,
                    FontWeight = FontWeights.Normal,
                    LabelPoint = val => val.Y +" "

                },
                new PieSeries
                {
                    StrokeThickness = 0,
                    PushOut = 5,
                    Title = "Break",
                    Values = new ChartValues<double> {Break},
                    DataLabels = dataBreak,
                    FontWeight = FontWeights.Normal,
                    LabelPoint = val => val.Y +" "
            },

            };
                //pieStatus.LegendLocation = LegendLocation.Bottom;

            }
            catch (Exception _e)
            {
                throw _e;
            }
        }

        //public void SetSolidGauge()
        //{
        //    solidGauge1.Uses360Mode = true;
        //    solidGauge1.From = 0;
        //    solidGauge1.To = 100;
        //    solidGauge1.Value = dummy;

        //    solidGauge1.Base.GaugeRenderTransform = new TransformGroup
        //    {
        //        Children = new TransformCollection
        //        {
        //            new RotateTransform(90),
        //            //new ScaleTransform {ScaleX = -1}
        //        }
        //    };

        //    solidGauge1.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(240, 53, 13));
        //    solidGauge1.LabelFormatter = val => val.ToString() + "%";

        //    solidGauge1.FontWeight = FontWeights.Light;
        //    solidGauge1.FontSize = 4;
        //    solidGauge1.FontStyle = FontStyles.Normal;
        //    solidGauge1.FontStretch = FontStretches.Condensed;
        //    solidGauge1.HighFontSize = 12;
        //    solidGauge1.FontFamily = System.Windows.SystemFonts.SmallCaptionFontFamily;

        //    Font fontM = new Font("Times New Roman", 12.0f);
        //    solidGauge1.Font = fontM;


        //    solidGauge2.Uses360Mode = true;
        //    solidGauge2.From = 0;
        //    solidGauge2.To = 10000;
        //    solidGauge2.Value = 2823;
        //    //solidGauge2.HighFontSize = 30;
        //    //solidGauge2.Base.Foreground = System.Windows.Media.Brushes.White;
        //    solidGauge2.InnerRadius = 45;
        //    //solidGauge2.GaugeBackground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(71, 128, 181));
        //    solidGauge2.Base.GaugeRenderTransform = new TransformGroup
        //    {
        //        Children = new TransformCollection
        //        {
        //            new RotateTransform(90),
        //            //new ScaleTransform {ScaleX = -1}
        //        }
        //    };
        //    //solidGauge2.FromColor = Colors.Blue;
        //    //solidGauge2.ToColor = Colors.Black;
        //    solidGauge2.LabelFormatter = val => "%" + val.ToString();
        //    //solidGauge2.HighFontSize = 14;


        //    solidGauge3.Uses360Mode = true;
        //    solidGauge3.From = 0;
        //    solidGauge3.To = 850;
        //    solidGauge3.Value = 823;
        //    solidGauge3.InnerRadius = 45;
        //    solidGauge3.Base.GaugeRenderTransform = new TransformGroup
        //    {
        //        Children = new TransformCollection
        //        {
        //            new RotateTransform(90),
        //        }
        //    };
        //    solidGauge3.Base.GaugeActiveFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(240, 53, 13));
        //    solidGauge3.LabelFormatter = val => "%" + val.ToString();
        //    solidGauge3.HighFontSize = 14;



        //    solidGauge4.Uses360Mode = true;
        //    solidGauge4.From = 0;
        //    solidGauge4.To = 32000;
        //    solidGauge4.Value = 24276;
        //    solidGauge4.InnerRadius = 40;
        //    solidGauge4.Base.GaugeRenderTransform = new TransformGroup
        //    {
        //        Children = new TransformCollection
        //        {
        //            new RotateTransform(90),
        //        }
        //    };
        //    solidGauge4.LabelFormatter = val => "%" + val.ToString();
        //    solidGauge4.HighFontSize = 12;

        //}

        private async void btnRecargar_Click(object sender, EventArgs e)
        {
            await getChatsAsync();
            await getCallsAsync();
            await getStatusAsync();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
