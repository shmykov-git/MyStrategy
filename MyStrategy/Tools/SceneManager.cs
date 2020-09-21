﻿using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MyStrategy.Commands;
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
        private string ClanName { get; set; } = "1";
        private Unit defaultUnit;

        private void Init()
        {
            viewer.ClanNameChangedCommand = new Command<string>(name => ClanName = name);

            viewer.ClickCommand = new Command<Vector>(position =>
            {
                var clan = Scene.Clans.Single(c => c.Name == ClanName);
                var unit = defaultUnit.Clone();
                unit.Position = position;
                unit.Clan = clan;
                unit.Scene = Scene;

                clan.Units.Add(unit);
                unit.IsActive = true;
                viewer.OnCreate(unit);
            });
        }

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
                unit.PairActs.Add(new FindAndMoveToFight() {Key = unit.Id});
                unit.PairActs.Add(new Attack() {Key = unit.Id});
            });

            defaultUnit = Scene.Units.First().Clone();

            Scene.Units.ForEach(unit => unit.IsActive = true);
        }

        public async Task PlayScene()
        {
            var round = 0;
            while (++round <= settings.RoundCount || settings.RoundCount < 0)
            {
                if (settings.FpsInterval > 0)
                    await Task.Delay(settings.FpsInterval);

                Scene.Round = round;

                log.Debug($"===== round {round} =====");
                foreach (var unit in Scene.Units.ToArray())
                {
                    foreach (var opponent in unit.GetUnderSightOpponents().ToArray())
                    foreach (var act in unit.PairActs.ToArray())
                    {
                        act.Do(unit, opponent);
                    }

                    foreach (var act in unit.SelfActs.ToArray())
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
        private readonly IViewer viewer;

        public SceneManager(ILog log, ISceneManagerSettings settings, IViewer viewer)
        {
            this.log = log;
            this.settings = settings;
            this.viewer = viewer;

            Init();
        }

        #endregion
    }
}
