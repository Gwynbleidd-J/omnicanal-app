using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using System.Threading.Tasks;
using LoginForms.Shared;
using LoginForms.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace LoginForms
{
    public partial class GetTimeStatus : Form
    {
        RestHelper rh = new RestHelper();
        public GetTimeStatus()
        {
            InitializeComponent();
        }

        private async void GetTimeStatus_Load(object sender, System.EventArgs e)
        {
            await GetStatusTime();
        }

        public async Task GetStatusTime()
        {
            var userId = GlobalSocket.currentUser.ID;
            var data = await rh.TotalTimeStatus(userId);
            var cleanData = (JObject)JsonConvert.DeserializeObject(data);
            var json = cleanData["data"].Children();

            cartesianChart1.Series = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50}
                }
            };

            cartesianChart1.Series.Add(new RowSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            cartesianChart1.Series[1].Values.Add(48d);

            cartesianChart1.AxisY.Add(new Axis
            {
                Labels = new[] { "Maria", "Susan", "Charles", "Frida" }
            });

            cartesianChart1.AxisX.Add(new Axis
            {
                LabelFormatter = value => value.ToString("N")
            });

            var tooltip = new DefaultTooltip
            {
                SelectionMode = TooltipSelectionMode.SharedXValues
            };

            cartesianChart1.DataTooltip = tooltip;
        }


    }
}
