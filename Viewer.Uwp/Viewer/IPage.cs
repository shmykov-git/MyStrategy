using Windows.UI.Xaml.Controls;

namespace Viewer.Uwp.Viewer
{
    interface IPage
    {
        Canvas Canvas { get; }
        double Width { get; }
        double Height { get; }
    }
}