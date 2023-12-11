using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameService : GenericSingleton<GameService> 
{
    Dictionary<PlayerBodySlots, Mesh> purchasedSlots = new Dictionary<PlayerBodySlots, Mesh>();
    [SerializeField] List<Mesh> purchasedItems;
    protected override void Awake()
    {
        base.Awake();
        if (instance != this )
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        purchasedItems = purchasedSlots.Values.ToList();
    }

    public void addToPurchaseSlot(PlayerBodySlots slot, Mesh itemAdded)
    {
        if(purchasedSlots.ContainsKey(slot))
        {
            purchasedSlots[slot] = itemAdded;
        }
        else purchasedSlots.Add(slot, itemAdded);
    }

    public void setPlayerShopSlot(PlayerShopSlots playerShopSlots)
    {
        if(purchasedSlots != null)
        {
            foreach(KeyValuePair<PlayerBodySlots,Mesh> pair in purchasedSlots)
            {
                playerShopSlots.setSlotItem(pair.Key,pair.Value);
            }
        }
    }

}
