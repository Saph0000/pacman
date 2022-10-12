namespace PacManGame;

public class Ghost : GameActor
{
    public int targetXPosition;
    public int targetYPosition;
    protected Ghost(int xPosition, int yPosition, int width, int height) : base(xPosition, yPosition, width, height)
    {
    }

    public double CalculateDistance(int distanceX, int distanceY)
    {
        return Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
    }
    
    public void GhostDecision(int targetXPosition, int targetYPosition)
    {
        var upDistance = double.PositiveInfinity;
        var downDistance = double.PositiveInfinity;
        var rightDistance = double.PositiveInfinity;
        var leftDistance = double.PositiveInfinity;
        
        foreach (var viewAngle in checkDirection(viewangle))
        {
            switch (viewAngle)
            {
                case ViewAngle.Up:
                    upDistance = CalculateDistance(targetXPosition - xPosition, targetYPosition - (yPosition - 50));
                    break;
                case ViewAngle.Down:
                    downDistance = CalculateDistance(targetXPosition - xPosition, targetYPosition - (yPosition + 50));
                    break;
                case ViewAngle.Right:
                    rightDistance = CalculateDistance(targetXPosition - (xPosition + 50), targetYPosition - yPosition);
                    break;
                case ViewAngle.Left:
                    leftDistance = CalculateDistance(targetXPosition - (xPosition - 50), targetYPosition - yPosition);
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

        if (distance == leftDistance)
        {
            viewangle = ViewAngle.Left;
            Move();
        }
    }

    private static double Min(params double[] values) => 
        values.Min();
}