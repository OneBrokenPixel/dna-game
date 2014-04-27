using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[System.Serializable]
public class CSelectionTools
{

    public static DNAScript[] s_input; // is reference to input array

    public struct SelectedGenes // struct of selected genes
    {
        public GeneScript[] firstSelected;
        public GeneScript[] secondSelected;

        public void init(int top, int bottom)
        {
            firstSelected = new GeneScript[top];
            secondSelected = new GeneScript[bottom];
        }
    }


    public abstract class BaseRuleSelector
    {
        public SelectedGenes[] selected = new SelectedGenes[2]; // array of selected genes;
        public Vector3[] selectionPoints = new Vector3[2];
        // one SelectedGenes Per strand of DNA selected;
        // rules 3 and 4 will have SelectedGenes[2]

        //public Bounds dnaSelectionBounds = new Bounds();

        protected int _dnaIndex = 0; // current index of the dna in s_input;
        public int dnaIndex
        {
            get { return _dnaIndex; }
            set { _dnaIndex = value; updateSelection(); } // updates the selected array
        }

        protected int _geneIndex = 0; // current index of the genes in the selected dna;
        public int geneIndex
        {
            get { return _geneIndex; }
            set { _geneIndex = value; updateSelection(); } // updates selected array;
        }

        protected abstract void updateSelection(); // abstact member function that updates the selected array.

        public abstract void appyRule();

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

    public class Rule1Selector : BaseRuleSelector
    {
        protected override void updateSelection()
        {
            if (s_input == null)
            {
                selected = null;
                return;
            }

            {
                selected = new SelectedGenes[2];
                selected[0].init(4, 4);
                selected[1].init(0, 0);
            }

            _dnaIndex = Mathf.Clamp(_dnaIndex, 0, s_input.Length - 1);

            DNAScript dna = s_input[_dnaIndex];

            _geneIndex = Mathf.Clamp(_geneIndex, 0, dna.length - 4);


            Vector3 center = new Vector3();

            for (int i = 0; i < 4; i++)
            {

                selected[0].firstSelected[i] = dna.topStrand[_geneIndex + i];
                selected[0].secondSelected[i] = dna.bottomStrand[_geneIndex + i];

                center += dna.topStrand[_geneIndex + i].transform.position / 8;
                center += dna.bottomStrand[_geneIndex + i].transform.position / 8;

            }

            selectionPoints[0] = center;
            selectionPoints[1] = center;
        }

        public override void appyRule()
        {
            for( int i = 0; i < selected[0].firstSelected.Length; i++)
            {
                char t = selected[0].firstSelected[i].colour;
                selected[0].firstSelected[i].changeType(selected[0].secondSelected[i].colour);
                selected[0].secondSelected[i].changeType(t);
            }
        }
    }

    public class Rule2Selector : BaseRuleSelector
    {

        protected override void updateSelection()
        {
            if (s_input == null)
            {
                selected = null;
                return;
            }

            {
                selected = new SelectedGenes[2];
                selected[0].init(8, 8);
                selected[1].init(0, 0);
            }

            _dnaIndex = Mathf.Clamp(_dnaIndex, 0, s_input.Length - 1);

            DNAScript dna = s_input[_dnaIndex];

            _geneIndex = Mathf.Clamp(_geneIndex, 0, dna.length - 8);
            //Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            Vector3 left = new Vector3(), right = new Vector3();
            int i = 0;
            for (; i < 4; i++)
            {

                selected[0].firstSelected[i] = dna.topStrand[_geneIndex + i];
                selected[0].secondSelected[i] = dna.bottomStrand[_geneIndex + i];

                left += dna.topStrand[_geneIndex + i].transform.position / 8;
                left += dna.bottomStrand[_geneIndex + i].transform.position / 8;

                //min = Vector3.Min(selected[0].secondSelected[i].renderer.bounds.min, min);
                //max = Vector3.Max(selected[0].firstSelected[i].renderer.bounds.max, max);
            }
            for (; i < 8; i++)
            {

                selected[0].firstSelected[i] = dna.topStrand[_geneIndex + i];
                selected[0].secondSelected[i] = dna.bottomStrand[_geneIndex + i];

                right += dna.topStrand[_geneIndex + i].transform.position / 8;
                right += dna.bottomStrand[_geneIndex + i].transform.position / 8;

                //min = Vector3.Min(selected[0].secondSelected[i].renderer.bounds.min, min);
                //max = Vector3.Max(selected[0].firstSelected[i].renderer.bounds.max, max);
            }
            //selected[0].selectionBounds.SetMinMax(min, max);


            selectionPoints[0] = left;
            selectionPoints[1] = right;

        }

        public override void appyRule()
        {
            int len = selected[0].firstSelected.Length-1;
            int hLen = selected[0].firstSelected.Length/2;
            for(int start = 0, end = len; start < hLen; start++, end--)
            {
                char t = selected[0].firstSelected[start].colour;
                selected[0].firstSelected[start].changeType(selected[0].firstSelected[end].colour);
                selected[0].firstSelected[end].changeType(t);

                t = selected[0].secondSelected[start].colour;
                selected[0].secondSelected[start].changeType(selected[0].secondSelected[end].colour);
                selected[0].secondSelected[end].changeType(t);
            }
        }
    }

    public class Rule3Selector : BaseRuleSelector
    {
        protected override void updateSelection()
        {
            if (s_input == null || s_input.Length < 2)
            {
                selected = null;
                return;
            }
            {
                selected = new SelectedGenes[2];
                selected[0].init(4, 4);
                selected[1].init(4, 4);
            }

            _dnaIndex = Mathf.Clamp(_dnaIndex, 0, s_input.Length - 2);

            Vector3 center = new Vector3();

            for (int d = 0; d < 2; d++)
            {
                DNAScript dna = s_input[_dnaIndex + d];

                if (dna.length != 0)
                {
                    _geneIndex = Mathf.Clamp(_geneIndex, 0, dna.length - 4);
                    
                    //Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                    
                    for (int i = 0; i < 4; i++)
                    {

                        selected[d].firstSelected[i] = dna.topStrand[_geneIndex + i];
                        selected[d].secondSelected[i] = dna.bottomStrand[_geneIndex + i];

                        center += dna.topStrand[_geneIndex + i].transform.position / 16;
                        center += dna.bottomStrand[_geneIndex + i].transform.position / 16;

                        //min = Vector3.Min(selected[d].secondSelected[i].renderer.bounds.min, min);
                        //max = Vector3.Max(selected[d].firstSelected[i].renderer.bounds.max, max);
                    }
                    //selected[d].selectionBounds.SetMinMax(min, max);
                }
            }

            selectionPoints[0] = center;
            selectionPoints[1] = center;
        }

        public override void appyRule()
        {
            for (int i = 0; i < selected[0].firstSelected.Length; i++)
            {
                char t = selected[0].firstSelected[i].colour;
                selected[0].firstSelected[i].changeType(selected[1].secondSelected[i].colour);
                selected[1].secondSelected[i].changeType(t);

                t = selected[0].secondSelected[i].colour;
                selected[0].secondSelected[i].changeType(selected[1].firstSelected[i].colour);
                selected[1].firstSelected[i].changeType(t);

            }
        }
    }

    public class Rule4Selector : BaseRuleSelector
    {

        protected override void updateSelection()
        {
            if (s_input == null || s_input.Length < 2)
            {
                selected = null;
                return;
            }

            _dnaIndex = Mathf.Clamp(_dnaIndex, 0, s_input.Length - 2);
            _geneIndex = Mathf.Clamp(_geneIndex, 0, 1);

            int len = s_input[_dnaIndex].length;

            selected = new SelectedGenes[2];
            if (_geneIndex == 0)
            {
                selected[0].init(len, 0);
                selected[1].init(len, 0);
            }
            else
            {

                selected[0].init(0, len);
                selected[1].init(0, len);
            }
          

            for (int d = 0; d < 2; d++)
            {
                DNAScript dna = s_input[_dnaIndex + d];

                Vector3 center = new Vector3();

                if (dna.length != 0)
                {

                    if (_geneIndex == 0)
                        for (int i = 0; i < dna.topStrand.Length; i++)
                        {
                            selected[d].firstSelected[i] = dna.topStrand[i];

                            center += dna.topStrand[i].transform.position / dna.topStrand.Length;
                        }
                    else
                        for (int i = 0; i < dna.bottomStrand.Length; i++)
                        {
                            selected[d].secondSelected[i] = dna.bottomStrand[i];

                            center += dna.bottomStrand[i].transform.position / dna.bottomStrand.Length;
                        }

                }

                selectionPoints[d] = center;
            }
        }

        public override void appyRule()
        {
            if( _geneIndex == 0)
                for (int i = 0; i < selected[0].firstSelected.Length; i++)
                {
                    char t = selected[0].firstSelected[i].colour;
                    selected[0].firstSelected[i].changeType(selected[1].firstSelected[i].colour);
                    selected[1].firstSelected[i].changeType(t);
                }
            else
                for (int i = 0; i < selected[0].secondSelected.Length; i++)
                {
                    char t = selected[0].secondSelected[i].colour;
                    selected[0].secondSelected[i].changeType(selected[1].secondSelected[i].colour);
                    selected[1].secondSelected[i].changeType(t);
                }
        }
    }


    public static  BaseRuleSelector[] rules = { new Rule1Selector(), new Rule2Selector(), 
                                                new Rule3Selector(), new Rule4Selector() };
}

