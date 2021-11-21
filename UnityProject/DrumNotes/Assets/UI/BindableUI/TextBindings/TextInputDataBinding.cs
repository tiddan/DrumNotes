using UnityEngine.UI;

namespace UI.BindableUI.TextBindings
{
    public class TextInputDataBinding : GenericDataBinding
    {
        private InputField text;

        private new void Awake()
        {
            base.Awake();
            text = GetComponent<InputField>();
            text.onValueChanged.AddListener(value => { UIEventManager.Instance.UpdateBinding(expressionPath, value); });
        }

        public override void OnValueChanged(object value)
        {
            var textValue = value?.ToString() ?? "";
            if (text.text != textValue)
            {
                text.text = textValue;
            }
        }
    }
}