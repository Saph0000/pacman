using System.Drawing.Text;
using PacManGame.GameObjects;
using PacManGame.GameObjects.Ghosts;
using Timer = System.Threading.Timer;

namespace PacManGame;

public sealed class World : IWorld
{
    private int frame;

    public World()
    {
        PacDots = WorldFactory.CreatePacDots(this);
        PowerPallets = WorldFactory.CreatePowerPallets(this);
        Walls = WorldFactory.CreateWalls(this);
        Pacman = new Pacman(this);
        Blinky = new Blinky(this);
        Ghosts = new List<Ghost>
        {
            Blinky,
            new Inky(this),
            new Pinky(this),
            new Clyde(this)
        };
        Player = new Player();
        ImageMap = Directory.GetFiles("Pictures", "*.png")
            .ToDictionary(Path.GetFileNameWithoutExtension, fileName => Image.FromFile(fileName));
        

        GameStartTime = DateTime.Now;
        ModeDurationIndex = 0;
        NextModeChangeTime = 0;
        ElroyModeCounter = 20;
    }

    public List<PacDot> PacDots { get; }
    public List<PowerPallet> PowerPallets { get; }
    public List<Wall> Walls { get; }
    public Player Player { get; set; }
    public Pacman Pacman { get; }
    public Blinky Blinky { get; }
    public DateTime FrightenedStartTime { get; set; }
    public DateTime GameStartTime { get; set; }
    public List<Ghost> Ghosts { get; }
    public IDictionary<string, Image> ImageMap { get; }
    public int NextModeChangeTime { get; set; }
    public int ModeDurationIndex { get; set; }
    private int ElroyModeCounter { get; }
    public GhostMode CurrentGhostMode { get; set; }
    public int eatenGhosts { get; set; }
    
    public void Draw(PaintEventArgs eventArgs)
    {
        foreach (var pacDot in PacDots)
            pacDot.Draw(eventArgs);
        foreach (var wall in Walls)
            wall.Draw(eventArgs);
        foreach (var powerPallet in PowerPallets)
            powerPallet.Draw(eventArgs);
        
        
        PrivateFontCollection collection = new PrivateFontCollection();
        collection.AddFontFile(@"Fonts\ARCADECLASSIC.TTF");
        var fontFamily = new FontFamily("ArcadeClassic", collection);
        var font = new Font(fontFamily, 32, FontStyle.Regular, GraphicsUnit.Pixel);

        foreach (var ghost in Ghosts)
        {
            ghost.Draw(eventArgs);
        }

        Pacman.Draw(eventArgs);
        eventArgs.Graphics.DrawString(Player.Score.ToString(), font, Brushes.White, 10,10);
        eventArgs.Graphics.DrawString(Player.Life.ToString(), font, Brushes.White, 100,10);
        
    }

    public void Tick()
    {
        if (PacDots.Count == 0 || Player.Lose)  return;
        foreach (var ghost in Ghosts)
        {
            if(ModeDurationIndex <= 7)
                ghost.GhostModeTimer(7, 20, 7, 20, 5, 20, 5, 20);
        }

        if (PacDots.Count == ElroyModeCounter)
        {
            Blinky.ElroyMode();
        }

        foreach (var ghost in Ghosts)
        {

            if (ghost.WouldOverlap(Pacman) && ghost.GhostMode == GhostMode.Frightened)
            {
                switch (eatenGhosts)
                {
                    case 0 :
                        Player.Score += 200;
                        eatenGhosts += 1;
                        break;
                    case 1:
                        Player.Score += 400;
                        eatenGhosts += 1;
                        break;
                    case 2:
                        Player.Score += 800;
                        eatenGhosts += 1;
                        break;
                    case 3:
                        Player.Score += 1600;
                        eatenGhosts += 1;
                        break;
                        
                }
                //score += kumulierender Wert.
                ghost.Die();
                
            }
            if (Pacman.WouldOverlap(ghost) && !(ghost.GhostMode is GhostMode.Frightened or GhostMode.Home))
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
        foreach (var ghost in Ghosts)
        {
            ghost.SetToNextTurn();
            ghost.CheckGhostMode();
            ghost.ReleaseGhost();
        }

        Pacman.CollectDots();
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