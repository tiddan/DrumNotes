using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BindableUI.Sliders
{
    public class SliderDataBinding : GenericDataBinding
    {
        [SerializeField] private Slider slider;

        protected override void Start()
        {
            base.Start();

            if (slider == null)
            {
                throw new UnityException("Error: Slider not assigned: " + gameObject.name);
            }
            
            slider.onValueChanged.AddListener(newValue =>
            {
                UIEventManager.Instance.UpdateBinding(this.expressionPath, Mathf.RoundToInt(newValue));
            });
            
            UIEventManager.Instance.GetLatestValue(expressionPath, this);
        }
        
        public override void OnValueChanged(object value)
        {
            if (value == null) return;
            var floatValue = float.Parse(value.ToString(), CultureInfo.InvariantCulture);
            slider.SetValueWithoutNotify(floatValue);
        }
    }
}