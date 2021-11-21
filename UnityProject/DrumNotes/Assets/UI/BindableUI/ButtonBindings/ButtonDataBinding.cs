using UnityEngine;
using UnityEngine.UI;

namespace UI.BindableUI.ButtonBindings
{
    /// <summary>
    /// In this binding the 'expressionPath' refer to a method that 
    /// accepts a string as parameter.
    /// </summary>
    public class ButtonDataBinding : GenericDataBinding
    {
        [SerializeField] private Button button;
        private string itemId;

        private new void Awake()
        {
            base.Awake();
            
            if(button == null)
                throw new UnityException($"Error in 'ButtonDataBinding'. Button is null. GameObject '{gameObject.name}'");
            
            button.onClick.AddListener(delegate { UIEventManager.Instance.ExecuteMethod(expressionPath, itemId); });
        }

        public override void BindToItem(string id)
        {
            base.BindToItem(id);
            itemId = id;
        }

        public override void OnValueChanged(object value)
        {
        }
    }
}