using System;
using System.Drawing;
using System.Collections;
using System.Windows.Media.Imaging;
using System.Windows;

namespace SlidePuzzle
{
    public class PuzzlePiece
    {
        private string PPimageFileName;
        private int PPImagePosition;
        private int PPCorrectPosition;
        private Image bMapImage;
        private Rectangle recta;

        public string ImageFileName
        {
            set
            {
                PPimageFileName=value;
            }
            get
            {
                return PPimageFileName;
            }
        }

        public Rectangle rect
        {
            get
            {
                return recta;
            }
        }
        public void SetCorrectPosition(int pos, int rows)
        {
            PPCorrectPosition = pos;
            
            //set image
            int cols = rows;  //square grid
            int imgSize;
            int CropX = 0;
            int CropY = 0;
            int cntPos = 0;
           
            bMapImage = new Bitmap(PPimageFileName);

            if (bMapImage.Width < bMapImage.Height)
            {
                imgSize = bMapImage.Width;
            }
            else
            {
                imgSize = bMapImage.Height;
            }

            imgSize /= rows;
            
            for (int i = 1; i <= rows; i++)
            {
                CropX = 0;
                if (i > 1) { CropY+=imgSize;}
                for (int j = 1; j <= cols; j++)
                {
                    if (j > 1) { CropX += imgSize; }
                    cntPos++;
                    if (pos == cntPos) { break; }
                }
                if (pos == cntPos) { break; }
            }

            Rectangle cropArea = new Rectangle();
            cropArea.X = CropX;
            cropArea.Y = CropY;
            cropArea.Width = imgSize;
            cropArea.Height = imgSize;

            Bitmap bmpImage = new Bitmap(bMapImage);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            bMapImage = bmpCrop;

        
        }
        public int CorrectPosition
        {
            get
            {
                return PPCorrectPosition;
            }
        }
        public int ImagePosition
        {
            get
            {
                return PPImagePosition;
            }
        }
        public void SetImagePosition(int pos, int rows)
        {
            PPImagePosition = pos;

            int imgWidth;
            int imgHeight;  //height is same as width - Image is square
            int imgX=0;
            int imgY=0;
            int spacer = 2;
            int cols = rows; //same number of columns as rows
            int cntPos = 0;
            int border = 7;

            imgWidth = 450 / rows;
            imgHeight = imgWidth;

            recta.Width = imgWidth;
            recta.Height = imgHeight;

            imgY = border;

            for (int i = 1; i <= rows; i++)
            {
                imgY = imgY + spacer;
                imgX = border;
                for (int j = 1; j <=cols; j++)
                {
                    cntPos++;
                    imgX = imgX + spacer;
                    if(cntPos==pos)
                    {
                        break;
                    }
                    imgX = imgX + imgWidth;
                }
                if (cntPos == pos)
                {
                    break;
                }
                imgY = imgY + imgHeight;
            }
                    recta.X=imgX;
                    recta.Y = imgY;

            //switch (PPImagePosition)
            //{
            //    case 1:
            //        recta.X=10;
            //        recta.Y=10;
            //        break;
            //    case 2:
            //        recta.X = 163;
            //        recta.Y = 10;
            //        break;
            //    case 3:
            //        recta.X = 316;
            //        recta.Y = 10;
            //        break;
            //    case 4:
            //        recta.X = 10;
            //        recta.Y = 163;
            //        break;
            //    case 5:
            //        recta.X = 163;
            //        recta.Y = 163;
            //        break;
            //    case 6:
            //        recta.X = 316;
            //        recta.Y = 163;
            //        break;
            //    case 7:
            //        recta.X = 10;
            //        recta.Y = 316;
            //        break;
            //    case 8:
            //        recta.X = 163;
            //        recta.Y = 316;
            //        break;
            //    case 9:
            //        recta.X = 316;
            //        recta.Y = 316;
            //        break;

            //}
        }
        
        public Image bMap
        {
            get
            {
                return bMapImage;
            }
        }

    }
}
