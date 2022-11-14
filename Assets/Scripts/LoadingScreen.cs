using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace HeneGames.Sceneloader
{
    public class LoadingScreen : MonoBehaviour
    {
        private float progress;

        [Header("References")]
        [SerializeField]
        private GameObject loadingContent;
        [SerializeField]
        private Transform movingLoadingThing;
        [SerializeField]
        private Image loadingFillImage;
        [SerializeField]
        private Text loadingText;

        [Header("Loading knob positions")]
        [SerializeField]
        private Transform startPoint;
        [SerializeField]
        private Transform endPoint;

        //  开始游戏按钮
        [Header("Buttons")]
        [SerializeField]
        private GameObject[] btns;

        private void Awake()
        {
            loadingContent.SetActive(false);
        }

        public void LoadScene(int startType)
        {
            foreach (GameObject g in btns)
            {
                g.SetActive(false);
            }
            StartCoroutine(LoadAsynchronously(1));

            GameSetting gs = new GameSetting();
            gs.startMode = startType;
            GameSettingConfig.Instance.SaveSetting(gs);
        }

        private float fakeProgress = 0f;
        IEnumerator LoadAsynchronously(int _sceneIndex)
        {
            loadingContent.SetActive(true);
            //  场景加载的太快了，这里弄一个假的进度条
            while (fakeProgress < 0.9f)
            {
                //  虚拟进度条，计算进度
                fakeProgress += 0.001f;
                float _progress = Mathf.Clamp01(fakeProgress / 0.9f);
                float _prosentProgress = _progress * 100f;
                loadingFillImage.fillAmount = _progress;
                loadingText.text = _prosentProgress.ToString("F0") + "%";
                movingLoadingThing.localPosition = new Vector3(Mathf.Lerp(startPoint.localPosition.x, endPoint.localPosition.x, _progress), 0f, 0f);
                progress = _prosentProgress;

                yield return null;
            }

            //  加载场景
            AsyncOperation _opearation = SceneManager.LoadSceneAsync(_sceneIndex);
            while (!_opearation.isDone)
            {
                //float _progress = Mathf.Clamp01(_opearation.progress / 0.9f);
                //float _prosentProgress = _progress * 100f;
                //loadingFillImage.fillAmount = _progress;
                //loadingText.text = _prosentProgress.ToString("F0") + "%";
                //movingLoadingThing.localPosition = new Vector3(Mathf.Lerp(startPoint.localPosition.x, endPoint.localPosition.x, _progress), 0f, 0f);
                //progress = _prosentProgress;

                yield return null;
            }
        }

        public float Progress()
        {
            return progress;
        }
    }
}