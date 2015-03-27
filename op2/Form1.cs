using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Timers;
//using Gif.Components;
using System.Drawing.Imaging;
using System.Xml.Linq;
//using PdfSharp.Pdf;
//using PdfSharp.Drawing;
using System.Diagnostics;

namespace op2
{
    public partial class mc : Form
    {

        public mc()
        {
            InitializeComponent();
        }

		// Pulsante per importare i dati e scaricare le mappe 27-11-2014
		// Inserimento del paese per scaricare i dati
		private void Import_Click(object sender, EventArgs e)
		{
			// Display the ProgressBar control.
			progressBar1.Visible = true;
			// Set Minimum to 1 to represent the first file being copied.
			progressBar1.Minimum = 1;
			// Set Maximum to the total number of files to copy.
			progressBar1.Maximum = 4;
			// Set the initial value of the ProgressBar.
			progressBar1.Value = 1;
			// Set the Step property to a value of 1 to represent each file being copied.
			progressBar1.Step = 4;


			int max = 13;
            string lon = "";
            string lat = "";
			string URLrain = "";
			string URLwind = "";
			string URL850t = "";
			string URL500p = "";

			DateTime thisday = DateTime.Today;
			DateTime now = DateTime.Now;
            int hour = now.Hour;
            string z = "";
			string today = String.Format("{0:yyyyMMdd}", thisday);

            eliminaFile();
            progressBar1.PerformStep();

			// Aggiorniamo le immagini 
			using (WebClient Client = new WebClient())
			{
				Client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
				Client.Headers.Add("Cache-Control", "no-cache");

				int f;

				f = 1;
                Client.DownloadFile("http://www.wetterzentrale.de/pics/Rtavn063.gif", "europeC" + f + ".gif");

                for (f = 2; f <= max; f++)
                {
                    if (!File.Exists("europeC" + f + ".gif"))
                    {
                        URLwind = "http://www.wetterzentrale.de/pics/Rtavn" + f * 6 + "3.gif";
                        Client.DownloadFile(URLwind, "europeC" + f + ".gif");
                    }
                }

                f = 1;
                Client.DownloadFile("http://www.wetterzentrale.de/pics/Rtavn064.gif", "europeR" + f + ".gif");

                for (f = 2; f <= max; f++)
                {
                    if (!File.Exists("europeR" + f + ".gif"))
                    {
                        URLrain = "http://www.wetterzentrale.de/pics/Rtavn" + f * 6 + "4.gif";
                        Client.DownloadFile(URLrain, "europeR" + f + ".gif");
                    }
                }

                f = 1;
                Client.DownloadFile("http://www.wetterzentrale.de/pics/Rtavn068.gif", "europeW" + f + ".gif");

                for (f = 2; f <= max; f++)
                {
                    if (!File.Exists("europeW" + f + ".gif"))
                    {
                        URLwind = "http://www.wetterzentrale.de/pics/Rtavn" + f * 6 + "8.gif";
                        Client.DownloadFile(URLwind, "europeW" + f + ".gif");
                    }
                }

                f = 1;
                Client.DownloadFile("http://www.wetterzentrale.de/pics/Rtavn062.gif", "europe8" + f + ".gif");

                for (f = 2; f <= max; f++)
                {
                    if (!File.Exists("europe8" + f + ".gif"))
                    {
                        URL850t = "http://www.wetterzentrale.de/pics/Rtavn" + f * 6 + "2.gif";
                        Client.DownloadFile(URL850t, "europe8" + f + ".gif");
                    }
                }

				f = 1;
				Client.DownloadFile("http://www.wetterzentrale.de/pics/Rtavn061.gif", "europe5" + f + ".gif");

                for (f = 2; f <= max; f++)
                {
                    if (!File.Exists("europe5" + f + ".gif"))
                    {
                        URL500p = "http://www.wetterzentrale.de/pics/Rtavn" + f * 6 + "1.gif";
                        Client.DownloadFile(URL500p, "europe5" + f + ".gif");
                    }
                }

                URL500p = "http://www.wetterzentrale.de/pics/Rtavn061.gif";

				progressBar1.PerformStep();

                lon = this.longBox.Text;
                lat = this.latBox.Text;

                if (this.longBox.Text.IndexOf(".") > 0)
                    lon = this.longBox.Text.Substring(0, this.longBox.Text.IndexOf("."));
                if (this.latBox.Text.IndexOf(".") > 0)
                    lat = this.latBox.Text.Substring(0, this.latBox.Text.IndexOf("."));

                if (this.longBox.Text.IndexOf(",") > 0)
                    lon = this.longBox.Text.Substring(0, this.longBox.Text.IndexOf(","));
                if (this.latBox.Text.IndexOf(",") > 0)
                    lat = this.latBox.Text.Substring(0, this.latBox.Text.IndexOf(","));
                
                if (hour >= 0 && hour <= 6) z = "18";
                if (hour > 6 && hour <= 12) z = "00";
                if (hour > 12 && hour <= 18) z = "06";
                if (hour > 18 && hour < 24) z = "12";

				Client.DownloadFile("http://www.wetterzentrale.de/pics/MS_" + lon + lat + "eur_g05.png", "meteogramma.png");
				Client.DownloadFile("http://modeles.meteociel.fr/modeles/gens/runs/" + today + z + "/graphe3_0000___" + this.longBox.Text + "_" + this.latBox.Text + "%20_.gif", "spaghi.png");

                System.OperatingSystem osInfo = System.Environment.OSVersion;
                if (osInfo.Platform.ToString().Substring(0, 3) == "Win")
                {
                    // Load the image.
                    System.Drawing.Image image1png = System.Drawing.Image.FromFile(@"meteogramma.png");
                    System.Drawing.Image image3png = System.Drawing.Image.FromFile(@"spaghi.png");

                    // Save the image in GIF format.
                    image1png.Save(@"meteogramma.gif", System.Drawing.Imaging.ImageFormat.Gif);
                    image3png.Save(@"spaghi.gif", System.Drawing.Imaging.ImageFormat.Gif);

                    this.pictureEvo.ImageLocation = "meteogramma.gif";
                    pictureEvo.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.pictureSpaghi.ImageLocation = "spaghi.gif";
                    pictureSpaghi.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    this.pictureEvo.ImageLocation = convertToGif("meteogramma.png");
                    pictureEvo.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.pictureSpaghi.ImageLocation = convertToGif("spaghi.png");
                    pictureSpaghi.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                
				pictureSat.ImageLocation = ("europeC1.gif");
				pictureSat.SizeMode = PictureBoxSizeMode.StretchImage;

				picture850t.ImageLocation = ("europe81.gif");
				picture850t.SizeMode = PictureBoxSizeMode.StretchImage;

				picture500p.ImageLocation = ("europe51.gif");
				picture500p.SizeMode = PictureBoxSizeMode.StretchImage;
                
				pictureRain.ImageLocation = ("europeR1.gif");
				pictureRain.SizeMode = PictureBoxSizeMode.StretchImage;

				pictureWind.ImageLocation = ("europeW1.gif"); 
				pictureWind.SizeMode = PictureBoxSizeMode.StretchImage;

				progressBar1.PerformStep();
				progressBar1.Visible = false;
			}
		}

		private string convertToGif(string imgIn) 
		{
			string imgOut = imgIn.Replace (".png", ".gif");
			var info = new ProcessStartInfo();
			info.FileName = "bash";
			info.Arguments = "-c 'convert " + imgIn + " " + imgOut + "'";

			info.UseShellExecute = false;
			info.CreateNoWindow = true;

			info.RedirectStandardOutput = true;
			info.RedirectStandardError = true;

			var p = Process.Start(info);
			p.WaitForExit();

			Console.ReadLine();

			Console.WriteLine (info.Arguments);

			return imgOut;
		}

		// Pulsante per visualizzare la prima immagine
		private void fastRew_Click(object sender, EventArgs e)
		{
			int i = 1;

			pictureSat.ImageLocation = "europeC" + i + ".gif";
			pictureRain.ImageLocation = "europeR" + i + ".gif";
			pictureWind.ImageLocation = "europeW" + i + ".gif";
			picture850t.ImageLocation = "europe8" + i + ".gif";
			picture500p.ImageLocation = "europe5" + i + ".gif";
		}

		// Pulsante per visualizzare l'immagine successiva
		private void indietro_Click(object sender, EventArgs e)
		{
			string ind = "";

			if (picture850t.ImageLocation.ToString().Substring(7, 2).IndexOf('.') < 0)
			{
				ind = picture850t.ImageLocation.ToString().Substring(7, 2);
			}
			else
				ind = picture850t.ImageLocation.ToString().Substring(7, 1);

			int i = Convert.ToInt16(ind);

			if (i > 1)
				i -= 1;

			pictureSat.ImageLocation = "europeC" + i + ".gif";
			pictureRain.ImageLocation = "europeR" + i + ".gif";
			pictureWind.ImageLocation = "europeW" + i + ".gif";
			picture850t.ImageLocation = "europe8" + i + ".gif";
			picture500p.ImageLocation = "europe5" + i + ".gif";
		}

		// Pulsante per visualizzare l'immagine precedente
		private void avanti_Click(object sender, EventArgs e)
		{
			string ind = "";
			int max = 13;

			if (picture850t.ImageLocation.ToString().Substring(7,2).IndexOf('.') < 0)
			{
				ind = picture850t.ImageLocation.ToString().Substring(7, 2);
			} else                
				ind = picture850t.ImageLocation.ToString().Substring(7, 1);

			int i = Convert.ToInt16(ind);

			if (i < max)
				i += 1;

			pictureSat.ImageLocation = "europeC" + i + ".gif";
			pictureRain.ImageLocation = "europeR" + i + ".gif";
			pictureWind.ImageLocation = "europeW" + i + ".gif";
			picture850t.ImageLocation = "europe8" + i + ".gif";
			picture500p.ImageLocation = "europe5" + i + ".gif";
		}

		private void fastForw_Click(object sender, EventArgs e)
		{
			int max = 13;

			pictureSat.ImageLocation = "europeC" + max + ".gif";
			pictureRain.ImageLocation = "europeR" + max + ".gif";
			pictureWind.ImageLocation = "europeW" + max + ".gif";
			picture850t.ImageLocation = "europe8" + max + ".gif";
			picture500p.ImageLocation = "europe5" + max + ".gif";
		}

		private void eliminaFile() {

			int d, max = 13;

			//Se le immagini esistono di già le eliminiamo 
			//per far posto a quelle aggiornate            
			for (d = 1; d <= max; d++)
			{
				File.Delete("europeC" + d + ".gif");
			}

			for (d = 1; d <= max; d++)
			{
				File.Delete("europeR" + d + ".gif");
			}

			for (d = 1; d <= max; d++)
			{
				File.Delete("europeW" + d + ".gif");
			}

			for (d = 1; d <= max; d++)
			{
				File.Delete("europe8" + d + ".gif");
			}

			for (d = 1; d <= max; d++)
			{
				File.Delete("europe5" + d + ".gif");
			}

            try
            {
                File.Delete("meteogramma.png");
                File.Delete("meteogramma.gif");
                File.Delete("spaghi.png");
                File.Delete("spaghi.gif");
            }
            catch (IOException ioe)
            {
                //MessageBox.Show("Problema durante la cancellazione di un file: " + ioe.Message);
            }
		}

        private void mc_Load(object sender, EventArgs e)
        {
            Import_Click(sender, e);
            caricaPaesi();
        }

        private void caricaPaesi()
        {
            DataTable dataTable = new DataTable("record");
            dataTable.Columns.Add("comune", typeof(string));
            citySet.Tables.Add(dataTable);
            citySet.ReadXml("comuni.xml");

            cityCombo.DataSource = citySet.Tables[0];
            cityCombo.DisplayMember = "comune";
        }

        private void previsioni_Click(object sender, EventArgs e)
        {
            string paese = cityCombo.Text; //cityText.Text;
            string file0, file1 = "";
            string URLxml0, URLxml1 = "";

            using (WebClient Client = new WebClient())
            {
                // Scarico i valori meteo
                //URLxml0 = "http://api.wunderground.com/api/c29a37bdfbb58905/conditions/q/IT/" + paese + ".xml";
                URLxml1 = "https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22" + paese + "%22)&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";

				try
                {
                    //Client.DownloadFile(URLxml0, "daily0.xml");
                    Client.DownloadFile(URLxml1, "daily1.xml");
                    //file0 = "daily0.xml";
                    file1 = "daily1.xml";
                }
                catch (WebException we)
                {
                    MessageBox.Show("Problem in Downloading file " + we);
                }
                aggiornaDatiPaese(paese, file1);
                currentWeather(paese, file1);
            }
        }

        void currentWeather(string paese, string file) 
		{
			string today = String.Format("{0:yyyyMMdd}", DateTime.Today);
            int hour = DateTime.Now.Hour;
            string z = "";
            string img = "";
            
            // Aggiorniamo le immagini 
			using (WebClient Client = new WebClient ()) 
			{
                this.errorLbl.Text = "";
				string lon = this.longBox.Text;
				string lat = this.latBox.Text;

                if (this.longBox.Text.IndexOf(".") > 0)
                    lon = this.longBox.Text.Substring(0, this.longBox.Text.IndexOf("."));
                if (this.latBox.Text.IndexOf(".") > 0)
                    lat = this.latBox.Text.Substring(0, this.latBox.Text.IndexOf("."));

				if (this.longBox.Text.IndexOf(",") > 0 )
					lon = this.longBox.Text.Substring (0, this.longBox.Text.IndexOf (","));
				if (this.latBox.Text.IndexOf(",") > 0 )
					lat = this.latBox.Text.Substring (0, this.latBox.Text.IndexOf (","));

                if (hour >= 0 && hour <= 6) z = "18";
                if (hour > 6 && hour <= 12) z = "00";
                if (hour > 12 && hour <= 18) z = "06";
                if (hour > 18 && hour < 24) z = "12";
                
                try
                {
                    Client.DownloadFile("http://www.wetterzentrale.de/pics/MS_" + lon + lat + "eur_g05.png", "meteogramma.png");
					Client.DownloadFile("http://modeles.meteociel.fr/modeles/gens/runs/" + today + z + "/graphe3_0000___" + this.longBox.Text + "_" + this.latBox.Text + "%20_.gif", "spaghi.png");

                    System.OperatingSystem osInfo = System.Environment.OSVersion;
                    if (osInfo.Platform.ToString().Substring(0, 3) == "Win")
                    {
                        // Load the image.
                        System.Drawing.Image image1png = System.Drawing.Image.FromFile(@"meteogramma.png");
                        System.Drawing.Image image3png = System.Drawing.Image.FromFile(@"spaghi.png");

                        // Save the image in GIF format.
                        image1png.Save(@"meteogramma.gif", System.Drawing.Imaging.ImageFormat.Gif);
                        image3png.Save(@"spaghi.gif", System.Drawing.Imaging.ImageFormat.Gif);

                        this.pictureEvo.ImageLocation = "meteogramma.gif";
                        this.pictureSpaghi.ImageLocation = "spaghi.gif";
                    }
                    else
                    {
                        this.pictureEvo.ImageLocation = convertToGif("meteogramma.png");
                        this.pictureSpaghi.ImageLocation = convertToGif("spaghi.png");
                    }
                }
                catch (WebException we)
                {
                    //MessageBox.Show("Web Problem: " + we.Message);
                    //MessageBox.Show("La città non rientra \n nell'elenco delle città rintracciabili \n" +
                    //                    " per cui la situazione attuale è attendibile, \n" +
                    //                    " ma i tab Andamento e Spaghi non sono coerenti: \n" + 
                    //                    " se vuoi coerenza nelle previsioni \n" + 
                    //                    " digita il nome di un'altra città! ;-)");
                    this.errorLbl.Text = "Dati citta' attualmente non disponibili: riprovare in un secondo momento.";
                    this.pictureEvo.ImageLocation = "";
                    this.pictureSpaghi.ImageLocation = "";
                    this.picture850t.Focus();
                }
			}

            // Parser del file xml dei valori meteo
            // e scrittura dei valori nei campi corrispondenti
            try
			{
                int size = -1;
                string text = File.ReadAllText(file);
                size = text.Length;

                XmlTextReader reader = new XmlTextReader("daily1.xml");
                reader.Read();

                //while (!reader.EOF && reader.Name != "temp_c")
                //    reader.Read();
                //tempMaxBox0.Text = "Temperatura: " + reader.ReadString() + " °C" + "\n";
                
                while (!reader.EOF && reader.Name != "yweather:wind")
                    reader.Read();
                reader.MoveToContent();
                windDirBox0.Text = "Direzione: " + winDirection(Convert.ToInt32(reader.GetAttribute("direction")));

                while (!reader.EOF && reader.Name != "yweather:wind")
                    reader.Read();
                reader.MoveToContent();
                windVelBox0.Text = "Velocita': " + Convert.ToInt32(reader.GetAttribute("speed").ToString()) * 1.609 + " Km/h" + "\n";

                while (!reader.EOF && reader.Name != "yweather:atmosphere")
                    reader.Read();
                reader.MoveToContent();
                humidityBox0.Text = "Umidita': " + reader.GetAttribute("humidity") + " %" + "\n";

                while (!reader.EOF && reader.Name != "yweather:atmosphere")
                    reader.Read();
                reader.MoveToContent();
                pressureBox0.Text = "Pressione': " + Convert.ToInt32(reader.GetAttribute("pressure").ToString().Replace(".","")) * 0.3386  + " hPa" + "\n";

                while (!reader.EOF && reader.Name != "yweather:condition")
                    reader.Read();
                reader.MoveToContent();
                tempMinBox0.Text = "Temperatura: " + (Convert.ToInt32(reader.GetAttribute("temp").ToString()) - 32) * 5 / 9 + " °C" + "\n";

                while (!reader.EOF && reader.Name != "yweather:condition")
                    reader.Read();
                reader.MoveToContent();
                skyBox0.Text = "Copertura: " + reader.GetAttribute("text") + "\n";
                
                //while (!reader.EOF && reader.Name != "precip_today_metric")
                //    reader.Read();
                //precipitationBox0.Text = "Precipitazioni: " + reader.ReadString() + " mm\n";

                //while (!reader.EOF && reader.Name != "description")
                //    reader.Read();
                ////reader.MoveToContent();
                //precipitationBox0.Text = "img:" + reader.ReadString();
                ////img = reader.GetAttribute("src");

                ////pictureBox1.ImageLocation = img;

                reader.Close();
            }
            catch (IOException ioe)
            {
                MessageBox.Show("Problem in opening file Current Weather" + ioe);
            }
            catch (XmlException xmle)
            {
                MessageBox.Show("Problem in opening Xml file Current Weather" + xmle);
            }
            catch (NullReferenceException ne)
            {
                MessageBox.Show("Problem in reading Xml variable" + ne);
            }
        }

        string winDirection(int deg)
        {
            string dir = "";
            if (deg > 337.5 || deg <= 22.5) dir = "N";
            if (deg > 22.5 && deg <= 67.5) dir = "NE";
            if (deg > 67.5 && deg <= 112.5) dir = "E";
            if (deg > 112.5 && deg <= 157.5) dir = "SE";
            if (deg > 157.5 && deg <= 202.5) dir = "S";
            if (deg > 202.5 && deg <= 247.5) dir = "SW";
            if (deg > 247.5 && deg <= 292.5) dir = "W";
            if (deg > 292.5 && deg <= 337.5) dir = "NW";

            return dir;
        }

        void aggiornaDatiPaese(string paese, string file)
        {
            // Parser del file xml dei valori meteo
            // e scrittura dei valori nei campi corrispondenti
            try
            {
                int size = -1;
                string text = File.ReadAllText(file);
                size = text.Length;
                
                XmlTextReader reader = new XmlTextReader(file);
                reader.Read();

                while (!reader.EOF && reader.Name != "title")
                    reader.Read();
                cityBox.Text = reader.ReadString().Substring(17);

                while (!reader.EOF && reader.Name != "geo:lat")
                    reader.Read();
                this.latBox.Text = reader.ReadString();
                infoBox.Text = "Latitudine:     " + this.latBox.Text + "\n";

                while (!reader.EOF && reader.Name != "geo:long")
                    reader.Read();
                this.longBox.Text = reader.ReadString();
                infoBox.Text += "Longitudine:     " + this.longBox.Text + "\n";

                //while (!reader.EOF && reader.Name != "elevation")
                //    reader.Read();
                //infoBox.Text += "Altitudine:     " + reader.ReadString();

                reader.Close();
            }
            catch (IOException)
            {
                MessageBox.Show("Problem in opening file on Future Weather");
            }
        }
        
        private void mc_OnClose(object sender, FormClosingEventArgs e)
        {
            eliminaFile();
        } 

		private void makePDF_Click(object sender, EventArgs e)
		{
            //PdfDocument pdf = new PdfDocument();
            //PdfPage pdfPage = pdf.AddPage();
            //XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            //XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

            //graph.DrawString("Meteo Casarano", font, XBrushes.Black,
            //    new XRect(40, 30, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
            //graph.DrawString("---------------------", font, XBrushes.Black,
            //    new XRect(40, 40, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

            //XImage imgPres = XImage.FromFile(pictureEvo.ImageLocation);
            //XImage imgRain = XImage.FromFile(pictureRain.ImageLocation);
            //XImage imgWind = XImage.FromFile(pictureWind.ImageLocation);
            //XImage img850t = XImage.FromFile(picture850t.ImageLocation);
            //XImage img500p = XImage.FromFile(picture500p.ImageLocation);
            //graph.DrawImage(imgPres, 50, 70, 500, 333);
            //graph.DrawImage(imgRain, 50, 410, 500, 333);

            //PdfPage pdfPage2 = pdf.AddPage();
            //XGraphics graph2 = XGraphics.FromPdfPage(pdfPage2);
            //graph2.DrawImage(imgWind, 50, 70, 500, 333);
            //graph2.DrawImage(img850t, 50, 410, 500, 333);

            //PdfPage pdfPage3 = pdf.AddPage();
            //XGraphics graph3 = XGraphics.FromPdfPage(pdfPage3);
            //graph3.DrawImage(img500p, 50, 70, 500, 333);

            //pdf.Save("firstpage.pdf");
            //Process.Start("firstpage.pdf");
            //pdf.Close();

		}
    }
}