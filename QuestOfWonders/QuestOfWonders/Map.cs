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
        Color ENEMY_COLOR = Color.FromArgb(100, 100, 100);
        Color LSHOOTER_COLOR = Color.FromArgb(255, 0, 255);
        Color RSHOOTER_COLOR = Color.FromArgb(50, 100, 50);
        Color USHOOTER_COLOR = Color.FromArgb(100, 255, 200);
        Color DSHOOTER_COLOR = Color.FromArgb(255, 200, 150);

        Brush tmpGrass = new SolidBrush(Color.DarkGreen);
        Brush tmpDirt = new SolidBrush(Color.Brown);
        Brush tmpSpike = new SolidBrush(Color.Red);
        Brush tmpShooter = new SolidBrush(Color.Firebrick);

        Bitmap grassImg = null;
        Bitmap dirtImg = null;
        Bitmap spikeImg = null;
        Bitmap shooterImg = null;

        public static int NOTHING_INT = 0;
        public static int DIRT_INT = 1;
        public static int GRASS_INT = 2;
        public static int SPIKE_INT = 3;
        public static int LSHOOTER_INT = 4;
        public static int RSHOOTER_INT = 5;
        public static int USHOOTER_INT = 6;
        public static int DSHOOTER_INT = 7;

        public static float shooterVel = 100.0f;

        int[,] map;
        public int widthInTiles;
        public int heightInTiles;

        public int bulletW = 10;
        public int bulletH = 10;
        public int shooterXOffset = TILE_SIZE / 3;
        public int shooterYOffset = TILE_SIZE / 3;

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
                    else if (col == ENEMY_COLOR)
                    {
                        Point enemyPoint = ArrayToScreenLocation(x, y - 1);
                        Enemy enemy = new Enemy(enemyPoint, Enemy.RIGHT);
                        frmMain.AddEnemy(enemy);
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
                    else if (col == LSHOOTER_COLOR)
                    {
                        map[x, y] = LSHOOTER_INT;
                        Point shooterPoint = ArrayToScreenLocation(x, y);
                        frmMain.AddShooter(new Point(shooterPoint.X - shooterXOffset, shooterPoint.Y + TILE_SIZE/2 - bulletH/2), new PointF(-shooterVel, 0.0f));
                    }
                    else if (col == RSHOOTER_COLOR)
                    {
                        map[x, y] = RSHOOTER_INT;
                        Point shooterPoint = ArrayToScreenLocation(x, y);
                        frmMain.AddShooter(new Point(shooterPoint.X + TILE_SIZE + shooterXOffset, shooterPoint.Y + TILE_SIZE/2 - bulletH/2), new PointF(shooterVel, 0.0f));
                    }
                    else if (col == USHOOTER_COLOR)
                    {
                        map[x, y] = USHOOTER_INT;
                        Point shooterPoint = ArrayToScreenLocation(x, y);
                        frmMain.AddShooter(new Point(shooterPoint.X + TILE_SIZE/2 - bulletW / 2, shooterPoint.Y - shooterYOffset), new PointF(0.0f, -shooterVel));
                    }
                    else if (col == DSHOOTER_COLOR)
                    {
                        map[x, y] = DSHOOTER_INT;
                        Point shooterPoint = ArrayToScreenLocation(x, y);
                        frmMain.AddShooter(new Point(shooterPoint.X + TILE_SIZE / 2 - bulletW / 2, shooterPoint.Y + TILE_SIZE + shooterYOffset), new PointF(0.0f, shooterVel));
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

            //if (newX < 0) newX = 0;
            //if (newX > (widthInTiles - 1)) newX = widthInTiles - 1;
            //if (newY < 0) newY = 0;
            //if (newY > (heightInTiles - 1)) newY = heightInTiles - 1;

            if (newX < 0) return -1;
            if (newX > (widthInTiles - 1)) return -1;
            if (newY < 0) return -1;
            if (newY > (heightInTiles - 1)) return -1;

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
                    else if (map[x, y] == LSHOOTER_INT)
                    {
                        drawBrush = tmpShooter;
                        img = shooterImg;
                    }
                    else if (map[x, y] == RSHOOTER_INT)
                    {
                        drawBrush = tmpShooter;
                        img = shooterImg;
                    }
                    else if (map[x, y] == USHOOTER_INT)
                    {
                        drawBrush = tmpShooter;
                        img = shooterImg;
                    }
                    else if (map[x, y] == DSHOOTER_INT)
                    {
                        drawBrush = tmpShooter;
                        img = shooterImg;
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
