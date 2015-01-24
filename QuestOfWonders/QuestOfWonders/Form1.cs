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
            DoCollision();
        }

        public void DoCollision()
        {
            if (player != null)
            {
                currentMap.CheckLocation(player.pos.X, player.pos.Y);
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
    }
}
