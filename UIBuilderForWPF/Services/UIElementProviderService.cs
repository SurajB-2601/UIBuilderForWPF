using UIBuilderForWPF.Interfaces;
using UIBuilderForWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace UIBuilderForWPF.Services
{
    public class UIElementProviderService : IUIElementProviderService
    {
        public UIElement GetElement(UIElementType elementType)
        {
            FrameworkElement createdElement = null;

            if (elementType == UIElementType.Button)
            {
                Button newButton = new Button();
                newButton.Content = "Button";
                createdElement = newButton;
            }
            else if (elementType == UIElementType.Checkbox)
            {
                CheckBox newCheckBox = new CheckBox();
                newCheckBox.Content = "CheckBox";
                newCheckBox.IsChecked = true;
                createdElement = newCheckBox;
            }
            else if (elementType == UIElementType.Label)
            {
                TextBlock newLabel = new TextBlock();
                newLabel.Text = "Text Label";
                createdElement = newLabel;
            }
            else if (elementType == UIElementType.EditText)
            {
                TextBox newTextBox = new TextBox();
                newTextBox.Text = "Edit Text";
                createdElement = newTextBox;
            }

            if (createdElement != null)
            {
                createdElement.Height = 20;
                createdElement.Width = 100;
            }

            return createdElement;

        }
    }
}
