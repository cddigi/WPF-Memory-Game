/*
    Author: Cornelius Donley
    Solution: Project5
    File: GameImage.cs
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Project5
{
    enum GameState { NoneShowing, OneShowing, TwoShowing }
    enum ImageState { Hidden, Showing, Cleared }

    class GameImage : Image
    {
        Uri hiddenUri;
        Uri clearedUri;
        BitmapImage hidden;
        BitmapImage cleared;
        BitmapImage showing;
        public readonly int ImageIndex;
        public ImageState State;

        public GameImage(int index)
        {
            ImageIndex = index;
            hiddenUri = new Uri("pack://application:,,,/images/hidden.png");
            clearedUri = new Uri("pack://application:,,,/images/cleared.png");
            Uri showingUri = new Uri("pack://application:,,,/images/car" + ImageIndex + ".png");
            hidden = new BitmapImage(hiddenUri);
            cleared = new BitmapImage(clearedUri);
            showing = new BitmapImage(showingUri);
            Hide();
        }

        public void Hide()
        {
            State = ImageState.Hidden;
            this.Source = hidden;
        }

        public void Clear()
        {
            State = ImageState.Cleared;
            this.Source = cleared;
        }

        public void Show()
        {
            State = ImageState.Showing;
            this.Source = showing;
        }
    }
}
