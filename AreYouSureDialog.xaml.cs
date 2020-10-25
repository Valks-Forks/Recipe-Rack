using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;


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
            if (IsThisToDelete == true)
            {
                string SelectedName = ((MainWindow)Application.Current.MainWindow).Card_RecipeName_Label.Content.ToString();

                if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Recipe Rack\\Recipes\\" + SelectedName + ".json"))
                {
                    System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Recipe Rack\\Recipes\\" + SelectedName + ".json");
                    this.Close();
                }
                else
                {
                    throw new FileNotFoundException("ERROR: Cannot find file name. Was the name changed or already deleted?");
                }
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
