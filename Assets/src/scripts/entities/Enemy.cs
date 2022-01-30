using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.src.scripts.entities
{
    public class Enemy : Character
    {
        public Enemy(int minRange, int maxRange, EnemyType type)
        {
            MinRange = minRange;
            MaxRange = maxRange;
            EnemyType = type;
        }

        public Enemy(EnemyType type, int level)
        {
            EnemyType = type;
            BaseStats = new BaseStats();
            BaseStats.Life += (BaseStats.Life / 5) * level;
            BaseStats.Attack += (BaseStats.Attack / 5) * level;
        }

        public AIMode AIMode { get; set; }
        public EnemyType EnemyType { get; set; }
        public int difficulty { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }
    }
}
