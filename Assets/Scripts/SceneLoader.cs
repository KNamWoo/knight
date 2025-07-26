using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum SceneNames { StartHome, Player }

namespace UnityNote {
    public class SceneLoader : MonoBehaviour {
        public static SceneLoader instance { get; private set; }

        [SerializeField]
        private GameObject loadingScreen; // 로딩 화면
        
        [SerializeField]
        private Image loadingBackground;

        [SerializeField]
        private Sprite[] loadingSprites;
        
        [SerializeField]
        private Slider loadingProgress;
        
        [SerializeField]
        private TextMeshProUGUI textProgress;

        private WaitForSeconds waitChangeDelay;
        private void Awake() {
            if (instance != null && instance != this) {
                Destroy(gameObject);
            }
            else {
                instance = this;
                waitChangeDelay = new WaitForSeconds(0.5f);

                DontDestroyOnLoad(gameObject);
            }
            loadingScreen.SetActive(false);
        }
        
        public void LoadScene(string name) {
            int index = Random.Range(0, loadingSprites.Length);
            loadingBackground.sprite = loadingSprites[index];
            loadingProgress.value = 0f;
            loadingScreen.SetActive(true);

            StartCoroutine(LoadSceneAsync(name));
        }
        
        public void LoadScene(SceneNames name) {
            LoadScene(name.ToString());
        }

        public float CurrentProgress{ get; private set; } = 0f;
        
        private IEnumerator LoadSceneAsync(string name) {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
            asyncOperation.allowSceneActivation = false; // 씬 전환 지연
            
            // 비동기 작업(씬 불러오기)이 완료될 때까지 반복
            // => 비동기 작업은 어느 한 작업을 수행하는 동안 다른 작업도 수행할 수 있는 방식임
            // 0 ~ 50% 씬 로딩
            while (asyncOperation.progress < 0.9f) {
                float progress = asyncOperation.progress * 0.5f;
                CurrentProgress = progress;
                loadingProgress.value = progress;
                textProgress.text = $"{Mathf.RoundToInt(progress * 100)}%";
                
                yield return null;
            }
            
            // 다음 씬 로딩완료 대기
            CurrentProgress = 0.5f;
            loadingProgress.value = 0.5f;
            textProgress.text = "50%";
            
            yield return waitChangeDelay;
            asyncOperation.allowSceneActivation = true;

            // 50 ~ 100% 세이브 데이터 로딩
            GameManager gm = GameManager.instance;
            gm.PlayerPrefab = GameObject.Find("Player");
            //gm.quickSlot = GameObject.FindGameObjectWithTag("");
            yield return StartCoroutine(LoadGameData());

            loadingProgress.value = 1.0f;
            textProgress.text = "100%";

            /*float fakeFill = 0.9f;
            float duration = 0.3f;
            
            while (fakeFill < 1.0f) {
                fakeFill += Time.deltaTime / duration;
                CurrentProgress = fakeFill;
                loadingProgress.value = fakeFill;
                textProgress.text = $"{Mathf.RoundToInt(fakeFill * 100)}%";

                yield return null;
            }*/

            //yield return waitChangeDelay;
            //asyncOperation.allowSceneActivation = true;
            loadingScreen.SetActive(false);
        }
        
        private IEnumerator LoadGameData() {
            SaveData saveData = LoadSystem.LoadGameData();
            
            if (saveData == null) {
                yield break;
            }

            float loadProgress = 0.5f;

            GameManager.instance.playerPosition = saveData.playerData.positions[0];
            loadProgress += 0.1f;
            loadingProgress.value = loadProgress;
            textProgress.text = $"{Mathf.RoundToInt(loadProgress * 100)}%";
            yield return null;

            yield return new WaitForSeconds(0.1f);
            GameManager gm = GameManager.instance;
            StartCoroutine(gm.LoadAfterSceneLoad(saveData));
            loadProgress = 1.0f;
            loadingProgress.value = loadProgress;
            textProgress.text = "100%";

            yield return null;
        }
    }
}
