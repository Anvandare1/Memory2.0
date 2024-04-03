using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private List<Card> cardlist = new List<Card>();
    private GameObject prefab;
    private Transform cardarea;
    private Sprite cardback;
    private string memory;
    private int indexmemory;
    private int remaingpairs;
    private bool canperformturn;
    public void Start()
    {
        canperformturn = true;
        memory = null;
        prefab = Resources.Load<GameObject>("Prefabs/Card");
        Card[] cards = Resources.LoadAll<Card>("Cards") as Card[];
        cardlist = cards.ToList();
        remaingpairs = cardlist.Count;
        cardarea = transform.GetChild(0);
        cardback = Resources.Load<Sprite>("Images/CardBack");

        for(int i = 0; i < cards.Length; i++)
        {
            cardlist.Add(cards[i]);
        }
        for (int i = 0; i < cardlist.Count; i++)
        {
            int j = Random.Range(i, cardlist.Count);
            Card temp = cardlist[i];
            cardlist[i] = cardlist[j];
            cardlist[j] = temp;
        }

        for(int i = 0; i < cardlist.Count; i++)
        {
            int index = i;
            //indexmemory = i;
            string tempid = cardlist[i].ID;
            GameObject currentcard = Instantiate(prefab);
            currentcard.transform.SetParent(cardarea);
            currentcard.GetComponent<Image>().sprite = cardback;
            currentcard.GetComponent<Button>().onClick.AddListener(delegate {Match(tempid, index); });
        }
    }

    void Update()
    {
        if(remaingpairs <= 0)
        {
            Debug.Log("Game Ended");
        }
    }


    public void Match(string id, int index)
    {
        if(canperformturn)
        {
            switch(memory)
        {
            case null:
               memory = id;
               indexmemory = index;
               cardarea.GetChild(index).GetComponent<Image>().sprite = cardlist[index].Image;
            break;

            default:
               if(id == memory && index != indexmemory)
               {
                   for(int i = 0; i < cardlist.Count; i++)
                   {
                   if(cardlist[i].ID == memory)
                   {
                       cardarea.GetChild(i).GetComponent<Button>().enabled = false;
                       cardarea.GetChild(i).GetComponent<Image>().enabled = false;
                       remaingpairs -= 1;
                   }
                   }
                   Debug.Log("Match " + id);
               }
               cardarea.GetChild(index).GetComponent<Image>().sprite = cardlist[index].Image;
               if(canperformturn)
               {
                   StartCoroutine(ResetTurn());
               }
               memory = null;
               indexmemory = -1;
            break;
        }
        }
    }

    IEnumerator ResetTurn()
    {
        canperformturn = false;
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < cardlist.Count; i++)
        {
            cardarea.GetChild(i).GetComponent<Image>().sprite = cardback;
        }
        canperformturn = true;
    }
}
