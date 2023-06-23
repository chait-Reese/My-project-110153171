using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed;     //移動的物體裝置
    [Header("按鍵綁定")]
    public KeyCode jumpKey = KeyCode.Space;  //根据需要修改jumpKey 跳跃操作的按键绑定。KeyCode.Space[空白建]
    [Header("攝影機抓取")]
    public Transform PlayerCamera;   // 攝影機



    public float groundDrag;         // 地面的減速裝置
    [Header("地面檢查")]
    public float playerHeight;
    public LayerMask whatIsGround;//地板檢測
    public bool grounded;            // 布林變數：檢查地面



    private float horizontalInput;   // 設定左右方向按鍵的裝置
    private float verticalInput;     // 設定上下方向按鍵的裝置
    private Vector3 moveDirection;   // Vector3 表示三维向量的数据.<moveDirection>用于保存移动的方向向量
    private Rigidbody rbFirstPerson; // 第一人稱物件(膠囊體)的剛體

    void Start()
    {
        rbFirstPerson = GetComponent<Rigidbody>();  //獲取剛體信息放到rbFirstPerson
                                                    // rbFirstPerson.freezeRotation = true;  不需要,已設定        {鎖定第一人稱物件剛體旋轉，不讓膠囊體因為碰到物件就亂轉}
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();       //告知後面有這兩個程式碼  01
        SpeedControl();   // 偵測速度，過快就減速  02


        // 射線檢測？
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        //地面檢測
        if (grounded)
            rbFirstPerson.drag = groundDrag;
        else
            rbFirstPerson.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();     // 新增物件移動裝置，      
    }
    //Update()方法在每一帧渲染之前被调用，而FixedUpdate()方法在固定的时间间隔内被调用，通常是每个物理步骤。这意味着在FixedUpdate()方法中处理物体的移动会更加准确和一致，因为它不受帧率的影响
    // 方法：取得目前玩家按方向鍵上下左右的數值
    private void MyInput()  //01
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        //Input.GetAxisRaw("Horizontal") 用于获取玩家按下水平方向键（左右）的输入值。返回的值是一个浮点数，表示按键的状态，-1表示按下左键，1表示按下右键，0表示未按下。
        verticalInput = Input.GetAxisRaw("Vertical");//上下同理
    }
    private void MovePlayer()   //02
    {
        // 計算移動方向(其實就是計算X軸與Z軸兩個方向的力量)PlayerCamera.forward 表示相机的前向向量，指向相机所面向的方向。
        //                                                 PlayerCamera.right 表示相机的右向向量，垂直于相机的前向向量，指向相机的右侧。
        moveDirection = PlayerCamera.forward * verticalInput + PlayerCamera.right * horizontalInput;
        // 推動第一人稱物件
        rbFirstPerson.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        //ForceMode.Force 表示施加一个力，该力将在连续的帧之间施加，并且受到物体的质量和阻力等因素的影响。
    }
    // 方法：偵測速度並減速
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z); // 取得僅X軸與Z軸的平面速度
        //0f 是一个表示浮点数 0 的写法

        // 如果平面速度大於預設速度值，就將物件的速度限定於預設速度值
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rbFirstPerson.velocity = new Vector3(limitedVel.x, rbFirstPerson.velocity.y, limitedVel.z);
        }
    }
}