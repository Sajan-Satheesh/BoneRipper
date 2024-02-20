using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "item", menuName = "MainMenu/MenuItemData")]
public class MenuItem_Data : ScriptableObject
{
    public Texture2D itemImage;
    public string itemName;
    public Mesh itemMesh;
    public PlayerBodySlots playerSlot;
}
