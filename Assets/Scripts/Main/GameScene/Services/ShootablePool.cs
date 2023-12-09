using System.Collections.Generic;
using UnityEngine;

public class ShootablePool : GenericPool<Shootable>
{
    public ShootablePool(Shootable shootable) : base(shootable) { }

    protected override void instantiationLogic(out Shootable newItem)
    {
        newItem = Object.Instantiate(item, spwanPosition, Quaternion.identity);
    }

    protected override void getLogic(ref Shootable item)
    {
        item.transform.position = spwanPosition;
        item.gameObject.SetActive(true);
    }

    protected override void returnLogic()
    {
        item.gameObject.SetActive(false);
    }

}






/*public Shootable shootable;
Stack<Shootable> bulletsAvailable = new Stack<Shootable>();
public List<Shootable> bulletsInUse = new List<Shootable>();

public ShootablePool(Shootable shootable)
{
    this.shootable = shootable;
}

public void returnShootable(Shootable shootable)
{
    if (bulletsInUse.Count == 0) return;

    bulletsInUse.Remove(shootable);
    shootable.gameObject.SetActive(false);
    bulletsAvailable.Push(shootable);
}

public Shootable getShootable(Vector3 position)
{
    Shootable topElement;
    if (bulletsAvailable.Count == 0)
    {
        topElement = Object.Instantiate(shootable, position, Quaternion.identity) as Shootable;
        bulletsInUse.Add(topElement);
    }
    else
    {
        topElement = bulletsAvailable.Pop();
        topElement.transform.position = position;
        topElement.gameObject.SetActive(true);
        bulletsInUse.Add(topElement);

    }
    return topElement;
}*/