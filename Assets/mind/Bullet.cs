using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject enemyExplosion; // 敵人被銷毀時播放的特效
    void Start()
    {
        Destroy(gameObject, 10); // 子彈預設十秒後會自動刪除自己
    }

    // 碰撞偵測：如果碰到一個物件帶有「Target」標籤，則刪除自己
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            // 播放特效
            Instantiate(enemyExplosion, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject); // 碰撞到敵人時，銷毀敵人物件
            Destroy(gameObject);
        }
    }
}
