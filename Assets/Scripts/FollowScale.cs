using UnityEngine;
using System.Collections;

public class FollowScale : MonoBehaviour
{
    public Transform target;
    public Transform current;

    void Update()
    {
        current.position = target.position;
    }
}
