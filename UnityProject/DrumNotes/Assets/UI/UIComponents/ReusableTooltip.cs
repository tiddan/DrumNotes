using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UIComponents
{
    public class ReusableTooltip : MonoBehaviour
    {
        private bool visible;
        private float currentAlpha;

        private Color tooltipImageColor;
        private Color tooltipTextColor;
        private Image tooltipImage;
        private TextMeshProUGUI tooltipText;
        private RectTransform tooltipRect;

        [SerializeField] private float transitionSpeed = 20.0f;
        [SerializeField] private Vector2 offsetPosition = new Vector2(0, 0);

        private void Awake()
        {
            tooltipImage = GetComponent<Image>();
            tooltipText = GetComponentInChildren<TextMeshProUGUI>();
            tooltipRect = GetComponent<RectTransform>();
            tooltipImageColor = tooltipImage.color;
            tooltipTextColor = tooltipText.color;
        }

        private IEnumerator FadeTo(bool fadeText, float targetAlpha)
        {
            while (Math.Abs(currentAlpha - targetAlpha) > 0.01f)
            {
                currentAlpha = Mathf.Clamp(Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * transitionSpeed),0.0f,1.0f);
                if(fadeText) ChangeTextColor();
                else ChangeImageColor();
                yield return null;
            }
            currentAlpha = targetAlpha;
            if (fadeText) ChangeTextColor();
            else ChangeImageColor();
        }

        private void Update()
        {
            if (currentAlpha > 0.001f)
            {
                var pos = Input.mousePosition;
                tooltipRect.position = new Vector3(pos.x+offsetPosition.x, pos.y+offsetPosition.y, -1.0f);
            }
        }

        private void ChangeImageColor()
        {
            tooltipImageColor.a = currentAlpha;
            tooltipImage.color = tooltipImageColor;
        }

        private void ChangeTextColor()
        {
            tooltipTextColor.a = currentAlpha;
            tooltipText.color = tooltipTextColor;
        }

        IEnumerator QueueNextTooltipProcess(string newText)
        {
            yield return FadeTo(true,0.0f);
            yield return FadeTo(false, 0.0f);
            yield return new WaitForSeconds(0.4f);
            tooltipText.text = newText;
            tooltipText.ForceMeshUpdate();
            tooltipImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tooltipText.GetRenderedValues().x+20.0f);
            yield return FadeTo(false,1.0f);
            yield return FadeTo(true, 1.0f);
        }

        public void QueueNextTooltip(string nextText)
        {
            StopAllCoroutines();
            StartCoroutine(QueueNextTooltipProcess(nextText));
        }

        public void HideTooltip()
        {
            StopAllCoroutines();
            StartCoroutine(FadeTo(true,0.0f));
            StartCoroutine(FadeTo(false, 0.0f));
        }
    }
}