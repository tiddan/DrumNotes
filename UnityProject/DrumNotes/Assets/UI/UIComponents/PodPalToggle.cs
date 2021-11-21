using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.UIComponents
{
    public class PodPalToggle : Toggle
    {
        public delegate void OnHoverChanged(bool isHover);
        public OnHoverChanged onHoverChangedEvent;
        
        // private ReusableTooltip tooltip;
        [SerializeField] private string customTogglePressSfx;
        [SerializeField] private string customToggleHoverSfx;

        private new void Start()
        {
            base.Start();
            
            onValueChanged.AddListener(isToggle =>
            {
                if (isToggle)
                {
                }
            });
            
            onHoverChangedEvent += hover =>
            {
                if (hover)
                {
                }
            };
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            onHoverChangedEvent?.Invoke(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            onHoverChangedEvent?.Invoke(false);
        }
    }
}