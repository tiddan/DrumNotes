using System.Collections.Generic;

namespace UI.BindableUI
{
    public class BindingData
    {
        public List<GenericDataBinding> Bindings { get; } = new List<GenericDataBinding>();
        public object LastValue;
    }
}