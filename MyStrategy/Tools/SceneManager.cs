using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MyStrategy.DataModel;
using MyStrategy.DataModel.Acts;
using MyStrategy.Extensions;
using Suit.Aspects;
using Suit.Extensions;
using Suit.Logs;

namespace MyStrategy.Tools
{
    public class SceneManager
    {
        public Scene Scene { get; private set; }

        public void InitScene()
        {
            Scene = File.ReadAllText(settings.SceneFileName).FromJson<Scene>();
            Scene.Clans.ForEach(clan => clan.Units.ForEach(unit =>
            {
                unit.Scene = Scene;
                unit.Clan = clan;
            }));

            Scene.Units.ForEach(unit=>
            {
                unit.PairActs.Add(new FindAndMoveToFight());
                unit.PairActs.Add(new Attack());
            });
        }

        public async Task PlayScene(int roundCount = 20)
        {
            var round = 0;
            while (++round < roundCount || roundCount < 0)
            {
                if (settings.FpsInterval > 0)
                    await Task.Delay(settings.FpsInterval);

                Scene.Round = round;

                log.Debug($"===== round {round} =====");
                foreach (var unit in Scene.Units.ToArray())
                {
                    foreach (var opponent in unit.GetUnderSightOpponents())
                    foreach (var act in unit.PairActs)
                    {
                        act.Do(unit, opponent);
                    }

                    foreach (var act in unit.SelfActs)
                    {
                        act.Do(unit);
                    }
                }
            }

            foreach (var clan in Scene.Clans.Where(c => c.Units.Count > 0))
            {
                log.Debug($"Win {clan}");

                foreach (var unit in clan.Units)
                {
                    log.Debug($"unit {unit.Id} hp:{unit.Hp}");
                }
            }
        }

        #region IoC

        private readonly ILog log;
        private readonly ISceneManagerSettings settings;

        public SceneManager(ILog log, ISceneManagerSettings settings)
        {
            this.log = log;
            this.settings = settings;
        }

        #endregion
    }
}
