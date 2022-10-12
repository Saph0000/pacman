namespace PacManGame;

public static class LevelFactory
{
    public static List<Wall> Walls =>
        File.ReadAllLines("Levels\\Walls.csv")
            .Skip(1)
            .Select(line => line.Split(';'))
            .Select(split => split.Select(int.Parse).ToArray())
            .Select(split => new Wall(split[0], split[1], split[2], split[3]))
            .ToList();
    public static List<PacDot> PacDots =>
        File.ReadAllLines("Levels\\pacDots.csv")
            .Skip(1)
            .Select(line => line.Split(';'))
            .Select(split => split.Select(int.Parse).ToArray())
            .Select(split => new PacDot(split[0], split[1], split[2], split[3]))
            .ToList();
    public static List<PowerPallets> PowerPallets =>
        File.ReadAllLines("Levels\\powerPallets.csv")
            .Skip(1)
            .Select(line => line.Split(';'))
            .Select(split => split.Select(int.Parse).ToArray())
            .Select(split => new PowerPallets(split[0], split[1], split[2], split[3]))
            .ToList();
   
}