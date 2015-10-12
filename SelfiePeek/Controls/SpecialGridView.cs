using SelfiePeek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace SelfiePeek.Controls
{
    /// <summary>
    /// The GridView provides no functionality of Drag and Drop of elements
    /// when the elements are stored in a groping variable sized component
    /// This SpecialGridView control based on the GridView
    /// Has been modified to provide the Drag and Drop functionality
    /// This code is based on the example provided by: Irina Pykhova and Greg Lutz
    /// On: http://www.codeproject.com
    /// </summary>
    public class SpecialGridView : GridView
    {
        private ScrollViewer _MyScroll { get; set; }

        public SpecialGridView()
        {
            this.DragItemsStarting += SpecialGridView_DragItemsStarting;
            this.Loaded += SpecialGridView_Loaded;
        }
        #region Incremental Loading
        void SpecialGridView_Loaded(object sender, RoutedEventArgs e)
        {
            _MyScroll = GetVisualChild<ScrollViewer>(this);
            if (_MyScroll != null)
            {
                _MyScroll.ViewChanged += _MyScroll_ViewChanged;
            }
        }

        void _MyScroll_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (_MyScroll.ScrollableHeight - _MyScroll.VerticalOffset < 300)
            {
                if (!App.PeekVM.ActivityVisible)
                {
                    App.PeekVM.LoadMore();
                }
            }
        }

        private T GetVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                DependencyObject v = VisualTreeHelper.GetChild(parent, i) as DependencyObject;
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
        #endregion

        #region Events Region
        void SpecialGridView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            _currentOverIndex = -1;
            _topReorderHintIndex = -1;
            _bottomReorderHintIndex = -1;
            object item = null;
            if (e.Items != null)
            {
                item = e.Items[0];
            }
            if (item != null)
            {
                _lastIndex = this.ItemContainerGenerator.IndexFromContainer(this.ItemContainerGenerator.ContainerFromItem(item));
            }
            OnDragStarting(e);
        }

        protected virtual void OnDragStarting(DragItemsStartingEventArgs e)
        {
            e.Data.Properties.Add("Items", e.Items);
        }

        public event EventHandler<BeforeDropItemsEventArgs> BeforeDrop;

        protected virtual void OnBeforeDrop(BeforeDropItemsEventArgs e)
        {
            if (null != BeforeDrop)
            {
                BeforeDrop(this, e);
            }
        }

        protected override void OnDrop(Windows.UI.Xaml.DragEventArgs e)
        {
            IList<object> items = (IList<object>)e.Data.GetView().Properties["Items"];
            object item = (items != null && items.Count > 0) ? items[0] : Items[_lastIndex];

            int newIndex = GetDragOverIndex(e);
            if (newIndex > 0)
            {
                ICollectionView view = this.ItemsSource as ICollectionView;
                if (newIndex != _lastIndex)
                {
                    if (newIndex > _lastIndex)
                    {
                        newIndex--;
                    }
                    BeforeDropItemsEventArgs args = new BeforeDropItemsEventArgs(item, _lastIndex, newIndex, e);
                    OnBeforeDrop(args);
                    if (!args.Cancel)
                    {
                        System.Collections.IList source = this.ItemsSource as System.Collections.IList;
                        if (source != null)
                        {
                            source.RemoveAt(_lastIndex);
                            source.Insert(newIndex, item);
                        }
                        else
                        {
                            Items.RemoveAt(_lastIndex);
                            Items.Insert(newIndex, item);
                        }
                    }
                }
            }
            _lastIndex = -1;
            _currentOverIndex = -1;

            base.OnDrop(e);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            int newIndex = GetDragOverIndex(e);
            if (newIndex > 0 && _currentOverIndex != newIndex)
            {
                _currentOverIndex = newIndex;
                if (_topReorderHintIndex != -1)
                {
                    GoItemToState(_topReorderHintIndex, "NoReorderHint", true);
                    _topReorderHintIndex = -1;
                }
                if (_bottomReorderHintIndex != -1)
                {
                    GoItemToState(_bottomReorderHintIndex, "NoReorderHint", true);
                    _bottomReorderHintIndex = -1;
                }
                if (newIndex > 0)
                {
                    _topReorderHintIndex = newIndex - 1;
                }
                if (newIndex < Items.Count)
                {
                    _bottomReorderHintIndex = newIndex;
                }
                if (_topReorderHintIndex >= 0)
                {
                    GoItemToState(_topReorderHintIndex, "TopReorderHint", true);
                }
                if (_bottomReorderHintIndex >= 0)
                {
                    GoItemToState(_bottomReorderHintIndex, "BottomReorderHint", true);
                }
            }
            base.OnDragOver(e);
        }

        #endregion

        #region Functionalities Region
        private int _currentOverIndex, _topReorderHintIndex,
            _bottomReorderHintIndex, _lastIndex;

        private int GetDragOverIndex(DragEventArgs e)
        {
            FrameworkElement root = Window.Current.Content as FrameworkElement;
            Point position = this.TransformToVisual(root).TransformPoint(e.GetPosition(this));

            int newIndex = -1;

            foreach (var element in VisualTreeHelper.FindElementsInHostCoordinates(position, root))
            {
                var container = element as ContentControl;
                if (container == null)
                {
                    continue;
                }
                int tempIndex = this.ItemContainerGenerator.IndexFromContainer(container);
                if (tempIndex >= 0)
                {
                    newIndex = tempIndex;
                    Point center = container.TransformToVisual(root).TransformPoint(new Point(container.ActualWidth / 2, container.ActualHeight / 2));
                    if (position.Y > center.Y)
                    {
                        newIndex++;
                    }
                    break;
                }
            }
            if (newIndex < 0)
            {
                foreach (var element in GetIntersectingItems(position, root))
                {
                    var container = element as ContentControl;
                    if (container == null)
                    {
                        continue;
                    }
                    int tempIndex = this.ItemContainerGenerator.IndexFromContainer(container);
                    if (tempIndex < 0)
                    {
                        continue;
                    }
                    Rect bounds = container.TransformToVisual(root).TransformBounds(new Rect(0, 0, container.ActualWidth, container.ActualHeight));
                    if (bounds.Left <= position.X && bounds.Top <= position.Y && tempIndex > newIndex)
                    {
                        newIndex = tempIndex;
                        if (position.Y > bounds.Top + container.ActualHeight / 2)
                        {
                            newIndex++;
                        }
                        if (bounds.Right > position.X && bounds.Bottom > position.Y)
                        {
                            break;
                        }
                    }
                }
            }
            if (newIndex < 0)
            {
                newIndex = 0;
            }
            if (newIndex >= Items.Count)
            {
                newIndex = Items.Count - 1;
            }
            return newIndex;
        }

        private static IEnumerable<UIElement> GetIntersectingItems(Point intersectingPoint, FrameworkElement root)
        {
            Rect rect = new Rect(0, 0, intersectingPoint.X, root.ActualHeight);
            return VisualTreeHelper.FindElementsInHostCoordinates(rect, root);
        }

        private void GoItemToState(int index, string state, bool useTransitions)
        {
            if (index >= 0)
            {
                Control control = this.ItemContainerGenerator.ContainerFromIndex(index) as Control;
                if (control != null)
                {
                    VisualStateManager.GoToState(control, state, useTransitions);
                }
            }
        }
        protected override void PrepareContainerForItemOverride(Windows.UI.Xaml.DependencyObject element, object item)
        {
            var myItem = (SelfiePeekDataModel)item;
            element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, myItem.ColSpan);
            element.SetValue(VariableSizedWrapGrid.RowSpanProperty, myItem.ColSpan);
            base.PrepareContainerForItemOverride(element, item);
        }
        #endregion
    }
    public sealed class BeforeDropItemsEventArgs : System.ComponentModel.CancelEventArgs
    {
        public BeforeDropItemsEventArgs(object item, int oldIndex, int newIndex, DragEventArgs dragEventArgs)
            : base()
        {
            Item = item;
            OldIndex = oldIndex;
            NewIndex = newIndex;

        }
        public object Item
        {
            get;
            private set;
        }

        public int OldIndex
        {
            get;
            private set;
        }

        public int NewIndex
        {
            get;
            private set;
        }

        public DragEventArgs DragEventArgs
        {
            get;
            private set;
        }
    }
}
