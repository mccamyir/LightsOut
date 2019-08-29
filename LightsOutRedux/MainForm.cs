using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOutRedux
{
    public partial class MainForm : Form
    {
        private const int GridOffset = 25; // Distance from upper-left side of window
        private const int GridLength = 200; // Size in pixels of grid
        private LightsOutGame game;

        public MainForm()
        {
            InitializeComponent();
            game = new LightsOutGame();
            game.NewGame();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            int CellLength = GridLength / game.GridSize;
            int NumCells = game.GridSize;
            Graphics g = e.Graphics;
            for (int r = 0; r < NumCells; r++)
            {
                for (int c = 0; c < NumCells; c++)
                {
                    // Get proper pen and brush for on/off
                    // grid section
                    Brush brush;
                    Pen pen;
                    if (game.GetGridValue(r, c))
                    {
                        pen = Pens.Black;
                        brush = Brushes.White; // On
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black; // Off
                    }
                    // Determine (x,y) coord of row and col to draw rectangle
                    int x = c * CellLength + GridOffset;
                    int y = r * CellLength + GridOffset;
                    // Draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, CellLength, CellLength);
                    g.FillRectangle(brush, x + 1, y + 1, CellLength - 1, CellLength - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            int CellLength = GridLength / game.GridSize;
            // Make sure click was inside the grid
            if (e.X < GridOffset || e.X > CellLength * game.GridSize + GridOffset ||
            e.Y < GridOffset || e.Y > CellLength * game.GridSize + GridOffset)
                return;
            // Find row, col of mouse press
            int r = (e.Y - GridOffset) / CellLength;
            int c = (e.X - GridOffset) / CellLength;
            // Invert selected box and all surrounding boxes
            game.Move(r, c);
            // Redraw grid
            this.Invalidate();
            // Check to see if puzzle has been solved
            if (game.IsGameOver())
            {
                // Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            game.NewGame();
            this.Invalidate();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutBox = new About();
            aboutBox.ShowDialog(this);
        }
    }
}
