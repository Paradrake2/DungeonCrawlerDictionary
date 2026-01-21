using System.IO;
using System.Text.Json;

public class Program
{
    public static void Main(string[] args)
    {
        var export = LoadEnemyStatsFromJson();

        Console.WriteLine($"ExportedAt: {export.exportedAt}");
        Console.WriteLine($"Enemies: {export.enemyStats.Count}");
        
        /*
        // Just to verify things are actually being read
        var firstEnemy = export.enemyStats[1];
        if (firstEnemy is not null)
        {
            Console.WriteLine($"First enemy: {firstEnemy.name} ({firstEnemy.stats.Count} stats)");
        }
        */


    }

    private static EnemyStatsExport LoadEnemyStatsFromJson()
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "Imports", "EnemyStats.json");
        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException(
                $"Could not find EnemyStats.json at '{jsonPath}'. Ensure it is copied to the output folder.",
                jsonPath);
        }

        var json = File.ReadAllText(jsonPath);
        var export = JsonSerializer.Deserialize<EnemyStatsExport>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return export ?? throw new InvalidDataException("EnemyStats.json was empty or invalid JSON.");
    }
}