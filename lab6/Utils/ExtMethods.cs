using System.Windows;
using System.Windows.Controls;

namespace RomeCalc.Utils
{
    public static class ExtMethods
    {
        public static void PerformClick(this Button btn)
            => btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    }
}
