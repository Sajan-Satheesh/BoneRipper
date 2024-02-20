using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] public Transform player;
    [SerializeField] Transform menuContents;
    [SerializeField] Button reset;
    [SerializeField] public MenuItemCollection_Data menuItemList;
    [SerializeField] ShopItem shopItemPrefab;
    [SerializeField] Material itemMaterial;
    private List<Sprite> itemSprites = new();
    private int rotationDirection = 1;
    [SerializeField,Range(0,1)] float minRotationSpeed;
    [SerializeField, Range(0, 1)] private float maxRotationSpeed;
    private float rotationSpeed = 0;
    private List<ShopItem> shopItems = new();

    private void Awake()
    {
        setUpMenuItems();
        reset.onClick.AddListener(GameService.instance.setDefaultPlayer);
        rotationSpeed = maxRotationSpeed;
    }
    private void OnEnable()
    {
        player.gameObject.SetActive(true);
        setUpMenuImages();
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
    private void setUpMenuImages()
    {
        foreach(ShopItem shopItem in shopItems)
        {
            for (int j = 0; j < GameService.instance.capturedImages.Count; j++)
            {
                if (GameService.instance.capturedImages[j].name == shopItem.itemText.text)
                {
                    shopItem.textureImage = GameService.instance.capturedImages[j];
                    Sprite itemSprite = Sprite.Create(shopItem.textureImage, new Rect(0, 0, shopItem.textureImage.width, shopItem.textureImage.height), new Vector2(0.5f, 0.5f),100f,0,SpriteMeshType.FullRect);
                    itemSprite.name= GameService.instance.capturedImages[j].name;
                    shopItem.itemImage.name = GameService.instance.capturedImages[j].name;
                    itemSprites.Add(itemSprite);
                    shopItem.itemImage.material = itemMaterial;
                    //AssetDatabase.CreateAsset(itemSprite, Application.dataPath + ($"/Thumbnails"));

                    shopItem.itemImage.sprite = itemSprite;
                }
            }
        }
    }
    private void setUpMenuItems()
    {
        for(int i=0; i < menuItemList.menuItems.Count; i++) 
        {
            ShopItem shopItem = Instantiate(shopItemPrefab, menuContents).GetComponent<ShopItem>();
            shopItem.setItem(i, menuItemList.menuItems[i].itemName, menuItemList.menuItems[i].itemImage, transform);
            
            shopItem.GetComponent<Button>().onClick.AddListener(onButtonClick);
            shopItems.Add(shopItem);
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
