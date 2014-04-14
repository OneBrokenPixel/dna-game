using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class CLoadLevelTools
{
    public struct SLevel
    {
        public struct SDNA
        {
            public string top;
            public string bottom;
        }

        public SDNA[] InputDNA;
        public SDNA GoalDNA;
    }

    /*
             string[] dna = levels[i].Split('\n');

            Debug.Log(dna.Length);

            sLevels[i].InputDNA = new SLevel.SDNA[dna.Length - 1];
            int j = 0;
            for ( j = 0; j < (dna.Length - 2); j++)
            {
                LoadDNA(dna[j], ref sLevels[i].InputDNA[j]);
            }
            LoadDNA(dna[j], ref sLevels[i].GoalDNA);
     */
    public static SLevel[] LoadLevels( string filename )
    {
        TextAsset levelsText = Resources.Load(filename) as TextAsset;
        string[] levels = levelsText.text.Replace("\n", "").Split('-');

        SLevel[] sLevels = new SLevel[levels.Length];

        for (int i = 0; i < levels.Length; i++ )
        {
            string[] dna = levels[i].Replace("\n", "").Split('/');
            sLevels[i].InputDNA = new SLevel.SDNA[dna.Length - 1];
            int j = 0;
            for (j = 0; j < dna.Length - 1; j++)
            {
                LoadDNA(dna[j], ref sLevels[i].InputDNA[j]);
            }
            LoadDNA(dna[j], ref sLevels[i].GoalDNA);
        }
        return sLevels;
    }

    private static void LoadDNA(string dna, ref SLevel.SDNA InputDNA)
    {
        string[] t_b = dna.Split(',');
        InputDNA.top = t_b[0].Replace("\u000D", "");
        InputDNA.bottom = t_b[1].Replace("\u000D", "");
    
    }
}
