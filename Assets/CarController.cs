using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject[] wheelMeshes = new GameObject[4];            // タイヤオブジェクトを格納する配列
    public WheelCollider[] wheelColliders = new WheelCollider[4];   // Wheel Colliderを格納する配列
    public float maxMotorTorque = 300f;     // 車輪に加える最大トルク
    public float brakeTorque = 500f;        // ブレーキのトルク
    public float maxSteerAngle = 25f;       // ステアリングの最大舵角
    float accel, steer;                     // アクセルとステアリングの入力値
    bool brake;                             // ブレーキをかけているかどうか
    GameObject brakeLight, headLight;       // ランプ類のオブジェクト

    // ゲーム開始時に1回のみ実行されるメソッド
    void Start()
    {
        // ランプ類のオブジェクトを探して取得
        brakeLight = GameObject.Find("SkyCarBrakeLightsGlow");
        headLight = GameObject.Find("SkyCarHeadLightsGlow");
    }

    // 画面を描画するたびに実行されるメソッド(実行間隔はフレームレートに依存)
    void Update()
    {
        // ユーザからの入力
        accel = Input.GetAxis("Horizontal");    // ←→で旋回
        steer = Input.GetAxis("Vertical");      // ↑↓でアクセル
        brake = Input.GetKey(KeyCode.Space);    // スペースでブレーキ

        // ランプ類の点灯・消灯
        brakeLight.SetActive(brake);
        if (Input.GetKeyDown(KeyCode.H))
        {
            headLight.SetActive(!headLight.activeSelf);
        }

        // Wheel Colliderの動きを見た目に反映する
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].GetWorldPose(out Vector3 position, out Quaternion rotation);
            wheelMeshes[i].transform.position = position;
            wheelMeshes[i].transform.rotation = rotation;
        }
    }

    // フレームレートに依存せず、定期的に実行されるメソッド(0.02秒に1回)
    void FixedUpdate()
    {
        // Wheel Colliderに各パラメータを代入
        for (int i = 0; i < 4; i++)
        {
            if (i < 2) { wheelColliders[i].steerAngle = steer * maxSteerAngle; }    // ステアリング(前輪のみ)
            wheelColliders[i].motorTorque = accel * maxMotorTorque;                 // アクセル
            wheelColliders[i].brakeTorque = brake ? brakeTorque : 0f;               // ブレーキ
        }
    }
}