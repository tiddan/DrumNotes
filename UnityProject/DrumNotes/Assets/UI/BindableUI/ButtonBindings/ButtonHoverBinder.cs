using UI.UIComponents;
using UnityEngine;

namespace UI.BindableUI.ButtonBindings
{
    /// <summary>
    /// Bind hover state to a boolean property for the PodPalButton.
    /// </summary>
    public class ButtonHoverBinder : GenericDataBinding
    {
        [SerializeField] private PodPalButton button;

        // private new void Awake()
        // {
        //     button.HoverChanged.AddListener(isHover =>
        //     {
        //         UIEventManager.Instance.UpdateBinding(expressionPath, isHover);
        //     });
        // }

        protected new void Start()
        {
            base.Start();
            button.HoverChanged.AddListener(isHover =>
            {
                UIEventManager.Instance.UpdateBinding(expressionPath, isHover);
            });
        }

        public override void OnValueChanged(object value)
        {
            // Do nothing.
        }
    }
}