using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BindableUI.ImageBindings
{
    /// <summary>
    /// Data binding to Image fill using min-max mapping.
    /// </summary>
    public class ImageFillDataBinding : GenericDataBinding
    {
        [SerializeField] private Vector2 minMaxMapping = new Vector2(0f,1f);
        private Image image;

        private new void Awake()
        {
            image = GetComponent<Image>();
        }

        public override void OnValueChanged(object value)
        {
            if (value == null || image == null) return;
            try
            {
                float normalizedValue;
                if (value is string)
                {
                    normalizedValue = float.Parse(value.ToString(), CultureInfo.InvariantCulture);
                }
                else if (value is float)
                {
                    normalizedValue = (float) value;
                }
                else
                {
                    throw new UnityException("Error: Cannot determine type.");
                }
                // var normalizedValue = float.Parse(value.ToString(), CultureInfo.InvariantCulture);
                // var normalizedValue = (float) value;
                var realValue = Mathf.Lerp(minMaxMapping.x, minMaxMapping.y, normalizedValue);
                image.fillAmount = realValue;
            }
            catch (UnityException e)
            {
                Debug.LogWarning("Warning: " + e.Message);
            }
        }
    }
}