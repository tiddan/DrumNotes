using System.Collections.Generic;
using UnityEngine;

namespace UI.LayoutManagers
{
    public class AbilityLayoutManager : LayoutManager
    {
        [SerializeField] private int defaultSize;
        [SerializeField] private Transform[] locations;
        
        public override void Refresh()
        {
            UpdateAllItems();
        }

        void UpdateAllItems()
        {
            var children = new List<Transform>();
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                children.Add(child);
            }
            
            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                // var childRect = child.GetComponent<RectTransform>();
                
                var transformToMoveTo = locations[i];
                var oldParent = child.parent;
                child.SetParent(transformToMoveTo,false);
                child.localPosition = Vector3.zero;
                child.SetParent(oldParent,true);
            }
        }
    }
}