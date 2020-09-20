using System.Reflection;
using MyStrategy.DataModel;
using MyStrategy.Tools;
using Suit.Logs;

namespace Viewer.Uwp.Viewer
{
    class UwpViewer : IViewer
    {
        public Controller Controller { get; set; }
        public bool IsActive { get; set; }

        public void OnKill(Unit unit)
        {
            Controller.Kill(unit);
        }

        public void OnPropertyChange(Unit unit, PropertyInfo property, object value)
        {
            if (!IsActive)
                return;

            switch (property.Name)
            {
                case nameof(Unit.Position):
                    log.Debug($" >>{unit} moves to {value}");
                    Controller.MoveUnit(unit, (Vector)value);
                    break;

                case nameof(Unit.Hp):
                    log.Debug($" >>{unit} hp:{value:F0}");
                    Controller.SetUnitHp(unit, (float)value);
                    break;
            }
        }

        #region IoC
        
        private readonly ILog log;

        public UwpViewer(ILog log)
        {
            this.log = log;
        }

        #endregion
    }
}
