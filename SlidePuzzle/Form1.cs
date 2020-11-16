using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SlidePuzzle
{

    public partial class Form1 : Form
    {
        private bool display_all_pieces = false;
        private String appPath = Application.StartupPath;
        private bool display_hint = false;
        private string imageFileName;
        private int numberOfColsRows = 3;
        private bool isImageClicked = false;
        private int imageClicked;
        private int movesToSimulate = 500;
        private PuzzlePiece[] PuzzlePieces = new PuzzlePiece[37];
        MainMenu MyMenu;

        public Form1()
        {
            // Create a main menu object. 
            MyMenu = new MainMenu();

            // Create Separator object
  
            // Add top-level menu items to the menu. 
            MenuItem mnuFile = new MenuItem("File");
            mnuFile.Tag = "FILE";
            MyMenu.MenuItems.Add(mnuFile);

            // Create File submenu 
            MenuItem submShuffle = new MenuItem("Shuffle");
            submShuffle.Tag = "SHUFFLE";
            mnuFile.MenuItems.Add(submShuffle);
            MenuItem submPuzzleSize = new MenuItem("Puzzle Size");
            submPuzzleSize.Tag = "PUZZLE_SIZE";
            mnuFile.MenuItems.Add(submPuzzleSize);
            MenuItem submPuzzleImage = new MenuItem("Puzzle Image");
            submPuzzleImage.Tag = "PUZZLE_IMAGE";
            mnuFile.MenuItems.Add(submPuzzleImage);
            MenuItem submDisplayHints = new MenuItem("Display Hints");
            submDisplayHints.Tag = "DISPLAY_HINTS";
            mnuFile.MenuItems.Add(submDisplayHints);
            if (display_hint) { submDisplayHints.Checked = true; }
            mnuFile.MenuItems.Add("-");
            MenuItem submExit = new MenuItem("Exit");
            mnuFile.MenuItems.Add(submExit);

            // Create Puzzle Size Submenu 
            MenuItem submPuzzleSize2 = new MenuItem("2x2");
            submPuzzleSize2.Tag = "2";
            submPuzzleSize.MenuItems.Add(submPuzzleSize2);
            MenuItem submPuzzleSize3 = new MenuItem("3x3");
            submPuzzleSize3.Tag = "3";
            submPuzzleSize.MenuItems.Add(submPuzzleSize3);
            MenuItem submPuzzleSize4 = new MenuItem("4x4");
            submPuzzleSize4.Tag = "4";
            submPuzzleSize.MenuItems.Add(submPuzzleSize4);
            MenuItem submPuzzleSize5 = new MenuItem("5x5");
            submPuzzleSize5.Tag = "5";
            submPuzzleSize.MenuItems.Add(submPuzzleSize5);
            MenuItem submPuzzleSize6 = new MenuItem("6x6");
            submPuzzleSize6.Tag = "6";
            submPuzzleSize.MenuItems.Add(submPuzzleSize6); 




            //Create Puzzle Image Submenu
            MenuItem submPuzzleImage1 = new MenuItem("Self Portrait");
            submPuzzleImage1.Tag = "vg_self_portrait.jpg";
            submPuzzleImage.MenuItems.Add(submPuzzleImage1);
            MenuItem submPuzzleImage2 = new MenuItem("Sunflowers");
            submPuzzleImage2.Tag = "vg_sunflowers.jpg";
            submPuzzleImage.MenuItems.Add(submPuzzleImage2);
            MenuItem submPuzzleImage3 = new MenuItem("Irises");
            submPuzzleImage3.Tag = "vg_irises.jpg";
            submPuzzleImage.MenuItems.Add(submPuzzleImage3);
            MenuItem submPuzzleImage4 = new MenuItem("Starry Night");
            submPuzzleImage4.Tag = "vg_starry_night.jpg";
            submPuzzleImage.MenuItems.Add(submPuzzleImage4);
            submPuzzleImage.MenuItems.Add("-");
            MenuItem submPuzzleImage5 = new MenuItem("Custom Image");
            submPuzzleImage.MenuItems.Add(submPuzzleImage5);

            imageFileName = submPuzzleImage1.Tag.ToString();
            imageFileName = appPath + @"\" + imageFileName;
            
            submPuzzleImage1.Checked = true;

            // Add event handlers for the menu items. 
            submShuffle.Click += new EventHandler(MMShuffleClick);
            submExit.Click += new EventHandler(MMExitClick);
            submPuzzleSize2.Click += new EventHandler(MMResizeClick);
            submPuzzleSize3.Click += new EventHandler(MMResizeClick);
            submPuzzleSize4.Click += new EventHandler(MMResizeClick);
            submPuzzleSize5.Click += new EventHandler(MMResizeClick);
            submPuzzleSize6.Click += new EventHandler(MMResizeClick);
            submPuzzleImage1.Click += new EventHandler(MMPuzzleImageClick);
            submPuzzleImage2.Click += new EventHandler(MMPuzzleImageClick);
            submPuzzleImage3.Click += new EventHandler(MMPuzzleImageClick);
            submPuzzleImage4.Click += new EventHandler(MMPuzzleImageClick);
            submPuzzleImage5.Click += new EventHandler(MMCustomImageClick);
            submDisplayHints.Click += new EventHandler(MMDisplayHints);

            // Assign the menu to the form. 
            Menu = MyMenu;

            if (numberOfColsRows == 3) { submPuzzleSize3.Checked = true; }
            if (numberOfColsRows == 4) { submPuzzleSize4.Checked = true; }
            if (numberOfColsRows == 5) { submPuzzleSize5.Checked = true; }

            InitializePuzzlePieces();
            simulateMoves(movesToSimulate);
            InitializeComponent();

            //moved from form designer 
            //have to compensate for spacers between rows
            int frmWidth = 450 + 10 + 10 + (2 * (numberOfColsRows - 1));
            int frmHeight = 450 + 10 + 10 + (2 * (numberOfColsRows - 1));
            this.ClientSize = new System.Drawing.Size(frmWidth, frmHeight);

            CenterToScreen();

        }

        private void InitializePuzzlePieces()
        {
            int PiecesToDisplay;
            
            PiecesToDisplay = numberOfColsRows * numberOfColsRows + 1;
            if (display_all_pieces) { PiecesToDisplay = (numberOfColsRows * numberOfColsRows) + 1; }

            for (int i = 1; i < (PiecesToDisplay); i++)
            {
                PuzzlePieces[i] = new PuzzlePiece();
                PuzzlePieces[i].ImageFileName=imageFileName;
                PuzzlePieces[i].SetCorrectPosition(i, numberOfColsRows);
                PuzzlePieces[i].SetImagePosition(i, numberOfColsRows);
            }
        }

        private void Form1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point mousePt = new Point(e.X, e.Y);
            int EmptyPosition;
            bool CanMove;

            for (int i = 1; i < (numberOfColsRows * numberOfColsRows); i++)
            {
                if (PuzzlePieces[i].rect.Contains(mousePt))

                {
                    isImageClicked = true;
                    imageClicked = i;
                }

            }

            if (isImageClicked && !display_all_pieces)
            {
                //get empty position
                EmptyPosition = empty_position();
                CanMove = can_move(EmptyPosition, PuzzlePieces[imageClicked].ImagePosition);
                if (CanMove)
                {
                    PuzzlePieces[imageClicked].SetImagePosition(EmptyPosition, numberOfColsRows);
                    Invalidate();
                    IsComplete();
                }
            }
        }

        private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;

            int PiecesToDisplay;

            PiecesToDisplay = numberOfColsRows * numberOfColsRows;
            if (display_all_pieces) { PiecesToDisplay = (numberOfColsRows * numberOfColsRows) + 1; }


            for (int i = 1; i < PiecesToDisplay; i++)
            {
                g.DrawImage(PuzzlePieces[i].bMap, PuzzlePieces[i].rect);
                if (display_hint)
                {
                    //FontStyle style = FontStyle.Bold;
                    //g.DrawString(i.ToString(), new Font("Tahoma", 24), Brushes.Black, PuzzlePieces[i].rect, strFormat);
                    //Rectangle FontArea;
                    //FontArea = PuzzlePieces[i].rect;
                    //FontArea.X += 2;
                    //FontArea.Y -= 2;
                    g.DrawString(i.ToString(), new Font("Tahoma", 20), Brushes.White, PuzzlePieces[i].rect, strFormat);
                }
            }

            if (isImageClicked == true)
            {
                Pen outline = new Pen(Color.Black, 2);
                //switch (imageClicked)
                //{
                //    case 0:
                //        g.DrawRectangle(outline, rectA);
                //        break;
                //    case 1:
                //        g.DrawRectangle(outline, rectB);
                //        break;
                //    case 2:
                //        g.DrawRectangle(outline, rectC);
                //        break;
                //    default:
                //        break;
                //}
            }
        }
        private int empty_position()
        {
            ArrayList myAL = new ArrayList();
            int output = 0;

            for (int i = 1; i < (numberOfColsRows * numberOfColsRows)+1; i++)
            {
                myAL.Add(i);
            }
            for (int i = 1; i < (numberOfColsRows * numberOfColsRows); i++)
            {
                myAL.Remove(PuzzlePieces[i].ImagePosition);
            }
            for (int i = 1; i < (numberOfColsRows * numberOfColsRows) + 1; i++)
            {
                if (myAL.Contains(i))
                {
                    output=i;
                    break;
                }
            }
            return output;
        }
        private bool can_move(int emp,int ip)
        {
            bool b=false;

            int ix=0;   //image x coordinate
            int iy=0;   //image y coordinate
            int ex=0;   //empty space x coordinate
            int ey=0;   //empty space y coordinate

            //get coordinates for image clicked
            getCoordinates(ip,ref ix,ref iy);

            //get coordinated for empty space
            getCoordinates(emp, ref ex, ref ey);

            if(ix==ex)
            {
                if(ey == (iy+1) | ey == (iy-1))
                {
                    b = true;
                }
            }

            if (iy == ey)
            {
                if (ex == (ix+1) | ex == (ix-1))
                {
                    b = true;
                }
            }
            return b;
        }

        private void simulateMoves(int NumberOfMoves)
        {
            int result;
            int EmptyPosition;
            int MoveCount=0;
            bool CanMove;
            Random random = new Random();

            do{
                EmptyPosition = empty_position();
                result = random.Next(1, numberOfColsRows * numberOfColsRows);
                CanMove = can_move(EmptyPosition, PuzzlePieces[result].ImagePosition);
                if (CanMove)
                {
                    PuzzlePieces[result].SetImagePosition(EmptyPosition, numberOfColsRows);
                    MoveCount++;
                }
            } while(MoveCount < NumberOfMoves);
        }
        
        private void IsComplete()
        {
            bool is_complete = true;
            for (int i = 1; i < (numberOfColsRows * numberOfColsRows); i++)
            {
                if (PuzzlePieces[i].ImagePosition != PuzzlePieces[i].CorrectPosition)
                {
                    is_complete = false;
                    break;
                }
            }
            if(is_complete)
            {
                display_all_pieces = true;
                ////InitializePuzzlePieces();

                ////this.Text = "Puzzle Complete";
                //var result = MessageBox.Show("Puzzle Complete! Would you like to play again?", "Puzzle Complete",
                //                 MessageBoxButtons.YesNo,
                //                 MessageBoxIcon.Question);

                //// If the no button was pressed ...
                //if (result == DialogResult.No)
                //{
                //    //Application.Exit();
                //}
                //if (result == DialogResult.Yes)
                //{
                //    simulateMoves(movesToSimulate);
                //    Invalidate();
                //}
            }
        }
        private void getCoordinates(int pos, ref int x, ref int y) 
        {
            //determine Y coordinate of PuzzlePiece
            for (int i = 1; i <= numberOfColsRows; i++)
            {
                if (pos <= numberOfColsRows * i)
                {
                    y = i;
                    break;
                }
            }

            //determine X coordinate of PuzzlePiece
            for (int i = 1; i <= numberOfColsRows; i++)
            {
                if ((pos % numberOfColsRows) == (i % numberOfColsRows))
                {
                    x = i;
                    break;
                }
            }
        }
        
        protected void MMExitClick(object obj, EventArgs e)
        {
            Application.Exit();
        }
        protected void MMShuffleClick(object obj, EventArgs e)
        {
            display_all_pieces = false;
            simulateMoves(movesToSimulate);
            Invalidate();
        }
        public void MMResizeClick(object obj, EventArgs e)
        {
            //clear all check marks in puzzle size menu
            foreach (MenuItem mi in Menu.MenuItems)
            {
                if (mi.Tag.ToString() == "FILE")
                {
                    foreach (MenuItem subm1 in mi.MenuItems)
                    {
                        if (subm1.Tag.ToString() == "PUZZLE_SIZE")
                        {
                            foreach (MenuItem subm2 in subm1.MenuItems)
                            {
                                subm2.Checked = false;
                            }
                            break;
                        }
                    }
                }
            }
            MenuItem mio = (MenuItem)obj;
            mio.Checked = true;
            numberOfColsRows = int.Parse(mio.Tag.ToString());
            display_all_pieces = false;            
            InitializePuzzlePieces();
            simulateMoves(movesToSimulate);
            Invalidate();
        }

        void MMPuzzleImageClick(object sender, EventArgs e)
        {
            //clear all check marks in puzzle size menu
            foreach (MenuItem mi in Menu.MenuItems)
            {
                if (mi.Tag.ToString() == "FILE")
                {
                    foreach (MenuItem subm1 in mi.MenuItems)
                    {
                        if (subm1.Tag.ToString() == "PUZZLE_IMAGE")
                        {
                            foreach (MenuItem subm2 in subm1.MenuItems)
                            {
                                subm2.Checked = false;
                            }
                            break;
                        }
                    }
                }
            }
            MenuItem mio = (MenuItem)sender;
            mio.Checked = true;
            imageFileName = mio.Tag.ToString();
            imageFileName = appPath + @"\" + imageFileName;

            for (int i = 1; i < (numberOfColsRows * numberOfColsRows) + 1; i++)
            {
                PuzzlePieces[i].ImageFileName = imageFileName;
                PuzzlePieces[i].SetCorrectPosition(PuzzlePieces[i].CorrectPosition, numberOfColsRows);
            }
            Invalidate();
        }
        void MMCustomImageClick(object sender, EventArgs e)
        {
            OpenFileDialog ofn = new OpenFileDialog();
            ofn.Filter = "Image Files (*.jpg)|*.jpg";
            ofn.Title = "Select an Image";
            
            if (ofn.ShowDialog() == DialogResult.OK)
            {
                //clear all check marks in puzzle size menu
                foreach (MenuItem mi in Menu.MenuItems)
                {
                    if (mi.Tag.ToString() == "FILE")
                    {
                        foreach (MenuItem subm1 in mi.MenuItems)
                        {
                            if (subm1.Tag.ToString() == "PUZZLE_IMAGE")
                            {
                                foreach (MenuItem subm2 in subm1.MenuItems)
                                {
                                    subm2.Checked = false;
                                }
                                break;
                            }
                        }
                    }
                }
                //MenuItem mio = (MenuItem)sender;
                //mio.Checked = true;
                
                imageFileName = ofn.FileName;
                for (int i = 1; i < (numberOfColsRows * numberOfColsRows); i++)
                {
                    PuzzlePieces[i].ImageFileName = imageFileName;
                    PuzzlePieces[i].SetCorrectPosition(PuzzlePieces[i].CorrectPosition, numberOfColsRows);
                }
                Invalidate();
            
            
            
            
            
            
            }
        }

        protected void MMDisplayHints(object obj, EventArgs e)
        {
            MenuItem mio = (MenuItem)obj;
            if (display_hint)
            {
                display_hint = false;
                mio.Checked = false;
            }
            else 
            {
                display_hint = true;
                mio.Checked = true;
            }
            Invalidate();
        }

    }
}
