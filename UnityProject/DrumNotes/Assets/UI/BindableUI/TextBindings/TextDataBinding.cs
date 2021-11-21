using UnityEngine.UI;

namespace UI.BindableUI.TextBindings
{
    public class TextDataBinding : GenericDataBinding
    {
        private Text text;

        private new void Awake()
        {
            base.Awake();
            text = GetComponent<Text>();
        }

        public override void OnValueChanged(object value)
        {
            text.text = value.ToString();
        }
    }
}