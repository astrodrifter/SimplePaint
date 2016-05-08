using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D; // For LineCap.
using System.IO;                // For Directory and Path.
using System.Drawing.Imaging;   // For ImageFormat.
using System.Diagnostics;       // For Debug.
/// <summary>
/// a change
/// </summary>

namespace SimplePaint {
    public partial class SimplePaintForm : Form {
        Bitmap initialBitmap;
        Graphics graphics;
        bool userIsDrawing = false;
        Point startPoint;
        Pen pen = new Pen(Color.Fuchsia, 3);  // Pen Width is 3, initially.
        bool imageIsModified;
        string currentFilePath;  /* Is empty, when the current image hasn't been opened or saved. */


        public SimplePaintForm() {
            InitializeComponent();
            InitializeWidthBox();
            New();
            // Make the ends of each line rounded.
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;

        }
        public void New() {
            currentFilePath = "";
            Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            ResetImageStatus();
            SetImage(bitmap);
        }

        private void ResetImageStatus() {
            imageIsModified = false;
        }

        private void SetImage(Bitmap bitmap) {
            initialBitmap = bitmap;
            Bitmap currentBitmap = (Bitmap)initialBitmap.Clone();
            pictureBox.Image = currentBitmap;
            graphics = Graphics.FromImage(currentBitmap);
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                userIsDrawing = true;
                startPoint = new Point(e.X, e.Y);
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e) {
            if (userIsDrawing) {
                Point endPoint = new Point(e.X, e.Y);
                graphics.DrawLine(pen, startPoint, endPoint);
                imageIsModified = true;
                pictureBox.Refresh();  // Repaint the PictureBox.
                startPoint = endPoint;  /* Set it for the next time we're called. */
            }


        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e) {
            userIsDrawing = false;

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            // Set the colorDialog's initial color to the current pen color.
            colorDialog.Color = pen.Color;

            /* Show the colorDialog and update the pen’s color if the user clicks OK. */
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                pen.Color = colorDialog.Color;
            }

        }

        private void widthBox_SelectedIndexChanged(object sender, EventArgs e) {
           pen.Width = (int)widthBox.SelectedItem;

        }

        private void InitializeWidthBox() {
            // Create some pen widths for the user. 
            for (int i = 1; i < 10; ++i) {
                widthBox.Items.Add(i);
            }
            for (int i = 10; i <= 100; i += 10) {
                widthBox.Items.Add(i);
            }
        }


    }
}
