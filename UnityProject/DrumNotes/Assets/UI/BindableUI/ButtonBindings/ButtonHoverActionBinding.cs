using UI.UIComponents;
using UnityEngine;

namespace UI.BindableUI.ButtonBindings
{
    /// <summary>
    /// Bind hover state to a method call.
    /// </summary>
    public class ButtonHoverActionBinding : GenericDataBinding
    { 
        [SerializeField] private PodPalButton button;
        private string itemId;

        private new void Awake()
        {
            base.Awake();
            button.HoverChanged.AddListener(isHover =>
            {
                UIEventManager.Instance.ExecuteMethod(expressionPath, isHover ? itemId : null);
            });
        }

        public override void BindToItem(string id)
        {
            base.BindToItem(id);
            itemId = id;
        }

        public override void OnValueChanged(object value)
        {
        }
    }
}
