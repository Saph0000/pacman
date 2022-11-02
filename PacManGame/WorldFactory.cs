using System.Text.Json;
using PacManGame.GameObjects;

namespace PacManGame;

public static class WorldFactory
{
    public static List<Wall> CreateWalls(IWorld world) =>
        CreateObjects<Wall>(world, "Walls");
    
    public static List<PacDot> CreatePacDots(IWorld world) =>
        CreateObjects<PacDot>(world, "PacDots");

    public static List<PowerPallet> CreatePowerPallets(IWorld world) =>
        CreateObjects<PowerPallet>(world, "PowerPallets");
    
    public static List<Fruit> CreateFruits(IWorld world) =>
        CreateObjects<Fruit>(world, "Fruits");
    
    private static List<TGameObject> CreateObjects<TGameObject>(IWorld world, string fileName)
        where TGameObject : GameObject
    {
        var content = File.ReadAllText($"Levels\\{fileName}.json");
        var result = JsonSerializer.Deserialize<List<TGameObject>>(content);
        result!.ForEach(gameObject => gameObject.World = world);
        return result;
    }
}