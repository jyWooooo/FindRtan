using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    string adType;
    string gameId;
    void Awake()
    {
        instance = this;

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            adType = "Rewarded_iOS";
            gameId = "5464014";
        }
        else
        {
            adType = "Rewarded_Android";
            gameId = "5464015";
        }

        Advertisement.Initialize(gameId, true);
    }

    public void ShowRewardAd()
    {
        if (Advertisement.IsReady())
        {
            ShowOptions options = new ShowOptions { resultCallback = ResultAds };
            Advertisement.Show(adType, options);
        }
    }

    void ResultAds(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:
                Debug.LogError("���� ���⿡ �����߽��ϴ�.");
                break;
            case ShowResult.Skipped:
                Debug.Log("���� ��ŵ�߽��ϴ�.");
                break;
            case ShowResult.Finished:
                // ���� ���� ���� ��� 
                SceneManager.LoadScene("MainScene");
                Debug.Log("���� ���⸦ �Ϸ��߽��ϴ�.");
                break;
        }
    }
}
