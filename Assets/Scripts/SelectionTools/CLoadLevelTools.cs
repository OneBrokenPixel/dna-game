using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class CLoadLevelTools
{

    public struct SLevel
    {
        public DNAScript[] InputDNA;
        public DNAScript GoalDNA;
    }


    public static SLevel[] LoadLevels( string filename )
    {
        TextAsset levelsText = Resources.Load(filename) as TextAsset;

        string[] levels = levelsText.text.Split('-');

        SLevel[] sLevels = new SLevel[levels.Length];

        for (int i = 0; i < levels.Length; i++ )
        {
            string[] dna = levels[i].Split('\n');
            
            sLevels[i].InputDNA = new DNAScript[dna.Length-1];
            for (int j = 0; j < dna.Length - 1; j++)
            {
                LoadDNA(dna[j], ref sLevels[i].InputDNA[j]);
            }
            LoadDNA(dna[dna.Length - 1], ref sLevels[i].GoalDNA);

        }
        return sLevels;
    }

    private static void LoadDNA(string dna, ref DNAScript InputDNA)
    {
        string[] t_b = dna.Split(',');
        InputDNA = new DNAScript();
        InputDNA.createDNA(t_b[0], t_b[1]);
        Debug.Log(t_b[0] + " " + t_b[1]);
    }

}
