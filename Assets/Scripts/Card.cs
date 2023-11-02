using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip flipSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCard()
    {
        audioSource.PlayOneShot(flipSFX);
        GetComponent<Animator>().SetBool("isOpen", true);
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if (GameManager.instance.firstCard == null)
            GameManager.instance.firstCard = gameObject;
        else
        {
            GameManager.instance.secondCard = gameObject;
            GameManager.instance.IsMatched();
        }
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 0.5f);
    }

    void CloseCardInvoke()
    {
        GetComponent<Animator>().SetBool("isOpen", false);
        transform.Find("front").gameObject.SetActive(false);
        transform.Find("back").gameObject.SetActive(true);
    }

    public void DestroyCard()
    {
        Destroy(gameObject, 0.5f);
    }
}
