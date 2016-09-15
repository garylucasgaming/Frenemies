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



}
public static class ConstantsAndFunctions
{


}
