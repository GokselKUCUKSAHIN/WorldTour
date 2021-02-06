using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace WorldTour
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<City> cities = new List<City>();

        private Random random = new Random();

        private static readonly string defaultSaveLocation = @"data/save.sav";
        private static readonly string dataURL = @"https://raw.githubusercontent.com/GokselKUCUKSAHIN/WorldTour/main/WorldTour/bin/Debug/data/save.sav";

        private void CheckFiles()
        {
            if (!Directory.Exists(@"data"))
            {
                Directory.CreateDirectory(@"data");
            }
            if (!File.Exists(@"data/save.sav"))
            {
                FirstTime();
            }
            LoadData();
        }

        private void LoadData()
        {
            LoadData(defaultSaveLocation);
        }

        private void LoadData(string path)
        {
            if (path != null && !path.Equals(String.Empty))
            {
                try
                {
                    using (var rdr = new StreamReader(path))
                    {
                        while (!rdr.EndOfStream)
                        {
                            string line = rdr.ReadLine().Trim();
                            if (!line.Equals(String.Empty))
                            {
                                var sep = line.Split(';');
                                this.cities.Add(new City(sep[0], sep[1], sep[2]));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bekleyenmen dosya okuma hatası. Hata mesajı: " + ex.Message, "HATA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //MessageBox.Show(this.cities.Count + "");
        }

        private void FirstTime()
        {
            try
            {
                var response = new WebClient().DownloadString(dataURL);
                using (var wrtr = new StreamWriter(defaultSaveLocation, false))
                {
                    wrtr.Write(response);
                }
            }
            catch
            {
                MessageBox.Show("Sorun!!!");
            }
        }

        private void SaveData()
        {
            SaveData(defaultSaveLocation);
        }

        private void SaveData(string path)
        {
            string data = DumpData();
            try
            {
                using (var wrtr = new StreamWriter(path, false))
                {
                    wrtr.Write(data);
                }
            }
            catch
            {
                //
            }
        }

        private string DumpData()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < cities.Count; i++)
            {
                City temp = cities[i];
                sb.Append(String.Format("{0};{1};{2}{3}", temp.getName(), temp.getCountry(), temp.GetOccurrence(), Environment.NewLine));
            }
            return sb.ToString();
        }

        private int GetRandomNumber()
        {
            return random.Next(0, this.cities.Count);
        }

        private City GetRandomCity()
        {
            int randomNumber = GetRandomNumber();
            City randomCity = cities[randomNumber];
            randomCity.increaseOccurrence();
            return randomCity;
        }

        private string CommentLabel(int times)
        {
            if (times == 1)
                return "İlk gidişiniz";
            else if (times >= 2 && times < 299)
                return times.ToString() + ". gidişiniz";
            else
                return times.ToString() + ". gidişiniz Yuh amk!";
        }


        private void UpdateLabels(City city)
        {
            labelCity.Text = city.getName();
            labelCountry.Text = city.getCountry();
            labelOccurance.Text = CommentLabel(city.GetOccurrence());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckFiles();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData();
        }

        private void buttonNext_MouseClick(object sender, MouseEventArgs e)
        {
            UpdateLabels(GetRandomCity());
            timer.Enabled = true;
            timer.Start();
            buttonNext.Enabled = false;
            delay = 20;
        }

        private void buttonReset_MouseClick(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("Önceki gidilen yerler sıfırlanacak. Sıfırlama yapmak istiyor musunuz?",
            "Emin misiniz?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (City city in cities)
                {
                    city.resetOccurrence();
                }
            }
        }

        private int delay = 20;

        private void timer_Tick(object sender, EventArgs e)
        {
            if (delay > 0)
            {
                // "Bir Sonraki Tıklama " + (delay--) / 10.0 + "s"
                buttonNext.Text = String.Format("Bir Sonraki Tıklama {0:0.0}s", (delay--) / 10.0);
            }
            else
            {
                delay = 0;
                buttonNext.Enabled = true;
                this.timer.Stop();
                this.timer.Enabled = false;
                buttonNext.Text = "Sıradaki";
            }
        }
    }
}
