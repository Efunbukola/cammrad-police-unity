using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Task_List_Controller : MonoBehaviour {

	public CAMMRADMainController mainController;
	public Text task1, task2, task3, task4, task5;
	// Use this for initialization
	void OnEnable() {

		task1.text = "";
		task2.text = "";
		task3.text = "";
		task4.text = "";
		task5.text = "";


		Debug.Log("start");

		if (!mainController.isWorkflowInit) {
			gameObject.SetActive(false);
			return;
		}


		int count = 0;

		foreach (CAMMRADMainController.CAMMRADWorkflowTask task in mainController.workflow.tasks) {
			count++;
			switch (count) {

			case 1:
				task1.text = count + ". " + task.task.task_name;
				break;
			case 2:
				task2.text = count + ". " + task.task.task_name;
				break;
			case 3: 
				task3.text = count + ". " + task.task.task_name;
				break;
			case 4:
				task4.text = count + ". " + task.task.task_name;
				break;

			case 5:
				task5.text = count + ". " + task.task.task_name;
				break;

			}

			//task_list += "\n \n " + ;
			//Debug.Log("3r3r3r");
		}

		//tasks.text = task_list; 



	}
	
	// Update is called once per frame
	void Update () {
		
	}

		private static string[] ones = {
		    "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", 
		    "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen",
		};

		private static string[] tens = { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

		private static string[] thous = { "hundred", "thousand", "million", "billion", "trillion", "quadrillion" };

		public static string ToWords(decimal number)
		{
		    if (number < 0)
		        return "negative " + ToWords(System.Math.Abs(number));

		    int intPortion = (int)number;
		    int decPortion = (int)((number - intPortion) * (decimal) 100);

		    return string.Format("{0} dollars and {1} cents", ToWords(intPortion), ToWords(decPortion));
		}

		private static string ToWords(int number, string appendScale = "")
		{
		    string numString = "";
		    if (number < 100)
		    {
		        if (number < 20)
		            numString = ones[number];
		        else
		        {
		            numString = tens[number / 10];
		            if ((number % 10) > 0)
		                numString += "-" + ones[number % 10];
		        }
		    }
		    else
		    {
		        int pow = 0;
		        string powStr = "";

		        if (number < 1000) // number is between 100 and 1000
		        {
		            pow = 100;
		            powStr = thous[0];
		        }
		        else // find the scale of the number
		        {
		            int log = (int)System.Math.Log(number, 1000);
				pow = (int)System.Math.Pow(1000, log);
		            powStr = thous[log];
		        }

		        numString = string.Format("{0} {1}", ToWords(number / pow, powStr), ToWords(number % pow)).Trim();
		    }

		    return string.Format("{0} {1}", numString, appendScale).Trim();
		}


}
