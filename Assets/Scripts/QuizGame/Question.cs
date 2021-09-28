using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Question
{
	public Sprite picture;

	[TextArea(3, 10)]
	public string question;

    public bool isToF = true;
    public bool ToFa = true;

	[TextArea(3, 10)]
	public string option0;

    [TextArea(3, 10)]
	public string option1;

    public int answer = 0;
}
