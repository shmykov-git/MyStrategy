using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using MyStrategy.DataModel;
using MyStrategy.Extensions;
using MyStrategy.Tools;
using Suit.Logs;

namespace Viewer.Uwp.Viewer
{
    class Controller
    {
        private readonly Dictionary<int, Ellipse> unitControls = new Dictionary<int, Ellipse>();
        private double sceneHeight;

        public void MoveUnit(Unit unit, Vector position)
        {
            var control = unitControls[unit.Id];

            Canvas.SetTop(control, sceneHeight - unit.Position.Y);
            Canvas.SetLeft(control, unit.Position.X);
        }

        public void SetUnitHp(Unit unit, float hp)
        {

        }

        public void Kill(Unit unit)
        {
            Page.Canvas.Children.Remove(unitControls[unit.Id]);
            unitControls.Remove(unit.Id);
        }

        public void Start()
        {
            this.sceneHeight = Page.Height;

            sceneManager.Start();
            viewer.Controller = this;
            viewer.IsActive = true;

            var scene = sceneManager.Scene;

            foreach (var unit in scene.Units)
            {
                var control = new Ellipse()
                {
                    Fill = new SolidColorBrush() { Color = Colors.Red },
                    Height = 15,
                    Width = 15,
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush() { Color = Colors.Black }
                };

                Canvas.SetTop(control, sceneHeight - unit.Position.Y);
                Canvas.SetLeft(control, unit.Position.X);

                Page.Canvas.Children.Add(control);
                unitControls.Add(unit.Id, control);
            }

            Play(scene);
        }


        private async void Play(Scene scene)
        {

            for (var round = 1; round < 20; round++)
            {
                await Task.Delay(1000);

                scene.Round = round;

                log.Debug($"===== round {round} =====");
                foreach (var unit in sceneManager.Scene.Units.ToArray())
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

            foreach (var clan in scene.Clans.Where(c => c.Units.Count > 0))
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
        private readonly Settings settings;
        private readonly UwpViewer viewer;
        private readonly SceneManager sceneManager;
        public IPage Page { get; set; }

        public Controller(ILog log, Settings settings, UwpViewer viewer, SceneManager sceneManager)
        {
            this.log = log;
            this.settings = settings;
            this.viewer = viewer;
            this.sceneManager = sceneManager;
        }

        #endregion
    }
}