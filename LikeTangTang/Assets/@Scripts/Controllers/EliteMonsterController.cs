using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class EliteMonsterController : MonsterController
{
    List<int> dropList;
    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
        InvokeMonsterData();
        transform.localScale = new Vector3(2f, 2f, 2f);
    }
    public override bool Init()
    {
        if (!base.Init()) return false;
        CreatureState = Define.CreatureState.Moving;

        Rigid.simulated = true;
        transform.localScale = new Vector3(2f, 2f, 2f);
        
        Skills = gameObject.GetOrAddComponent<SkillComponent>();
        objType = Define.ObjectType.EliteMonster;

        dropList = Manager.GameM.CurrentWaveData.EliteDropItemId;
        return true;
    }

    private void Start()
    {
        Init();
        InvokeMonsterData();
    }
    public override void OnDead()
    {
        base.OnDead();

        //TODO : 엘리트보스 잡고 난 후의 보상
        //Manager.GameM.player.Skills
        if (Manager.GameM.MissionDic.TryGetValue(Define.MissionTarget.EliteMonsterKill, out MissionInfo mission))
            mission.Progress++;
        
        Manager.GameM.TotalEliteMonsterKillCount++;

        DropItem();
    }
    
    void DropItem()
    {
        dropList = Manager.GameM.CurrentWaveData.EliteDropItemId;
        

        if(dropList.Count == 0) return;
        
        int randIndex = UnityEngine.Random.Range(0, dropList.Count);
        int randomDropId = dropList[randIndex];
       

        if(Manager.DataM.DropItemDic.TryGetValue(randomDropId, out Data.DropItemData dropItem))
        {
            Vector3 dropPos = transform.position;

            switch(dropItem.DropItemType)
            {
                case Define.DropItemType.Potion :
                    PotionController potion = Manager.ObjectM.Spawn<PotionController>(dropPos, _prefabName: Define.DROPITEMNAME);
                    potion.Init();
                    potion.SetInfo(dropItem);
                break;

                case Define.DropItemType.DropBox :
                    DropBoxController dropBox = Manager.ObjectM.Spawn<DropBoxController>(dropPos, _prefabName: Define.DROPITEMNAME);
                    dropBox.Init();
                    dropBox.SetInfo(dropItem);
                break;

                default :
                Debug.LogWarning($"[DropItem] 미등록 타입 발견 : {dropItem.DropItemType}");
                break;
            }
        }
    }
}
