using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    Coroutine coUpdateMonsterSpawn;

    public bool isStop {get; set;} = false;

    public void StartSpawn()
    {
        if(coUpdateMonsterSpawn == null)
        {
            coUpdateMonsterSpawn = StartCoroutine(CoUpdateSpawn());
        }
    }
    IEnumerator CoUpdateSpawn()
    {
        while(true)
        {
            if(Manager.GameM.CurrentWaveData.MonsterID.Count == 1)
            {
                //ID가 하나면 하나만 소환.
                for(int i =0; i<Manager.GameM.CurrentWaveData.OnceSpawnCount; i++)
                {
                    Vector2 pos = Utils.CreateMonsterSpawnPoint(Manager.GameM.player.transform.position);
                    Manager.ObjectM.Spawn<MonsterController>(pos, Manager.GameM.CurrentWaveData.MonsterID[0]);
                }
                yield return new WaitForSeconds(Manager.GameM.CurrentWaveData.SpawnInterval);
            }
            else
            {
                for(int i =0; i<Manager.GameM.CurrentWaveData.OnceSpawnCount; i++)
                {
                    Vector2 pos = Utils.CreateMonsterSpawnPoint(Manager.GameM.player.transform.position);
                    if(Random.value <= Manager.GameM.CurrentWaveData.FirstMonsterSpawnRate) 
                    {
                        Manager.ObjectM.Spawn<MonsterController>(pos, Manager.GameM.CurrentWaveData.MonsterID[0]);
                    }
                    else
                    {
                        int randIndex = Random.Range(1, Manager.GameM.CurrentWaveData.MonsterID.Count);
                        Manager.ObjectM.Spawn<MonsterController>(pos, Manager.GameM.CurrentWaveData.MonsterID[randIndex]);
                    }
                }
                yield return new WaitForSeconds(Manager.GameM.CurrentWaveData.SpawnInterval);
            }
        }
        
    }
}
