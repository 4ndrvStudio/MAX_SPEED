using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarSelection : MonoBehaviour
{
    public GameObject Table;
    public GameObject[] cars;
    public int currentCar;
    public bool inGamePlayScene = false;

    void Start()
    {
        int selectedCar = PlayerPrefs.GetInt("SelectedCarID");
        if (inGamePlayScene == true)
        {
            cars[selectedCar].SetActive(true);
            currentCar = selectedCar;
        }
    }

    void Update()
    {
        Table.transform.Rotate(Vector3.up * 10 * Time.deltaTime);
    }

    public void Right()
    {

        if (currentCar <= cars.Length - 1)
        {
            if (currentCar == cars.Length - 1)
                currentCar = 0;
            else
                currentCar += 1;
            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].SetActive(false);
                cars[currentCar].SetActive(true);
            }
        }
    }

    public void Left()
    {

        if (currentCar > 0)
        {
            currentCar -= 1;
            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].SetActive(false);
                cars[currentCar].SetActive(true);
            }
        }
    }

    public void Select()
    {
        PlayerPrefs.SetInt("SelectedCarID", currentCar);
        SceneManager.LoadScene(1);
    }
}
