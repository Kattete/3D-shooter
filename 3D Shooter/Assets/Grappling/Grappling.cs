using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private Movement pm;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform gunTip;
    [SerializeField] private LayerMask whatIsGrappable;
    [SerializeField] LineRenderer lr;

    [Header("Grappling")]
    [SerializeField] private float maxGrapplingDistance;
    [SerializeField] private float grappleDelayTime;
    [SerializeField] private float overShootY;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    [SerializeField] private float grapplingCd;
    private float grapplingCdTimer;

    private bool grappling;

    private void Start()
    {
        pm = GetComponent<Movement>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)) StartGrapple();

        if(grapplingCdTimer > 0) grapplingCdTimer -= Time.deltaTime;

    }

    private void LateUpdate()
    {
        if (grappling) lr.SetPosition(0, gunTip.position);
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;

        pm.freeze = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrapplingDistance, whatIsGrappable))
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrapplingDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeY = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeY + overShootY;

        if (grapplePointRelativeY < 0) highestPointOnArc = overShootY;
        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        pm.freeze = false;
        grappling = false;

        grapplingCdTimer = grapplingCd;

        lr.enabled = false;
    }
}
