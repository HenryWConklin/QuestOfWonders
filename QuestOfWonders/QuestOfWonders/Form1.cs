using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestOfWonders
{
    public partial class frmMain : Form
    {
        Bitmap buffer;
        Graphics bufferGraphics;
        Graphics panelGraphics;

        bool running = false;

        Brush skyBrush = new SolidBrush(Color.SkyBlue);
        
        public static int viewX = 0;
        public static int viewY = 0;
        public static int viewWidth;
        public static int viewHeight;

        Map currentMap;
        static Player player;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            buffer = new Bitmap(pnlMain.Width, pnlMain.Height);
            bufferGraphics = Graphics.FromImage(buffer);
            panelGraphics = pnlMain.CreateGraphics();
            viewWidth = pnlMain.Width;
            viewHeight = pnlMain.Height;
        }



        public void Run()
        {
            currentMap = new Map("Resources/QuestOfWondersStage1.bmp");
            while (running)
            {
                UpdateGame();
                Draw();
                Application.DoEvents();
            }
        }

        public void Draw()
        {
            bufferGraphics.FillRectangle(skyBrush, 0, 0, pnlMain.Width, pnlMain.Height);

            if(currentMap != null) currentMap.Draw(bufferGraphics);
            if (player != null) player.Draw(bufferGraphics);

            panelGraphics.DrawImage(buffer, 0, 0, pnlMain.Width, pnlMain.Height);
        }

        public void UpdateGame()
        {
            if(currentMap != null) currentMap.Update(1);
            if (player != null) player.Update(1);
            UpdateView();
            DoCollision();
        }

        public void DoCollision()
        {
            if (player != null)
            {
                Point startPos = new Point(player.pos.X, player.pos.Y);

                int tl = currentMap.CheckLocation(player.pos.X, player.pos.Y);
                int tr = currentMap.CheckLocation(player.pos.X + Map.TILE_SIZE, player.pos.Y);
                int hl = currentMap.CheckLocation(player.pos.X, player.pos.Y + Map.TILE_SIZE);
                int hr = currentMap.CheckLocation(player.pos.X + Map.TILE_SIZE, player.pos.Y + Map.TILE_SIZE);
                int bl = currentMap.CheckLocation(player.pos.X, player.pos.Y + 2 * Map.TILE_SIZE);
                int br = currentMap.CheckLocation(player.pos.X + Map.TILE_SIZE, player.pos.Y + 2 * Map.TILE_SIZE);

                if (tl + tr + hl + hr + bl + br != 0)
                {
                    player.pos.Y = (player.pos.Y + Map.TILE_SIZE/2) / Map.TILE_SIZE * Map.TILE_SIZE;
                    if (player.pos.Y < startPos.Y)
                        player.OnCollide(Direction.Down);
                    else
                        player.OnCollide(Direction.Up);
                }

                tl = currentMap.CheckLocation(player.pos.X, player.pos.Y);
                tr = currentMap.CheckLocation(player.pos.X + Map.TILE_SIZE, player.pos.Y);
                hl = currentMap.CheckLocation(player.pos.X, player.pos.Y + Map.TILE_SIZE);
                hr = currentMap.CheckLocation(player.pos.X + Map.TILE_SIZE, player.pos.Y + Map.TILE_SIZE);
                bl = currentMap.CheckLocation(player.pos.X, player.pos.Y + 2 * Map.TILE_SIZE);
                br = currentMap.CheckLocation(player.pos.X + Map.TILE_SIZE, player.pos.Y + 2 * Map.TILE_SIZE);

                if (tl + tr + hl + hr + bl + br != 0)
                {
                    player.pos.X = (player.pos.X + Map.TILE_SIZE/2)/ Map.TILE_SIZE * Map.TILE_SIZE;
                    if (player.pos.X < startPos.X)
                        player.OnCollide(Direction.Right);
                    else
                        player.OnCollide(Direction.Left);
                }
                
            }
        }
        
        public void UpdateView()
        {
            if (player != null)
            {
                int hBuffer = 200;
                int vBuffer = 100;

                int newX = viewX;
                int newY = viewY;

                int playerX = player.pos.X;
                int playerY = player.pos.Y;

                if (playerX < viewX + hBuffer)
                {
                    newX = (playerX - hBuffer);
                }
                if (playerX > viewX + viewWidth - hBuffer)
                {
                    newX = (playerX + hBuffer - viewWidth);
        }


                if (playerY < viewY + vBuffer)
                {
                    newY = (playerY - vBuffer);
                }
                if (playerY > viewY + viewHeight - vBuffer)
                {
                    newY = (playerY + vBuffer - viewHeight);
                }

                newX = Math.Min(Math.Max(newX, 0), currentMap.widthInTiles * Map.TILE_SIZE - viewWidth);
                newY = Math.Min(Math.Max(newY, 0), currentMap.heightInTiles * Map.TILE_SIZE - viewHeight);

                viewX = newX;
                viewY = newY;
            }
        }

        public static void CreatePlayer(int x, int y)
        {
            player = new Player(x, y);
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            running = true;
            btnBegin.Visible = false;
            btnBegin.Enabled = false;
            Run();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            running = false;
        }

        private void frmMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            player.OnKeyDown(e.KeyCode);
        }

        public void MoveView(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
            {
                viewX = Math.Min(viewX + 10, currentMap.widthInTiles * Map.TILE_SIZE - viewWidth);
            }
            if (e.KeyCode == Keys.A)
            {
                viewX = Math.Max(0, viewX - 10);
            }
            if (e.KeyCode == Keys.W)
            {
                viewY = Math.Max(viewY - 10, 0);
            }
            if (e.KeyCode == Keys.S)
            {
                viewY = Math.Min(viewY + 10, currentMap.heightInTiles * Map.TILE_SIZE - viewHeight);
            }
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            player.OnKeyUp(e.KeyCode);

            if (player != null) player.OnKeyDown(e.KeyCode);
        }
    }
}
