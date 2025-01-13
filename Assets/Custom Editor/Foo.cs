using UnityEngine;

public class Foo : MonoBehaviour
{
    public int value = 10;

    public int Value => value;

    // Update is called once per frame
    void Update() => Debug.Log("Foo Update");
}
