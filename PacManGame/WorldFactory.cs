using PacManGame.GameObjects;

namespace PacManGame;

public static class WorldFactory
{
    public static List<Wall> CreateWalls(IWorld world) =>
        LoadFile("Walls.csv", split => new Wall(world, split[0], split[1], split[2], split[3]));

    public static List<PacDot> CreatePacDots(IWorld world) =>
        LoadFile("PacDots.csv", split => new PacDot(world, split[0], split[1], split[2], split[3]));

    public static List<PowerPallets> CreatePowerPallets(IWorld world) =>
        LoadFile("PowerPallets.csv", split => new PowerPallets(world, split[0], split[1], split[2], split[3]));

    private static List<TGameObject> LoadFile<TGameObject>(string fileName, Func<int[], TGameObject> mapperFunc)
        where TGameObject : GameObject =>
        File.ReadAllLines($"Levels\\{fileName}")
            .Skip(1)
            .Select(line => line.Split(';'))
            .Select(split => split.Select(int.Parse).ToArray())
            .Select(mapperFunc)
            .ToList();
}