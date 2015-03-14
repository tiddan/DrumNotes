using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using DrumNotes.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

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

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            MainViewModel_Designer();
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
                    Songs.Add(new Song{Title = "New Song"});
                }));
            }
        }

        public RelayCommand AddNewSection
        {
            get
            {
                return m_addNewSection ?? (m_addNewSection = new RelayCommand(() =>
                {
                    SelectedSong.Sections.Add(new Section{Intensity = IntensityLevels.Normal, Type = SectionType.Verse});
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
    }
}