using System.Drawing.Text;
using System.Text.Json;
using PacManGame.GameObjects;
using PacManGame.GameObjects.Ghosts;
using PacManGame.Levels;

namespace PacManGame;

public sealed class World : IWorld
{
    private int frame;

    public World()
    {
        Walls = WorldFactory.CreateWalls(this);
        Player = new Player();
        ImageMap = Directory.GetFiles("Pictures", "*.png")
            .ToDictionary(Path.GetFileNameWithoutExtension, fileName => Image.FromFile(fileName));
        IsPaused = false;
        LoadLevel();
        if (!File.Exists("highscore.txt"))
            File.WriteAllText("highscore.txt", "0");
    }

    public Level? Level { get; set; }
    public List<PacDot> PacDots { get; set; }
    public List<PowerPallet> PowerPallets { get; set; }
    public bool IsPoweredUp { get; set; }
    public List<Wall> Walls { get; }
    public List<Fruit> Fruits { get; set; }
    public Player Player { get; set; }
    public Pacman Pacman { get; set; }
    public Blinky Blinky { get; set; }
    public DateTime FrightenedStartTime { get; set; }
    public TimeSpan FrightenedTime { get; set; }
    public DateTime GameStartTime { get; set; }
    public DateTime PauseTime { get; set; }
    public bool IsPaused { get; set; }
    public DateTime PacmanDeathTime { get; set; }
    public List<Ghost> Ghosts { get; set; }
    public IDictionary<string, Image> ImageMap { get; }
    public TimeSpan NextModeChangeTime { get; set; }
    public int ModeDurationIndex { get; set; }
    private int ElroyModeCounter { get; set; }
    public int Highscore { get; set; }
    public GhostMode CurrentGhostMode { get; set; }
    public int eatenGhosts { get; set; }
    public int[] GhostModeTime { get; set; }
    public Control Control { get; set; }

    public void LoadLevel()
    {
        Level[] levels = JsonSerializer.Deserialize<Level[]>(File.ReadAllText(@"Levels\Level.json"));
        Level = levels.Where(level => level.Levels.Contains(Player.Level)).FirstOrDefault();
        PacDots = WorldFactory.CreatePacDots(this);
        PowerPallets = WorldFactory.CreatePowerPallets(this);
        Fruits = WorldFactory.CreateFruits(this);
        Pacman = new Pacman(this, (float)(Level.PacmanSpeed / 100 * 3));
        IsPoweredUp = false;
        Blinky = new Blinky(this, (float)(Level.GhostSpeed / 100 * 3), Level.FrightenedSpeed);
        Ghosts = new List<Ghost>
        {
            Blinky,
            new Inky(this, Level.GhostSpeed / 100 * 3, Level.FrightenedSpeed),
            new Pinky(this, Level.GhostSpeed / 100 * 3, Level.FrightenedSpeed),
            new Clyde(this, Level.GhostSpeed / 100 * 3, Level.FrightenedSpeed)
        };
        Highscore = int.Parse(File.ReadAllText("highscore.txt"));
        GameStartTime = DateTime.Now + TimeSpan.FromSeconds(3);
        ModeDurationIndex = 0;
        CurrentGhostMode = GhostMode.Chase;
        NextModeChangeTime = TimeSpan.Zero;
        ElroyModeCounter = Level.ElroyModeCounter;
        FrightenedTime = TimeSpan.FromSeconds(Level.FrightenedTime);
        GhostModeTime = Level.GhostModeTime;
    }
    
    public void Draw(PaintEventArgs eventArgs)
    {
        foreach (var pacDot in PacDots)
            pacDot.Draw(eventArgs);
        foreach (var wall in Walls)
            wall.Draw(eventArgs);
        foreach (var powerPallet in PowerPallets)
            powerPallet.Draw(eventArgs);
        if(PacDots.Count <= 170 && !(PacmanDeathTime + TimeSpan.FromSeconds(5) > DateTime.Now))
            foreach (var fruit in Fruits)
                fruit.Draw(eventArgs);
        
        
        PrivateFontCollection collection = new PrivateFontCollection();
        collection.AddFontFile(@"Fonts\ARCADECLASSIC.TTF");
        var fontFamily = new FontFamily("ArcadeClassic", collection);
        var font = new Font(fontFamily, 40, FontStyle.Regular, GraphicsUnit.Pixel);

        foreach (var ghost in Ghosts.Where(g => g.GhostMode != GhostMode.Off))
        {
            ghost.Draw(eventArgs);
        }

        Pacman.Draw(eventArgs);
        if (Player.Life >= 1)
            Pacman.Draw(eventArgs, 50, 850);
        if (Player.Life >= 2)
            Pacman.Draw(eventArgs, 105, 850);
        if (Player.Life >= 3)
            Pacman.Draw(eventArgs, 160, 850);
        eventArgs.Graphics.DrawString(Player.Score.ToString(), font, Brushes.White, 90,40);
        eventArgs.Graphics.DrawString(Player.Level.ToString() + "UP", font, Brushes.White, 100,10);
        eventArgs.Graphics.DrawString("High     Score", font, Brushes.White, 240,10);
        eventArgs.Graphics.DrawString(Highscore.ToString(), font, Brushes.White, 290,40);
        if (Player.Lose)
        {
            eventArgs.Graphics.DrawString("GameOver", new Font(fontFamily, 60, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Yellow, 205,310);
        }

        if (GameStartTime >= DateTime.Now)
        {
            eventArgs.Graphics.DrawString("Player", new Font(fontFamily, 60, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Yellow, 240,310);
            eventArgs.Graphics.DrawString("Ready!", new Font(fontFamily, 60, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Yellow, 240,460);
        }

        if (PacmanDeathTime >= DateTime.Now && !Player.Lose)
        {
            eventArgs.Graphics.DrawString("Ready!", new Font(fontFamily, 60, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Yellow, 240,460);
        }

        foreach (var ghost in Ghosts)
        {
            if (ghost.IsReleased) continue;
            if (ghost.GetType() == typeof(Clyde))
            {
                ghost.Draw(eventArgs, 275,375);
            }
            if (ghost.GetType() == typeof(Inky))
            {
                ghost.Draw(eventArgs, 375,375);
            }
            if (ghost.GetType() == typeof(Pinky))
            {
                ghost.Draw(eventArgs, 325,375);
            }
        }
    }

    public void Tick(PaintEventArgs eventArgs)
    {
        var collection = new PrivateFontCollection();
        collection.AddFontFile(@"Fonts\ARCADECLASSIC.TTF");
        var fontFamily = new FontFamily("ArcadeClassic", collection);
        var font = new Font(fontFamily, 32, FontStyle.Regular, GraphicsUnit.Pixel);
        
        if (IsPaused)
        {
            if (Control is Control.Esc or Control.Enter)
            {
                NextModeChangeTime += DateTime.Now - PauseTime;
                FrightenedTime += DateTime.Now - PauseTime;
                IsPaused = false;
                if(Control == Control.Enter)
                {
                    Player = new Player();
                    GameStartTime = DateTime.Now;
                    LoadLevel();
                }else
                    Control = Control.Off;
            }
            else return;
        }

        if (Control == Control.Esc)
        {
            PauseTime = DateTime.Now;
            IsPaused = true;
            Control = Control.Off;
        }

        if (!IsPoweredUp && Control == Control.PowerUp)
        {
            Pacman.speed *= 2;
            Pacman.HitBox.padding = 50;
            IsPoweredUp = true;
            Control = Control.Off;
        }

        if (IsPoweredUp && Control == Control.PowerUp)
        {
            IsPoweredUp = false;
            Pacman.speed /= 2;
        }

        if (PacDots.Count == 0)
        {
            foreach (var wall in Walls)
            {
                wall.Draw(eventArgs);
            }
            Player.Level++;
            LoadLevel();
        }

        if (Player.Lose)
        {
            if (Player.Score > Highscore)
            {
                File.WriteAllText("highscore.txt", Player.Score.ToString());
            }

            if (Control == Control.Enter)
            {
                Player = new Player();
                GameStartTime = DateTime.Now;
                LoadLevel();
            }else return;
        }
        if(GameStartTime >= DateTime.Now)  return;
        if (PacmanDeathTime >= DateTime.Now)
        {
            return;
        }

        if (PacDots.Count == ElroyModeCounter)
        {
            Blinky.ElroyMode();
        }

        foreach (var ghost in Ghosts)
        {
            ghost.SetToNextTurn();
            ghost.CheckGhostMode();
            ghost.ReleaseGhost();
            
            if(ModeDurationIndex <= 7)
                ghost.GhostModeTimer(GhostModeTime);

            if (ghost.WouldOverlap(Pacman.HitBox) && ghost.GhostMode == GhostMode.Frightened)
            {
                ghost.Die();
                ghost.deathTimer = DateTime.Now + TimeSpan.FromSeconds(1.5);
            }

            if (DateTime.Now <= ghost.deathTimer)
            {
                eventArgs.Graphics.DrawString(ghost.eatenGhostPoints.ToString(), new Font(fontFamily, 40, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.White, ghost.currentXPosition,ghost.currentYPosition);
            }

            if (Pacman.HitBox.WouldOverlap(ghost) && ghost.GhostMode is not (GhostMode.Frightened or GhostMode.Home) && !IsPoweredUp)
                Pacman.Die();
        }


        Pacman.CouldTurn(Pacman.viewangle, Pacman.nextViewangle);
        if (!Pacman.WouldHitWall(Pacman.nextViewangle))
        {
            Pacman.viewangle = Pacman.nextViewangle;
            Pacman.nextViewangle = ViewAngle.None;
        }

        if (!Pacman.WouldHitWall(Pacman.viewangle))
            Pacman.Move();
        

        Pacman.CollectPacDots();
        Pacman.CollectFruits();
        Pacman.CollectPowerPallets();

        frame++;
        if (frame is 5 or 10)
        {
            Pacman.AnimateActor();
        }

        if (frame == 10)
        {
            foreach (var ghost in Ghosts)
            {
                ghost.AnimateActor();
            }

            frame = 0;
        }

        Control = Control.Off;
    }

  
}