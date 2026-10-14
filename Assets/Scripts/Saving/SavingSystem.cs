using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        private const string StringState = "string";
        private const string FloatState = "float";
        private const string IntegerState = "int";
        private const string BooleanState = "bool";
        private const string Vector3State = "vector3";

        private class SaveFileData
        {
            public int LastSceneBuildIndex = -1;
            public readonly Dictionary<string, SavedEntityData> Entities =
                new Dictionary<string, SavedEntityData>();
        }

        private class SavedEntityData
        {
            public readonly Dictionary<string, SavedComponentData> Components =
                new Dictionary<string, SavedComponentData>();
        }

        [Serializable]
        private class SavedComponentData
        {
            public string componentType;
            public string stateType;
            public string stringValue;
            public float floatValue;
            public int integerValue;
            public bool booleanValue;
            public SerializableVector3 vector3Value;
        }

        // JsonUtility cannot serialize dictionaries, so these DTOs are only used
        // at the JSON boundary. Runtime lookups still use the dictionaries above.
        [Serializable]
        private class SaveFileJsonData
        {
            public int lastSceneBuildIndex = -1;
            public List<SavedEntityJsonData> entities = new List<SavedEntityJsonData>();
        }

        [Serializable]
        private class SavedEntityJsonData
        {
            public string identifier;
            public List<SavedComponentData> components = new List<SavedComponentData>();
        }

        public async Task LoadLastSceneAsync(string saveFile)
        {
            SaveFileData data = await LoadFileAsync(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (data.LastSceneBuildIndex >= 0)
            {
                buildIndex = data.LastSceneBuildIndex;
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(buildIndex);
            if (loadOperation != null)
            {
                await WaitForAsyncOperation(loadOperation);
            }
            RestoreState(data);
        }

        public async Task SaveAsync(string saveFile)
        {
            SaveFileData data = await LoadFileAsync(saveFile);
            CaptureState(data);
            await SaveFileAsync(saveFile, data);
        }

        public async Task LoadAsync(string saveFile)
        {
            SaveFileData data = await LoadFileAsync(saveFile);
            RestoreState(data);
        }

        public async Task DeleteAsync(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            await Task.Run(() =>
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            });
        }

        private async Task<SaveFileData> LoadFileAsync(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new SaveFileData();
            }

            try
            {
                string json = await File.ReadAllTextAsync(path);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new SaveFileData();
                }

                return DeserializeSaveFile(json);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Could not load save file '{path}': {exception.Message}", this);
                return new SaveFileData();
            }
        }

        private async Task SaveFileAsync(string saveFile, SaveFileData data)
        {
            string path = GetPathFromSaveFile(saveFile);
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = SerializeSaveFile(data);
            await File.WriteAllTextAsync(path, json);
            Debug.Log("Saving to " + path, this);
        }

        private void CaptureState(SaveFileData data)
        {
            foreach (SaveableEntity saveable in SaveableEntity.RegisteredEntities)
            {
                if (saveable == null) continue;

                string identifier = saveable.GetUniqueIdentifier();
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    Debug.LogWarning($"SaveableEntity '{saveable.name}' has no identifier.", saveable);
                    continue;
                }

                if (!data.Entities.TryGetValue(identifier, out SavedEntityData entityData))
                {
                    entityData = new SavedEntityData();
                    data.Entities[identifier] = entityData;
                }

                entityData.Components.Clear();
                foreach (ISaveable component in saveable.GetComponents<ISaveable>())
                {
                    SavedComponentData componentData = SerializeComponent(component);
                    if (componentData != null)
                    {
                        entityData.Components[componentData.componentType] = componentData;
                    }
                }
            }

            data.LastSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(SaveFileData data)
        {
            foreach (SaveableEntity saveable in SaveableEntity.RegisteredEntities)
            {
                if (saveable == null) continue;

                string identifier = saveable.GetUniqueIdentifier();
                if (!data.Entities.TryGetValue(identifier, out SavedEntityData entityData))
                {
                    continue;
                }

                foreach (ISaveable component in saveable.GetComponents<ISaveable>())
                {
                    string componentType = component.GetType().ToString();
                    if (!entityData.Components.TryGetValue(
                        componentType,
                        out SavedComponentData componentData))
                    {
                        continue;
                    }

                    object state = DeserializeComponent(componentData);
                    if (state != null)
                    {
                        component.RestoreState(state);
                    }
                }
            }
        }

        private SavedComponentData SerializeComponent(ISaveable component)
        {
            object state = component.CaptureState();
            if (state == null) return null;

            SavedComponentData data = new SavedComponentData
            {
                componentType = component.GetType().ToString()
            };

            if (state is string stringValue)
            {
                data.stateType = StringState;
                data.stringValue = stringValue;
            }
            else if (state is float floatValue)
            {
                data.stateType = FloatState;
                data.floatValue = floatValue;
            }
            else if (state is int integerValue)
            {
                data.stateType = IntegerState;
                data.integerValue = integerValue;
            }
            else if (state is bool booleanValue)
            {
                data.stateType = BooleanState;
                data.booleanValue = booleanValue;
            }
            else if (state is SerializableVector3 vector3Value)
            {
                data.stateType = Vector3State;
                data.vector3Value = vector3Value;
            }
            else
            {
                Debug.LogWarning(
                    $"Cannot save state type '{state.GetType()}' from '{data.componentType}'.",
                    this);
                return null;
            }

            return data;
        }

        private object DeserializeComponent(SavedComponentData data)
        {
            switch (data.stateType)
            {
                case StringState:
                    return data.stringValue;
                case FloatState:
                    return data.floatValue;
                case IntegerState:
                    return data.integerValue;
                case BooleanState:
                    return data.booleanValue;
                case Vector3State:
                    return data.vector3Value;
                default:
                    Debug.LogWarning(
                        $"Cannot restore unknown state type '{data.stateType}' for '{data.componentType}'.",
                        this);
                    return null;
            }
        }

        private string SerializeSaveFile(SaveFileData data)
        {
            SaveFileJsonData jsonData = new SaveFileJsonData
            {
                lastSceneBuildIndex = data.LastSceneBuildIndex
            };

            foreach (KeyValuePair<string, SavedEntityData> entityPair in data.Entities)
            {
                SavedEntityJsonData entityJson = new SavedEntityJsonData
                {
                    identifier = entityPair.Key
                };

                foreach (SavedComponentData componentData in entityPair.Value.Components.Values)
                {
                    entityJson.components.Add(componentData);
                }

                jsonData.entities.Add(entityJson);
            }

            return JsonUtility.ToJson(jsonData, true);
        }

        private SaveFileData DeserializeSaveFile(string json)
        {
            SaveFileJsonData jsonData = JsonUtility.FromJson<SaveFileJsonData>(json);
            SaveFileData data = new SaveFileData();
            if (jsonData == null) return data;

            data.LastSceneBuildIndex = jsonData.lastSceneBuildIndex;
            if (jsonData.entities == null) return data;

            foreach (SavedEntityJsonData entityJson in jsonData.entities)
            {
                if (string.IsNullOrWhiteSpace(entityJson.identifier)) continue;

                SavedEntityData entityData = new SavedEntityData();
                if (entityJson.components != null)
                {
                    foreach (SavedComponentData componentData in entityJson.components)
                    {
                        if (componentData == null ||
                            string.IsNullOrWhiteSpace(componentData.componentType))
                        {
                            continue;
                        }

                        entityData.Components[componentData.componentType] = componentData;
                    }
                }

                data.Entities[entityJson.identifier] = entityData;
            }

            return data;
        }

        private static Task WaitForAsyncOperation(AsyncOperation operation)
        {
            if (operation.isDone) return Task.CompletedTask;

            TaskCompletionSource<bool> completionSource =
                new TaskCompletionSource<bool>();
            operation.completed += _ => completionSource.TrySetResult(true);
            return completionSource.Task;
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".json");
        }
    }
}
