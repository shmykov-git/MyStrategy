using System.Collections.Generic;

namespace MyStrategy.DataModel
{
    public class Scene
    {
        public int Round { get; set; }

        public SceneNet Net { get; set; }

        public List<Clan> Clans { get; set; }

        public IEnumerable<Unit> Units
        {
            get
            {
                foreach (var clan in Clans)
                foreach (var unit in clan.Units)
                    yield return unit;
            }
        }
    }
}