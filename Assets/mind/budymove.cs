using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class budymove : MonoBehaviour
{
    public float x;    //創造名為x座標[浮點]
    public float y;    //同理y
    public Transform PlayerCamera;//攝影機選項  [Transform方向等訊息]
    float xRotation;   //x的旋轉
    float yRotaiton;   //同理



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 鎖定滑鼠游標在畫面中央         {Cu.....ate 设置为 C...Mode.Locked 时，滑鼠会被锁定在游戲}
        Cursor.visible = false;  //Cursor.     {visible}用于控制鼠标光标的可见性開關      {false;隐藏}
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * x;   // 取得滑鼠游標的X軸移動
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * y;   // 取得滑鼠游標的Y軸移動

        xRotation -= mouseY; // 將滑鼠Y軸移動數值"倒轉"過來(正變負負變正)
        yRotaiton += mouseX;
        transform.rotation = Quaternion.Euler(xRotation, yRotaiton, 0); // 設定攝影機角度
    }
}
