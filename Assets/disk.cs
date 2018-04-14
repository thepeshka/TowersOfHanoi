using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class disk : MonoBehaviour {

    public int diskSize;
    public int towerID;
    public bool selected = false;
    public tower twr;
    Transform trans;
    gameGlobal game;
    // Use this for initialization
    void Start()
    {
        game = GameObject.Find("gameGlobal").GetComponent<gameGlobal>();
        //twr = game.RegisterNewDisk(this);
        trans = GetComponent<Transform>();
    }

    public void setTower(tower Twr)
    {
        twr = Twr;
    }

    public void moveToTower(tower trgtTwr)
    {
        if (trgtTwr.topDisc == null || trgtTwr.topDisc.diskSize > this.diskSize)
        {
            GameObject.Find("stepsLabel").GetComponent<Text>().text = "Steps: "+(++game.stepsNum).ToString();
            twr.RemoveDisk();
            trgtTwr.AddDisk(this);
            game.Animator.Animate(animator.AT_MOVE_TO_TOWER, this, trgtTwr);
            selected = false;
            game.checkWin();
        }
    }

    public bool ToggleSelect()
    {
        Debug.Log("Try to selected disk with size " + diskSize);
        if (twr.IsDiskOnTop(this))
        {
            Debug.Log("Disk with size " + diskSize + ". Select it now.");
            if (!selected)
            {
                game.Animator.Animate(animator.AT_SELECT_DISK, this);
            } else
            {
                game.Animator.Animate(animator.AT_DESELECT_DISK, this);
            }
            selected = !selected;
            return selected;
        }
        Debug.Log("Disk with size " + diskSize + " is not on top. Selection is canceled.");
        return selected;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
