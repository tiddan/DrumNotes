using UnityEngine;
using UnityEngine.UI;

namespace UI.BindableUI.ImageBindings
{
    public class ImageSpriteDataBinding : GenericDataBinding
    {
        [SerializeField] Image image;

        public override void OnValueChanged(object value)
        {
            if(expressionPath.Contains("Captain"))
                Debug.Log("Captains log");
            
            if (value == null || image == null) return;
            try
            {
                var sprite = Resources.Load<Sprite>((string) value);
                if (sprite == null)
                    throw new UnityException($"Error in 'ImageSpriteDataBinding': Cannot find resource: '{value}'");
                image.sprite = sprite;
            }
            catch (UnityException e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}