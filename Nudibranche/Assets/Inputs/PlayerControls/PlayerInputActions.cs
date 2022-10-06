//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.2
//     from Assets/Inputs/PlayerControls/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Character"",
            ""id"": ""dffa9ebb-0019-42d8-a081-3b1c7bf71c5c"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ac956f6a-dc73-41d6-9833-8f75f133be92"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AimGamepad"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ae59ba16-f537-4224-b680-334308ee54b3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone(min=0.5)"",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AimMouse"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f1c40998-9253-4700-bb31-0c5829920c00"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone(min=0.5)"",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ShootGamepad"",
                    ""type"": ""Value"",
                    ""id"": ""d600c100-b086-4823-9ac5-0c12ebbd7e4f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ShootMouse"",
                    ""type"": ""Button"",
                    ""id"": ""401b10aa-7c54-4eaa-b94d-7e087f53826a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Parry"",
                    ""type"": ""Button"",
                    ""id"": ""c2cf6a4f-006b-4a95-89c0-f979a9658b40"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4c71cc50-a055-4d31-a797-60514fd8cde5"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.5)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""856e2a24-7fa2-4ec7-b234-548b0521a21e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c14bf9e8-f1e8-46a5-9a57-9ac9a91d344f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""53395857-a3b6-4cb4-80b2-5f8d6201ce8b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""de1779ef-59c9-4785-9e7d-855e5e0a8a5e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4473d14e-3a59-4269-a6c5-25dd10d82c38"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1a1f5ae8-2dfe-40ed-9695-7d61ec6ca101"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AimGamepad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6936a2b5-46b6-4ac9-b0a7-f42f7202ef4a"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": ""Hold(duration=0.2)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ShootGamepad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b1d74db-9646-4cd7-bacb-f6b002a7b556"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""ShootMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5586d11c-374a-4ca8-97bf-5ab9701ee63c"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""AimMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0bbdd00c-058a-4a15-85b8-5ca6474f5917"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a7c830a-c5b1-4806-ad78-9ae8df976f70"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Character
        m_Character = asset.FindActionMap("Character", throwIfNotFound: true);
        m_Character_Movement = m_Character.FindAction("Movement", throwIfNotFound: true);
        m_Character_AimGamepad = m_Character.FindAction("AimGamepad", throwIfNotFound: true);
        m_Character_AimMouse = m_Character.FindAction("AimMouse", throwIfNotFound: true);
        m_Character_ShootGamepad = m_Character.FindAction("ShootGamepad", throwIfNotFound: true);
        m_Character_ShootMouse = m_Character.FindAction("ShootMouse", throwIfNotFound: true);
        m_Character_Parry = m_Character.FindAction("Parry", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Character
    private readonly InputActionMap m_Character;
    private ICharacterActions m_CharacterActionsCallbackInterface;
    private readonly InputAction m_Character_Movement;
    private readonly InputAction m_Character_AimGamepad;
    private readonly InputAction m_Character_AimMouse;
    private readonly InputAction m_Character_ShootGamepad;
    private readonly InputAction m_Character_ShootMouse;
    private readonly InputAction m_Character_Parry;
    public struct CharacterActions
    {
        private @PlayerInputActions m_Wrapper;
        public CharacterActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Character_Movement;
        public InputAction @AimGamepad => m_Wrapper.m_Character_AimGamepad;
        public InputAction @AimMouse => m_Wrapper.m_Character_AimMouse;
        public InputAction @ShootGamepad => m_Wrapper.m_Character_ShootGamepad;
        public InputAction @ShootMouse => m_Wrapper.m_Character_ShootMouse;
        public InputAction @Parry => m_Wrapper.m_Character_Parry;
        public InputActionMap Get() { return m_Wrapper.m_Character; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
        public void SetCallbacks(ICharacterActions instance)
        {
            if (m_Wrapper.m_CharacterActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
                @AimGamepad.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAimGamepad;
                @AimGamepad.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAimGamepad;
                @AimGamepad.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAimGamepad;
                @AimMouse.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAimMouse;
                @AimMouse.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAimMouse;
                @AimMouse.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAimMouse;
                @ShootGamepad.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnShootGamepad;
                @ShootGamepad.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnShootGamepad;
                @ShootGamepad.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnShootGamepad;
                @ShootMouse.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnShootMouse;
                @ShootMouse.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnShootMouse;
                @ShootMouse.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnShootMouse;
                @Parry.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnParry;
                @Parry.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnParry;
                @Parry.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnParry;
            }
            m_Wrapper.m_CharacterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @AimGamepad.started += instance.OnAimGamepad;
                @AimGamepad.performed += instance.OnAimGamepad;
                @AimGamepad.canceled += instance.OnAimGamepad;
                @AimMouse.started += instance.OnAimMouse;
                @AimMouse.performed += instance.OnAimMouse;
                @AimMouse.canceled += instance.OnAimMouse;
                @ShootGamepad.started += instance.OnShootGamepad;
                @ShootGamepad.performed += instance.OnShootGamepad;
                @ShootGamepad.canceled += instance.OnShootGamepad;
                @ShootMouse.started += instance.OnShootMouse;
                @ShootMouse.performed += instance.OnShootMouse;
                @ShootMouse.canceled += instance.OnShootMouse;
                @Parry.started += instance.OnParry;
                @Parry.performed += instance.OnParry;
                @Parry.canceled += instance.OnParry;
            }
        }
    }
    public CharacterActions @Character => new CharacterActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface ICharacterActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnAimGamepad(InputAction.CallbackContext context);
        void OnAimMouse(InputAction.CallbackContext context);
        void OnShootGamepad(InputAction.CallbackContext context);
        void OnShootMouse(InputAction.CallbackContext context);
        void OnParry(InputAction.CallbackContext context);
    }
}
