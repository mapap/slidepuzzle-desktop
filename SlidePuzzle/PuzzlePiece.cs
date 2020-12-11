using System.Drawing;

namespace SlidePuzzle
{
    public class PuzzlePiece
    {
        public Rectangle Rectangle { get { return _rect; } }
        public string ImageFileName { get; set; }
        public Image BitmapImage { get; private set; }
        public int CorrectPosition { get; private set; }
        public int ImagePosition { get; private set; }
        private Rectangle _rect;
 
        public void SetCorrectPosition(int pos, int rows)
        {
            CorrectPosition = pos;
            
            //set image
            int cols = rows;  //square grid
            int imgSize;
            int CropX = 0;
            int CropY = 0;
            int cntPos = 0;

            BitmapImage = new Bitmap(ImageFileName);

            if (BitmapImage.Width < BitmapImage.Height)
            {
                imgSize = BitmapImage.Width;
            }
            else
            {
                imgSize = BitmapImage.Height;
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

            Bitmap bmpImage = new Bitmap(BitmapImage);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            BitmapImage = bmpCrop;
        }

        public void SetImagePosition(int pos, int rows)
        {
            ImagePosition = pos;

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

            _rect.Width = imgWidth;
            _rect.Height = imgHeight;

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

            _rect.X=imgX;
            _rect.Y = imgY;
        }
    }
}
