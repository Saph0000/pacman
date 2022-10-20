using PacManGame.Helper;

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
        SetGhostImage();
        XPosition = xStartPosition;
        YPosition = yStartPosition;
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
                    upDistance = Maths.CalculateDistance(targetXPosition, targetYPosition,  XPosition, YPosition - 50);
                    break;
                case ViewAngle.Down:
                    downDistance = Maths.CalculateDistance(targetXPosition,  targetYPosition, XPosition, YPosition + 50);
                    break;
                case ViewAngle.Right:
                    rightDistance = Maths.CalculateDistance(targetXPosition, targetYPosition, XPosition + 50, YPosition);
                    break;
                case ViewAngle.Left:
                    leftDistance = Maths.CalculateDistance(targetXPosition, targetYPosition,  XPosition - 50, YPosition);
                    break;

            }
        }

        var distance = Maths.Min(upDistance, downDistance, leftDistance, rightDistance);

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

    public void Frightened()
    {
        speed /= 2;
        var possibleDirections = CheckDirection(viewangle);
        viewangle = possibleDirections.Any() ? possibleDirections[Random.Next(0, possibleDirections.Count)] : ViewAngle.None;
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
            case GhostMode.Home:
                Home();
                break;
        }
    }

    private void Home()
    {
        left = new[] { "home_Left", "home_Left" };
        right =  new[] { "home_Right", "home_Right" };
        up = new[] { "home_Up", "home_Up" };
        down = new[] { "home_Down", "home_Down" };
        
        speed = 8;
        targetXPosition = XStartPosition;
        targetYPosition = YStartPosition;
        GhostDecision(targetXPosition, targetYPosition);
        if (XPosition == targetXPosition && YPosition == targetYPosition)
        {
            GhostMode = GhostMode.Chase;
            speed = currentSpeed;
            SetGhostImage();
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

    public override void Die()
    {
        GhostMode = GhostMode.Home;
        SetGhostImage();
    }
}
