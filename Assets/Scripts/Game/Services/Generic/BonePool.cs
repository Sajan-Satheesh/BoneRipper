using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class BonePool : GenericPool<BoneView,Transform>
{
    public BonePool(BoneView boneView) : base(boneView) { }

    protected override void getLogic(ref BoneView releasedItem, Transform spawnReference)
    {
        releasedItem.gameObject.transform.position = spawnReference.position;
        releasedItem.transform.localRotation = spawnReference.localRotation;
        releasedItem.gameObject.SetActive(true);
    }

    protected override void instantiationLogic(out BoneView newItem, Transform spawnReference)
    {
        newItem = Object.Instantiate(this.item, spawnReference.position, spawnReference.localRotation);
        newItem.transform.localRotation = spawnReference.localRotation;
    }

    protected override void returnLogic(ref BoneView returnedItem)
    {
        returnedItem.gameObject.SetActive(false);
    }
}
