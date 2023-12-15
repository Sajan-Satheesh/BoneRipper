using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "item", menuName = "MainMenu/MenuItem")]
public class SO_MenuItem : ScriptableObject
{
    public Image itemImage;
    public string itemName;
    public Mesh itemMesh;
    public PlayerBodySlots playerSlot;
}
