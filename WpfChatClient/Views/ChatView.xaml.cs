using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using WpfChatClient.ViewModels;

namespace WpfChatClient.Views
{
    public partial class ChatView : UserControl
    {
        private ScrollViewer? _messageScrollViewer;
        private bool _shouldAutoScroll = true;

        public ChatView()
        {
            InitializeComponent();
            DataContextChanged += ChatView_DataContextChanged;
            Loaded += ChatView_Loaded;
            PreviewKeyDown += ChatView_PreviewKeyDown;
        }

        private void ChatView_Loaded(object sender, RoutedEventArgs e)
        {
            _messageScrollViewer = FindVisualChild<ScrollViewer>(MessagesListBox);
            if (_messageScrollViewer != null)
            {
                _messageScrollViewer.ScrollChanged -= MessageScrollViewer_ScrollChanged;
                _messageScrollViewer.ScrollChanged += MessageScrollViewer_ScrollChanged;
            }
        }

        private void ChatView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ChatViewModel oldVm)
            {
                oldVm.Messages.CollectionChanged -= Messages_CollectionChanged;
            }

            if (e.NewValue is ChatViewModel newVm)
            {
                newVm.Messages.CollectionChanged += Messages_CollectionChanged;
            }
        }

        private void MessageScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var distanceFromBottom = e.ExtentHeight - e.ViewportHeight - e.VerticalOffset;
            _shouldAutoScroll = distanceFromBottom < 80;
        }

        private void Messages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add || !_shouldAutoScroll)
            {
                return;
            }

            Dispatcher.BeginInvoke(() =>
            {
                if (DataContext is ChatViewModel vm && vm.Messages.Count > 0 && MessagesListBox != null)
                {
                    MessagesListBox.ScrollIntoView(vm.Messages[^1]);
                }
            }, DispatcherPriority.Background);
        }

        private void ChatView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && DataContext is ChatViewModel vm && vm.IsProfileCardOpen)
            {
                vm.CloseProfileCommand.Execute(null);
                e.Handled = true;
            }
        }

        private static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                {
                    return typedChild;
                }

                var descendant = FindVisualChild<T>(child);
                if (descendant != null)
                {
                    return descendant;
                }
            }

            return null;
        }
    }
}
