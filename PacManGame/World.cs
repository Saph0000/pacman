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
    public Pacman Pacman { get; }
    public Blinky Blinky { get; }
    public DateTime FrightenedStartTime { get; set; }
    public DateTime GameStartTime { get; set; }
    public List<Ghost> Ghosts { get; }
    public IDictionary<string, Image> ImageMap { get; }
    public int NextModeChangeTime { get; set; }
    private int ModeDurationIndex { get; set; }
    private int ElroyModeCounter { get; }
    
    public void Draw(PaintEventArgs eventArgs)
    {
        foreach (var pacDot in PacDots)
            pacDot.Draw(eventArgs);
        foreach (var wall in Walls)
            wall.Draw(eventArgs);
        foreach (var powerPallet in PowerPallets)
            powerPallet.Draw(eventArgs);
        foreach (var ghost in Ghosts)
        {
            ghost.Draw(eventArgs);
            eventArgs.Graphics.FillEllipse(Brushes.Coral, ghost.targetXPosition, ghost.targetYPosition, 10, 10);

        }

        Pacman.Draw(eventArgs);

    }

    public void Tick()
    {
        if(ModeDurationIndex <= 7)
            GhostModeTimer(7, 20, 7, 20, 5, 20, 5, 20);

        if (PacDots.Count == ElroyModeCounter)
        {
            Blinky.ElroyMode();
        }

        foreach (var ghost in Ghosts.Where(ghost => ghost.GhostMode == GhostMode.Frightened))
        {

            if (ghost.WouldOverlap(Pacman))
            {
                //score += kumulierender Wert.
                ghost.Die();
            }
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

    private void GhostModeTimer(params int[] modeDurations)
    {

        if (DateTime.Now - GameStartTime >= TimeSpan.FromSeconds(NextModeChangeTime))
        {
            NextModeChangeTime += modeDurations[ModeDurationIndex];
            foreach (var ghost in Ghosts)
            {
                switch (ghost.GhostMode)
                {
                    case GhostMode.Chase:
                        ghost.viewangle = ghost.viewangle.GetOppositeDirection();
                        ghost.GhostMode = GhostMode.Scatter;
                        break;
                    case GhostMode.Scatter:
                        ghost.viewangle = ghost.viewangle.GetOppositeDirection();
                        ghost.GhostMode = GhostMode.Chase;
                        break;
                }
            }

            ModeDurationIndex++;
        }
        
    }
}