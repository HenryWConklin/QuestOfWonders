﻿using System;
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
        private const float FRAME_TIME = 1f / 60;

        Bitmap buffer;
        Graphics bufferGraphics;
        Graphics panelGraphics;

        Bitmap bkgImg;

        bool running = false;

        Brush skyBrush = new SolidBrush(Color.SkyBlue);
        
        public static int viewX = 0;
        public static int viewY = 0;
        public static int viewWidth;
        public static int viewHeight;

        public bool hasDrawnMap = false;

        public float timeAccum;

        Map currentMap;
        static Player player;

        long prevTicks;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            bkgImg = (Bitmap)Bitmap.FromFile("Resources/sky sunset.png");
            buffer = new Bitmap(pnlMain.Width, pnlMain.Height);
            bufferGraphics = Graphics.FromImage(buffer);
            panelGraphics = pnlMain.CreateGraphics();
            viewWidth = pnlMain.Width;
            viewHeight = pnlMain.Height;

            panelGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            panelGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            timeAccum = 0;
        }

        public void Run()
        {
            prevTicks = DateTime.Now.Ticks;
            currentMap = new Map("Resources/QuestOfWondersStage1_1.bmp");
            while (running)
            {
                UpdateGame();
                //Draw();
                pnlMain.Refresh();
                Application.DoEvents();
            }
        }

        public void Draw()
        {
            //bufferGraphics.FillRectangle(skyBrush, 0, 0, pnlMain.Width, pnlMain.Height);
            bufferGraphics.DrawImage(bkgImg, 0, 0, pnlMain.Width, pnlMain.Height);

            if (currentMap != null && !hasDrawnMap) currentMap.Draw(bufferGraphics);
            
            if (player != null) player.Draw(bufferGraphics);
            
            panelGraphics.DrawImage(buffer, 0, 0, pnlMain.Width, pnlMain.Height);
        }

        public void Draw(Graphics g)
        {
            //bufferGraphics.FillRectangle(skyBrush, 0, 0, pnlMain.Width, pnlMain.Height);
            bufferGraphics.DrawImage(bkgImg, 0, 0, pnlMain.Width, pnlMain.Height);

            if (currentMap != null && !hasDrawnMap) currentMap.Draw(bufferGraphics);

            if (player != null) player.Draw(bufferGraphics);

            g.DrawImage(buffer, 0, 0, pnlMain.Width, pnlMain.Height);
        }

        public void UpdateGame()
        {

            long currentTicks = DateTime.Now.Ticks;
            long ticksElapsed = currentTicks - prevTicks;
            prevTicks = currentTicks;

            float deltaTime = (float)(TimeSpan.FromTicks(ticksElapsed).TotalSeconds);
            //float deltaTime = 1;
            timeAccum += deltaTime;
            while (timeAccum > FRAME_TIME)
            {
                if (currentMap != null) currentMap.Update(FRAME_TIME);
            if (player != null)
            {
                    player.Update(FRAME_TIME);
                    DoCollision();
                if (currentMap.CheckLocation(player.GetPos().X + 1, player.GetPos().Y + 2 * Map.TILE_SIZE + 1) != 0 ||
                    currentMap.CheckLocation(player.GetPos().X + Map.TILE_SIZE - 1, player.GetPos().Y + 2 * Map.TILE_SIZE + 1) != 0)
                    player.Ground();
            }
                timeAccum -= FRAME_TIME;
            }
            UpdateView();
        }

        public bool CheckPlayerMapCollision()
        {
            int tl = currentMap.CheckLocation(player.GetPos().X + 1,                 player.GetPos().Y +1);
            int tr = currentMap.CheckLocation(player.GetPos().X + Map.TILE_SIZE-1,   player.GetPos().Y+1);
            int hl = currentMap.CheckLocation(player.GetPos().X+1,                   player.GetPos().Y + Map.TILE_SIZE);
            int hr = currentMap.CheckLocation(player.GetPos().X + Map.TILE_SIZE-1,   player.GetPos().Y + Map.TILE_SIZE);
            int bl = currentMap.CheckLocation(player.GetPos().X+1,                   player.GetPos().Y + 2 * Map.TILE_SIZE-1);
            int br = currentMap.CheckLocation(player.GetPos().X + Map.TILE_SIZE-1,   player.GetPos().Y + 2 * Map.TILE_SIZE-1);

            return (tl + tr + hl + hr + bl + br != 0);
        }

        public void DoCollision()
        {
            if (player != null)
            {
                Point startPos = new Point(player.GetPos().X, player.GetPos().Y);

                if (CheckPlayerMapCollision())
                {
                    player.SetPosY((player.GetPos().Y + Map.TILE_SIZE/2) / Map.TILE_SIZE * Map.TILE_SIZE);   
                }               

                if (CheckPlayerMapCollision())
                {
                    player.SetPosY(startPos.Y);
                    player.SetPosX((player.GetPos().X + Map.TILE_SIZE/2)/ Map.TILE_SIZE * Map.TILE_SIZE);
                    if (player.GetPos().X < startPos.X)
                        player.OnCollide(Direction.Right);
                    else
                        player.OnCollide(Direction.Left);
                }

                if (CheckPlayerMapCollision())
                {
                    player.SetPosY((player.GetPos().Y + Map.TILE_SIZE / 2) / Map.TILE_SIZE * Map.TILE_SIZE);

                }

                if (player.GetPos().Y < startPos.Y)
                    player.OnCollide(Direction.Down);
                else if (player.GetPos().Y > startPos.Y)
                    player.OnCollide(Direction.Up);
                
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

                int playerX = player.GetPos().X;
                int playerY = player.GetPos().Y;

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
            e.SuppressKeyPress = true;
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

            if (player != null) player.OnKeyUp(e.KeyCode);
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        private void pnlMain_Click(object sender, EventArgs e)
        {
            pnlMain.Paint += new PaintEventHandler(pnlMain_Paint);
        }
    }
}
