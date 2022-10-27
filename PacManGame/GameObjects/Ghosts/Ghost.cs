using static PacManGame.Helper.Maths;

namespace PacManGame.GameObjects.Ghosts;

public abstract class Ghost : GameActor
{
    private static readonly Random Random = new();
    public int targetXPosition;
    public int targetYPosition;
    protected int currentSpeed;
    
    public GhostMode GhostMode { get; set; }
    public bool IsReleased { get; set; }

    protected Ghost(IWorld world, int xStartPosition, int yStartPosition, int width, int height) : base(world, xStartPosition, yStartPosition, width, height)
    {
        XPosition = xStartPosition;
        YPosition = yStartPosition;
        GhostMode = GhostMode.Off;
        IsReleased = false;
    }

    protected void GhostDecision(int targetXPosition, int targetYPosition)
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
        if (DateTime.Now - World.FrightenedStartTime >= TimeSpan.FromSeconds(7))
        {
            GhostMode = GhostMode.Chase;
        }


        maxFrame = DateTime.Now - World.FrightenedStartTime <= TimeSpan.FromSeconds(4) ? 2 : 0;
        speed /= 2;
        var possibleDirections = CheckDirection(viewangle, GhostMode);
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

    public virtual void ReleaseGhost()
    {
    }
    public virtual void GhostModeTimer(params int[] modeDurations)
    {
        if (DateTime.Now - World.GameStartTime < TimeSpan.FromSeconds(World.NextModeChangeTime)) return;
        World.NextModeChangeTime += modeDurations[World.ModeDurationIndex];
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
