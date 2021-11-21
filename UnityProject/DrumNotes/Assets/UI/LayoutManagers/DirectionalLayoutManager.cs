using PodPalGames.Common;
using UnityEngine;

namespace UI.LayoutManagers
{
    public class DirectionalLayoutManager : LayoutManager
    {
        [SerializeField] private bool isVertical;
        [SerializeField] private int defaultSize;
        [SerializeField] private int marginTop;
        [SerializeField] private int marginBottom;
        [SerializeField] private int marginLeft;
        [SerializeField] private int marginRight;
        [SerializeField] private int spacing;
        public override void Refresh()
        {
            if (isVertical)
            {
                UpdateVertically();
                
                    
                //GetComponent< RectTransform >( ).SetSizeWithCurrentAnchors( RectTranform.Axis.Vertical, myHeight); worked for me. For width use: RectTransform.Axis.Horizontal.
            }
            else
            {
                UpdateHorizontally();
            }
        }

        private void UpdateVertically()
        {
            var y = marginTop;
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var childRect = child.GetComponent<RectTransform>();
                var height = Mathf.CeilToInt(childRect.sizeDelta.y);
                if (height == 0)
                    height = defaultSize;

                childRect.pivot = new Vector2(0, 1);
                childRect.anchoredPosition = new Vector2(0, -y);
                childRect.anchorMin = new Vector2(0, 1);
                childRect.anchorMax = new Vector2(1, 1);
                childRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                childRect.SetLeft(marginLeft);
                childRect.SetRight(marginRight);
               
                y = y + (height + spacing);
            }
            y += marginBottom;
            
            var rect = GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
            
        }
        
        private void UpdateHorizontally()
        {
            var x = marginLeft;
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var childRect = child.GetComponent<RectTransform>();
                var width = Mathf.CeilToInt(childRect.sizeDelta.x);
                if (width == 0)
                    width = defaultSize;

                childRect.pivot = new Vector2(0, 1);
                childRect.anchoredPosition = new Vector2(x, 0);
                childRect.anchorMin = new Vector2(0, 0);
                childRect.anchorMax = new Vector2(0, 1);
                childRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                childRect.SetTop(marginTop);
                childRect.SetBottom(marginBottom);
               
                x = x + (width + spacing);
            }
            x += marginRight;
            var rect = GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
        }
        
    }
}