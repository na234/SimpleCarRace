using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public Text timeText;       // タイム表示用のテキスト
    float startTime;            // 計測開始の時刻
    bool start, check, goal;    // 各地点の通過フラグ
    void Start()
    {
        timeText = GameObject.Find("TimeText").GetComponent<Text>();    // オブジェクトのコンポーネントを取得
        timeText.text = "TIME  00.000";         // テキストの初期化
    }
    void Update()
    {
        // スタートしてからゴールするまでタイムを表示
        if (!goal && start) { timeText.text = "TIME  " + (Time.time - startTime).ToString("00.000"); }

        // ゴール後に「R」キーを押すとリスタート
        if (goal && Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // トリガーオブジェクトに侵入した時に呼び出されるメソッド
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "StartPoint")
        {
            if (check)
            {
                goal = true;                    // チェックポイント通過済みならゴール
                timeText.color = Color.red;
            }
            else if (!start && !check)
            {
                start = true;                   // チェックポイントを通過していない場合、タイム計測開始
                startTime = Time.time;
            }
        }
        else if (start && other.gameObject.name == "CheckPoint")
        {
            check = true;                       // チェックポイントを通過
        }
    }
}
