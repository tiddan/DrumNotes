using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UI.BindableUI;
using UnityEngine;

namespace UI
{
    public class MainViewModel : DataContainer
    {
        public MainViewModel(string prefix, DataContainer parent = null) : base(prefix, parent)
        {
        }
    }

    public class SetListViewModel : DataContainer
    {
        public SetListViewModel(string prefix, DataContainer parent = null) : base(prefix, parent)
        {
        }
    }

    public class SongViewModel : DataContainer
    {
        private ObservableCollection<SongPartViewModel> songParts;
        private string songTitle;
        private int songBpm;

        public ObservableCollection<SongPartViewModel> SongParts
        {
            get => songParts;
            set { songParts = value; OnDataChanged(); }
        }

        public string SongTitle
        {
            get => songTitle;
            set { songTitle = value; OnDataChanged(); }
        }

        public int SongBpm
        {
            get => songBpm;
            set { songBpm = value; OnDataChanged(); }
        }

        public SongViewModel(string prefix, DataContainer parent = null) : base(prefix, parent)
        {
        }
    }

    public class SongPartViewModel : DataContainer
    {
        private SongPartTypes partType;

        public SongPartTypes PartType
        {
            get => partType;
            set { partType = value; OnDataChanged(); }
        }

        public SongPartViewModel(string prefix, DataContainer parent = null) : base(prefix, parent)
        {
        }
    }

    public enum SongPartTypes
    {
        Intro,
        Verse,
        Refrain,
        Interlude,
        Bridge,
        Instrumental,
    }
    
}
