using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";

        private static readonly HashSet<SaveableEntity> Registry =
            new HashSet<SaveableEntity>();

        public static IReadOnlyCollection<SaveableEntity> RegisteredEntities => Registry;

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                Registry.Add(this);
            }
        }

        private void OnDisable()
        {
            Registry.Remove(this);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetRegistry()
        {
            Registry.Clear();
        }

#if UNITY_EDITOR
        private static readonly Dictionary<string, SaveableEntity> GlobalLookup =
            new Dictionary<string, SaveableEntity>();

        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            GlobalLookup[property.stringValue] = this;
        }

        private bool IsUnique(string candidate)
        {
            if (!GlobalLookup.ContainsKey(candidate)) return true;
            if (GlobalLookup[candidate] == this) return true;

            if (GlobalLookup[candidate] == null)
            {
                GlobalLookup.Remove(candidate);
                return true;
            }

            if (GlobalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                GlobalLookup.Remove(candidate);
                return true;
            }

            return false;
        }
#endif
    }
}
