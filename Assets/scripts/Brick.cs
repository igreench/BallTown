using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brick : MonoBehaviour
{
    [HideInInspector]
    private static List<Brick> bricks = new List<Brick>();

    private static int deletedBricks = 0;

    private bool isScalingDown = false;
    private bool isScalingUp = false;
    private bool isRemovable = false;
    private bool isScalingConst = false;

    private int numberScalingSteps = 10;
    private int currentScalingSteps = 0;

    void Awake()
    {
        bricks.Add(this);
        transform.position -= new Vector3(0, transform.localScale.y, 0);
        isScalingUp = true;
    }

    public static void DestroyBricks()
    {
        foreach (Brick brick in bricks)
        {
            if (brick != null)
            {
                Destroy(brick.gameObject);
            }
        }
        bricks.Clear();
    }

    public static void AllDown()
    {
        foreach (Brick brick in bricks)
        {
            if (brick != null)
            {
                brick.isScalingConst = true;
                brick.isScalingDown = true;
            }
        }        
    }

    void Update()
    {
        if (isScalingUp)
        {
            currentScalingSteps++;
            transform.position += new Vector3(0, transform.localScale.y / (numberScalingSteps * 3), 0); //3???
            if (currentScalingSteps == numberScalingSteps * 3) //3???
            {
                currentScalingSteps = 0;
                if (!isScalingConst)
                {
                    isScalingUp = false;
                }
            }            
        }
        if (isScalingDown)
        {
            transform.localScale += new Vector3(0, -GM.k / numberScalingSteps, 0);
            transform.position += new Vector3(0, -GM.k / (numberScalingSteps * 2), 0);
            
            if (currentScalingSteps == numberScalingSteps)
            {
                if (!isScalingConst)
                {
                    isScalingDown = false;
                }
                currentScalingSteps = 0;
                if (isRemovable)
                {
                    Destroy(gameObject);
                    deletedBricks++;
                    if (deletedBricks == bricks.Count)
                    {
                        deletedBricks = 0;
                        bricks.Clear();
                        DestroyBricks();
                        GM.instance.LevelWin();
                    }
                }
            }
            currentScalingSteps++;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (!isScalingDown && !isScalingUp)
        {
            if (col.gameObject.name == "Ball")
            {
                GM.instance.CountCombo++;
                if (GM.instance.CountCombo % 5 == 0)
                {
                    GM.instance.Popup("COMBO");
                    AudioManager.instance.PlayCombo(GM.instance.CountCombo / 5);
                }
                isRemovable = false;
                isScalingDown = true;
                if (transform.localScale.y - GM.k > 0)
                {
                    GM.Score += 100;
                }
                else
                {
                    isRemovable = true;
                    GM.Score += 200;
                }
                GM.instance.SetTextGame();
                GM.instance.InitExplosion(transform.position);
            }
        }
    }
}
