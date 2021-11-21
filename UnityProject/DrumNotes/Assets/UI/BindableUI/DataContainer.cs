using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UI.BindableUI
{
    public abstract class DataContainer : IDisposable
    {
        private readonly List<string> pathPrefixes = new List<string>();

        private string uniqueId;
        // public string UniqueId { get; } = System.Guid.NewGuid().ToString().Replace("-", "");

        public string UniqueId
        {
            get
            {
                if (string.IsNullOrEmpty(uniqueId))
                {
                    uniqueId = System.Guid.NewGuid().ToString().Replace("-", "");
                }
        
                return uniqueId;
            }
            set => uniqueId = value;
        }


        protected DataContainer(string prefix, DataContainer parent = null)
        {
            /* If prefix is null then use UniqueId */
            pathPrefixes.Add(prefix ?? UniqueId);

            if (parent!=null)
            {
                foreach (var pathPrefix in parent.pathPrefixes)
                {
                    pathPrefixes.Add(pathPrefix + "." + prefix ?? UniqueId);    
                }
            }
        }

        public void AddPrefix(string prefix)
        {
            pathPrefixes.Add(prefix);
        }

        public void RemovePrefix(string prefix)
        {
            pathPrefixes.Remove(prefix);
        }

        ~DataContainer()
        {
            UIEventManager.Instance.UnregisterContainer(UniqueId);
            UIEventManager.Instance.RemoveBindings(UniqueId);
        }

        public void Dispose()
        {
            // UIEventManager.Instance.UnregisterContainer(UniqueId);
            // UIEventManager.Instance.RemoveBindings(UniqueId);
        }

        protected virtual void OnDataChanged([CallerMemberName] string propertyName = null)
        {
            var property = GetType().GetProperty(propertyName);
            if (property == null) 
                throw new UnityException("Property should not be null");

            foreach (var prefix in pathPrefixes)
            {
                var propertyChangeName = prefix + "." + propertyName;
                UIEventManager.Instance.NotifyChange(propertyChangeName, property.GetValue(this));
            }

            /* Note: This might be not smart. Could potentially be circular ( Stack Overflow ) */
            if (property.GetType().IsSubclassOf(typeof(DataContainer)))
            {
                var subDataContainer = (property.GetValue(this) as DataContainer);
                subDataContainer?.TriggerChangeOnAllProperties();
            }
        }

        public void UpdateProperty(string propertyName, object value)
        {
            var property = GetType().GetProperty(propertyName);
            if (property == null) throw new UnityException("Error updating property. Property does not exist.");
            property.SetValue(this, value);
        }

        public void TriggerChangeOnAllProperties()
        {
            var properties = GetType().GetProperties().Select(x => x.Name).ToList();
            foreach (var property in properties)
            {
                OnDataChanged(property);
            }
        }
    }
}