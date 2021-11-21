using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using UnityEngine;

namespace UI.BindableUI
{
    /// <summary>
    /// This is a abstract class defining some of the basic
    /// mechanics of Data binding. Whenever you do a Data-binding
    /// you need to inherit from this class.
    /// </summary>
    public abstract class DataBinder : MonoBehaviour
    {
        protected DataContainer Data;

        protected abstract void DataOnPropertyChanged(object sender, PropertyChangedEventArgs e);

        protected string GetStringFromData(string propertyName)
        {
            if (Data == null) return string.Empty;
            var property = Data.GetType().GetProperty(propertyName);
            if (property == null) return string.Empty;
            var textValue = property.GetValue(Data, BindingFlags.GetProperty, null, null, CultureInfo.InvariantCulture).ToString();
            return textValue;
        }

        protected float GetFloatFromData(string propertyName)
        {
            if (Data == null) return float.NaN;
            var property = Data.GetType().GetProperty(propertyName);
            if (property == null) return float.NaN;
            return (float) property.GetValue(
                Data, BindingFlags.GetProperty, 
                null, 
                null, 
                CultureInfo.InvariantCulture 
            );
        }
    }
}