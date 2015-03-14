using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DrumNotes.Annotations;

namespace DrumNotes.Model
{
    public class Song : INotifyPropertyChanged
    {
        private string m_title;
        private ObservableCollection<Section> m_sections;
        private Section m_selectedSection;
        private ICollectionView m_sectionsView;

        public string Title
        {
            get { return m_title; }
            set { m_title = value; OnPropertyChanged(); }
        }

        public Section SelectedSection
        {
            get { return m_selectedSection ?? (m_selectedSection = Sections.FirstOrDefault()); }
            set { m_selectedSection = value; OnPropertyChanged();}
        }

        public ICollectionView SectionsView
        {
            get { return m_sectionsView ?? (m_sectionsView = CollectionViewSource.GetDefaultView(Sections)); }
        }

        public ObservableCollection<Section> Sections
        {
            get { return m_sections ?? (m_sections = new ObservableCollection<Section>()); }
            set { m_sections = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
