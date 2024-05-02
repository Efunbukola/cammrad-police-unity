using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ai_suggestions_script : MonoBehaviour
{
    
    public Text suggestionText, heading;
    public GameObject suggestionIcon;
    public CAMMRADCacheManager cacheManager;

    // Start is called before the first frame update
    void Start()
    {
        suggestionText.enabled = false;
        heading.enabled = false;
        suggestionIcon.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void showSuggestion(string id){
        heading.enabled = true;
        suggestionText.enabled = true;
        suggestionIcon.active = true;

       	switch (id) {
			case "EMPATHY":
				suggestionText.text = "Express empathy by acknowledging a person's responses and emotions";
                cacheManager.GetComponent<CAMMRADCacheManager>().LoadSuggestionIcon("empathy_icon");
				break;
            case "QUESTIONS":
				suggestionText.text = "Ask open-ended questions";
                cacheManager.GetComponent<CAMMRADCacheManager>().LoadSuggestionIcon("questions_icon");
				break;
            case "MIRROR":
				suggestionText.text = "Mirror/Paraphrase response";
                cacheManager.GetComponent<CAMMRADCacheManager>().LoadSuggestionIcon("mirror_icon");
				break;
            case "LOWER":
				suggestionText.text = "Lower voice";
                cacheManager.GetComponent<CAMMRADCacheManager>().LoadSuggestionIcon("lower_icon");
				break;
            case "INTERRUPT":
				suggestionText.text = "Don’t Interrupt";
                cacheManager.GetComponent<CAMMRADCacheManager>().LoadSuggestionIcon("interrupt_icon");
				break;
            case "SOLUTION":
				suggestionText.text = "Provide a solution";
                cacheManager.GetComponent<CAMMRADCacheManager>().LoadSuggestionIcon("solution_icon");
				break;
			}

    }

}
