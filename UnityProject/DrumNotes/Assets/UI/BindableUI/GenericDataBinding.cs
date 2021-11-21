using UnityEngine;

namespace UI.BindableUI
{
    public abstract class GenericDataBinding : MonoBehaviour
    {
        [SerializeField] protected string expressionPath;

        public abstract void OnValueChanged(object value);

        public virtual void BindToItem(string itemId)
        {
            //IsListItem = true;
            UIEventManager.Instance.Unsubscribe(expressionPath, this);
            expressionPath = expressionPath.Replace("$DataRow", itemId);
            UIEventManager.Instance.Subscribe(expressionPath, this);
        }
        
        ~GenericDataBinding()
        {
             // UIEventManager.Instance.RemoveBindings(UniqueId);
        }

        protected virtual void Awake()
        {
            if (!gameObject.activeSelf) return;
            
            //UIEventManager.Instance.GetLatestValue(expressionPath, this);
            // UIEventManager.Instance.Subscribe(expressionPath, this);
        }

        protected virtual void Start()
        {
            if (!gameObject.activeSelf) return;

            UIEventManager.Instance.Subscribe(expressionPath, this);
        }
    }
}