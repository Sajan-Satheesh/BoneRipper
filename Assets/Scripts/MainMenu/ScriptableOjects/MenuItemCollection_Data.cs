using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "all Items", menuName = "MainMenu/ItemListData")]
public class MenuItemCollection_Data : ScriptableObject
{
   public List<MenuItem_Data> menuItems;
}
