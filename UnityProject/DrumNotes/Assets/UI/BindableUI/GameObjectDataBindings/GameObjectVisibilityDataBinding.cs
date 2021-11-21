using System.Collections.Generic;
using UnityEngine;

namespace UI.BindableUI.GameObjectDataBindings
{
    public class GameObjectVisibilityDataBinding : GenericDataBinding
    {
        [SerializeField] private List<GameObject> panels;
        [SerializeField] private bool inverseExpression;

        public override void OnValueChanged(object value)
        {
            if (value == null) return;
            foreach (var panel in panels)
            {
                if (panel == null || !panel.gameObject.activeSelf) return;
                panel.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2((bool) value ? 20.0f : 10000.0f, 45.0f);
            }
        }
    }
}