using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BindableUI.ImageBindings
{
    /// <summary>
    /// Data binding to Image fill color boost animation.
    /// </summary>
    public class ImageColorBoostDataBinding : GenericDataBinding
    {
        [SerializeField] private float colorBoostAmount;

        private Color originalColor;
        private Color boostColor;
        
        private Image image;
        private Coroutine colorAnimation;

        private new void Awake()
        {
            image = GetComponent<Image>();
            originalColor = image.color;
            boostColor = originalColor * colorBoostAmount;
        }

        public override void OnValueChanged(object value)
        {
            if (value == null || !image) return;
            try
            {
                StartCoroutine(ColorAnimation());
            }
            catch (UnityException e)
            {
                Debug.LogWarning("Warning: " + e.Message);
            }
        }

        IEnumerator ColorAnimation()
        {
            var startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < 0.1f)
            {
                var a = Mathf.Clamp((Time.realtimeSinceStartup - startTime)*10.0f,0f,1f);
                image.color = Color.Lerp(boostColor, originalColor, a);
                yield return null;
            }
            image.color = originalColor;
        }
        
    }
}