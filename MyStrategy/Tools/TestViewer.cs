using System.Reflection;
using MyStrategy.DataModel;
using Suit.Logs;

namespace MyStrategy.Tools
{
    public class TestViewer : IViewer
    {
        public bool IsActive { get; set; }

        public void OnPropertyChange(Unit unit, PropertyInfo property, object value)
        {
            if (!IsActive)
                return;

            switch (property.Name)
            {
                case nameof(Unit.Position):
                    log.Debug($" >>{unit} moves to {value}");
                    break;

                case nameof(Unit.Hp):
                    log.Debug($" >>{unit} hp:{value:F0}");
                    break;
            }
        }

        #region IoC

        private readonly ILog log;

        public TestViewer(ILog log)
        {
            this.log = log;
        }

        #endregion
    }
}
