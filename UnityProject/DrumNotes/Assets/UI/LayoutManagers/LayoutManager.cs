using UnityEngine;

namespace UI.LayoutManagers
{
    public abstract class LayoutManager : MonoBehaviour
    {
        public abstract void Refresh();

        protected virtual void Awake()
        {
            Refresh();
        }
        
    }
}