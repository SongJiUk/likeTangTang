using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    /* ToDo : 여기서 스폰 관리하기
    */
    float spawnInterval = 1f;
    int maxMonsterCount = 100;
    Coroutine coUpdateMonsterSpawn;

    private void Start()
    {
        coUpdateMonsterSpawn =  StartCoroutine(UpdateSpawn());
    }

    void TrySpawn()
    {
        int monsterCount = Manager.ObjectM.mcSet.Count;
        if (monsterCount >= maxMonsterCount) return;

        MonsterController mc = Manager.ObjectM.Spawn<MonsterController>(Random.Range(0, 2));
        mc.transform.position = new Vector2(Random.RandomRange(-10, 10), Random.RandomRange(-10, 10));

    }
    IEnumerator UpdateSpawn()
    {
        while(true)
        {
            TrySpawn();
            yield return new WaitForSeconds(spawnInterval);
        }
        
    }
}
