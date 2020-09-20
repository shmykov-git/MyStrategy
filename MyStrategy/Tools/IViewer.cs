using System.Reflection;
using MyStrategy.DataModel;

namespace MyStrategy.Tools
{
    public interface IViewer
    {
        void OnPropertyChange(Unit unit, PropertyInfo property, object value);
    }
}