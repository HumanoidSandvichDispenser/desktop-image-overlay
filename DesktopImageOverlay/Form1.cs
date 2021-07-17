using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace DesktopImageOverlay
{
    public partial class Form1 : Form
    {
        private Overlay currentOverlay = null;

        public Overlay CurrentOverlay
        {
            get
            {
                if (currentOverlay == null)
                    currentOverlay = new Overlay();

                return currentOverlay;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (files.Length == 1 && files[0].EndsWith(".png"))
            {
                CurrentOverlay.Show();
                CurrentOverlay.SetImage(files[0], trackBar1.Value / 100f);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                CurrentOverlay.Show();
                CurrentOverlay.SetImage(Clipboard.GetImage(), trackBar1.Value / 100f);
            }
            else
            {
                MessageBox.Show("no image found in clipboard");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentOverlay.Visible)
            {
                CurrentOverlay.SetOverlay(checkBox1.Checked);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = $"Scale: {trackBar1.Value}%";
            CurrentOverlay.SetScale(trackBar1.Value / 100f);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CurrentOverlay.Visible = !CurrentOverlay.Visible;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label2.Text = $"Opacity: {trackBar2.Value}";
            CurrentOverlay.Opacity = trackBar2.Value / 100.0;
        }
    }
}
