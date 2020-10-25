using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using Newtonsoft.Json;

public class Recipes : RecipeCategory
{

	public string RecipeName;

	public string BodyMsg;

	public int Difficulty;

	public List<string> IngredientList = new List<string>();

	public bool IsFavorite;



	public Recipes(string RecipeCategory, string _recipeName, string _bodyMsg, List<string> _IngredientList, int _Difficulty, bool _isFavorite = false)
	{
		categoryName = RecipeCategory;
		RecipeName = _recipeName;
		BodyMsg = _bodyMsg;
		IngredientList = _IngredientList;
		Difficulty = _Difficulty;
		IsFavorite = _isFavorite;
	}

	

	

	
}
