using UnityEngine;
using UnityEngine.UI;

namespace UI.BindableUI.ButtonBindings
{
    public class ToggleDataBindingMethod : GenericDataBinding
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private bool invertExpression;

        private string itemId;
        
        private new void Awake()
        {
            base.Awake();
            
            toggle.onValueChanged.AddListener(isOn =>
            {
                UIEventManager.Instance.ExecuteMethod(expressionPath, itemId);
            });
        }
        
        public override void BindToItem(string id)
        {
            base.BindToItem(id);
            itemId = id;
        }

        public override void OnValueChanged(object value)
        {
            if (value == null || toggle == null) return;

            var finalValue = invertExpression ? !(bool) value : (bool) value;
            // toggle.isOn = finalValue;
            toggle.SetIsOnWithoutNotify(finalValue);
        }
    }
}