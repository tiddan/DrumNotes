using TMPro;
using UnityEngine;

namespace UI.BindableUI.TextBindings
{
    public class TextMeshDataBinding : GenericDataBinding
    {
        [SerializeField] private TextMeshProUGUI text;
        
        public override void OnValueChanged(object value)
        {
            if (value == null) return;

            // if (text == null)
            // {
            //     // throw new UnityException("Error: Text field is not set.");
            //     Debug.LogError("Text field is not set");
            //     return;
            // }

            if (text == null)
            {
                Debug.Log("Error on GameObject: " + this.gameObject.name);
                Debug.Log("LOL");
            }

            text.text = value.ToString();
        }
    }
}