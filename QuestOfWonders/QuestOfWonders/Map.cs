using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace QuestOfWonders
{
    class Map
    {
        public static int TILE_SIZE = 16;

        Color DIRT_COLOR = Color.FromArgb(0, 0, 0);
        Color GRASS_COLOR = Color.FromArgb(0, 255, 0);

        Brush tmpGrass = new SolidBrush(Color.DarkGreen);
        Brush tmpDirt = new SolidBrush(Color.Brown);

        public const int NOTHING_INT = 0;
        public const int DIRT_INT = 1;
        public const int GRASS_INT = 2;

        int[,] map;
        public int widthInTiles;
        public int heightInTiles;

        public Map(string imgFileLoc)
        {
            Bitmap img = (Bitmap)Bitmap.FromFile(imgFileLoc);

            map = new int[img.Width, img.Height];
            widthInTiles = img.Width;
            heightInTiles = img.Height;
            
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    map[x, y] = 0; //Default nothing

                    //Grab color.
                    Color col = img.GetPixel(x, y);
                    if (col == DIRT_COLOR)
                    {
                        map[x, y] = 1;
                    }
                    if (col == GRASS_COLOR)
                    {
                        map[x, y] = 2;
                    }
                }
            }
        }

        //Reads in game coords, converts to map coords, and tells you what's there.
        public int CheckLocation(int x, int y)
        {

            return 0;
        }

        public Point ArrayToScreenLocation(int x, int y)
        {
            Point toRet = new Point(x * TILE_SIZE, y * TILE_SIZE);
            return toRet;
        }

        public void Draw(Graphics g)
        {
            for (int x = 0; x < widthInTiles; x++)
            {
                for (int y = 0; y < heightInTiles; y++)
                {
                    Brush drawBrush = null;
                    if (map[x, y] == DIRT_INT)
                    {
                        drawBrush = tmpDirt;
                    }
                    if (map[x, y] == GRASS_INT)
                    { 
                        drawBrush = tmpGrass;
                    }
                    if (drawBrush != null)
                    {
                        Point locInWorld = ArrayToScreenLocation(x, y);
                        g.FillRectangle(drawBrush, locInWorld.X -frmMain.viewX, locInWorld.Y - frmMain.viewY, TILE_SIZE, TILE_SIZE);
                    }
                }
            }
        }

        public void Update(float time)
        {

        }
    }
}
