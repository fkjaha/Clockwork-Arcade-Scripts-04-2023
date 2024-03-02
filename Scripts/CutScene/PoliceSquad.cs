using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceSquad : MonoBehaviour
{
    [SerializeField] private List<PoliceController> policeControllers;

    public void ActivatePolicemans()
    {
        foreach (PoliceController policeController in policeControllers)
        {
            policeController.ActivateMovement();
        }
    }
}
