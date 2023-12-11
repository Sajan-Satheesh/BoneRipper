using UnityEngine;

public class PlayerShopSlots : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer HeadHairSlot;
    [SerializeField] SkinnedMeshRenderer HeadItemSlot;
    [SerializeField] SkinnedMeshRenderer FaceItemSlot;
    [SerializeField] SkinnedMeshRenderer FaceHairSlot;
    [SerializeField] SkinnedMeshRenderer UpperSlot;
    [SerializeField] SkinnedMeshRenderer HandSlot;
    [SerializeField] SkinnedMeshRenderer LowerSlot;
    [SerializeField] SkinnedMeshRenderer FootSlot;

    public void setSlotItem(SO_MenuItem item)
    {
        SkinnedMeshRenderer slotToInsert;
        getDesiredSlot(item.playerSlot,out slotToInsert);
        insertInSlot(item.item, slotToInsert);
    }

    public void setSlotItem(PlayerBodySlots slot, Mesh mesh)
    {
        SkinnedMeshRenderer slotToInsert;
        getDesiredSlot(slot, out slotToInsert);
        insertInSlot(mesh, slotToInsert);
    }
    public Mesh getMeshInSlot(PlayerBodySlots slot)
    {
        SkinnedMeshRenderer desiredSlot;
        getDesiredSlot(slot,out desiredSlot);
        return desiredSlot.sharedMesh;
    }

    private void getDesiredSlot(PlayerBodySlots slot, out SkinnedMeshRenderer slotToInsert)
    {
        switch (slot)
        {
            case PlayerBodySlots.HeadHairSlot:
                slotToInsert = HeadHairSlot;
                break;
            case PlayerBodySlots.HeadItemSlot:
                slotToInsert = HeadItemSlot;
                break;
            case PlayerBodySlots.FaceItemSlot:
                slotToInsert = FaceItemSlot;
                break;
            case PlayerBodySlots.FaceHairSlot:
                slotToInsert = FaceHairSlot;
                break;
            case PlayerBodySlots.UpperSlot:
                slotToInsert = UpperSlot;
                break;
            case PlayerBodySlots.HandSlot:
                slotToInsert = HandSlot;
                break;
            case PlayerBodySlots.LowerSlot:
                slotToInsert = LowerSlot;
                break;
            case PlayerBodySlots.FootSlot:
                slotToInsert = FootSlot;
                break;
            default:
                slotToInsert = null;
                break;
        }
    }

    private void insertInSlot(Mesh mesh, SkinnedMeshRenderer slot)
    {
        if (slot.sharedMesh == mesh)
        {
            slot.sharedMesh = new Mesh();
            return;
        }
        setGameObjectInSlot(mesh, slot);
    }

    private void setGameObjectInSlot(Mesh item, SkinnedMeshRenderer parent)
    {
        parent.sharedMesh = item;
    }


}

public enum PlayerBodySlots
{
    HeadHairSlot, HeadItemSlot, FaceItemSlot, FaceHairSlot, UpperSlot, HandSlot, LowerSlot, FootSlot
}