using UnityEngine;

public class playermove : MonoBehaviour
{
    //    [Header("移動設定")]
    //   public float moveSpeed;     //移動的物體裝置
    [Header("移動設定")]
    public float moveSpeed;
    public float jumpForce;          // 跳躍力道
    public float jumpCooldown;       // 設定要幾秒後才能向上跳躍
    public float groundDrag;         // 地面的減速


    [Header("按鍵綁定")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("基本設定")]
    public Transform PlayerCamera;   // 攝影機

    [Header("地板確認")]
    public float playerHeight;       // 設定玩家高度
    public LayerMask whatIsGround;   // 設定哪一個圖層是射線可以打到的
    public bool grounded;            // 布林變數：有沒有打到地面

    private bool readyToJump;        // 設定是否可以跳躍
    private float horizontalInput;   // 左右方向按鍵的數值(-1 <= X <= +1)
    private float verticalInput;     // 上下方向按鍵的數值(-1 <= Y <= +1)

    private Vector3 moveDirection;   // 移動方向

    private Rigidbody rbFirstPerson; // 第一人稱物件(膠囊體)的剛體
                                     //    //<>
                                     //    public float jumpCooldown;       // 設定要幾秒後才能向上跳躍
                                     //    public float jumpForce;//裝置...同理放到Jump()計算
                                     //    //<>
                                     //    //,,,,,,,,,,,,,,
                                     //    [Header("按鍵綁定")]
                                     //    public KeyCode jumpKey = KeyCode.Space;  //根据需要修改jumpKey 跳跃操作的按键绑定。KeyCode.Space[空白建]

    //    //,,,,,,,,,,,,,,,,,,,,,,,
    //    [Header("攝影機抓取")]
    //    public Transform PlayerCamera;   // 攝影機
    //    //<>
    //    private bool readyToJump;        // 設定是否可以跳躍
    //   
    //    //<>
    //    private float horizontalInput;   // 設定左右方向按鍵的裝置
    //    private float verticalInput;     // 設定上下方向按鍵的裝置
    //    private Vector3 moveDirection;   // Vector3 表示三维向量的数据.<moveDirection>用于保存移动的方向向量
    //    private Rigidbody rbFirstPerson; // 第一人稱物件(膠囊體)的剛體

    //    void Start()
    //    {
    //        rbFirstPerson = GetComponent<Rigidbody>();  //獲取剛體信息放到rbFirstPerson
    // rbFirstPerson.freezeRotation = true;  不需要,已設定        {鎖定第一人稱物件剛體旋轉，不讓膠囊體因為碰到物件就亂轉}
    //    }

    // Update is called once per frame
    //    void Update()
    //    {
    //        MyInput();       //告知後面有這兩個程式碼  01
    //        SpeedControl();   // 偵測速度，過快就減速  02


    //    }

    //    private void FixedUpdate()
    //    {
    //        MovePlayer();     // 新增物件移動裝置，      
    //    }
    //    //Update()方法在每一帧渲染之前被调用，而FixedUpdate()方法在固定的时间间隔内被调用，通常是每个物理步骤。这意味着在FixedUpdate()方法中处理物体的移动会更加准确和一致，因为它不受帧率的影响
    //    // 方法：取得目前玩家按方向鍵上下左右的數值
    //    private void MyInput()  //01
    //    {
    //        horizontalInput = Input.GetAxisRaw("Horizontal");
    //Input.GetAxisRaw("Horizontal") 用于获取玩家按下水平方向键（左右）的输入值。返回的值是一个浮点数，表示按键的状态，-1表示按下左键，1表示按下右键，0表示未按下。
    //        verticalInput = Input.GetAxisRaw("Vertical");//上下同理
    //        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

    //       // 如果按下設定的跳躍按鍵
    //      if (Input.GetKey(jumpKey) == true) //如果jumpKey執行成功
    //        {
    //            Jump(); // 執行這個方法
    //        }

    //        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    //        // 如果按下設定的跳躍按鍵
    //        //<>
    //        if (Input.GetKey(jumpKey) && readyToJump)
    //        {
    //            readyToJump = false;
    //            Jump();
    //            Invoke(nameof(ResetJump), jumpCooldown); // 如果跳躍過後，就會依照設定的限制時間倒數，時間到了才能往上跳躍
    //        }
    //        //<>
    //    }
    //   // private void MovePlayer()   //02
    //   // {
    //        // 計算移動方向(其實就是計算X軸與Z軸兩個方向的力量)PlayerCamera.forward 表示相机的前向向量，指向相机所面向的方向。
    //        //                                                 PlayerCamera.right 表示相机的右向向量，垂直于相机的前向向量，指向相机的右侧。
    //     //   moveDirection = PlayerCamera.forward * verticalInput + PlayerCamera.right * horizontalInput;
    //        // 推動第一人稱物件
    //     //   rbFirstPerson.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    //        //ForceMode.Force 表示施加一个力，该力将在连续的帧之间施加，并且受到物体的质量和阻力等因素的影响。
    //    }
    ////  方法：偵測速度並減速

    // //,
    //private void MovePlayer()
    //{
    //    // 計算移動方向(其實就是計算X軸與Z軸兩個方向的力量)
    //    moveDirection = PlayerCamera.forward * verticalInput + PlayerCamera.right * horizontalInput;

    //    // 如果在地面，則可以移動
    //    if (grounded)
    //        rbFirstPerson.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    // }
    // //,



    //private void SpeedControl()
    //    {
    //        Vector3 flatVel = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z); // 取得僅X軸與Z軸的平面速度
    //0f 是一个表示浮点数 0 的写法

    // 如果平面速度大於預設速度值，就將物件的速度限定於預設速度值
    //        if (flatVel.magnitude > moveSpeed)
    //        {
    //            Vector3 limitedVel = flatVel.normalized * moveSpeed;
    //            rbFirstPerson.velocity = new Vector3(limitedVel.x, rbFirstPerson.velocity.y, limitedVel.z);
    //        }
    //    }
    // 方法：偵測速度並減速
    //private void SpeedControl()
    //{
    //    Vector3 flatVel = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z); // 取得僅X軸與Z軸的平面速度
    //
    //    // 如果平面速度大於預設速度值，就將物件的速度限定於預設速度值
    //    if (flatVel.magnitude > moveSpeed)
    //    {
    //        Vector3 limitedVel = flatVel.normalized * moveSpeed;
    //        rbFirstPerson.velocity = new Vector3(limitedVel.x, rbFirstPerson.velocity.y, limitedVel.z);
    //   }
    //}
    //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

    // 方法：跳躍
    //private void Jump()
    //    {
    // 重新設定Y軸速度
    //        rbFirstPerson.velocity = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z);
    // 由下往上推第一人稱物件，ForceMode.Impulse可以讓推送的模式為一瞬間，會更像跳躍的感覺
    //        rbFirstPerson.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    //   }

    // 方法：跳躍
    //private void Jump()
    //{
    // 重新設定Y軸速度
    //   rbFirstPerson.velocity = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z);
    //  // 由下往上推第一人稱物件，ForceMode.Impulse可以讓推送的模式為一瞬間，會更像跳躍的感覺
    //   rbFirstPerson.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    //}
    //<>
    // 方法：重新設定變數readyToJump為true的方法
    //private void ResetJump()
    //  {
    //        readyToJump = true;
    //   }
    //<>
    private void Start()
    {
        rbFirstPerson = GetComponent<Rigidbody>();
        rbFirstPerson.freezeRotation = true;         // 鎖定第一人稱物件剛體旋轉，不讓膠囊體因為碰到物件就亂轉
        readyToJump = true;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();   // 偵測速度，過快就減速

        // 射出一條看不到的射線，來判斷有沒有打到地面？
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        Debug.DrawRay(transform.position, new Vector3(0, -(playerHeight * 0.5f + 0.3f), 0), Color.red); // 在測試階段將射線設定為紅色線條，來看看線條長度夠不夠？
        // 如果碰到地板，就設定一個反作用力(這個可以製造人物移動的減速感)
        if (grounded)
            rbFirstPerson.drag = groundDrag;
        else
            rbFirstPerson.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();     // 只要是物件移動，建議你放到FixedUpdate()        
    }

    // 方法：取得目前玩家按方向鍵上下左右的數值，控制跳躍行為
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // 如果按下設定的跳躍按鍵
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // 如果跳躍過後，就會依照設定的限制時間倒數，時間到了才能往上跳躍
        }
    }

    private void MovePlayer()
    {
        // 計算移動方向(其實就是計算X軸與Z軸兩個方向的力量)
        moveDirection = PlayerCamera.forward * verticalInput + PlayerCamera.right * horizontalInput;

        // 如果在地面，則可以移動
        if (grounded)
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

    // 方法：跳躍
    private void Jump()
    {
        // 重新設定Y軸速度
        rbFirstPerson.velocity = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z);
        // 由下往上推第一人稱物件，ForceMode.Impulse可以讓推送的模式為一瞬間，會更像跳躍的感覺
        rbFirstPerson.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // 方法：重新設定變數readyToJump為true的方法
    private void ResetJump()
    {
        readyToJump = true;
    }
}

