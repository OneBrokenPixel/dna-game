using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(DNA))]
public class DNA_Editor : Editor {

    Vector2 scroll = new Vector2(0, 0);

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DNA myTarget = (DNA)target;

        myTarget.dna.length = Mathf.Max(0, EditorGUILayout.IntField("Length", myTarget.dna.length));

        EditorGUILayout.Separator();

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(75f));

        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < myTarget.dna.length; i++)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField((i+1).ToString(), GUILayout.Width(32.0f));
            myTarget.dna.top[i].type = (DNA.GATC)EditorGUILayout.EnumPopup(myTarget.dna.top[i].type);
            myTarget.dna.bottom[i].type = (DNA.GATC)EditorGUILayout.EnumPopup(myTarget.dna.bottom[i].type);

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();

        

        serializedObject.ApplyModifiedProperties();
    }
}
