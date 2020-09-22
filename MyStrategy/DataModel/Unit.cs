using System.Collections.Generic;
using System.Threading;
using MyStrategy.Aspects;
using MyStrategy.DataModel.Acts;
using Newtonsoft.Json;
using Suit.Extensions;

namespace MyStrategy.DataModel
{
    public class Unit
    {
        public UnitType Type { get; set; }

        [NotifyViewerAspect]
        public int Hp { get; set; }

        [NotifyViewerAspect]
        public Vector Position { get; set; }

        [NotifyViewerAspect]
        public float Speed { get; set; }

        [NotifyViewerAspect]
        public float Damage { get; set; }

        [NotifyViewerAspect]
        public int AttackCount { get; set; }

        [NotifyViewerAspect]
        public float DamageDistance { get; set; }

        [NotifyViewerAspect]
        public float SightDistance { get; set; }



        private static int _id = 0;
        public int Id { get; }

        [JsonIgnore]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public Scene Scene { get; set; }

        [JsonIgnore]
        public Clan Clan { get; set; }

        public Unit()
        {
            Id = Interlocked.Increment(ref _id);
        }

        [JsonIgnore]
        public List<IAct> Acts { get; set; } = new List<IAct>();

        [JsonIgnore]
        public VectorFn PositionFn => () => Position;

        private int round;
        private UnitRound unitRound;

        [JsonIgnore]
        public UnitRound Round
        {
            get
            {
                if (round != Scene.Round)
                {
                    unitRound = new UnitRound();
                    round = Scene.Round;
                }

                return unitRound;
            }
        }

        public override string ToString()
        {
            return $"{Id}";
        }

        public Unit Clone()
        {
            return this.ToJsonNamedStr().FromNamedJson<Unit>();
        }
    }
}
