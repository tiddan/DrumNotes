using UnityEngine;

namespace UI.UIComponents
{
    public class TooltipBinder : MonoBehaviour
    {
        [SerializeField] private PodPalButton button;
        [SerializeField] private ReusableTooltip tooltip;
        [SerializeField] private string tooltipText;

        private void Awake()
        {
            if(button==null)
                throw new UnityException("Error in TooltipBinder: button not assigned.");
            
            button.HoverChanged.AddListener(OnHoverChanged);
        }

        private void OnHoverChanged(bool isHover)
        {
            if (isHover)
            {
                tooltip.QueueNextTooltip(tooltipText);
            }
            else
            {
                tooltip.HideTooltip();
            }
        }
    }
}