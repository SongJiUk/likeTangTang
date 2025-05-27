using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HP_Bar : UI_Base, ITickable
{

    Slider slider;
    enum GameObjects
    {
        HPBar
    }

    private void OnDestroy()
    {
        Manager.UpdateM.Unregister(this);

    }
    //TDOO : Init어디서 해줄지 생각.
    public override bool Init()
    {
        if(!base.Init()) return false;
        Bind<GameObject>(typeof(GameObjects));

        slider = GetObject(typeof(GameObjects), (int)GameObjects.HPBar).GetComponent<Slider>();
        Manager.UpdateM.Register(this);
        return true;
    }
    public void Tick(float _deltatime)
    {
        transform.rotation = Camera.main.transform.rotation;

        float ratio = Manager.GameM.player.Hp / Manager.GameM.player.MaxHp;

        SetHpBar(ratio);
    }

    public void SetHpBar(float _ratio)
    {
        slider.value = _ratio;
    }
}
