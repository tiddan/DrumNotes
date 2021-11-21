using UI.UIComponents;
using UnityEngine;

namespace UI.BindableUI.ButtonBindings
{
    /// <summary>
    /// Bind to bool to set button enabled/disabled.
    /// </summary>
    public class ButtonEnabledDataBinding : GenericDataBinding
    {
        [SerializeField] private PodPalButton button;

        private new void Awake()
        {
            base.Awake();

            if (button == null)
            {
                throw new UnityException(
                    $"Error in 'ButtonDataBinding'. Button is null. GameObject '{gameObject.name}'");
            }
        }

        public override void OnValueChanged(object value)
        {
            var newValue = value != null && (bool) value;
            button.interactable = newValue;
        }
    }
}