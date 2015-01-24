using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace QuestOfWonders
{
    public class Map
    {
        public static int TILE_SIZE = 24;

        Color DIRT_COLOR = Color.FromArgb(0, 0, 0);
        Color GRASS_COLOR = Color.FromArgb(0, 255, 0);
        Color SPIKE_COLOR = Color.FromArgb(255, 0, 0);
        Color PLAYER_COLOR = Color.FromArgb(0, 0, 255);
        Color ORB_COLOR = Color.FromArgb(255, 255, 0);

        Brush tmpGrass = new SolidBrush(Color.DarkGreen);
        Brush tmpDirt = new SolidBrush(Color.Brown);
        Brush tmpSpike = new SolidBrush(Color.Red);

        Bitmap grassImg = null;
        Bitmap dirtImg = null;
        Bitmap spikeImg = null;

        public static int NOTHING_INT = 0;
        public static int DIRT_INT = 1;
        public static int GRASS_INT = 2;
        public static int SPIKE_INT = 3;
        public static int ORB_INT = 4;

        int[,] map;
        public int widthInTiles;
        public int heightInTiles;

        public Map(string imgFileLoc)
        {
            Bitmap img = (Bitmap)Bitmap.FromFile(imgFileLoc);

            map = new int[img.Width, img.Height];
            widthInTiles = img.Width;
            heightInTiles = img.Height;

            LoadImages();

            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    map[x, y] = 0; //Default nothing

                    //Grab color.
                    Color col = img.GetPixel(x, y);
                    if (col == DIRT_COLOR)
                    {
                        map[x, y] = DIRT_INT;
                    }
                    else if (col == GRASS_COLOR)
                    {
                        if (y > 1)
                        {
                            if (map[x, y - 1] == GRASS_INT || map[x, y - 1] == DIRT_INT)
                            {
                                map[x, y] = DIRT_INT;
                            }
                            else
                            {
                                map[x, y] = GRASS_INT;
                            }
                        }
                        else
                        {
                            map[x, y] = GRASS_INT;
                        }
                    }
                    else if (col == SPIKE_COLOR)
                    {
                        map[x, y] = SPIKE_INT;
                    }
                    else if (col == ORB_COLOR)
                    {
                        Point wonderPoint = ArrayToScreenLocation(x, y);
                        OrbOfWonder orb = new OrbOfWonder(wonderPoint.X, wonderPoint.Y);
                        frmMain.SetWonder(orb);
                    }
                    else if (col == PLAYER_COLOR)
                    {
                        Point playerPoint = ArrayToScreenLocation(x, y - 1);
                        frmMain.CreatePlayer(playerPoint.X, playerPoint.Y);
                    }
                }
            }
        }

        //Loads in the tile images
        public void LoadImages()
        {
            grassImg = new Bitmap(Bitmap.FromFile("Resources/grassBig.png"), TILE_SIZE, TILE_SIZE);
            dirtImg = new Bitmap(Bitmap.FromFile("Resources/stoneBig.png"), TILE_SIZE, TILE_SIZE);
            spikeImg = new Bitmap(Bitmap.FromFile("Resources/spikes.png"), TILE_SIZE, TILE_SIZE);
        }

        //Reads in game coords, converts to map coords, and tells you what's there.
        public int CheckLocation(int x, int y)
        {
            Point newPnt = ScreenToArrayLocation(x, y);
            int newX = newPnt.X;
            int newY = newPnt.Y;

            if (newX < 0) newX = 0;
            if (newX > (widthInTiles - 1)) newX = widthInTiles - 1;
            if (newY < 0) newY = 0;
            if (newY > (heightInTiles - 1)) newY = heightInTiles - 1;

            return map[newX , newY];
        }

        public Point ScreenToArrayLocation(int x, int y)
        {
            int newX = (int)Math.Floor((float)x / (float)TILE_SIZE);
            int newY = (int)Math.Floor((float)y / (float)TILE_SIZE);

            if (newX < 0) newX = 0;
            if (newX > (widthInTiles)) newX = (widthInTiles);

            if (newY < 0) newY = 0;
            if (newY > (heightInTiles)) newY = (heightInTiles);

            return new Point(newX, newY);
        }

        public Point ArrayToScreenLocation(int x, int y)
        {
            Point toRet = new Point(x * TILE_SIZE, y * TILE_SIZE);
            return toRet;
        }

        public void Draw(Graphics g)
        {

            int hBuffer = TILE_SIZE * 2;
            int vBuffer = TILE_SIZE * 2;
            Point start = ScreenToArrayLocation(frmMain.viewX - hBuffer, frmMain.viewY - vBuffer);
            Point end = ScreenToArrayLocation(frmMain.viewX + frmMain.viewWidth + hBuffer, frmMain.viewY + frmMain.viewHeight + vBuffer);

            for (int x = start.X; x < end.X; x++)
            {
                for (int y = start.Y; y < end.Y; y++)
                {
                    Brush drawBrush = null;
                    Bitmap img = null;

                    if (map[x, y] == DIRT_INT)
                    {
                        g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                        drawBrush = tmpDirt;
                        img = dirtImg;
                    }
                    else if (map[x, y] == GRASS_INT)
                    {
                        g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                        drawBrush = tmpGrass;
                        img = grassImg;
                    }
                    else if (map[x, y] == SPIKE_INT)
                    {
                        drawBrush = tmpSpike;
                        img = spikeImg;
                    }

                    if (img != null)
                    {
                        Point locInWorld = ArrayToScreenLocation(x, y);
                        g.DrawImage(img, locInWorld.X - frmMain.viewX, locInWorld.Y - frmMain.viewY, TILE_SIZE, TILE_SIZE);
                    }
                    else if (drawBrush != null)
                    {
                        Point locInWorld = ArrayToScreenLocation(x, y);
                        g.FillRectangle(drawBrush, locInWorld.X - frmMain.viewX, locInWorld.Y - frmMain.viewY, TILE_SIZE, TILE_SIZE);
                    }
                    g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                }
            }
        }

        public void Update(float time)
        {

        }
    }
}
