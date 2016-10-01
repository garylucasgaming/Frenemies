using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(d_MapGenerator))]
public class d_MapGeneratorInspector :  Editor
{
    bool editingSubdivisionValues = false;
    bool editingRoomValues = false;
    bool editingHallValues = false;

    void OnEnable()
    {

    }
    

    public override void OnInspectorGUI()
    {
        d_MapGenerator mapGenerator = (d_MapGenerator)target;




        editingSubdivisionValues = EditorGUILayout.BeginToggleGroup("Edit Subdivision Values",editingSubdivisionValues);
        if(editingSubdivisionValues)
        {
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Map X: "));
                mapGenerator.MapXSize = EditorGUILayout.IntSlider(mapGenerator.MapXSize, 0, 512);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Map Y: "));
                mapGenerator.MapYSize = EditorGUILayout.IntSlider(mapGenerator.MapYSize, 0, 512);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Map Subdivisions: ", "The number of times the map is divided into seperate rooms."));
                mapGenerator.Subdivisions = EditorGUILayout.IntSlider(mapGenerator.Subdivisions, 1, 8);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.MinMaxSlider(new GUIContent("Subdivision X Range"), ref mapGenerator.SubdivisionXRange.x, ref mapGenerator.SubdivisionXRange.y, 0.0f, 1.0f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.MinMaxSlider(new GUIContent("Subdivision Y Range"), ref mapGenerator.SubdivisionYRange.x, ref mapGenerator.SubdivisionYRange.y, 0.0f, 1.0f);
            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndToggleGroup();


        editingRoomValues = EditorGUILayout.BeginToggleGroup("Edit Room Values", editingRoomValues);
        if(editingRoomValues)
        {
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.MinMaxSlider(new GUIContent("Room X Range"), ref mapGenerator.RoomXRange.x, ref mapGenerator.RoomXRange.y, 0.0f, 1.0f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.MinMaxSlider(new GUIContent("Room Y Range"), ref mapGenerator.RoomYRange.x, ref mapGenerator.RoomYRange.y, 0.0f, 1.0f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                mapGenerator.EliminateRoomsSmallerOnX = EditorGUILayout.IntSlider(new GUIContent("Eliminate Rooms < X: "), mapGenerator.EliminateRoomsSmallerOnX, 0, mapGenerator.MapXSize);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                mapGenerator.EliminateRoomsSmallerOnY = EditorGUILayout.IntSlider(new GUIContent("Eliminate Rooms < Y: "), mapGenerator.EliminateRoomsSmallerOnY, 0, mapGenerator.MapYSize);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndToggleGroup();

        editingHallValues = EditorGUILayout.BeginToggleGroup("Edit Hall Values", editingHallValues);
        if(editingHallValues)
        {
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Minimum Choke Points: "));
                mapGenerator.MinimumChokePoints = EditorGUILayout.IntSlider(mapGenerator.MinimumChokePoints, 1, 5);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Hall Wideness: "));
                mapGenerator.HallWideness = EditorGUILayout.IntSlider(mapGenerator.HallWideness, 1, 5);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Hall Segments: "));
                mapGenerator.CorridorSegments = EditorGUILayout.IntSlider(mapGenerator.CorridorSegments, 1, 5);
            EditorGUILayout.EndHorizontal();



        }
        EditorGUILayout.EndToggleGroup();


        if (mapGenerator.generating == false && EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Generate Map"))
            {
                mapGenerator.StartCoroutine(mapGenerator.GenerateMap());
            }
        }

    }
}
