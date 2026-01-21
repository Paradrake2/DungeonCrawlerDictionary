namespace my_wpf_app
{
    public class Calculation
    {
        private List<Stat> playerStats = new List<Stat>();
        private List<Stat> enemyStats = new List<Stat>();
        public bool CanDefeatEnemy()
        {
            float playerDamage = GetPlayerStat("Damage");
            float playerDefense = GetPlayerStat("Defense");
            float playerHealth = GetPlayerStat("Health");
            float playerAttackSpeed = GetPlayerStat("AttackSpeed");
            
            float enemyDefense = GetEnemyStat("Defense");
            float enemyHealth = GetEnemyStat("Health");
            float enemyDamage = GetEnemyStat("Damage");
            float enemyAttackSpeed = GetEnemyStat("AttackSpeed");
            // first calculate how many hits it takes for the player to kill the enemy
            // then calculate how many hits it takes for the enemy to kill the player
            // multiply that by attack speed
            // if the value of above to kill player is less than number of hits to kill enemy, return false
            return true;
        }
        public void AddPlayerStat(string name, float value)
        {
            playerStats.Add(new Stat(name, value));
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
        float GetPlayerStat(string name)
        {
            foreach (Stat stat in playerStats)
            {
                if (stat.name == name)
                {
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
    }
    
}