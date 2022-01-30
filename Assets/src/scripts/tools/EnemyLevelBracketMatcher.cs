using Assets.src.scripts.entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.src.scripts.tools
{
    public class EnemyLevelBracketMatcher
    {
        public SQLiteConnector connector;
        public List<Enemy> enemyList;
        public EnemyLevelBracketMatcher()
        {
            connector = new SQLiteConnector();
            CreateMonsterTable();
            //InsertMonster(0, 0, EnemyType.BASIC_BROKEN, true);
            //InsertMonster(1, 3, EnemyType.BASIC, true);
            //InsertMonster(2, 5, EnemyType.FLYING, true);
        }

        public List<EnemyType> GetEnemyTypesFromLevelBracket(int level)
        {
            //IDbCommand cmd = connector.dbcon.CreateCommand();
            ////cmd.CommandText = "SELECT * FROM " + connector.monsterTable + " WHERE startinglevel <= " + level + " AND endinglevel >= " + level+";";
            //cmd.CommandText = "SELECT * FROM " + connector.monsterTable + " ;";

            //var result = connector.ExecuteQuery(cmd);
            //return null;

            return enemyList.Where(e => e.MinRange <= level && e.MaxRange >= level).Select(e => e.EnemyType).ToList();
        }

        private void InsertMonster(int startinglevel, int endinglevel, EnemyType type, bool isactive)
        {
            IDbCommand cmd = connector.dbcon.CreateCommand();
            var id = Guid.NewGuid().ToString();
            cmd.CommandText = "INSERT INTO " + connector.monsterTable + " VALUES ('" + startinglevel + "','" +endinglevel+ "','"+id+ "','"+type+ "','"+isactive+ "')";
            connector.ExecuteQuery(cmd);
        }

        private void CreateMonsterTable()
        {
            enemyList = new List<Enemy>()
             {
                 new Enemy(0,0,EnemyType.BASIC_BROKEN),
                 new Enemy(1,3,EnemyType.BASIC),
                 new Enemy(3,5,EnemyType.FLYING)
             };
        }
    }
}
