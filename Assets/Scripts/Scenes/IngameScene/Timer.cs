using UnityEngine;
using TMPro;

namespace Scenes.IngameScene
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerUi;
        [SerializeField] private TextMeshProUGUI star3;
        [SerializeField] private TextMeshProUGUI star2;
        [SerializeField] private TextMeshProUGUI floor;

        [Header ("Debug")]
        [SerializeField] private float debugTime = 0f;

        public float elapsedTime;
        private bool isStop = true;
        private float star2Time;
        private float star3Time;

        void Awake()
        {
            elapsedTime += debugTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (isStop) return;

            elapsedTime += Time.deltaTime;
            timerUi.text = TimeText;
        }

        public void StartTimer()
        {
            isStop = false;
        }

        public void StopTimer()
        {
            isStop = true;
        }

        public void SetStar3Time(float time)
        {
            star3Time = time;
            star3.text = ConvertTimeText(time);
        }

        public void SetStar2Time(float time)
        {
            star2Time = time;
            star2.text = ConvertTimeText(time);
        }

        public void SetFloorText(int floorNum)
        {
            floor.text = floorNum.ToString();
        }

        public string TimeText
        {
            get {
                return ConvertTimeText(elapsedTime);
            }
        }

        public string ConvertTimeText(float time)
        {
            var minute = (int)time/60;
            var sec = (int)time%60;
            var msec = (int)(time*10%10);
            return string.Format("{0:0}:{1:00}.{2:0}", minute, sec, msec);
        }

        public int GetStar
        {
            get {
                if (elapsedTime <= star3Time) return 3;
                if (elapsedTime <= star2Time) return 2;
                return 1;
            }
        }
    }
}
