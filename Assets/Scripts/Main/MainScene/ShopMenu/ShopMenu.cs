using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] public Transform player;
    [SerializeField] Transform menuContents;
    [SerializeField] public SO_MenuItemCollection menuItemList;
    [SerializeField] ShopItem shopItemPrefab;
    private int rotationDirection = 1;
    [SerializeField,Range(0,1)] float minRotationSpeed;
    [SerializeField, Range(0, 1)] private float maxRotationSpeed;
    private float rotationSpeed = 0;

    private void Awake()
    {
        setUpMenuItems();
        rotationSpeed = maxRotationSpeed;
    }
    private void OnEnable()
    {
        player.gameObject.SetActive(true);
    }
    void Update()
    {
        if(playerPos.position != player.position)
        {
            player.position = Vector3.Lerp(player.transform.position, playerPos.position, 0.1f);
        }
        if(gameObject.activeSelf)
        {
            if (!player.gameObject.activeSelf)
            {
                player.gameObject.SetActive(true);
            }
            scrollPlayer();
        }
    }
    private void setUpMenuItems()
    {
        for(int i=0; i < menuItemList.menuItems.Count; i++) 
        {
            ShopItem shopItem = Instantiate(shopItemPrefab, menuContents).GetComponent<ShopItem>();
            shopItem.setItem(i, menuItemList.menuItems[i].itemName, menuItemList.menuItems[i].itemImage,transform);
            shopItem.GetComponent<Button>().onClick.AddListener(onButtonClick);
        }
    }

    public void onButtonClick()
    {
        rotationSpeed = maxRotationSpeed;
        rotationDirection = -rotationDirection;
        
    }

    public void scrollPlayer()
    {
        if (player.gameObject.activeSelf)
            if (rotationSpeed > minRotationSpeed)
            {
                rotationSpeed -= 0.007f;
            }
            else rotationSpeed = minRotationSpeed;
            //player.transform.localRotation = Quaternion.Lerp(player.localRotation,player.GetChild(2).localRotation, rotationSpeed);
            player.forward = Vector3.Lerp(player.forward, player.right* rotationDirection, rotationSpeed);
    }
}
