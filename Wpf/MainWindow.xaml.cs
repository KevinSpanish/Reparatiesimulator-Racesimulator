using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private RaceStatistics? _raceStatistics;
        private CompetitionStatistics? _competitionStatistics;

        public MainWindow()
        {
            Data.Initialize();
            Initialize();
            InitializeComponent();
        }

        private void Initialize()
        {
            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Data.CurrentRace.NextRace += OnNextRace;

            Visualization.Initialize(Data.CurrentRace);
        }

        public void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            TrackImage.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    TrackImage.Source = null;
                    TrackImage.Source = Visualization.DrawTrack(e.Track);
                }));
        }

        private void OnNextRace(object model, NextRaceEventArgs e)
        {
            Data.NextRace();
            Initialize();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void MenuItem_Open_CometitionStatisticsWindow(object sender, RoutedEventArgs e)
        {
            _competitionStatistics = new CompetitionStatistics();
            _competitionStatistics.Show();
        }

        private void MenuItem_Open_RaceStatisticsWindow(object sender, RoutedEventArgs e)
        {
            _raceStatistics = new RaceStatistics();
            _raceStatistics.Show();
        }
    }
}
