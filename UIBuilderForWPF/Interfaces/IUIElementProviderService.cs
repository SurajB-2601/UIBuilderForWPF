using UIBuilderForWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace UIBuilderForWPF.Interfaces
{
    public interface IUIElementProviderService
    {
        UIElement GetElement(UIElementType elementType);
    }
}
