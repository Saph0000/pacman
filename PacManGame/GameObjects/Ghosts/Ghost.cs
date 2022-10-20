using static PacManGame.Helper.Maths;

namespace PacManGame.GameObjects.Ghosts;

public abstract class Ghost : GameActor
{
    private static readonly Random Random = new();
    protected int targetXPosition;
    protected int targetYPosition;
    protected int currentSpeed;
    
    public GhostMode GhostMode { get; set; }
    
    protected Ghost(IWorld world, int xStartPosition, int yStartPosition, int width, int height) : base(world, xStartPosition, yStartPosition, width, height)
    {
        XPosition = xStartPosition;
        YPosition = yStartPosition;
    }

    protected void GhostDecision(int targetXPosition, int targetYPosition)
    {
        var minDistance = double.PositiveInfinity;
        var minDistanceViewAngle = ViewAngle.None;
        foreach (var viewAngle in CheckDirection(viewangle))
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
        if (DateTime.Now - World.FrightenedStartTime >= TimeSpan.FromSeconds(7))
        {
            GhostMode = GhostMode.Chase;
        }

        maxFrame = DateTime.Now - World.FrightenedStartTime <= TimeSpan.FromSeconds(4) ? 2 : 0;
        speed /= 2;
        var possibleDirections = CheckDirection(viewangle);
        viewangle = possibleDirections.Any() ? possibleDirections[Random.Next(0, possibleDirections.Count)] : ViewAngle.None;
        CouldTurn(viewangle, nextViewangle);
        if(!WouldHitWall(viewangle))
            Move();
        speed *= 2;
        
        
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
            default:
                break;
        }
    }

    private void Home()
    {
        speed = 8;
        targetXPosition = xStartPosition;
        targetYPosition = yStartPosition;
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
        GhostMode = GhostMode.Home;
    }
}
