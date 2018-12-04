using UnityEngine;

public class GameInput : MonoBehaviour
{
    [Header("Inputs")]
    public Vector2 joy1;
    public Vector2 joy2;
    public bool button1;
    public bool button2;
    public bool button3;
    public bool button4;

    public virtual void Button1Down() {}
    public virtual void Button1Up() {}

    public virtual void Button2Down() {}
    public virtual void Button2Up() {}
    
    public virtual void Button3Down() {}
    public virtual void Button3Up() {}
    
    public virtual void Button4Down() {}
    public virtual void Button4Up() {}

    public virtual void Reset() {}
}
