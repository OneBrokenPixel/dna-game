using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[System.Serializable]
public abstract class CSelectionTools
{

    public static DNAScript[] s_input; // is reference to input array

    public struct SelectedGenes // struct of selected genes
    {
        public GeneScript[] firstSelected;
        public GeneScript[] secondSelected;
        public Bounds selectionBounds;

        public void init(int top, int bottom)
        {
            firstSelected = new GeneScript[top];
            secondSelected = new GeneScript[bottom];
            selectionBounds = new Bounds();
        }
    }

    public SelectedGenes[] selected; // array of selected genes;
                                     // one SelectedGenes Per strand of DNA selected;
                                     // rules 3 and 4 will have SelectedGenes[2]

    public Bounds dnaSelectionBounds = new Bounds();

    protected int _dnaIndex = 0; // current index of the dna in s_input;
    public int dnaIndex
    {
        get { return _dnaIndex;                     }
        set { _dnaIndex = value; updateSelection(); } // updates the selected array
    }

    protected int _geneIndex = 0; // current index of the genes in the selected dna;
    public int geneIndex
    {
        get { return _geneIndex;                     }
        set { _geneIndex = value; updateSelection(); } // updates selected array;
    }

    static System.Collections.Generic.List<CSelectionTools> _tools;

    protected abstract void updateSelection(); // abstact member function that updates the selected array.

    public void initalise()
    {
        updateSelection();
    }

    public void initalise(int dna, int gene)
    {
        _dnaIndex = dna;
        _geneIndex = gene;

        updateSelection();
    }

    public void forceUpdateSelection()
    {
        updateSelection();
    }
}

public class Rule1Selector : CSelectionTools
{
    protected override void updateSelection()
    {
        if (s_input == null)
        {
            selected = null;
            return;
        }
        if (selected == null || selected.Length != 1)
        {
            selected = new SelectedGenes[1];

            selected[0].init(4, 4);
        }

        _dnaIndex = Mathf.Clamp(_dnaIndex, 0, s_input.Length-1);

        DNAScript dna = s_input[_dnaIndex];

        dnaSelectionBounds = dna.renderer.bounds;

        _geneIndex = Mathf.Clamp(_geneIndex, 0, dna.length-4);

        Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        for (int i = 0; i < 4; i++)
        {

            //Debug.Log("ha" + selected[0].topStrand.Length);
            selected[0].firstSelected[i] = dna.topStrand[_geneIndex + i];
            selected[0].secondSelected[i] = dna.bottomStrand[_geneIndex + i];


            min = Vector3.Min(selected[0].secondSelected[i].renderer.bounds.min, min);
            max = Vector3.Max(selected[0].firstSelected[i].renderer.bounds.max, max);
        }

        selected[0].selectionBounds.SetMinMax(min, max);
    }
}

public class Rule2Selector : CSelectionTools
{

    protected override void updateSelection()
    {
        if (s_input == null)
        {
            selected = null;
            return;
        }
        if (selected == null || selected.Length != 1)
        {
            selected = new SelectedGenes[1];
            selected[0].init(8, 8);
        }

        _dnaIndex = Mathf.Clamp(_dnaIndex, 0, s_input.Length-1);

        DNAScript dna = s_input[_dnaIndex];

        dnaSelectionBounds = dna.renderer.bounds;

        _geneIndex = Mathf.Clamp(_geneIndex, 0, dna.length - 8);
        Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        for (int i = 0; i < 8; i++)
        {

            selected[0].firstSelected[i] = dna.topStrand[_geneIndex + i];
            selected[0].secondSelected[i] = dna.bottomStrand[_geneIndex + i];


            min = Vector3.Min(selected[0].secondSelected[i].renderer.bounds.min, min);
            max = Vector3.Max(selected[0].firstSelected[i].renderer.bounds.max, max);
        }
        selected[0].selectionBounds.SetMinMax(min, max);
    }
}

public class Rule3Selector : CSelectionTools
{

    protected override void updateSelection()
    {
        if (s_input == null || s_input.Length < 2)
        {
            selected = null;
            return;
        }
        if (selected == null || selected.Length != 2)
        {
            selected = new SelectedGenes[2];
            for( int i = 0; i < 2; i++)
            {
                selected[i].init(4, 4);
            }
        }

        _dnaIndex = Mathf.Clamp(_dnaIndex, 0, s_input.Length-2);


        for (int d = 0; d < 2; d++)
        {
            DNAScript dna = s_input[_dnaIndex+d];
            dnaSelectionBounds = dna.renderer.bounds;
            if (dna.length != 0)
            {
                _geneIndex = Mathf.Clamp(_geneIndex, 0, dna.length - 4);
                Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                for (int i = 0; i < 4; i++)
                {

                    selected[d].firstSelected[i] = dna.topStrand[_geneIndex + i];
                    selected[d].secondSelected[i] = dna.bottomStrand[_geneIndex + i];


                    min = Vector3.Min(selected[d].secondSelected[i].renderer.bounds.min, min);
                    max = Vector3.Max(selected[d].firstSelected[i].renderer.bounds.max, max);
                }
                selected[d].selectionBounds.SetMinMax(min, max);
            }

        }

    }
}

public class Rule4Selector : CSelectionTools
{

    protected override void updateSelection()
    {
        if (s_input == null || s_input.Length < 2)
        {
            selected = null;
            return;
        }


        _dnaIndex = Mathf.Clamp(_dnaIndex, 0, s_input.Length - 2);

        int len = s_input[_dnaIndex].length;
        if (selected == null || selected.Length != 2)
        {
            selected = new SelectedGenes[2];
            for (int i = 0; i < 2; i++)
            {
                selected[i].init(len,0);
            }
        }

        for (int d = 0; d < 2; d++)
        {
            DNAScript dna = s_input[_dnaIndex + d];
            dnaSelectionBounds = dna.renderer.bounds;
            if (dna.length != 0)
            {
                _geneIndex = Mathf.Clamp(_geneIndex, 0, dna.length - 4);
                Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                for (int i = 0; i < dna.topStrand.Length; i++)
                {

                    selected[d].firstSelected[i] = dna.topStrand[i];


                    min = Vector3.Min(selected[d].firstSelected[i].renderer.bounds.min, min);
                    max = Vector3.Max(selected[d].firstSelected[i].renderer.bounds.max, max);
                }
                selected[d].selectionBounds.SetMinMax(min, max);
            }
        }
    }
}