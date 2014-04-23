using UnityEngine;
using System.Collections;

// Attached to: Empty game object in scene.
// Needs: 1 goal
//        1+ dna scene objects.
//        selection box
//        rule display

public class MainScript : MonoBehaviour {

    public DNAScript[] inputDNA;
    public DNAScript goalDNA;

    public GameObject ruleDisplay;
    public SpriteRenderer[] selBox;
    private SpriteRenderer ruleBackRend;
    private SpriteRenderer ruleSignRend;
    private Sprite[] ruleBackSprites;
    private Sprite[] ruleSignSprites;
    private Sprite[] selBoxSprites;

    private int currentRule = 0;

    public float margin = 2.6f;
    public Vector3 selectionStretch = new Vector3(1.0f, 0.8f, 1f);
    private CLoadLevelTools.SLevel[] levels;
    public int startLevel = 0;
    private int _level = 0;
    public int level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = Mathf.Clamp(value, 0, levels.Length - 1);
            foreach (DNAScript dna in inputDNA)
            {
                dna.clearGenes();
            }
            DNAScript[] iDNA = new DNAScript[levels[_level].InputDNA.Length];
            for (int i = 0; i < levels[_level].InputDNA.Length; i++)
            {
                string top = levels[_level].InputDNA[i].top;
                string bottom = levels[_level].InputDNA[i].bottom;
                inputDNA[i].createDNA(top, bottom);
                iDNA[i] = inputDNA[i];
            }

            goalDNA.createDNA(levels[_level].GoalDNA.top, levels[_level].GoalDNA.bottom);

            CSelectionTools.s_input = iDNA;
            CComparisonTools.s_input = iDNA;
            CComparisonTools.s_goal = goalDNA;

            CSelectionTools.rules[currentRule].forceUpdateSelection();
            CComparisonTools.UpdateCompleteness();
            highlightDNA();
        }
    }

    private Vector3 rule3SelectionRise = new Vector3(0, 0.22f, 0);
    private float pxUnit = 0;

    public Animator welldoneScreen;

	// Use this for initialization
	void Start () {


        pxUnit = 1f / (Screen.width / (Camera.main.orthographicSize * 2));

        ruleBackRend = ruleDisplay.transform.FindChild("back").GetComponent<SpriteRenderer>();
        ruleSignRend = ruleDisplay.transform.FindChild("sign").GetComponent<SpriteRenderer>();

        ruleBackSprites = Resources.LoadAll<Sprite>("rule_back");
        ruleSignSprites = Resources.LoadAll<Sprite>("rule_sign");
        selBoxSprites = Resources.LoadAll<Sprite>("selection");


        Vector3 pos = inputDNA[0].transform.position;
        float y = pos.y;
        for (var i = 1; i < inputDNA.Length; i++)
        {    
            //inputDNA[i].createDNA("rRGgbBYyrRGgbBYyrRGgbBYy", "yYBbgGRryYBbgGRryYBbgGRy");
            // position dna relative to the first
            y -= margin;
            inputDNA[i].transform.position = new Vector3(pos.x, y, 0);
            
        }
         

        levels = CLoadLevelTools.LoadLevels("Levels");

        // set the start level
        level = startLevel;

        // start at rule 0
        changeRules(currentRule);

        foreach (CSelectionTools.BaseRuleSelector rule1 in CSelectionTools.rules)
        {
            rule1.initalise(0, 0);
        }


	}

	// Update is called once per frame
	void Update () {

        /*
         * debug draw code removed
        for (int i = 0; CSelectionTools.rules[currentRule].selected != null && i < CSelectionTools.rules[currentRule].selected.Length; i++)
        {
            Debug.DrawLine(CSelectionTools.rules[currentRule].selectionPoints[0], CSelectionTools.rules[currentRule].selectionPoints[0] + Vector3.up, Color.yellow);
            Debug.DrawLine(CSelectionTools.rules[currentRule].selectionPoints[1], CSelectionTools.rules[currentRule].selectionPoints[1] + Vector3.up, Color.yellow);
        }
         */

        bool isOn = welldoneScreen.GetBool("isOn");

        if (isOn == true)
        {
            if(Input.anyKeyDown == true)
            {
                
                if( level == levels.Length-1)
                {
                    Application.Quit();
                    return;
                }
                level += 1;
                
                welldoneScreen.SetBool("isOn", false);
            }
        }
        else
        {
            // change rule
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.E))
            {
                int nextRule = (currentRule + 1) % 4;
                changeRules(nextRule);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                int nextRule = (currentRule == 0) ? 3 : currentRule - 1;
                changeRules(nextRule);
            }

            // move the selection box
            if (Input.GetKeyDown(KeyCode.W) || (Input.GetKeyDown(KeyCode.UpArrow)))
            {
                CSelectionTools.rules[currentRule].dnaIndex--;
                highlightDNA();

            }
            else if (Input.GetKeyDown(KeyCode.S) || (Input.GetKeyDown(KeyCode.DownArrow)))
            {
                CSelectionTools.rules[currentRule].dnaIndex++;
                highlightDNA();
            }
            if (Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                CSelectionTools.rules[currentRule].geneIndex--;
                highlightGenes();
            }
            else if (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.RightArrow)))
            {
                CSelectionTools.rules[currentRule].geneIndex++;
                highlightGenes();
            }

            //change level
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                level++;
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                level--;
            }

            // perform action (i.e flip or swap according to rule selected)
            if (Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0)))
            {
                /*
                CSelectionTools.SelectedGenes[] sel = CSelectionTools.rules[currentRule].selected;

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
                */
                CSelectionTools.rules[currentRule].appyRule();
                foreach (float f in CComparisonTools.compare())
                {
                    if (f >= 1f)
                        welldoneScreen.SetBool("isOn", true);
                }
            }
        }

	}

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 400));
        {
            GUILayout.BeginVertical();
            {
                foreach (DNAScript dna in CComparisonTools.s_input)
                {
                    //Debug.Log(dna.completeness);
                    GUILayout.Label(dna.gameObject.name + ": " + (dna.completeness * 100f) + "%");
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
    }

    // set the background of the current dna
    public void highlightDNA()
    {
        int dnaIndex = CSelectionTools.rules[currentRule].dnaIndex;
        Vector3 pos = new Vector3(0, 0, 0);
        float midPoint = 0;

        // move the selection box and dna highlighter to the relevant dna
        if (currentRule == 0 || currentRule == 1)
        {
            //ruleDisplay.transform.position = inputDNA[dnaIndex].transform.position;
            pos = inputDNA[dnaIndex].transform.position;
            midPoint = pos.y;
        }
        else if (currentRule == 2 || currentRule == 3)
        {
            float y1 = 0;
            float y2 = 0;
            // if we use more than 3 dna, this will have to be changed
            if (dnaIndex == 0)
            {
                y1 = inputDNA[0].transform.position.y;
                y2 = inputDNA[1].transform.position.y;
            }
            else if (dnaIndex == 1)
            {
                y1 = inputDNA[1].transform.position.y;
                y2 = inputDNA[2].transform.position.y;
            }

            //Vector3 pos = ruleDisplay.transform.position;
            pos = inputDNA[0].transform.position;
            midPoint = (y1 + y2) / 2;
            //Vector3 newPos = new Vector3(pos.x, midPoint, 0);
            //ruleDisplay.transform.position = newPos;
        }
        Vector3 newPos = new Vector3(pos.x-0.8f, midPoint, 0);
        ruleDisplay.transform.position = newPos;
        highlightGenes();
    }

    // move the selection box to the selected genes
    public void highlightGenes()
    {
        
        if (currentRule == 0)
        {
            Vector3 offset = new Vector3(0, selBoxSprites[0].bounds.max.y - pxUnit, 0);

            selBox[0].sprite = selBoxSprites[0];
            selBox[0].transform.position = CSelectionTools.rules[currentRule].selectionPoints[0] + offset;

            selBox[1].sprite = selBoxSprites[0];
            selBox[1].transform.position = CSelectionTools.rules[currentRule].selectionPoints[1] - offset;

        }
        else if (currentRule == 1)
        {
            Vector3 offset = new Vector3(0, 0, 0);

            selBox[0].sprite = selBoxSprites[1];
            selBox[0].transform.position = CSelectionTools.rules[currentRule].selectionPoints[0] - offset;

            selBox[1].sprite = selBoxSprites[1];
            selBox[1].transform.position = CSelectionTools.rules[currentRule].selectionPoints[1] + offset;
        }
        else if (currentRule == 2)
        {
            Vector3 offset = new Vector3(0, selBoxSprites[1].bounds.max.y - pxUnit, 0);

            selBox[0].sprite = selBoxSprites[1];
            selBox[0].transform.position = CSelectionTools.rules[currentRule].selectionPoints[0] + offset;

            selBox[1].sprite = selBoxSprites[1];
            selBox[1].transform.position = CSelectionTools.rules[currentRule].selectionPoints[1] - offset;
        }
        else if (currentRule == 3)
        {

            Vector3 offset = new Vector3();

            if( CSelectionTools.rules[currentRule].geneIndex == 1 )
            {
                offset.Set(0, selBoxSprites[0].bounds.min.y + pxUnit, 0);
            }
            else
            {
                offset.Set(0, selBoxSprites[0].bounds.max.y - pxUnit, 0);
            }

            selBox[0].sprite = selBoxSprites[2];
            selBox[0].transform.position = CSelectionTools.rules[currentRule].selectionPoints[0] + offset;
            selBox[1].sprite = selBoxSprites[2];
            selBox[1].transform.position = CSelectionTools.rules[currentRule].selectionPoints[1] + offset;
        }

    }

    // animate a flip between two genes
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
        if (nextRule >= 2 && CSelectionTools.s_input.Length < 2)
        {
            nextRule = nextRule % 2;
        }

        if (nextRule == 0 || nextRule == 1)
        {
            ruleBackRend.sprite = ruleBackSprites[1];
            // do some stretching to fit dna size
            ruleDisplay.transform.position = inputDNA[0].transform.position;
        }
        else if (nextRule == 2 || nextRule == 3)
        {
            // needs to be a check here that there are 2+ dna
            // (otherwise rules 2 and 3 don't make sense)
            ruleBackRend.sprite = ruleBackSprites[0];
            Vector3 pos = ruleDisplay.transform.position;
            pos.y = (inputDNA[0].transform.position.y + inputDNA[1].transform.position.y) / 2;
            ruleDisplay.transform.position = pos;
        }

        currentRule = nextRule;

        ruleSignRend.sprite = ruleSignSprites[nextRule];
        ruleSignRend.transform.position = new Vector3(
                                            ruleBackRend.bounds.max.x,
                                            ruleBackRend.bounds.max.y,
                                            0);

        // reset highlighter and selection box to first dna
        CSelectionTools.rules[currentRule].dnaIndex = 0;
        highlightDNA();
	}

}
