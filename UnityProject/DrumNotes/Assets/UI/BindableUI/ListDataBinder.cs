using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UI.LayoutManagers;
using UnityEngine;

namespace UI.BindableUI
{
    public abstract class ListDataBinder<T> : GenericDataBinding where T : DataContainer
    {
        /* Fields and props */    
        
        #region Serialize fields
        [SerializeField] private GameObject itemTemplate;
        [SerializeField] private LayoutManager layoutManager;
        #endregion

        #region Private fields
        private ObservableCollection<T> collection;
        private Dictionary<string, GameObject> listMap = new Dictionary<string, GameObject>();
        private bool dontUpdateLayoutOnAdd = false;
        #endregion
        
        #region Protected properties
        protected abstract Func<T, bool> IncludePredicate { get; set; }
        protected virtual Vector3 InitialPosition { get; } 
        #endregion
        
        /* Methods */
        
        #region Public methods
        /// <summary>
        /// Whenever the value is changed we need to set up the observable collection
        /// stuff again. 
        /// </summary>
        /// <param name="value"></param>
        public override void OnValueChanged(object value)
        {
            if (collection != null)
            {
                foreach (var data in collection)
                {
                    Remove(data);
                    data.Dispose();
                }
                collection.CollectionChanged -= CollectionOnCollectionChanged;
            }

            collection = value as ObservableCollection<T>;
            if (collection == null)
            {
                layoutManager.Refresh();
                return;
            }

            var collectionFiltered = collection.Where(IncludePredicate).ToList();

            dontUpdateLayoutOnAdd = true;
            for (var i = 0; i < collectionFiltered.Count; i++)
            {
                Add(collectionFiltered[i], i);
            }

            dontUpdateLayoutOnAdd = false;
            collection.CollectionChanged += CollectionOnCollectionChanged;
            
            WaitOneFrameAnd(layoutManager.Refresh);
            
        }
        #endregion

        #region Private methods
        
        private new void Awake()
        {
            if (!enabled)
                return;
            
            base.Awake();
            
            var children = new List<Transform>();
            for (var i = 0; i < layoutManager.transform.childCount; i++)
            {
                children.Add(layoutManager.transform.GetChild(i));
            }
            
            children.ForEach(x=>Destroy(x.gameObject));
            
            // UIEventManager.Instance.GetLatestValue(expressionPath, this);
            WaitOneFrameAnd(layoutManager.Refresh);
            // StartCoroutine(CheckForNewData());
        }

        // IEnumerator CheckForNewData()
        // {
        //     while (gameObject)
        //     {
        //         yield return new WaitForSeconds(2f);
        //         UIEventManager.Instance.GetLatestValue(expressionPath, this);
        //         GameViewModel.Instance.TriggerChangeOnAllProperties();
        //     }
        // }
        
        void OnDisable()
        {
            if (collection != null)
                collection.CollectionChanged -= CollectionOnCollectionChanged;
        }

        void WaitOneFrameAnd(Action postAction)
        {
            StartCoroutine(_WaitProcess(postAction));
        }

        IEnumerator _WaitProcess(Action postAction)
        {
            yield return new WaitForFixedUpdate();
            postAction?.Invoke();
        }

        void Add(DataContainer data, int index)
        {
            // 1. Add new game object
            var go = Instantiate(
                itemTemplate,
                InitialPosition,
                Quaternion.identity,
                layoutManager.transform
            );
            
            go.transform.SetSiblingIndex(index);
            go.transform.localPosition = InitialPosition;

            var allBindings = go.GetComponentsInChildren<GenericDataBinding>();
            if (data == null) throw new UnityException("Data should not be null");
            listMap.Add(data.UniqueId, go);
            foreach (var binding in allBindings)
            {
                binding.BindToItem(data.UniqueId);
            }
            data.TriggerChangeOnAllProperties();
            
            if(!dontUpdateLayoutOnAdd)
                StartCoroutine(WaitAndRefresh());
        }

        void Remove(DataContainer removeItem)
        {
            if (removeItem != null && listMap.ContainsKey(removeItem.UniqueId))
            {
                var go = listMap[removeItem.UniqueId];
                
                var allBindings = go.GetComponentsInChildren<GenericDataBinding>();
                // UIEventManager.Instance.RemoveBindings(removeItem.UniqueId);
            
                Destroy(listMap[removeItem.UniqueId]);
                listMap.Remove(removeItem.UniqueId);
                StartCoroutine(WaitAndRefresh());
            }
        }

        IEnumerator WaitAndRefresh()
        {
            yield return null;
            layoutManager.Refresh();
        }

        void Clear()
        {
            foreach (var key in listMap.Keys)
            {
                DestroyImmediate(listMap[key]);
            }
            listMap.Clear();
            StartCoroutine(WaitAndRefresh());
            // layoutManager.Refresh();
            // StartCoroutine(ClearProcess());
        }

        // private IEnumerator ClearProcess()
        // {
        //     foreach (var key in listMap.Keys)
        //     {
        //         Destroy(listMap[key]);
        //     }
        //     listMap.Clear();
        //     yield return null;
        //     layoutManager.Refresh();
        // }

        void CollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    try
                    {
                        layoutManager.Refresh();

                        var shouldInclude = IncludePredicate(e.NewItems[0] as T);
                        if (!shouldInclude) break;


                        Add(e.NewItems[0] as DataContainer, e.NewStartingIndex);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("LOL");
                    }

                    break;
                }
                case NotifyCollectionChangedAction.Move:
                {
                    //throw new UnityException("'Move' command not implemented yet.");
                    var child = transform.GetChild(e.OldStartingIndex);
                    child.SetSiblingIndex(e.NewStartingIndex);
                    layoutManager.Refresh();
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    foreach (var oldItem in e.OldItems)
                    {
                        Remove(oldItem as DataContainer);    
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Replace:
                {
                    throw new UnityException("'Replace' command not implemented yet.");
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    Clear();
                    break;
                }
            }
        }
        
        #endregion
    }
}