using static PacManGame.Helper.Maths;
using Timer = System.Threading.Timer;

namespace PacManGame.GameObjects.Ghosts;

public abstract class Ghost : GameActor
{
    private static readonly Random Random = new();
    public float targetXPosition;
    public float targetYPosition;
    protected float currentSpeed;
    public float currentXPosition;
    public float currentYPosition;
    public int eatenGhostPoints;
    public DateTime deathTimer;
    public float frightenedSpeed;
    
    
    public GhostMode GhostMode { get; set; }
    public bool IsReleased { get; set; }

    protected Ghost(IWorld world, int xStartPosition, int yStartPosition, int width, int height, float frightenedSpeed) : base(world, xStartPosition, yStartPosition, width, height)
    {
        XPosition = xStartPosition;
        YPosition = yStartPosition;
        GhostMode = GhostMode.Off;
        IsReleased = false;
    }

    protected void GhostDecision(float targetXPosition, float targetYPosition)
    {
        var minDistance = double.PositiveInfinity;
        var minDistanceViewAngle = ViewAngle.None;
        foreach (var viewAngle in CheckDirection(viewangle, GhostMode))
        {
            var distance = viewAngle switch
            {
                ViewAngle.Up => CalculateDistance(targetXPosition, targetYPosition, XPosition, YPosition - 50),
                ViewAngle.Down => CalculateDistance(targetXPosition, targetYPosition, XPosition, YPosition + 50),
                ViewAngle.Right => CalculateDistance(targetXPosition, targetYPosition, XPosition + 50, YPosition),
                ViewAngle.Left => CalculateDistance(targetXPosition, targetYPosition, XPosition - 50, YPosition),
                _ => double.PositiveInfinity
            };
            if (distance < minDistance)
            {
                minDistance = distance;
                minDistanceViewAngle = viewAngle;
            }
        }
        viewangle = minDistanceViewAngle;
        Move();
    }

    public void Frightened()
    {
        if (DateTime.Now - World.FrightenedStartTime >= World.FrightenedTime)
        {
            GhostMode = GhostMode.Chase;
        }


        maxFrame = DateTime.Now - World.FrightenedStartTime <= World.FrightenedTime - TimeSpan.FromSeconds(3) ? 2 : 0;
        speed *= frightenedSpeed / 100;
        var possibleDirections = CheckDirection(viewangle, GhostMode);
        viewangle = possibleDirections.Any() ? possibleDirections[Random.Next(0, possibleDirections.Count)] : ViewAngle.None;
        CouldTurn(viewangle, nextViewangle);
        if(!WouldHitWall(viewangle))
            Move();
        speed = currentSpeed;
        
        
    }

    public void CheckGhostMode()
    {
        switch (GhostMode)
        {
            case GhostMode.Chase:
                Chase(World.Pacman, World.Blinky);
                break;
            case GhostMode.Scatter:
                Scatter();
                break;
            case GhostMode.Frightened:
                Frightened();
                break;
            case GhostMode.Home:
                Home();
                break;
            case GhostMode.Off:
                
                break;
        }
    }

    private void Home()
    {
        speed = 6;
        targetXPosition = 325;
        targetYPosition = 375;
        GhostDecision(targetXPosition, targetYPosition);
        if (XPosition == targetXPosition && YPosition == targetYPosition)
        {
            GhostMode = GhostMode.Chase;
            speed = currentSpeed;
        }
    }

    protected override string[] GetImageNames() =>
        GhostMode switch
        {
            GhostMode.Frightened => new[]
            {
                "ghost_Frightened (1)", "ghost_Frightened (2)", "ghost_Frightened (3)", "ghost_Frightened (4)"
            },
            GhostMode.Home => new[] { $"home_{viewangle}" },
            _ => new[] { $"{ImageName}_{viewangle} (1)", $"{ImageName}_{viewangle} (2)" }
        };

    protected abstract void Chase(Pacman pacman, Ghost blinky);
    protected abstract void Scatter();
    
    protected abstract string ImageName { get; }

    public override void Die()
    {
        currentXPosition = XPosition;
        currentYPosition = YPosition;
        GhostMode = GhostMode.Home;
        switch (World.eatenGhosts)
        {
            case 0 :
                eatenGhostPoints = 200;
                World.eatenGhosts += 1;
                break;
            case 1:
                eatenGhostPoints = 400;
                World.eatenGhosts += 1;
                break;
            case 2:
                eatenGhostPoints = 800;
                World.eatenGhosts += 1;
                break;
            case 3:
                eatenGhostPoints = 1600;
                World.eatenGhosts += 1;
                break;
                        
        }

        World.Player.Score += eatenGhostPoints;
    }

    public virtual void ReleaseGhost()
    {
    }
    public virtual void GhostModeTimer(params int[] modeDurations)
    {
        if (DateTime.Now - World.GameStartTime < World.NextModeChangeTime) return;
        World.NextModeChangeTime += TimeSpan.FromSeconds(modeDurations[World.ModeDurationIndex]);
        switch (World.CurrentGhostMode)
        {
            case GhostMode.Chase:
                World.CurrentGhostMode = GhostMode.Scatter;
                break;
            case GhostMode.Scatter:
                World.CurrentGhostMode = GhostMode.Chase;
                break;
        }
        foreach (var ghost in World.Ghosts)
        {
            if (ghost.GhostMode == GhostMode.Off) continue;
            
            ghost.viewangle = ghost.viewangle.GetOppositeDirection();
            ghost.GhostMode = World.CurrentGhostMode;

        }

        World.ModeDurationIndex++;

    }
}
