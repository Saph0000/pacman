using System.Drawing.Text;
using PacManGame.GameObjects;
using PacManGame.GameObjects.Ghosts;

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
        Level = 1;
        LoadLevel();
        
    }

    public List<PacDot> PacDots { get; set; }
    public List<PowerPallet> PowerPallets { get; set; }
    public List<Wall> Walls { get; }
    public List<Fruit> Fruits { get; set; }
    public Player Player { get; set; }
    public Pacman Pacman { get; set; }
    public Blinky Blinky { get; set; }
    public DateTime FrightenedStartTime { get; set; }
    public DateTime GameStartTime { get; set; }
    public List<Ghost> Ghosts { get; set; }
    public IDictionary<string, Image> ImageMap { get; }
    public int NextModeChangeTime { get; set; }
    public int ModeDurationIndex { get; set; }
    private int ElroyModeCounter { get; set; }
    public GhostMode CurrentGhostMode { get; set; }
    public int eatenGhosts { get; set; }
    public int Level { get; set; }
    public int[] GhostModeTime { get; set; }

    public void LoadLevel()
    {
        PacDots = WorldFactory.CreatePacDots(this);
        PowerPallets = WorldFactory.CreatePowerPallets(this);
        Fruits = WorldFactory.CreateFruits(this);
        Pacman = new Pacman(this, 4);
        Blinky = new Blinky(this, 2);
        Ghosts = new List<Ghost>
        {
            Blinky,
            new Inky(this, 2),
            new Pinky(this, 2),
            new Clyde(this, 2)
        };

        GameStartTime = DateTime.Now + TimeSpan.FromSeconds(5);
        ModeDurationIndex = 0;
        NextModeChangeTime = 0;
        ElroyModeCounter = 20;
        GhostModeTime = new []{7,20,7,20,5,20,5,20};
    }
    
    public void Draw(PaintEventArgs eventArgs)
    {
        foreach (var pacDot in PacDots)
            pacDot.Draw(eventArgs);
        foreach (var wall in Walls)
            wall.Draw(eventArgs);
        foreach (var powerPallet in PowerPallets)
            powerPallet.Draw(eventArgs);
        if(PacDots.Count <= 170)
            foreach (var fruit in Fruits)
                fruit.Draw(eventArgs);
        
        
        PrivateFontCollection collection = new PrivateFontCollection();
        collection.AddFontFile(@"Fonts\ARCADECLASSIC.TTF");
        var fontFamily = new FontFamily("ArcadeClassic", collection);
        var font = new Font(fontFamily, 32, FontStyle.Regular, GraphicsUnit.Pixel);

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
        
        eventArgs.Graphics.DrawString(Player.Score.ToString(), font, Brushes.White, 10,10);
        eventArgs.Graphics.DrawString(Level.ToString(), font, Brushes.White, 100,10);
        //eventArgs.Graphics.DrawString(eatenGhosts.ToString(), font, Brushes.White, 100,10);
        
        if (Player.Lose)
        {
            eventArgs.Graphics.DrawString("GameOver", new Font(fontFamily, 60, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.White, 205,310);
        }

        if (GameStartTime >= DateTime.Now)
        {
            eventArgs.Graphics.DrawString("Player", new Font(fontFamily, 60, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.White, 240,310);
            eventArgs.Graphics.DrawString("Ready!", new Font(fontFamily, 60, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.White, 240,460);
        }

        foreach (var ghost in Ghosts)
        {
            if (!ghost.IsReleased)
            {
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
    }

    public void Tick(PaintEventArgs eventArgs)
    {
        PrivateFontCollection collection = new PrivateFontCollection();
        collection.AddFontFile(@"Fonts\ARCADECLASSIC.TTF");
        var fontFamily = new FontFamily("ArcadeClassic", collection);
        var font = new Font(fontFamily, 32, FontStyle.Regular, GraphicsUnit.Pixel);
        
        if (PacDots.Count == 0)
        {
            Level++;
            LoadLevel();
        }
        if (Player.Lose || GameStartTime >= DateTime.Now)  return;

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

            if (Pacman.HitBox.WouldOverlap(ghost) && ghost.GhostMode is not (GhostMode.Frightened or GhostMode.Home))
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
    }

  
}