using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonSwitcher : MonoBehaviour
{
    public float duration = 1.5f;
    public int sceneIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(duration);
        LoadScene();
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
}
