using System.Collections.Generic;

namespace MyStrategy.DataModel
{
    public class Clan
    {
        public string Name { get; set; }
        public List<Unit> Units { get; set; }

        public override string ToString()
        {
            return $"Clan {Name}: {Units.Count}";
        }
    }
}