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
using Suit.Extensions;
using Suit.Logs;
using Viewer.Uwp.Commands;
using Color = Windows.UI.Color;
using Point = Windows.Foundation.Point;

namespace Viewer.Uwp.Viewer
{
    class Controller
    {
        private readonly Dictionary<int, Ellipse> unitControls = new Dictionary<int, Ellipse>();
        private double sceneHeight;

        public void MoveUnit(Unit unit, Vector position)
        {
            var control = GetUnitControl(unit);
            var w2 = control.Width / 2;
            var h2 = control.Height / 2;

            Canvas.SetLeft(control, unit.Position.X - w2);
            Canvas.SetTop(control, sceneHeight - unit.Position.Y - h2);
        }

        public void SetUnitHp(Unit unit, float hp)
        {
            var control = GetUnitControl(unit);
            ((LinearGradientBrush) control.Fill).GradientStops[1].Offset = 1 - (double) unit.Hp / unit.BaseHp;

            MoveUnit(unit, unit.Position);
        }

        public void Kill(Unit unit)
        {
            var control = GetUnitControl(unit);
            Page.Canvas.Children.Remove(control);
            unitControls.Remove(unit.Id);
        }

        public void Create(Unit unit)
        {
            GetUnitControl(unit);
        }

        private void AddUnit(Point point)
        {
            log.Debug($"point:{point}");
            viewer.ClickCommand?.Execute((point.X, sceneHeight - point.Y));
        }

        public void Start()
        {
            viewer.Controller = this;
            this.sceneHeight = settings.SceneHeight;

            var context = new
            {
                CanvasWidth = settings.SceneWidth,
                CanvasHeight = settings.SceneHeight,
                SelectClanCommand = new Command(clanName => viewer.ClanNameChangedCommand.Execute(clanName.ToString())),
                RefreshCommand = new TaskCommand(Play),
            };
            Page.DataContext = context;

            this.Page.Canvas.PointerPressed += (o, a) => { AddUnit(a.GetCurrentPoint(this.Page.Canvas).Position); };

            context.RefreshCommand.Execute(null);
        }

        private Task Play()
        {
            viewer.IsActive = false;
            sceneManager.InitScene();
            viewer.IsActive = true;

            unitControls.Clear();
            Page.Canvas.Children.Cast<Ellipse>().ToArray().ForEach(ellipse => Page.Canvas.Children.Remove(ellipse));

            return sceneManager.PlayScene();
        }

        private Ellipse GetUnitControl(Unit unit)
        {
            if (unitControls.ContainsKey(unit.Id))
                return unitControls[unit.Id];

            var clanColor = ClanToColor(unit.Clan);
            var brush = new LinearGradientBrush() {StartPoint = new Point(0.5, 0), EndPoint = new Point(0.5, 1)};
            brush.GradientStops.Add(new GradientStop() { Color = Colors.White, Offset = 0 });
            brush.GradientStops.Add(new GradientStop() { Color = clanColor, Offset = 0 });
            brush.GradientStops.Add(new GradientStop() { Color = clanColor, Offset = 1 });

            var control = new Ellipse()
            {
                //Fill = new SolidColorBrush() { Color = ClanToColor(unit.Clan) },
                Fill = brush,
                StrokeThickness = 1,
                Stroke = new SolidColorBrush() { Color = Colors.Black }
            };
            control.Height = unit.Radius * 2;
            control.Width = unit.Radius * 2;

            unitControls.Add(unit.Id, control);

            SetUnitHp(unit, unit.Hp);
            MoveUnit(unit, unit.Position);

            Page.Canvas.Children.Add(control);

            return unitControls[unit.Id];
        }

        //private double HpToDiameter(float hp)
        //{
        //    return 5 + 5 * Math.Log(1 + hp);
        //}

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