using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI(){
        MapGenerator mapGenerator = (MapGenerator) target;

        //if any value was changed in the editor
        if(DrawDefaultInspector()){
            if(mapGenerator.autoUpdate){
                mapGenerator.generateMap();
            }
        }

        if(GUILayout.Button("Generate")){
            mapGenerator.generateMap();
        }

    }
}
