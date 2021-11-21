using UnityEngine;
using UnityEngine.Serialization;

namespace UI.BindableUI.GameObjectDataBindings
{
    public class GameObjectToggleDataBinding : GenericDataBinding
    {
        [FormerlySerializedAs("GO")] [SerializeField] private GameObject go;
        [FormerlySerializedAs("Inverse")] [SerializeField] private bool inverse;

        public override void OnValueChanged(object value)
        {
            if (value == null || go == null) return;
            go.SetActive(inverse ? !(bool) value : (bool) value);
        }
    }
}