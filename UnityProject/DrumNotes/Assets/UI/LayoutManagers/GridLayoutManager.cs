using System.Collections.Generic;
using UnityEngine;

namespace UI.LayoutManagers
{
    public class GridLayoutManager : LayoutManager
    {
        /* Fields and props */
        
        #region Serialize fields
        [SerializeField] private Vector2 tileSize;
        [SerializeField] private int columnCount;
        [SerializeField] private int marginTop;
        [SerializeField] private int marginBottom;
        [SerializeField] private int marginLeft;
        [SerializeField] private int marginRight;
        [SerializeField] private int spacing;
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private int spawnCount;
        #endregion
        
        /* Methods */
        
        #region Public methods
        public override void Refresh()
        {
            var currentColumn = 0;
            var currentRow = 0;
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                
                var childRect = child.GetComponent<RectTransform>();
                childRect.pivot = new Vector2(0, 1);
                
                var x = marginLeft + (currentColumn * (tileSize.x+spacing));
                var y = marginTop + (currentRow * -(tileSize.y+spacing));
                childRect.anchoredPosition = new Vector2(x,y);
                
                childRect.anchorMin = new Vector2(0, 1);
                childRect.anchorMax = new Vector2(0, 1);
                childRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,tileSize.x);
                childRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,tileSize.y);

                currentColumn++;
                if (currentColumn >= columnCount)
                {
                    currentColumn = 0;
                    currentRow++;
                }
            }
        }

        public void BuildTiles()
        {
            var listOfChildren = new List<Transform>();
            for (var i = 0; i < transform.childCount; i++)
            {
                listOfChildren.Add(transform.GetChild(i));
            }
            listOfChildren.ForEach(x=>DestroyImmediate(x.gameObject));

            for (var i = 0; i < spawnCount; i++)
            {
                Instantiate(tilePrefab, transform);
            }
            Refresh();
        }

        #endregion

        #region Private methods

        void Start()
        {
            Refresh();
        }

        #endregion

        public void Setup(int columns, int rows, int size)
        {
            columnCount = columns;
            tileSize = new Vector2(size,size);
            spawnCount = columns * rows;
            BuildTiles();
        }
    }
}