using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Media;
using DrumNotes.Model;

namespace DrumNotes.Converters
{
    public class TypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SectionType type;
            if (Enum.TryParse(value.ToString(), true, out type))
            {
                switch (type)
                {
                    case SectionType.Intro:
                        return Brushes.Gray;
                    case SectionType.Verse:
                        return Brushes.LightGreen;
                    case SectionType.Ref:
                        return Brushes.LightBlue;
                    case SectionType.Bridge:
                        return Brushes.LightSalmon;
                    case SectionType.Outtro:
                        return Brushes.Gray;
                    case SectionType.Other:
                        return Brushes.LightYellow;

                }
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
