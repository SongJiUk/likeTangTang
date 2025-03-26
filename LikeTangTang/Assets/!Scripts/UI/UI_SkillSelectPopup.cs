using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_SkillSelectPopup : UI_Base
{
    // [ ] 스킬 팝업 그리드를 찾아서, 프리팹을 만들어 채워줘야함(리스트에 하던지, 딕셔너리에 하던지 알아서)
    [SerializeField]
    Transform _grid;

    List<UI_Base> _items = new List<UI_Base>();

    void Start()
    {
        PopulateGird();
    }

    void PopulateGird()
    {
        foreach(Transform tr in _grid.transform)
            Manager.ResourceM.Destory(tr.gameObject);
        
        // [ ] 데이터에 따라서 바꿔줘야함.
        for(int i =0; i<3; i++)
        {
            var go = Manager.ResourceM.Instantiate("UI_SkillCardItem.prefab", _pooling: false);
            UI_SkillCardItem item = go.GetOrAddComponent<UI_SkillCardItem>();

            item.transform.SetParent(_grid);

            _items.Add(item);
        }
    }
}
