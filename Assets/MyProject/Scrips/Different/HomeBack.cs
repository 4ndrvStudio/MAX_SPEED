using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HomeBack : MonoBehaviour
{
    public Transform panelBack;

    public void LoadHome()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void ClickBack()
    {
        panelBack.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void ClickResume()
    {
        panelBack.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
