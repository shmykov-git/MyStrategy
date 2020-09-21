using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Viewer.Uwp.Viewer
{
    interface IPage
    {
        Canvas Canvas { get; }
        object DataContext { get; set; }
    }
}