using System.IO;
using MyStrategy.DataModel;
using MyStrategy.DataModel.Acts;
using Suit.Aspects;
using Suit.Extensions;
using Suit.Logs;

namespace MyStrategy.Tools
{
    public class SceneManager
    {
        public Scene Scene { get; private set; }

        public void Start()
        {
            Scene = File.ReadAllText("scene.json").FromJson<Scene>();
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
