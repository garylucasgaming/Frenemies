using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonStructs;
#if UNITY_EDITOR
using UnityEditor;
#endif

public struct d_DungeonCoordinates
{
    public int2 upperLeft;
    public int2 lowerRight;
}
public class d_Room
{
    public d_DungeonCoordinates coords;
    public d_Room parent;
    public d_Room[] children;
    public int subdivisionLevel;
    public bool roomDrawn;

    public d_Room Init(d_Room CreateParent)
    {
        parent = CreateParent;
        if (parent.children == null)
        {
            parent.children = new d_Room[2];
        }
        if (parent.children[0] == null)
            parent.children[0] = this;
        else if (parent.children[1] == null)
            parent.children[1] = this;
        subdivisionLevel = parent.subdivisionLevel + 1;
        roomDrawn = false;
        return this;
    }
    public d_Room Init()
    {
        subdivisionLevel = 0;
        roomDrawn = false;
        return this;
    }

}
public class d_MapGenerator : MonoBehaviour
{
    public bool[,] tiles;
    public int MapXSize;
    public int MapYSize;

    public int Subdivisions;
    public Vector2 SubdivisionXRange;
    public Vector2 SubdivisionYRange;

    public Vector2 RoomXRange;
    public Vector2 RoomYRange;

    public int EliminateRoomsSmallerOnX;
    public int EliminateRoomsSmallerOnY;
    void Start()
    {
        StartCoroutine(GenerateMap(MapXSize, MapYSize, Subdivisions));
    }
    IEnumerator GenerateMap(int xSizeOfMap, int ySizeOfMap, int deepestDivisionLevel = 3)
    {
        bool horizontalSubdivision;
        tiles = new bool[xSizeOfMap, ySizeOfMap];
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                tiles[x, y] = false;
            }
        }

        List<d_Room> OpenRooms = new List<d_Room>();

        OpenRooms.Add(new d_Room().Init());
        OpenRooms[0].coords.upperLeft = new int2(0, ySizeOfMap - 1);

        OpenRooms[0].coords.lowerRight = new int2(xSizeOfMap - 1, 0);
        List<d_Room> ClosedRooms = new List<d_Room>();


        while (true)
        {
            if (OpenRooms.Count > 0)
            {
                if (OpenRooms[0].subdivisionLevel <= deepestDivisionLevel)
                {
                    d_Room[] children = new d_Room[2];
                    children[0] = new d_Room().Init(OpenRooms[0]);
                    children[1] = new d_Room().Init(OpenRooms[0]);

                    children[0].coords.upperLeft = OpenRooms[0].coords.upperLeft;
                    children[1].coords.lowerRight = OpenRooms[0].coords.lowerRight;

                    int xSizeOfRoom = OpenRooms[0].coords.lowerRight.x - OpenRooms[0].coords.upperLeft.x;
                    int ySizeOfRoom = OpenRooms[0].coords.upperLeft.y - OpenRooms[0].coords.lowerRight.y;

                    horizontalSubdivision = xSizeOfRoom > ySizeOfRoom ? true : false;


                    if (horizontalSubdivision)
                    {
                        int xDiv = OpenRooms[0].coords.upperLeft.x + (int)((OpenRooms[0].coords.lowerRight.x - OpenRooms[0].coords.upperLeft.x) * Random.Range(SubdivisionXRange.x, SubdivisionXRange.y));
                        children[0].coords.lowerRight.x = xDiv;
                        children[0].coords.lowerRight.y = OpenRooms[0].coords.lowerRight.y;
                        children[1].coords.upperLeft.x = xDiv;
                        children[1].coords.upperLeft.y = OpenRooms[0].coords.upperLeft.y;
                    }
                    else
                    {
                        int yDiv = OpenRooms[0].coords.lowerRight.y + (int)((OpenRooms[0].coords.upperLeft.y - OpenRooms[0].coords.lowerRight.y) * Random.Range(SubdivisionYRange.x, SubdivisionYRange.y));
                        children[0].coords.lowerRight.y = yDiv;
                        children[0].coords.lowerRight.x = OpenRooms[0].coords.lowerRight.x;
                        children[1].coords.upperLeft.y = yDiv;
                        children[1].coords.upperLeft.x = OpenRooms[0].coords.upperLeft.x;
                    }
                    OpenRooms.AddRange(children);
                }
                ClosedRooms.Add(OpenRooms[0]);
                OpenRooms.RemoveAt(0);
            }
            else
            {
                break;
            }
            yield return null;
        }

        OpenRooms.AddRange(ClosedRooms);
        ClosedRooms.Clear();
        while (true)
        {
            if (OpenRooms.Count > 0)
            {
                if (OpenRooms[0].children == null)
                {
                    int xSize = OpenRooms[0].coords.lowerRight.x - OpenRooms[0].coords.upperLeft.x;
                    int NewxSize = (int)(xSize * Random.Range(RoomXRange.x, RoomXRange.y));
                    int ySize = OpenRooms[0].coords.upperLeft.y - OpenRooms[0].coords.lowerRight.y;
                    int NewySize = (int)(ySize * Random.Range(RoomYRange.x, RoomYRange.y));


                    OpenRooms[0].coords.upperLeft.x += (int)((xSize - NewxSize) / 2.0f);
                    OpenRooms[0].coords.lowerRight.x -= (int)((xSize - NewxSize) / 2.0f);

                    OpenRooms[0].coords.upperLeft.y -= (int)((ySize - NewySize) / 2.0f);
                    OpenRooms[0].coords.lowerRight.y += (int)((ySize - NewySize) / 2.0f);

                    if (NewxSize < EliminateRoomsSmallerOnX || NewySize < EliminateRoomsSmallerOnY)
                    {
                    }
                    else
                    {
                        bool addToClosed = true;
                        List<int2> tilesToTurn = new List<int2>();
                        for (int x = OpenRooms[0].coords.upperLeft.x; x < OpenRooms[0].coords.lowerRight.x; x++)
                        {
                            for (int y = OpenRooms[0].coords.lowerRight.y; y < OpenRooms[0].coords.upperLeft.y; y++)
                            {
                                if (x == 0 || x == xSizeOfMap - 1 || y == 0 || y == ySizeOfMap - 1)
                                {
                                    addToClosed = false;
                                }
                                else
                                {
                                    tilesToTurn.Add(new int2(x,y));
                                }
                            }
                        }
                        if(addToClosed)
                        {
                            OpenRooms[0].roomDrawn = true;
                            ClosedRooms.Add(OpenRooms[0]);
                            for(int i = 0; i < tilesToTurn.Count; i++)
                            {
                                tiles[tilesToTurn[i].x,tilesToTurn[i].y] = true;
                            }
                        }
                    }
                }
                OpenRooms.RemoveAt(0);
            }
            else
            {
                break;
            }
            yield return null;
        }
        OpenRooms.AddRange(ClosedRooms);
        ClosedRooms.Clear();
        
        //while (true)
        //{
        //    if (OpenRooms.Count > 0)
        //    {
        //        d_Room child1 = OpenRooms[0].parent.children[0];
        //        d_Room child2 = OpenRooms[0].parent.children[1];
        //        if (OpenRooms.Contains(child1) && OpenRooms.Contains(child2))
        //        {
        //            int2 child1Center = new int2(child1.coords.upperLeft.x + (int)((child1.coords.lowerRight.x - child1.coords.upperLeft.x) / 2.0f), child1.coords.lowerRight.y + (int)((child1.coords.upperLeft.y - child1.coords.lowerRight.y) / 2.0f));
        //
        //            int2 child2Center = new int2(child2.coords.upperLeft.x + (int)((child2.coords.lowerRight.x - child2.coords.upperLeft.x) / 2.0f), child2.coords.lowerRight.y + (int)((child2.coords.upperLeft.y - child2.coords.lowerRight.y) / 2.0f));
        //            Debug.Log("c1: " + child1Center + " c2: " + child2Center);
        //
        //            int OffsetX = child1Center.x - child2Center.x;
        //
        //            int OffsetY = child1Center.y - child2Center.y;
        //            Debug.Log(OffsetX);
        //            for (int x = 1; x < Mathf.Abs(OffsetX); x++)
        //            {
        //                tiles[(x * (int)-Mathf.Sign(OffsetX)) + child1Center.x, child1Center.y] = true;
        //
        //            }
        //            for (int y = 1; y < Mathf.Abs(OffsetY); y++)
        //            {
        //                tiles[child1Center.x + OffsetX, (y * (int)-Mathf.Sign(OffsetY)) + child1Center.y] = true;
        //            }
        //            ClosedRooms.Add(child1);
        //            ClosedRooms.Add(child2);
        //        }
        //        else
        //        if (!ClosedRooms.Contains(OpenRooms[0]))
        //        {
        //            ClosedRooms.Add(OpenRooms[0]);
        //        }
        //        OpenRooms.Remove(child1);
        //        OpenRooms.Remove(child2);
        //
        //    }
        //    else
        //    {
        //        break;
        //    }
        //    yield return null;
        //}
        List<d_Room> hallThing = new List<d_Room>();
        //pick highest parent
        d_Room parent = OpenRooms[0];
        while (true)
        {
            if (parent.parent == null)
                break;
            else
                parent = parent.parent;
        }

        //add all children that don't have children
        hallThing.AddRange(parent.children);
        int c = 0;
        while (true)
        {
            if (c < hallThing.Count)
            {
                if (hallThing[c].children != null)
                {
                    if (!hallThing.Contains(hallThing[c].children[0]))
                        hallThing.Add(hallThing[c].children[0]);
                    if (!hallThing.Contains(hallThing[c].children[1]))
                        hallThing.Add(hallThing[c].children[1]);
                }
                c++;
            }
            else
                break;

            yield return null;
        }


        //pick a child with no children
        while (true)
        {

            break;
            yield return null;
        }




        Debug.Log("Done");


    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (tiles != null)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    Gizmos.color = tiles[x, y] == true ? Color.blue : Color.red;
                    Gizmos.DrawCube(new Vector2(x, y), Vector3.one * 0.5f);
                }
            }
        }
    }

#endif
}
