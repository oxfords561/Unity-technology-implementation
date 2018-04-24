// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-04 17:04:19
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;

public class CameraCtr : MonoBehaviour {

    public static CameraCtr Instance;
    /// <summary>
    /// 控制摄像机上下
    /// </summary>
    [SerializeField]
    private Transform m_CameraUpAndDown;
    /// <summary>
    /// 控制摄像机缩放父物体
    /// </summary>
    [SerializeField]
    private Transform m_CameraZoomContainer;
    /// <summary>
    /// 摄像机容器
    /// </summary>
    [SerializeField]
    private Transform m_CameraContainer;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        m_CameraUpAndDown.localEulerAngles = new Vector3(0, 0, Mathf.Clamp(m_CameraUpAndDown.localEulerAngles.z, 10f, 80f));
    }

    /// <summary>
    /// 设置摄像机旋转 
    /// </summary>
    /// <param name="type">0=左 1=右</param>
    public void SetCameraRotate(int type)
    {
        transform.Rotate(0,80*Time.deltaTime * (type == 1?-1:1),0);
    }

    /// <summary>
    /// 设置摄像机上下 
    /// </summary>
    /// <param name="type">0=上 1=下</param>
    public void SetCameraUpAndDown(int type)
    {
        m_CameraUpAndDown.Rotate(0,0, 30 * Time.deltaTime * (type == 1 ? -1 : 1));
        m_CameraUpAndDown.localEulerAngles = new Vector3(0,0,Mathf.Clamp(m_CameraUpAndDown.localEulerAngles.z,10f,80f));
    }

    /// <summary>
    /// 设置摄像机缩放 
    /// </summary>
    /// <param name="type">0=拉近 1=拉远</param>
    public void SetCameraZoom(int type)
    {
        m_CameraContainer.Translate(Vector3.forward * 20 * Time.deltaTime * (type == 1 ? -1 : 1));
        m_CameraContainer.localPosition = new Vector3(0, 0, Mathf.Clamp(m_CameraContainer.localPosition.z, -5f, 8f));
    }

    public void AutoLookAt(Vector3 pos)
    {
        m_CameraZoomContainer.LookAt(pos);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,15f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 14f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 12f);
    }
}
