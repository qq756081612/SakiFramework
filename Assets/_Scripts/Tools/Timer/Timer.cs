using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer
{
    private int count;          //执行回调的次数 不限次数用-1表示
    private float intervalTime; //间隔时间
    private float leftTime;     //剩余时间
    private float initTime;     //Timer初始化时的时间
    private bool isScale;     //是否不受时间缩放影响
    private Action timeHandler; //回调函数

    public Timer(int count,float intervalTime,bool isUnScale,Action callBack)
    {
        this.count = count;
        this.intervalTime = intervalTime;
        this.leftTime = intervalTime;
        this.initTime = Time.realtimeSinceStartup;
        this.isScale = isUnScale;
        this.timeHandler = callBack;
    }

    public void Init(int count, float intervalTime, bool isUnScale, Action callBack)
    {
        this.count = count;
        this.intervalTime = intervalTime;
        this.leftTime = intervalTime;
        this.initTime = Time.realtimeSinceStartup;
        this.isScale = isUnScale;
        this.timeHandler = callBack;
    }

    /// <summary>
    /// 若返回真 则继续保留定时器实例 返回假 则删除定时器
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    public bool Update(float deltaTime)
    {
        if (this.timeHandler == null)
        {
            return false;
        }

        
        if (this.isScale == true)
        {
            //如果受时间缩放影响 用剩余时间判断 因为deltaTime会受Time.timeScale控制
            this.leftTime -= deltaTime;

            if (this.leftTime > 0)
            {
                return true;
            }
        }
        else
        {
            //如果不受时间缩放影响 用Time.realtimeSinceStartup判断 这个值不受Time.timeScale影响
            if (this.intervalTime > Time.realtimeSinceStartup - this.initTime)
            {
                return true;
            }
        }

        //时间到达了间隔时间再接着判断次数

        this.timeHandler();
        
        //count小于0为重复定时器
        if (this.count < 0)
        {
            return true;
        }
        else
        {
            this.count -= 1;
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void Dispose()
    {
        this.count = 0;
        this.intervalTime = 0;
        this.leftTime = 0;
        this.initTime = 0;
        this.isScale = false;
        this.timeHandler = null;
    }
}
