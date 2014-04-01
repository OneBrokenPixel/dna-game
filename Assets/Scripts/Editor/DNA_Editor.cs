using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(DNA))]
public class DNA_Editor : Editor {

    Vector2 scroll = new Vector2(0, 0);

    public override void OnInspectorGUI()
    {


        serializedObject.Update();
        DNA dnaTarget = target as DNA;
        GUI.changed = false;

        if (DNA.Gene == null)
        {
            DNA.Gene = Resources.Load<GameObject>("Gene");
        }
        if (Gene.Genes == null)
        {
            Gene.Genes = Resources.LoadAll<Sprite>("dna");
        }

        dnaTarget.dna.parent = dnaTarget.transform;

        float stepSize = EditorGUILayout.FloatField("Step Size", dnaTarget.dna.stepSize );

        if (dnaTarget.dna.stepSize != stepSize)
        {
            dnaTarget.dna.stepSize = stepSize;
            Vector3 step = dnaTarget.transform.position;
            for (int i = 0; i < dnaTarget.dna.length; i++)
            {
                dnaTarget.dna.topSprites[i].transform.position = step;
                dnaTarget.dna.bottomSprites[i].transform.position = step;
                step.x += stepSize;
            }
        }

        dnaTarget.dna.length = Mathf.Max(0, EditorGUILayout.IntField("Length", dnaTarget.dna.length));

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(100f));
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < dnaTarget.dna.length; i++)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField((i+1).ToString(), GUILayout.Width(32.0f));
            //dnaTarget.dna.top[i] = (DNA.GATC)EditorGUILayout.EnumPopup(dnaTarget.dna.top[i]);
            //dnaTarget.dna.topSprites[i].sprite = DNA.Genes[(int)dnaTarget.dna.top[i]];
            dnaTarget.dna.topSprites[i].type = (Gene.GATC)EditorGUILayout.EnumPopup(dnaTarget.dna.topSprites[i].type);

            //dnaTarget.dna.bottom[i] = (DNA.GATC)EditorGUILayout.EnumPopup(dnaTarget.dna.bottom[i]);
            dnaTarget.dna.bottomSprites[i].type = (Gene.GATC)EditorGUILayout.EnumPopup(dnaTarget.dna.bottomSprites[i].type);
            //dnaTarget.dna.bottomSprites[i].sprite = DNA.Genes[(int)dnaTarget.dna.bottom[i]];



            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();

        //Debug.Log("Dirty " + GUI.changed);

        if (GUI.changed)
            EditorUtility.SetDirty(dnaTarget);

        serializedObject.ApplyModifiedProperties();
    }
}
