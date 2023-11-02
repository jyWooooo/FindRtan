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
                Debug.LogError("광고 보기에 실패했습니다.");
                break;
            case ShowResult.Skipped:
                Debug.Log("광고를 스킵했습니다.");
                break;
            case ShowResult.Finished:
                // 광고 보기 보상 기능 
                SceneManager.LoadScene("MainScene");
                Debug.Log("광고 보기를 완료했습니다.");
                break;
        }
    }
}
