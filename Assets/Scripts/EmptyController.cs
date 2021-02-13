using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyController : MonoBehaviour
{
    void Start()
    {
        var episode = Episode.Load("sample");
        Debug.Log(episode.title);
        Debug.Log(episode.intro.ToString());
    }
}
