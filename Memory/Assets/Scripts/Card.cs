using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Card", menuName = "Card", order = 0)]
public class Card : ScriptableObject 
{
    public string ID{get{return id;}}
    public Sprite Image{get{return image;}}
    [SerializeField] private string id;
    [SerializeField] private Sprite image;   
}

