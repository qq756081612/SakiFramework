using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassObjectPool<T> where T : class,new()
{
    protected Stack<T> pool = new Stack<T>();

    protected int maxNum = 0;

    protected int noRecycleNum = 0;

    public ClassObjectPool(int maxNum)
    {
        this.maxNum = maxNum;
        for (int i = 0; i < this.maxNum; ++i)
        {
            pool.Push(new T());
        }
    }

    public T Spawn(bool createIfPoolEmpty)
    {
        T res = null;
        if (pool.Count > 0)
        {
            res = pool.Pop();
            if (res == null)
            {
                if (createIfPoolEmpty)
                {
                    res = new T();
                }
            }
            else
            {
                res = null;
            }
        }
        else
        {
            if (createIfPoolEmpty)
            {
                res = new T();
            }
        }

        if(res != null)
        {
            noRecycleNum = noRecycleNum + 1;
        }

        return res;
    }

    public bool Recycle(T obj)
    {
        if (obj == null)
        {
            return false;
        }

        //这里待商榷
        noRecycleNum = noRecycleNum - 1;

        if(pool.Count >= maxNum)
        {
            obj = null;
            return false;
        }

        pool.Push(obj);
        return true;
    }
}
