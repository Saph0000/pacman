using PacManGame.GameObjects;
using PacManGame.GameObjects.Ghosts;

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
    }

    public List<PacDot> PacDots { get; }
    public List<PowerPallets> PowerPallets { get; } 
    public List<Wall> Walls { get; }
    public Pacman Pacman {  get; }
    public Blinky Blinky {  get; }
    public List<Ghost> Ghosts { get; }
    public DateTime FrightenedStartTime { get; set; }

    public void Draw(PaintEventArgs eventArgs)
    {
        foreach (var pacDot in PacDots)
            pacDot.Draw(eventArgs);
        foreach (var wall in Walls)
            wall.Draw(eventArgs);
        foreach (var powerPallet in PowerPallets)
            powerPallet.Draw(eventArgs);
        foreach (var ghost in Ghosts)
            ghost.Draw(eventArgs);
        Pacman.Draw(eventArgs);
    }

    public void Tick()
    {
        foreach (var ghost in Ghosts.Where(ghost => ghost.GhostMode == GhostMode.Frightened))
        {
            if (DateTime.Now - FrightenedStartTime >= TimeSpan.FromSeconds(7))
            {
                ghost.GhostMode = GhostMode.Chase;
                ghost.SetGhostImage();
            }

            if (ghost.WouldOverlap(Pacman))
            {
                //score += kumulierender Wert.
                ghost.Die();
            }    
        }
        
        
        Pacman.CouldTurn( Pacman.viewangle,  Pacman.nextViewangle);
        if (! Pacman.WouldHitWall(Pacman.nextViewangle))
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
        }

        Pacman.CollectDots();
        Pacman.CollectPowerPallets();
        
        frame++;
        if (frame is 5 or 10 )
        {
            Pacman.DrawActor(3);
        }

        if (frame == 10)
        {
            foreach (var ghost in Ghosts)
                ghost.DrawActor(2);
            frame = 0;
        }
    }
}