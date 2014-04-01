using UnityEngine;
using UnityEditor;
using System.Collections;

[CanEditMultipleObjects]
[CustomEditor(typeof(Gene))]
public class Gene_Editor : Editor
{

    Vector2 scroll = new Vector2(0, 0);

    public override void OnInspectorGUI()
    {


        if (Gene.Genes == null)
        {
            Gene.Genes = Resources.LoadAll<Sprite>("dna");
        }

        serializedObject.Update();
        Gene gt = target as Gene;

        
        GUI.changed = false;
        gt.type = (Gene.GATC)EditorGUILayout.EnumPopup(gt.type);

        foreach (Object t in targets)
        {
            Gene geneTarget = t as Gene;



            if (GUI.changed)
            {
                geneTarget.type = gt.type;
                EditorUtility.SetDirty(geneTarget);
            }

        }
        serializedObject.ApplyModifiedProperties();
    }
}
