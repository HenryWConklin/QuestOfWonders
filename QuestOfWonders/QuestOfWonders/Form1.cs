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

        Map currentMap;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            buffer = new Bitmap(pnlMain.Width, pnlMain.Height);
            bufferGraphics = Graphics.FromImage(buffer);
            panelGraphics = pnlMain.CreateGraphics();
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

            currentMap.Draw(bufferGraphics);

            panelGraphics.DrawImage(buffer, 0, 0, pnlMain.Width, pnlMain.Height);
        }

        public void UpdateGame()
        {
            currentMap.Update(1);
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
                
            }
        }
    }
}
