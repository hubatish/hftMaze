using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    /// <summary>
    /// Represents an x,y position in the grid
    /// Instead of floats, ints are used to store xy position, and they probably won't get much larger than the size of the grid
    /// Is essentially a Vector2, but offers clarity while reading code and replaces previous ijPos
    /// </summary>
    public class GridVector
    {
        //private static GridManager grid = GridManager.instance;

        public int x { get; set; }
        public int y { get; set; }

        public GridVector(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public GridVector(Vector2 ijVector)
        {
            this.x = (int) ijVector.x;
            this.y = (int) ijVector.y;
        }

        public static GridVector operator +(GridVector v1, GridVector v2)
        {
            return new GridVector(v1.x+v2.x,v1.y+v2.y);
        }

        public static GridVector operator -(GridVector v1, GridVector v2)
        {
            return new GridVector(v1.x - v2.x, v1.y - v2.y);
        }

        public static GridVector operator *(int i, GridVector vG)
        {
            return new GridVector(vG.x * i, vG.y * i);
        }

        public static GridVector operator *(GridVector vG, int i)
        {
            return i*vG;
        }

        //Implicitly convert a Vector2 position to a GridVector with the exact same values
        public static explicit operator Vector2(GridVector vG)
        {
            Vector2 v2 = new Vector2(vG.x,vG.y);
            return v2;
        }

        //Convert the other way - GridVector to Vector2
        public static explicit operator GridVector(Vector2 v2)
        {
            GridVector vG = new GridVector((int)v2.x,(int)v2.y);
            return vG;
        }

        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }

        public override bool Equals(object obj)
        {
            var newObj = obj as GridVector;
            if (null != newObj)
            {
                return x == newObj.x &&
                    y == newObj.y;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /*
         * These explicit conversions were to weird and should really be used as function calls from grid
         * //Explicit conversions take some "real world" position and find where that position would be in grid
         * 
         * 
        private static string gridNullMessage = "Grid is null.  Has the GridManager in this room been initialized yet?";
        public static explicit operator GridVector(Vector3 v3)
        {
            if(grid==null)
            {
                throw new Exception(gridNullMessage);
            }
            GridVector vG = grid.xyzToij(v3);
            return vG;
        }

        //
        public static explicit operator Vector3(GridVector vG)
        {
            if (grid == null)
            {
                throw new Exception(gridNullMessage);
            }
            Vector3 v3 = grid.ijToxyz(vG);
            return v3;
        }

        public static explicit operator Vector2(GridVector vG)
        {
            if (grid == null)
            {
                throw new Exception(gridNullMessage);
            }
            Vector2 v2 = grid.ijToxy(vG);
            return v2;
        }

        public static explicit operator GridVector(Vector2 v2)
        {
            if (grid == null)
            {
                throw new Exception(gridNullMessage);
            }
            GridVector vG = grid.xyToij(v2);
            return vG;
        }*/

    }
