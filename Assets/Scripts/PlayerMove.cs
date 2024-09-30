using System.Collections;
using TKH3DCoffee;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMove : Character
{

    public LayerMask movementMask;
    public LayerMask interactionMask;
    [SerializeField]
    private Camera cam;

    public delegate void OnFocusChanged(Interactable newFocus);
    public OnFocusChanged onFocusChangedCallback;

    private Interactable focus;
    [Header("Movement")]
    [SerializeField] private ParticleSystem clickEffect;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (!isSitting)
        {
            HandleMovement();
            HandleInteraction();
        }
    }

    private void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, movementMask))
            {
                MoveTo(hit.point);
                if (clickEffect != null)
                {
                    Instantiate(clickEffect, hit.point + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
                }
                SetFocus(null);
            }
        }
    }

    private void HandleInteraction()
    {
        if (Input.GetMouseButtonDown(1))
        {
           // Ray ray = cam.ScreenPointToRay(Input.mousePosition);
           // RaycastHit hit;

            //if (Physics.Raycast(ray, out hit, 100f, interactionMask))
            //{
            
            //    Chair chair = hit.collider.GetComponent<Chair>();
            //    SetFocus(chair);
            //    InteractWithChair(chair);
            //}
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chair")
        {
            if (1 > 0)
            {
                Chair chair = other.GetComponentInParent<Chair>();
                InteractWithChair(chair);
            }
        }
    }
 
    private IEnumerator WaitAndStand()
    {
        yield return new WaitForSeconds(2);
        StandUp();
    }

    protected override void StandUp()
    {
        base.StandUp();
    }

    private void SetFocus(Interactable newFocus)
    {
        if (onFocusChangedCallback != null)
            onFocusChangedCallback.Invoke(newFocus);

        if (focus != newFocus && focus != null)
        {
            focus.OnDefocused();
        }

        focus = newFocus;

        if (focus != null)
        {
            focus.OnFocused(transform);
        }
    }

}
