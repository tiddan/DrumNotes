using UnityEngine;
using UnityEngine.UI;

namespace UI.BindableUI.ButtonBindings
{
    public class ToggleDataBinding : GenericDataBinding
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private bool invertExpression;

        private new void Awake()
        {
            base.Awake();
            
            toggle.onValueChanged.AddListener(isOn =>
            {
                UIEventManager.Instance.UpdateBinding(this.expressionPath, isOn); 
            });
        }

        public override void OnValueChanged(object value)
        {
            if (value == null || toggle == null) return;

            var finalValue = invertExpression ? !(bool) value : (bool) value;
            toggle.isOn = finalValue;
        }
    }
}