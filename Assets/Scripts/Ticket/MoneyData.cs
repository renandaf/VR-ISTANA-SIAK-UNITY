using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyData : MonoBehaviour
{
    private bool isGrab = false;

    public void SetIsGrab(bool value)
    {
        isGrab = value;
    }

    public bool GetIsGrab()
    {
        return isGrab;
    }
}
