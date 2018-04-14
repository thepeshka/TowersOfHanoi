using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tower : MonoBehaviour {
    public int towerID;
    public int diskCount;
    public KeyCode key;
    public bool isStart;
    Transform trans = null;
    bool selected = false;
    Stack<disk> disks = new Stack<disk>();
    public disk topDisc = null;
    gameGlobal game;
    // Use this for initialization
    void Start()
    {
        game = GameObject.Find("gameGlobal").GetComponent<gameGlobal>();
        trans = GetComponent<Transform>();
        //game.RegisterNewTower(this);
	}

    bool SetDisks(disk[] disksArray)
    {
        if (disksArray == null)
            return false;
        try
        {
            for (int i = 0; i < disksArray.Length; i++)
            {
                disks.Push(disksArray[i]);
            }
            topDisc = disksArray[disksArray.Length - 1];
            diskCount = disksArray.Length;
        }
        catch
        {
            return false;
        }
        return true;
    }

    public void AddDisk(disk dsk)
    {
        disks.Push(dsk);
        topDisc = dsk;
        dsk.towerID = towerID;
        dsk.twr = this;
        diskCount++;
    }

    public void RemoveDisk()
    {
        disks.Pop();
        diskCount--;
        if (disks.Count > 0)
            topDisc = disks.Peek();
        else
            topDisc = null;
    }

    Transform GetTransform()
    {
        return trans;
    }

    public bool IsDiskOnTop(disk dsk)
    {
        return dsk == topDisc;
    }

    bool ToggleSelection()
    {
        return topDisc.ToggleSelect();
    }

	// Update is called once per frame
	void Update () {
        if (!game.inputBlocked && !game.win && Input.GetKeyDown(key))
        { // if left button pressed...
            for (int i = 0; i < game.disks.Count; i++)
            {
                if (game.disks[i].selected && game.disks[i].twr != this)
                {
                    game.disks[i].moveToTower(this);
                    return;
                }
            }
            if (topDisc)
                topDisc.ToggleSelect();
        }
    }
}
