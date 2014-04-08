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

    public GameObject selector;
    private bool selecting;

    // deals with rule and rule changes
    public Transform rulesNode;
    public Transform[] ruleScreens;
    public int activeRule = (int)Rule.Split;

    public float margin = 1.0f;
    public Vector2 offset = new Vector2(0, 0);

	// Use this for initialization
	void Start () {
        selecting = true;

        DNAScript.sprites = Resources.LoadAll<Sprite>("dna");

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
            dna.transform.position = new Vector3(offset.x, offset.y + accumOffset, 0f);

            accumOffset += margin;
        }

        goalDNA.createDNA("yYBbgGRryYBbgGRryYBbgGRr", "rRGgbBYyrRGgbBYyrRGgbBYy");
        goalDNA.transform.position = new Vector3(offset.x, -4.5f);

	}
	

	// Update is called once per frame
	void Update () {
        // temporary - turns the selection box on/off
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
                foreach (DNAScript dna in inputDNA)
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
        Vector3 dir = ruleScreens[newRule].localPosition - ruleScreens[activeRule].localPosition;
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
            int newRule = Mathf.Clamp(activeRule - 1, (int)Rule.Split, (int)Rule.Flip_Y);

            if (newRule != activeRule)
            {
                //StopCoroutine("ModeSwitchAnimation");
                StartCoroutine(ModeSwitchAnimation(newRule));
                activeRule = newRule;
            }
        }

        if (GUI.Button(new Rect(half + 5, 10, 50, 50), GUIContent.none) && isScreenAnimating == false)
        {
            int newRule = Mathf.Clamp(activeRule + 1, (int)Rule.Split, (int)Rule.Flip_Y);

            if (newRule != activeRule)
            {
                //StopCoroutine("ModeSwitchAnimation");
                StartCoroutine(ModeSwitchAnimation(newRule));
                activeRule = newRule;
            }
        }
    }
	
}
