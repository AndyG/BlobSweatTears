using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
  // Prefab for this pool. The prefab must have a component of type T on it.
  public T m_prefab;

  // Size of this object pool
  public int m_size;

  // The list of free and used objects for tracking.
  // We use the generic collections so we can give them our type T.
  private List<T> m_freeList;
  private List<T> m_usedList;

  public void Awake()
  {
    m_freeList = new List<T>(m_size);
    m_usedList = new List<T>(m_size);

    // Instantiate the pooled objects and disable them.
    for (var i = 0; i < m_size; i++)
    {
      var pooledObject = Instantiate(m_prefab, transform);
      pooledObject.gameObject.SetActive(false);
      m_freeList.Add(pooledObject);
    }
  }

  public T Get()
  {
    var numFree = m_freeList.Count;
    if (numFree == 0)
    {
      Debug.LogWarning("Ran out of room in pool: " + GetType().Name);
      return null;
    }

    // Pull an object from the end of the free list.
    var pooledObject = m_freeList[numFree - 1];
    m_freeList.RemoveAt(numFree - 1);
    m_usedList.Add(pooledObject);
    pooledObject.gameObject.SetActive(true);
    return pooledObject;
  }

  public void ReturnObject(T pooledObject)
  {
    Debug.Assert(m_usedList.Contains(pooledObject));

    // Put the pooled object back in the free list.
    m_usedList.Remove(pooledObject);
    m_freeList.Add(pooledObject);

    // Reparent the pooled object to us, and disable it.
    var pooledObjectTransform = pooledObject.transform;
    pooledObjectTransform.parent = transform;
    pooledObjectTransform.localPosition = Vector3.zero;
    pooledObject.gameObject.SetActive(false);
  }
}