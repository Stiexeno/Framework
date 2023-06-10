using Framework.Core;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.SimpleInput
{
    public class InputManager : MonoBehaviour, IInputManager
    {
        [SF] private InputHandler axisInput;
        [SF] private float sensitivity = 10;

        public static bool Enabled { get; set; } = true;
        
        // Private fields
        
        private Vector2 axis;
        private Canvas canvas;

        public Vector2 Axis => axis;
        
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
                
                axis = Vector2.Lerp(Axis, input, sensitivity * Time.deltaTime);
            }
            else
            {
                if (axisInput) // Iterate through InputHandler[]
                {
                    axis = Vector2.Lerp(Axis, axisInput.Direction, sensitivity * Time.deltaTime);
                    
                    if (axisInput.Direction == Vector2.zero && Axis.sqrMagnitude < 0.01f) // consider it's zero already at this point
                    {
                        axis = Vector2.zero;
                    }
                }
                else
                {
                    axis = Vector2.Lerp(Axis,Vector2.zero,sensitivity * Time.deltaTime);
                    
                    if (Axis.sqrMagnitude < 0.01f) // consider it's zero already at this point
                    {
                        axis = Vector2.zero;
                    }
                }
            }
        }
    }
}
