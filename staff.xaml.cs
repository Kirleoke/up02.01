using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace projectUP02._01
{
    /// <summary>
    /// Логика взаимодействия для staff.xaml
    /// </summary>
    public partial class staff : Window
    {
        public staff()
        {
            InitializeComponent();
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            staff staff = new staff();
            MainWindow main = new MainWindow();
            String s = id.Text;
            main.id1.Text = s;
            String d = firstname.Text;
            main.firstname1.Text = d;
            String b = Name.Text;
            main.Name1.Text = b;
            String r = Department.Text;
            main.Department1.Text = r;
            main.Show();
        }
    }
}
