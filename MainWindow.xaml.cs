using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Recipe_Rack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// This method takes the button names from the Recipe Category Grid and turns them into RecipeCategory objects in a list for later reference when user adds recipes.
        /// </summary>
        public static List<RecipeCategory> InitializeRecipeCategories(Grid grid)
        {
            List<RecipeCategory> recipeCategoryList = new List<RecipeCategory>();
            foreach (Button item in grid.Children)
            {
                recipeCategoryList.Add(new RecipeCategory(item.Content.ToString()));
            }
            return recipeCategoryList;
        }
        
        public MainWindow()
        {
            //Check if user already has the directories and files for the Recipe Rack. If they do then dont create any.
            if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Recipe Rack\\"))
            {  
                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Recipe Rack\\");
            }
            if (!System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Recipe Rack\\TipsDocument.json"))
            {
                System.IO.File.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Recipe Rack\\TipsDocument.json");
            }
            InitializeComponent();
            Show_UIControls(false);
        }

        /// <summary>
        /// This method takes a long string and adds "\n" breaks every 16 characters. This helps outputting info to a ListView.
        /// </summary>
        private static string MakeTextReadableForList(string StringToConvert)
        {
            if (StringToConvert.Length > 16)
            {
                int iteration = 0;
                for (int characterindex = 0; characterindex < StringToConvert.Length; characterindex++)
                {
                    if (StringToConvert.ToArray()[characterindex] == ' ' && iteration > 16)
                    {
                        StringToConvert = StringToConvert.Remove(characterindex, 1);
                        StringToConvert = StringToConvert.Insert(characterindex, "\n");
                        iteration = 0;
                    }
                    iteration++;
                }
                return StringToConvert;
            }
            else
            {
                return StringToConvert;
            }
        }

        private void add_New_Recipe_Click(object sender, RoutedEventArgs e)
        {
            // Give the user another window for adding a new recipe
            EnterNewRecipeMenu enterNewRecipeMenu = new EnterNewRecipeMenu();
            enterNewRecipeMenu.Owner = this;
            enterNewRecipeMenu.WindowStartupLocation = WindowStartupLocation.Manual;
            enterNewRecipeMenu.Top = this.Top + 150;
            enterNewRecipeMenu.Left = this.Left + 300;
            enterNewRecipeMenu.ShowDialog();
        }

        private void Button_Click_Populate_Recipe_Name_List(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var recipes = SqliteDataAccess.LoadAllRecipes();
            SelectRecipes_ListBox.Items.Clear(); 

            // This section of code just clears the recipe Card Viewer section of old previous data
            Card_IsFavoriteStar_Image.Visibility = Visibility.Hidden;
            Card_RecipeBody_RichTextBox.Document.Blocks.Clear();
            Card_RecipeCategory_Label.Content = "";
            Card_RecipeDifficulty_Label.Content = "";
            Card_RecipeIngredients_ListBox.Items.Clear();
            Card_RecipeName_Label.Content = "";
            Card_RecipeBody_RichTextBox.IsEnabled = false;
            Card_RecipeCategory_Label.IsEnabled = false;
            Card_RecipeDifficulty_Label.IsEnabled = false;
            Card_RecipeIngredients_ListBox.IsEnabled = false;
            Card_RecipeName_Label.IsEnabled = false;

            // This loops through all the recipe objects generated from the files. This then narrows the recipes down to only the ones that match the 
            // recipe Category button the user pressed and displays all of them to the SelectRecipes listbox.
            // The two seperate loops are there to prioritize Favorite recipes to put at the top of the list
            foreach (var item in recipes)
            {
                if (item.categoryName == button.Content.ToString() && item.IsFavorite == true)
                {
                    SelectRecipes_ListBox.Items.Add(MakeTextReadableForList(item.RecipeName));
                }
            }
            foreach (var item in recipes)
            {
                if (item.categoryName == button.Content.ToString() && item.IsFavorite == false)
                {
                    SelectRecipes_ListBox.Items.Add(MakeTextReadableForList(item.RecipeName));
                }
            }

            // Hide the entire ui, then only show user the next thing they should select *Quality of Life*
            if (SelectRecipes_ListBox.Items.Count >= 1)
            {
                Show_UIControls(false);
                SelectRecipeName_Groupbox.Visibility = Visibility.Visible;
                SelectRecipeName_Groupbox.IsEnabled = true;           
            }
            else
            {
                Show_UIControls(false);
            }
        }

        private void SelectRecipes_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This section auto clears the card view section of all previous data once the user clicks a different recipe 
            var recipes = SqliteDataAccess.LoadAllRecipes();
            Card_IsFavoriteStar_Image.Visibility = Visibility.Hidden;
            Card_RecipeBody_RichTextBox.Document.Blocks.Clear();
            Card_RecipeBody_RichTextBox.IsEnabled = true;
            Card_RecipeCategory_Label.Content = "";
            Card_RecipeCategory_Label.IsEnabled = true;
            Card_RecipeDifficulty_Label.Content = "";
            Card_RecipeDifficulty_Label.IsEnabled = true;
            Card_RecipeIngredients_ListBox.Items.Clear();
            Card_RecipeIngredients_ListBox.IsEnabled = true;
            Card_RecipeName_Label.Content = "";
            Card_RecipeName_Label.IsEnabled = true;

            if (SelectRecipes_ListBox.SelectedIndex >= 0)
            { 
                Show_UIControls(true);
            }
            
            // This section tells the card viewer what item it should show. *This area is prone to an Out of Index exception if i loops too high* 
            // This can be caused by the listbox not being able to match the correct name with the ones generated in the Recipe list
            int i = 0;
            foreach (var item in recipes)
            {
                if (!(SelectRecipes_ListBox.SelectedItem == null))
                {
                    if (SelectRecipes_ListBox.SelectedItem.ToString() == MakeTextReadableForList(item.RecipeName))
                    {
                        break;
                    }
                    i++;
                }
            }

            // If i looped too high throw an exception here so it is easier to track
            if (i > recipes.Count)
            {
                throw new IndexOutOfRangeException("ERROR: Could not match a name from the SelectRecipes_ListBox" +
                    " to a recipe name in database.");
            }
            
            // This section is responsible for displaying the selected recipe to the user in the Card Viewer section
            if (SelectRecipes_ListBox.Items.Count > 0)
            {
                Recipe thisRecipe = recipes[i];
                Card_RecipeBody_RichTextBox.AppendText(thisRecipe.DirectionsMsg);
                Card_RecipeCategory_Label.Content = thisRecipe.categoryName;
                Card_RecipeDifficulty_Label.Content = thisRecipe.Difficulty.ToString();
                Card_RecipeName_Label.Content = thisRecipe.RecipeName;

                //If this recipe is a favorite recipe show the user a start icon
                if (thisRecipe.IsFavorite)
                {
                    Card_IsFavoriteStar_Image.Visibility = Visibility.Visible;
                }
                
                for (int item = 0; item < thisRecipe.IngredientList.Count; item++)
                {
                    Card_RecipeIngredients_ListBox.Items.Add(MakeTextReadableForList(thisRecipe.IngredientList[item]));
                    Card_RecipeIngredients_ListBox.Items.Add("");
                }
            }
        }

        private void EditRecipe_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(SelectRecipes_ListBox.SelectedItem == null))
            {
                // This makes a copy of the NewRecipeMenu and uses it to edit a recipe instead, allowing the user to change a current selected recipe it 
                // uses info from the card viewer
                EnterNewRecipeMenu enterEditRecipeMenu = new EnterNewRecipeMenu();
                enterEditRecipeMenu.Owner = this;
                enterEditRecipeMenu.WindowStartupLocation = WindowStartupLocation.Manual;
                enterEditRecipeMenu.Top = this.Top + 150;
                enterEditRecipeMenu.Left = this.Left + 300;
                enterEditRecipeMenu.IsThisAnEdit = true;
                enterEditRecipeMenu.OldRecipeName = Card_RecipeName_Label.Content.ToString().Trim();
                enterEditRecipeMenu.Directions_RichTextBox.Document.Blocks.Clear();
                enterEditRecipeMenu.Directions_RichTextBox.AppendText((new TextRange(Card_RecipeBody_RichTextBox.Document.ContentStart, Card_RecipeBody_RichTextBox.Document.ContentEnd).Text));
                enterEditRecipeMenu.RecipeName_TextBox.Text = Card_RecipeName_Label.Content.ToString();
                enterEditRecipeMenu.RecipeDifficulty_ComboBox.SelectedItem = Card_RecipeDifficulty_Label.Content;
                enterEditRecipeMenu.RecipeCategory_ComboBox.SelectedItem = Card_RecipeCategory_Label.Content;

                // Check if the Recipe is already a Favorite. If it is then preCheck the checkbox in the edit menu for the user.
                if (Card_IsFavoriteStar_Image.Visibility == Visibility.Visible)
                {
                    enterEditRecipeMenu.IsFavorite_Checkbox.IsChecked = true;
                }

                // add items from list to the new edit box
                foreach (var item in Card_RecipeIngredients_ListBox.Items)
                {
                    // This checks for empty whitespaces and deletes them so there are not any blank ingredient items in the list.
                    if (item.ToString().Length < 4)
                    {
                        enterEditRecipeMenu.EnterNewRecipe_IngredientListView.Items.Remove(item);
                    }
                    enterEditRecipeMenu.EnterNewRecipe_IngredientListView.Items.Add(item);
                }

                // remove the trailing whitespace
                enterEditRecipeMenu.EnterNewRecipe_IngredientListView.Items.RemoveAt(enterEditRecipeMenu.EnterNewRecipe_IngredientListView.Items.Count - 1);

                // This is here so when the dialog box is closed the SelectRecipeNames list doesnt loop out of index *Bug avoidance*
                // Also force the edit and delete recipe buttons to disable.
                SelectRecipes_ListBox.Items.Clear();
                DeleteRecipe_Button.IsEnabled = false;
                EditRecipe_Button.IsEnabled = false;
                enterEditRecipeMenu.ShowDialog();
                Show_UIControls(false);
            }
        }
           
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            // Give the user a dialog box to confirm they want to delete a recipe.
            if (!(SelectRecipes_ListBox.SelectedItem == null))
            {
                AreYouSureDialog areYouSureDialog = new AreYouSureDialog();
                areYouSureDialog.Owner = this;
                areYouSureDialog.WindowStartupLocation = WindowStartupLocation.Manual;
                areYouSureDialog.Top = this.Top + 150;
                areYouSureDialog.Left = this.Left + 300;
                areYouSureDialog.ShowDialog();
                SelectRecipes_ListBox.Items.Clear();
                Show_UIControls(false);
            }
        }

        private void ShowTips_Button_Click(object sender, RoutedEventArgs e)
        {
            // This generates the TipsMenu Window for a user to edit tips.
            ShowTipsMenu showTipsMenu = new ShowTipsMenu();
            showTipsMenu.Owner = this;
            showTipsMenu.WindowStartupLocation = WindowStartupLocation.Manual;
            showTipsMenu.Top = this.Top + 50;
            showTipsMenu.Left = this.Left + 100;
            showTipsMenu.ShowDialog();
        }
        /// <summary>
        /// This method shows/hides and enables/disables the UI elements for the user. If True UI is shown to user. If False UI is hidden from user.
        /// </summary>
        private void Show_UIControls(bool show)
        {
            if (show == true)
            {
                SelectRecipeName_Groupbox.IsEnabled = true;
                RecipeCardCategory_Groupbox.IsEnabled = true;
                RecipeCardDifficulty_Groupbox.IsEnabled = true;
                RecipeCardDirections_Groupbox.IsEnabled = true;
                RecipeCardIngredientsList_Groupbox.IsEnabled = true;
                RecipeCardName_Groupbox.IsEnabled = true;
                DeleteRecipe_Button.IsEnabled = true;
                EditRecipe_Button.IsEnabled = true;

                // Show the UI for the User
                SelectRecipeName_Groupbox.Visibility = Visibility.Visible;
                RecipeCardCategory_Groupbox.Visibility = Visibility.Visible;
                RecipeCardDifficulty_Groupbox.Visibility = Visibility.Visible;
                RecipeCardDirections_Groupbox.Visibility = Visibility.Visible;
                RecipeCardIngredientsList_Groupbox.Visibility = Visibility.Visible;
                RecipeCardName_Groupbox.Visibility = Visibility.Visible;
                RecipeCardOutline_Groupbox1.Visibility = Visibility.Visible;
                RecipeCardOutline_Groupbox2.Visibility = Visibility.Visible;
                DeleteRecipe_Button.Visibility = Visibility.Visible;
                EditRecipe_Button.Visibility = Visibility.Visible;
                RecipeCard_Label.Visibility = Visibility.Visible;

                //Check if the user has any recipes already if they do then dont show the GetStarted label if they dont display it
                var recipes = SqliteDataAccess.LoadAllRecipes();
                if (recipes.Count > 0)
                {
                    GetStarted_Label.Visibility = Visibility.Hidden;
                }
                else
                {
                    GetStarted_Label.Visibility = Visibility.Visible;
                } 
            }
            else
            {
                SelectRecipeName_Groupbox.IsEnabled = false;
                RecipeCardCategory_Groupbox.IsEnabled = false;
                RecipeCardDifficulty_Groupbox.IsEnabled = false;
                RecipeCardDirections_Groupbox.IsEnabled = false;
                RecipeCardIngredientsList_Groupbox.IsEnabled = false;
                RecipeCardName_Groupbox.IsEnabled = false;
                DeleteRecipe_Button.IsEnabled = false;
                EditRecipe_Button.IsEnabled = false;

                //Hide the UI for the User
                SelectRecipeName_Groupbox.Visibility = Visibility.Hidden;
                RecipeCardCategory_Groupbox.Visibility = Visibility.Hidden;
                RecipeCardDifficulty_Groupbox.Visibility = Visibility.Hidden;
                RecipeCardDirections_Groupbox.Visibility = Visibility.Hidden;
                RecipeCardIngredientsList_Groupbox.Visibility = Visibility.Hidden;
                RecipeCardName_Groupbox.Visibility = Visibility.Hidden;
                RecipeCardOutline_Groupbox1.Visibility = Visibility.Hidden;
                RecipeCardOutline_Groupbox2.Visibility = Visibility.Hidden;
                DeleteRecipe_Button.Visibility = Visibility.Hidden;
                EditRecipe_Button.Visibility = Visibility.Hidden;
                RecipeCard_Label.Visibility = Visibility.Hidden;

                //Check if the user has any recipes already if they do then dont show the GetStarted label if they dont display it
                var recipes = SqliteDataAccess.LoadAllRecipes();
                if (recipes.Count == 0)
                {
                    GetStarted_Label.Visibility = Visibility.Visible;
                }
                else
                {
                    GetStarted_Label.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
