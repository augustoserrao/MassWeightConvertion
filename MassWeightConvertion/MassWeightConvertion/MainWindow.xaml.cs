/* Augusto Serrao
   Deepti Aggarwal
   Hartaj Dhillon
   Gagandeep Singh
   Shoaib Hassan
*/

using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MassWeightConvertion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MassWeightConvertionVM massWeightVM = new MassWeightConvertionVM();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = massWeightVM;
        }

        private void Button_Convert_Click(object sender, RoutedEventArgs e)
        {
            massWeightVM.ConvertMassIntoWeight();
        }

        private void Button_Learn_More_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=rFdbY_V7vIo");
        }

        private void MassValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numbers to be typed
            e.Handled = isNumber(e.Text);
        }

        private static bool isNumber(string text)
        {
            Regex regex = new Regex("[^0-9.]+");
            return regex.IsMatch(text);
        }
    }
}
