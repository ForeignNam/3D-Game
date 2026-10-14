using System;
using System.Threading.Tasks;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWapper : MonoBehaviour
    {
        private const string DefaultSaveFile = "save";
        private SavingSystem savingSystem;

        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
        }

        private async void Update()
        {
            try
            {
                if (Input.GetKeyDown(KeyCode.L))
                {
                    await LoadAsync();
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    await SaveAsync();
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception, this);
            }
        }

        public Task SaveAsync()
        {
            return savingSystem.SaveAsync(DefaultSaveFile);
        }

        public Task LoadAsync()
        {
            return savingSystem.LoadAsync(DefaultSaveFile);
        }
    }
}
