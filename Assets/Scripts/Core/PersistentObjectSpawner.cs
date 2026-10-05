using UnityEngine;
namespace RPG.Core
{
   public class PersistentObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject persistentObjectsPrefab;
    private bool hasSpawned = false;
    private void Start()
    {
        if(hasSpawned) return;
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

