using AppFive.ModelsSQL;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace AppFive
{
    /// <summary>
    /// Логика взаимодействия для Mysql.xaml
    /// </summary>
    public partial class Mysql : Window
    {
        private ScientificFoundationContext _context;
        public Mysql()
        {
            InitializeComponent();
            _context = new ScientificFoundationContext();
            LoadComboLinq();
            LoadComboSql();
            LoadDataToGridLinq();
        }
         private void LoadComboLinq()
        {
            var scient = _context.Scientists.Select(s => s.FioScient).ToList();
            scient.Insert(0, "Все");
            fioCbLinq.ItemsSource = scient;
            fioCbLinq.SelectedIndex = 0;

            var direct = _context.Directions.Select(d => d.NameDirection).ToList();
            direct.Insert(0, "Все");
            directCbLinq.ItemsSource = direct;
            directCbLinq.SelectedIndex = 0;
        }

        private void LoadComboSql()
        {
            var scient = _context.Scientists.Select(s => s.FioScient).ToList();
            scient.Insert(0, "Все");
            fioCbSql.ItemsSource = scient;
            fioCbSql.SelectedIndex = 0;

            var direct = _context.Directions.Select(d => d.NameDirection).ToList();
            direct.Insert(0, "Все");
            directCbSql.ItemsSource = direct;
            directCbSql.SelectedIndex = 0;
        }

        private void LoadDataToGridLinq()
        {
            string selectedScient = fioCbLinq.SelectedItem as string;
            string selectedDirect = directCbLinq.SelectedItem as string;

            var query = from grant in _context.Grants
                        join scientist in _context.Scientists on grant.IdScient equals scientist.IdScient
                        join direction in _context.Directions on grant.IdDirection equals direction.IdDirection
                        select new InfoGrants
                        {
                            Id = grant.IdGrants,
                            Name = grant.NameTheme,
                            Summa = grant.Summa,
                            DateStart = grant.DateStart,
                            DateEnd = grant.DateEnd,
                            Organization = grant.Organization,
                            FioScient = scientist.FioScient,
                            NameDirect = direction.NameDirection
                        };

            if (selectedScient != null && selectedScient != "Все")
                query = query.Where(g => g.FioScient.Contains(selectedScient));
            if (selectedDirect != null && selectedDirect != "Все")
                query = query.Where(g => g.NameDirect.Contains(selectedDirect));
            var result = query.ToList();
            gridLinq.ItemsSource = result;
        }

        private void LoadDataToGridSql()
        {
            string selectedScient = fioCbSql.SelectedItem as string;
            string selectedDirect = directCbSql.SelectedItem as string;

            var query = _context.Grants.FromSqlRaw(
                @"SELECT g.id_grants, g.name_theme, g.summa, g.date_start, g.date_end, g.organization,
                         s.fio_scient, d.name_direction, s.id_scient,d.id_direction
                  FROM grants g
                  INNER JOIN scientists s ON g.id_scient = s.id_scient
                  INNER JOIN directions d ON g.id_direction = d.id_direction");

            var result = query.Select(g => new InfoGrants
            {
                Id = g.IdGrants,
                Name = g.NameTheme,
                Summa = g.Summa,
                DateStart = g.DateStart,
                DateEnd = g.DateEnd,
                Organization = g.Organization,
                FioScient = g.IdScientNavigation.FioScient,
                NameDirect = g.IdDirectionNavigation.NameDirection
            }).ToList();

            if (selectedScient != null && selectedScient != "Все")
                result = result.Where(g => g.FioScient.Contains(selectedScient)).ToList();
            if (selectedDirect != null && selectedDirect != "Все")
                result = result.Where(g => g.NameDirect.Contains(selectedDirect)).ToList();
            gridSql.ItemsSource = result;
        }
        private void linq_Click(object sender, RoutedEventArgs e)
        {
            linqQuery.Visibility = Visibility.Visible;
            sqlQuery.Visibility = Visibility.Collapsed;
        }

        private void sql_Click(object sender, RoutedEventArgs e)
        {
            linqQuery.Visibility = Visibility.Collapsed;
            sqlQuery.Visibility = Visibility.Visible;
            LoadDataToGridSql();
        }

        private void fioCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDataToGridLinq();
        }

        private void directCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDataToGridLinq();
        }

        private void fioCbSql_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDataToGridSql();
        }

        private void directCbSql_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDataToGridSql();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

    }
    public class InfoGrants
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Organization { get; set; }
        public int? Summa { get; set; }
        public string FioScient { get; set; }
        public string NameDirect { get; set; }
    }
}
