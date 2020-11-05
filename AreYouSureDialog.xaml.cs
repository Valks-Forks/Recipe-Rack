using System.Windows;

namespace Recipe_Rack
{
    /// <summary>
    /// Interaction logic for AreYouSureDialog.xaml
    /// </summary>
    public partial class AreYouSureDialog : Window
    {
        public bool IsThisToDelete = true;
        public bool DoesUserWantToSave = false;
        public AreYouSureDialog()
        {
            InitializeComponent();
        }

        private void AYSD_Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AYSD_Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            // Delete the users current selected Recipe.
            if (IsThisToDelete == true)
            {
                string SelectedName = ((MainWindow)Application.Current.MainWindow).Card_RecipeName_Label.Content.ToString();                    
                SqliteDataAccess.DeleteRecipe(SelectedName);
                this.Close();
            }

            // Instead of deleting the recipe this is used to reuse this window for the tipsWindow to ask user to save before closing instead.
            if (IsThisToDelete == false)
            {
                DoesUserWantToSave = true;
                this.Close();
            }
        }
    }
}
