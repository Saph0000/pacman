namespace PacManGame.GameObjects.Ghosts;

public abstract class Ghost : GameActor
{
    private static readonly Random Random = new();
    protected int targetXPosition;
    protected int targetYPosition;
    
    public GhostMode GhostMode { get; set; }
    
    protected Ghost(IWorld world, int xPosition, int yPosition, int width, int height) : base(world, xPosition, yPosition, width, height)
    {
        SetGhostImage();
    }

    protected double CalculateDistance(int distanceX, int distanceY)
    {
        return Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
    }

    protected void GhostDecision(int targetXPosition, int targetYPosition)
    {
        var upDistance = double.PositiveInfinity;
        var downDistance = double.PositiveInfinity;
        var rightDistance = double.PositiveInfinity;
        var leftDistance = double.PositiveInfinity;
        
        foreach (var viewAngle in CheckDirection(viewangle))
        {
            switch (viewAngle)
            {
                case ViewAngle.Up:
                    upDistance = CalculateDistance(targetXPosition - XPosition, targetYPosition - (YPosition - 50));
                    break;
                case ViewAngle.Down:
                    downDistance = CalculateDistance(targetXPosition - XPosition, targetYPosition - (YPosition + 50));
                    break;
                case ViewAngle.Right:
                    rightDistance = CalculateDistance(targetXPosition - (XPosition + 50), targetYPosition - YPosition);
                    break;
                case ViewAngle.Left:
                    leftDistance = CalculateDistance(targetXPosition - (XPosition - 50), targetYPosition - YPosition);
                    break;

            }
        }

        var distance = Min(upDistance, downDistance, leftDistance, rightDistance);

        if (distance == upDistance)
        {
            viewangle = ViewAngle.Up;
            Move();
        }

        if (distance == downDistance)
        {
            viewangle = ViewAngle.Down;
            Move();
        }

        if (distance == rightDistance)
        {
            viewangle = ViewAngle.Right;
            Move();
        }

        if (distance != leftDistance) return;
        viewangle = ViewAngle.Left;
        Move();
    }

    private static double Min(params double[] values) => 
        values.Min();
    
    public void Frightened()
    {
        speed /= 2;
        var possibleDirections = CheckDirection(viewangle);
        viewangle = possibleDirections[Random.Next(0, possibleDirections.Count - 1)];
        CouldTurn(viewangle, nextViewangle);
        if(!WouldHitWall(viewangle))
            Move();
        left = right = up = down = new[] { "ghost_Frightened (1)", "ghost_Frightened (2)" };
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
        }
    }

    public void SetGhostImage()
    {
        image = Image.FromFile($@"pictures\{ImageName}_Left (1).png");
        left = new[] { $"{ImageName}_Left (2)", $"{ImageName}_Left (1)" };
        right = new[] { $"{ImageName}_Right (2)", $"{ImageName}_Right (1)" };
        up = new[] { $"{ImageName}_Up (2)", $"{ImageName}_Up (1)" };
        down = new[]{ $"{ImageName}_Down (2)", $"{ImageName}_Down (1)"};
    }

    public abstract void Chase(Pacman pacman, Ghost blinky);
    public abstract void Scatter();
    
    protected abstract string ImageName { get; }
}
