using TMPro;
using UnityEngine;

namespace UI.BindableUI.TextBindings
{
    public class TextMeshToggleColorDataBinding : GenericDataBinding
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Color onColor;
        [SerializeField] private Color offColor;

        public override void OnValueChanged(object value)
        {
            if (value == null) return;
            text.color = (bool) value ? onColor : offColor;
        }
    }
}