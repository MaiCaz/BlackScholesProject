using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BlackScholesModelisation
{
    public partial class View : Form
    {

        private Controller _controller;

        public Controller controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        /// <summary>
        ///  enum State represents the different forms of the application
        /// </summary>

        public enum State {FIRST, PRESENTATION, DESCRIPTION, SETTINGS, GRAPHICS, FILE};

        /// <summary>
        ///  _state is the active form
        /// </summary>

        private State _state;
 
        // Constructor of the application
        //Initialize the form and state at PRESENTATION
        public View()
        {
            InitializeComponent();
            this.main_tab.BackColor = Color.White;
            this.main_tab.ForeColor = Color.White;
            this.state = State.FIRST;
        }

        //Getter and Setter of state
        public State state 
        {
            get { return _state; }
            set {
                if (value == State.FIRST) {
                    this.first_panel.Show();
                    this.description_menu.Hide();
                    this.settings_menu.Hide();
                    this.simulate_menu.Hide();
                    this.presentation_menu.Hide();
                    this.aboutus_panel.Hide();
                    this.from_filepanel.Hide();
                    _state = value;
                } else if (value == State.PRESENTATION) {
                    this.description_menu.Hide();
                    this.settings_menu.Hide();
                    this.simulate_menu.Hide();
                    this.presentation_menu.Show();
                    this.first_panel.Hide();
                    this.aboutus_panel.Hide();
                    this.from_filepanel.Hide();
                    _state = value;
                } else if (value == State.DESCRIPTION) {
                    this.presentation_menu.Hide();
                    this.settings_menu.Hide();
                    this.simulate_menu.Hide();
                    this.description_menu.Show();
                    this.first_panel.Hide();
                    this.aboutus_panel.Hide();
                    this.from_filepanel.Hide();
                    _state = value;
                } else if (value == State.SETTINGS) {
                    this.T_helpLabel.Text = "";
                    this.n_helpLabel.Text = "";
                    this.S0_helpLabel.Text = "";
                    this.mu_helpLabel.Text = "";
                    this.sigma_helpLabel.Text = "";
                    this.presentation_menu.Hide();
                    this.description_menu.Hide();
                    this.simulate_menu.Hide();
                    this.settings_menu.Show();
                    this.first_panel.Hide();
                    this.aboutus_panel.Hide();
                    this.from_filepanel.Hide();
                    _state = value;
                }
                else if (value == State.FILE)
                {
                    this.presentation_menu.Hide();
                    this.description_menu.Hide();
                    this.simulate_menu.Hide();
                    this.settings_menu.Hide();
                    this.first_panel.Hide();
                    this.aboutus_panel.Hide();
                    this.from_filepanel.Show();
                    _state = value;
                }
                else if (value == State.GRAPHICS)
                {
                    Settings settings = new Settings();
                    settings.mu_value = Common.ConvertToDouble(this.mu_text.Text.Replace('.',','));
                    settings.n_value = Common.ConvertToDouble(this.n_Textbox.Text.Replace('.', ','));
                    settings.S0_value = Common.ConvertToDouble(this.S0_text.Text.Replace('.', ','));
                    settings.sigma_value = Common.ConvertToDouble(this.sigma_text.Text.Replace('.', ','));
                    settings.T_value = Common.ConvertToDouble(this.T_textbox.Text.Replace('.', ','));

                    if (settings.valid()) {
                        this.from_filepanel.Hide();
                        this.presentation_menu.Hide();
                        this.description_menu.Hide();
                        this.settings_menu.Hide();
                        this.first_panel.Hide();
                        this.aboutus_panel.Hide();
                        this.simulate_menu.Show();
                        this.controller.settings(settings);
                        this.mu_label.Text = "m : " + settings.mu_value.ToString();
                        this.theta_label.Text = "q : " + settings.teta_value().ToString();
                        _state = value;
                        this.clearCharts();
                        this.draw_(controller.settings());
                    }
                        
                    else {
                        MessageBox.Show(
                           "You should enter values for all settings !",
                           "Warning",
                           MessageBoxButtons.OK);
                    }

                }
            }

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
                    Application.Exit();

        }
        private void BlackScholes_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        /// <summary>
        /// Different actions of the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"> User Click </param>
        ///         

        private void accueilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.state = State.PRESENTATION;
        }

        private void descriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.state = State.DESCRIPTION;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.state = State.SETTINGS;
        }

        private void graphicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.state = State.GRAPHICS;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        /// <summary>
        /// Pushbuttons
        /// </summary>

        private void button_simulate_Click(object sender, EventArgs e)
        {
            this.state = State.SETTINGS;
        }

        private void graphics_button_Click(object sender, EventArgs e)
        {
            this.state = State.GRAPHICS;

        }

        private void clearCharts(){
            foreach (var series in this.main_chart.Series) {
                series.Points.Clear();
            }
            foreach (var series in this.chart_teta.Series) {
                series.Points.Clear();
            }
            foreach (var series in this.chart_sigma.Series) {
                series.Points.Clear();
            }
        }

        private void help_label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.state = State.DESCRIPTION;
        }

        private void Back_button_Click(object sender, EventArgs e)
        {
            this.state = State.SETTINGS;
        }

        private void show_errors() {

        }
        private void draw_(Settings setting) {
            //main_chart.ChartAreas[0].AxisX.Maximum = (double)setting.T_value;
            //main_chart.ChartAreas[0].AxisX.MajorTickMark.Interval = (double)(setting.T_value/setting.n_value);
            main_chart.Series["Estimation"].Points.AddXY(0,setting.S0_value);
            main_chart.Series["Brownien"].Points.AddXY(0, 0);
           // main_chart.Series["Sigma"].Points.AddXY(0, setting.sigma_value);
            double[] b = Calcul.brownian_mouvement(setting);
            double[] s = Calcul.equation_stochastic(setting);
            double[] t = Calcul.equation_teta(setting,s);
            double[] x = Calcul.equation_x(setting, s);
            double[] si = Calcul.equation_sigma(setting, x);

            for (int i = 1; i <= (int)setting.n_value; i++) {
                //main_chart.Series["Brownien"].Points.AddXY(i * (setting.T_value / setting.n_value), b[i]);
                main_chart.Series["Estimation"].Points.AddXY(i*(setting.T_value/setting.n_value),s[i]);
                chart_teta.Series["Teta_est"].Points.AddXY(i * (setting.T_value / setting.n_value), t[i]);
                chart_teta.Series["teta"].Points.AddXY(i * (setting.T_value / setting.n_value), setting.teta_value());
                chart_sigma.Series["Sigma_est"].Points.AddXY(i , si[i]);
                chart_sigma.Series["sigma"].Points.AddXY(i , setting.sigma_value);
            }
            Boolean good1 = false;
            if (Calcul.erreur_sigma(setting,si)<= (double)(0.05)) {
                good1=true;
            }
            Boolean good2 = false;
            if (Calcul.erreur_teta(setting, t) <= (double)(0.05))
            {
                good2=true;
            }
            this.erreur1_label.Text = "Volatility error : " + Math.Round(Calcul.erreur_sigma(setting, si), 2);
            if (good1 == true)
            {
                this.erreur1_label.ForeColor = System.Drawing.Color.Green;
            }
            else {
                this.erreur1_label.ForeColor = System.Drawing.Color.Red;
            }
            this.erreur2_label.Text = "Risk-free rate error : " + Math.Round(Calcul.erreur_teta(setting, t),2);
            if (good2 == true)
            {
                this.erreur2_label.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                this.erreur2_label.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void main_chart_Click(object sender, EventArgs e)
        {
            this.main_chart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
	        this.main_chart.ChartAreas[0].AxisY.ScaleView.ZoomReset(0);
        }

        private void chart_sigma_Click(object sender, EventArgs e)
        {
            this.chart_sigma.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
            this.chart_sigma.ChartAreas[0].AxisY.ScaleView.ZoomReset(0);
        }

        private void chart_teta_Click(object sender, EventArgs e)
        {
            this.chart_teta.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
            this.chart_teta.ChartAreas[0].AxisY.ScaleView.ZoomReset(0);
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            this.state = State.SETTINGS;
        }

        private void button_resimulate_Click(object sender, EventArgs e)
        {
            this.clearCharts();
            this.draw_(controller.settings());
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            this.T_textbox.Text="";
            this.n_Textbox.Text="";
            this.S0_text.Text="";
            this.mu_text.Text="";
            this.sigma_text.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.state = State.FILE;
        }

        private void button_save_Click(object sender, EventArgs e)
        {   // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Pdf|*.pdf";
            saveFileDialog1.Title = "Save a Report File";
            saveFileDialog1.ShowDialog();
            String location = saveFileDialog1.FileName;

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
 
            Document doc = new Document(iTextSharp.text.PageSize.A4);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(location, FileMode.Create));

            doc.Open();

            var dateAndTime = DateTime.Now;
            var date = dateAndTime.Date;
            Chunk chunk = new Chunk("BLACK-SCHOLES MODELIZATION OF " + date.ToString("d"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 15.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE));
            Chunk chunk1 = new Chunk("Black-Scholes Simulation", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE));
            Chunk chunk2 = new Chunk("Volatility Estimation", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE));
            Chunk chunk3 = new Chunk("Risk-free rate Estimation", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE));

            Paragraph main_title = new Paragraph(chunk);
            Paragraph title1 = new Paragraph(chunk1);
            Paragraph title2 = new Paragraph(chunk2);
            Paragraph title3 = new Paragraph(chunk3);
            String theta = theta_label.Text;
            int size = theta.Length - 4;
            String theta_value = theta.Substring(4, size);

            main_title.Alignment = Element.ALIGN_CENTER;
            title1.Alignment = Element.ALIGN_CENTER;
            title2.Alignment = Element.ALIGN_CENTER;
            title3.Alignment = Element.ALIGN_CENTER;

            iTextSharp.text.Font fntTableFontHdr = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            
            PdfPTable table = new PdfPTable(6);
            PdfPCell CellT = new PdfPCell(new Phrase("Maturity", fntTableFontHdr));
            CellT.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(CellT);
            PdfPCell CellN = new PdfPCell(new Phrase("Observations", fntTableFontHdr));
            CellN.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(CellN);
            PdfPCell CellS = new PdfPCell(new Phrase("Initial condition", fntTableFontHdr));
            CellS.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(CellS);
            PdfPCell CellR = new PdfPCell(new Phrase("Risk-free rate", fntTableFontHdr));
            CellR.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(CellR);
            PdfPCell CellV = new PdfPCell(new Phrase("Volatility", fntTableFontHdr));
            CellV.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(CellV);
            PdfPCell CellD = new PdfPCell(new Phrase("Drift", fntTableFontHdr));
            CellD.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(CellD);
            PdfPCell Cellt = new PdfPCell(new Phrase(T_textbox.Text));
            Cellt.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(Cellt);
            PdfPCell Celln = new PdfPCell(new Phrase(n_Textbox.Text));
            Celln.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(Celln);
            PdfPCell Cells = new PdfPCell(new Phrase(S0_text.Text));
            Cells.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(Cells);
            PdfPCell Cellr = new PdfPCell(new Phrase(theta_value));
            Cellr.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(Cellr);
            PdfPCell Cellv = new PdfPCell(new Phrase(sigma_text.Text));
            Cellv.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(Cellv);
            PdfPCell Celld = new PdfPCell(new Phrase(mu_text.Text));
            Celld.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(Celld);


            List list1 = new List(List.UNORDERED);
            list1.Add("Risk-free rate Error");

            List list2 = new List(List.UNORDERED);
            list2.Add("Volatility Error");

            var chartimage1 = new MemoryStream();
            var chartimage2 = new MemoryStream();
            var chartimage3 = new MemoryStream();

            this.main_chart.SaveImage(chartimage1, System.Drawing.Imaging.ImageFormat.Png);
            this.chart_sigma.SaveImage(chartimage2, System.Drawing.Imaging.ImageFormat.Png);
            this.chart_teta.SaveImage(chartimage3, System.Drawing.Imaging.ImageFormat.Png);

            iTextSharp.text.Image Chart_image1 = iTextSharp.text.Image.GetInstance(chartimage1.GetBuffer());
            iTextSharp.text.Image Chart_image2 = iTextSharp.text.Image.GetInstance(chartimage2.GetBuffer());
            iTextSharp.text.Image Chart_image3 = iTextSharp.text.Image.GetInstance(chartimage3.GetBuffer());

            PdfPTable tablechart1 = new PdfPTable(1);
            tablechart1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cell = new PdfPCell(Chart_image1, true);
            tablechart1.AddCell(cell);

            PdfPTable tablechart2 = new PdfPTable(1);
            tablechart2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cell2 = new PdfPCell(Chart_image2, true);
            tablechart2.AddCell(cell2);

            PdfPTable tablechart3 = new PdfPTable(1);
            tablechart3.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cell3 = new PdfPCell(Chart_image3, true);
            tablechart3.AddCell(cell3);
            
            doc.Add(main_title);
            doc.Add(new Chunk("\n"));
            doc.Add(table);
            doc.Add(new Chunk("\n"));
            doc.Add(title1);
            doc.Add(new Chunk("\n"));
            doc.Add(tablechart1);
           // doc.Add(Chart_image1);
            doc.Add(new Chunk("\n"));
            doc.Add(title2);
            doc.Add(new Chunk("\n"));
            doc.Add(tablechart2);
           // doc.Add(Chart_image2);
            doc.Add(new Chunk("\n"));
            //doc.Add(list2);
           doc.Add(title3);
            doc.Add(new Chunk("\n"));
            doc.Add(tablechart3);
            //doc.Add(Chart_image3);
            //doc.Add(list1);


            doc.Close();
            MessageBox.Show("File saved.", "Warning", MessageBoxButtons.OK);
            }
        }

        private void home_label_Click(object sender, EventArgs e)
        {
            this.state = State.PRESENTATION;
        }

        private void T_textbox_MouseHover(object sender, EventArgs e)
        {
            this.T_helpLabel.Text = "Time of the simulation , it must be a strictly positive real number.";
        }

        private void All_MouseLeave(object sender, EventArgs e)
        {
            this.T_helpLabel.Text = "";
            this.n_helpLabel.Text = "";
            this.S0_helpLabel.Text = "";
            this.mu_helpLabel.Text = "";
            this.sigma_helpLabel.Text = "";
        }

        private void n_Textbox_MouseHover(object sender, EventArgs e)
        {
            this.n_helpLabel.Text = "Number of elements in the subdivision, in other words, it is the sample size. It must be a strictly positive real number.";
        }

        private void S0_text_MouseHover(object sender, EventArgs e)
        {
            this.S0_helpLabel.Text = "Initial price of the asset, it must be a non negative number.";
        }

        private void mu_text_MouseHover(object sender, EventArgs e)
        {
            this.mu_helpLabel.Text = "Drift of the price process, it must be a real number.";
        }

        private void sigma_text_MouseHover(object sender, EventArgs e)
        {
            this.sigma_helpLabel.Text = "Volatility of the risky asset. It's the statistical measure of the returns dispersion, it must be a positive real.";
        }

        private void back_button1_Click(object sender, EventArgs e)
        {

            this.state = State.PRESENTATION;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.format_comboBox.Text == ""){

                MessageBox.Show("Choose a format.", "Warning", MessageBoxButtons.OK);
            } else{
                OpenFileDialog file = new OpenFileDialog();
                if (this.format_comboBox.Text == ".xls")
                {

                    file.Filter = "Excel Files(.xls)|*.xls| Excel Files(.xlsx)|*.xlsx| Excel Files(*.xlsm)|*.xlsm";
                }
                else if (this.format_comboBox.Text == ".csv")
                {
                    file.Filter = "CSV files (*.csv)|*.csv";
                }
                
                if (file.ShowDialog() == DialogResult.OK)
                {
                    file_text.Text = file.FileName;
                }
            }
            
            

        }

        private void simulate_btn_Click(object sender, EventArgs e)
        {
            var reader = new StreamReader(File.OpenRead(file_text.Text));
            List<string> listA = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                listA.Add(values[0]);
            }
        }

        



    }
}
