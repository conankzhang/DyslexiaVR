using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gazeTrigger : MonoBehaviour {

	// Use this for initialization
	public TextMesh _activeMesh;
	public RaycastHit hit;
	int[] easy = {0, 2, 1, 5, 4, 3, 6, 7, 9, 8, 10, 12, 11, 13, 14, 15, 17, 16, 18, 19, 22, 20, 21, 23, 24, 25};
	int[] medium = { 0, 2, 3, 1, 5, 4, 6, 8, 10, 7, 9, 12, 11, 13, 14, 17, 15, 16, 18, 19, 21, 20, 23, 24, 22, 25 };
	int[] hard = { 0, 2, 3, 5, 1, 4, 6, 8, 10, 7, 9, 12, 11, 14, 13, 17, 15, 16, 19, 18, 22, 20, 23, 24, 21, 25 };
	char[] similar1 = { 'B', 'D', 'P' };
	char[] similar2 = { 'b', 'd', 'p', 'q' };
	char[] similar3 = { 'Q', 'O', 'C', 'U'};
	char[] similar4 = { 'c', 'o', 'u'};
	char[] similar5 = { 'E', 'F'};
	char[] similar6 = { 'T', 'I', 'L' };
	char[] similar7 = { 'V', 'A'};

	int _tempIndex = 0;

	GameObject _whiteboard;
	TextMesh[] _textMesh = new TextMesh[4];
	string[] original = new string[4];
	string copy;
	string word ="";
	float x, y, z;
	float jumbleTime = 0.5f;
	float flipTime = 2f;
	string difficulty;
	bool[] makeRandom = {true, true, true, true};
	string[] tag = { "board0", "board1", "board2", "board3" };
	int _activeIndex;

	void Start () {
		difficulty = "easy";
		_whiteboard = GameObject.FindWithTag ("board");
		_textMesh = _whiteboard.GetComponentsInChildren<TextMesh>(true);
		for (int i = 0; i < _textMesh.Length; i++) {
			original [i] = _textMesh [i].text;
		}

	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		Time deltaTime;
		string s = "";
		int i = 0;
		Vector3 fwd = transform.TransformDirection (Vector3.forward)*10;
		Debug.DrawRay(transform.position, fwd, Color.green);
		if (Time.time > jumbleTime) {
			foreach (TextMesh _currentMesh in _textMesh) {
				_activeIndex = i;
				i++;
				string tag = _currentMesh.tag;
				//print (tag);
				if (makeRandom [_activeIndex]) {
					StartRandom (_currentMesh);
				}
				if (hit.collider && hit.collider.tag == _currentMesh.tag) {
					print ("Still Hit");
				} else {
					makeRandom[_activeIndex] = true;
				}

				if (Physics.Raycast (transform.position, fwd, out hit)) {
					if (hit.collider && hit.collider.tag == _currentMesh.tag) {
						StartCoroutine(CallCollisionCheck(_activeIndex));
					}
				}
			}
			jumbleTime += 1f;
			i = 0;
		}
	}

	IEnumerator CallCollisionCheck(int _tempIndex)	{
		yield return new WaitForSeconds(3f);
		CheckCollision(_tempIndex);
	}

	void CheckCollision(int i_index) {
		print (i_index);
		Vector3 fwd2 = transform.TransformDirection (Vector3.forward);
			if (Physics.Raycast (transform.position, fwd2, out hit)) {
			if (hit.collider && hit.collider.tag == "board"+i_index) {
				makeRandom[i_index] = false;
					_textMesh[i_index].text = original[i_index];
					print ("After dealy");
				}
			}
	
	}
	void StartRandom(TextMesh i_mesh)	{
		TextMesh o_text = i_mesh;
		//print (o_text.text);
		string word = "";
		string randomText = "";
		string s = o_text.text;
		s += " ";

			//Debug.Log (s);
			for (int i = 0; i < s.Length; i++) { 
				if (s [i] == ' ') {
					if (randomText.Length != 0)
						randomText += ' ';
					randomText += RandomText (word, difficulty);
					word = "";
				} else {
					word += s [i];
				}
			}
			randomText = SimilarWords (randomText);
			i_mesh.text = randomText;		
	}

	string RandomText(string word, string difficulty)	{

		string random = "";

		switch (difficulty)
		{
		case "easy":
			for (int i = 0; i < word.Length; i++)
			{
				int temp = easy[i];
				while (temp > word.Length - 1) {
					temp = easy [i + 1];
					i++;
				}
				random += word[temp]; 
			}
			break;

		case "medium":
			for (int i = 0; i < word.Length; i++)
			{
				int temp = medium[i];
				while (temp > word.Length)
					temp--;
				random += word[temp];
			}
			break;

		case "hard":
			for (int i = 0; i < word.Length; i++)
			{
				int temp = hard[i];
				while (temp > word.Length)
					temp--;
				random += word[temp];
			}
			break;
		}
		return random;
	}

	Text Mirror(Text s)
	{
		//Debug.Log(s.rectTransform.localScale.x);
		s.rectTransform.localScale = new Vector3(s.rectTransform.localScale.x * -1, s.rectTransform.localScale.y, s.rectTransform.localScale.z);
		return s;
	}

	string SimilarWords(string word) {
		int randomNumber = 0;

		for (int i=0; i<word.Length; i++)
		{
			if (System.Array.IndexOf(similar1, word[i]) != -1)
			{
				randomNumber = Random.Range(0, similar1.Length - 1);
				word = word.Replace(word[i], similar1[randomNumber]);
			}
			else if (System.Array.IndexOf(similar2, word[i]) != -1)
			{
				randomNumber = Random.Range(0, similar2.Length - 1);
				word = word.Replace(word[i], similar2[randomNumber]);
			}
			else if (System.Array.IndexOf(similar3, word[i]) != -1)
			{
				randomNumber = Random.Range(0, similar3.Length - 1);
				word = word.Replace(word[i], similar3[randomNumber]);
			}
			else if (System.Array.IndexOf(similar4, word[i]) != -1)
			{
				randomNumber = Random.Range(0, similar4.Length - 1);
				word = word.Replace(word[i], similar4[randomNumber]);
			}
			else if (System.Array.IndexOf(similar5, word[i]) != -1)
			{
				randomNumber = Random.Range(0, similar5.Length - 1);
				word = word.Replace(word[i], similar5[randomNumber]);
			}
			else if (System.Array.IndexOf(similar6, word[i]) != -1)
			{
				randomNumber = Random.Range(0, similar6.Length - 1);
				word = word.Replace(word[i], similar6[randomNumber]);
			}
			else if (System.Array.IndexOf(similar7, word[i]) != -1)
			{
				randomNumber = Random.Range(0, similar7.Length - 1);
				word = word.Replace(word[i], similar7[randomNumber]);
			}
		}
		return word;
	}

}


