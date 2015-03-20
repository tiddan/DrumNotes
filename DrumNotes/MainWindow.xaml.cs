using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DrumNotes.Util;
using DrumNotes.ViewModel;

namespace DrumNotes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker m_bgWorker;
        private Storyboard m_sb;
        private MainViewModel m_vm;
        private TempoUtility m_tempoUtility = new TempoUtility();

        public MainWindow()
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.Initialize();
            InitializeComponent();
            m_sb = FindResource("BlinkAnimation") as Storyboard;
            m_bgWorker = new BackgroundWorker();
            m_bgWorker.DoWork += BgWorkerOnDoWork;
            m_bgWorker.RunWorkerAsync();
            m_vm = DataContext as MainViewModel;
        }

        private void BgWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (true)
            {
                GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    m_sb.Begin();
                });

                Thread.Sleep(m_vm.SelectedSong.Tempo > 30 && m_vm.SelectedSong.Tempo < 300 ? m_tempoUtility.CalculateMs(m_vm.SelectedSong.Tempo) : m_tempoUtility.CalculateMs(30));
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
