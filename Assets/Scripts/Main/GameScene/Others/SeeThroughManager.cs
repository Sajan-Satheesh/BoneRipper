using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeeThroughManager : MonoBehaviour
{
    RaycastHit obstructionHit;
    [SerializeField] LayerMask obstructableLayers;
    Vector3 playerPosition;
    [field: SerializeField] public Material mainMaterial { get; private set; }
    [field: SerializeField] public Material hiddenMaterial { get; private set; }
    [SerializeField, Range(0f, 1f)] float minOpacity;
    [SerializeField] Dictionary<GameObject,KeyValuePair<Material,bool>> hiddenItems= new Dictionary<GameObject, KeyValuePair<Material, bool>>();
    [SerializeField] Dictionary<GameObject, bool> hiddenItemsDic = new Dictionary<GameObject, bool>();
    [SerializeField] List<GameObject> keys;
    int itemIndex = 0;
  
    private void Update()
    {
        keys = hiddenItems.Keys.ToList();
    }
    private void LateUpdate()
    {
        newFading();
        newResetObstructions();
        newRayCastOnPlayer(transform.position, obstructableLayers);
        //checkObstruction();
    }
    private KeyValuePair<K,V> setKeyValue<K,V>(K key, V value)
    {
        return new KeyValuePair<K, V>(key,value);
    }

    private KeyValuePair<Material, bool> setHiddenObjData(Material key, bool value)
    {
        return setKeyValue<Material,bool>(key,value);
    }

    private void checkItemIndex()
    {
        if (itemIndex >= hiddenItems.Count) itemIndex = 0;
    }
    private void setAsOpaque(GameObject item, Material itemMat)
    {
        Color mainColor = itemMat.GetColor("_BaseColor");
        itemMat.shader = mainMaterial.shader;
        itemMat.CopyPropertiesFromMaterial(mainMaterial);

        itemMat.SetColor("_BaseColor", mainColor);
        item.GetComponent<Renderer>().material = itemMat;
    }
    private void setAstransparent(GameObject item, Material itemMat)
    {
        Color hiddenColor = itemMat.GetColor("_BaseColor");
        itemMat.shader = hiddenMaterial.shader;
        itemMat.CopyPropertiesFromMaterial(hiddenMaterial);
        
        itemMat.SetColor("_BaseColor", hiddenColor);
        item.GetComponent<Renderer>().material = itemMat;
    }
    private void newFading()
    {
        if (hiddenItemsDic.Count == 0) return;
        ++itemIndex;
        checkItemIndex();
        GameObject key = hiddenItemsDic.ElementAt(itemIndex).Key;
        Renderer[] allRenderers = key.transform.GetComponentsInChildren<Renderer>();

        if (hiddenItemsDic.ElementAt(itemIndex).Value == true)
        {
            fadeOutAll(allRenderers);
        }
        else
        {
            fadeInAll(allRenderers);
        }

        void fadeInAll(Renderer[] items)
        {
            foreach(Renderer obj in items) 
            {
                Material mat = obj.gameObject.GetComponent<Renderer>().material;
                if (mat.color.a < 1f)
                {
                    Color color = mat.color;
                    color.a += 0.05f;
                    mat.SetColor("_BaseColor", color);
                }
                else
                {
                    setAsOpaque(obj.gameObject, mat);
                    hiddenItemsDic.Remove(key);
                }
            }
        }

        void fadeOutAll(Renderer[] items)
        {
            foreach (Renderer obj in items)
            {
                Material mat = obj.gameObject.GetComponent<Renderer>().material;
                if (mat.color.a >= 1f)
                {
                    setAstransparent(obj.gameObject, mat);
                }
                if (mat.color.a > minOpacity)
                {
                    Color color = mat.color;
                    color.a -= 0.05f;
                    mat.color = color;
                }
            }
        }

    }
    private void fading()
    {
        if (hiddenItems.Count == 0) return;
        ++itemIndex;
        checkItemIndex();
        GameObject key = hiddenItems.ElementAt(itemIndex).Key;
        Material keyMat = hiddenItems.ElementAt(itemIndex).Value.Key;
        Color color = keyMat.GetColor("_BaseColor");
        if (hiddenItems[key].Value == true)
        {
            if (color.a >= 1f)
            {
                setAstransparent(key, keyMat);
            }
            if (color.a > minOpacity)
            {
                color.a -= 0.05f;
                key.GetComponent<Renderer>().material.SetColor("_BaseColor", color);
            }
        }
        else
        {
            if (color.a < 1f)
            {
                color.a += 0.05f;
                key.GetComponent<Renderer>().material.SetColor("_BaseColor", color);
            }
            else
            {
                setAsOpaque(key, keyMat);
                hiddenItems.Remove(key);
            }
        }
        
    }

    private void newResetObstructions()
    {
        if (hiddenItemsDic.Count > 0)
        {
            checkItemIndex();
            if (hiddenItemsDic.ElementAt(itemIndex).Value == true)
            {
                hiddenItemsDic[hiddenItemsDic.ElementAt(itemIndex).Key] = false;
            }
        }
    }
    private void resetObstructions()
    {
        if (hiddenItems.Count > 0)
        {
            checkItemIndex();
            if (hiddenItems.ElementAt(itemIndex).Value.Value == true)
            {
                KeyValuePair<Material, bool> objData = hiddenItems[hiddenItems.ElementAt(itemIndex).Key];
                hiddenItems[hiddenItems.ElementAt(itemIndex).Key] = setHiddenObjData(objData.Key, false);
            }
        }
    }

    
    
    private void rayCastOnPlayer(Vector3 start, LayerMask obstructable)
    {
        playerPosition = PlayerService.instance.getPlayerLocation() + Vector3.up * 0.3f;
        Ray obstructionRay = new Ray(start, (playerPosition - start).normalized);
        
        if (Physics.Raycast(obstructionRay, out obstructionHit, Vector3.Distance(playerPosition, start), obstructable))
        {
            GameObject hitGO = obstructionHit.collider.gameObject;
            Debug.DrawRay(obstructionRay.origin, obstructionRay.direction * Vector3.Distance(start, obstructionHit.point), Color.red);
            if (Vector3.Distance(obstructionHit.point, playerPosition) < 0.5f) return;
            if (!hitGO.TryGetComponent(out PlayerView playerView) && !hitGO.TryGetComponent(out Follower water))
            {
                if (!hiddenItems.ContainsKey(obstructionHit.collider.gameObject))
                {
                    hiddenItems.Add(hitGO, setHiddenObjData(hitGO.GetComponent<Renderer>().material,true));
                }
                else
                {
                    hiddenItems[hitGO] = setHiddenObjData(hiddenItems[hitGO].Key, true);
                }
                rayCastOnPlayer(obstructionHit.point + obstructionRay.direction*0.5f,obstructable);
            }
            else return;
        }
        else return;

    }
    private void newRayCastOnPlayer(Vector3 start, LayerMask obstructable)
    {
        playerPosition = PlayerService.instance.getPlayerLocation() + Vector3.up * 0.3f;
        Ray obstructionRay = new Ray(start, (playerPosition - start).normalized);

        if (Physics.Raycast(obstructionRay, out obstructionHit, Vector3.Distance(playerPosition, start), obstructable))
        {
            GameObject hitGO = obstructionHit.collider.gameObject;
            Debug.DrawRay(obstructionRay.origin, obstructionRay.direction * Vector3.Distance(start, obstructionHit.point), Color.red);
            if (Vector3.Distance(obstructionHit.point, playerPosition) < 0.5f) return;
            if (!hitGO.TryGetComponent(out Iplayer player) && !hitGO.TryGetComponent(out Follower water))
            {
                if (!hiddenItemsDic.ContainsKey(obstructionHit.collider.gameObject))
                {
                    hiddenItemsDic.Add(hitGO, true);
                }
                else
                {
                    hiddenItemsDic[hitGO] =  true;
                }
                newRayCastOnPlayer(obstructionHit.point + obstructionRay.direction * 0.5f, obstructable);
            }
            else return;
        }
        else return;

    }


    /*    private void checkObstruction()
        {

            if (rayCastToPlayer())
            {
                if (!hiddenItems.ContainsKey(obstructionHit.collider.gameObject))
                {
                    hiddenItems.Add(obstructionHit.collider.gameObject, setHiddenObjData(obstructionHit.collider.GetComponent<Renderer>().material, true));
                    //fadeIn(obstructionHit.collider.gameObject);
                }
                else
                {
                    hiddenItems[obstructionHit.collider.gameObject] = setHiddenObjData(hiddenItems[obstructionHit.collider.gameObject].Key, true);
                }
            }
        }

     private bool rayCastToPlayer()
        {
            playerPosition = PlayerService.instance.getPlayerLocation() + Vector3.up * 0.5f;
            Ray obstructionRay = new Ray(transform.position, (playerPosition - transform.position).normalized);
            Debug.DrawRay(obstructionRay.origin,obstructionRay.direction * Vector3.Distance(playerPosition, transform.position), Color.red);
            return (Physics.Raycast(obstructionRay, out obstructionHit, Vector3.Distance(playerPosition, transform.position), obstructableLayers));
        }
    */

}
