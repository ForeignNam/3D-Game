using UnityEngine;
using RPG.Core;
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Canvas rootcanvas;
    [SerializeField] private RectTransform foreground;
     
    // Update is called once per frame
    void Update()
    {
        if(Mathf.Approximately(health.GetFraction(), 0f)
         || Mathf.Approximately(health.GetFraction(), 1f))
        {
            rootcanvas.enabled = false;
            return;
        }
        
            rootcanvas.enabled = true;
      
        foreground.localScale = new Vector3(health.GetFraction(), 1f, 1f);
    }
}
