using UnityEngine;

namespace TKH3DCoffee
{
    public class PlayerController : MonoBehaviour
    {
        public LayerMask movementMask;      // The ground
        public LayerMask interactionMask;
        PlayerMotor motor;
        [SerializeField]
        protected Camera cam;

        public delegate void OnFocusChanged(Interactable newFocus);
        public OnFocusChanged onFocusChangedCallback;

        public Interactable focus;	// Our current focus: Item, Enemy etc.

        public void Start()
        {
            motor = GetComponent<PlayerMotor>();
            
        }

        public void Update()
        {
            // If we press left mouse
            if (Input.GetMouseButtonDown(0))
            {
                // Shoot out a ray
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // If we hit
                if (Physics.Raycast(ray, out hit, movementMask))
                {
                    motor.MoveToPoint(hit.point);

                    SetFocus(null);
                }
            }

            // If we press right mouse
            if (Input.GetMouseButtonDown(1))
            {
                // Shoot out a ray
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // If we hit
                if (Physics.Raycast(ray, out hit, 100f, interactionMask))
                {
                    SetFocus(hit.collider.GetComponent<Interactable>());
                }
            }


        }


        void SetFocus(Interactable newFocus)
        {
            if (onFocusChangedCallback != null)
                onFocusChangedCallback.Invoke(newFocus);

            // If our focus has changed
            if (focus != newFocus && focus != null)
            {
                // Let our previous focus know that it's no longer being focused
                focus.OnDefocused();
            }

            // Set our focus to what we hit
            // If it's not an interactable, simply set it to null
            focus = newFocus;

            if (focus != null)
            {
                // Let our focus know that it's being focused
                focus.OnFocused(transform);
            }

        }
    }
}
