using ConsoleGame.Behaviour;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class BitmapToAscii
    {
        char[][] UICharacters = new char[][]
        {
            new char[] { '┼', '┤', '├', '┴', '┬', '┌', '└', '┘', '┐', '│', '─', '▪' },
            new char[] { '╬', '╣', '╠', '╩', '╦', '╔', '╚', '╝', '╗', '║', '═', '▪' }
        };

        public enum Pivot
        {
            TopLeft = 0,
            Top,
            TopRight,
            Left,
            Center,
            Right,
            BottomLeft,
            Bottom,
            BottomRight
        }

        Bitmap bmpUIChatbox;
        Bitmap bmpUIBackgroundbox;
        Bitmap bmpUIpBar;
        Graphics gr;
        Graphics grBack;
        Graphics grpbar;

        public Bitmap DrawChatBoxRect(Vector2 position, Vector2 size)
        {
            if (bmpUIChatbox == null)
                bmpUIChatbox = new Bitmap(Screen.Width, Screen.Height);

            if (gr == null)
            {
                gr = Graphics.FromImage(bmpUIChatbox);
                gr.SmoothingMode = SmoothingMode.HighSpeed;
            }

            gr.FillRectangle(Brushes.White, new Rectangle(0, 0, bmpUIChatbox.Width, bmpUIChatbox.Height));
            var brush = new SolidBrush(Color.FromArgb(20, 20, 20));

            Rectangle rect = new Rectangle(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y));
            gr.FillRectangle(brush, rect);
            using (Pen thick_pen = new Pen(Color.FromArgb(0,0,0), 1))
            {
                gr.DrawRectangle(thick_pen, rect);
            }
            return bmpUIChatbox;
        }

        public Bitmap DrawGreenRect(Vector2 position, Vector2 size)
        {
            if (bmpUIBackgroundbox == null)
                bmpUIBackgroundbox = new Bitmap(Screen.Width, Screen.Height);

            if (grBack == null)
            {
                grBack = Graphics.FromImage(bmpUIBackgroundbox);
                grBack.SmoothingMode = SmoothingMode.HighSpeed;
                grBack.FillRectangle(Brushes.White, new Rectangle(0, 0, bmpUIBackgroundbox.Width, bmpUIBackgroundbox.Height));
            }

            Rectangle rect = new Rectangle(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y));
            using (Pen thick_pen = new Pen(Color.FromArgb(0, 0, 5), 1))
            {
                grBack.DrawRectangle(thick_pen, rect);
            }
            return bmpUIBackgroundbox;
        }

        public Bitmap DrawButton(Vector2 position, Vector2 size)
        {
            if (bmpUIChatbox == null)
                bmpUIChatbox = new Bitmap(Screen.Width, Screen.Height);

            if (gr == null)
            {
                gr = Graphics.FromImage(bmpUIChatbox);
                gr.SmoothingMode = SmoothingMode.HighSpeed;
            }

            //gr.FillRectangle(Brushes.White, new Rectangle(0, 0, bmpUIChatbox.Width, bmpUIChatbox.Height));

            Rectangle rect = new Rectangle(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y));
            using (Pen thick_pen = new Pen(Color.FromArgb(0, 0, 0), 1))
            {
                gr.DrawRectangle(thick_pen, rect);
            }
            return bmpUIChatbox;
        }

        public Bitmap DrawProgressBar(int len)
        {
            if (bmpUIpBar == null)
                bmpUIpBar = new Bitmap(Screen.Width, Screen.Height);

            if (grpbar == null)
            {
                grpbar = Graphics.FromImage(bmpUIpBar);
                grpbar.SmoothingMode = SmoothingMode.HighSpeed;
            }

            grpbar.FillRectangle(Brushes.White, new Rectangle(0, 0, bmpUIpBar.Width, bmpUIpBar.Height));

            using (Pen thick_pen = new Pen(Color.FromArgb(239, 228, 176), 1))
            {
                grpbar.DrawLine(thick_pen, 1, Screen.Height - 2, Screen.Width - 3, Screen.Height - 2);
            }
            using (Pen thick_pen = new Pen(Color.FromArgb(127, 127, 127), 1))
            {
                grpbar.DrawLine(thick_pen, 1, Screen.Height - 2, len, Screen.Height - 2);
            }
            return bmpUIpBar;
        }

        public void Draw(string path, Screen.ScreenLayer layer, int top = 0, int left = 0, bool transparentBackground = false, Pivot pivot = Pivot.Center, bool clear = true, bool BitmapFix = false)
        {
            Draw(new Bitmap(path), (int)layer, new Vector2(left, top), transparentBackground, pivot, clear, BitmapFix);
        }

        public void Draw(string path, int layer, int top = 0, int left = 0, bool transparentBackground = false, Pivot pivot = Pivot.Center, bool clear = true, bool BitmapFix = false)
        {
            Draw(new Bitmap(path), (int)layer, new Vector2(left, top), transparentBackground, pivot, clear, BitmapFix);
        }

        public void Draw(Bitmap bmp, Screen.ScreenLayer layer, int top = 0, int left = 0, bool transparentBackground = false, Pivot pivot = Pivot.Center, bool clear = true, bool BitmapFix = false)
        {
            Draw(bmp, (int)layer, new Vector2(left, top), transparentBackground, pivot, clear, BitmapFix);
        }

        public void Draw(Bitmap bmp, int layer, int top = 0, int left = 0, bool transparentBackground = false, Pivot pivot = Pivot.Center, bool clear = true, bool BitmapFix = false)
        {
            Draw(bmp, (int)layer, new Vector2(left, top), transparentBackground, pivot, clear, BitmapFix);
        }

        public void Draw(string path, Screen.ScreenLayer layer, Vector2 position, bool transparentBackground = false, Pivot pivot = Pivot.Center, bool clear = true, bool BitmapFix = false)
        {
            Draw(new Bitmap(path), (int)layer, position, transparentBackground, pivot, clear, BitmapFix);
        }

        public void Draw(string path, int layer, Vector2 position, bool transparentBackground = false, Pivot pivot = Pivot.Center, bool clear = true, bool BitmapFix = false)
        {
            Draw(new Bitmap(path), (int)layer, position, transparentBackground, pivot, clear, BitmapFix);
        }

        public void Draw(Bitmap bmp, Screen.ScreenLayer layer, Vector2 position, bool transparentBackground = false, Pivot pivot = Pivot.Center, bool clear = true, bool BitmapFix = false)
        {
            Draw(bmp, (int)layer, position, transparentBackground, pivot, clear, BitmapFix);
        }

        public void Draw(Bitmap bmp, int layer, Vector2 position, bool transparentBackground = false, Pivot pivot = Pivot.Center, bool clear = true, bool BitmapFix = false)
        {
            Color[][] bitmap = getBitmapColorMatrix(bmp);
            Vector2 bitmapSize = new Vector2(bitmap[0].Length, bitmap.Length);
            if (clear) Screen.ClearLayer(layer);

            //Vector2 newLoc = CalculatePivot(pivot, position, bitmapSize);

            int Y = BitmapFix ? bitmap.Length - 1 : bitmap.Length;
            int X = BitmapFix ? bitmap[0].Length : bitmap[0].Length - 1;

            for (int y = 0; y < Y; y++)
            {
                if (position.y + y < 0) continue;
                for (int x = 0; x < X; x++)
                {
                    if (position.x + x < 0) continue;
                    if (transparentBackground && bitmap[y][x] == Color.FromArgb(255, 255, 255)) 
                        continue;

                    char ch = GetSurroundings(new Vector2(y, x), bitmap);
                    ConsoleColor col = GetSurroundingsColor(new Vector2(y, x), bitmap);
                    if (transparentBackground)
                    {
                        if (ch == 'z') ch = ' ';
                    }
                    else
                    {
                        if (ch == 'z')
                        {
                            ch = '█';
                            col = ConsoleColor.Black;
                        }
                    }

                    if (BitmapFix)
                        Screen.AppendChar(layer, new Vector2(position.x + y, position.y + x), ch, col);
                    else
                        Screen.AppendChar(layer, new Vector2(position.x + x, position.y + y), ch, col);
                }
            }
        }

        private ConsoleColor GetSurroundingsColor(Vector2 p, Color[][] bitmap)
        {
            Color workerColor = bitmap[Mathf.RoundToInt(p.x)][Mathf.RoundToInt(p.y)];
            if (workerColor == Color.FromArgb(0, 0, 5)) return ConsoleColor.DarkGreen;
            return ConsoleColor.White;
        }

        private char GetSurroundings(Vector2 p, Color[][] bitmap)
        {
            Color workerColor = bitmap[Mathf.RoundToInt(p.x)][Mathf.RoundToInt(p.y)];
            if (workerColor == Color.FromArgb(255, 255, 255)) return ' ';
            if (workerColor == Color.FromArgb(239, 228, 176)) return '░';
            if (workerColor == Color.FromArgb(195, 195, 195)) return '▒';
            if (workerColor == Color.FromArgb(127, 127, 127)) return '▓';
            if (workerColor == Color.FromArgb(136, 0, 21)) return '█';
            if (workerColor == Color.FromArgb(20, 20, 20)) return 'z';
            bool[,] matrix = new bool[3, 3];

            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    if (p.x + x < bitmap.Length && p.x + x >= 0 && p.y + y < bitmap[0].Length && p.y + y >= 0)
                    {
                        if (x == 0 && y == -1)
                            Console.Write("");
                        if (workerColor == bitmap[Mathf.RoundToInt(p.x) + x][Mathf.RoundToInt(p.y) + y])
                            matrix[x + 1, y + 1] = true;
                    }
                }

            int CharIndex = 0;

            if (matrix[2, 1] && matrix[0, 1] && matrix[1, 0] && matrix[1, 2]) return UICharacters[CharIndex][0];
            else if (matrix[2, 1] && matrix[0, 1] && matrix[1, 0]) return UICharacters[CharIndex][1];
            else if (matrix[2, 1] && matrix[0, 1] && matrix[1, 2]) return UICharacters[CharIndex][2];
            else if (matrix[0, 1] && matrix[1, 2] && matrix[1, 0]) return UICharacters[CharIndex][3];
            else if (matrix[2, 1] && matrix[1, 2] && matrix[1, 0]) return UICharacters[CharIndex][4];
            else if (matrix[2, 1] && matrix[1, 2]) return UICharacters[CharIndex][5];
            else if (matrix[0, 1] && matrix[1, 2]) return UICharacters[CharIndex][6];
            else if (matrix[0, 1] && matrix[1, 0]) return UICharacters[CharIndex][7];
            else if (matrix[1, 0] && matrix[2, 1]) return UICharacters[CharIndex][8];
            else if (matrix[2, 1] || matrix[0, 1]) return UICharacters[CharIndex][9];
            else if (matrix[1, 2] || matrix[1, 0]) return UICharacters[CharIndex][10];
            else return UICharacters[CharIndex][11];
        }

        //─│┌┐└┘├┤┬┴┼═║╚╝╔╗╠╣╦╩╬▀▄▌▐▼←↑→↓  ░▒▓█

        private Color[][] getBitmapColorMatrix(string filePath)
        {
            Bitmap bmp = new Bitmap(filePath);
            return getBitmapColorMatrix(bmp);
        }

        private Color[][] getBitmapColorMatrix(Bitmap bmp)
        {
            Color[][] matrix;
            int height = bmp.Height;
            int width = bmp.Width;
            if (height > width)
            {
                matrix = new Color[bmp.Width][];
                for (int i = 0; i <= bmp.Width - 1; i++)
                {
                    matrix[i] = new Color[bmp.Height];
                    for (int j = 0; j < bmp.Height - 1; j++)
                    {
                        matrix[i][j] = bmp.GetPixel(i, j);
                    }
                }
            }
            else
            {
                matrix = new Color[bmp.Height][];
                for (int i = 0; i <= bmp.Height - 1; i++)
                {
                    matrix[i] = new Color[bmp.Width];
                    for (int j = 0; j < bmp.Width - 1; j++)
                    {
                        matrix[i][j] = bmp.GetPixel(j, i);
                    }
                }
            }
            return matrix;
        }

        private Vector2 CalculatePivot(Pivot pivot, Vector2 position, Vector2 size)
        {
            switch (pivot)
            {
                default:
                case Pivot.TopLeft:
                    return new Vector2(position.x, position.y);
                case Pivot.Top:
                    return new Vector2(position.x - size.x / 2, position.y);
                case Pivot.TopRight:
                    return new Vector2(position.x - size.x, position.y);
                case Pivot.Left:
                    return new Vector2(position.x, position.y -  size.y / 2);
                case Pivot.Center:
                    return new Vector2(position.x - size.x / 2, position.y - size.y / 2);
                case Pivot.Right:
                    return new Vector2(position.x - size.x, position.y - size.y / 2);
                case Pivot.BottomLeft:
                    return new Vector2(position.x, position.y - size.y);
                case Pivot.Bottom:
                    return new Vector2(position.x - size.x / 2, position.y - size.y);
                case Pivot.BottomRight:
                    return new Vector2(position.x - size.x, position.y - size.y);
            }
        }

        private Color[][] RotateImage(Bitmap bmp, float angle)
        {
            float alpha = angle;

            while (alpha < 0) alpha += 360;
            while (alpha > 360) alpha -= 360;

            float gamma = 90;
            float beta = 180 - angle - gamma;

            float c1 = bmp.Height;
            float a1 = (float)(c1 * Math.Sin(alpha * Math.PI / 180) / Math.Sin(gamma * Math.PI / 180));
            float b1 = (float)(c1 * Math.Sin(beta * Math.PI / 180) / Math.Sin(gamma * Math.PI / 180));

            float c2 = bmp.Width;
            float a2 = (float)(c2 * Math.Sin(alpha * Math.PI / 180) / Math.Sin(gamma * Math.PI / 180));
            float b2 = (float)(c2 * Math.Sin(beta * Math.PI / 180) / Math.Sin(gamma * Math.PI / 180));

            int width = Convert.ToInt32(b2 + a1);
            int height = Convert.ToInt32(b1 + a2);

            Bitmap rotatedImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(rotatedImage.Width / 2, rotatedImage.Height / 2); //set the rotation point as the center into the matrix
                g.RotateTransform(angle); //rotate
                g.TranslateTransform(-rotatedImage.Width / 2, -rotatedImage.Height / 2); //restore rotation point into the matrix
                g.DrawImage(bmp, new Point((width - bmp.Width) / 2, (height - bmp.Height) / 2)); //draw the image on the new bitmap
            }
            return getBitmapColorMatrix(bmp);
        }
    }
}
