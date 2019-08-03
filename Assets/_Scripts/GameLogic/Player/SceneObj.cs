using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    Idle = 0,
    Move = 1,
    Attack = 2,
}

public class SceneObj : MonoBehaviour
{
    protected Animator animator;

    private float timer;
    private int animType;

    private void Awake()
    {
        timer = 0;
        animType = 0;

        animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        //animator.SetInteger("State", 1);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1)
        {
            PlayerAnimation(animType);
            animType++;
            timer = 0;

            if (animType > 2)
            {
                animType = 0;
            }
        }
    }

    public void PlayerAnimation(int type)
    {
        animator.SetInteger("State", type);
        //if (type == AnimationType.Idle)
        //{
        //    animator.SetInteger("State", 0);
        //}
        //else if (type == AnimationType.Attack)
        //{
        //    animator.SetInteger("State", 1);
        //}
        //else if (type == AnimationType.Attack)
        //{
        //    animator.SetInteger("State", 2);
        //}
        //else
        //{
        //    animator.SetInteger("State", 0);
        //}
    }
}
