/*
    Author: Cornelius Donley
    Solution: Project5
    File: MainWindow.xaml.cs
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Project5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameImage[] gameImages = new GameImage[28];
        GameState state = GameState.NoneShowing;
        int clickCount, sec, min, hr, numCleared;
        int [] imgsClicked = new int[2];
        DispatcherTimer clockTimer = new DispatcherTimer();
        DispatcherTimer waitTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            CreateBorders();
            Reset();
            clockTimer.Tick += Timer_Tick;
            clockTimer.Interval = new TimeSpan(0, 0, 1);
            waitTimer.Tick += Timer_Wait;
            waitTimer.Interval = new TimeSpan(0, 0, 2);
        }

        private void Timer_Wait(object sender, EventArgs e)
        {
            waitTimer.Stop();
            var bd1 = ImageGrid.Children[imgsClicked[0]] as Border;
            var img1 = bd1.Child as GameImage;
            var bd2 = ImageGrid.Children[imgsClicked[1]] as Border;
            var img2 = bd2.Child as GameImage;

            if (img1.ImageIndex == img2.ImageIndex)
            {
                img1.Clear();
                img2.Clear();
                numCleared++;
            }
            else
            {
                img1.Hide();
                img2.Hide();
            }
            state = GameState.NoneShowing;
            if(numCleared == 14)
            {
                clockTimer.Stop();
                MessageBox.Show(String.Format("Congratulations! You Have won the game!\n\nElapsed Time: {0}\nNumber of Clicks: {1}", ElapsedTime.Content.ToString(), clickCount));
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            sec++;
            if(sec == 60) { sec = 0; min++; }
            if(min == 60) { min = 0; hr++; }
            ElapsedTime.Content = String.Format("{0:00}:{1:00}:{2:00}", hr, min, sec);
        }

        private void CreateBorders()
        {
            for(int i = 0; i < gameImages.Length; i++)
            {
                var bd = new Border();
                bd.BorderThickness = new Thickness(1);
                bd.BorderBrush = Brushes.Black;
                bd.Padding = new Thickness(5);
                bd.Margin = new Thickness(5);
                bd.Tag = i;
                ImageGrid.Children.Add(bd);
            }
        }

        private void SetImages()
        {
            var rand = new Random();
            var stack = new Stack<int>();
            while(stack.Count < 14)
            {
                var r = rand.Next(14);
                if(!stack.Contains(r)) stack.Push(r);
            }
            foreach(int idx in stack)
            {
                var r = rand.Next(28);
                while(gameImages[r] != null) r = rand.Next(28);
                gameImages[r] = new GameImage(idx);
                while (gameImages[r] != null) r = rand.Next(28);
                gameImages[r] = new GameImage(idx);
            }
        }

        private void Reset()
        {
            numCleared = clickCount = sec = min = hr = 0;
            NumberClicks.Content = clickCount;
            clockTimer.Start();
            SetImages();
            for(int i = 0; i < gameImages.Length; i++)
            {
                var bd = ImageGrid.Children[i] as Border;
                bd.Child = gameImages[i];
            }
        }

        void Clicked(object sender, InputEventArgs e)
        {
            var bd = sender as Border;
            var img = bd.Child as GameImage;
            
            switch (state)
            {
                case GameState.NoneShowing:
                    img.Show();
                    clickCount++;
                    imgsClicked[0] = (int)bd.Tag;
                    state = GameState.OneShowing;
                    break;
                case GameState.OneShowing:
                    if (imgsClicked[0] == (int)bd.Tag) return;
                    clickCount++;
                    imgsClicked[1] = (int)bd.Tag;
                    img.Show();
                    state = GameState.TwoShowing;
                    waitTimer.Start();
                    break;
            }
            NumberClicks.Content = clickCount;
        }
    }
}
