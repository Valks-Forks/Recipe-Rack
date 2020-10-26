using System.Collections.Generic;

namespace Recipe_Rack
{
	public class Recipe : RecipeCategory
	{
		public string RecipeName;
		public string BodyMsg;
		public int Difficulty;
		public List<string> IngredientList = new List<string>();
		public bool IsFavorite;

		public Recipe(string RecipeCategory, string _recipeName, string _bodyMsg, List<string> _IngredientList, int _Difficulty, bool _isFavorite = false)
		{
			categoryName = RecipeCategory;
			RecipeName = _recipeName;
			BodyMsg = _bodyMsg;
			IngredientList = _IngredientList;
			Difficulty = _Difficulty;
			IsFavorite = _isFavorite;
		}
	}
}
