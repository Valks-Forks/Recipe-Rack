namespace RecipeRack;

/// <summary>
/// Interaction logic for EnterNewRecipeMenu.xaml
/// </summary>
public partial class EnterNewRecipeMenu : Window
{
    // Check if User is Editing or doing a new recipe
    public bool IsThisAnEdit = false;
    public string OldRecipeName;

    public EnterNewRecipeMenu()
    {
        InitializeComponent();
    }

    //This adds an item to the ingredient list
    private void IngredList_TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        // Add the Ingredient to the list if enter is pressed 
        if (e.Key == Key.Enter && IngredList_TextBox.Text.Length > 3)
        {
            string UsrInput = IngredList_TextBox.Text;
            EnterNewRecipe_IngredientListView.Items.Add(UsrInput);
            IngredList_TextBox.Text = null;
        }
    }

    // This removes an item from the ingredients list
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (EnterNewRecipe_IngredientListView.Items.Count == 1)
        {
            EnterNewRecipe_IngredientListView.Items.Clear();
        }
        else
        {
            EnterNewRecipe_IngredientListView.Items.Remove(EnterNewRecipe_IngredientListView.SelectedItem);
        }

        CheckIfButtonCanBePressed();
    }

    // These turn on and off Spellcheck for the directions text box
    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        Directions_RichTextBox.SpellCheck.IsEnabled = true;
    }

    private void Spell_CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        Directions_RichTextBox.SpellCheck.IsEnabled = false;
    }

    //Various checks on events to make sure data is entered correctly
    private void CheckForButtonUpdate(object sender, TextChangedEventArgs e)
    {
        CheckIfButtonCanBePressed();
    }

    //Various checks on events to make sure data is entered correctly
    private void RecipeDifficulty_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        CheckIfButtonCanBePressed();
    }

    /// <summary>
    ///  Enable or disable the Finalize button based off of what the user entered in the Menu. *Data Integrity Protection*
    /// </summary>
    private void CheckIfButtonCanBePressed()
    {
        int AccumulatedValue = 0;
        if (RecipeCategory_ComboBox == null)
        { }
        else if (RecipeCategory_ComboBox.SelectedIndex >= 0)
        { AccumulatedValue++; }

        if (RecipeName_TextBox.Text.Length >= 3)
        { AccumulatedValue++; }

        if (RecipeDifficulty_ComboBox == null)
        { }
        else if (RecipeDifficulty_ComboBox.SelectedIndex >= 0)
        { AccumulatedValue++; }

        if (EnterNewRecipe_IngredientListView.Items.Count > 1)
        { AccumulatedValue++; }

        if (AccumulatedValue == 4)
        {
            FinalizeRecipe_Button.IsEnabled = true;
        }
        else if (FinalizeRecipe_Button != null)
        {
            FinalizeRecipe_Button.IsEnabled = false;
        }
    }

    private List<string> ParseListInfo(ListView _ListView)
    {
        List<string> StringList = new List<string>();

        foreach (var item in _ListView.Items)
        {
            _ListView.SelectedItem = item;
            StringList.Add((string)_ListView.SelectedItem);
        }

        return StringList;
    }

    //Check if data looks okay one last time and Finalize the Output
    private void FinalizeRecipe_Button_Click(object sender, RoutedEventArgs e)
    {
        CheckIfButtonCanBePressed();

        // Check if user is attempting to save a recipe with a similiar name and prevent it
        List<Recipe> ListofRecipes = SqliteDataAccess.LoadAllRecipes();
        foreach (var item in ListofRecipes)
        {
            if (item.RecipeName == RecipeName_TextBox.Text && IsThisAnEdit == false)
            {
                FinalizeRecipe_Button.IsEnabled = false;
                InformUserOfName informUserOfName = new()
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    Top = this.Top + 120,
                    Left = this.Left + 30
                };
                System.Media.SystemSounds.Hand.Play();
                informUserOfName.ShowDialog();
                RecipeName_TextBox.Text = "";
            }
        }

        // Check if button is still enabled, If it is write the Recipe to the SQLite DB.
        if (FinalizeRecipe_Button.IsEnabled == true)
        {
            if (IsThisAnEdit)
            {
                SqliteDataAccess.DeleteRecipe(OldRecipeName);
            }

            Recipe recipe = new(RecipeCategory_ComboBox.SelectedItem.ToString(), RecipeName_TextBox.Text,  new TextRange(Directions_RichTextBox.Document.ContentStart, Directions_RichTextBox.Document.ContentEnd).Text.ToString(), ConvertListToString(ParseListInfo(EnterNewRecipe_IngredientListView)), Int16.Parse(RecipeDifficulty_ComboBox.Text.ToString()), ConvertBoolToInt(IsFavorite_Checkbox.IsChecked.Value));
            this.Close();
            SqliteDataAccess.SaveRecipe(recipe);
        }
    }

    private void RecipeName_TextBox_KeyUp(object sender, KeyEventArgs e)
    {
        // Limit the user from entering invalid characters into the Recipe Name field
        // This is a list of all illegal characters
        char[] InvalidCharacter = new char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*', '\'', '.', ',', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '=', '+', '`', '~', ';', '[', ']', '{', '}' };

        // Iterate through the text box to check for invalid characters
        foreach (var item in RecipeName_TextBox.Text)
        {
            for (int i = 0; i < InvalidCharacter.Length; i++)
            {
                if (item == InvalidCharacter[i])
                {
                    // Play windows error sound if invalid key is pressed
                    System.Media.SystemSounds.Hand.Play();
                    RecipeName_TextBox.Clear();
                }
            }
        }
    }

    private void RecipeCategory_ComboBox_Initialized(object sender, EventArgs e)
    {
        // Grab the list of Category names from the main App and populate this combobox menu with those choices
        List<RecipeCategory> RecipeCategoriesList = MainWindow.InitializeRecipeCategories(((MainWindow)Application.Current.MainWindow).RecipeCategory_Grid);
        foreach (var item in RecipeCategoriesList)
        {
            RecipeCategory_ComboBox.Items.Add(item.categoryName);
        }
    }

    private void EnterNewRecipe_IngredientListView_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        CheckIfButtonCanBePressed();
    }

    private void Directions_RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        CheckIfButtonCanBePressed();
    }

    /// <summary>
    /// Returns Int value in place of a bool.
    /// </summary>
    /// <param name="boolToConvert"></param>
    /// <returns>Returns 1 if true or 0 if false.</returns>
    private static int ConvertBoolToInt(bool boolToConvert)
    {
        return boolToConvert ? 1 : 0;
    }
    /// <summary>
    /// Returns a single String in place of a List(Strings)
    /// </summary>
    /// <param name="ListToConvert"></param>
    /// <returns></returns>
    private static string ConvertListToString(List<string> ListToConvert)
    {
        string stringToReturn = "";
        foreach (var item in ListToConvert)
        {
            stringToReturn += item + '\n';
        }

        return stringToReturn;
    }
}
