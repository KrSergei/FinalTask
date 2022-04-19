using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerShooting : MonoBehaviour
{
    public Transform _shootSpot;
    public Rig aimLayer;
    public float aimDuration = 0.2f;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButton(1)) aimLayer.weight += Time.deltaTime / aimDuration;
        else aimLayer.weight -= Time.deltaTime / aimDuration;
    }
}
