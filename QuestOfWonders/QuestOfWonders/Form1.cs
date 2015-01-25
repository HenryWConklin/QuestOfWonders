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
        private const float FRAME_TIME = 1f / 60;
        public static bool soundOn = true;

        public static bool startLevel = true;
        public static bool inStartText = false;

        Bitmap buffer;
        Graphics bufferGraphics;
        Graphics panelGraphics;

        static Bitmap bkgImg;

        bool running = false;
        public static bool allowPlayerControl = true;

        Brush skyBrush = new SolidBrush(Color.SkyBlue);
        
        public static int viewX = 0;
        public static int viewY = 0;
        public static int viewWidth;
        public static int viewHeight;

        public float timeAccum;
        public static float shooterTimer = 0;
        public float shooterTime = 2.5f; //Seconds

        public static Map currentMap;
        public static Player player;

        private static String[] levelMaps;
        private static List<String>[] levelStartText;
        private static int[] levelGrass;
        private static int currentLevel;

        private static Bitmap[] backgrounds;

        long prevTicks;

        public static Textbox text;

        public static Wonder wonder = null;

        public static Laser laser = null;

        public static List<Projectile> projectiles = new List<Projectile>();
        public static List<Enemy> enemies = new List<Enemy>();
        public static Dictionary<Point, PointF> shooters = new Dictionary<Point, PointF>();
        public static List<Switch> switches = new List<Switch>();
        public static Door door;

        private List<Keys> pressedKeys;

        public static List<Entity> drawEntities = new List<Entity>();

        public static bool finalKey = false;

        private Animation explosionAnim;
        private bool exploding = false;

        public static bool endGameTextOn = false;
        private List<String> endGameText = new List<string>()
        {
            "...",
            "...",
            "So... What do we do now?"
        };
        public Brush endTextCol1 = new SolidBrush(Color.DarkSlateBlue);
        public Brush endTextCol2 = new SolidBrush(Color.DarkRed);
        public Brush endTextCol3 = new SolidBrush(Color.DarkSlateGray);

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

            panelGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            panelGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            timeAccum = 0;

            levelMaps = new String[] { "Resources/QuestOfWondersStage1.bmp",
                "Resources/QuestOfWondersStage2_1.bmp",
                "Resources/QuestOfWondersStage3.bmp",
                "Resources/QuestOfWondersStage4.bmp",
                "Resources/FinalLevel.png" };
            levelGrass = new int[] { 0, 0, 1, 1, 0 };
            currentLevel = 0;


            levelStartText = new List<String>[]
            {
                new List<String>() {"At long last, oh Torpe, you mighty hero, we have found the location of the Orb of Wonders – the mystical object that can defeat the nefarious Dr. Waru, and his Dastardly Plot!\n\n(Space to Progress)",
                    "Only one trial remains: The Dreaded Spike Maze…",
                    "OF DEATH!"},
                new List<String>() {"Thus, after another long, arduous journey, I have led our hero Torpe once more to a magical artifact: The Ancient Staff of Wondrous Wonders.",
                    "Let the valiant quest resume! As we boldly go where no - OH MY GOD! How many spikes are there!?",
                    "And look at those big, scary creatures! They're terrifying! Definitely. Do. Not. Touch. Those."},
                new List<String>() {"Okay, here. The Red Fields of Horrendously Hideous Trials. Home to the most elusive of the world's mystical objects...",
                    "You should be grateful.",
                    "The Secret Sword of Wonderfully Wonderful Wonders, otherwise known as the  world's LAST hope, is here."},
                new List<String>() {"The Cave OF DOOM!",
                    "Have fun!",
                    "...", "Oh, fine...", "So, in The Cave OF DOOM, there are three switches.", "Three.", "See, there's one right down there.", "Got that?",
                    "Good.",
                    "They open the door to Dr. Waru's lair.", "Good Luck!", "You'll need it..."},
                    null
            };


            backgrounds = new Bitmap[] {
                new Bitmap(Bitmap.FromFile("Resources/sky blue.png")),
                new Bitmap(Bitmap.FromFile("Resources/sky foggy.png")),
                new Bitmap(Bitmap.FromFile("Resources/sky sunset.png")),
                new Bitmap(Bitmap.FromFile("Resources/cave.png")),
                new Bitmap(Bitmap.FromFile("Resources/sky foggy.png"))
            };

            bkgImg = backgrounds[currentLevel];

            enemies = new List<Enemy>();
            projectiles = new List<Projectile>();

            explosionAnim = new Animation("explosion", 9, false);

            pressedKeys = new List<Keys>();
            SoundSystem.playSound("QuestOfWonders.Resources.Quest of Wonders1.wav", true);
            
        }

        public void Run()
        {
            prevTicks = DateTime.Now.Ticks;
            currentMap = new Map(levelMaps[0], levelGrass[0], levelStartText[0]);

            pnlMain.Paint += new PaintEventHandler(pnlMain_Paint);
            pnlMain.Refresh();
            pnlMain.Update();

            while (running)
            {
                UpdateGame();
                pnlMain.Refresh();
                Application.DoEvents();
            }

            
        }

        public static void NextLevel()
        {
            if (currentLevel < 4)
            {
                currentLevel++;
                SoundSystem.stopAllSounds();
                if (currentLevel <= 3) 
                    SoundSystem.playSound("QuestOfWonders.Resources.Quest of Wonders" + (currentLevel + 1) + ".wav", true);
                bkgImg = backgrounds[currentLevel];
                startLevel = true;
                Restart();
            }
        }

        public static void StopPlayerHoriz()
        {
            player.StopHorizVelocity();
        }

        public void Draw(Graphics g)
        {
            //bufferGraphics.FillRectangle(skyBrush, 0, 0, pnlMain.Width, pnlMain.Height);
           

            bufferGraphics.DrawImage(bkgImg, 0, 0, pnlMain.Width, pnlMain.Height);

            if (currentMap != null) currentMap.Draw(bufferGraphics);

            if (door != null) door.Draw(bufferGraphics);

            if (laser != null) laser.Draw(bufferGraphics);

            if (player != null) player.Draw(bufferGraphics);

            if (wonder != null) wonder.Draw(bufferGraphics);

            

            foreach (Switch s in switches)
            {
                s.Draw(bufferGraphics);
            }

            foreach (Enemy e in enemies)
            {
                e.Draw(bufferGraphics);
            }

            foreach (Projectile p in projectiles)
            {
                p.Draw(bufferGraphics);
            }

            foreach (Entity e in drawEntities)
            {
                e.Draw(bufferGraphics);
            }

            if (text != null) text.Draw(bufferGraphics);

            if (exploding)
                bufferGraphics.DrawImage(explosionAnim.GetFrame(), 0, 0, viewWidth, viewHeight);

            if (endGameTextOn && text.done)
            {
                bufferGraphics.DrawImage((Bitmap)Bitmap.FromFile("Resources/credits.png"), 0, 0, buffer.Width, buffer.Height);
            }

            g.DrawImage(buffer, 0, 0, pnlMain.Width, pnlMain.Height);


        }

        public void UpdateGame()
        {
            if (player != null && player.IsDead())
            {
                Restart();
            }

            long currentTicks = DateTime.Now.Ticks;
            long ticksElapsed = currentTicks - prevTicks;
            prevTicks = currentTicks;

            float deltaTime = (float)(TimeSpan.FromTicks(ticksElapsed).TotalSeconds);
            //float deltaTime = 1;
            timeAccum += deltaTime;
            shooterTimer += deltaTime;
            if (shooterTimer > shooterTime)
            {
                FireShooters();
                shooterTimer = 0;
            }

            if (door != null)
            {

                bool openDoor = true;
                foreach (Switch s in switches)
                {
                    if (!s.isOn)
                    {
                        openDoor = false;
                        break;
                    }
                }
                if (openDoor)
                {
                    door.opened = true;
                }
            }
            while (timeAccum > FRAME_TIME)
            {
                if (currentMap != null) currentMap.Update(FRAME_TIME);
                
                foreach (Enemy e in enemies)
                {
                    e.Update(FRAME_TIME);
                }
                
                foreach (Projectile p in projectiles)
                {
                    p.Update(FRAME_TIME);
                }

                if (player != null)
                {
                    player.Update(FRAME_TIME);
                    DoCollision();
                }
                timeAccum -= FRAME_TIME;
            }

            if (text != null) text.Update(deltaTime);
            if (wonder != null)
            {
                Rectangle wonderRect = new Rectangle(wonder.getX(), wonder.getY(), wonder.getWidth(), wonder.getHeight());
                Rectangle playerRect = player.GetCollisionRectangle();
                if (wonderRect.IntersectsWith(playerRect))
                {
                    wonder.LaunchCollisionEvent();
                }
                wonder.Update(deltaTime);
            }
            if (laser != null)
            {
                Rectangle laserRect = laser.GetCollisionRect();
                Rectangle playerRect = player.GetCollisionRectangle();
                if (laserRect.IntersectsWith(playerRect))
                {
                    laser.OnCollide();
                }
            }

            if (exploding)
            {
                explosionAnim.Update(deltaTime);
                if (explosionAnim.frame == explosionAnim.frames.Length - 1)
                {
                    NextLevel();
                    exploding = false;
                    finalKey = false;
                    Textbox end = new Textbox(endGameText);
                    allowPlayerControl = false;
                    end.backColor = endTextCol1;
                    text = end;
                    endGameTextOn = true;
                }
            }

            UpdateView();
        }

        public void FireShooters()
        {
            if (shooters.Keys.Count > 0) 
                //SoundSystem.playSound("QuestOfWonders.Resources.Shoot.wav", false);
            foreach (Point p in shooters.Keys)
            {
                Projectile proj = new Projectile(p, shooters[p]);
                projectiles.Add(proj);
            }
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
            // Player

            //bullets
            for (int i=0; i<projectiles.Count(); i++)
            {
                Projectile proj = projectiles[i];
                if (player.GetCollisionRectangle().Contains(proj.getPos()))
                {
                    player.Kill();
                    projectiles.RemoveAt(i);
                    i--;
                    continue;
                }
                if (currentMap.CheckLocation(proj.getPos().X, proj.getPos().Y) != 0)
                {
                    projectiles.RemoveAt(i);
                    i--;
                }
            }

            //spikes
            int bl = currentMap.CheckLocation(player.GetPos().X, player.GetPos().Y + 2 * Map.TILE_SIZE);
            int br = currentMap.CheckLocation(player.GetPos().X + Map.TILE_SIZE - 1, player.GetPos().Y + 2 * Map.TILE_SIZE);
            if (bl == Map.SPIKE_INT || br == Map.SPIKE_INT)
            {
                player.Kill();
            }
            
            //walls
            Point startPos = new Point(player.GetPos().X, player.GetPos().Y);
            Rectangle playerRect = player.GetCollisionRectangle();
            Rectangle doorRect = new Rectangle();
            if (door != null)
            {
                doorRect = door.GetCollisionRectangle();
            }

            if (CheckPlayerMapCollision() || doorRect.IntersectsWith(playerRect))
            {
                player.SetPosY((player.GetPos().Y + Map.TILE_SIZE/2) / Map.TILE_SIZE * Map.TILE_SIZE);   
            }               

            if (CheckPlayerMapCollision() || doorRect.IntersectsWith(playerRect))
            {
                player.SetPosY(startPos.Y);
                player.SetPosX((player.GetPos().X + Map.TILE_SIZE/2)/ Map.TILE_SIZE * Map.TILE_SIZE);
                if (player.GetPos().X < startPos.X)
                    player.OnCollide(Direction.Right);
                else
                    player.OnCollide(Direction.Left);
            }

            if (CheckPlayerMapCollision() || doorRect.IntersectsWith(playerRect))
            {
                player.SetPosY((player.GetPos().Y + Map.TILE_SIZE / 2) / Map.TILE_SIZE * Map.TILE_SIZE);

            }

            if (player.GetPos().Y < startPos.Y)
                player.OnCollide(Direction.Down);
            else if (player.GetPos().Y > startPos.Y)
                player.OnCollide(Direction.Up);

            if (currentMap.CheckLocation(player.GetPos().X + 1, player.GetPos().Y + 2 * Map.TILE_SIZE + 1) != 0 ||
                currentMap.CheckLocation(player.GetPos().X + Map.TILE_SIZE - 1, player.GetPos().Y + 2 * Map.TILE_SIZE + 1) != 0)
                player.Ground();

            // Enemies
            
            foreach (Enemy e in enemies)
            {
                if (e.GetCollisionBounds().IntersectsWith(playerRect)) 
                {
                    player.Kill();
                }

                int inFront = 0;
                int belowFront = 0;
                if (e.GetDir() == Enemy.LEFT)
                {
                    inFront = currentMap.CheckLocation(e.GetPos().X - 1, e.GetPos().Y + 2 * Map.TILE_SIZE - 1);
                    belowFront = currentMap.CheckLocation(e.GetPos().X - 1, e.GetPos().Y + 2 * Map.TILE_SIZE + 1);
                }
                else if (e.GetDir() == Enemy.RIGHT)
                {
                    inFront = currentMap.CheckLocation(e.GetPos().X + Map.TILE_SIZE + 1, e.GetPos().Y + 2 * Map.TILE_SIZE - 1);
                    belowFront = currentMap.CheckLocation(e.GetPos().X + Map.TILE_SIZE + 1, e.GetPos().Y + 2 * Map.TILE_SIZE + 1);
                }
                if (inFront != 0 || belowFront == 0)
                {
                    e.Turn();
                }

            }

            //Switches
            foreach (Switch s in switches)
            {
                if (s.GetCollisionRectangle().IntersectsWith(playerRect))
                {
                    s.isOn = true;
                }
            }
            
        }

        public static void AddEntity(Entity e)
        {
            drawEntities.Add(e);
        }

        public static void Restart()
        {
            drawEntities.Clear();
            shooters.Clear();
            enemies.Clear();
            projectiles.Clear();
            drawEntities.Clear();
            text = null;
            laser = null;
            wonder = null;
            player = null;
            allowPlayerControl = true;
            currentMap = new Map(levelMaps[currentLevel], levelGrass[currentLevel], levelStartText[currentLevel]);
            shooterTimer = 0;
        }
        
        public void UpdateView()
        {
            if (player != null)
            {
                int hBuffer = 300;
                int vBufferUpper = 200;
                int vBufferLower = 200;

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


                if (playerY < viewY + vBufferUpper)
                {
                    newY = (playerY - vBufferUpper);
                }
                if (playerY > viewY + viewHeight - vBufferLower)
                {
                    newY = (playerY + vBufferLower - viewHeight);
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
        public static void AddEnemy(Enemy e)
        {
            enemies.Add(e);
        }
        public static void SetWonder(Wonder theWonder)
        {
            wonder = theWonder;   
        }
        public static void AddShooter(Point location, PointF spawnVel)
        {
            if (!shooters.ContainsKey(location))
            {
                shooters.Add(location, spawnVel);
            }
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
            if (!pressedKeys.Contains(e.KeyCode))
            {
                if (allowPlayerControl) player.OnKeyDown(e.KeyCode);
                if (wonder != null)
				{
					if (wonder.OnKeyDown(e.KeyCode))
						return;
				}
                if (e.KeyCode == Keys.Space && inStartText && text != null)
                {
                    text.Advance();
                    if (text.done)
                    {
                        text = null;
                        inStartText = false;
                        allowPlayerControl = true;
                        startLevel = false;
                    }
                }
                pressedKeys.Add(e.KeyCode);
            }
            if (finalKey)
            {
                LaunchDoomsdayEvent();
            }
            if (laser != null) laser.KeyDown(e.KeyCode);
            if (endGameTextOn && text != null)
            {
                if (text.backColor == endTextCol1)
                {
                    text.backColor = endTextCol2;
                }
                else if (text.backColor == endTextCol2)
                {
                    text.backColor = endTextCol3;
                }
              
                text.Advance();
            }
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
            pressedKeys.Remove(e.KeyCode);
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        private void pnlMain_Click(object sender, EventArgs e)
        {
        }

        internal static void SetLaser(Laser las)
        {
            laser = las;
        }

        public void LaunchDoomsdayEvent()
        {
            exploding = true;
            finalKey = false;
            SoundSystem.playSound("QuestOfWonders.Resources.Explosion.wav", false);
        }
    }
}
