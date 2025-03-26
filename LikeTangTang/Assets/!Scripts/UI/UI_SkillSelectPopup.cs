using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_SkillSelectPopup : UI_Base
{
    // [x] 스킬 팝업 그리드를 찾아서, 프리팹을 만들어 채워줘야함
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
        
        // [ ] 데이터에 따라서 바꿔줘야함.(하드코딩말고 쉽게 찾을 수 있는 방법이 있나 확인해보기.)
        for(int i =0; i<3; i++)
        {
            var go = Manager.ResourceM.Instantiate("UI_SkillCardItem.prefab", _pooling: false);
            UI_SkillCardItem item = go.GetOrAddComponent<UI_SkillCardItem>();

            item.transform.SetParent(_grid);

            _items.Add(item);
        }
    }
}
