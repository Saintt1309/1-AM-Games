using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueTrigger : MonoBehaviour 
{
    [Header("UI References")]
    public GameObject dialogueBox;
    public Text dialogueText;
    public GameObject continuePrompt;

    [Header("Dialogue Settings")]
    [TextArea(3, 10)] public string[] dialogueLines;
    public float typingSpeed = 0.05f;
    public AudioClip dialogueSound;
    public KeyCode continueKey = KeyCode.E;

    private AudioSource audioSource;
    // ...existing code...
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private int currentLine = 0;
    private Coroutine typingCoroutine;

    void Start()
    {
        audioSource = dialogueBox.GetComponent<AudioSource>();
        // ...existing code...
        dialogueBox.SetActive(false);
        continuePrompt.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && !isDialogueActive) 
            StartDialogue();
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(continueKey))
        {
            if (isTyping)
            {
                // Skip typing effect
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogueLines[currentLine - 1];
                isTyping = false;
                continuePrompt.SetActive(true);
            }
            else
            {
                ShowNextLine();
            }
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        currentLine = 0;
        dialogueBox.SetActive(true);
        ShowNextLine();
    }

    void ShowNextLine()
    {
        if (currentLine < dialogueLines.Length)
        {
            typingCoroutine = StartCoroutine(TypeText(dialogueLines[currentLine]));
            currentLine++;
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        continuePrompt.SetActive(false);
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            if (dialogueSound != null && letter != ' ')
                audioSource.PlayOneShot(dialogueSound);
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        continuePrompt.SetActive(currentLine < dialogueLines.Length);
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        dialogueBox.SetActive(false);
        continuePrompt.SetActive(false);
        Destroy(gameObject); // Remove trigger after use
    }
}
