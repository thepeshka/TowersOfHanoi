using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameGlobal : MonoBehaviour {
    public float diskThickness;
    List<tower> towers = new List<tower>();
    public List<disk> disks = new List<disk>();
    public bool inputBlocked = false;
    public List<disk> registerQueue = new List<disk>();
    public animator Animator;
	// Use this for initialization
	void Start () {
        Animator = GetComponent<animator>();
        RegisterNewTower(GameObject.Find("tower_0").GetComponent<tower>());
        RegisterNewTower(GameObject.Find("tower_1").GetComponent<tower>());
        RegisterNewTower(GameObject.Find("tower_2").GetComponent<tower>());
        RegisterNewDisk(GameObject.Find("disk_2").GetComponent<disk>());
        RegisterNewDisk(GameObject.Find("disk_1").GetComponent<disk>());
        RegisterNewDisk(GameObject.Find("disk_0").GetComponent<disk>());
    }
	
    public tower RegisterNewDisk(disk dsk)
    {
        tower twr = GetTowerById(dsk.towerID);
        if (twr != null)
        {
            Debug.Log("Regester new disk (" + dsk.ToString() + ") with size " + dsk.diskSize);
            twr.AddDisk(dsk);
            disks.Add(dsk);
            return twr;
        }
        registerQueue.Add(dsk);
        Debug.Log("Can register new disk (" + dsk.ToString() + ") with size " + dsk.diskSize + " because tower with id " + dsk.towerID + " is not exists;");
        return null;
    }

    public void RegisterNewTower(tower twr)
    {
        Debug.Log("Regester new tower (" + twr.ToString() + ") with id " + twr.towerID);
        towers.Add(twr);
        List<disk> toDel = new List<disk>();
        for (int i = 0; i < registerQueue.Count; i++)
        {
            if (registerQueue[i].towerID == twr.towerID)
            {
                toDel.Add(registerQueue[i]);
                twr.AddDisk(registerQueue[i]);
                disks.Add(registerQueue[i]);
                registerQueue[i].setTower(twr);
                Debug.Log("Regester new disk (" + registerQueue[i].ToString() + ") with size " + registerQueue[i].diskSize);
            }
        }
        for (int i = 0; i < toDel.Count; i++)
        {
            registerQueue.Remove(toDel[i]);
        }
    }

    public tower GetTowerById(int towerID)
    {
        for (int i=0; i < towers.Count; i++)
        {
            if (towers[i].towerID == towerID)
            {
                Debug.Log("Get tower class for towerID " + towerID);
                return towers[i];
            }
        }

        Debug.Log("Cant find tower class for towerID " + towerID);
        return null;
    }

    public Vector3 GetTowerPosById(int towerID)
    {
        Debug.Log("Get tower position for towerID " + towerID);
        return towers[towerID].GetComponent<Transform>().position;
    }

    public void blockInput(bool state)
    {
        if (state != inputBlocked)
        {
            inputBlocked = state;
            if (state)
            {
                Debug.Log("Input is disabled");
            } else
            {
                Debug.Log("Input is enabled");
            }
        }
    }

	// Update is called once per frame
	void Update () {
    }
}
