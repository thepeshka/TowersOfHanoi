using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static int diskNum = 5;

    private void Start()
    {
        diskNum = 5;
    }

    public void StartGame()
    {
        SceneManager.UnloadScene("mainMenu");
        SceneManager.LoadScene("main");
    }

    public void AddDisks()
    {
        diskNum++;
        GameObject.Find("disksNum").GetComponent<Text>().text = diskNum.ToString();
    }

    public void SubDisks()
    {
        if (diskNum > 3)
            diskNum--;
        GameObject.Find("disksNum").GetComponent<Text>().text = diskNum.ToString();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
