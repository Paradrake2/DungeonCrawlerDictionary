using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace MyWpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

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
                foreach (var stat in selectedEnemy.stats)
                {
                    EnemyDetails.Text += $"- {stat.statId}: {stat.value}\n";
                }
            }
            
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