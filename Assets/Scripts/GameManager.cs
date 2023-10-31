using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text timeText;
    public GameObject EndText;
    public GameObject cardPrefab;
    float time;
    int matchCnt = 0;

    public GameObject firstCard;
    public GameObject secondCard;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();
        //int[] arr2 = new int[rtans.Length];
        //bool[] visited = new bool[rtans.Length];
        //for (int i = 0; i < rtans.Length; i++)
        //{
        //    while (true)
        //    {
        //        int randomIndex = Random.Range(0, arr2.Length);

        //        if (!visited[randomIndex])
        //        {
        //            arr2[randomIndex] = rtans[i];
        //            visited[randomIndex] = true;
        //            break;
        //        }
        //    }
        //}
        //rtans = arr2;
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
        if (time > 30f)
            GameOver();
    }

    public void IsMatched()
    {
        string firstName = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondName = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstName == secondName)
        {
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
        EndText.SetActive(true);
    }
}
