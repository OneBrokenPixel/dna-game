using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {

    public enum Rule : int
    {
        Split = 0, Swap = 1, Flip_X = 2, Flip_Y = 3
    };

    public DNAScript[] input_dna;
    public DNAScript goal_dna;

    public GameObject selector;

    public Transform rules_node;
    public Transform[] rule_Screens;
    public int active_rule = (int)Rule.Split;

    //public Rect buttonBar = new Rect(10, 10, 75, 200);

    //private DNAScript dnaScript;
    private bool selecting;
	// Use this for initialization
	void Start () {
        selecting = true;

        DNAScript.sprites = Resources.LoadAll<Sprite>("dna");

        rules_node = transform.GetChild(0);

        rule_Screens = new Transform[rules_node.childCount];

        for (int i = 0; i < rules_node.childCount; i++)
        {
            rule_Screens[i] = rules_node.GetChild(i);
        }

        foreach (DNAScript dna in input_dna)
        {
            // This will be replaced by however we're loading in a level
            dna.createDNA("rRGgbBYy", "yYBbgGRr");
        }
	}
	

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            selecting = true;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            selecting = false;
            selector.transform.position = new Vector3(0, 0, -10);
        }

	    if (selecting)
        {
            Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selector.transform.position = new Vector3(mPos.x, 0f, 0);

            if (Input.GetMouseButtonDown(0))
            {
                SpriteRenderer rend = selector.GetComponent<SpriteRenderer>();
                foreach (DNAScript dna in input_dna)
                {
                    dna.flipGenesInArea(Vector3.Scale(selector.transform.position - rend.sprite.bounds.extents, selector.transform.localScale),
                                          Vector3.Scale(selector.transform.position + rend.sprite.bounds.extents, selector.transform.localScale));
                }
            }
        }
	}

    int steps = 10;
    bool isScreenAnimating = false;

    IEnumerator ModeSwitchAnimation(int newRule)
    {
        isScreenAnimating = true;
        //print("new: " + newRule + " old " + active_rule);
        Vector3 dir = rule_Screens[newRule].localPosition - rule_Screens[active_rule].localPosition;

        Vector3 pos = rules_node.transform.position;

        for (int i = 1; i <= steps; i++ )
        {
            rules_node.transform.position = pos - (dir*(i*0.1f));
            yield return null;
        }
        isScreenAnimating = false;
    }


    void OnGUI()
    {
        /*
        GUILayout.BeginArea(buttonBar);
        GUILayout.BeginVertical();

        if(GUILayout.Button("Rule 1"))
        {
            print("You clicked the Rule 1 button!");
        }

        if (GUILayout.Button("Rule 2"))
        {
            print("You clicked the Rule 2 button!");
        }
        if (GUILayout.Button("Rule 3"))
        {
            print("You clicked the Rule 3 button!");
        }
        if (GUILayout.Button("Rule 4"))
        {
            print("You clicked the Rule 4 button!");
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
         */


        float half = Screen.width / 2;

        if (GUI.Button(new Rect(half - 55, 10, 50, 50), GUIContent.none) && isScreenAnimating == false)
        {
            int newRule = Mathf.Clamp(active_rule - 1, (int)Rule.Split, (int)Rule.Flip_Y);

            if (newRule != active_rule)
            {
                //StopCoroutine("ModeSwitchAnimation");
                StartCoroutine(ModeSwitchAnimation(newRule));
                active_rule = newRule;
            }
        }

        if (GUI.Button(new Rect(half + 5, 10, 50, 50), GUIContent.none) && isScreenAnimating == false)
        {
            int newRule = Mathf.Clamp(active_rule + 1, (int)Rule.Split, (int)Rule.Flip_Y);

            if (newRule != active_rule)
            {
                //StopCoroutine("ModeSwitchAnimation");
                StartCoroutine(ModeSwitchAnimation(newRule));
                active_rule = newRule;
            }
        }
    }
}
