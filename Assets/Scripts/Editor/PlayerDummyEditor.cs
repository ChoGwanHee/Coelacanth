using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerDummy))]
public class PlayerDummyEditor : Editor {

    private PlayerDummy script;

    private void OnEnable()
    {
        script = target as PlayerDummy;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("기본 위치 설정"))
        {
            script.originPos = script.transform.position;
        }
        if (GUILayout.Button("기본 위치로 이동"))
        {
            script.transform.position = script.originPos;
        }

        GUILayout.EndHorizontal();
        
    }
}
