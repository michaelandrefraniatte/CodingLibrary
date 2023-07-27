
 	public SharpDXMouse(CursorPositionTranslater positionTranslater)
 	{
		this.positionTranslater = positionTranslater;
		mouseCounter = new MouseDeviceCounter();
		directInput = new DInput.DirectInput();
		mouse = new DInput.Mouse(directInput);
		mouse.Properties.AxisMode = DInput.DeviceAxisMode.Absolute;
		mouse.Acquire();
		currentState = new DInput.MouseState();
	}

        public static void Initialize()
        {
            if (IsInitialized) return;
            
            _DirectInput = new DirectInput();
            _Mouse = new SharpDX.DirectInput.Mouse(_DirectInput);
            _Mouse.Properties.AxisMode = DeviceAxisMode.Relative;
            try
            {
                _Mouse.Acquire();
            }
            catch (SharpDX.SharpDXException)
            {
                Console.WriteLine("Error: Failed to aquire mouse !");
                return;
            }

            IsInitialized = true;
        }

	private void _initializeInputs()
        {
            Console.Write("Initializing inputs... ");

            _directInput = ToDispose<DirectInput>(new DirectInput());

            _dKeyboard = ToDispose<DKeyboard>(new DKeyboard(_directInput));
            _dKeyboard.Properties.BufferSize = 256;
            _dKeyboard.SetCooperativeLevel(_form, CooperativeLevel.Foreground | CooperativeLevel.Exclusive);
            Keyboard = new Keyboard(_dKeyboard);

            _dMouse = ToDispose<DMouse>(new DMouse(_directInput));
            _dMouse.Properties.AxisMode = DeviceAxisMode.Relative;
            _dMouse.SetCooperativeLevel(_form, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);
            Mouse = new Mouse(_form, _dMouse);
            Console.WriteLine("done.");
	}