using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ClearData
{
    public int StageNumber;
    public int GetStar;

    public ClearData(int mapNo, int getStar)
    {
        StageNumber = mapNo;
        GetStar = getStar;
    }
}
