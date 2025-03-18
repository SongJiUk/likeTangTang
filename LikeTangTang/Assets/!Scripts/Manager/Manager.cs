using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class Manager : MonoBehaviour
{

    //드래그 드랍방식이아닌 코드에서 생성
    static Manager instance;
    static bool init = false;


    //매니저를 분할해서 사용
    #region Contents
    GameManager gameM = new GameManager();
    ObjectManager objectM = new ObjectManager();
    PoolManager poolM = new PoolManager();

    public static GameManager GameM { get { return Instance?.gameM; } }
    public static ObjectManager ObjectM { get { return Instance?.objectM; } }
    public static PoolManager PoolM { get { return Instance?.poolM; } }

    #endregion

    //엔진쪽에서 사용하는 부분(모든 게임에서 똑같은 부분)
    #region System
    DataManager dataM = new DataManager();
    ResourceManager resourceM = new ResourceManager();
    SceneManager sceneM = new SceneManager();
    SoundManager soundM = new SoundManager();
    UIManager uiM = new UIManager();

    public static DataManager DataM { get { return Instance?.dataM; } }
    public static ResourceManager ResourceM {get { return Instance?.resourceM; } }
    public static SceneManager SceneM { get { return Instance?.sceneM; } }
    public static SoundManager SoundM { get { return Instance?.soundM; } }
    public static UIManager UiM { get { return Instance?.uiM; } }
    #endregion



    public static Manager Instance
    {
        get
        {
            if(init == false)
            {
                init = true;

                GameObject obj = GameObject.Find("!Manager");

                if(obj == null)
                {
                    obj = new GameObject() {name = "!Manager"};
                    obj.AddComponent<Manager>();
                }
                DontDestroyOnLoad(obj);
                instance = obj.GetComponent<Manager>();

            }
            return instance;
        }
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
