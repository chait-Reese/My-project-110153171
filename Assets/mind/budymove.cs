using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class budymove : MonoBehaviour
{
    [Header("鏡頭轉動敏感度")]        //字體
    public float x;    //創造名為x的東西[浮點]    後面用x调整移动的敏感度或速度
    public float y;    //同理y
    public Transform PlayerCamera;//攝影機選項  [Transform有方向等訊息]
    float xRot;   //帶指x旋轉存在,有存在才能帶入
    float yRotaiton;   //同理
    //移動
    [Header("移動設定")]
    private float horizontalInput;   // 左右方向按鍵的數值(-1 <= X <= +1)
    private float verticalInput;     // 上下方向按鍵的數值(-1 <= Y <= +1)

    private Vector3 moveDirection;   // 移動方向

    private Rigidbody rbFirstPerson; // 第一人稱物件(膠囊體)的剛體
    [Header("移動設定")]
    public float moveSpeed;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 鎖定滑鼠游標在畫面中央         {Cu.....ate 设置为 C...Mode.Locked 时，滑鼠会被锁定在游戲}
        Cursor.visible = false;  //Cursor.     {visible}用于控制鼠标光标的可见性開關      {false;隐藏}


        rbFirstPerson = GetComponent<Rigidbody>();
        rbFirstPerson.freezeRotation = true;         // 鎖定第一人稱物件剛體旋轉，不讓膠囊體因為碰到物件就亂轉
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * x;   // 取得滑鼠游標的X軸移動
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * y;   // 然後反代 float ....

        //Input.GetAxisRaw("Mouse X") 是鼠标在 X 轴上的移动值。正向右，负向左。Time.deltaTime 时间增量，上一帧到当前帧的时间间隔 x调整移动的敏感度或速度。
        xRot -= mouseY;             // 將滑鼠Y軸移動數值"倒轉"過來(正變負負變正)  {移動值轉座標{旋轉}值}
        yRotaiton += mouseX;        //反正是相反
        xRot = Mathf.Clamp(xRot ,- 90f, 30f) ;  // 限定X軸轉動在正30度到負90度間(抬頭和低頭有限制角度)
        transform.rotation = Quaternion.Euler(xRot, yRotaiton, 0); // 帶入座標造成旋轉



        MyInput();
        SpeedControl();   // 偵測速度，過快就減速
    }
    private void FixedUpdate()
    {
        MovePlayer();     // 只要是物件移動，建議你放到FixedUpdate()        
    }

    // 方法：取得目前玩家按方向鍵上下左右的數值
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // 計算移動方向(其實就是計算X軸與Z軸兩個方向的力量)
        moveDirection = PlayerCamera.forward * verticalInput + PlayerCamera.right * horizontalInput;
        // 推動第一人稱物件
        rbFirstPerson.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    // 方法：偵測速度並減速
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z); // 取得僅X軸與Z軸的平面速度

        // 如果平面速度大於預設速度值，就將物件的速度限定於預設速度值
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rbFirstPerson.velocity = new Vector3(limitedVel.x, rbFirstPerson.velocity.y, limitedVel.z);
        }
    }
}

