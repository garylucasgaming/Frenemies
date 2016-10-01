using UnityEngine;
using System.Collections;
namespace CommonStructs
{
    public struct int2
    {
        public int x;
        public int y;

        public int2(int X, int Y)
        {
            this.x = X;
            this.y = Y;
        }

        public static bool operator ==(int2 f, int2 s)
        {
            if (f.x == s.x && f.y == s.y)
                return true;
            else
                return false;
        }
        public static bool operator !=(int2 f, int2 s)
        {
            if (f.x == s.x && f.y == s.y)
                return false;
            else
                return true;
        }
        public override string ToString()
        {
            return "[" + x + "," + y + "]";
        }
    }

    public enum TileType
    {
        Undeclared = 0,
        Floor = 1,
        DoorFrame = 2,
        Computer = 3,
        WallSolid = 4,
        WallSingleEdge = 5,
        WallDoubleEdge = 6,
        WallEnd = 7,
        WallNub = 8,
        WallCrescent = 9,
        WallGibbous = 10,
        TableEnd = 11,
        TableMiddle = 12,
        Hall = 13

    }





}
public static class ConstantsAndFunctions
{


}
