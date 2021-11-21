using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace UI.BindableUI
{
    public class UIEventManager
    {
        /* Fields and props */

        private readonly DiContainer container;
        
        #region Singleton
        private static UIEventManager instance;
        public static UIEventManager Instance => instance ?? (instance = new UIEventManager());
        #endregion

        #region Public properties
        private Dictionary<string, DataContainer> DataContainers { get; } = new Dictionary<string, DataContainer>();
        public Dictionary<string, BindingData> Bindings { get; } = new Dictionary<string, BindingData>();
        public int BindingRemovals { get; private set; }

        #endregion
        
        #region Private properties
        
        #endregion

        /* Methods */
        
        #region Public methods
        public void Subscribe(string bindingPath, GenericDataBinding binding)
        {
            if (!Bindings.ContainsKey(bindingPath)) Bindings.Add(bindingPath, new BindingData());
            Bindings[bindingPath].Bindings.Add(binding);
            GetLatestValue(bindingPath, binding);
        }

        public void GetLatestValue(string bindingPath, GenericDataBinding binding)
        {
            if(Bindings.ContainsKey(bindingPath))
                binding.OnValueChanged(Bindings[bindingPath].LastValue);
        }

        public void Unsubscribe(string bindingPath, GenericDataBinding binding)
        {
            try
            {
                if (!Bindings.ContainsKey(bindingPath))
                    return;
                
                Bindings[bindingPath].Bindings.Remove(binding);
                if (Bindings[bindingPath].Bindings.Count == 0)
                {
                    Bindings.Remove(bindingPath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Trying to unsubscribe '{bindingPath}' but failed. Error: {e.Message}");
            }
        }

        public void RegisterContainer(DataContainer container)
        {
            if (!DataContainers.ContainsKey(container.UniqueId))
            {
                DataContainers.Add(container.UniqueId, container);
            }
        }

        public void UnregisterContainer(string key)
        {
            if (DataContainers.ContainsKey(key))
            {
                DataContainers.Remove(key);
            }
        }

        public bool ContainsDataContainer(string key)
        {
            return DataContainers.ContainsKey(key);
        }

        public int ContainerCount => DataContainers.Count;

        public DataContainer GetDataContainer(string key)
        {
            return DataContainers.ContainsKey(key) ? DataContainers[key] : null;
        }

        public void ClearAllBindings()
        {
            foreach (var binding in Instance.Bindings)
            {
                binding.Value.Bindings.Clear();
            }

            Instance.Bindings.Clear();
            Instance.DataContainers.Clear();
        }

        public void NotifyChange(string bindingPath, object newValue)
        {
            if(bindingPath.Contains("Controlled")) Debug.Log("LOL");
            if (!Bindings.ContainsKey(bindingPath))
            {
                Bindings.Add(bindingPath, new BindingData());
            }

            Bindings[bindingPath].LastValue = newValue;
            foreach (var binding in Bindings[bindingPath].Bindings)
            {
                if (binding == null || !binding.enabled) continue;
                binding.OnValueChanged(newValue);
            }
        }

        public void UpdateBinding(string bindingPath, object newValue)
        {
            /* Resolve root */
            var parts = new List<string>(bindingPath.Split('.'));
            if (parts.Count < 2) throw new UnityException("Binding path expression is not valid");

            var rootName = parts[0];
            DataContainer root;
            if (DataContainers.ContainsKey(rootName))
            {
                root = DataContainers[rootName];
            }
            else
            {
                root = GetRootFromString(rootName) as DataContainer;
            }

            if (root == null) throw new UnityException("Could not find root");

            /* Resolve the rest */
            var restOfBindingName = parts.Aggregate((curr, next) => curr + "." + next);
            ChangeFromRoot(root, restOfBindingName, newValue);
        }

        public void ExecuteMethod(string bindingPath, object param)
        {
            /* Resolve root */
            var parts = new List<string>(bindingPath.Split('.'));
            if (parts.Count < 2) throw new UnityException("Binding path expression is not valid");

            var rootName = parts[0];
            parts.RemoveAt(0);

            DataContainer root;
            if (DataContainers.ContainsKey(rootName))
            {
                root = DataContainers[rootName];
            }
            else
            {
                root = GetRootFromString(rootName) as DataContainer;
            }

            if (root == null) throw new UnityException("Could not find root");

            var currentObj = root;
            while (parts.Count > 1)
            {
                var nextPropertyName = parts.ElementAt(0);
                parts.RemoveAt(0);

                var nextProperty = currentObj.GetType().GetProperty(nextPropertyName);
                if (nextProperty == null) throw new UnityException("Error finding the property");

                currentObj = nextProperty.GetValue(currentObj) as DataContainer;
                if (currentObj == null)
                    throw new UnityException(
                        "Error trying to process expression. Part of expressin is not a data container.");
            }

            var methodName = parts.ElementAt(0);
            var method = currentObj.GetType().GetMethod(methodName);
            if (method == null) throw new UnityException("Unable to find method");
            method.Invoke(currentObj, new[] {param});
        }
        #endregion
        
        #region Private methods

        private void ChangeFromRoot(DataContainer root, string bindingPath, object newValue)
        {
            var parts = new List<string>(bindingPath.Split('.'));
            parts.RemoveAt(0);

            var currentObj = root;

            while (parts.Count > 1)
            {
                var nextPropertyName = parts.ElementAt(0);
                parts.RemoveAt(0);

                var nextProperty = currentObj.GetType().GetProperty(nextPropertyName);
                if (nextProperty == null) throw new UnityException("Error finding the property");

                currentObj = nextProperty.GetValue(currentObj) as DataContainer;
                if (currentObj == null)
                    throw new UnityException(
                        "Error trying to process expression. Part of expression is not a data container.");
            }

            var lastPropName = parts.ElementAt(0);
            var lastProp = currentObj.GetType().GetProperty(lastPropName);
            if (lastProp == null)
                throw new UnityException("Unable to find the last property of the binding expression: " + bindingPath);
            lastProp.SetValue(currentObj, newValue);
        }
        
        /// <summary>
        /// TODO: Need to remove these non-generic refs asap! 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="UnityException"></exception>
        private object GetRootFromString(string name)
        {
            var type = Type.GetType(name);
            if(type == null)
                throw new UnityException($"Error in 'GetRootFromString({name}): Type not found. Make sure your VM has no namespace.");

            return container.Resolve(type);
        }

        private UIEventManager()
        {
            container = GameObject.FindObjectOfType<SceneContext>().Container;
        }

        #endregion

        
        public void RemoveBindings(string prefix)
        {
            var removeKeys = new List<string>();
            
            foreach (var keyValuePair in Bindings.Where(x => x.Key.Contains(prefix)))
            {
                removeKeys.Add(keyValuePair.Key);
            }
            
            var countBefore = Bindings.Count;
            foreach (var key in removeKeys.ToList())
            {
                Bindings.Remove(key);
            }
            
            Debug.Log($"REMOVED {removeKeys.Count} keys. Before: {countBefore}. After: {Bindings.Count}");
        }
    }
}