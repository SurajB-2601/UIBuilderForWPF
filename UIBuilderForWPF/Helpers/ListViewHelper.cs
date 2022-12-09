using UIBuilderForWPF.Helpers;
using UIBuilderForWPF.Interfaces;
using UIBuilderForWPF.Model;
using UIBuilderForWPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UIBuilderForWPF.Helpers
{
    public class ListViewHelper
    {
        public int GetListViewItemIndexForPoint(ListView listView, Point mousePosition)
        {
            int index = -1;
            for (int i = 0; i < listView.Items.Count; ++i)
            {
                ListViewItem item = GetListViewItem(listView, i);
                if (this.IsMouseOverTarget(item, mousePosition))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public bool IsMouseOverTarget(Visual target, Point mousePosition)
        {
            Point startPoint = target.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            bounds.X = startPoint.X;
            bounds.Y = startPoint.Y;
            return bounds.Contains(mousePosition);
        }

        public ListViewItem GetListViewItem(ListView listView, int index)
        {
            if (listView.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;

            return listView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }
    }
}
