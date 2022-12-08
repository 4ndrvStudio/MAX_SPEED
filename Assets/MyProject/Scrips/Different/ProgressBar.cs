using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ProgressBar : MonoBehaviour
{
    public Transform WheelLoad;
    public string SceneName;
    public float fakeDuration;

    private float startTime;
    private AsyncOperation loadingOperation;

    public void StartLoadScene()
    {
        gameObject.SetActive(true);
        DontDestroyOnLoad(this);
        startTime = Time.unscaledTime;
        loadingOperation = SceneManager.LoadSceneAsync(SceneName);
        Time.timeScale = 0;
    }
    private void Update()
    {
        WheelLoad.Rotate(new Vector3(0, 0, 1), 200 * -Time.deltaTime);
        if (loadingOperation == null) return;
        float fakeProgess = (Time.unscaledTime - startTime) / fakeDuration;
        float finalProgress = Mathf.Min(fakeProgess, loadingOperation.progress);
        //progress.SetProgressValue(finalProgress);

        if (loadingOperation.isDone && finalProgress >= 1f)
        {
            FinishLoading();
        }
    }

    private void FinishLoading()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
    }

}
