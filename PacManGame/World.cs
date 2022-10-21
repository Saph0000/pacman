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
    public int TotalFrightenedTime { get; set; }
    
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

    private void StartGame()
    {
        GameStartTime = DateTime.Now;
    }

    public void Tick()
    {
        if(DateTime.Now -GameStartTime <= TimeSpan.FromSeconds(90))
            GhostModeTimer(7, 20, 7, 20, 5, 20, 5);


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

    private void GhostModeTimer(int firstScatter,int firstChase, int secondScatter, int secondChase,
        int thirdScatter, int thirdChase,int fourthScatter)
    {
        foreach (var ghost in Ghosts.Where(ghost => ghost.GhostMode is not (GhostMode.Frightened or GhostMode.Home)))
        {


            if (TimeDuration(TotalFrightenedTime + firstScatter, firstChase))
                ghost.GhostMode = GhostMode.Chase;
            if(TimeDuration(TotalFrightenedTime + firstScatter + firstChase,secondScatter))
                ghost.GhostMode = GhostMode.Scatter;
            if (TimeDuration(TotalFrightenedTime + firstScatter + firstChase + secondScatter,secondChase))
                ghost.GhostMode = GhostMode.Chase;
            if(TimeDuration(TotalFrightenedTime + firstScatter + firstChase + secondScatter + secondChase,thirdScatter))
                ghost.GhostMode = GhostMode.Scatter;
            if (TimeDuration(TotalFrightenedTime + firstScatter + firstChase + secondScatter + secondChase + thirdScatter,thirdChase))
                ghost.GhostMode = GhostMode.Chase;
            if(TimeDuration(TotalFrightenedTime + firstScatter + firstChase + secondScatter + secondChase + thirdScatter + thirdChase,fourthScatter))
                ghost.GhostMode = GhostMode.Scatter;
            if(TimeDuration(TotalFrightenedTime + firstScatter + firstChase + secondScatter + secondChase + thirdScatter + thirdChase + fourthScatter,1))
                ghost.GhostMode = GhostMode.Chase;
            
                

        }
        
    }

    private bool TimeDuration(int pastTime, int duration)
    {
        return DateTime.Now - GameStartTime > TimeSpan.FromSeconds(pastTime) &&
               DateTime.Now - GameStartTime <= TimeSpan.FromSeconds(pastTime + duration);
    }
}