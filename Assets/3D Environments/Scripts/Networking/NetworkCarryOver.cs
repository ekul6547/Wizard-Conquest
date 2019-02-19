using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCarryOver : MonoBehaviour {

    public GameObject P;

	void Update()
    {
        NetworkPlayerProperties comp = gameObject.GetComponent<NetworkPlayerProperties>();
        if(comp != null && P != null)
        {
            CopyComponent(comp, P);
        }
    }
    Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
}
