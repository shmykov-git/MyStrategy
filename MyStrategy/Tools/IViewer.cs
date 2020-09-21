using System;
using System.Reflection;
using System.Windows.Input;
using MyStrategy.Commands;
using MyStrategy.DataModel;

namespace MyStrategy.Tools
{
    public interface IViewer
    {
        Command<string> ClanNameChangedCommand { get; set; }
        Command<Vector> ClickCommand { get; set; }

        void OnPropertyChange(Unit unit, PropertyInfo property, object value);
        void OnKill(Unit unit);
        void OnCreate(Unit unit);
    }
}