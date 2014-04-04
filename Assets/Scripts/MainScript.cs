using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {

    public GameObject dna;
    public GameObject selector;

    public Rect buttonBar = new Rect(10, 10, 75, 200);

    private DNAScript dnaScript;
    private bool selecting;
	// Use this for initialization
	void Start () {
        selecting = true;

        dnaScript = dna.GetComponent<DNAScript>();
        dnaScript.Start();

        // This will be replaced by however we're loading in a level
        dnaScript.createDNA("rRGgbBYy", "yYBbgGRr");
	}
	

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            print("selecting");
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
            //print(selector.transform.
            selector.transform.position = new Vector3(mPos.x, 0f, 0);
        }
	}

    void OnGUI()
    {

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
    }
}
