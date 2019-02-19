using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisps : MonoBehaviour {
    public Light light;
    public float minFlickerIntensity  = 0.5f;
    public float maxFlickerIntensity = 2.5f;
    public float flickerSpeed = 0.035f;
    float randomizer = 0f;
    Vector3 originalPos;

    public Vector3 floatStrength;
    public float speedTime = 1;
    public Vector3 offset;
    public Vector3 offsetIncrease;

    void Start()
    {
        InvokeRepeating("flicker", 1f, flickerSpeed);
        this.originalPos = this.transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(originalPos.x + ((float)System.Math.Sin((Time.time * speedTime) + offset.x) * floatStrength.x), originalPos.y + ((float)System.Math.Sin((Time.time * speedTime) + offset.y) * floatStrength.y), originalPos.z + ((float)System.Math.Sin((Time.time * speedTime) + offset.z) * floatStrength.z));
        offset += offsetIncrease*Time.deltaTime;
    }
    void flicker()
    {
        if (randomizer == 0)
        {
            light.intensity = (Random.Range(minFlickerIntensity, maxFlickerIntensity));

        }
        else light.intensity = (Random.Range(minFlickerIntensity, maxFlickerIntensity));

        randomizer = Random.Range(0f, 1.1f);
    }
}
