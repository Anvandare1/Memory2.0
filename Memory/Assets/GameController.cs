using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private bool multiplayer;
    private List<Card> cardlist = new List<Card>();
    private GameObject prefab;
    private GameObject prefab2;
    private GameObject prefab3;
    private GameObject Game;
    private GameObject WinScreen; 
    private GameObject TeamSelection;
    private Transform cardarea;
    private Transform cardcollection1;
    private Transform cardcollection2;
    private Sprite cardback;
    private string team1;
    private string team2;
    private string memory;
    private int indexmemory;
    private int remaingpairs;
    private int playerturn;
    private bool graceturn;
    private bool canperformturn;
    private Text turncoutner;
    public void Start()
    {
        graceturn = true;
        canperformturn = true;
        memory = null;
        prefab = Resources.Load<GameObject>("Prefabs/Card");
        prefab2 = Resources.Load<GameObject>("Prefabs/CollecedCard");
        prefab3 = Resources.Load<GameObject>("Prefabs/Button");
        Card[] cards = Resources.LoadAll<Card>("Cards") as Card[];
        cardlist = cards.ToList();
        remaingpairs = cardlist.Count;
        Game = transform.GetChild(0).gameObject;
        WinScreen = transform.GetChild(1).gameObject;
        TeamSelection = transform.GetChild(2).gameObject;
        cardarea = transform.GetChild(0).GetChild(0);
        cardcollection1 = transform.GetChild(0).GetChild(1).GetChild(1);
        cardcollection2 = transform.GetChild(0).GetChild(2).GetChild(1);
        turncoutner = transform.GetChild(0).GetChild(3).GetComponent<Text>();
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

        for (int i = 0; i < cardlist.Count; i++)
        {
           print("Create button");
           GameObject button = Instantiate(prefab);
           button.name = ""  + i;
           button.transform.SetParent(cardarea, false);

           int index = i;
           string tempid = cardlist[i].ID;
           button.GetComponent<Image>().sprite = cardback;
           button.GetComponent<Button>().onClick.AddListener(delegate {Match(tempid, index); });
        }
    }

    public void SelectTeam(int team)
    {
        Sprite cat = Resources.Load<Sprite>("Images/Cat");
        Sprite dog = Resources.Load<Sprite>("Images/Dog");
        switch(team)
        {
            case 0:
               transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().sprite = cat;
               team1 = "Katt";
               transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite = dog;
               team2 = "Hund";
            break;

            case 1:
               transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().sprite = dog;
               team1 = "Hund";
               transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite = cat;
               team2 = "Katt";
            break;

            default:
               SelectTeam(0);
            break;
        }
        TeamSelection.SetActive(false);
        Game.SetActive(true);

    }

    public void SelectMode(bool mutiplayer)
    {
        this.multiplayer = mutiplayer;
    }

    public void EndMenu(int index)
    {
        switch(index)
        {
            case 0:
               SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            break;

            case 1:
               SceneManager.LoadScene(0);
            break;
        }
    }
    public void Match(string id, int index)
    {
        bool correctmatch = false;
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
                      }
                   }

                   remaingpairs -= 1;
                   GameObject tempcard = Instantiate(prefab2);
                   tempcard.GetComponent<Image>().sprite = cardlist[index].Image;
                   Debug.Log("Match " + id);

                   switch(multiplayer)
                   {
                       case true:
                       correctmatch = true;
                       switch(playerturn)
                       {
                           case 0:
                              tempcard.transform.SetParent(cardcollection1); 
                           break;

                           case 1:
                              tempcard.transform.SetParent(cardcollection2);
                           break;
                       }
                       break;

                       case false:
                          tempcard.transform.SetParent(cardcollection1);
                       break;
                   }
               }
               cardarea.GetChild(index).GetComponent<Image>().sprite = cardlist[index].Image;
               if(canperformturn)
               {
                   StartCoroutine(ResetTurn());
               }
               memory = null;
               indexmemory = -1;

               if(multiplayer)
               {
                   switch(correctmatch)
                   {
                      case true:
                         graceturn = true;
                      break;

                      case false:
                         switch(graceturn)
                         {
                           case true:
                              graceturn = false;
                           break;
   
                           case false:
                              playerturn += 1;
                              graceturn = true;
                           break;
                         }
                      break;
                   }
                   playerturn = (int)Mathf.Repeat(playerturn, 2);
                   switch(playerturn)
                   {
                      case 0:
                         turncoutner.text = "Drag: Lag " + team1 + " (Spelare 1)";
                      break;

                      case 1:
                         turncoutner.text = "Drag: Lag " + team2 + " (Spelare 2)";
                      break;
                   }
               }
            break;
        }
        }
    }
    void Create()
    {
       for(int i = 0; i < 2; i++)
       { 
           print("Create button");
           GameObject button = Instantiate(prefab3);
           button.name = "" + i;
           button.transform.SetParent(WinScreen.transform, false);
           button.transform.position += new Vector3((button.GetComponent<RectTransform>().sizeDelta.x * i) + 160 * i,0,0);
       }

       transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = "Spela Igen";
       transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate {EndMenu(0);});

       transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text = "Återgå till huvudmenyn";
       transform.GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(delegate {EndMenu(1);});
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

        if(remaingpairs <= 0)
        {
            Game.SetActive(false);
            WinScreen.SetActive(true);
            Create();
        }
    }
}
