using System.Collections.Generic;
using UnityEngine;

namespace UI.BindableUI.CanvasGroupBindings
{
    public class CanvasGroupDataBinding : GenericDataBinding
    {
        [SerializeField] private List<CanvasGroup> canvasGroups;
        [SerializeField] private bool inverseExpression;

        public override void OnValueChanged(object value)
        {
            if (value == null) return;

            foreach (var canvasGroup in canvasGroups)
            {
                if (canvasGroup == null || canvasGroup.gameObject == null) continue;

                var finalValue = inverseExpression ? !(bool) value : (bool) value;
                
                canvasGroup.alpha = finalValue ? 1.0f : 0.0f;
                canvasGroup.interactable = finalValue;
                canvasGroup.blocksRaycasts = finalValue;
            }
        }
    }
}