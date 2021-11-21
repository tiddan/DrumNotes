using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.UIComponents
{
    public class UnityBoolEvent : UnityEvent<bool>
    {
    }

    public class PodPalButton : Button
    {
        public UnityBoolEvent HoverChanged = new UnityBoolEvent();
        // private ReusableTooltip tooltip;
        [SerializeField] private string customButtonPressSfx;
        [SerializeField] private string customButtonHoverSfx;
        [SerializeField] private bool disableAfterClick = true;

        private new void Start()
        {
            base.Start();
            // tooltip = GameObject.Find("Canvas").GetComponentInChildren<ReusableTooltip>();
            onClick.AddListener(() =>
            {
                StopAllCoroutines();
                if(disableAfterClick)
                    StartCoroutine(DisableForShortTime());
            });
            
            HoverChanged.AddListener(isHover =>
            {
                Debug.Log("Button hover");
            });
        }

        IEnumerator DisableForShortTime()
        {
            this.interactable = false;
            yield return new WaitForSecondsRealtime(1f);
            this.interactable = true;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            HoverChanged.Invoke(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            HoverChanged.Invoke(false);
        }
    }
}