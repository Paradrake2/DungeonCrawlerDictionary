using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;

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
            }
        }
        private void PlayerStatsSubmit_Click(object sender, RoutedEventArgs e)
        {
            calc.ClearPlayerStats();
            // MessageBox.Show("Player stats submission is not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            float playerHealth = PlayerHealth.Text != "" ? float.Parse(PlayerHealth.Text) : 0;
            float playerDamage = PlayerDamage.Text != "" ? float.Parse(PlayerDamage.Text) : 0;
            float playerDefense = PlayerDefense.Text != "" ? float.Parse(PlayerDefense.Text) : 0;
            float playerAttackSpeed = PlayerAttackSpeed.Text != "" ? float.Parse(PlayerAttackSpeed.Text) : 0;
            // Attribute damages
            float playerWaterDamage = WaterDamage.Text != "" ? float.Parse(WaterDamage.Text) : 0;
            float playerFireDamage = FireDamage.Text != "" ? float.Parse(FireDamage.Text) : 0;
            float playerEarthDamage = EarthDamage.Text != "" ? float.Parse(EarthDamage.Text) : 0;
            float playerWindDamage = WindDamage.Text != "" ? float.Parse(WindDamage.Text) : 0;
            float playerLightDamage = LightDamage.Text != "" ? float.Parse(LightDamage.Text) : 0;
            float playerDarkDamage = DarkDamage.Text != "" ? float.Parse(DarkDamage.Text) : 0;
            // Attribute defenses
            float playerWaterDefense = WaterDefense.Text != "" ? float.Parse(WaterDefense.Text) : 0;
            float playerFireDefense = FireDefense.Text != "" ? float.Parse(FireDefense.Text) : 0;
            float playerEarthDefense = EarthDefense.Text != "" ? float.Parse(EarthDamage.Text) : 0;
            float playerWindDefense = WindDefense.Text != "" ? float.Parse(WindDefense.Text) : 0;
            float playerLightDefense = LightDefense.Text != "" ? float.Parse(LightDamage.Text) : 0;
            float playerDarkDefense = DarkDefense.Text != "" ? float.Parse(DarkDamage.Text) : 0;
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
            
            // Assuming player stats have already been added via PlayerStatsSubmit_Click
            // and enemy stats have been added via EnemiesList_SelectionChanged
            Debug.WriteLine("Starting calculation to determine if player can defeat enemy...");
            bool canDefeat = calc.CanDefeatEnemy();
            string message = canDefeat ? "You can defeat the enemy!" : "You cannot defeat the enemy.";
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
    }
}