using System.Collections.Generic;
using System.Threading;
using MyStrategy.Acts;
using MyStrategy.Aspects;
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
        public Vector Position
        {
            get => position;
            set
            {
                position = value;

                if (IsActive)
                    Scene.Net.NotifyPosition(this);
            }
        }

        public Index PositionIndex { get; set; }

        public float Radius { get; set; }

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

        public float ActiveDistance { get; set; }


        private static int _id = 0;
        public int Id { get; }

        public int BaseHp { get; set; }

        [JsonIgnore]
        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                if (isActive)
                    Scene.Net.AddPosition(this);
                else
                    Scene.Net.RemovePosition(this);
            }
        }

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
        private Vector position;
        private bool isActive;

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
