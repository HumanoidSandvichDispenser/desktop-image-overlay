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
using System.Runtime.InteropServices;

namespace DesktopImageOverlay
{
    public partial class Overlay : Form
    {
        public enum GWL
        {
            ExStyle = -20
        }

        public enum WS_EX
        {
            Layered = 0x80000,
            Transparent = 0x20
        }

        public enum LWA
        {
            ColorKey = 1,
            Alpha = 2
        }

        private int initialStyle;

        private bool isDragged = false;
        private int offsetX = 0;
        private int offsetY = 0;

        private bool isAltDown = false;

        public Overlay()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public void SetImage(string path, float scale)
        {
            Image image = Image.FromFile(path);
            SetImage(image, scale);
        }
        public void SetImage(Image image, float scale)
        {
            PictureBox.Image = image;
            SetScale(scale);
        }

        public void SetScale(float scale)
        {
            Image image = PictureBox.Image;
            Width = PictureBox.Width = (int)(image.Width * scale);
            Height = PictureBox.Height = (int)(image.Height * scale);
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            isDragged = true;

            offsetX = e.X;
            offsetY = e.Y;
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDragged = false;
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                Left += e.X - offsetX;
                Top += e.Y - offsetY;
            }
        }

        private void Overlay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                isAltDown = true;
            }
        }

        private void Overlay_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                isAltDown = false;
            }
        }

        private void Overlay_Load(object sender, EventArgs e)
        {
            initialStyle = GetWindowLong(Handle, -20);

            SetOverlay(true);
        }

        public void SetOverlay(bool overlay)
        {
            if (overlay)
            {
                SetWindowLong(Handle, -20, initialStyle | 0x80000 | 0x20);
            }
            else
            {
                SetWindowLong(Handle, -20, initialStyle);
            }
        }
    }
}
