using Framework.Core;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.SimpleInput
{
    public class InputSystem : MonoBehaviour
    {
        [SF] private InputHandler axisInput;
        [SF] private float sensitivity = 10;

        public static Vector2 Axis { get; private set; }
        public static bool Enabled { get; set; } = true;
        
        // Private fields
        
        private Canvas canvas;
        
        // MonoBehaviour

        [Inject]
        private void Construct(IUIManager uiManager)
        {
            canvas = GetComponentInChildren<Canvas>();
            canvas.worldCamera = uiManager.Camera;
        }

        private void Update()
        {
            if (Enabled == false)
                return;

            //Axis = Vector2.zero; 
            
            //! choppy input
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (Mathf.Approximately(horizontal,0) == false || Mathf.Approximately(vertical,0) == false)
            {
                var input = new Vector2(horizontal, vertical);
                
                if (input.sqrMagnitude > 1)
                {
                    input.Normalize();
                }
                
                Axis = Vector2.Lerp(Axis, input, sensitivity * Time.deltaTime);
            }
            else
            {
                if (axisInput) // Iterate through InputHandler[]
                {
                    Axis = Vector2.Lerp(Axis, axisInput.Direction, sensitivity * Time.deltaTime);
                    
                    if (axisInput.Direction == Vector2.zero && Axis.sqrMagnitude < 0.01f) // consider it's zero already at this point
                    {
                        Axis = Vector2.zero;
                    }
                }
                else
                {
                    Axis = Vector2.Lerp(Axis,Vector2.zero,sensitivity * Time.deltaTime);
                    
                    if (Axis.sqrMagnitude < 0.01f) // consider it's zero already at this point
                    {
                        Axis = Vector2.zero;
                    }
                }
            }
        }
    }
}
