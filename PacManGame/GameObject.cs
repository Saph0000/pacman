namespace PacManGame;

public abstract class GameObject
{
    public int xPosition;
    public int yPosition;
    public int width;
    public int height;
    public int totalPacDots = 240;
    
    public Player player = new Player();

    protected GameObject(int xPosition, int yPosition, int width, int height)
    {
        this.xPosition = xPosition;
        this.yPosition = yPosition;
        this.width = width;
        this.height = height;
    }

    protected bool WouldHitObject(GameObject gameObject, ViewAngle viewAngle, int xPlus = 0, int yPlus = 0) =>
        viewAngle switch
        {
            ViewAngle.Left => WouldOverlap(gameObject, -1, 0, xPlus, yPlus),
            ViewAngle.Right => WouldOverlap( gameObject, 1, 0, xPlus, yPlus),
            ViewAngle.Up => WouldOverlap(gameObject, 0, -1, xPlus, yPlus),
            ViewAngle.Down => WouldOverlap(gameObject, 0, 1, xPlus, yPlus),
            _ => true
        };

    protected bool WouldOverlap(GameObject gameObject, int xDelta = 0, int yDelta = 0, int xPlus = 0, int yPlus = 0)
    {
        var leftX = Math.Max(xPosition + xPlus + xDelta, gameObject.xPosition);
        var rightX = Math.Min(xPosition + xPlus + xDelta + width, gameObject.xPosition + gameObject.width);
        var topY= Math.Max(yPosition + yPlus + yDelta, gameObject.yPosition);
        var bottomY = Math.Min(yPosition + yPlus + yDelta + height, gameObject.yPosition + gameObject.height);
        return leftX < rightX && topY < bottomY;
    }

    
    public abstract void Draw(PaintEventArgs paintEventArgs);
}