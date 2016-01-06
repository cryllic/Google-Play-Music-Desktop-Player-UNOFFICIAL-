﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Google_Play_Music
{
    public class SongAlert : Form
    {
        public Timer timer = new Timer();
        public int currentStep;

        public SongAlert(string song, string artist, string album, string url)
        {
            BackColor = Color.FromArgb(240, 240, 240);
            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(390, 70);
            Opacity = 0;
            ShowInTaskbar = false;
            Load += new EventHandler(Alert_Loaded);

            PictureBox albumArt = new PictureBox();
            try
            {
                albumArt.Load(url);
            } catch (Exception)
            {
                // Ignore it
            };
            albumArt.Size = new Size(70, 70);
            albumArt.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(albumArt);

            int n = 15;
            Font titleFont = new Font("Calibri", n, FontStyle.Regular);
            while (TextRenderer.MeasureText(song, titleFont).Width > 312)
            {
                if (n < 6)
                {
                    break;
                }
                titleFont = new Font("Calibri", n--, FontStyle.Regular);
            }

            Label songTitle = makeLabel(song);
            songTitle.Location = new Point(78, 8);
            songTitle.Font = titleFont;
            Controls.Add(songTitle);

            if (n > 12)
            {
                n = 12;
            }
            Font infoFont = new Font("Calibri", n, FontStyle.Regular);
            while (TextRenderer.MeasureText(artist + " - " + album, infoFont).Width > 312 * 2)
            {
                if (n < 6)
                {
                    break;
                }
                infoFont = new Font("Calibri", n--, FontStyle.Regular);
            }

            Label songInfo = makeLabel(artist + " - " + album);
            songInfo.Location = new Point(78, 32);
            if (TextRenderer.MeasureText(artist + " - " + album, infoFont).Width <= 312)
            {
                songInfo.Location = new Point(78, 38);
            }
            songInfo.AutoSize = false;
            songInfo.Size = new Size(312, 40);
            songInfo.Font = infoFont;
            Controls.Add(songInfo);
        }

        private Label makeLabel(string text)
        {
            Label lab = new Label();
            lab.Text = text;
            lab.Width = 312;
            lab.ForeColor = Color.FromArgb(30, 30, 30);
            return lab;
        }

        public bool loaded = false;

        private void Alert_Loaded(object sender, EventArgs e)
        {
            loaded = true;
            timer.Interval = 10;

            currentStep = 0;
            int fadeSteps = 25;
            int totalSteps = 200;
            timer.Tick += (arg1, arg2) =>
            {
                if (IsDisposed)
                {
                    this.Close();
                    timer.Stop();
                    timer.Dispose();
                    return;
                }
                currentStep++;
                if (currentStep == 1)
                {
                    Point loc = Screen.PrimaryScreen.WorkingArea.Location;
                    int X = loc.X + Screen.PrimaryScreen.WorkingArea.Width - Size.Width - 16;
                    int Y = loc.Y + Screen.PrimaryScreen.WorkingArea.Height - Size.Height - 16;
                    Location = new Point(X, Y);
                }
                if (currentStep <= fadeSteps)
                {
                    Opacity = ((double)currentStep) / fadeSteps;
                } else if (currentStep <= totalSteps - fadeSteps)
                {
                    // Do something
                } else if (currentStep <= totalSteps)
                {
                    Opacity = ((double)(totalSteps - currentStep) / fadeSteps);
                } else
                {
                    this.Close();
                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.Start();
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private const int WS_EX_TOPMOST = 0x00000008;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_TOPMOST;
                return createParams;
            }
        }
    }
}
