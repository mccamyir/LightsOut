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
        private const int NumCells = 3; // Number of cells in grid
        private const int CellLength = GridLength / NumCells;
        private bool[,] grid; // Stores on/off state of cells in grid
        private Random rand; // Used to generate random numbers

        public MainForm()
        {
            InitializeComponent();
            rand = new Random(); // Initializes random number generator
            grid = new bool[NumCells, NumCells];
            // Turn entire grid on
            for (int r = 0; r < NumCells; r++)
                for (int c = 0; c < NumCells; c++)
                    grid[r, c] = true;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int r = 0; r < NumCells; r++)
            {
                for (int c = 0; c < NumCells; c++)
                {
                    // Get proper pen and brush for on/off
                    // grid section
                    Brush brush;
                    Pen pen;
                    if (grid[r, c])
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

        private bool PlayerWon()
        {
            bool OnBlack = true;
            for (int r = 0; r < NumCells && OnBlack==true; r++)
                for (int c = 0; c < NumCells && OnBlack==true; c++)
                {
                    if(grid[r,c]==true)
                    {
                        OnBlack=false;
                    }
                }
            return OnBlack;
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Make sure click was inside the grid
            if (e.X < GridOffset || e.X > CellLength * NumCells + GridOffset ||
            e.Y < GridOffset || e.Y > CellLength * NumCells + GridOffset)
                return;
            // Find row, col of mouse press
            int r = (e.Y - GridOffset) / CellLength;
            int c = (e.X - GridOffset) / CellLength;
            // Invert selected box and all surrounding boxes
            for (int i = r - 1; i <= r + 1; i++)
                for (int j = c - 1; j <= c + 1; j++)
                    if (i >= 0 && i < NumCells && j >= 0 && j < NumCells)
                        grid[i, j] = !grid[i, j];
            // Redraw grid
            this.Invalidate();
            // Check to see if puzzle has been solved
            if (PlayerWon())
            {
                // Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Fill grid with either white or black
            for (int r = 0; r < NumCells; r++)
                for (int c = 0; c < NumCells; c++)
                    grid[r, c] = rand.Next(2) == 1;
            // Redraw grid
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
