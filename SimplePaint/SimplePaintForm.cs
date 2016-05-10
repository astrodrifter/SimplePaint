﻿using System;
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

namespace SimplePaint {
    public partial class SimplePaintForm : Form {
        Bitmap initialBitmap;
        Graphics graphics;
        bool userIsDrawing = false;
        Point startPoint;
        Pen pen = new Pen(Color.Fuchsia, 3);  // Pen Width is 3, initially.
        bool imageIsModified;
        string currentFilePath;  /* Is empty, when the current image hasn't been opened or saved. */

        string currentFolder = Directory.GetCurrentDirectory();
        const string SAVE_FILE_FILTER =
            "BMP (*.bmp)|*.bmp" +
            "|GIF (*.gif)|*.gif" +
            "|JPEG (*.jpg;*.jif;*.jpeg)|*.jpg;*.jif;*.jpeg" +
            "|PNG (*.png)|*.png";
        const string OPEN_FILE_FILTER =
            "All image types (*.bmp *.gif *.jpg *.png)|*.bmp;*.gif;*.jpg;*.png|" +
            SAVE_FILE_FILTER;


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

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            New(); //creates new file
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            Open();
        }

        private void Open() {
            openFileDialog.InitialDirectory = currentFolder;
            openFileDialog.Filter = OPEN_FILE_FILTER;
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                currentFilePath = openFileDialog.FileName;
                SetCurrentFolder();
                ResetImageStatus();
                try {
                    /* Load the image. This locks the file -- preventing us from 
                       saving directly to it -- so we have to copy the image. */
                    Image imageFromFile = Image.FromFile(currentFilePath);
                    Bitmap newImage = new Bitmap(imageFromFile);
                    // Close the original image, unlocking the file.
                    imageFromFile.Dispose();
                    // Display the new image.
                    SetImage(newImage);
                } catch (Exception) {
                    MessageBox.Show("An error occurred when loading the image. Please try again with another image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void SetCurrentFolder() {
            currentFolder = Path.GetDirectoryName(currentFilePath);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveAs();
        }

        private void SaveAs() {
            saveFileDialog.InitialDirectory = currentFolder;
            saveFileDialog.Filter = SAVE_FILE_FILTER;

            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                currentFilePath = saveFileDialog.FileName;
                SetCurrentFolder();
                ImageFormat imageFormat;

                switch (saveFileDialog.FilterIndex) {
                    case 1:
                        imageFormat = ImageFormat.Bmp;
                        break;

                    case 2:
                        imageFormat = ImageFormat.Gif;
                        break;

                    case 3:
                        imageFormat = ImageFormat.Jpeg;
                        break;

                    case 4:
                        imageFormat = ImageFormat.Png;
                        break;
                    default:
                        imageFormat = null;
                        break;
                }
                pictureBox.Image.Save(currentFilePath, imageFormat);
                imageIsModified = false;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            Save();
        }
        private void Save() {
            // We only save when the image is modified.
            if (imageIsModified) {
                // If it's a new image
                if (currentFilePath == "") {
                    SaveAs();
                } else {
                    pictureBox.Image.Save(currentFilePath);
                    imageIsModified = false;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
