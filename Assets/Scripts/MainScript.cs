using UnityEngine;
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
    //public Transform rulesNode;
    //public Transform[] ruleScreens;
    public static Rule activeRule = Rule.Split;

    public GameObject ruleDisplay;
    private SpriteRenderer ruleBackRend;
    private SpriteRenderer ruleSignRend;
    private Sprite[] ruleBackSprites;
    private Sprite[] ruleSignSprites;

    private int currentRule = 0;

    public float margin = 1.0f;
    public Vector2 offset = new Vector2(0, 0);

	// Use this for initialization
	void Start () {

        CSelectionTools.s_input = inputDNA;
        CSelectionTools.s_lastRule = activeRule;

        ruleBackRend = ruleDisplay.transform.FindChild("back").GetComponent<SpriteRenderer>();
        ruleSignRend = ruleDisplay.transform.FindChild("sign").GetComponent<SpriteRenderer>();

        ruleBackSprites = Resources.LoadAll<Sprite>("rule_back");
        ruleSignSprites = Resources.LoadAll<Sprite>("rule_sign");

        // This will be replaced by however we're loading in a level
        foreach (DNAScript dna in inputDNA)
        {
            dna.createDNA("rRGgbBYyrRGgbBYyrRGgbBYy", "yYBbgGRryYBbgGRryYBbgGRr");
        }
        goalDNA.createDNA("yYBbgGRryYBbgGRryYBbgGRr", "rRGgbBYyrRGgbBYyrRGgbBYy");

        changeRules(currentRule);

        foreach (CSelectionTools rule1 in rules)
        {
            rule1.initalise(0, 0);
        }
	}

    private CSelectionTools[] rules = { new Rule1Selector(), new Rule2Selector(), new Rule3Selector(), new Rule4Selector() };

	// Update is called once per frame
	void Update () {

        Debug.DrawLine(rules[currentRule].dnaSelectionBounds.center, rules[currentRule].dnaSelectionBounds.center + Vector3.up * 2, Color.blue);
        Debug.DrawLine(rules[currentRule].dnaSelectionBounds.min, rules[currentRule].dnaSelectionBounds.max, Color.blue);
        for (int i = 0; rules[currentRule].selected != null && i < rules[currentRule].selected.Length; i++)
        {
            Debug.DrawLine(rules[currentRule].selected[i].selectionBounds.center, rules[currentRule].selected[i].selectionBounds.center + Vector3.up * 2, Color.green);
            Debug.DrawLine(rules[currentRule].selected[i].selectionBounds.min, rules[currentRule].selected[i].selectionBounds.max, Color.green);
        }

        if ( Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.E) )
        {
            currentRule = (currentRule + 1) % 4;
            changeRules(currentRule);
        }
        else if ( Input.GetKeyDown(KeyCode.Q) )
        {
            //currentRule = (currentRule - 1) % 4;
            currentRule = (currentRule == 3) ? 0 : currentRule + 1;
            changeRules(currentRule);
        }
        if( Input.GetKeyDown(KeyCode.W) )
        {
            rules[currentRule].dnaIndex++;
        }
        else if(Input.GetKeyDown(KeyCode.S) )
        {
            rules[currentRule].dnaIndex--;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            rules[currentRule].geneIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            rules[currentRule].geneIndex++;
        }
	}

    private void changeRules(int nextRule)
    {
        print(nextRule);
        if (nextRule == 0 || nextRule == 1)
        {
            ruleBackRend.sprite = ruleBackSprites[1];
            // do some stretching to fit dna size
            ruleDisplay.transform.position = inputDNA[0].transform.position;
        }
        else if (nextRule == 2 || nextRule == 3)
        {
            // needs to be a check here that there
            ruleBackRend.sprite = ruleBackSprites[0];
            Vector3 pos = ruleDisplay.transform.position;
            pos.y = (inputDNA[0].transform.position.y + inputDNA[1].transform.position.y) / 2;
            ruleDisplay.transform.position = pos;
        } 
        ruleSignRend.sprite = ruleSignSprites[nextRule];
        print(ruleBackRend.sprite.bounds.max);
        ruleSignRend.transform.position = new Vector3(
                                            ruleBackRend.bounds.max.x,
                                            ruleBackRend.bounds.max.y,
                                            0);

	}


    /*
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
	*/
}
