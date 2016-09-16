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

    public int CorridorSegments;
    public int MinimumChokePoints;
    public int HallWideness = 1;

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

        Debug.Log("Map Generator - Start Rooms");
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
        Debug.Log("Map Generator - End Rooms");

        Debug.Log("Map Generator - Start Halls");
        List<d_Room> areasToConnect = new List<d_Room>();
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
        MinimumChokePoints = MinimumChokePoints > 1 ? MinimumChokePoints : 1;
        for (int i = 0; i < MinimumChokePoints; i++)
        {
            areasToConnect.Add(parent);
        }
        int c = 0;
        while (true)
        {
            if (c < areasToConnect.Count)
            {
                if (areasToConnect[c].children != null)
                {
                    if (areasToConnect[c].children[0].children != null)
                        if (!areasToConnect.Contains(areasToConnect[c].children[0]))
                            areasToConnect.Add(areasToConnect[c].children[0]);

                    if (areasToConnect[c].children[1].children != null)
                        if (!areasToConnect.Contains(areasToConnect[c].children[1]))
                            areasToConnect.Add(areasToConnect[c].children[1]);
                }
                c++;
            }
            else
                break;

            yield return null;
        }

        while (true)
        {

            //for each child with children, pick 2 children that DON'T have a child beneath it and connect them with a hall.
            if (areasToConnect.Count > 0)
            {
                List<d_Room> child0Choices = new List<d_Room>();
                child0Choices.Add(areasToConnect[0].children[0]);
                List<d_Room> child1Choices = new List<d_Room>();
                child1Choices.Add(areasToConnect[0].children[1]);



                int choiceIndex = 0;
                while (child0Choices.Count > choiceIndex)
                {
                    if(child0Choices[choiceIndex].roomDrawn)
                    {
                        choiceIndex++;
                        continue;
                    }
                    else if(child0Choices[choiceIndex].children != null)
                    {
                        if(child0Choices[choiceIndex].children[0] != null)
                        {
                            if(!child0Choices.Contains(child0Choices[choiceIndex].children[0]))
                            {
                                child0Choices.Add(child0Choices[choiceIndex].children[0]);
                            }
                        }
                        if (child0Choices[choiceIndex].children[1] != null)
                        {
                            if (!child0Choices.Contains(child0Choices[choiceIndex].children[1]))
                            {
                                child0Choices.Add(child0Choices[choiceIndex].children[1]);
                            }
                        }

                    }

                    child0Choices.RemoveAt(choiceIndex);

                    yield return null;
                }

                choiceIndex = 0;



                while (child1Choices.Count > choiceIndex)
                {
                    if (child1Choices[choiceIndex].roomDrawn)
                    {
                        choiceIndex++;
                        continue;
                    }
                    else if (child1Choices[choiceIndex].children != null)
                    {
                        if (child1Choices[choiceIndex].children[0] != null)
                        {
                            if (!child1Choices.Contains(child1Choices[choiceIndex].children[0]))
                            {
                                child1Choices.Add(child1Choices[choiceIndex].children[0]);
                            }
                        }
                        if (child1Choices[choiceIndex].children[1] != null)
                        {
                            if (!child1Choices.Contains(child1Choices[choiceIndex].children[1]))
                            {
                                child1Choices.Add(child1Choices[choiceIndex].children[1]);
                            }
                        }

                    }

                    child1Choices.RemoveAt(choiceIndex);

                    yield return null;
                }

                CorridorSegments = CorridorSegments < 1 ? 1 : CorridorSegments;
                if (child0Choices.Count > 0 && child1Choices.Count > 0)
                {
                    d_Room child0 = child0Choices[Random.Range(0, child0Choices.Count)];
                    d_Room child1 = child1Choices[Random.Range(0, child1Choices.Count)];

                    int2 child0Center = new int2(child0.coords.upperLeft.x + (int)((child0.coords.lowerRight.x - child0.coords.upperLeft.x) / 2.0f), child0.coords.lowerRight.y + (int)((child0.coords.upperLeft.y - child0.coords.lowerRight.y) / 2.0f));
                    int2 child1Center = new int2(child1.coords.upperLeft.x + (int)((child1.coords.lowerRight.x - child1.coords.upperLeft.x) / 2.0f), child1.coords.lowerRight.y + (int)((child1.coords.upperLeft.y - child1.coords.lowerRight.y) / 2.0f));

                    int OffsetX = child1Center.x - child0Center.x;
                                                 
                    int OffsetY = child1Center.y - child0Center.y;

                    bool UseXSegmentFirst = Mathf.Abs(OffsetX) > Mathf.Abs(OffsetY) ? true : false;
                    
                    if (UseXSegmentFirst)
                    {
                        int loops = 0;
                        int SegX = OffsetX / CorridorSegments;
                        int SegY = OffsetY / CorridorSegments;
                        while(true)
                        {
                            bool writing = false;
                            if (loops < CorridorSegments)
                            {
                                for (int x = 0; x < Mathf.Abs(SegX); x++)
                                {
                                    tiles[child0Center.x + (SegX * loops) + ((int)Mathf.Sign(OffsetX) * x), child0Center.y + (SegY * loops)] = true;
                                    for (int i = 1; i < HallWideness; i++)
                                    {
                                        if (child0Center.y + (SegY * (loops)) - i > 0 && child0Center.y + (SegY * (loops)) < ySizeOfMap - 1 && child0Center.x + (SegX * (loops)) - i > 0 && child0Center.x + (SegX * (loops)) < xSizeOfMap - 1)
                                        {
                                        tiles[child0Center.x + (SegX * loops) + ((int)Mathf.Sign(OffsetX) * x) - i, child0Center.y + (SegY * (loops)) - i] = true;
                                        }
                                    }
                                }
                                writing = true;
                            }
                            if (loops < CorridorSegments)
                            {
                                for (int y = 0; y < Mathf.Abs(SegY); y++)
                                {
                                    tiles[child0Center.x + (SegX * (loops + 1)), child0Center.y + (SegY * loops) + ((int)Mathf.Sign(OffsetY) * y)] = true; 
                                    for (int i = 1; i < HallWideness; i++)
                                    {
                                        if (child0Center.y + (SegY * (loops)) - i > 0 && child0Center.y + (SegY * (loops)) < ySizeOfMap - 1 && child0Center.x + (SegX * (loops)) - i > 0 && child0Center.x + (SegX * (loops)) < xSizeOfMap - 1)
                                        {
                                            tiles[child0Center.x + (SegX * (loops + 1)) - i, child0Center.y + (SegY * loops) + ((int)Mathf.Sign(OffsetY) * y) - i] = true;
                                        }
                                    }
                                }
                                writing = true;
                            }
                            loops++;

                            if (!writing)
                                break;

                            yield return null;
                        }
                    }
                    else
                    {
                        int loops = 0;
                        int SegX = OffsetX / CorridorSegments;
                        int SegY = OffsetY / CorridorSegments;
                        while (true)
                        {
                            bool writing = false;
                            if (loops < CorridorSegments)
                            {
                                for (int y = 0; y < Mathf.Abs(SegY); y++)
                                {
                                    tiles[child0Center.x + (SegX * (loops)), child0Center.y + (SegY * loops) + ((int)Mathf.Sign(OffsetY) * y)] = true;
                                    for (int i = 1; i < HallWideness; i++)
                                    {
                                        if (child0Center.y + (SegY * (loops)) - i > 0 && child0Center.y + (SegY * (loops)) < ySizeOfMap - 1 && child0Center.x + (SegX * (loops)) - i > 0 && child0Center.x + (SegX * (loops)) < xSizeOfMap - 1)
                                        {
                                            tiles[child0Center.x + (SegX * (loops)) - i, child0Center.y + (SegY * loops) + ((int)Mathf.Sign(OffsetY) * y) - i] = true;
                                        }
                                    }
                                }
                                writing = true;
                            }
                            if (loops < CorridorSegments)
                            {
                                for (int x = 0; x < Mathf.Abs(SegX); x++)
                                {
                                    tiles[child0Center.x + (SegX * loops) + ((int)Mathf.Sign(OffsetX) * x), child0Center.y + (SegY * (loops+1))] = true;
                                    for (int i = 1; i < HallWideness; i++)
                                    {
                                        if (child0Center.y + (SegY * (loops)) - i > 0 && child0Center.y + (SegY * (loops)) < ySizeOfMap - 1 && child0Center.x + (SegX * (loops)) - i > 0 && child0Center.x + (SegX * (loops)) < xSizeOfMap - 1)
                                        {
                                            tiles[child0Center.x + (SegX * loops) + ((int)Mathf.Sign(OffsetX) * x) - i, child0Center.y + (SegY * (loops + 1)) - i] = true;
                                        }
                                    }
                                }
                                writing = true;
                            }
                            
                            loops++;

                            if (!writing)
                                break;

                            yield return null;
                        }
                    }
                }
                areasToConnect.RemoveAt(0);


            }
            else
                break;

            yield return null;
        }




        Debug.Log("Map Generator - End Halls");
        Debug.Log("Map Generator - TODO: Start Tile Generation");


        Debug.Log("Map Generator - TODO: End Tile Generation");


        Debug.Log("Map Generator - TODO: Start Prop Generation");


        Debug.Log("Map Generator - TODO: End Prop Generation");

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
