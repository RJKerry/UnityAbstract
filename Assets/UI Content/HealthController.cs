using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{

    public Slider Healthbar;

    void Start()
    {
        Healthbar = this.transform.Find("Healthbar").GetComponent<Slider>();

        //healthbar validity check
        if (Healthbar == null)
        {
            Debug.LogError("Healthbar not found");
        }

        Healthbar.value = 1;

    }

    public void changeHealth(float health)
    {
        Healthbar.value = health;
    }

}
