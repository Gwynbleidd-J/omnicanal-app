using System;
using LoginForms.Shared;
using Newtonsoft.Json;
using LoginForms.Models;
using System.Windows.Forms;
using LoginForms.Utils;
using GemBox.Spreadsheet;

namespace LoginForms
{
    public partial class ExcelForm : Form
    {
        RestHelper rh = new RestHelper();
        private string FechaInicial = "";
        private string FechaFinal = "";
        ExcelItems classItems;


        public ExcelForm()
        {
            InitializeComponent();
            ComboBoxGetUsers();
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
        }

        private async void ComboBoxGetUsers()
        {
            //Log log = new Log(appPath);
            string users = await rh.getUsers();
            Json jsonUsers = JsonConvert.DeserializeObject<Json>(users);
            //log.Add($"[ChangeParameters][ComboBoxGetUsers]: Usuarios encontrados:{jsonUsers.data.users.Count}");
            for (int i = 0; i < jsonUsers.data.users.Count; i++)
            {
                if (jsonUsers.data.users[i].rolID == "1")
                {
                        //cmbAgents.Items.Add(new ParametersItems(jsonUsers.data.users[i].name, jsonUsers.data.users[i].ID));
                        cmbAgentes.Items.Add(new ExcelItems(jsonUsers.data.users[i].name, jsonUsers.data.users[i].ID));
                }
            }
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            FechaInicial = e.Start.ToString("yyyy-MM-dd");
            FechaFinal = e.End.ToString("yyyy-MM-dd");

            Console.WriteLine(FechaInicial);
            Console.WriteLine(FechaFinal);
        }

        private void cmbAgentes_SelectedIndexChanged(object sender, EventArgs e)
        {
            classItems = (ExcelItems)cmbAgentes.SelectedItem;
        }

        private async void btnGenerarExcelUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog file = new SaveFileDialog())
                {
                    file.Title = "SIDI OMNICHANNEL";
                    file.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                    file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    file.AddExtension = true;
                    file.DefaultExt = ".xlsx";
                    if (file.ShowDialog() == DialogResult.OK)
                    {
                        string excel = await rh.GenerateExcel(classItems.Value, FechaInicial, FechaFinal);
                        ExcelFile excelFile = new ExcelFile();
                        ExcelWorksheet worksheet = excelFile.Worksheets.Add("Reporte Llamadas");
                        Json jsonExcel = JsonConvert.DeserializeObject<Json>(excel);

                        worksheet.Cells[0, 0].Value = "Nombre";
                        worksheet.Cells[0, 1].Value = "Apellido Paterno";
                        worksheet.Cells[0, 2].Value = "Apellido Materno";
                        worksheet.Cells[0, 3].Value = "Fecha";
                        worksheet.Cells[0, 4].Value = "Hora de Inicio";
                        worksheet.Cells[0, 5].Value = "Hora de Fin";
                        worksheet.Cells[0, 6].Value = "Duración Llamada";
                        worksheet.Cells[0, 7].Value = "Comentarios";
                        worksheet.Cells[0, 8].Value = "Extension";
                        worksheet.Cells[0, 9].Value = "Numero exterior";
                        worksheet.Cells[0, 10].Value = "Tipo Llamada";
                        worksheet.Cells[0, 11].Value = "Colgo";
                        worksheet.Cells[0, 12].Value = "Folio";

                        int row = 0;    

                        for (int i = 0; i < jsonExcel.data.users.Count; i++)
                        {
                            for (int j = 0; j < jsonExcel.data.users[i].call.Count; j++)
                            {
                                if(jsonExcel.data.users[i].call[j].colgo == 0)
                                {
                                    if(jsonExcel.data.users[i].call[j].tipoLlamada == 1)
                                    {
                                        TimeSpan HoraInicial = TimeSpan.Parse(jsonExcel.data.users[i].call[j].startTime);
                                        TimeSpan HoraFinal = TimeSpan.Parse(jsonExcel.data.users[i].call[j].endingTime);
                                        var Duracion =  HoraFinal.Subtract(HoraInicial).TotalSeconds;
                                        var tiempoTotal = Duracion * (1 / 60);
                                        string cadena = jsonExcel.data.users[i].call[j].agentExtension;
                                        string cadena1 = jsonExcel.data.users[i].call[j].clientPhoneNumber;

                                        Console.WriteLine($"Duración de la llamada en sugundos:{Duracion}");
                                        Console.WriteLine($"Duración de la llamada en minutos:{tiempoTotal}");

                                        var agente = jsonExcel.data.users[i].call[j].colgo.ToString();
                                        var entrante = jsonExcel.data.users[i].call[j].tipoLlamada.ToString();
                                        agente = "Agente";
                                        entrante = "Entrante";
                                        worksheet.Cells[++row, 0].Value = jsonExcel.data.users[i].name;
                                        worksheet.Cells[row, 1].Value = jsonExcel.data.users[i].paternalSurname;
                                        worksheet.Cells[row, 2].Value = jsonExcel.data.users[i].maternalSurname;
                                        worksheet.Cells[row, 3].Value = jsonExcel.data.users[i].call[j].date;
                                        worksheet.Cells[row, 4].Value = jsonExcel.data.users[i].call[j].startTime;
                                        worksheet.Cells[row, 5].Value = jsonExcel.data.users[i].call[j].endingTime;
                                        worksheet.Cells[row, 6].Value = Duracion;
                                        worksheet.Cells[row, 7].Value = jsonExcel.data.users[i].call[j].comments;
                                        worksheet.Cells[row, 8].Value = cadena.Split('@')[0];
                                        worksheet.Cells[row, 9].Value = cadena1.Split('@')[0];
                                        worksheet.Cells[row, 10].Value = entrante;
                                        worksheet.Cells[row, 11].Value = agente;
                                        worksheet.Cells[row, 12].Value = jsonExcel.data.users[i].call[j].folioLlamada;
                                    }
                                    else if(jsonExcel.data.users[i].call[j].tipoLlamada == 2)
                                    {
                                        TimeSpan HoraInicial = TimeSpan.Parse(jsonExcel.data.users[i].call[j].startTime);
                                        TimeSpan HoraFinal = TimeSpan.Parse(jsonExcel.data.users[i].call[j].endingTime);
                                        var Duracion = HoraFinal.Subtract(HoraInicial).TotalSeconds;
                                        var tiempoTotal = Duracion * (1 / 60);
                                        string cadena = jsonExcel.data.users[i].call[j].agentExtension;
                                        string cadena1 = jsonExcel.data.users[i].call[j].clientPhoneNumber;

                                        Console.WriteLine($"Duración de la llamada en sugundos:{Duracion}");
                                        Console.WriteLine($"Duración de la llamada en minutos:{tiempoTotal}");

                                        var agente = jsonExcel.data.users[i].call[j].colgo.ToString();
                                        var saliente = jsonExcel.data.users[i].call[j].tipoLlamada.ToString();
                                        agente = "Agente";
                                        saliente = "Saliente";
                                        worksheet.Cells[++row, 0].Value = jsonExcel.data.users[i].name;
                                        worksheet.Cells[row, 1].Value = jsonExcel.data.users[i].paternalSurname;
                                        worksheet.Cells[row, 2].Value = jsonExcel.data.users[i].maternalSurname;
                                        worksheet.Cells[row, 3].Value = jsonExcel.data.users[i].call[j].date;
                                        worksheet.Cells[row, 4].Value = jsonExcel.data.users[i].call[j].startTime;
                                        worksheet.Cells[row, 5].Value = jsonExcel.data.users[i].call[j].endingTime;
                                        worksheet.Cells[row, 6].Value = Duracion;
                                        worksheet.Cells[row, 7].Value = jsonExcel.data.users[i].call[j].comments;
                                        worksheet.Cells[row, 8].Value = cadena.Split('@')[0];
                                        worksheet.Cells[row, 9].Value = cadena1.Split('@')[0];
                                        worksheet.Cells[row, 10].Value = saliente;
                                        worksheet.Cells[row, 11].Value = agente;
                                        worksheet.Cells[row, 12].Value = jsonExcel.data.users[i].call[j].folioLlamada;
                                    }
                                }
                                else if(jsonExcel.data.users[i].call[j].colgo == 1)
                                {
                                    if (jsonExcel.data.users[i].call[j].tipoLlamada == 1)
                                    {
                                        TimeSpan HoraInicial = TimeSpan.Parse(jsonExcel.data.users[i].call[j].startTime);
                                        TimeSpan HoraFinal = TimeSpan.Parse(jsonExcel.data.users[i].call[j].endingTime);
                                        var Duracion = HoraFinal.Subtract(HoraInicial).TotalSeconds;
                                        var tiempoTotal = Duracion * (1 / 60);
                                        string cadena = jsonExcel.data.users[i].call[j].agentExtension;
                                        string cadena1 = jsonExcel.data.users[i].call[j].clientPhoneNumber;

                                        Console.WriteLine($"Duración de la llamada en sugundos:{Duracion}");
                                        Console.WriteLine($"Duración de la llamada en minutos:{tiempoTotal}");

                                        var externo = jsonExcel.data.users[i].call[j].colgo.ToString();
                                        var entrante = jsonExcel.data.users[i].call[j].tipoLlamada.ToString();
                                        externo = "Externo";
                                        entrante = "Entrante";
                                        worksheet.Cells[++row, 0].Value = jsonExcel.data.users[i].name;
                                        worksheet.Cells[row, 1].Value = jsonExcel.data.users[i].paternalSurname;
                                        worksheet.Cells[row, 2].Value = jsonExcel.data.users[i].maternalSurname;
                                        worksheet.Cells[row, 3].Value = jsonExcel.data.users[i].call[j].date;
                                        worksheet.Cells[row, 4].Value = jsonExcel.data.users[i].call[j].startTime;
                                        worksheet.Cells[row, 5].Value = jsonExcel.data.users[i].call[j].endingTime;
                                        worksheet.Cells[row, 6].Value = Duracion;
                                        worksheet.Cells[row, 7].Value = jsonExcel.data.users[i].call[j].comments;
                                        worksheet.Cells[row, 8].Value = cadena.Split('@')[0];
                                        worksheet.Cells[row, 9].Value = cadena1.Split('@')[0];
                                        worksheet.Cells[row, 10].Value = entrante;
                                        worksheet.Cells[row, 11].Value = externo;
                                        worksheet.Cells[row, 12].Value = jsonExcel.data.users[i].call[j].folioLlamada;
                                    }
                                    else if(jsonExcel.data.users[i].call[j].tipoLlamada == 2)
                                    {
                                        TimeSpan HoraInicial = TimeSpan.Parse(jsonExcel.data.users[i].call[j].startTime);
                                        TimeSpan HoraFinal = TimeSpan.Parse(jsonExcel.data.users[i].call[j].endingTime);
                                        var Duracion = HoraFinal.Subtract(HoraInicial).TotalSeconds;
                                        var tiempoTotal = Duracion * (1 / 60);
                                        string cadena = jsonExcel.data.users[i].call[j].agentExtension;
                                        string cadena1 = jsonExcel.data.users[i].call[j].clientPhoneNumber;

                                        Console.WriteLine($"Duración de la llamada en sugundos:{Duracion}");
                                        Console.WriteLine($"Duración de la llamada en minutos:{tiempoTotal}");

                                        var externo = jsonExcel.data.users[i].call[j].colgo.ToString();
                                        var saliente = jsonExcel.data.users[i].call[j].tipoLlamada.ToString();
                                        externo = "Externo";
                                        saliente = "Saliente";
                                        worksheet.Cells[++row, 0].Value = jsonExcel.data.users[i].name;
                                        worksheet.Cells[row, 1].Value = jsonExcel.data.users[i].paternalSurname;
                                        worksheet.Cells[row, 2].Value = jsonExcel.data.users[i].maternalSurname;
                                        worksheet.Cells[row, 3].Value = jsonExcel.data.users[i].call[j].date;
                                        worksheet.Cells[row, 4].Value = jsonExcel.data.users[i].call[j].startTime;
                                        worksheet.Cells[row, 5].Value = jsonExcel.data.users[i].call[j].endingTime;
                                        worksheet.Cells[row, 6].Value = Duracion;
                                        worksheet.Cells[row, 7].Value = jsonExcel.data.users[i].call[j].comments;
                                        worksheet.Cells[row, 8].Value = cadena.Split('@')[0];
                                        worksheet.Cells[row, 9].Value = cadena1.Split('@')[0];
                                        worksheet.Cells[row, 10].Value = saliente;
                                        worksheet.Cells[row, 11].Value = externo;
                                        worksheet.Cells[row, 12].Value = jsonExcel.data.users[i].call[j].folioLlamada;
                                    }
                                }

                                //Console.WriteLine($"Usuario: {jsonExcel.data.users[i].name} {jsonExcel.data.users[i].paternalSurname} {jsonExcel.data.users[i].maternalSurname} Fecha Llamada: {jsonExcel.data.users[i].call[j].date} Inicio: {jsonExcel.data.users[i].call[j].startTime} Fin: {jsonExcel.data.users[i].call[j].endingTime} Folio:{jsonExcel.data.users[i].call[j].folioLlamada}");
                            }
                        }

                        excelFile.Save(file.FileName);
                    }
                }
                cmbAgentes.Items.Clear();
                ComboBoxGetUsers();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error][btnGenerarExcel_Click]: {ex}");
            }
        }

        private async void btnGenerarExcelFecha_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog file = new SaveFileDialog())
                {
                    file.Title = "SIDI OMNICHANNEL";
                    file.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                    file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    file.AddExtension = true;
                    file.DefaultExt = ".xlsx";
                    if (file.ShowDialog() == DialogResult.OK)
                    {
                        string excel = await rh.GenerateExcel("", FechaInicial, FechaFinal);
                        ExcelFile excelFile = new ExcelFile();
                        ExcelWorksheet worksheet = excelFile.Worksheets.Add("Reporte Llamadas");
                        Json jsonExcel = JsonConvert.DeserializeObject<Json>(excel);

                        worksheet.Cells[0, 0].Value = "Nombre";
                        worksheet.Cells[0, 1].Value = "Apellido Paterno";
                        worksheet.Cells[0, 2].Value = "Apellido Materno";
                        worksheet.Cells[0, 3].Value = "Fecha";
                        worksheet.Cells[0, 4].Value = "Hora de Inicio";
                        worksheet.Cells[0, 5].Value = "Hora de Fin";
                        worksheet.Cells[0, 6].Value = "Duración Llamada";
                        worksheet.Cells[0, 7].Value = "Comentarios";
                        worksheet.Cells[0, 8].Value = "Extension";
                        worksheet.Cells[0, 9].Value = "Numero exterior";
                        worksheet.Cells[0, 10].Value = "Tipo Llamada";
                        worksheet.Cells[0, 11].Value = "Colgo";
                        worksheet.Cells[0, 12].Value = "Folio";

                        int row = 0;    

                        for (int i = 0; i < jsonExcel.data.users.Count; i++)
                        {
                            for (int j = 0; j < jsonExcel.data.users[i].call.Count; j++)
                            {
                                if(jsonExcel.data.users[i].call[j].colgo == 0)
                                {
                                    if(jsonExcel.data.users[i].call[j].tipoLlamada == 1)
                                    {
                                        DateTime HoraInicial = DateTime.Parse(jsonExcel.data.users[i].call[j].startTime);
                                        DateTime HoraFinal = DateTime.Parse(jsonExcel.data.users[i].call[j].endingTime);
                                        
                                        var Duracion = ((float)HoraFinal.Subtract(HoraInicial).TotalMinutes);
                                        string cadena = jsonExcel.data.users[i].call[j].agentExtension;
                                        string cadena1 = jsonExcel.data.users[i].call[j].clientPhoneNumber;

                                        Console.WriteLine(Duracion);

                                        var agente = jsonExcel.data.users[i].call[j].colgo.ToString();
                                        var entrante = jsonExcel.data.users[i].call[j].tipoLlamada.ToString();
                                        agente = "Agente";
                                        entrante = "Entrante";
                                        worksheet.Cells[++row, 0].Value = jsonExcel.data.users[i].name;
                                        worksheet.Cells[row, 1].Value = jsonExcel.data.users[i].paternalSurname;
                                        worksheet.Cells[row, 2].Value = jsonExcel.data.users[i].maternalSurname;
                                        worksheet.Cells[row, 3].Value = jsonExcel.data.users[i].call[j].date;
                                        worksheet.Cells[row, 4].Value = jsonExcel.data.users[i].call[j].startTime;
                                        worksheet.Cells[row, 5].Value = jsonExcel.data.users[i].call[j].endingTime;
                                        worksheet.Cells[row, 6].Value = Duracion;
                                        worksheet.Cells[row, 7].Value = jsonExcel.data.users[i].call[j].comments;
                                        worksheet.Cells[row, 8].Value = cadena.Split('@')[0];//jsonExcel.data.users[i].call[j].agentExtension;
                                        worksheet.Cells[row, 9].Value = cadena1.Split('@')[0];//jsonExcel.data.users[i].call[j].clientPhoneNumber;
                                        worksheet.Cells[row, 10].Value = entrante;
                                        worksheet.Cells[row, 11].Value = agente;
                                        worksheet.Cells[row, 12].Value = jsonExcel.data.users[i].call[j].folioLlamada;
                                    }
                                    else if(jsonExcel.data.users[i].call[j].tipoLlamada == 2)
                                    {
                                        DateTime HoraInicial = DateTime.Parse(jsonExcel.data.users[i].call[j].startTime);
                                        DateTime HoraFinal = DateTime.Parse(jsonExcel.data.users[i].call[j].endingTime);
                                        var Duracion = ((float)HoraFinal.Subtract(HoraInicial).TotalMinutes);
                                        string cadena = jsonExcel.data.users[i].call[j].agentExtension;
                                        string cadena1 = jsonExcel.data.users[i].call[j].clientPhoneNumber;
                                        Console.WriteLine(Duracion);

                                        var agente = jsonExcel.data.users[i].call[j].colgo.ToString();
                                        var saliente = jsonExcel.data.users[i].call[j].tipoLlamada.ToString();
                                        agente = "Agente";
                                        saliente = "Saliente";
                                        worksheet.Cells[++row, 0].Value = jsonExcel.data.users[i].name;
                                        worksheet.Cells[row, 1].Value = jsonExcel.data.users[i].paternalSurname;
                                        worksheet.Cells[row, 2].Value = jsonExcel.data.users[i].maternalSurname;
                                        worksheet.Cells[row, 3].Value = jsonExcel.data.users[i].call[j].date;
                                        worksheet.Cells[row, 4].Value = jsonExcel.data.users[i].call[j].startTime;
                                        worksheet.Cells[row, 5].Value = jsonExcel.data.users[i].call[j].endingTime;
                                        worksheet.Cells[row, 6].Value = Duracion;
                                        worksheet.Cells[row, 7].Value = jsonExcel.data.users[i].call[j].comments;
                                        worksheet.Cells[row, 8].Value = cadena.Split('@')[0];//jsonExcel.data.users[i].call[j].agentExtension;
                                        worksheet.Cells[row, 9].Value = cadena1.Split('@')[0];//jsonExcel.data.users[i].call[j].clientPhoneNumber;
                                        worksheet.Cells[row, 10].Value = saliente;
                                        worksheet.Cells[row, 11].Value = agente;
                                        worksheet.Cells[row, 12].Value = jsonExcel.data.users[i].call[j].folioLlamada;
                                    }
                                }
                                else if(jsonExcel.data.users[i].call[j].colgo == 1)
                                {
                                    if (jsonExcel.data.users[i].call[j].tipoLlamada == 1)
                                    {
                                        DateTime HoraInicial = DateTime.Parse(jsonExcel.data.users[i].call[j].startTime);
                                        DateTime HoraFinal = DateTime.Parse(jsonExcel.data.users[i].call[j].endingTime);
                                        var Duracion = ((float)HoraFinal.Subtract(HoraInicial).TotalMinutes);
                                        string cadena = jsonExcel.data.users[i].call[j].agentExtension;
                                        string cadena1 = jsonExcel.data.users[i].call[j].clientPhoneNumber;
                                        Console.WriteLine(Duracion);

                                        var externo = jsonExcel.data.users[i].call[j].colgo.ToString();
                                        var entrante = jsonExcel.data.users[i].call[j].tipoLlamada.ToString();
                                        externo = "Externo";
                                        entrante = "Entrante";
                                        worksheet.Cells[++row, 0].Value = jsonExcel.data.users[i].name;
                                        worksheet.Cells[row, 1].Value = jsonExcel.data.users[i].paternalSurname;
                                        worksheet.Cells[row, 2].Value = jsonExcel.data.users[i].maternalSurname;
                                        worksheet.Cells[row, 3].Value = jsonExcel.data.users[i].call[j].date;
                                        worksheet.Cells[row, 4].Value = jsonExcel.data.users[i].call[j].startTime;
                                        worksheet.Cells[row, 5].Value = jsonExcel.data.users[i].call[j].endingTime;
                                        worksheet.Cells[row, 6].Value = Duracion;
                                        worksheet.Cells[row, 7].Value = jsonExcel.data.users[i].call[j].comments;
                                        worksheet.Cells[row, 8].Value = cadena.Split('@')[0];//jsonExcel.data.users[i].call[j].agentExtension;
                                        worksheet.Cells[row, 9].Value = cadena1.Split('@')[0];//jsonExcel.data.users[i].call[j].clientPhoneNumber;
                                        worksheet.Cells[row, 10].Value = entrante;
                                        worksheet.Cells[row, 11].Value = externo;
                                        worksheet.Cells[row, 12].Value = jsonExcel.data.users[i].call[j].folioLlamada;
                                    }
                                    else if(jsonExcel.data.users[i].call[j].tipoLlamada == 2)
                                    {
                                        DateTime HoraInicial = DateTime.Parse(jsonExcel.data.users[i].call[j].startTime);
                                        DateTime HoraFinal = DateTime.Parse(jsonExcel.data.users[i].call[j].endingTime);
                                        var Duracion = ((float)HoraFinal.Subtract(HoraInicial).TotalMinutes);
                                        string cadena = jsonExcel.data.users[i].call[j].agentExtension;
                                        string cadena1 = jsonExcel.data.users[i].call[j].clientPhoneNumber;
                                        Console.WriteLine(Duracion);

                                        var externo = jsonExcel.data.users[i].call[j].colgo.ToString();
                                        var saliente = jsonExcel.data.users[i].call[j].tipoLlamada.ToString();
                                        externo = "Externo";
                                        saliente = "Saliente";
                                        worksheet.Cells[++row, 0].Value = jsonExcel.data.users[i].name;
                                        worksheet.Cells[row, 1].Value = jsonExcel.data.users[i].paternalSurname;
                                        worksheet.Cells[row, 2].Value = jsonExcel.data.users[i].maternalSurname;
                                        worksheet.Cells[row, 3].Value = jsonExcel.data.users[i].call[j].date;
                                        worksheet.Cells[row, 4].Value = jsonExcel.data.users[i].call[j].startTime;
                                        worksheet.Cells[row, 5].Value = jsonExcel.data.users[i].call[j].endingTime;
                                        worksheet.Cells[row, 6].Value = Duracion;
                                        worksheet.Cells[row, 7].Value = jsonExcel.data.users[i].call[j].comments;
                                        worksheet.Cells[row, 8].Value = cadena.Split('@')[0];//jsonExcel.data.users[i].call[j].agentExtension;
                                        worksheet.Cells[row, 9].Value = cadena1.Split('@')[0];//jsonExcel.data.users[i].call[j].clientPhoneNumber;
                                        worksheet.Cells[row, 10].Value = saliente;
                                        worksheet.Cells[row, 11].Value = externo;
                                        worksheet.Cells[row, 12].Value = jsonExcel.data.users[i].call[j].folioLlamada;
                                    }
                                }

                                //Console.WriteLine($"Usuario: {jsonExcel.data.users[i].name} {jsonExcel.data.users[i].paternalSurname} {jsonExcel.data.users[i].maternalSurname} Fecha Llamada: {jsonExcel.data.users[i].call[j].date} Inicio: {jsonExcel.data.users[i].call[j].startTime} Fin: {jsonExcel.data.users[i].call[j].endingTime} Folio:{jsonExcel.data.users[i].call[j].folioLlamada}");
                            }
                        }

                        excelFile.Save(file.FileName);
                    }
                }
                cmbAgentes.Items.Clear();
                ComboBoxGetUsers();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[Error][btnGenerarExcel_Click]: {ex}");
            }
        }
    }
    class ExcelItems
    {
        public string Name;
        public string Value;

        public ExcelItems(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
