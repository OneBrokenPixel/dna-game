using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[System.Serializable]
public abstract class CSelectionTools
{

    public static DNAScript[] s_input; // is reference to input array

    public static MainScript.Rule s_lastRule;

    public struct SelectedGenes // struct of selected genes
    {
        public GeneScript[] topStrand;
        public GeneScript[] bottomStrand;
    }

    public SelectedGenes[] selected; // array of selected genes;
                                     // one SelectedGenes Per strand of DNA selected;
                                     // rules 3 and 4 will have SelectedGenes[2]

    public Bounds dnaSelectionBounds = new Bounds();
    public Bounds geneSelectionBounds = new Bounds();

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
}

public class Rule1Selector : CSelectionTools
{
    protected override void updateSelection()
    {
        if (s_input == null)
        {
            return;
        }
        if (selected == null || selected.Length != 1)
        {
            selected = new SelectedGenes[1];
            selected[0].topStrand = new GeneScript[4];
            selected[0].bottomStrand = new GeneScript[4];
        }

        _dnaIndex = Mathf.Clamp(_dnaIndex, 0, s_input.Length);

        DNAScript dna = s_input[_dnaIndex];

        dnaSelectionBounds = dna.renderer.bounds;

        _geneIndex = Mathf.Clamp(_geneIndex, 0, dna.Length-4);
        Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        for (int i = 0; i < 4; i++)
        {

            selected[0].topStrand[i] = dna.topStrand[_geneIndex + i];
            selected[0].bottomStrand[i] = dna.bottomStrand[_geneIndex + i];


            min = Vector3.Min(selected[0].bottomStrand[i].renderer.bounds.min, min);
            max = Vector3.Max(selected[0].topStrand[i].renderer.bounds.max, max);
        }

        geneSelectionBounds.SetMinMax(min, max);

    }
}

public class Rule2Selector : CSelectionTools
{

    protected override void updateSelection()
    {
        throw new NotImplementedException();
    }
}

public class Rule3Selector : CSelectionTools
{

    protected override void updateSelection()
    {
        throw new NotImplementedException();
    }
}

public class Rule4Selector : CSelectionTools
{

    protected override void updateSelection()
    {
        throw new NotImplementedException();
    }
}