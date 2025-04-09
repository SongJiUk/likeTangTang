using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{   

    public void init()
    {
        Manager.GameM.CurrentMap = this;
    }

    public void MagneticFieldReduction()
    {
        if (Manager.GameM.CurrentWaveIndex > 7) return;

        Vector3 baseSize = Vector3.one * 20f;

        float reductionRate = (10 - Manager.GameM.CurrentWaveIndex) * 0.1f;
        Vector3 targetScale = baseSize * reductionRate;

    }
}
