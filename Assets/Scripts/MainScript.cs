﻿using UnityEngine;
using System.Collections;

// Attached to: Empty game object.
// Needs: 1 goal
//        1+ dna scene objects.

public class MainScript : MonoBehaviour {

    // Anthony -> do the rule numbers here correspond to the rule numbers in the mock-up?
    public enum Rule : int
    {
        Split = 0, Swap = 1, Flip_X = 2, Flip_Y = 3
    };

    public DNAScript[] inputDNA;
    public DNAScript goalDNA;



    // deals with rule and rule changes
    public Transform rulesNode;
    public Transform[] ruleScreens;
    public static Rule activeRule = Rule.Split;

    public float margin = 1.0f;
    public Vector2 offset = new Vector2(0, 0);

	// Use this for initialization
	void Start () {

        DNAScript.sprites = Resources.LoadAll<Sprite>("dna");

        CSelectionTools.s_input = inputDNA;
        CSelectionTools.s_lastRule = activeRule;


        rulesNode = transform.FindChild("RuleScreens");

        ruleScreens = new Transform[rulesNode.childCount];

        for (int i = 0; i < rulesNode.childCount; i++)
        {
            ruleScreens[i] = rulesNode.GetChild(i);
        }

        // dna positioning calculations
        float midpoint = (inputDNA.Length-1) * margin * 0.5f;
        float accumOffset = -midpoint;
        foreach (DNAScript dna in inputDNA)
        {
            // This will be replaced by however we're loading in a level
            dna.createDNA("rRGgbBYyrRGgbBYyrRGgbBYy", "yYBbgGRryYBbgGRryYBbgGRr");
               
            //dna.transform.position = new Vector3(offset.x, offset.y + accumOffset, 0f);
            /*
            Vector3 spos = Camera.main.WorldToScreenPoint(dna.transform.position);
            float dnaX = spos.x - (dna.length / 8 * 28);
            print(spos + " " + dnaX);
            dna.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(dnaX, spos.y, 0));
            accumOffset += margin;
             * */
        }

        goalDNA.createDNA("yYBbgGRryYBbgGRryYBbgGRr", "rRGgbBYyrRGgbBYyrRGgbBYy");
        //goalDNA.transform.position = new Vector3(offset.x, -4.5f);



        rule1.initalise(0,0);
	}

    private Rule1Selector rule1 = new Rule1Selector();

	// Update is called once per frame
	void Update () {

        Debug.DrawLine(rule1.dnaSelectionBounds.center, rule1.dnaSelectionBounds.center + Vector3.up*2, Color.blue);
        Debug.DrawLine(rule1.dnaSelectionBounds.min, rule1.dnaSelectionBounds.max, Color.blue);
        Debug.DrawLine(rule1.geneSelectionBounds.center, rule1.geneSelectionBounds.center + Vector3.up * 2, Color.green);
        Debug.DrawLine(rule1.geneSelectionBounds.min, rule1.geneSelectionBounds.max, Color.green);
	}

    int steps = 10;
    bool isScreenAnimating = false;

    IEnumerator ModeSwitchAnimation(Rule newRule)
    {
        isScreenAnimating = true;
        //print("new: " + newRule + " old " + active_rule);
        Vector3 dir = ruleScreens[(int)newRule].localPosition - ruleScreens[(int)activeRule].localPosition;
        Vector3 pos = rulesNode.transform.position;

        for (int i = 1; i <= steps; i++)
        {
            rulesNode.transform.position = pos - (dir * (i * 0.1f));
            yield return null;
        }
        isScreenAnimating = false;
    }

    void OnGUI()
    {
        
        float half = Screen.width / 2;

        if (GUI.Button(new Rect(half - 55, 10, 50, 50), GUIContent.none) && isScreenAnimating == false)
        {
            Rule newRule = (Rule)Mathf.Clamp((int)activeRule - 1, (int)Rule.Split, (int)Rule.Flip_Y);

            if (newRule != activeRule)
            {
                //StopCoroutine("ModeSwitchAnimation");
                StartCoroutine(ModeSwitchAnimation(newRule));
                activeRule = newRule;
            }
        }

        if (GUI.Button(new Rect(half + 5, 10, 50, 50), GUIContent.none) && isScreenAnimating == false)
        {
            Rule newRule = (Rule)Mathf.Clamp((int)activeRule + 1, (int)Rule.Split, (int)Rule.Flip_Y);

            if (newRule != activeRule)
            {
                //StopCoroutine("ModeSwitchAnimation");
                StartCoroutine(ModeSwitchAnimation(newRule));
                activeRule = newRule;
            }
        }

    }
	
}
