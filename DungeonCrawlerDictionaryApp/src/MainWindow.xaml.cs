using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace DCD
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Calculation calc = new Calculation();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var export = LoadEnemyStatsFromJson();

                StatusText.Text = $"ExportedAt: {export.exportedAt} | Enemies: {export.enemyStats.Count}";
                EnemiesList.ItemsSource = export.enemyStats; // will call ToString() per item
                EnemyDetails.Text = "Select an enemy to see details.";
            }
            catch (Exception ex)
            {
                StatusText.Text = "Failed to load EnemyStats.json";
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void EnemiesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedEnemy = EnemiesList.SelectedItem as Enemy;
            if (selectedEnemy != null)
            {
                EnemyDetails.Text = $"Enemy: {selectedEnemy.name}\nStats:\n";
                calc.ClearEnemyStats();
                foreach (var stat in selectedEnemy.stats)
                {
                    EnemyDetails.Text += $"- {stat.statId}: {stat.value}\n";
                    calc.AddEnemyStat(stat.statId, (float)stat.value);
                }
                foreach (var attr in selectedEnemy.enemyAttributesList)
                {
                    EnemyDetails.Text += $"- {attr.statId}: {attr.value}\n";
                    calc.AddEnemyStat(attr.statId, (float)attr.value);
                }
                if (selectedEnemy.weakness != null)
                {
                    EnemyDetails.Text += $"- Weakness {selectedEnemy.weakness.statId}: {selectedEnemy.weakness.value}\n";
                    calc.SetEnemyWeakness(selectedEnemy.weakness.statId, (float)selectedEnemy.weakness.value);
                }
            }
        }
        private static float ReadFloatOrZero(TextBox tb)
        {
            var s = tb?.Text?.Trim();
            if (string.IsNullOrEmpty(s))
                return 0f;

            // Prefer invariant (so "1.5" works regardless of Windows locale), but fall back to current culture.
            if (float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var v))
                return v;

            if (float.TryParse(s, NumberStyles.Float, CultureInfo.CurrentCulture, out v))
                return v;

            return 0f; // invalid input => treat as 0 (no crash)
        }
        // at one point this was a button, has been repurposed to be called before calculation
        private void PlayerStatsSubmit_Click(object sender, RoutedEventArgs e)
        {
            calc.ClearPlayerStats();
            // MessageBox.Show("Player stats submission is not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            float playerHealth = ReadFloatOrZero(PlayerHealth);
            float playerDamage = ReadFloatOrZero(PlayerDamage);
            float playerDefense = ReadFloatOrZero(PlayerDefense);
            float playerAttackSpeed = ReadFloatOrZero(PlayerAttackSpeed);
            // Attribute damages
            float playerWaterDamage = ReadFloatOrZero(WaterDamage);
            float playerFireDamage = ReadFloatOrZero(FireDamage);
            float playerEarthDamage = ReadFloatOrZero(EarthDamage);
            float playerWindDamage = ReadFloatOrZero(WindDamage);
            float playerLightDamage = ReadFloatOrZero(LightDamage);
            float playerDarkDamage = ReadFloatOrZero(DarkDamage);
            // Attribute defenses
            float playerWaterDefense = ReadFloatOrZero(WaterDefense);
            float playerFireDefense = ReadFloatOrZero(FireDefense);
            float playerEarthDefense = ReadFloatOrZero(EarthDefense);
            float playerWindDefense = ReadFloatOrZero(WindDefense);
            float playerLightDefense = ReadFloatOrZero(LightDefense);
            float playerDarkDefense = ReadFloatOrZero(DarkDefense);
            // add player stats to calculation
            calc.AddPlayerStat("Health", playerHealth);
            calc.AddPlayerStat("Damage", playerDamage);
            calc.AddPlayerStat("Defense", playerDefense);
            calc.AddPlayerStat("AttackSpeed", playerAttackSpeed);
            calc.AddPlayerStat("WaterDamage", playerWaterDamage);
            calc.AddPlayerStat("FireDamage", playerFireDamage);
            calc.AddPlayerStat("EarthDamage", playerEarthDamage);
            calc.AddPlayerStat("WindDamage", playerWindDamage);
            calc.AddPlayerStat("LightDamage", playerLightDamage);
            calc.AddPlayerStat("DarkDamage", playerDarkDamage);
            calc.AddPlayerStat("WaterDefense", playerWaterDefense);
            calc.AddPlayerStat("FireDefense", playerFireDefense);
            calc.AddPlayerStat("EarthDefense", playerEarthDefense);
            calc.AddPlayerStat("WindDefense", playerWindDefense);
            calc.AddPlayerStat("LightDefense", playerLightDefense);
            calc.AddPlayerStat("DarkDefense", playerDarkDefense);
            // Notify user
        }
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerStatsSubmit_Click(sender, e); // Ensure player stats are up to date
            // Assuming player stats have already been added via PlayerStatsSubmit_Click
            // and enemy stats have been added via EnemiesList_SelectionChanged
            Debug.WriteLine("Starting calculation to determine if player can defeat enemy...");
            bool canDefeat = calc.CanDefeatEnemy();
            string message = canDefeat ? "You can defeat the enemy! \n Remaining health: " + calc.GetRemainingHealth() : "You cannot defeat the enemy.";
            MessageBox.Show(message, "Calculation Result", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private static EnemyStatsExport LoadEnemyStatsFromJson()
        {
            var jsonPath = Path.Combine(AppContext.BaseDirectory, "Imports", "EnemyStats.json");
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"Could not find EnemyStats.json at '{jsonPath}'.", jsonPath);

            var json = File.ReadAllText(jsonPath);
            var export = JsonSerializer.Deserialize<EnemyStatsExport>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return export ?? throw new InvalidDataException("EnemyStats.json was empty or invalid JSON.");
        }
        private void LoadPlayerStatsButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            try
            {
                var jsonPath = Path.Combine(AppContext.BaseDirectory, "Imports", "PlayerStatsExport.json");
                if (!File.Exists(jsonPath))
                    throw new FileNotFoundException($"Could not find PlayerStatsExport.json at '{jsonPath}'.", jsonPath);

                var json = File.ReadAllText(jsonPath);

                using var doc = JsonDocument.Parse(json);

                if (!doc.RootElement.TryGetProperty("playerStats", out var playerStatsElement) ||
                    playerStatsElement.ValueKind != JsonValueKind.Array)
                {
                    throw new InvalidDataException("PlayerStatsExport.json does not contain a 'playerStats' array.");
                }

                // Map statId -> TextBox (unknown statIds will be ignored)
                var map = new Dictionary<string, TextBox>(StringComparer.OrdinalIgnoreCase)
                {
                    ["Health"] = PlayerHealth,
                    ["Damage"] = PlayerDamage,
                    ["Defense"] = PlayerDefense,
                    ["AttackSpeed"] = PlayerAttackSpeed,

                    ["WaterDamage"] = WaterDamage,
                    ["FireDamage"] = FireDamage,
                    ["EarthDamage"] = EarthDamage,
                    ["WindDamage"] = WindDamage,
                    ["LightDamage"] = LightDamage,
                    ["DarkDamage"] = DarkDamage,

                    ["WaterDefense"] = WaterDefense,
                    ["FireDefense"] = FireDefense,
                    ["EarthDefense"] = EarthDefense,
                    ["WindDefense"] = WindDefense,
                    ["LightDefense"] = LightDefense,
                    ["DarkDefense"] = DarkDefense,
                };

                int applied = 0;

                foreach (var stat in playerStatsElement.EnumerateArray())
                {
                    if (!stat.TryGetProperty("statId", out var statIdEl) || statIdEl.ValueKind != JsonValueKind.String)
                        continue;

                    var statId = statIdEl.GetString();
                    if (string.IsNullOrWhiteSpace(statId))
                        continue;

                    if (!map.TryGetValue(statId, out var targetTextBox))
                    {
                        // Not on the UI (e.g., Poison) => ignore
                        continue;
                    }

                    if (!stat.TryGetProperty("value", out var valueEl))
                        continue;

                    // value might be number or string; handle both
                    if (valueEl.ValueKind == JsonValueKind.Number && valueEl.TryGetDouble(out var num))
                    {
                        targetTextBox.Text = num.ToString(CultureInfo.InvariantCulture);
                        applied++;
                    }
                    else if (valueEl.ValueKind == JsonValueKind.String)
                    {
                        var s = valueEl.GetString();
                        if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
                        {
                            targetTextBox.Text = parsed.ToString(CultureInfo.InvariantCulture);
                            applied++;
                        }
                    }
                }

                StatusText.Text = $"Loaded player stats. Applied: {applied}";
            }
            catch (Exception ex)
            {
                StatusText.Text = "Failed to load player stats.";
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}