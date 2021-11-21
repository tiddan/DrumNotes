using TMPro;
using UnityEngine;

namespace UI.BindableUI.TextBindings
{
    public class TextMeshBoolDataBinding : GenericDataBinding
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private string trueText;
        [SerializeField] private string falseText;
        
        public override void OnValueChanged(object value)
        {
            if (value == null) return;

            if ((bool)value)
            {
                text.text = trueText;
            }
            else
            {
                text.text = falseText;
            }
        }
    }
}