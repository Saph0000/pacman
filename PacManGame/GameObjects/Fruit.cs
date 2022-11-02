using PacManGame.GameObjects;

public class Fruit : GameObject
{
    
    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.DrawImage(Image.FromFile(FruitImage), XPosition, YPosition, Height, Width);
    }



    //public readonly Image cherry = Image.FromFile(@"Pictures\cherry.png");
}