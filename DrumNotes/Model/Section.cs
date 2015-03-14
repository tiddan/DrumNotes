using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DrumNotes.Annotations;

namespace DrumNotes.Model
{
    public enum IntensityLevels
    {
        NoDrums = 0,
        Light = 1,
        Normal = 2,
        High = 3
    };

    public enum SectionType
    {
        Intro = 0,
        Verse = 1,
        Ref = 2,
        Bridge = 3,
        Outtro = 4,
        Other = 5
    };

    public class Section : INotifyPropertyChanged
    {
        private IntensityLevels m_intensity;
        private SectionType m_type;
        private string m_note;

        

        public IntensityLevels Intensity
        {
            get { return m_intensity; }
            set { m_intensity = value; OnPropertyChanged(); }
        }

        public SectionType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public string Note
        {
            get { return m_note; }
            set { m_note = value; OnPropertyChanged(); }
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
