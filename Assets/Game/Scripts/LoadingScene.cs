using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image loadingBarFill;

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }
    IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID);
        LoadingScreen.SetActive(true);
        while (!asyncLoad.isDone)
        {
            float progressBar = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingBarFill.fillAmount = progressBar;
            yield return null;
        }
    }
}
