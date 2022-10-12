namespace PacManGame;

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
    protected GameActor(int xPosition, int yPosition, int width, int height) : base(xPosition, yPosition, width, height)
    {
    }

    /*
    public ViewAngle viewangle
    {
        get { return Viewangle; }
        set
        {
            Viewangle = value;
            this.DrawActor(currFrame);
        }
    }
    */
    
    public void DrawActor(int maxFrames)
    {
        switch (viewangle)
        {
            case ViewAngle.None:
                image = baseImage;
                break;
            case ViewAngle.Right:
                image = Image.FromFile(@"C:\Users\reutimann\RiderProjects\pacman\PacManGame\pictures\" + right[currFrame] +".png");
                break;
            case ViewAngle.Left:
                image = Image.FromFile(@"C:\Users\reutimann\RiderProjects\pacman\PacManGame\pictures\" + left[currFrame] +".png");
                break;
            case ViewAngle.Up:
                image = Image.FromFile(@"C:\Users\reutimann\RiderProjects\pacman\PacManGame\pictures\" + up[currFrame] +".png");
                break;
            case ViewAngle.Down:
                image = Image.FromFile(@"C:\Users\reutimann\RiderProjects\pacman\PacManGame\pictures\" + down[currFrame] +".png");
                break;
        }
        
        currFrame++;
        if (currFrame == maxFrames)
            currFrame = 0;
    }
    public void Move()
    {
        xPosition = GetXPositionAfterMove(viewangle);
        yPosition = GetYPositionAfterMove(viewangle);
        var x = WouldHitWall(viewangle, out var hitWall);
        if (x)
        {
            switch (viewangle)
            {
                case ViewAngle.Right:
                    xPosition = hitWall.xPosition - width;
                    break;
                case ViewAngle.Left:
                    xPosition = hitWall.xPosition + hitWall.width;
                    break;
                case ViewAngle.Up:
                    yPosition = hitWall.yPosition + hitWall.height;
                    break;
                case ViewAngle.Down:
                    yPosition = hitWall.yPosition - height;
                    break;
            }
        }
        if (xPosition > Settings.ScreenWidth)
            xPosition = -width;
        else if (xPosition < -width)
            xPosition = Settings.ScreenWidth;
        if (yPosition > Settings.ScreenHeight)
            yPosition = height;
        else if (yPosition < 0)
            yPosition = Settings.ScreenHeight;
    }

    private int GetYPositionAfterMove(ViewAngle viewangle) =>
        viewangle switch
        {
            ViewAngle.Up => yPosition - speed,
            ViewAngle.Down => yPosition + speed,
            _ => yPosition
        };

    private int GetXPositionAfterMove(ViewAngle viewangle) =>
        viewangle switch
        {
            ViewAngle.Right => xPosition + speed,
            ViewAngle.Left => xPosition - speed,
            _ => xPosition
        };

    public void CouldTurn(ViewAngle currentViewAngle, ViewAngle nextViewAngle)
    {
        if (!WouldHitWall(nextViewAngle, out var hitWall) || nextViewAngle == ViewAngle.None)
            return;
        switch (currentViewAngle)
        {
            case ViewAngle.Right:
                if (xPosition + width > hitWall.xPosition + hitWall.height &&
                    GetXPositionAfterMove(viewangle) >= hitWall.xPosition + hitWall.width)
                    xPosition = hitWall.xPosition + hitWall.width;
                break;
            case ViewAngle.Left:
                if (xPosition < hitWall.xPosition && GetXPositionAfterMove(viewangle) + width <= hitWall.xPosition)
                    xPosition = hitWall.xPosition - width;
                break;
            case ViewAngle.Up:
                if (yPosition < hitWall.yPosition && !(GetYPositionAfterMove(viewangle) + height >= hitWall.yPosition))
                    yPosition = hitWall.yPosition - height;
                break;
            case ViewAngle.Down:
                if (yPosition + height > hitWall.yPosition + hitWall.width && 
                    GetYPositionAfterMove(viewangle) >= hitWall.yPosition + hitWall.height)
                    yPosition = hitWall.yPosition + hitWall.height;
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
        foreach (var wall in LevelFactory.Walls)
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
                        xPosition += xPlus;
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
                        xPosition += xPlus;
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
                        yPosition += yPlus;
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
                        yPosition += yPlus;
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