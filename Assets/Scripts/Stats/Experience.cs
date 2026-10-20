using UnityEngine;
using RPG.Saving;
namespace RPG.Stats
{
public class Experience : MonoBehaviour, ISaveable
{
    public delegate bool ExampleDelegate(float value);
    public ExampleDelegate OnExampleDelegate;
    [SerializeField] private float experiencePoints = 0;
     public void GainExperience(float experience)
    {
            experiencePoints += experience;
            
    }
    public float GetExperience()
    {
        return experiencePoints;
    }
    public object CaptureState()
    {
        return experiencePoints;
    }
    public void RestoreState(object state)
    {
        experiencePoints = (float)state;
    }
    
    
}

}
