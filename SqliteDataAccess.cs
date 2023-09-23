namespace RecipeRack;

class SqliteDataAccess
{
    /// <summary>
    /// Checks the SQLite DB and returns all entries.
    /// </summary>
    /// <returns>A List of Recipe objects.</returns>
    public static List<Recipe> LoadAllRecipes()
    {
        // Ensure that the database doesnt remain open after accessing it
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());

        List<Recipe> recipes = new List<Recipe>();

        SQLiteCommand CMD = new SQLiteCommand
        {
            Connection = (SQLiteConnection)cnn,
            CommandType = CommandType.Text,
            CommandText = "SELECT * FROM Recipe"
        };

        CMD.Connection.Open();
        SQLiteDataReader reader = CMD.ExecuteReader();

        while (reader.Read())
        {
            Recipe thisRecipe = new(
                reader.GetString(0), 
                reader.GetString(1), 
                reader.GetString(2), 
                reader.GetString(3), 
                reader.GetInt32(4), 
                reader.GetInt32(5));

            recipes.Add(thisRecipe);
        }

        reader.Close();
        CMD.Connection.Close();
        return recipes;
    }

    /// <summary>
    /// Store a recipe object into the DB.
    /// </summary>
    /// <param name="recipe"></param>
    public static void SaveRecipe(Recipe recipe)
    {
        // Ensure that the database doesnt remain open after accessing it
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        
        SQLiteCommand CMD = new SQLiteCommand
        {
            Connection = (SQLiteConnection)cnn,
            CommandType = CommandType.Text
        };

        CMD.Parameters.AddWithValue("@categoryName", recipe.categoryName);
        CMD.Parameters.AddWithValue("@RecipeName", recipe.RecipeName);
        CMD.Parameters.AddWithValue("@DirectionsMsg", recipe.DirectionsMsg);
        CMD.Parameters.AddWithValue("@TempIngredientList", recipe.IngredientsToString());
        CMD.Parameters.AddWithValue("@Difficulty", recipe.Difficulty);
        CMD.Parameters.AddWithValue("@IsFavorite_Int", recipe.IsFavoriteToInt());
        CMD.CommandText = @"INSERT INTO Recipe (categoryName, RecipeName, DirectionsMsg, TempIngredientList, Difficulty, IsFavorite_Int) values (@categoryName, @RecipeName, @DirectionsMsg, @TempIngredientList, @Difficulty, @IsFavorite_Int)";
        CMD.Connection.Open();
        CMD.ExecuteNonQuery();
        CMD.Connection.Close();
    }

    /// <summary>
    /// Delete a recipe from the DB.
    /// </summary>
    public static void DeleteRecipe(string recipeName)
    {
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        
        SQLiteCommand CMD = new SQLiteCommand
        {
            Connection = (SQLiteConnection)cnn,
            CommandType = CommandType.Text,
            CommandText = String.Format("DELETE FROM Recipe WHERE RecipeName='{0}'", recipeName)
        };

        CMD.Connection.Open();
        CMD.ExecuteNonQuery();
        CMD.Connection.Close();
    }

    private static string LoadConnectionString(string id = "Default")
    {
        return ConfigurationManager.ConnectionStrings[id].ConnectionString;
    }
}
