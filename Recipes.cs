namespace RecipeRack;

public class Recipe : RecipeCategory
{
    public string RecipeName;
    public string DirectionsMsg;
    public int Difficulty;
    public bool IsFavorite;
    public List<string> IngredientList = new List<string>();

    // These are only used to convert values in order to be accepted by SQLite syntax.
    public string TempIngredientList;
    public int IsFavorite_AsInt;

    public Recipe(string _categoryName, string _RecipeName, string _DirectionsMsg, string _TempIngredientList, int _Difficulty, int _IsFavorite_Int)
    {
        categoryName = _categoryName;
        RecipeName = _RecipeName;
        DirectionsMsg = _DirectionsMsg;
        Difficulty = _Difficulty;

        // Convert values from the DB to the default Recipe Class
        string[] ingredientArray = _TempIngredientList.Split('\n');
        foreach (global::System.String ingredient in ingredientArray)
        {
            if (ingredient.Length > 0)
            {
                IngredientList.Add(ingredient);
            }
        }

        if (_IsFavorite_Int == 1)
        {
            IsFavorite = true;
        }
        else if (_IsFavorite_Int == 0)
        {
            IsFavorite = false;
        }
    }

    /// <summary>
    /// Check the bool IsFavorite then turn it into an Int value instead. This is used because SQLite doesnt support Boolean values.
    /// </summary>
    /// <returns>If the recipe is a Favorite return a 1, if not return 0.</returns>
    public int IsFavoriteToInt()
    {
        return IsFavorite == true ? 1 : 0;
    }

    /// <summary>
    /// Convert the recipes List into a single string for the DB.
    /// </summary>
    /// <returns></returns>
    public string IngredientsToString()
    {
        string outputstring = "";

        foreach (var item in IngredientList)
        {
            outputstring += item.ToString() + "\n";
        }

        return outputstring;
    }
}
