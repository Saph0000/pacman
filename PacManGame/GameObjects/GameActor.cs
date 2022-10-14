namespace PacManGame.GameObjects;

public abstract class GameActor : GameObject
{
    public ViewAngle viewangle;
    public ViewAngle nextViewangle;
    public int speed;
    private bool isDead;
    public int currFrame = 0;
    public Image image;
    public Image baseImage;
    public string[] left;
    public string[] right;
    public string[] up;
    public string[] down;
    
    protected GameActor(IWorld world, int xPosition, int yPosition, int width, int height) : base(world, xPosition, yPosition, width, height)
    {
    }

    public void DrawActor(int maxFrames)
    {
        image = viewangle switch
        {
            ViewAngle.None => baseImage,
            ViewAngle.Right => Image.FromFile(@"pictures\" + right[currFrame] + ".png"),
            ViewAngle.Left => Image.FromFile(@"pictures\" + left[currFrame] + ".png"),
            ViewAngle.Up => Image.FromFile(@"pictures\" + up[currFrame] + ".png"),
            ViewAngle.Down => Image.FromFile(@"pictures\" + down[currFrame] + ".png"),
            _ => image
        };

        currFrame++;
        if (currFrame == maxFrames)
            currFrame = 0;
    }
    public void Move()
    {
        XPosition = GetXPositionAfterMove(viewangle);
        YPosition = GetYPositionAfterMove(viewangle);
        var x = WouldHitWall(viewangle, out var hitWall);
        if (x)
        {
            switch (viewangle)
            {
                case ViewAngle.Right:
                    XPosition = hitWall.XPosition - Width;
                    break;
                case ViewAngle.Left:
                    XPosition = hitWall.XPosition + hitWall.Width;
                    break;
                case ViewAngle.Up:
                    YPosition = hitWall.YPosition + hitWall.Height;
                    break;
                case ViewAngle.Down:
                    YPosition = hitWall.YPosition - Height;
                    break;
            }
        }
        if (XPosition > Settings.ScreenWidth)
            XPosition = -Width;
        else if (XPosition < -Width)
            XPosition = Settings.ScreenWidth;
        if (YPosition > Settings.ScreenHeight)
            YPosition = Height;
        else if (YPosition < 0)
            YPosition = Settings.ScreenHeight;
    }

    private int GetYPositionAfterMove(ViewAngle viewangle) =>
        viewangle switch
        {
            ViewAngle.Up => YPosition - speed,
            ViewAngle.Down => YPosition + speed,
            _ => YPosition
        };

    private int GetXPositionAfterMove(ViewAngle viewangle) =>
        viewangle switch
        {
            ViewAngle.Right => XPosition + speed,
            ViewAngle.Left => XPosition - speed,
            _ => XPosition
        };

    public void CouldTurn(ViewAngle currentViewAngle, ViewAngle nextViewAngle)
    {
        if (!WouldHitWall(nextViewAngle, out var hitWall) || nextViewAngle == ViewAngle.None)
            return;
        switch (currentViewAngle)
        {
            case ViewAngle.Right:
                if (XPosition + Width > hitWall.XPosition + hitWall.Height &&
                    GetXPositionAfterMove(viewangle) >= hitWall.XPosition + hitWall.Width)
                    XPosition = hitWall.XPosition + hitWall.Width;
                break;
            case ViewAngle.Left:
                if (XPosition < hitWall.XPosition && GetXPositionAfterMove(viewangle) + Width <= hitWall.XPosition)
                    XPosition = hitWall.XPosition - Width;
                break;
            case ViewAngle.Up:
                if (YPosition < hitWall.YPosition && !(GetYPositionAfterMove(viewangle) + Height >= hitWall.YPosition))
                    YPosition = hitWall.YPosition - Height;
                break;
            case ViewAngle.Down:
                if (YPosition + Height > hitWall.YPosition + hitWall.Width && 
                    GetYPositionAfterMove(viewangle) >= hitWall.YPosition + hitWall.Height)
                    YPosition = hitWall.YPosition + hitWall.Height;
                break;
        }
    }
    
    public bool WouldHitWall(ViewAngle viewAngle) =>
        WouldHitWall(viewAngle, out _, 0, 0);
    
    public bool WouldHitWall(ViewAngle viewAngle, out Wall? hitWall) =>
        WouldHitWall(viewAngle, out hitWall, 0, 0);
    
    public bool WouldHitWall(ViewAngle viewAngle, out Wall? hitWall, int xPlus, int yPlus)
    {
        hitWall = null;
        foreach (var wall in World.Walls)
        {
            if (WouldHitObject(wall, viewAngle, xPlus, yPlus))
            {
                hitWall = wall;
                return true;
            }
        }
        return false;
    }

    public List<ViewAngle> CheckDirection(ViewAngle currentviewAngle)
    {
        var possibleDirections = new List<ViewAngle>();
        if (!WouldHitWall(ViewAngle.Up) && currentviewAngle != ViewAngle.Down)
            possibleDirections.Add(ViewAngle.Up);
        if(!WouldHitWall(ViewAngle.Down) && currentviewAngle != ViewAngle.Up)
            possibleDirections.Add(ViewAngle.Down);
        if (!WouldHitWall(ViewAngle.Right) && currentviewAngle != ViewAngle.Left)
            possibleDirections.Add(ViewAngle.Right);
        if (!WouldHitWall(ViewAngle.Left) && currentviewAngle != ViewAngle.Right)
            possibleDirections.Add(ViewAngle.Left);
          

        return possibleDirections;
    }

    public void SetToNextTurn()
    {
        int yPlus = 0;
        int xPlus = 0;
        switch (viewangle)
        {
            case ViewAngle.Right:
                xPlus = 0;
                while (xPlus < speed)
                {
                    if (!WouldHitWall(ViewAngle.Up, out _, xPlus, yPlus) || !WouldHitWall(ViewAngle.Down, out _, xPlus, yPlus))
                    {
                        XPosition += xPlus;
                        break;
                    }
                    xPlus++;
                }
                break;
            case ViewAngle.Left:
                xPlus = 0;
                while (xPlus > -speed)
                {
                    if (!WouldHitWall(ViewAngle.Up, out _, xPlus, yPlus) || !WouldHitWall(ViewAngle.Down, out _, xPlus, yPlus))
                    {
                        XPosition += xPlus;
                        break;
                    }
                    xPlus--;
                }
                break;
            case ViewAngle.Up:
                yPlus = 0;
                while (yPlus > -speed)
                {
                    if (!WouldHitWall(ViewAngle.Right, out _, xPlus, yPlus) || !WouldHitWall(ViewAngle.Left, out _, xPlus, yPlus))
                    {
                        YPosition += yPlus;
                        break;
                    }
                    yPlus--;
                }
                break;
            case ViewAngle.Down:
                yPlus = 0;
                while (yPlus < speed)
                {
                    if (!WouldHitWall(ViewAngle.Right, out _, xPlus, yPlus) || !WouldHitWall(ViewAngle.Left, out _, xPlus, yPlus))
                    {
                        YPosition += yPlus;
                        break;
                    }
                    yPlus++;
                }
                break;
                
        }
       
    }
    public void Die()
    {
        isDead = true;
    }
}