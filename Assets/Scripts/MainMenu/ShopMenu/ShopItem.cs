using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [field : SerializeField] public Texture2D textureImage { get;  set; }
    [field: SerializeField] public Image itemImage { get; set; }
    [field : SerializeField] public TMP_Text itemText { get;  set; }

    private Transform shopMenuRef { get; set; }
    public int listIndex { get; private set; }

    public void setItem(int i, string itemName, Texture2D itemImage, Transform shopMenuRef)
    {
        listIndex= i;
        itemText.text = itemName;
        textureImage = itemImage;
        this.shopMenuRef= shopMenuRef;
    }

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(onButtonClick);
    }
    private void onButtonClick()
    {
        MenuItem_Data selectedItem = shopMenuRef.GetComponent<ShopMenu>().menuItemList.menuItems[listIndex];
        //shopMenuRef.GetComponent<ShopMenu>().player.GetComponent<PlayerShopSlots>().setSlotItem(selectedItem);
        GameService.instance.addToSlot(selectedItem.playerSlot, selectedItem);
        GameService.instance.setPlayerShopSlot(shopMenuRef.GetComponent<ShopMenu>().player.GetComponent<PlayerShopSlots>());
    }
}
