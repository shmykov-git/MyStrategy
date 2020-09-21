using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using MyStrategy.DataModel;
using MyStrategy.Extensions;
using MyStrategy.Tools;
using Suit.Logs;
using Viewer.Uwp.Commands;
using Color = Windows.UI.Color;

namespace Viewer.Uwp.Viewer
{
    class Controller
    {
        private readonly Dictionary<int, Ellipse> unitControls = new Dictionary<int, Ellipse>();
        private double sceneHeight;

        public void MoveUnit(Unit unit, Vector position)
        {
            var control = unitControls[unit.Id];
            var w2 = control.Width / 2;

            Canvas.SetTop(control, sceneHeight - unit.Position.Y + w2);
            Canvas.SetLeft(control, unit.Position.X - w2);
        }

        public void SetUnitHp(Unit unit, float hp)
        {
            var diameter = HpToDiameter(hp);
            var control = unitControls[unit.Id];
            control.Height = diameter;
            control.Width = diameter;

            MoveUnit(unit, unit.Position);
        }

        public void Kill(Unit unit)
        {
            Page.Canvas.Children.Remove(unitControls[unit.Id]);
            unitControls.Remove(unit.Id);
        }

        private void AddUnit(Point point)
        {
            log.Debug($"point:{point}");
        }

        private string selectedClanName = "1";

        public void Start()
        {
            this.sceneHeight = Page.Canvas.Height;
            ICommand cms;
            this.Page.DataContext = new
            {
                SelectClanCommand = new Command(clanName => selectedClanName = clanName.ToString()),
                RefreshCommand = new Command(_=>{}),
            };
            this.Page.Canvas.PointerPressed += (o, a) => { AddUnit(a.GetCurrentPoint(this.Page.Canvas).Position); };

            sceneManager.InitScene();
            viewer.Controller = this;
            viewer.IsActive = true;

            var scene = sceneManager.Scene;

            foreach (var unit in scene.Units)
            {
                var control = new Ellipse()
                {
                    Fill = new SolidColorBrush() { Color = ClanToColor(unit.Clan) },
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush() { Color = Colors.Black }
                };

                unitControls.Add(unit.Id, control);

                SetUnitHp(unit, unit.Hp);
                MoveUnit(unit, unit.Position);

                Page.Canvas.Children.Add(control);
            }

            sceneManager.PlayScene();
        }

        private double HpToDiameter(float hp)
        {
            return 5 + 5 * Math.Log(1 + hp);
        }

        private Color ClanToColor(Clan clan)
        {
            switch (clan.Name)
            {
                case "1":
                    return Colors.Red;
                case "2":
                    return Colors.Green;
                default:
                    throw new NotImplementedException();
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