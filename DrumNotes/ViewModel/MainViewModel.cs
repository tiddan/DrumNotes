using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using System.Xml;
using System.Xml.Serialization;
using DrumNotes.Model;
using DrumNotes.Util;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

namespace DrumNotes.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Song> m_songs;
        private Song m_selectedSong;
        private RelayCommand m_addNewSong;
        private RelayCommand m_save;
        private RelayCommand m_moveUp;
        private RelayCommand m_moveDown;
        private RelayCommand m_toggleEditMode;
        private string m_editWidth;
        private RelayCommand m_addNewSection;
        private RelayCommand m_tempoClicked;
        private DateTime m_lastTempoClick;
        private double m_tempoAverage;

        protected TempoUtility m_tempoUtility = SimpleIoc.Default.GetInstance<TempoUtility>();
        private List<double> m_tempoAverages;
        private int m_tempoAverageInt;
        protected BackgroundWorker m_bgWorker = new BackgroundWorker();
        private string m_startAnimation;
        private RelayCommand m_deleteSong;
        private RelayCommand m_deleteSection;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            MainViewModel_Designer();

            m_bgWorker.DoWork += (sender, args) =>
            {
                while (true)
                {
                    StartAnimation = "ON";
                    StartAnimation = "OFF";

                    Thread.Sleep(500);
                }
            };

            m_bgWorker.RunWorkerAsync();
        }

        public string EditWidth
        {
            get { return m_editWidth ?? (m_editWidth = "0"); }
            set { m_editWidth = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<Song> Songs
        {
            get { return m_songs ?? (m_songs = new ObservableCollection<Song>()); }
            set { m_songs = value; RaisePropertyChanged(); }
        }

        public Song SelectedSong
        {
            get { return m_selectedSong ?? (m_selectedSong = Songs.FirstOrDefault()); }
            set { m_selectedSong = value; RaisePropertyChanged(); }
        }

        public DateTime LastTempoClick
        {
            get { return m_lastTempoClick; }
            set { m_lastTempoClick = value; RaisePropertyChanged(); }
        }

        public string StartAnimation
        {
            get { return m_startAnimation; }
            set { m_startAnimation = value; RaisePropertyChanged(); }
        }

        public double TempoAverage
        {
            get { return m_tempoAverage; }
            set { m_tempoAverage = value; RaisePropertyChanged(); }
        }

        public List<double> TempoAverages
        {
            get { return m_tempoAverages ?? (m_tempoAverages = new List<double>()); }
            set { m_tempoAverages = value; RaisePropertyChanged(); }
        }

        public void MainViewModel_Designer()
        {
            var appDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!File.Exists(appDir+@"\Songs.xml")) return;

            var serializer = new XmlSerializer(typeof (ObservableCollection<Song>));
            var reader = new StreamReader(appDir+@"\Songs.xml");
            var songs = (ObservableCollection<Song>) serializer.Deserialize(reader);
            Songs.Clear();
            foreach (var song in songs) Songs.Add(song);
            reader.Close();
        }

        public RelayCommand AddNewSong
        {
            get
            {
                return m_addNewSong ?? (m_addNewSong = new RelayCommand(() =>
                {
                    var newSong = new Song {Title = "New Song", Tempo = 60};
                    Songs.Add(newSong);
                    SelectedSong = newSong;
                }));
            }
        }

        public RelayCommand AddNewSection
        {
            get
            {
                return m_addNewSection ?? (m_addNewSection = new RelayCommand(() =>
                {
                    var newSection = new Section {Intensity = IntensityLevels.Normal, Type = SectionType.Verse};
                    SelectedSong.Sections.Add(newSection);
                    SelectedSong.SelectedSection = newSection;
                }));
            }
        }

        public RelayCommand Save
        {
            get
            {
                return m_save ?? (m_save = new RelayCommand(() =>
                {
                    var appDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                    var xsSubmit = new XmlSerializer(typeof(ObservableCollection<Song>));
                    var sww = new StringWriter();
                    var writer = XmlWriter.Create(sww);
                    xsSubmit.Serialize(writer, Songs);
                    var xml = sww.ToString(); // Your xml
                    if (File.Exists(appDir+ @"\Songs.xml"))
                    {
                        File.Delete(appDir+@"\Songs.xml.bak");
                        File.Move(appDir+@"\Songs.xml",appDir+@"\Songs.xml.bak");
                    }
                    var file = new StreamWriter(appDir+@"\Songs.xml",false);
                    file.Write(xml);
                    file.Close();
                }));
            }
        }

        public RelayCommand MoveUp
        {
            get
            {
                return m_moveUp ?? (m_moveUp = new RelayCommand(() =>
                {
                    var pos = SelectedSong.SectionsView.CurrentPosition;
                    if (pos == 0) return;
                    var tmp = SelectedSong.Sections[pos];
                    SelectedSong.Sections[pos] = SelectedSong.Sections[pos - 1];
                    SelectedSong.Sections[pos - 1] = tmp;
                    SelectedSong.SectionsView.MoveCurrentToPosition(pos - 1);
                }));
            }
        }

        public RelayCommand MoveDown
        {
            get
            {
                return m_moveDown ?? (m_moveDown = new RelayCommand(() =>
                {
                    var pos = SelectedSong.SectionsView.CurrentPosition;
                    if (pos >= SelectedSong.Sections.Count - 1) return;
                    var tmp = SelectedSong.Sections[pos];
                    SelectedSong.Sections[pos] = SelectedSong.Sections[pos + 1];
                    SelectedSong.Sections[pos + 1] = tmp;
                    SelectedSong.SectionsView.MoveCurrentToPosition(pos + 1);
                }));
            }
            set { m_moveDown = value; }
        }

        public RelayCommand ToggleEditMode
        {
            get
            {
                return m_toggleEditMode ?? (m_toggleEditMode = new RelayCommand(() =>
                {
                    EditWidth = EditWidth == "3*" ? "0" : "3*";
                }));
            }
        }

        public RelayCommand TempoClicked
        {
            get
            {
                return m_tempoClicked ?? (m_tempoClicked = new RelayCommand(() =>
                {
                    if (DateTime.Now - LastTempoClick > new TimeSpan(0, 0, 3))
                    {
                        Console.WriteLine(@"Resetting time");
                        LastTempoClick = DateTime.Now;
                        TempoAverages = new List<double>();
                        TempoAverage = 0;
                    }
                    else
                    {
                        var now = DateTime.Now;
                        var tempo = m_tempoUtility.CalculateTampo(LastTempoClick, now);

                        TempoAverages.Add(tempo);
                        TempoAverage = TempoAverages.Sum()/TempoAverages.Count;
                        SelectedSong.Tempo = (int)Math.Round(TempoAverage);

                        LastTempoClick = now;
                        Console.WriteLine(@"Current tempo: " + SelectedSong.Tempo);
                    }
                }));
            }
        }

        public RelayCommand DeleteSong
        {
            get
            {
                return m_deleteSong ?? (m_deleteSong = new RelayCommand(() =>
                {
                    Songs.Remove(SelectedSong);
                    var lastSong = Songs.LastOrDefault();
                    if(lastSong!=null) SelectedSong = lastSong;
                }));
            }
        }

        public RelayCommand DeleteSection
        {
            get
            {
                return m_deleteSection ?? (m_deleteSection = new RelayCommand(() =>
                {
                    SelectedSong.Sections.Remove(SelectedSong.SelectedSection);
                    var lastSection = SelectedSong.Sections.LastOrDefault();
                    if (lastSection != null) SelectedSong.SelectedSection = lastSection;
                }));
            }
            
        }
    }
}