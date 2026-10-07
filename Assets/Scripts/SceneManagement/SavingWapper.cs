using UnityEngine;
using RPG.Saving;
namespace RPG.SceneManagement
{
public class SavingWapper : MonoBehaviour
{
    private const string defaultSaveFile = "save";
   private SavingSystem savingSystem;
   private void Awake()
   {
    savingSystem = GetComponent<SavingSystem>();
   }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }
    }

    public void Save()
    {
        savingSystem.Save(defaultSaveFile);
    }
    public void Load()
    {
        savingSystem.Load(defaultSaveFile);
    }
    }
}