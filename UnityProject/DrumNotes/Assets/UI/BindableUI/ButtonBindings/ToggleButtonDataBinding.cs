using UnityEngine;
using UnityEngine.UI;

namespace UI.BindableUI.ButtonBindings
{
    /// <summary>
    /// Binding Button colors to on/off (toggle) state.
    /// Bind to boolean property to make this work.
    /// </summary>
    public class ToggleButtonDataBinding : GenericDataBinding
    {
        [SerializeField] private Button button;
        [Header("ON colors")] [SerializeField] private ColorBlock toggleOnColors;

        [Header("OFF colors")] [SerializeField]
        private ColorBlock toggleOffColors;

        private new void Awake()
        {
            base.Awake();
            button.colors = toggleOnColors;
        }

        public override void OnValueChanged(object value)
        {
            if (value == null || button == null) return;

            if ((bool) value)
            {
                button.colors = toggleOnColors;
            }
            else
            {
                button.colors = toggleOffColors;
            }
        }
    }
}