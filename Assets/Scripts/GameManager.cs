using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text timeText;
    public GameObject gameOverWindow;
    public GameObject cardPrefab;
    float time;
    int matchCnt = 0;

    [HideInInspector] public GameObject firstCard;
    [HideInInspector] public GameObject secondCard;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip matchSFX;

    [SerializeField] List<float> bestTimes;
    [SerializeField] List<Text> bestTimeTexts;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();
        for (int i = 0; i < 16; i++)
        {
            var newCard = Instantiate(cardPrefab);
            newCard.transform.parent = GameObject.Find("Cards").transform;
            newCard.transform.position = new Vector3(i % 4 * 1.4f - 2.1f, i / 4 * 1.4f - 3f, 0);
            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timeText.text = time.ToString("N2");
        if (time > 30f && !Mathf.Approximately(Time.timeScale, 0f))
            GameOver();

        if (Input.GetKeyDown(KeyCode.A))
            AddBestTime();
        if (Input.GetKeyDown(KeyCode.S))
            SaveBestTimes();
        if (Input.GetKeyDown(KeyCode.D))
            LoadBestTimes();
    }

    public void IsMatched()
    {
        string firstName = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondName = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstName == secondName)
        {
            audioSource.PlayOneShot(matchSFX);
            firstCard.GetComponent<Card>().DestroyCard();
            secondCard.GetComponent<Card>().DestroyCard();
            matchCnt++;
            if (matchCnt == 8)
                Invoke("GameOver", 0.5f);
        }
        else
        {
            firstCard.GetComponent<Card>().CloseCard();
            secondCard.GetComponent<Card>().CloseCard();
        }

        firstCard = null;
        secondCard = null;
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        LoadBestTimes();
        if (time <= 30f)
            AddBestTime();
        SaveBestTimes();
        for (int i = 0; i < bestTimes.Count; i++)
            bestTimeTexts[i].text = bestTimes[i].ToString("N2");
        gameOverWindow.SetActive(true);
    }

    public void AddBestTime()
    {
        bestTimes.Add(time);
        bestTimes = bestTimes.OrderBy(item => item).ToList();
        while (bestTimes.Count > 5)
            bestTimes.Remove(bestTimes.Last());
    }

    public void SaveBestTimes()
    {
        string key = "bestTimes";
        string value = "";
        for (int i = 0; i < bestTimes.Count; i++)
            value += bestTimes[i].ToString("N2") + ',';
        value = value.TrimEnd(',');
        PlayerPrefs.SetString(key, value);
        Debug.Log("save : " + key + ":" + value);
    }

    public void LoadBestTimes()
    {
        List<float> res = new List<float>();
        string key = "bestTimes";
        string value = PlayerPrefs.GetString(key);
        Debug.Log("load : " + key + ":" + value);
        if (value.Length != 0)
        {
            var splits = value.Split(',');
            for (int i = 0; i < splits.Length; i++)
                res.Add(float.Parse(splits[i]));
            bestTimes = res;
        }
    }
}
