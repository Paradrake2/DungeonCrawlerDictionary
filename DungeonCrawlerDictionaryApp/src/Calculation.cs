using System.Diagnostics;
using System.Windows;

namespace DCD
{
    public class Calculation
    {
        private List<Stat> playerStats = new List<Stat>();
        private List<Stat> enemyStats = new List<Stat>();
        private Stat weakness = new Stat("", 0f);
        public bool CanDefeatEnemy()
        {
            Console.WriteLine("Starting CanDefeatEnemy calculation...");
            // load player stats
            float playerDamage = GetPlayerStat("Damage");
            Console.WriteLine($"Player Damage: {playerDamage}");
            Console.WriteLine($"Player Stats Count: {playerStats.Count}");
            float playerDefense = GetPlayerStat("Defense");
            float playerHealth = GetPlayerStat("Health");
            float playerAttackSpeed = GetPlayerStat("AttackSpeed");
            float playerWaterDamage = GetPlayerStat("WaterDamage");
            float playerFireDamage = GetPlayerStat("FireDamage");
            float playerEarthDamage = GetPlayerStat("EarthDamage");
            float playerWindDamage = GetPlayerStat("WindDamage");
            float playerLightDamage = GetPlayerStat("LightDamage");
            float playerDarkDamage = GetPlayerStat("DarkDamage");
            float playerWaterDefense = GetPlayerStat("WaterDefense");
            float playerFireDefense = GetPlayerStat("FireDefense");
            float playerEarthDefense = GetPlayerStat("EarthDefense");
            float playerWindDefense = GetPlayerStat("WindDefense");
            float playerLightDefense = GetPlayerStat("LightDefense");
            float playerDarkDefense = GetPlayerStat("DarkDefense");

            // load enemy stats
            float enemyDefense = GetEnemyStat("Defense");
            float enemyHealth = GetEnemyStat("Health");
            float enemyDamage = GetEnemyStat("Damage");
            float enemyAttackSpeed = GetEnemyStat("AttackSpeed");
            float enemyWaterDamage = GetEnemyStat("WaterDamage");
            float enemyFireDamage = GetEnemyStat("FireDamage");
            float enemyEarthDamage = GetEnemyStat("EarthDamage");
            float enemyWindDamage = GetEnemyStat("WindDamage");
            float enemyLightDamage = GetEnemyStat("LightDamage");
            float enemyDarkDamage = GetEnemyStat("DarkDamage");
            float enemyWaterDefense = GetEnemyStat("WaterDefense");
            float enemyFireDefense = GetEnemyStat("FireDefense");
            float enemyEarthDefense = GetEnemyStat("EarthDefense");
            float enemyWindDefense = GetEnemyStat("WindDefense");
            float enemyLightDefense = GetEnemyStat("LightDefense");
            float enemyDarkDefense = GetEnemyStat("DarkDefense");
            // first calculate how many hits it takes for the player to kill the enemy
            // then calculate how many hits it takes for the enemy to kill the player
            // multiply that by attack speed
            // if the value of above to kill player is less than number of hits to kill enemy, return false
            // attributes are gonna be a pain

            // calculate effective damages per attribute
            float totalPlayerDamage = CalculateDamageAfterDefense(playerDamage, enemyDefense)
                + CalculateDamageAfterDefense(playerWaterDamage, enemyWaterDefense)
                + CalculateDamageAfterDefense(playerFireDamage, enemyFireDefense)
                + CalculateDamageAfterDefense(playerEarthDamage, enemyEarthDefense)
                + CalculateDamageAfterDefense(playerWindDamage, enemyWindDefense)
                + CalculateDamageAfterDefense(playerLightDamage, enemyLightDefense)
                + CalculateDamageAfterDefense(playerDarkDamage, enemyDarkDefense);
            if (weakness.name != "")
            {
                totalPlayerDamage = totalPlayerDamage * weakness.value;
            }
            float totalEnemyDamage = CalculateDamageAfterDefense(enemyDamage, playerDefense)
                + CalculateDamageAfterDefense(enemyWaterDamage, playerWaterDefense)
                + CalculateDamageAfterDefense(enemyFireDamage, playerFireDefense)
                + CalculateDamageAfterDefense(enemyEarthDamage, playerEarthDefense)
                + CalculateDamageAfterDefense(enemyWindDamage, playerWindDefense)
                + CalculateDamageAfterDefense(enemyLightDamage, playerLightDefense)
                + CalculateDamageAfterDefense(enemyDarkDamage, playerDarkDefense);
            // calculate hits to defeat
            Console.WriteLine(totalPlayerDamage);
            float hitsToDefeatEnemy = (float)Math.Ceiling(enemyHealth / totalPlayerDamage);
            Console.WriteLine($"Hits to defeat enemy: {hitsToDefeatEnemy}");
            float hitsToDefeatPlayer = (float)Math.Ceiling(playerHealth / totalEnemyDamage);
            Console.WriteLine($"Hits to defeat player: {hitsToDefeatPlayer}");
            // adjust for attack speed
            float timeToDefeatEnemy = hitsToDefeatEnemy / playerAttackSpeed;
            float timeToDefeatPlayer = hitsToDefeatPlayer / enemyAttackSpeed;
            if (timeToDefeatPlayer < timeToDefeatEnemy)
            {
                Console.WriteLine("Player cannot defeat enemy.");
                return false;
            }
            return true;
        }
        public void AddPlayerStat(string name, float value)
        {
            Console.WriteLine($"Adding player stat: {name} with value {value}");
            playerStats.Add(new Stat(name, value));
            Console.WriteLine($"Total player stats count: {playerStats.Count}");
        }
        public void AddEnemyStat(string name, float value)
        {
            enemyStats.Add(new Stat(name, value));
        }
        public void ClearEnemyStats()
        {
            enemyStats.Clear();
        }
        public void ClearPlayerStats()
        {
            playerStats.Clear();
        }
        public void SetEnemyWeakness(string name, float value)
        {
            weakness = new Stat(name, value);
        }
        float GetPlayerStat(string name)
        {
            Console.WriteLine($"Retrieving player stat: {name}");
            foreach (Stat stat in playerStats)
            {
                //Console.WriteLine($"Checking player stat: {stat.name}");
                if (stat.name == name)
                {
                    Console.WriteLine($"Found player stat: {stat.name} with value {stat.value}");
                    return stat.value;
                }
            }
            return 0f;
        }
        float GetEnemyStat(string name)
        {
            foreach (Stat stat in enemyStats)
            {
                if (stat.name == name)
                {
                    return stat.value;
                }
            }
            return 0f;
        }
        float CalculateDamageAfterDefense(float damage, float defense)
        {
            float effectiveDamage = damage - defense;
            return effectiveDamage > 0 ? effectiveDamage : 0;
        }
    }
    
}