using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraSetScript : MonoBehaviour
{
    public DataPersistenceManager dataPersistence;
    public ScriptableObject aura_SO;
    private void Awake()
    {
        dataPersistence = FindObjectOfType<DataPersistenceManager>();
    }
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => dataPersistence.initialized);
        CheckForAvailableAura();
    }
    public void CheckForAvailableAura()
    {
        if (!dataPersistence.availableAuraList.Contains(aura_SO))
        {
            gameObject.SetActive(false);
        }
    }
}
