using UnityEngine;

namespace Harpa
{
    public class PlayerController : MonoBehaviour
    {
        public bool inFP;
        public bool gameOnly;
        FPPawn fpPawn;

        GameInput gameInput;
        public GameObject texturePreview;

        void Awake()
        {
            fpPawn = FindObjectOfType<FPPawn>();
            gameInput = FindObjectOfType<GameInput>();

            if (!gameOnly)
            {
                fpPawn.flying = true;
            }
            else
            {
                inFP = false;
            }
        }

        void SetCursorLockState(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }

        void Update()
        {
            // Debug.Log( "num joysticks: " + Input.GetJoystickNames().Length );

            // for( int i = 0; i < Input.GetJoystickNames().Length; i++ )
            // {
            //     Debug.Log( "joystick " + i + ": " + Input.GetJoystickNames()[ i ] );
            // }

            if (Input.GetKeyDown(KeyCode.F1) && !gameOnly)
            {
                inFP = !inFP;
                SetCursorLockState(inFP);
            }

            if (Input.GetKeyDown(KeyCode.F2) && !gameOnly)
                texturePreview.SetActive(!texturePreview.activeSelf);

            if (inFP)
            {
                fpPawn.moveInput = new Vector3();
                if (Input.GetKey(KeyCode.D))
                    fpPawn.moveInput.x += 1;
                if (Input.GetKey(KeyCode.A))
                    fpPawn.moveInput.x -= 1;
                if (Input.GetKey(KeyCode.W))
                    fpPawn.moveInput.z += 1;
                if (Input.GetKey(KeyCode.S))
                    fpPawn.moveInput.z -= 1;

                fpPawn.lookYaw = Input.GetAxisRaw("Mouse X");
                fpPawn.lookPitch = Input.GetAxisRaw("Mouse Y");

                fpPawn.run = Input.GetKey(KeyCode.LeftShift);

                // if (Input.GetKeyDown(KeyCode.F))
                //     fpPawn.flying = !fpPawn.flying;

                fpPawn.fly = 0;
                if (Input.GetKey(KeyCode.Space))
                    fpPawn.fly += 1;
                if (Input.GetKey(KeyCode.LeftControl))
                    fpPawn.fly -= 1;
            }
            else
            {
                gameInput.joy1 = new Vector2();
                if (Input.GetKey(KeyCode.D))
                    gameInput.joy1.x += 1;
                if (Input.GetKey(KeyCode.A))
                    gameInput.joy1.x -= 1;
                if (Input.GetKey(KeyCode.W))
                    gameInput.joy1.y += 1;
                if (Input.GetKey(KeyCode.S))
                    gameInput.joy1.y -= 1;

                gameInput.joy1.x = Mathf.Clamp( gameInput.joy1.x + Input.GetAxisRaw("Horizontal1A") + Input.GetAxisRaw("Horizontal1B"), -1f, 1f );
                gameInput.joy1.y = Mathf.Clamp( gameInput.joy1.y + Input.GetAxisRaw("Vertical1A") + Input.GetAxisRaw("Vertical1B"), -1f, 1f );
                    
                gameInput.joy2 = new Vector2();
                if (Input.GetKey(KeyCode.L))
                    gameInput.joy2.x += 1;
                if (Input.GetKey(KeyCode.J))
                    gameInput.joy2.x -= 1;
                if (Input.GetKey(KeyCode.I))
                    gameInput.joy2.y += 1;
                if (Input.GetKey(KeyCode.K))
                    gameInput.joy2.y -= 1;

                gameInput.joy2.x = Mathf.Clamp( gameInput.joy2.x + Input.GetAxisRaw("Horizontal2A") + Input.GetAxisRaw("Horizontal2B"), -1f, 1f );
                gameInput.joy2.y = Mathf.Clamp( gameInput.joy2.y + Input.GetAxisRaw("Vertical2A") + Input.GetAxisRaw("Vertical2B"), -1f, 1f );
                    
                gameInput.button1 = Input.GetKey(KeyCode.Q);
                gameInput.button2 = Input.GetKey(KeyCode.E);

                gameInput.button3 = Input.GetKey(KeyCode.U);
                gameInput.button4 = Input.GetKey(KeyCode.O);
                
                if (Input.GetKeyDown(KeyCode.Q))
                    gameInput.Button1Down();
                if (Input.GetKeyUp(KeyCode.Q))
                    gameInput.Button1Up();

                if (Input.GetKeyDown(KeyCode.E))
                    gameInput.Button2Down();
                if (Input.GetKeyUp(KeyCode.E))
                    gameInput.Button2Up();
                    
                if (Input.GetKeyDown(KeyCode.U))
                    gameInput.Button3Down();
                if (Input.GetKeyUp(KeyCode.U))
                    gameInput.Button3Up();
                    
                if (Input.GetKeyDown(KeyCode.O))
                    gameInput.Button4Down();
                if (Input.GetKeyUp(KeyCode.O))
                    gameInput.Button4Up();

                if (Input.GetKeyDown(KeyCode.Alpha0))
                    gameInput.Reset();
            }
        }
    }
}
