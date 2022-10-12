namespace PacManGame;

public static class LevelFactory
{
    public static List<Wall> Walls { get; } =
        LoadFile("Walls.csv", split => new Wall(split[0], split[1], split[2], split[3]));

    public static List<PacDot> PacDots { get; } =
        LoadFile("PacDots.csv", split => new PacDot(split[0], split[1], split[2], split[3]));

    public static List<PowerPallets> PowerPallets { get; } =
        LoadFile("PowerPallets.csv", split => new PowerPallets(split[0], split[1], split[2], split[3]));

    private static List<TGameObject> LoadFile<TGameObject>(string fileName, Func<int[], TGameObject> mapperFunc)
        where TGameObject : GameObject =>
        File.ReadAllLines($"Levels\\{fileName}")
            .Skip(1)
            .Select(line => line.Split(';'))
            .Select(split => split.Select(int.Parse).ToArray())
            .Select(mapperFunc)
            .ToList();
}