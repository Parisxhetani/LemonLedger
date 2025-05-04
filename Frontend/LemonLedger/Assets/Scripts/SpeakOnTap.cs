using UnityEngine;
using UnityEngine.UI;     // Image
using TMPro;              // TextMeshPro
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Fade timing")]
    [SerializeField] private float fadeInDuration  = 1f;
    [SerializeField] private float fadeOutDuration = 1f;

    [SerializeField] private bool  startFaded = true;

    /* ─── References ──────────────────────────────────────────────── */
    private TextMeshProUGUI raySpeak;   // “RaySpeak”  (Text)
    private Image           commentImage; // “Comment”  (UI Image)
    private SpriteRenderer  raiSprite;    // “Rai_0”    (SpriteRenderer)

    /* ─── Dialogue content ───────────────────────────────────────── */
    private readonly string[] segments = {
        "“Hey there! I’m Rai, your friendly financial guide, and welcome to LemonLedger!",
        " I’m here to help you turn a simple lemonade stand into a thriving little business—",
        "while teaching you all about budgeting, credit, and smart money moves along the way.”",
        "“See that sunny street? That’s your canvas! Every morning, you’ll draw from",
        " a deck of Event Cards—things like surprise fees, marketing boosters, or tempting upgrades.”",
        "“You’ll choose wisely, balance your expenses and earnings, and keep your credit score healthy.",
        "Hit the milestone ‘First Loan Paid,’ and I’ll be right here celebrating with confetti!”",
        "“But first, every great entrepreneur needs a little seed money.",
        "To buy your very first lemonade stand—complete with cups, lemons, and a snazzy awning—you’ll",
        "take a micro-loan from RaiffeiFund, our youth-friendly lending program.”",
        "“Don’t worry, I’ll walk you through each step, and repayments are simple.😊”",
        "“So… how much would you like to borrow to get your lemonade stand up and running?”"
    };
    private int currentIndex = 0;

    /* ─────────────────────────────────────────────────────────────── */

    void Awake()
    {
        /* --- RaySpeak ------------------------------------------------ */
        var go = GameObject.Find("RaySpeak");
        if (go == null || (raySpeak = go.GetComponent<TextMeshProUGUI>()) == null)
        {
            Debug.LogError("RaySpeak (TextMeshProUGUI) not found."); enabled = false; return;
        }

        /* --- Comment (UI Image) -------------------------------------- */
        var commentGO = GameObject.Find("Comment");
        if (commentGO == null || (commentImage = commentGO.GetComponent<Image>()) == null)
        {
            Debug.LogError("Comment (Image) not found."); enabled = false; return;
        }

        /* --- Rai_0 (SpriteRenderer) ---------------------------------- */
        var raiGO = GameObject.Find("Rai_0");
        if (raiGO == null || (raiSprite = raiGO.GetComponent<SpriteRenderer>()) == null)
        {
            Debug.LogError("Rai_0 (SpriteRenderer) not found."); enabled = false; return;
        }

        /* --- Start transparent if we fade-in ------------------------- */
        if (startFaded)
        {
            commentImage.color = WithA(commentImage.color, 0f);
            raiSprite.color    = WithA(raiSprite.color,    0f);
            raySpeak.color     = WithA(raySpeak.color,     0f);
        }
    }

    /* ─── Unity lifecycle ──────────────────────────────────────────── */

    void Start()
    {
        if (startFaded)
        {
            enabled = false;                 // ignore taps while fading-in
            StartCoroutine(FadeInRoutine());
        }
        else
        {
            ShowNextLine();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ShowNextLine();
    }

    /* ─── Dialogue flow ───────────────────────────────────────────── */

    private void ShowNextLine()
    {
        if (currentIndex < segments.Length)
        {
            raySpeak.text = segments[currentIndex++];
        }
        else
        {
            enabled = false;                 // ignore further taps
            StartCoroutine(FadeOutRoutine());
            ChangeScene();
        }
    }

    /* ─── Fade-in / Fade-out coroutines ───────────────────────────── */

    private IEnumerator FadeInRoutine()
    {
        ShowNextLine();                      // first line (still invisible)

        float t = 0f;
        Color textStart    = raySpeak.color;
        Color commentStart = commentImage.color;
        Color raiStart     = raiSprite.color;

        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0f, 1f, t / fadeInDuration);

            commentImage.color = WithA(commentStart, a);
            raiSprite.color    = WithA(raiStart,   a);
            raySpeak.color     = WithA(textStart,  a);

            yield return null;
        }

        enabled = true;                      // user can tap now
    }

    private IEnumerator FadeOutRoutine()
    {
        float t = 0f;
        Color textStart    = raySpeak.color;
        Color commentStart = commentImage.color;
        Color raiStart     = raiSprite.color;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t / fadeOutDuration);

            commentImage.color = WithA(commentStart, a);
            raiSprite.color    = WithA(raiStart,   a);
            raySpeak.color     = WithA(textStart,  a);

            yield return null;
        }

        commentImage.gameObject.SetActive(false);
        raiSprite.gameObject.SetActive(false);
        raySpeak.gameObject.SetActive(false);
    }

    /* ─── Utility ─────────────────────────────────────────────────── */

    private static Color WithA(Color c, float a) => new Color(c.r, c.g, c.b, a);

    public void ChangeScene()
    {
        SceneManager.LoadScene("TakeLoan");
    }
}
