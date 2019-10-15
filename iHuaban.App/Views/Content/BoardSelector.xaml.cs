using iHuaban.App.Models;
using iHuaban.App.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace iHuaban.App.Views
{
    public sealed partial class BoardSelector : UserControl
    {
        private Popup popup;
        public BoardSelector()
        {
            this.InitializeComponent();
            this.SetBounds();
            Window.Current.SizeChanged += Current_SizeChanged;

            popup = new Popup();
            popup.Child = this;
            this.DataContext = UnityConfig.ResolveObject<BoardSelectorViewModel>();
        }

        private BoardSelectorViewModel ViewModel
        {
            get
            {
                return this.DataContext as BoardSelectorViewModel;
            }
        }
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            SetBounds();
        }

        private void SetBounds()
        {
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
        }

        private void OnShow(Action<Board> afterSelectBoard)
        {
            popup.IsOpen = true;
            this.ViewModel.AfterSelectBoard = afterSelectBoard;
        }

        private static BoardSelector instance;
        internal static void Show(Action<Board> afterSelectBoard)
        {
            instance = instance ?? new BoardSelector();
            instance.OnShow(afterSelectBoard);
        }
    }
}
