using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mäumäng
{
    public partial class Form1 : Form
    {
        bool allowClick = false;
        PictureBox firstGuess;
        Random rnd = new Random();
        System.Windows.Forms.Timer clickTimer = new System.Windows.Forms.Timer();
        int time = 60;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 1000 };


        public Form1()
        {
            InitializeComponent();
            HideImages();
        }

        private PictureBox[] pictureBoxes
        {
            get { return Controls.OfType<PictureBox>().ToArray(); }
        }

        private static IEnumerable<Image> images => new Image[]
                {
                    Mälumäng.Resource1.img1,
                    Mälumäng.Resource1.img2,
                    Mälumäng.Resource1.img3,
                    Mälumäng.Resource1.img4,
                    Mälumäng.Resource1.img5,
                    Mälumäng.Resource1.img6,
                    Mälumäng.Resource1.img7,
                    Mälumäng.Resource1.img8
                };

        //taimer alustab ja näitab allesjäänud aega
        private void startGameTimer()
        {
            timer.Start();
            timer.Tick += delegate
            {
                time--;
                if (time < 0)
                {
                    timer.Stop();
                    DialogResult dialogResult = MessageBox.Show("Aeg sai täis! Kas mängime uuesti?", "Kaotasid", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ResetImages();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        Close();
                    }
                }

                var ssTime = TimeSpan.FromSeconds(time);

                lblAeg.Text = "00: " + time.ToString();
            };
        }

        //uue mängu alustamisel piltide asukohad muutuvad
        private void ResetImages()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Tag = null;
                pic.Visible = true;
            }

            HideImages();
            setRandomImages();
            time = 60;
            timer.Start();
        }

        //tagurpidi kaart
        private void HideImages()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Image = Mälumäng.Resource1.tagune;
            }
        }

        private PictureBox getFreeSlot()
        {
            int num;

            do
            {
                num = rnd.Next(0, pictureBoxes.Count());
            }
            while (pictureBoxes[num].Tag != null);
            return pictureBoxes[num];
        }

        private void setRandomImages()
        {
            foreach (var image in images)
            {
                getFreeSlot().Tag = image;
                getFreeSlot().Tag = image;
            }
        }

        private void CLICKTIMER_TICK(object sender, EventArgs e)
        {
            HideImages();

            allowClick = true;
            clickTimer.Stop();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!allowClick) return;

            var pic = (PictureBox)sender;

            if (firstGuess == null)
            {
                firstGuess = pic;
                pic.Image = (Image)pic.Tag;
                return;
            }

            pic.Image = (Image)pic.Tag;

            if (pic.Image == firstGuess.Image && pic != firstGuess)
            {
                pic.Visible = firstGuess.Visible = false;
                {
                    firstGuess = pic;
                }
                HideImages();
            }
            else
            {
                allowClick = false;
                clickTimer.Start();
            }

            firstGuess = null;
            if (pictureBoxes.Any(p => p.Visible)) return;

            DialogResult dialogResult = MessageBox.Show("Sa võitsid! Kas mängime uuesti?", "Tubli!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ResetImages();
            }
            else if (dialogResult == DialogResult.No)
            {
                Close();
            }
        }

        private void btnAlusta_Click(object sender, EventArgs e)
        {
            allowClick = true;
            setRandomImages();
            HideImages();
            startGameTimer();
            clickTimer.Interval = 1000;
            clickTimer.Tick += CLICKTIMER_TICK;
            btnAlusta.Enabled = false;
        }

        private void UpdateForm()
        {
            InitializeComponent();
            HideImages();
            btnAlusta.Enabled = true;
        }
    }
}