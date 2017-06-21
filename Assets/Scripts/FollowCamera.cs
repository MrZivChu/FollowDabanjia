using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{

    public Transform target;

    void Update()
    {
        transform.rotation = target.rotation;
    }
}
