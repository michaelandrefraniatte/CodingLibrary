using System;
using SharpDX.RawInput;
using SharpDX.Multimedia;
using System.Windows.Forms;
using SharpDX;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Fade
{
    public class InputManager : IUpdatable
    {
        public enum MouseButtons
        {
            Button1,
            Button2,
            Button3
        }

        //TODO use rawInput instead of RawInput and winform solution
        //ISSUE: can't no press key at once..done

        [DllImport("user32.dll")]
        internal static extern bool GetCursorPos(ref Point position);

        [DllImport("user32.dll")]
        internal static extern bool ScreenToClient(IntPtr windowHandle, ref Point position);

        public float MousePositionX => MousePosition.X;
        public float MousePositionY => MousePosition.Y;
        public Vector2 MousePosition { get; private set; }

        public delegate void KeyboardInputDelegate();
        public event KeyboardInputDelegate KeyboardInputEventHandler;

        private Dictionary<Keys, bool> keyStateDict = new Dictionary<Keys, bool>();
        private Dictionary<MouseButtons, bool> mouseStateDict = new Dictionary<MouseButtons, bool>();

        public InputManager() {
            var keyList = Enum.GetValues(typeof(Keys));
            foreach (var key in keyList) {
                var k = (Keys)key;
                if (keyStateDict.ContainsKey(k)) {
                    continue;
                }
                keyStateDict.Add(k, false);
            }
            var mouseButtonList = Enum.GetValues(typeof(MouseButtons));
            foreach (var mouseButton in mouseButtonList) {
                var m = (MouseButtons)mouseButton;
                if (mouseStateDict.ContainsKey(m)) {
                    continue;
                }
                mouseStateDict.Add(m, false);
            }

            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericKeyboard, DeviceFlags.None);
            Device.KeyboardInput += Device_KeyboardInput;
            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericMouse, DeviceFlags.None);
            Device.MouseInput += Device_MouseInput;
        }

        private void PrintMouseDict() {
            foreach (var k in mouseStateDict) {
                Console.WriteLine("{0}:{1}", k.Key, k.Value);
            }
        }

        public bool IsMouseButtonDown(MouseButtons mouseButton) => mouseStateDict[mouseButton];

        private void Device_MouseInput(object sender, MouseInputEventArgs e) {
            switch (e.ButtonFlags) {
                case MouseButtonFlags.Button1Down:
                    mouseStateDict[MouseButtons.Button1] = true;
                    break;
                case MouseButtonFlags.Button1Up:
                    mouseStateDict[MouseButtons.Button1] = false;
                    break;
                case MouseButtonFlags.Button2Down:
                    mouseStateDict[MouseButtons.Button2] = true;
                    break;
                case MouseButtonFlags.Button2Up:
                    mouseStateDict[MouseButtons.Button2] = false;
                    break;
                case MouseButtonFlags.Button3Down:
                    mouseStateDict[MouseButtons.Button3] = true;
                    break;
                case MouseButtonFlags.Button3Up:
                    mouseStateDict[MouseButtons.Button3] = false;
                    break;
                case MouseButtonFlags.MouseWheel:
                    //处理滚轮
                    break;
                default:
                    break;
            }
            PrintMouseDict();
        }

        public bool IsKeyDown(Keys key) => keyStateDict[key];

        private void Device_KeyboardInput(object sender, KeyboardInputEventArgs e) {
            if (e.State == KeyState.KeyDown) {
                keyStateDict[e.Key] = true;
            }
            else if (e.State == KeyState.KeyUp) {
                keyStateDict[e.Key] = false;
            }
            KeyboardInputEventHandler();
        }

        public void Update(float dt) {
            //更新鼠标位置
            Point mousePos = Point.Zero;
            GetCursorPos(ref mousePos);
            ScreenToClient(Game.Instance.Form.Handle, ref mousePos);
            MousePosition = mousePos;
        }
    }
}