using UnityEngine;
namespace RPG.Core
{
   [DisallowMultipleComponent]
   public class PersistentObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject persistentObjectsPrefab;
    private static bool hasSpawned;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticState()
    {
        hasSpawned = false;
    }

    private void Start()
    {
        if(hasSpawned) return;

        if (persistentObjectsPrefab == null)
        {
            Debug.LogError("Persistent Objects Prefab has not been assigned.", this);
            return;
        }

        SpawnPersistentObjects();
        hasSpawned = true;
    }

        private void SpawnPersistentObjects()
    {
        GameObject instance = Instantiate(persistentObjectsPrefab);    
        DontDestroyOnLoad(instance);
    }   
    

} 
}
