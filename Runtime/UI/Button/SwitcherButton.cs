using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.UI
{
    public class SwitcherButton : Button
    {
        [SF] private bool enabledByDefault = false;
        [SF] private GameObject enabledState;
        [SF] private GameObject disabledState;

        private bool state;

        private void UpdateView()
        {
            if (enabledState)
                enabledState.SetActive(state);
            
            if (disabledState)
                disabledState.SetActive(!state);
        }
        
        protected override void ClickPerformed()
        {
            state = !state;
            UpdateView();
            base.ClickPerformed();
        }

        protected override void OnInit()
        {
	        base.OnInit();
	        state = enabledByDefault;
            UpdateView();
        }
    }
}
