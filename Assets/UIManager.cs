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
        //SceneManager.UnloadScene("mainMenu");
        //SceneManager.LoadScene("main");
        SceneManager.LoadScene("main");
    }

    public void toMainMenu()
    {
        //SceneManager.UnloadScene("main");
        //SceneManager.LoadScene("mainMenu");
        SceneManager.LoadScene("mainMenu");
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

    public static void showWinText()
    {
        Vector3 pos = GameObject.Find("win").GetComponent<RectTransform>().position;
        GameObject.Find("win").GetComponent<RectTransform>().position = new Vector3(pos.x,pos.y,0);
        GameObject.Find("win").GetComponent<Button>().interactable = true;
    }

    public static void hideWinText()
    {
        Vector3 pos = GameObject.Find("win").GetComponent<RectTransform>().position;
        GameObject.Find("win").GetComponent<RectTransform>().position = new Vector3(pos.x, pos.y, -5000);
        GameObject.Find("win").GetComponent<Button>().interactable = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
