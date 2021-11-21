using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BindableUI.ImageBindings
{
    public class ImageOpacityDataBinding : GenericDataBinding
    {
        private Image image;
        [SerializeField] private Color color;

        private new void Awake()
        {
            image = GetComponent<Image>();
        }

        public override void OnValueChanged(object value)
        {
            if (value == null || image == null) return;
            try
            {
                image.color = new Color(color.r, color.g, color.b, float.Parse(value.ToString(), CultureInfo.InvariantCulture));
            }
            catch (UnityException e)
            {
                Debug.LogWarning("Warning: " + e.Message);
            }
        }
    }
}