using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ControlType
{
    Type1,  // Joystick + otomatik hız
    Type2,  // Variable joystick + speed butonu
    Type3   // Telefon tilt ile kontrol
}

public class SeagullController : MonoBehaviour
{
    public ControlType controlType;

    // Inputlar
    [Header("Input Settings")]
    public Joystick joystick;
    public Joystick variableJoystick;
    public Button speedButton;

    // UI
    [Header("UI Settings")]
    public TextMeshProUGUI speedText;
    private bool isStarted = false;
    [SerializeField] Button gameStartButton;
    public TMP_Dropdown controlDropdown;

    // Animator
    private Animator animator;

    // MiniGame
    [Header("MiniGame Settings")]
    [SerializeField] MiniGameManager miniGameManager;

    // Movement ve input sistemi
    private IControlInput controlInput;
    private SeagullMovement movement;

    private const float tiltSensivity = 2f;
    private const float tiltDeadZone = 0.05f;
    float speedUpdateTimer;


    private void Awake()
    {
        movement = GetComponent<SeagullMovement>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        CheckAndSaveControlType();

        if (gameStartButton != null)
            gameStartButton.onClick.AddListener(() => { isStarted = true; });

        if (speedButton != null)
            speedButton.onClick.AddListener(() => StartCoroutine(ReleaseSpeedButton()));

        controlDropdown.onValueChanged.AddListener(OnControlTypeChanged);
        SetControlType(controlType);
    }

    private void CheckAndSaveControlType()
    {
        // Kaydedilmiş control type varsa yükle
        if (PlayerPrefs.HasKey("ControlType"))
        {
            int savedIndex = PlayerPrefs.GetInt("ControlType");
            controlType = (ControlType)savedIndex;

            controlDropdown.onValueChanged.RemoveListener(OnControlTypeChanged);
            controlDropdown.value = savedIndex;
            controlDropdown.onValueChanged.AddListener(OnControlTypeChanged);
        }
    }

    private void SetControlType(ControlType type)
    {
        switch (type)
        {
            case ControlType.Type1:
                controlInput = new JoystickControl(joystick);
                if (!isStarted) return;

                joystick.gameObject.SetActive(!miniGameManager.IsMiniGameActive);

                variableJoystick.gameObject.SetActive(false);
                speedButton.gameObject.SetActive(false);

                break;
            case ControlType.Type2:
                controlInput = new VariableJoystickControl(variableJoystick);
                if (!isStarted) return;
                variableJoystick.gameObject.SetActive(!miniGameManager.IsMiniGameActive);
                speedButton.gameObject.SetActive(!miniGameManager.IsMiniGameActive);

                joystick.gameObject.SetActive(false);
                break;
            case ControlType.Type3:
                controlInput = new TiltControl(tiltSensivity, tiltDeadZone);
                if (!isStarted) return;
                speedButton.gameObject.SetActive(!miniGameManager.IsMiniGameActive);

                variableJoystick.gameObject.SetActive(false);
                joystick.gameObject.SetActive(false);
                break;
        }
    }

    private void OnControlTypeChanged(int index)
    {
        controlType = (ControlType)index;
        PlayerPrefs.SetInt("ControlType", index);
        SetControlType(controlType);
    }

    private IEnumerator ReleaseSpeedButton()
    {
        yield return new WaitForSeconds(0.1f);
        movement.isSpeedButtonPressed = false;
    }

    void Update()
    {
        if (!isStarted) return;

        SetControlType(controlType);

        // Input
        Vector2 dir = controlInput.GetDirection();
        bool boost = controlInput.IsSpeedBoostActive();

        movement.Move(dir.x, dir.y, controlType, boost);

        // Speed UI her 0.15 saniyede bir güncellenecek
        speedUpdateTimer += Time.unscaledDeltaTime;
        if (speedUpdateTimer >= 0.15f)
        {
            speedUpdateTimer = 0f;
            UpdateSpeedUI();
        }
    }

    void UpdateSpeedUI()
    {
        if (speedText != null)
        {
            float speedKmH = movement.GetSpeed() * 3.6f;
            speedText.SetText("{0:1} km/h", speedKmH); // TMP GC üretmez
        }
    }


    public void SeagullFlyAnimation()
    {
        animator.SetTrigger("FlyTrigger");
    }

    public void StartCatchMiniGame(Transform fish)
    {
        Debug.Log("MiniGame Başlıyor...");

        miniGameManager.StartCatchSequence(fish, animator,
            onSuccess: () => CatchSuccess(),
            onFail: () => CatchFail()
        );
    }

    void CatchSuccess() 
    {
        Debug.Log("Martı balığı YAKALADI!");
    }

    void CatchFail()
    {
        Time.timeScale = 1f; // Zamanı normal hıza getir
        Time.fixedDeltaTime = 0.02f; // Fizik zamanını normal hıza getir

        Debug.Log("Martı balığı KAÇIRDI!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sea") || collision.gameObject.CompareTag("Obstacle"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bound"))
        {
            transform.Rotate(0f, 180f, 0f, Space.World);
        }
    }
}
