﻿namespace PacManGame.GameObjects;

public abstract class GameObject
{
    public IWorld World { get; set; }
    public float XPosition { get; set; }
    public float YPosition { get; set; }
    public int Width { get; init; }
    public int Height { get; init; }
    public string FruitImage { get; set; }
    
    protected bool WouldHitObject(GameObject gameObject, ViewAngle viewAngle, float xPlus = 0, float yPlus = 0) =>
        viewAngle switch
        {
            ViewAngle.Left => WouldOverlap(gameObject, -1, 0, xPlus, yPlus),
            ViewAngle.Right => WouldOverlap( gameObject, 1, 0, xPlus, yPlus),
            ViewAngle.Up => WouldOverlap(gameObject, 0, -1, xPlus, yPlus),
            ViewAngle.Down => WouldOverlap(gameObject, 0, 1, xPlus, yPlus),
            _ => true
        };

    public bool WouldOverlap(GameObject gameObject, int xDelta = 0, int yDelta = 0, float xPlus = 0, float yPlus = 0)
    {
        var leftX = Math.Max(XPosition + xPlus + xDelta, gameObject.XPosition);
        var rightX = Math.Min(XPosition + xPlus + xDelta + Width, gameObject.XPosition + gameObject.Width);
        var topY= Math.Max(YPosition + yPlus + yDelta, gameObject.YPosition);
        var bottomY = Math.Min(YPosition + yPlus + yDelta + Height, gameObject.YPosition + gameObject.Height);
        return leftX < rightX && topY < bottomY;
    }
    
    public virtual void Draw(PaintEventArgs paintEventArgs)
    {
    }
}