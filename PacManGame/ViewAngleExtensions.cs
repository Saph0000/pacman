namespace PacManGame;

public static class ViewAngleExtensions
{
    public static ViewAngle GetOppositeDirection(this ViewAngle viewAngle) =>
        viewAngle switch
        {
            ViewAngle.Right => ViewAngle.Left,
            ViewAngle.Left => ViewAngle.Right,
            ViewAngle.Up => ViewAngle.Down,
            ViewAngle.Down => ViewAngle.Up,
            ViewAngle.None or _ => ViewAngle.None
        };
}