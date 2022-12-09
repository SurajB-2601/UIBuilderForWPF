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

namespace UIBuilderForWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UIElementType CurrentRenderElement { get; set; }
        Timer completionTimer ;
        IUIElementProviderService UIElementProviderService;
        ListViewHelper listViewHelper;
        public MainWindow(IUIElementProviderService UIElementProviderService, ListViewHelper listViewHelper)
        {
            InitializeComponent();
            this.UIElementProviderService = UIElementProviderService;
            this.listViewHelper = listViewHelper;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView != null && listView.SelectedItem != null)
            {
                UIElementAnimationHelper animationHelper = new UIElementAnimationHelper(this);//  UIElementAnimationHelper.GetInstance()
                if (listView.SelectedItem as Button != null)
                {
                    CurrentRenderElement = UIElementType.Button;
                } 
                else if (listView.SelectedItem as CheckBox != null)
                {
                    CurrentRenderElement = UIElementType.Checkbox;
                }
                else if (listView.SelectedItem as TextBlock != null)
                {
                    CurrentRenderElement = UIElementType.Label;
                }
                else if (listView.SelectedItem as TextBox != null)
                {
                    CurrentRenderElement = UIElementType.EditText;
                }
                
                UIElement animationControl = this.UIElementProviderService.GetElement(CurrentRenderElement);

                toolboxListView.IsHitTestVisible = false;
                canvasGrid.Children.Add(animationControl);
                animationHelper.RegisterTransform(animationControl);

                Point startingObjectPosition = (listView.SelectedItem as Visual).TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
                Point endingObjectPosition = renderedListView.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
                    
                if (renderedListView.Items.Count > 0)
                {
                    endingObjectPosition.Y = endingObjectPosition.Y + renderedListView.DesiredSize.Height;
                }

                animationHelper.AnimationCompleted += AnimationHelper_AnimationCompleted;
                animationHelper.AddElementToCollectionWithAnimation(startingObjectPosition, endingObjectPosition);
                listView.UnselectAll();
            }
        }

        private void AnimationHelper_AnimationCompleted(object? sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                canvasGrid.Children.Clear();
                var elementToAdd = this.UIElementProviderService.GetElement(CurrentRenderElement);
                toolboxListView.IsHitTestVisible = true;
                elementToAdd.IsHitTestVisible = false;
                renderedListView.Items.Add(elementToAdd);
            });
        }

        Point dragStartPoint;
        int dragStartItemIndex = -1;
        bool m_IsDraging = false;

        private void renderedListView_Drop(object sender, DragEventArgs e)
        {
            var data = e.Data;
            Point dropPoint = e.GetPosition(this);

            if (data.GetDataPresent(typeof(UIElement)))
            {
                var droppedElement = data.GetData(typeof(UIElement)) as UIElement;
                int dropItemIndex = listViewHelper.GetListViewItemIndexForPoint(renderedListView, dropPoint);
                if (dropItemIndex >= 0 && dragStartItemIndex >= 0)
                {
                    renderedListView.SelectedIndex = dragStartItemIndex;
                    var items = renderedListView.Items;

                    items.RemoveAt(dragStartItemIndex);
                    items.Insert(dropItemIndex , droppedElement);
                }
            }
        }

        private void renderedListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragStartItemIndex = listViewHelper.GetListViewItemIndexForPoint(renderedListView, e.GetPosition(this));
        }

        private void renderedListView_MouseMove(object sender, MouseEventArgs e)
        {
            var listView = sender as ListView;

            if (listView.SelectedItem == null) return;

            if (e.LeftButton == MouseButtonState.Pressed && !m_IsDraging)
            {
                Point position = e.GetPosition(null);

                if (Math.Abs(position.X - dragStartPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - dragStartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    m_IsDraging = true;
                    DataObject data = new DataObject(typeof(UIElement), listView.SelectedItem);
                    DragDropEffects de = DragDrop.DoDragDrop(listView, data, DragDropEffects.Move);
                    m_IsDraging = false;
                }

            }
        }

    }
    
}
