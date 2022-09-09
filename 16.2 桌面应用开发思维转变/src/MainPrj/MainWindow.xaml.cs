using System.Windows;
using _16._2_桌面应用开发思维转变.ViewModel;

namespace _16._2_桌面应用开发思维转变
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            // 数据绑定：数据上下文来源于WindowModel
            this.DataContext = new WindowViewModel();
        }
    }
}
