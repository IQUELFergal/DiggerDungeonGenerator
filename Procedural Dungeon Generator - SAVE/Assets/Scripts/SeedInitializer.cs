using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedInitializer : MonoBehaviour
{
    public void InitSeed()
    {
        Random.InitState(Random.Range(-999999, 999999));
    }

    public void InitSeed(string seed)
    {
        Random.InitState(seed.GetHashCode());
    }

    public void InitSeed(int seed)
    {
        Random.InitState(seed);
    }
}
