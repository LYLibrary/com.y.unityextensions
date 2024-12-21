using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[DefaultExecutionOrder(-1000)]
public class CloneItem : MonoBehaviour
{
    [Header("项目容器")]
    [Tooltip("如果为空，自动识别此物体为容器")]
    public Transform container;

    [Header("项目预设")] public List<GameObject> itemPrefabs = new List<GameObject>();

    [SerializeField, Header("当前生成的项目")] private List<GameObject> currentGenerateItems = new List<GameObject>();
    public List<GameObject> CurrentGenerateItems { get { return currentGenerateItems; } }

    private void Awake()
    {
        if (container == null)
        {
            container = transform;
        }
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].gameObject.SetActive(false);
            }
        }
    }

    public GameObject Clone(int prefabIndex = 0, bool active = true)
    {
        if (prefabIndex < 0 || prefabIndex >= itemPrefabs.Count)
        {
            return null;
        }
        GameObject prefab = itemPrefabs[prefabIndex];
        if (prefab != null)
        {
            GameObject item = Instantiate(prefab, container);
            item.name = prefab.name;
            item.SetActive(active);
            currentGenerateItems.Add(item);
            return item;
        }
        return null;
    }

    public T Clone<T>(int prefabIndex = 0, bool active = true) where T : Component
    {
        GameObject item = Clone(prefabIndex, active);
        if (item != null)
        {
            return item.GetComponent<T>();
        }
        return null;
    }

    public void DeleteItem(int itemIndex, bool destroyImmediate = false)
    {
        if (itemIndex < 0 || itemIndex >= currentGenerateItems.Count)
        {
            return;
        }
        GameObject item = currentGenerateItems[itemIndex];
        currentGenerateItems.RemoveAt(itemIndex);
        if (item != null)
        {
            item.SetActive(false);
            if (destroyImmediate)
            {
                DestroyImmediate(item);
            }
            else
            {
                Destroy(item);
            }
        }
    }

    public void DeleteAll(bool destroyImmediate = false)
    {
        List<GameObject> tempList = new List<GameObject>(currentGenerateItems);
        currentGenerateItems.Clear();
        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i] != null)
            {
                tempList[i].SetActive(false);
                if (destroyImmediate)
                {
                    DestroyImmediate(tempList[i]);
                }
                else
                {
                    Destroy(tempList[i]);
                }
            }
        }
    }

}
