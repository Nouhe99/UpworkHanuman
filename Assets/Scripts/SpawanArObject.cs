using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawanArObject : MonoBehaviour
{
    public GameObject Pref;

   public void OnButton(GameObject pref)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        Quaternion spawnRotation = Quaternion.LookRotation(-Camera.main.transform.forward);
        Instantiate(pref, spawnPosition, spawnRotation,this.transform);
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
