using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameService : GenericSingleton<GameService> 
{
    List<SlotData> serialisedItems { get; set; }

    JsonDataService dataService;
    string savePath = "/playerSlotSave.json";
    Dictionary<PlayerBodySlots, SlotData> purchasedSlots { get; set; }
    Dictionary<PlayerBodySlots, string> savedItems;
    Dictionary<PlayerBodySlots, SavableData> savedSlots { get; set; }
    [SerializeField] SO_MenuItemCollection defaultItems;
    [SerializeField] SO_MenuItemCollection allItems;

    [SerializeField] GameObject menuPlayer;
    private Mesh none;

    protected override void Awake()
    {
        base.Awake();
        if (instance != this )
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        dataService = new JsonDataService();
        purchasedSlots = new Dictionary<PlayerBodySlots, SlotData>();
        savedItems = new Dictionary<PlayerBodySlots, string>();
        savedSlots = new Dictionary<PlayerBodySlots, SavableData>();
        none = new Mesh();
    }
    private void Start()
    {
        loadPlayerSlots();
    }
    private void Update()
    {
        if(purchasedSlots != null)
            serialisedItems = purchasedSlots.Values.ToList();
    }
    
    private void savePlayerSlots()
    {
        dataService.saveData(savePath, savedItems);
    }

    private void loadPlayerSlots()
    {
        savedItems = dataService.loadData<Dictionary<PlayerBodySlots, string>>(savePath);
        if (savedItems == default)
        {
            savedItems = new Dictionary<PlayerBodySlots, string>();
            setDefaultPlayer();
        }
        else
        {
            savedSlotsToPurchasedSlots();
            setPlayerShopSlot(menuPlayer.GetComponent<PlayerShopSlots>());
        }
    }

    private void savedSlotsToPurchasedSlots()
    {
        foreach(KeyValuePair<PlayerBodySlots,string> pair in savedItems)
        {
            if (pair.Value == "none") 
            {
                purchasedSlots.Add(pair.Key, new SlotData("none", pair.Key, none, null));
                continue;
            }
            foreach (SO_MenuItem item in allItems.menuItems)
            {
                if (pair.Value == item.itemName) 
                {
                    purchasedSlots.Add(item.playerSlot, new SlotData(item));
                    break;
                }
            }
        }
    }

    public void setDefaultPlayer()
    {
        foreach (SO_MenuItem item in defaultItems.menuItems)
        {
            addToSlot(item.playerSlot, item);
        }
        setPlayerShopSlot(menuPlayer.GetComponent<PlayerShopSlots>());
        savePlayerSlots();
    }

    public void addToSlot(PlayerBodySlots slot, SO_MenuItem itemAdded)
    {
        if (purchasedSlots.ContainsKey(itemAdded.playerSlot))
        {
            if (purchasedSlots[slot].itemMesh == itemAdded.itemMesh)
            {
                purchasedSlots[slot].itemName = "none";
                purchasedSlots[slot].itemMesh = none;
                savedItems[slot] = "none";
            }
            else
            {
                purchasedSlots[slot].itemMesh = itemAdded.itemMesh;
                purchasedSlots[slot].itemName = itemAdded.itemName;
                savedItems[slot] = itemAdded.itemName;
            }
            Debug.Log($"modifying purchase slot : {slot} with {purchasedSlots[slot].itemName}");
        }
        else
        {
            purchasedSlots.Add(slot, new SlotData(itemAdded));
            savedItems.Add(slot, itemAdded.itemName);
            Debug.Log($"Adding New purchase slot : {slot} with {purchasedSlots[slot].itemName}");
        }
        savePlayerSlots();
    }

    public void setPlayerShopSlot(PlayerShopSlots playerShopSlots)
    {
        if(purchasedSlots != null)
        {
            foreach(SlotData data in purchasedSlots.Values)
            {
                playerShopSlots.setSlotItem(data);
            }
        }
    }

}

[Serializable]
public class SlotData
{
    public string itemName;
    public PlayerBodySlots playerSlot;
    public Mesh itemMesh;
    public Material itemMaterial;


    public SlotData(SO_MenuItem item) { setSlotDataFromSO(item); }
    public SlotData(string itemName, PlayerBodySlots playerSlot, Mesh itemMesh, Material itemMaterial)
    {
        this.itemName = itemName;
        this.playerSlot = playerSlot;
        this.itemMesh = itemMesh;
        this.itemMaterial = itemMaterial;
    }

    public void setSlotDataFromSO(SO_MenuItem item)
    {
        this.itemName = item.itemName;
        this.playerSlot = item.playerSlot;
        this.itemMesh = item.itemMesh;
        //this.itemMaterial = item.itemMaterial;
    }
}
public class SavableData
{
    public string itemName;
    public SavableData(string itemName)
    {
        this.itemName = itemName;
    }
    public void setSlotDataFromSO(SO_MenuItem item)
    {
        this.itemName = item.itemName;
    }
}
