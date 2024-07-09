using System;
using System.Drawing;
using System.Windows.Forms;

namespace CanvasDraw
{
    public partial class Form1 : Form
    {
        private bool _isDrawing;
        private Point _startPoint;
        private Point _endPoint;
        private Graphics _graphics;
        private Bitmap _bitmap;
        private string _drawMode;

        public Form1()
        {
            InitializeComponent();
            _isDrawing = false;
            _bitmap = new Bitmap(panelCanvas.Width, panelCanvas.Height);
            _graphics = Graphics.FromImage(_bitmap);
            panelCanvas.BackgroundImage = _bitmap;
            _drawMode = "Sketch"; // Default draw mode
        }

        private void panelCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDrawing = true;
                _startPoint = e.Location;
                if (_drawMode == "Sketch")
                {
                    _endPoint = e.Location;
                }
            }
        }

        private void panelCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                _endPoint = e.Location;
                if (_drawMode == "Sketch")
                {
                    _graphics.DrawLine(Pens.Black, _startPoint, _endPoint);
                    _startPoint = _endPoint;
                    panelCanvas.Invalidate();
                }
                else
                {
                    panelCanvas.Invalidate();
                }
            }
        }

        private void panelCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDrawing = false;
                if (_drawMode != "Sketch")
                {
                    DrawShape(_graphics, _startPoint, _endPoint);
                    panelCanvas.Invalidate(); // Refresh the panel to show the drawing
                }
            }
        }

        private void panelCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (_isDrawing && _drawMode != "Sketch")
            {
                DrawShape(e.Graphics, _startPoint, _endPoint);
            }
        }

        private void DrawShape(Graphics g, Point start, Point end)
        {
            switch (_drawMode)
            {
                case "Line":
                    g.DrawLine(Pens.Black, start, end);
                    break;
                case "Rectangle":
                    g.DrawRectangle(Pens.Black, GetRectangle(start, end));
                    break;
                case "Ellipse":
                    g.DrawEllipse(Pens.Black, GetRectangle(start, end));
                    break;
            }
        }

        private Rectangle GetRectangle(Point start, Point end)
        {
            return new Rectangle(
                Math.Min(start.X, end.X),
                Math.Min(start.Y, end.Y),
                Math.Abs(start.X - end.X),
                Math.Abs(start.Y - end.Y));
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _graphics.Clear(Color.White);
            panelCanvas.Invalidate(); // Refresh the panel to clear the drawing
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSketch.Checked)
                _drawMode = "Sketch";
            else if (radioButtonLine.Checked)
                _drawMode = "Line";
            else if (radioButtonRectangle.Checked)
                _drawMode = "Rectangle";
            else if (radioButtonEllipse.Checked)
                _drawMode = "Ellipse";
        }
    }
}
