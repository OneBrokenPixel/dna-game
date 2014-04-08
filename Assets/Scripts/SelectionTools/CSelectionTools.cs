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

    protected abstract void updateSelection(); // abstact member function that updates the selected array.

}

public class Rule1Selector : CSelectionTools
{

    protected override void updateSelection()
    {
        if( selected.Length != 1)
        {
            selected = new SelectedGenes[1];
            selected[0].topStrand = new GeneScript[4];
            selected[0].bottomStrand = new GeneScript[4];
        }

        _dnaIndex = Mathf.Clamp(_dnaIndex, 0, s_input.Length);

        DNAScript dna = s_input[_dnaIndex];

        _geneIndex = Mathf.Clamp(_geneIndex, 0, dna.Length-4);

        for( int i = _geneIndex; i < dna.Length-4; i++)
        {
            selected[0].topStrand[i] = dna.topStrand[i];
            selected[0].bottomStrand[i] = dna.bottomStrand[i];
        }

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