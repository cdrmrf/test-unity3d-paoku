using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControl : IGameReset
{

    private enum DirectionEnum
    {
        RIGHT,
        LEFT,
    }

    private Rigidbody2D rbody;
    private Animator animator;

    private bool isOnGround;
    private int jumpCounter = 0;
    private DirectionEnum curDirection;

    private PlayerService service;
    private GameManage manage;
    private GameObject collision = null;
    private bool rebornStatus = false;

    //  角色初始位置
    private Vector3 playerInitPosition;

    void Awake()
    {
        playerInitPosition = transform.position;
        service = new PlayerService();
        curDirection = DirectionEnum.RIGHT;
        //  注册事件监听
        EventCenter.GetInstance().AddEventListener2(EventEnum.GAME_RESET, Reset);
    }

    // Start is called before the first frame update
    void Start()
    {
        manage = GameManage.INSTANCE;
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manage.IsRunning())
        {
            OnKeyDown();
        }
        //  游戏暂停
        var prevSpeed = animator.speed;
        if (manage.IsRunning() && prevSpeed <= 0)
        {
            animator.speed = 1.5f;
        }
        else if (!manage.IsRunning() && prevSpeed > 0)
        {
            animator.speed = 0f;
        }
        //  重生
        if (manage.gameStatus == GameStatusEnum.REBORN && !rebornStatus)
        {
            Reborn();
            rebornStatus = true;
        }
    }

    private void OnKeyDown()
    {
        //  空格跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.D))
        {
            //  按 D 键 前进
            if (curDirection == DirectionEnum.LEFT)
            {
                curDirection = DirectionEnum.RIGHT;
                transform.Rotate(0f, 180f, 0f);
            }
            //  每过一关速度提升一点
            Move(0.005f * GameManage.INSTANCE.playerInfo.level);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //  按 A 健后退
            if (curDirection == DirectionEnum.RIGHT)
            {
                curDirection = DirectionEnum.LEFT;
                transform.Rotate(0f, 180f, 0f);
            }
            Move(0.015f * GameManage.INSTANCE.playerInfo.level * -1);
        }
    }

    //  角色移动
    private void Move(float distance)
    {
        //  获取角色当前位置
        Vector3 playerPostion = transform.position;
        playerPostion.x = playerPostion.x + distance;
        //  -2   是最左边的边界
        //  7.2 是最右边的边界
        if (playerPostion.x < -2 || playerPostion.x > 7.2)
        {
            return;
        }
        transform.position = playerPostion;
    }

    public void Jump()
    {
        if (!manage.IsRunning())
        {
            return;
        }
        //  2段跳
        if (isOnGround || jumpCounter < 2)
        {
            rbody.AddForce(Vector2.up * 400);
            AudioSourceControl.Instance.Play("跳");
            jumpCounter++;
        }
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        //  过关
        if (coll.collider.tag == "EndGround")
        {
            isOnGround = true;
            jumpCounter = 0;
            animator.SetBool("IsJump", false);
            GameManage.INSTANCE.SetGameStatus(GameStatusEnum.PASS);
        }
        //  死了
        if (coll.collider.tag == "Die")
        {
            die();
        }
        if (coll.collider.tag == "Enemy")
        {
            collision = coll.collider.gameObject;
            die();
        }
        //  回到地面
        if (coll.collider.tag.Contains("Ground"))
        {
            isOnGround = true;
            jumpCounter = 0;
            animator.SetBool("IsJump", false);
        }
    }

    public void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.collider.tag.Contains("Ground"))
        {
            isOnGround = false;
            animator.SetBool("IsJump", true);
        }
    }

    private void die()
    {
        rebornStatus = false;
        if (!manage.IsRunning())
        {
            return;
        }
        AudioSourceControl.Instance.Play("Boss死了");
        animator.SetBool("IsDie", true);

        //  三次复活机会
        var player = manage.playerInfo;
        if (player.HP > 0)
        {
            manage.SetGameStatus(GameStatusEnum.WAIT_REBORN);
        } else
        {
            //  扣一命
            if (player.life > 1)
            {
                player.life -= 1;
                manage.SetGameStatus(GameStatusEnum.DEAD);
            }
            else
            {
                //  游戏结束
                manage.SetGameStatus(GameStatusEnum.GAME_OVER);
            }
        }
        //  保存用户信息
        service.save(manage.playerInfo);
    }

    public override void Reset()
    {
        GameManage.INSTANCE.playerInfo.ResetHP();
        transform.position = playerInitPosition;
        animator.SetBool("IsDie", false);
        animator.SetBool("IsJump", false);
        collision = null;
    }

    /// <summary>
    /// 角色重生
    /// </summary>
    private void Reborn()
    {
        //  扣除一次复活机会
        var player = manage.playerInfo;
        player.HP -= 1;
        service.save(manage.playerInfo);

        //  玩家当前的位置
        float playerPosition = transform.position.x;
        float rebornPositionX = 0f;
        float rebornPositionY = transform.position.y;

        //  碰撞到障碍物死掉的，原地复活，销毁障碍物
        if (collision != null)
        {
            rebornPositionX = playerPosition;
            Destroy(collision);
            collision = null;
        } else
        {
            //  掉下去死的，复活在下一块土地上
            var g = GetRebornGround();
            if(g == null)
            {
                manage.SetGameStatus(GameStatusEnum.GAME_OVER);
                return;
            }
            //  清除土地上的所有元素
            ClearGround(g);
            //  重生后的位置
            rebornPositionY = g.transform.position.y + 1f;
            float width = g.gameObject.GetComponent<Renderer>().bounds.size.x;
            rebornPositionX = g.transform.position.x - (width / 2) + 1f;
        }
        //  修改位置
        transform.position = new Vector3(rebornPositionX, rebornPositionY, 1f);
        animator.SetBool("IsDie", false);
        animator.SetBool("IsJump", false);

        //  修正方向
        if (curDirection == DirectionEnum.LEFT)
        {
            curDirection = DirectionEnum.RIGHT;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    //  重生在离自己最近的一块土地上
    private GameObject GetRebornGround()
    {
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
        float playerPosition = transform.position.x;
        foreach (GameObject ground in grounds)
        {
            float start = ground.transform.position.x;
            if(start >= playerPosition)
            {
                return ground;
            }
        }
        return null;
    }

    //  清除土地上的所有障碍物
    private void ClearGround(GameObject ground)
    {
        int cc = ground.transform.childCount;
        for (int i = 0; i < cc; i++)
        {
            var g = ground.transform.GetChild(i);
            Destroy(g.gameObject);
        }
    }

}