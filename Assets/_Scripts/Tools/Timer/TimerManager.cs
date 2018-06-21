using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerManager
{
    private static TimerManager instance;

    public static TimerManager Instance
    {
        get
        {
            if (instance == null)
                instance = new TimerManager();
            return instance;
        }
    }

    private Dictionary<int,Timer> timerDic;
    private int timerID;

    private TimerManager()
    {
        timerDic = new Dictionary<int, Timer>();
        timerID = 0;
    }

    /// <summary>
    /// 单次定时器 只执行一次自动销毁 
    /// </summary>
    public void AddOnceTimer(float intervalTime, bool isScale, Action callBack)
    {
        InitTimer(1, intervalTime, isScale, callBack);
    }

    /// <summary>
    /// 多次计时器 执行一定次数后销毁
    /// </summary>
    public void AddCountTimer(float intervalTime, bool isScale, Action callBack,int count)
    {
        InitTimer(count, intervalTime, isScale, callBack);
    }

    /// <summary>
    /// 循环定时器 不被销毁就会无限执行 
    /// 返回ID供使用者控制销毁
    /// </summary>
    public int AddRepeatTimer(float intervalTime, bool isScale, Action callBack)
    {
        InitTimer(-1, intervalTime, isScale, callBack);
        return this.timerID;
    }

    public void UpdateTimer(float deltaTime)
    {
        List<int> removeList = new List<int>();
        foreach (KeyValuePair<int,Timer> item in this.timerDic)
        {
            if (!item.Value.Update(deltaTime))
            {
                //removeList.Add(item.Key);
            }
        }

        //for (int i = 0; i < removeList.Count; ++i)
        //{
        //    this.timerDic.Remove(removeList[i]);
        //}
    }

    private void InitTimer(int count, float intervalTime, bool isScale, Action callBack)
    {
        Timer timer = new Timer(count, intervalTime, isScale, callBack);
        timerDic.Add(timerID, timer);
        timerID += 1;
    }

    private void DisposeTimer(int id)
    {
        if (this.timerDic.ContainsKey(id))
        {
            this.timerDic.Remove(id);
        }
        else
        {
            Debug.LogError(String.Format("Timer ID : {0} is not exist",id));
        }
    }
}
