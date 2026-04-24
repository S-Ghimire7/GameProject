using UnityEngine;
using UnityEngine.SceneManagement;

public class TUTEndScreen : MonoBehaviour
{
   public void fr()
    {
        SceneManager.LoadScene("City");
    }

    public void mm()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
