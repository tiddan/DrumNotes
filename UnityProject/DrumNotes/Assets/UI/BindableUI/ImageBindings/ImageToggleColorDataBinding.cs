using UnityEngine;
using UnityEngine.UI;

namespace UI.BindableUI.ImageBindings
{
    public class ImageToggleColorDataBinding : GenericDataBinding
    {
        [SerializeField] private Image image;
        [Header("ON colors")] [SerializeField] private Color toggleOnColors;

        [Header("OFF colors")] [SerializeField]
        private Color toggleOffColors;

        private new void Awake()
        {
            image.color = toggleOnColors;
        }

        public override void OnValueChanged(object value)
        {
            if (value == null || image == null) return;

            if (expressionPath.Contains("IsSelected"))
                Debug.Log("Changed shit");

            if ((bool) value)
            {
                image.color = toggleOnColors;
            }
            else
            {
                image.color = toggleOffColors;
            }
        }
    }
}