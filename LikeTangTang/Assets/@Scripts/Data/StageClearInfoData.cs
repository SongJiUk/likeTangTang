using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageClearInfoData
{
    public int StageIndex =1;
    public int MaxWaveIndex = 0;
    public bool isOpenFirstBox = false;
    public bool isOpenSecondBox = false;
    public bool isOpenThirdBox = false;
    public bool isClear = false;

}
