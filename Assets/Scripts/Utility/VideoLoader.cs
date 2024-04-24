using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoLoader : Singleton<VideoLoader>
{
    /* https://drive.google.com/uc?export=download&id=DRIVE_FILE_ID
     * https://drive.google.com/file/d/1-OK-ibBwEYcx_8fyFvTXgmZgG9INzaP6/view?usp=sharing
     *
     * https://drive.google.com/uc?export=download&id=1-OK-ibBwEYcx_8fyFvTXgmZgG9INzaP6
     */

    public string MUrl = "";
    public bool mClearCache = false;
    private VideoPlayer mPlayer = null;
    private AssetBundle mBundle = null;

    [SerializeField]
    private UnityEvent finishedEvent;

    private void Awake()
    {
        mPlayer = GetComponent<VideoPlayer>();
        mPlayer.loopPointReached += endVideoReached;

        Caching.compressionEnabled = false;

        if (mClearCache)
            ClearCache();

        // start downloading assets from drive
        StartCoroutine(download());

        finishedEvent.AddListener(finishedVideoVoid);
    }

    public void ClearCache()
    {
        mBundle = null;
        mPlayer.clip = null;

        if (Caching.ClearCache())
        {
            Debug.Log("Successfully cleaned the cache");
        }
        else
        {
            Debug.Log("Cache is being used");
        }
    }

    private void finishedVideoVoid()
    {
        //Debug.Log($"{mPlayer.clip.name} finished playing");
        MainMenuUI.Instance.setVideoPanel(false);
    }

    private IEnumerator download()
    {
        LoadingHandler.Instance.ShowLoading();
        WWW request = WWW.LoadFromCacheOrDownload(MUrl, 0);

        while (!request.isDone)
        {
            LoadingHandler.Instance.Progress(request.progress);
            yield return null;
        }

        if (request.error == null)
        {
            mBundle = request.assetBundle;
            LoadingHandler.Instance.FinishedLoading();

            Debug.Log($"Downloading Success! {request.assetBundle}");
        }
        else
        {
            LoadingHandler.Instance.FailLoading(request.error);

            Debug.Log($"Downloading Failed because {request.error}");
        }

        yield return null;
    }

    public void PlayDone(string videoName, UnityAction doneAction = null)
    {
        if (string.IsNullOrEmpty(videoName))
        {
            Debug.Log("VIDEO LOADER : string is empty");
            return;
        }

        if (!mBundle)
        {
            Debug.Log("VIDEO LOADER : Bundle Failed to load");

            doneAction?.Invoke();
            return;
        }

        VideoClip loaded = mBundle.LoadAsset<VideoClip>(videoName);

        if (loaded == null)
        {
            Debug.Log($"VIDEO LOADER : Cannot find {videoName} in bundle");

            doneAction?.Invoke();
            return;
        }

        mPlayer.clip = loaded;

        MainMenuUI.Instance.setVideoPanel(true);
        finishedEvent.AddListener(doneAction);

        mPlayer.Play();
    }

    public void Play(string videoName) //testing
    {
        if (!mBundle)
        {
            Debug.Log("BUNDLE FAILED TO LOAD");
            return;
        }

        mPlayer.clip = mBundle.LoadAsset<VideoClip>(videoName);
        MainMenuUI.Instance.setVideoPanel(true);
        mPlayer.Play();
    }

    private void endVideoReached(VideoPlayer source)
    {
        finishedEvent?.Invoke();
        finishedEvent.RemoveAllListeners();

        finishedEvent.AddListener(finishedVideoVoid);
    }
}