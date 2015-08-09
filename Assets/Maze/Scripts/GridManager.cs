using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

    public class GridManager : MonoBehaviour
    {
        //lots of grid variables

        public static GridManager Instance;

        //these are defined in inspector
        public int numRows = 10;
        public int numColumns = 10;
        public Transform baseSize;
		public float xBuffer = 1.15f;
		public float yBuffer = 1.05f;

        //just for code
        [HideInInspector]
        public float xGridSize;  //the size of each individual grid square, based off baseSize Transform
        [HideInInspector]
        public float yGridSize;
        [HideInInspector]
        public Vector3 center; //the center of the transform grid is attached
        [HideInInspector]
        public Vector3 ext;  //extents of transform
        [HideInInspector]
        private float xOffset;  //the leftmost position of a block - ie, out of 10 blocks, the 1st block will be at this position
        [HideInInspector]
        private float yOffset;  //the lowest position of a block

        //to grab four edges:
        //whoever wants to do this should just convert ij positions in grid to Vector3 positions


        public void Awake()
        {
            Instance = this;

            // Start with size of block, determine size of grid
			if (baseSize == null) {
				Debug.Log("Please assign a transform to grid's base size.");
            }
            BoxCollider2D box = baseSize.GetComponent<BoxCollider2D>();
            xGridSize = box.size.x*xBuffer;
            yGridSize = box.size.y*yBuffer;

            //Doesn't really matter if collider or collider2D used
            //Just select a bounds
            Bounds bounds;
            if(GetComponent<Collider>()!=null)
            {
                bounds = GetComponent<Collider>().bounds;
            }
            else if (GetComponent<Collider2D>()!=null)
            {
                bounds = GetComponent<Collider2D>().bounds;
            }
            else
            {
                Debug.LogError("ERROR!  no collider attached to grid!");
                bounds = new Bounds();
            }

            center = bounds.center;
            ext = new Vector3(0, 0, 0);

            float w; //width of entire grid
            float h;
            w = xGridSize * numColumns;
            h = yGridSize * numRows;
            ext.x = w / 2f;
            ext.y = h / 2f;

            //position of first block is at xOffset, yOffset
            xOffset = (center.x - ext.x + xGridSize / 2);
            yOffset = (center.y - ext.y + yGridSize / 2);

            //resize grid so it matches new ext values
            Vector3 newScale = new Vector3(ext.x / bounds.extents.x, ext.y / bounds.extents.y, 1f);

            float extraScale = 1.1f;

            transform.localScale = new Vector3(newScale.x * transform.localScale.x,
                                            newScale.y * transform.localScale.y,
                                            newScale.z * transform.localScale.z);
            transform.localScale *= extraScale;
        }

        public void Update()
        {
            //drawBounds2();
            DrawGridCells();
        }

        //just for debugging and helping me figure out where the corners of my rectangle were
        public void drawBounds2(bool verbose)
        {
            Vector3 c = center;
            Vector3 e = ext;
            //Get all four corners of the front face
            Vector3 pt1 = new Vector3(c.x - e.x, c.y + e.y, c.z);
            Vector3 pt2 = new Vector3(c.x - e.x, c.y - e.y, c.z);
            Vector3 pt3 = new Vector3(c.x + e.x, c.y + e.y, c.z);
            Vector3 pt4 = new Vector3(c.x + e.x, c.y - e.y, c.z);
            //Draw them to the screen as a rectangle
            Color lineColor = Color.green;
            Debug.DrawLine(pt1, pt2, lineColor);
            Debug.DrawLine(pt1, pt3, lineColor);
            Debug.DrawLine(pt2, pt4, lineColor);
            Debug.DrawLine(pt3, pt4, lineColor);
            if (verbose)
            {
                //Print them to the screen individually
                Debug.Log("top left: " + pt1);
                Debug.Log("bottom left: " + pt2);
                Debug.Log("top right: " + pt3);
                Debug.Log("bottom left: " + pt4);
            }
        }

                //for debugging, outline center of each cell of the grid
        public void DrawGridCells()
        {
            bool firstJ = true, firstI = true;
            for (float i = 0; i <= numColumns;i+=1 )//(float i = center.x - ext.x; i <= center.x + ext.x; i += xGridSize)
            {
                float x = (i * xGridSize) + center.x - ext.x;
                firstJ = true;
                for (float j = 0; j <= numRows; j++)//= center.y - ext.y; j <= center.y + ext.y; j += yGridSize)
                {
                    float y = (j * yGridSize) + center.y - ext.y;
                    //Draw line from current position to previous one
                    if (!firstJ)
                        Debug.DrawLine(new Vector3(x, y - yGridSize), new Vector3(x, y));
                    firstJ = false;
                    if (!firstI)
                        Debug.DrawLine(new Vector3(x - xGridSize, y), new Vector3(x, y));
                }
                firstI = false;
            }
        }

        //snap transform to nearest spot on grid
        public void SnapToGrid(Transform t)
        {
            GridVector ijPos = xyzToij(t.position);
            float zPos = t.position.z;
            Vector3 newPos = ijToxyz(ijPos);
            newPos.z = zPos;
            t.position = newPos;
        }

        //where i is x position, j is y position

        //given an xyz, return what that would be in i, j, from grid
        public GridVector xyzToij(Vector3 xyzPos)
        {
            /*GridVector ijPos = new GridVector();
            ijPos.x = (int) Mathf.Round((xyzPos.x - xOffset) / xGridSize);
            ijPos.y = (int) Mathf.Round((xyzPos.y - yOffset) / yGridSize);
            return ijPos;*/
            return xyToij((Vector2)xyzPos);
        }

        //given an xy, return what that would be in i, j, from grid
        public GridVector xyToij(Vector2 xyPos)
        {
            GridVector ijPos = new GridVector();
            ijPos.x = (int)Mathf.Round((xyPos.x - xOffset) / xGridSize);
            ijPos.y = (int)Mathf.Round((xyPos.y - yOffset) / yGridSize);
            return ijPos;
        }

        //given an ij in grid, return that block's real x,y,z location
        public Vector3 ijToxyz(GridVector ijPos)
        {
            return new Vector3((float)xOffset + ijPos.x * xGridSize, (float)yOffset + ijPos.y * yGridSize, transform.position.z);
        }

        public Vector2 ijToxy(GridVector ijPos)
        {
            return new Vector2((float)xOffset + ijPos.x * xGridSize, (float)yOffset + ijPos.y * yGridSize);
        }

        //get all ijPositions so can iterate over them
        public IEnumerable<GridVector> allijPositions
        {
            get
            {
                List<GridVector> allijs = new List<GridVector>();
                for (int i = 0; i < numColumns; i++)
                {
                    for (int j = 0; j < numRows; j++)
                    {
                        allijs.Add(new GridVector(i, j));
                    }
                }
                return allijs;
            }
        }

        //All functions below (except for GetHitsAtij) don't really use the grid and are rather a convenience method for checking what objects are at positions
        
        //get first tag at position
        public string GetTagAtPos(Vector3 pos, out Collider2D col)
        {
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, ~(1 << 8));
            if (hit != null)
            {
                if (hit.collider != null)
                {
                    col = hit.collider;
                    return hit.collider.tag;
                }
            }
            col = null;
            return "";
        }
        
        public bool IsTagAtPos(Vector3 pos, string tag)
        {
            IEnumerable<Collider2D> cols;
            return IsTagAtPos(pos, tag, out cols);
        }

        //return whether a specific tag is at a position, and returns all colliders with that tag as an out variable 
        public bool IsTagAtPos(Vector3 pos, string tag, out IEnumerable<Collider2D> cols)
        {
            IEnumerable<RaycastHit2D> hits = GetHitsAtPos(pos);
            cols = hits.Select(hit => hit.collider).Where(collider => collider.tag.Equals(tag));
            return cols.Any();
        }


        public IEnumerable<RaycastHit2D> GetHitsAtPos(Vector3 xyzPos)
        {
            return GetHitsAtPos((Vector2)xyzPos);
        }

        public IEnumerable<RaycastHit2D> GetHitsAtij(GridVector ijPos)
        {
            return GetHitsAtPos(ijToxy(ijPos));
        }

        public IEnumerable<RaycastHit2D> GetHitsAtPos(Vector2 xyPos)
        {
            List<RaycastHit2D> hits = new List<RaycastHit2D>(Physics2D.RaycastAll(xyPos, Vector2.zero));
            return hits;
        }
    }
