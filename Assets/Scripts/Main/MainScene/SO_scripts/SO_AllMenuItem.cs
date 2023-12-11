using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "all Items", menuName = "MainMenu/ItemList")]
public class SO_AllMenuItem : ScriptableObject
{
   public List<SO_MenuItem> menuItems;
}
