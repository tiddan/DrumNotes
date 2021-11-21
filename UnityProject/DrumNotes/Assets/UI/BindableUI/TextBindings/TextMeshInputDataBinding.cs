using TMPro;
using UnityEngine;

namespace UI.BindableUI.TextBindings
{
    public class TextMeshInputDataBinding : GenericDataBinding
    {
        [SerializeField] private TMP_InputField inputField;

        protected override void Start()
        {
            base.Start();

            if (inputField == null)
            {
                throw new UnityException("Error: TMPInputField not assigned: " + gameObject.name);
                return;
            }
            
            inputField.onValueChanged.AddListener(newValue =>
            {
                UIEventManager.Instance.UpdateBinding(this.expressionPath, newValue);
            });
            
            UIEventManager.Instance.GetLatestValue(expressionPath, this);
        }
        
        public override void OnValueChanged(object value)
        {
            if (value == null) return;
            // inputField.text = value.ToString();
            inputField.SetTextWithoutNotify(value.ToString());
        }
    }
}