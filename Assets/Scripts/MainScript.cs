using UnityEngine;
using System.Collections;

// Attached to: Empty game object in scene.
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
    public static Rule activeRule = Rule.Split;
    private CSelectionTools[] rules = { new Rule1Selector(), new Rule2Selector(), 
                                        new Rule3Selector(), new Rule4Selector() };

    public GameObject ruleDisplay;
    private SpriteRenderer ruleBackRend;
    private SpriteRenderer ruleSignRend;
    private Sprite[] ruleBackSprites;
    private Sprite[] ruleSignSprites;

    private int currentRule = 0;

    // positioning
    public float margin = 1.0f;
    public Vector2 offset = new Vector2(0, 0);

	// Use this for initialization
	void Start () {

        CSelectionTools.s_input = inputDNA;
        CSelectionTools.s_lastRule = activeRule;

        CComparasonTools.s_input = inputDNA;
        CComparasonTools.s_goal = goalDNA;

        ruleBackRend = ruleDisplay.transform.FindChild("back").GetComponent<SpriteRenderer>();
        ruleSignRend = ruleDisplay.transform.FindChild("sign").GetComponent<SpriteRenderer>();

        ruleBackSprites = Resources.LoadAll<Sprite>("rule_back");
        ruleSignSprites = Resources.LoadAll<Sprite>("rule_sign");

        // This will be replaced by however we're loading in a level
        foreach (DNAScript dna in inputDNA)
        {
            dna.createDNA("rRGgbBYyrRGgbBYyrRGgbBYy", "yYBbgGRryYBbgGRryYBbgGRy");
        }
        goalDNA.createDNA("rRGggGRryYBbgGRryYBbgGRr", "yYBbbBYyrRGgbBYyrRGgbBYy");

        // start at rule 0
        changeRules(currentRule);

        foreach (CSelectionTools rule1 in rules)
        {
            rule1.initalise(0, 0);
        }
	}

	// Update is called once per frame
	void Update () {

        Debug.DrawLine(rules[currentRule].dnaSelectionBounds.center, rules[currentRule].dnaSelectionBounds.center + Vector3.up * 2, Color.blue);
        Debug.DrawLine(rules[currentRule].dnaSelectionBounds.min, rules[currentRule].dnaSelectionBounds.max, Color.blue);
        for (int i = 0; rules[currentRule].selected != null && i < rules[currentRule].selected.Length; i++)
        {
            Debug.DrawLine(rules[currentRule].selected[i].selectionBounds.center, rules[currentRule].selected[i].selectionBounds.center + Vector3.up * 2, Color.yellow);
            Debug.DrawLine(rules[currentRule].selected[i].selectionBounds.min, rules[currentRule].selected[i].selectionBounds.max, Color.yellow);
        }

        // change rule
        if ( Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.E) )
        {
            currentRule = (currentRule + 1) % 4;
            changeRules(currentRule);
        }
        else if ( Input.GetKeyDown(KeyCode.Q) )
        {
            currentRule = (currentRule == 3) ? 0 : currentRule + 1;
            changeRules(currentRule);
        }

        // move the selection box
        if (Input.GetKeyDown(KeyCode.W) || (Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rules[currentRule].dnaIndex++;
        }
        else if(Input.GetKeyDown(KeyCode.S) || (Input.GetKeyDown(KeyCode.DownArrow)) )
        {
            rules[currentRule].dnaIndex--;
        }
        if (Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            rules[currentRule].geneIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.RightArrow)))
        {
            rules[currentRule].geneIndex++;
        }

        // perform action (i.e flip or swap according to rule selected)
        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0)))
        {
            CSelectionTools.SelectedGenes[] sel = rules[currentRule].selected;

            // right now, we can only flip genes
            for (int i = 0; i < sel.Length; i++)
            {
                GeneScript[] first = sel[i].firstSelected;
                GeneScript[] second = sel[i].secondSelected;

                for (int j = 0; j < first.Length; j++)
                {
                    flip(first[j], second[j]);
                }
            }

        }

        foreach( float v in CComparasonTools.compare())
        {
            Debug.Log(v);
        }
	}

    public void flip(GeneScript first, GeneScript second)
    {
        Vector3 firstPos = first.transform.position;
        Vector3 secondPos = second.transform.position;

        char firstColour = first.colour;
        char secondColour = second.colour;

        float timeF = first.flipFrom(secondPos);
        float timeS = second.flipFrom(firstPos);

        first.changeType(secondColour);
        second.changeType(firstColour);
    }

    // move to the next rule, and change the background to reflect this
    private void changeRules(int nextRule)
    {
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
        ruleSignRend.transform.position = new Vector3(
                                            ruleBackRend.bounds.max.x,
                                            ruleBackRend.bounds.max.y,
                                            0);

	}

}
