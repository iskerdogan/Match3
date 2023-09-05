using UnityEngine;
using Zenject;

namespace Game.View
{
    public class InputManager : MonoBehaviour,IInitializable
    {
        public delegate void ClickAction();
        public event ClickAction InitialClick;
        public event ClickAction Clicked;
        public event ClickAction ClickedUp;
        public event ClickAction ClickHold;
        
        public void Initialize()
        {
            Clicked += OnInitialClick;
        }

        private static string buttonName { get; } = "Fire1";

        void Update()
        {
            CheckInput();
        }

        void CheckInput()
        {
            if (Input.GetButtonDown(buttonName))
            {
                OnClicked();
            }
            if (Input.GetButton(buttonName))
            {
                OnClickHold();
            }
            if (Input.GetButtonUp(buttonName))
            {
                OnClickedUp();
            }
        }
        private void OnClicked()
        {
            Clicked?.Invoke();
        }
        private void OnClickedUp()
        {
            ClickedUp?.Invoke();
        }
        private void OnClickHold()
        {
            ClickHold?.Invoke();
        }
        private void OnInitialClick()
        {
            Clicked -= OnInitialClick;
            InitialClick?.Invoke();
        }
        
    }
}
