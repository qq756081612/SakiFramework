using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.UI;

public class ContentView : MonoBehaviour
{
    private static LuaEnv luaEnv = new LuaEnv();
    private float luaGCTime = 0;
    private const float luaGCInterval = 1.0f;

    private Action luaAwake;
    private Action luaStart;
    private Action luaUpdate;
    private Action luaOnDestory;

    private LuaTable spriteEnv;

    void Awake()
    {
        spriteEnv = luaEnv.NewTable();

        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        spriteEnv.SetMetaTable(meta);
        meta.Dispose();

        spriteEnv.Set("self",this);

        TextAsset luaTxt = Resources.Load<TextAsset>("Lua/main.lua");
        luaEnv.DoString(luaTxt.text, "ContentView", spriteEnv);

        this.luaAwake = spriteEnv.Get<Action>("Awake");
        this.luaStart = spriteEnv.Get<Action>("Start");
        this.luaUpdate = spriteEnv.Get<Action>("Update");
        this.luaOnDestory = spriteEnv.Get<Action>("OnDestory");

        if(this.luaAwake != null)
        {
            this.luaAwake();
        }
    }


    // Use this for initialization
    void Start ()
    {
        if (luaStart != null)
        {
            luaStart();
        }
        ForTest();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(luaUpdate != null)
        {
            luaUpdate();
        }
        if (Time.time - this.luaGCTime >= luaGCInterval)
        {
            luaEnv.Tick();
            LuaBehaviour.lastGCTime = Time.time;
        }

        TimerManager.Instance.UpdateTimer(Time.deltaTime);
	}

    void OnDestroy()
    {
        if (luaOnDestory != null)
        {
            luaOnDestory();
        }

        luaAwake = null;
        luaStart = null;
        luaUpdate = null;
        luaOnDestory = null;
        luaEnv.Dispose();
    }

    private void ForTest()
    {
        //TimerManager.Instance.AddOnceTimer(3, false, () =>
        //  {
        //      Debug.LogError(luaEnv + "AddOnceTimer");
        //  });
        XMLTools.Test();
    }
}