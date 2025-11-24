using UnityEngine;

public class EnableCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // desbloquear cursor
        Cursor.visible = true;                   // mostrar cursor
    }
}
