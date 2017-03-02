using UnityEngine;
using System.Collections;
using UnityEditor;

//Which Script is this an editor for
[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor {


    public override void OnInspectorGUI()
    {
        MapGenerator map = target as MapGenerator;

        if (DrawDefaultInspector())
        {
             map.GeneratorMap();
        }

        if (GUILayout.Button("GenerateMap"))
        {
            map.GeneratorMap();
        }
      
    }

}
