using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    static Manager instance;
    static bool init = false;


    #region Contents
    GameManager gameM = new GameManager();
    ObjectManager objectM = new ObjectManager();
    PoolManager poolM = new PoolManager();

    public static GameManager GameM { get { return instance?.gameM; } }
    public static ObjectManager ObjectM { get { return instance?.objectM; } }
    public static PoolManager PoolM { get { return instance?.poolM; } }

    #endregion

    #region System

    DataManager dataM = new DataManager();
    ResourceManager resourceM = new ResourceManager();
    SceneManager sceneM = new SceneManager();
    SoundManager soundM = new SoundManager();
    UIManager uiM = new UIManager();

    public static DataManager DataM { get { return instance?.dataM; } }
    public static ResourceManager ResourceM {get { return instance?.resourceM; } }
    public static SceneManager SceneM { get { return instance?.sceneM; } }
    public static SoundManager SoundM { get { return instance?.soundM; } }
    public static UIManager UiM { get { return instance?.uiM; } }
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

}
