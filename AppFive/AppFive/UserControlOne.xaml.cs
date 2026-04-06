using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppFive
{
    /// <summary>
    /// Логика взаимодействия для UserControlOne.xaml
    /// </summary>
    public partial class UserControlOne : UserControl
    {
        public UserControlOne()
        {
            InitializeComponent();
            this.DataContextChanged += UserControlOne_DataContextChanged;
            this.DataContextChanged += PostgreUserControlOne_DataContextChanged;
        }
        private void UserControlOne_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is InfoGrants grant)
            {
                SetData(grant);
            }
        }
        public void SetData(InfoGrants grant)
        {
            if (grant != null)
            {
                nameGrantTb.Text = grant.Name;
                fioScientTb.Text = "Ученый: " + grant.FioScient;
                directNameTb.Text = "Направление: " + grant.NameDirect;
                dateStartTb.Text = "Дата начала: " + grant.DateStart.Value.ToString("dd.MM.yyyy");
                dateEndTb.Text = "Дата окончания: " + grant.DateEnd.Value.ToString("dd.MM.yyyy");
                organizationTb.Text = "Организация: " + grant.Organization ;
                summaTb.Text = grant.Summa.Value.ToString("N0") + " руб.";
            }
        }
        private void PostgreUserControlOne_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is InfoGrantsPostgre grant)
            {
                SetDataPostgre(grant);
            }
        }
        public void SetDataPostgre(InfoGrantsPostgre grant)
        {
            if (grant != null)
            {
                nameGrantTb.Text = grant.Name;
                fioScientTb.Text = "Ученый: " + grant.FioScient;
                directNameTb.Text = "Направление: " + grant.NameDirect;
                dateStartTb.Text = "Дата начала: " + grant.DateStart;
                dateEndTb.Text = "Дата окончания: " + grant.DateEnd;
                organizationTb.Text = "Организация: " + grant.Organization;
                summaTb.Text = grant.Summa.Value.ToString("N0") + " руб.";
            }
        }
    }
}
