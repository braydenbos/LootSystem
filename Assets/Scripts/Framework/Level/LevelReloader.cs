using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelReloader : MonoBehaviour
{
    private bool _isReloading;
    [SerializeField] private int staticObjectsSceneIndex = 1;
    [SerializeField] private UnityEvent onReset = new UnityEvent();

    private void Start()
    {
        //Reset();
    }

    public void Reset()
    {
        if (_isReloading) return;
        onReset.Invoke();
        Unload();
        _isReloading = true;
    }

    private void Unload()
    {
        StartCoroutine(UnloadScene());
    }

    IEnumerator UnloadScene()
    {
        if (SceneManager.sceneCount > 1)
        {
            AsyncOperation ao = SceneManager.UnloadSceneAsync(staticObjectsSceneIndex);
            yield return ao;            
        }
        LoadScene();
        yield return new WaitForSeconds(1);
        _isReloading = false;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(staticObjectsSceneIndex, LoadSceneMode.Additive);
    }
}
