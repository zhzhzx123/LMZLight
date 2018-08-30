using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneManager  : Single<LoadSceneManager>
{ 
	
	public delegate void LoadSceneCallBack(params object[] obj_arr);
    public delegate void LoadingCallBack(float progress);

    private event LoadingCallBack loadingCallBack;//加载中的回调

    private LoadSceneMono load;

    #region 注册回调
    public void AddLoadingCallBack(LoadingCallBack callback)
    {
        loadingCallBack += callback;
    }

    public void RemoveLoadingCallBack(LoadingCallBack callback)
    {
        loadingCallBack -= callback;
    }

    #endregion

    #region 加载
    public LoadSceneManager()
	{
		GameObject obj = new GameObject ("LoadSceneManager");
		load = obj.AddComponent<LoadSceneMono> ();
		GameObject.DontDestroyOnLoad (obj);
	}

	public void LoadScene(SceneName sceneName, LoadSceneMode mode, LoadSceneCallBack callback)
	{
		load.StartCoroutine (Load(sceneName.ToString(), mode, callback));
	}

	public void LoadScene(SceneName sceneName, LoadSceneCallBack callback)
	{
		load.StartCoroutine (Load(sceneName.ToString(), LoadSceneMode.Single, callback));
	}

    public void LoadScene(SceneName sceneName, LoadSceneCallBack callback, bool isWait)
    {
        load.StartCoroutine(Load(sceneName.ToString(), LoadSceneMode.Single, callback, isWait));
    }

    public void LoadScene(SceneName sceneName)
	{
		load.StartCoroutine (Load(sceneName.ToString(), LoadSceneMode.Single, null));
	}

    public void LoadScene(SceneName sceneName, bool isWait)
    {
        load.StartCoroutine(Load(sceneName.ToString(), LoadSceneMode.Single, null, isWait));
    }

    public void LoadScene(int sceneIndex, LoadSceneMode mode, LoadSceneCallBack callback)
	{
		load.StartCoroutine (Load(((SceneName)sceneIndex).ToString(), mode, callback));
	}

	public void LoadScene(int sceneIndex, LoadSceneCallBack callback)
	{
		load.StartCoroutine (Load(((SceneName)sceneIndex).ToString(), LoadSceneMode.Single, callback));
	}

	public void LoadScene(int sceneIndex)
	{
		load.StartCoroutine (Load(((SceneName)sceneIndex).ToString(), LoadSceneMode.Single, null));
	}

	IEnumerator Load(string sceneName, LoadSceneMode mode, LoadSceneCallBack callback, bool isWait = true)
	{
        UIManager._Instance.OpenWindow(WindowName.LoadScene, UIType.Loading);
        AsyncOperation ao = SceneManager.LoadSceneAsync (sceneName, mode);
		ao.allowSceneActivation = false;
		float progress = 0f;
		while (!ao.isDone && progress < 1) { //是否加载完成
			//Debug.Log(ao.progress);
			if (ao.progress < 0.9f) {
				progress = Mathf.Lerp (progress, ao.progress, Time.deltaTime);
			} else {
				progress = Mathf.Lerp (progress, 1.05f, Time.deltaTime);
			}
            if (loadingCallBack != null)
            {
                loadingCallBack(progress > 1 ? 1 : progress);
            }
			yield return null;
		}
		ao.allowSceneActivation = true;
		yield return null;
		if (callback != null) {
			callback ();
		}
        loadingCallBack = null;
        if (isWait)
        {
            yield return new WaitForSeconds(0.5f);
        }
        UIManager._Instance.CloseWindow(WindowName.LoadScene);
    }

    #endregion


}
