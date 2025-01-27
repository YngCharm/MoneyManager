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
    
namespace Money_Manager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDragging = false;
        private Point clickPosition;
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Content = new LoginView();
        }
        public string TitleBarText
        {
            get { return TitleBar.Text; }
            set { TitleBar.Text = value; }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDragging = true;
                clickPosition = e.GetPosition(this);
                this.Cursor = Cursors.Hand;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var currentPosition = e.GetPosition(this);
                var offset = currentPosition - clickPosition;
                this.Left += offset.X;
                this.Top += offset.Y;
                clickPosition = currentPosition;
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            this.Cursor = Cursors.Arrow;
        }

        private void btnMinimize_CLick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
