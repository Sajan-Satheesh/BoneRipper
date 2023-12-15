using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [field : SerializeField] private Image menuImage { get;  set; }
    [field : SerializeField] private TMP_Text itemText { get;  set; }

    private Transform shopMenuRef { get; set; }
    public int listIndex { get; private set; }

    public void setItem(int i, string itemName, Image itemImage, Transform shopMenuRef)
    {
        listIndex= i;
        itemText.text = itemName;
        menuImage = itemImage;
        this.shopMenuRef= shopMenuRef;
    }

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(onButtonClick);
    }
    private void onButtonClick()
    {
        SO_MenuItem selectedItem = shopMenuRef.GetComponent<ShopMenu>().menuItemList.menuItems[listIndex];
        //shopMenuRef.GetComponent<ShopMenu>().player.GetComponent<PlayerShopSlots>().setSlotItem(selectedItem);
        GameService.instance.addToSlot(selectedItem.playerSlot, selectedItem);
        GameService.instance.setPlayerShopSlot(shopMenuRef.GetComponent<ShopMenu>().player.GetComponent<PlayerShopSlots>());
    }
}
