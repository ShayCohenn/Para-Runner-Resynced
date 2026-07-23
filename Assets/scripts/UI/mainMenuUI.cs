using UnityEngine;
using UnityEngine.SceneManagement;

namespace ParaRunner.UI
{
    public class mainMenuUI : MonoBehaviour
    {
        public void playButtonClicked()
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}