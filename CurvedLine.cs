using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;           // For Pen.
using System.Drawing.Drawing2D; // For GraphicsPath.

namespace SimplePaint {
    class CurvedLine {
        GraphicsPath graphicsPath = new GraphicsPath();
        Pen pen;

        public CurvedLine (Pen pen) {
            this.pen = (Pen)pen.Clone();
            // Make the internal ends of each line rounded.
            pen.LineJoin = LineJoin.Round;
        }

        public void Add(Point startPoint, Point endPoint) {
            graphicsPath.AddLine(startPoint, endPoint);
        }

        public void Draw(Graphics graphics) {
            graphics.DrawPath(pen, graphicsPath);
        }
    }
}
