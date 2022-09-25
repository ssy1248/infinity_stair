using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ObjectManager objectManager;
    public DSLManager dslManager;
    public DontDestory dontDestroy;

    public GameObject[] players, stairs, UI;
    public GameObject pauseBtn, backGround;

    public AudioSource[] sound;
    public Animator[] anim;
    public Text finalScoreText, bestScoreText, scoreText;
    public Image gauge;
    public Button[] settingButton;

    int score, sceneCount, selectedIndex;

    Vector3 beforePos;
    Vector3 startPos = new Vector3(-0.8f, 1.5f, 0),
        leftPos = new Vector3(-0.8f, 0.4f, 0),
        rightPos = new Vector3(0.8f, -0.4f, 0),
        leftDir = new Vector3(0.8f, -0.4f, 0),
        rightDir = new Vector3(-0.8f, -0.4f, 0);

    public bool[] isChangeDir = new bool[20];
    public bool gauageStart = false;
    public bool isGamePaused = false;
    public bool vibrateOn = true;
    float gaugeReductionRate = 0.0025f;

    enum State { START, LEFTDIR, RIGHTDIR}
    State state = State.START;

    private void Awake()
    {
        players[selectedIndex].SetActive(true);
        player = players[selectedIndex].GetComponent<Player>();

        StairsInit();
        GaugeReduce();
        StartCoroutine(CheckGauge());

        UI[0].SetActive(dslManager.IsRetry());
        UI[1].SetActive(!dslManager.IsRetry());
    }

    void GaugeReduce()
    {
        if(gauageStart)
        {
            if (score > 30)
                gaugeReductionRate = 0.0033f;
            if (score > 60)
                gaugeReductionRate = 0.0037f;
            if (score > 100)
                gaugeReductionRate = 0.0043f;
            if (score > 150)
                gaugeReductionRate = 0.005f;
            if (score > 200)
                gaugeReductionRate = 0.005f;
            if (score > 300)
                gaugeReductionRate = 0.0065f;
            if (score > 400)
                gaugeReductionRate = 0.0075f;

            gauge.fillAmount -= gaugeReductionRate;
        }

        Invoke("GaugeReduce", 0.01f);
    }

    IEnumerator CheckGauge()
    {
        while(gauge.fillAmount != 0)
        {
            yield return new WaitForSeconds(0.4f);
        }
        GameOver();
    }

    void GameOver()
    {
        anim[0].SetBool("GameOver", true);
        player.animator.SetBool("Die", true);

        ShowScore();
        pauseBtn.SetActive(false);

        player.isDie = true;
        player.MoveAnimation();

        CancelInvoke();
    }

    void ShowScore()
    {
        finalScoreText.text = score.ToString();
        dslManager.SaveRankScore(score);
        bestScoreText.text = dslManager.GetBestScore().ToString();

        if (score == dslManager.GetBestScore() && score != 0)
            UI[2].SetActive(true);
    }

    void StairsInit() //계단을 생성하는 함수
    {
        for (int i = 0; i < 20; i++)
        {
            switch(state)
            {
                case State.START:
                    stairs[i].transform.position = startPos;
                    state = State.LEFTDIR;
                    break;
                case State.LEFTDIR:
                    stairs[i].transform.position = beforePos + leftPos;
                    break;
                case State.RIGHTDIR:
                    stairs[i].transform.position = beforePos + rightPos;
                    break;
            }

            beforePos = stairs[i].transform.position;

            if (i != 0)
            {
                if (Random.Range(1, 9) < 3)
                    objectManager.MakeObj("coin", i);
                if(Random.Range(1, 9) < 3 && i < 19)
                {
                    if (state == State.LEFTDIR)
                        state = State.RIGHTDIR;
                    else if(state == State.RIGHTDIR)
                        state = State.LEFTDIR;

                    isChangeDir[i + 1] = true;
                }
            }
        }
    }

   void SpawnStair(int num)
    {
        isChangeDir[num + 1 == 20 ? 0 : num + 1] = false;
        beforePos = stairs[num == 0 ? 19 : num - 1].transform.position;

        switch(state)
        {
            case State.LEFTDIR:
                stairs[num].transform.position = beforePos + leftPos;
                break;
            case State.RIGHTDIR:
                stairs[num].transform.position = beforePos + rightPos;
                break;
        }

        if (Random.Range(1, 9) < 3)
            objectManager.MakeObj("coin", num);
        if (Random.Range(1, 9) < 3)
        {
            if (state == State.LEFTDIR)
                state = State.RIGHTDIR;
            else if (state == State.RIGHTDIR)
                state = State.LEFTDIR;

            isChangeDir[num + 1 == 20 ? 0 : num + 1] = true;
        }
    }

    public void StairMove(int stairIndex, bool isChange, bool isLeft)
    {
        if (player.isDie)
            return;

        for(int i = 0; i < 20; i++)
        {
            if (isLeft)
                stairs[i].transform.position += leftDir;
            else
                stairs[i].transform.position += rightDir;
        }

        for(int i = 0; i < 20;  i++)
        {
            if (stairs[i].transform.position.y < -5)
                SpawnStair(i);
        }

        if(isChangeDir[stairIndex] != isChange)
        {
            GameOver();
            return;
        }

        backGround.transform.position += backGround.transform.position.y < -14f ?
            new Vector3(0, 4.7f, 0) : new Vector3(0, -0.05f, 0);
    }

    public void BtnDown(GameObject btn)
    {
        btn.transform.localPosition = new Vector3(0.8f, 0.8f, 0.8f);
        if (btn.name == "ClimbBtn")
            player.Climb(false);
        else if (btn.name == "ChangeDirBtn")
            player.Climb(true);
    }

    public void BtnUp(GameObject btn)
    {
        btn.transform.localPosition = new Vector3(1f, 1f, 1f);
        if(btn.name == "PauseBtn")
        {
            CancelInvoke();
            isGamePaused = true;
        }
        else if(btn.name == "ResumeBtn")
        {
            GaugeReduce();
            isGamePaused = false;
        }
    }
}
