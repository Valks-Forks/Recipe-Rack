using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Recipe_Rack
{
    /// <summary>
    /// Interaction logic for ShowTipsMenu.xaml
    /// </summary>
    public partial class ShowTipsMenu : Window
    {
        public bool HasTxtChanged = false;
        public ShowTipsMenu()
        {
            InitializeComponent();
            HasTxtChanged = false;


        }

        private void EnableSpellCheck_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Tips_Rich_TextBox.SpellCheck.IsEnabled = true;

        }

        private void EnableSpellCheck_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Tips_Rich_TextBox.SpellCheck.IsEnabled = false;
        }

        private void EditMode_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Tips_Rich_TextBox.IsEnabled = false;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Tips_Rich_TextBox.IsEnabled = true;
        }

        private void FontSize_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSize_ComboBox.SelectedItem != null && FontSize_ComboBox.Text != "")
            {
                Tips_Rich_TextBox.FontSize = double.Parse(FontSize_ComboBox.Text);
            }
            
            
        }

        

       

        private void FontSize_ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (FontSize_ComboBox.SelectedItem != null && FontSize_ComboBox.Text != "")
            {
                Tips_Rich_TextBox.FontSize = double.Parse(FontSize_ComboBox.Text);
            }
        }

        private void Undo_Button_Click(object sender, RoutedEventArgs e)
        {
            Tips_Rich_TextBox.Undo();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Recipe_JsonHandler.WriteToJsonFile_Tips(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Recipe Rack\\Tips\\TipsDocument.json", new TextRange(Tips_Rich_TextBox.Document.ContentStart, Tips_Rich_TextBox.Document.ContentEnd).Text.ToString());
            HasTxtChanged = false;

        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Tips_Rich_TextBox.AppendText(Recipe_JsonHandler.ReadFromJsonFile_Tips(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Recipe Rack\\Tips\\TipsDocument.json"));
        }

        private void Save_Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Recipe_JsonHandler.WriteToJsonFile_Tips(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Recipe Rack\\Tips\\TipsDocument.json", new TextRange(Tips_Rich_TextBox.Document.ContentStart, Tips_Rich_TextBox.Document.ContentEnd).Text.ToString());
            HasTxtChanged = false;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // If user has unsaved data ask them to confirm if they want to save first before closing.
            if (HasTxtChanged == true)
            {
                //Convert the Dialog so it is reusable for this instance
                AreYouSureDialog areYouSureDialog = new AreYouSureDialog();
                areYouSureDialog.Main_Label.Text = "Would you like to save before closing?";
                areYouSureDialog.Warning_Label.Text = "";
                areYouSureDialog.AYSD_Cancel_Button.Content = "Dont Save";
                areYouSureDialog.AYSD_Delete_Button.Content = "Save";
                areYouSureDialog.IsThisToDelete = false;

                //Position window correctly
                
                areYouSureDialog.Owner = this;
                areYouSureDialog.WindowStartupLocation = WindowStartupLocation.Manual;
                areYouSureDialog.Top = Owner.Top + 300;
                areYouSureDialog.Left = Owner.Left + 450;

                areYouSureDialog.ShowDialog();

                if (areYouSureDialog.DoesUserWantToSave == true)
                {
                    Recipe_JsonHandler.WriteToJsonFile_Tips(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Recipe Rack\\Tips\\TipsDocument.json", new TextRange(Tips_Rich_TextBox.Document.ContentStart, Tips_Rich_TextBox.Document.ContentEnd).Text.ToString());
                    HasTxtChanged = false;
                    
                }
                
            }
            
        }

        private void Tips_Rich_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            HasTxtChanged = true;
        }

       
        
    }
}
