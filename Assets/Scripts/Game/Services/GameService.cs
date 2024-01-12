using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameService : GenericSingleton<GameService> 
{
    List<SlotData> serialisedItems { get; set; }
    [SerializeField] Camera itemCaptureCam;
    [SerializeField] Transform itemCapturePos;
    [SerializeField] Material defaultItemMaterial;
    [SerializeField] float captureSetBack;
    [Header("Camera Positions")]
    [SerializeField] Transform handCamPos;
    [SerializeField] Transform faceCamPos;
    [SerializeField] Transform headCamPos;
    [SerializeField] Transform torsoCamPos;
    [SerializeField] Transform lowerCamPos;
    [SerializeField] Transform footCamPos;
    //[SerializeField] public List<Image> captures;
    Coroutine endOfFramCapture;
    public List<Texture2D> capturedImages = new();

    JsonDataService dataService;
    string savePath = "/playerSlotSave.json";
    Dictionary<PlayerBodySlots, SlotData> purchasedSlots { get; set; }
    Dictionary<PlayerBodySlots, string> savedItems;
    Dictionary<PlayerBodySlots, SavableData> savedSlots { get; set; }
    [SerializeField] MenuItemCollection_Data defaultItems;
    [SerializeField] MenuItemCollection_Data allItems;

    [SerializeField] public GameObject menuPlayer;
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
        if(itemCaptureCam!=null)
            endOfFramCapture = StartCoroutine(CaptureAllItemThumbnails());
    }
    private IEnumerator CaptureAllItemThumbnails()
    {
        GameObject item = new GameObject();
        item.AddComponent<MeshFilter>();
        item.AddComponent<MeshRenderer>();
        for (int i=0; i<allItems.menuItems.Count; i++)
        {
            item.GetComponent<MeshFilter>().sharedMesh = allItems.menuItems[i].itemMesh;
            //item.GetComponent<MeshRenderer>().material = defaultItemMaterial;
            item.GetComponent<MeshRenderer>().materials = new Material[]{ defaultItemMaterial,defaultItemMaterial };
            item.name = allItems.menuItems[i].itemName;
            item.transform.parent = itemCapturePos;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            Vector3 target = item.GetComponent<MeshRenderer>().bounds.center;
            //itemCaptureCam.transform.position = new Vector3(target.x,target.y,target.z - item.GetComponent<MeshRenderer>().bounds.size.x * captureSetBack);
            itemCaptureCam.transform.parent = getCamRoot(allItems.menuItems[i].playerSlot);
            itemCaptureCam.transform.localPosition = Vector3.zero;
            itemCaptureCam.transform.localRotation = Quaternion.identity;   
            yield return new WaitForEndOfFrame();
            SaveCameraView(itemCaptureCam, allItems.menuItems[i].itemName, 512, "/Art/Thumbnails");
        }
    }

    private Transform getCamRoot(PlayerBodySlots slot)
    {
        switch (slot)
        {
            case PlayerBodySlots.HeadHairSlot:
                return headCamPos;
            case PlayerBodySlots.HeadItemSlot:
                return headCamPos;
            case PlayerBodySlots.FaceItemSlot:
                return faceCamPos;
            case PlayerBodySlots.FaceHairSlot:
                return faceCamPos;
            case PlayerBodySlots.UpperSlot:
                return torsoCamPos;
            case PlayerBodySlots.HandSlot:
                return handCamPos;
            case PlayerBodySlots.LowerSlot:
                return lowerCamPos;
            case PlayerBodySlots.FootSlot:
                return footCamPos;
            default:
                return torsoCamPos;
        }
    }
    void SaveCameraView(Camera cam, string imageName, int size, string saveLoc)
    {
        RenderTexture screenTexture = new RenderTexture(size, size, 16);
        cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        cam.Render();
        Texture2D renderedTexture = new Texture2D(size, size);
        renderedTexture.ReadPixels(new Rect(0, 0, size, size), 0, 0);
        RenderTexture.active = null;
        byte[] byteArray = renderedTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + ($"{saveLoc}/{imageName}.png"), byteArray);
        capturedImages.Add(LoadTexture(Application.dataPath + ($"{saveLoc}/{imageName}.png")));
        capturedImages.Last().name= imageName;
        /*Image capture;*/
    }
    public Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
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
            foreach (MenuItem_Data item in allItems.menuItems)
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
        foreach (MenuItem_Data item in defaultItems.menuItems)
        {
            addToSlot(item.playerSlot, item, true);
        }
        setPlayerShopSlot(menuPlayer.GetComponent<PlayerShopSlots>());
        savePlayerSlots();
    }

    public void addToSlot(PlayerBodySlots slot, MenuItem_Data itemAdded, bool isDefault = false)
    {
        if (purchasedSlots.ContainsKey(itemAdded.playerSlot))
        {
            if (purchasedSlots[slot].itemMesh == itemAdded.itemMesh && !isDefault)
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


    public SlotData(MenuItem_Data item) { setSlotDataFromSO(item); }
    public SlotData(string itemName, PlayerBodySlots playerSlot, Mesh itemMesh, Material itemMaterial)
    {
        this.itemName = itemName;
        this.playerSlot = playerSlot;
        this.itemMesh = itemMesh;
        this.itemMaterial = itemMaterial;
    }

    public void setSlotDataFromSO(MenuItem_Data item)
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
    public void setSlotDataFromSO(MenuItem_Data item)
    {
        this.itemName = item.itemName;
    }
}
