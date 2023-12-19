using UniRx;
using UnityEngine;

namespace Assets.Scripts.Core.Input
{
    public class LevelInputListener : MonoBehaviour
    {
        public ReactiveProperty<byte> numericKey = new ReactiveProperty<byte>();
        public Vector3 AxisInput;

        public virtual Vector3 CursorPosition => UnityEngine.Input.mousePosition;
        public virtual bool Button0 => Disabled ? false : UnityEngine.Input.GetMouseButton(0);
        public virtual bool ButtonDown0 => Disabled ? false : UnityEngine.Input.GetMouseButtonDown(0);
        public virtual bool ButtonUp0 => Disabled ? false : UnityEngine.Input.GetMouseButtonUp(0);
        public virtual bool ButtonR => Disabled ? false : UnityEngine.Input.GetKeyDown(KeyCode.R);

        public bool Disabled { get; private set; }
        private void Start()
        {
            Game.Events.Pause.Subscribe(OnPause).AddTo(this);
        }

        private void OnPause(bool isPause)
        {
            Disabled = isPause;
        }

        protected void UpdateAxis()
        {
            AxisInput.x = UnityEngine.Input.GetAxis("Horizontal");
            AxisInput.y = UnityEngine.Input.GetAxis("Vertical");
        }
        protected void HandleNumericInput(KeyCode keyCode, byte index)
        {
            if (UnityEngine.Input.GetKeyDown(keyCode) && Game.Player.Inventory.ActionBarItemCount > index)
                numericKey.Value = index;
        }
        private void Update()
        {
            OnUpdate();
        }
        protected virtual void OnUpdate()
        {
            if (Disabled) return;

            UpdateAxis();

            HandleNumericInput(KeyCode.Alpha1, 0);
            HandleNumericInput(KeyCode.Alpha2, 1);
            HandleNumericInput(KeyCode.Alpha3, 2);
            HandleNumericInput(KeyCode.Alpha4, 3);
            HandleNumericInput(KeyCode.Alpha5, 4);
        }
    }
}