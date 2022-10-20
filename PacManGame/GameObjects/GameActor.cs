namespace PacManGame.GameObjects;

public abstract class GameActor : GameObject
{
    public ViewAngle viewangle;
    public ViewAngle nextViewangle;
    protected int speed;
    private bool isDead;
    private int currFrame;
    protected readonly int xStartPosition;
    protected readonly int yStartPosition;
    protected int maxFrame = 0;
    
    protected GameActor(IWorld world, int xStartPosition, int yStartPosition, int width, int height)
    {
        this.xStartPosition = xStartPosition;
        this.yStartPosition = yStartPosition;
        Width = width;
        Height = height;
        World = world;
        XPosition = xStartPosition;
        YPosition = yStartPosition;
        
    }

    public void AnimateActor()
    {
        var images = GetImageNames();
        currFrame++;
        if (maxFrame == 0)
            maxFrame = images.Length;
        if (currFrame >= maxFrame)
            currFrame = 0;
    }
    
    public void Move()
    {
        XPosition = GetXPositionAfterMove(viewangle);
        YPosition = GetYPositionAfterMove(viewangle);
        CorrectPositionAfterWouldHitWall();
        CorrectPositionAfterLeftBorder();
    }

    private void CorrectPositionAfterWouldHitWall()
    {
        var wouldHitWall = WouldHitWall(viewangle, out var hitWall);
        if (wouldHitWall)
        {
            (XPosition, YPosition) = viewangle switch
            {
                ViewAngle.Right => (hitWall!.XPosition - Width, YPosition),
                ViewAngle.Left => (hitWall!.XPosition + hitWall.Width, YPosition),
                ViewAngle.Up => (XPosition, hitWall!.YPosition + hitWall.Height),
                ViewAngle.Down => (XPosition, hitWall!.YPosition - Height),
                _ => (XPosition, YPosition)
            };
        }
    }

    private void CorrectPositionAfterLeftBorder()
    {
        int CorrectPosition(int currentPosition, int lowerBound, int upperBound)
        {
            if (currentPosition > upperBound)
                return lowerBound;
            return currentPosition < lowerBound ? upperBound : currentPosition;
        }
        XPosition = CorrectPosition(XPosition, -Width, Settings.ScreenWidth);
        YPosition = CorrectPosition(YPosition, -Height, Settings.ScreenHeight);
    }


    private int GetYPositionAfterMove(ViewAngle viewAngle) =>
        viewAngle switch
        {
            ViewAngle.Up => YPosition - speed,
            ViewAngle.Down => YPosition + speed,
            _ => YPosition
        };

    private int GetXPositionAfterMove(ViewAngle viewAngle) =>
        viewAngle switch
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
                if (XPosition + Width > hitWall!.XPosition + hitWall.Height &&
                    GetXPositionAfterMove(viewangle) >= hitWall.XPosition + hitWall.Width)
                    XPosition = hitWall.XPosition + hitWall.Width;
                break;
            case ViewAngle.Left:
                if (XPosition < hitWall!.XPosition && GetXPositionAfterMove(viewangle) + Width <= hitWall.XPosition)
                    XPosition = hitWall.XPosition - Width;
                break;
            case ViewAngle.Up:
                if (YPosition < hitWall!.YPosition && !(GetYPositionAfterMove(viewangle) + Height >= hitWall.YPosition))
                    YPosition = hitWall.YPosition - Height;
                break;
            case ViewAngle.Down:
                if (YPosition + Height > hitWall!.YPosition + hitWall.Width && 
                    GetYPositionAfterMove(viewangle) >= hitWall.YPosition + hitWall.Height)
                    YPosition = hitWall.YPosition + hitWall.Height;
                break;
        }
    }
    
    public bool WouldHitWall(ViewAngle viewAngle) =>
        WouldHitWall(viewAngle, out _, 0, 0);

    private bool WouldHitWall(ViewAngle viewAngle, out Wall? hitWall) =>
        WouldHitWall(viewAngle, out hitWall, 0, 0);

    private bool WouldHitWall(ViewAngle viewAngle, out Wall? hitWall, int xPlus, int yPlus)
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

    protected List<ViewAngle> CheckDirection(ViewAngle currentViewAngle)
    {
        var possibleDirections = new List<ViewAngle>();
        if (!WouldHitWall(ViewAngle.Up) && currentViewAngle != ViewAngle.Down)
            possibleDirections.Add(ViewAngle.Up);
        if(!WouldHitWall(ViewAngle.Down) && currentViewAngle != ViewAngle.Up)
            possibleDirections.Add(ViewAngle.Down);
        if (!WouldHitWall(ViewAngle.Right) && currentViewAngle != ViewAngle.Left)
            possibleDirections.Add(ViewAngle.Right);
        if (!WouldHitWall(ViewAngle.Left) && currentViewAngle != ViewAngle.Right)
            possibleDirections.Add(ViewAngle.Left);
        return possibleDirections;
    }

    public void SetToNextTurn()
    {
        var yPlus = 0;
        var xPlus = 0;
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

            case ViewAngle.None:
                break;
        }
       
    }
    
    public override void Draw(PaintEventArgs e)
    {
        var imageNames = GetImageNames();
        var imageName = currFrame >= imageNames.Length ? imageNames[0] : imageNames[currFrame];
        if (!World.ImageMap.ContainsKey(imageName)) 
            return;
        var image = World.ImageMap[imageName];
        e.Graphics.DrawImage(image, XPosition, YPosition, Width, Height);
    }

    protected abstract string[] GetImageNames();
    
    public virtual void Die()
    {
        isDead = true;
    }
}