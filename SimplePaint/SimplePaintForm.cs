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
        }
    }
}
