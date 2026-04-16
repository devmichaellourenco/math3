using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtDialog;

    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialog(Dialog dialog)
    {
        txtName.text = dialog.name;

        sentences.Clear();

        foreach(string sentence in dialog.sentences){
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }
    
    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        txtDialog.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            txtDialog.text += letter;
            yield return null;
        }
    }

    void EndDialog()
    {
        Debug.Log("End of conversation");
    }
}
