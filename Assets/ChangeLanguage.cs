using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLanguage : MonoBehaviour
{
    public string  language;
    void Start()
    {
        
    }

public void ChangeLanguageToPortuguese(){
    language = "portuguese";
    SetLanguage(language);
}
public void ChangeLanguageToEnglish(){
    language = "english";
    SetLanguage(language);
}
public void SetLanguage(string newLanguage){
           if (!ES2.Exists("language"))
       {        
           ES2.Save(newLanguage, "language");
            this.language = ES2.Load<string>("language");
       } else{
           ES2.Save(newLanguage, "language");
            this.language = ES2.Load<string>("language");
       }
}
}
