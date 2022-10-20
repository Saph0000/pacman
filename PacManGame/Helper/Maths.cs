namespace PacManGame.Helper;

public static class Maths
{
    public static double Min(params double[] values) => 
        values.Min();
    
    public static double CalculateDistance(double xPosition1, double yPosition1, double xPosition2, double yPosition2)
    {
        var distanceX = xPosition1 - xPosition2;
        var distanceY = yPosition1 - yPosition2;
        return Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
    }
}