using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameGlobal : MonoBehaviour {
    public float diskThickness;
    public GameObject diskPrefab;
    public int diskCount = 5;
    List<tower> towers = new List<tower>();
    public List<disk> disks = new List<disk>();
    public bool inputBlocked = false;
    public List<disk> registerQueue = new List<disk>();
    public animator Animator;
    List<GameObject> disksGO = new List<GameObject>();
    public int stepsNum = 0;
    Text timerText;
    float time = 0f;
	// Use this for initialization
	void Start () {
        timerText = GameObject.Find("timer").GetComponent<Text>();
        Animator = GetComponent<animator>();
        RegisterNewTower(GameObject.Find("tower_0").GetComponent<tower>());
        RegisterNewTower(GameObject.Find("tower_1").GetComponent<tower>());
        RegisterNewTower(GameObject.Find("tower_2").GetComponent<tower>());
        InitDisks(UIManager.diskNum);
    }
	
    public bool InitDisks(int diskCount)
    {
        int a = 0;
        while (GameObject.Find("diskPrefab(Clone)"))
        {
            Destroy(GameObject.Find("diskPrefab(Clone)"));
            if (a++ > 999)
            {
                Debug.Log("inf loop");
                break;
            }
        }
        disks.Clear();
        try
        {
            GameObject twr0 = GameObject.Find("tower_0");
            Vector3 pos = twr0.GetComponent<Transform>().position;
            GameObject twr1 = GameObject.Find("tower_1");
            GameObject twr2 = GameObject.Find("tower_2");
            float scale = 1f;
            float scaleInc = 0.6f / (diskCount - 1f);
            for (int i = diskCount-1; i >= 0; i--)
            {
                pos = new Vector3(pos.x, diskThickness * (diskCount - i - 1), pos.z);
                GameObject dsk = Instantiate(diskPrefab, pos, diskPrefab.GetComponent<Transform>().rotation);
                dsk.GetComponent<Transform>().localScale = new Vector3(scale, scale, dsk.GetComponent<Transform>().localScale.z);
                dsk.GetComponent<disk>().towerID = 0;
                dsk.GetComponent<disk>().diskSize = i;
                RegisterNewDisk(dsk.GetComponent<disk>());
                twr0.GetComponent<Transform>().localScale = new Vector3(twr0.GetComponent<Transform>().localScale.x, twr0.GetComponent<Transform>().localScale.y, twr0.GetComponent<Transform>().localScale.z + 0.2f);
                twr1.GetComponent<Transform>().localScale = new Vector3(twr1.GetComponent<Transform>().localScale.x, twr1.GetComponent<Transform>().localScale.y, twr1.GetComponent<Transform>().localScale.z + 0.2f);
                twr2.GetComponent<Transform>().localScale = new Vector3(twr2.GetComponent<Transform>().localScale.x, twr2.GetComponent<Transform>().localScale.y, twr2.GetComponent<Transform>().localScale.z + 0.2f);
                scale -= scaleInc;
            }
        } catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
        return true;
    }

    public void checkWin()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i].diskCount == disks.Count && !towers[i].isStart)
            {
                SceneManager.UnloadScene("main");
                SceneManager.LoadScene("mainMenu");
            }
        }
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
        time += Time.deltaTime;
        double min = Math.Floor(time / 60);
        double sec = time % 60;
        timerText.text = (min < 10 ? "0" : "") + ((int)min).ToString() + ":" + (sec < 10?"0":"") + sec.ToString("0.00");
    }
}
