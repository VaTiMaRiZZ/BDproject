using AppFive.ModelsPostgre;
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
    /// Логика взаимодействия для PostgreSql.xaml
    /// </summary>
    public partial class PostgreSql : Window
    {
        private ScienceDbContext _context;
        public PostgreSql()
        {
            InitializeComponent();
            _context = new ScienceDbContext();
            loadCbLinq();
            loadCbSql();
            LoadGridLinq();
            LoadGridSql();
        }
        private void loadCbLinq()
        {
            var scient = _context.Scientists.Select(s=>s.FioScient).ToList();
            scient.Insert(0, "Все");
            fioScientCb.ItemsSource = scient;
            fioScientCb.SelectedIndex = 0;
            var direct = _context.Directions.Select(d=>d.NameDirection).ToList();
            direct.Insert(0, "Все");
            nameDirectCb.ItemsSource = direct;
            nameDirectCb.SelectedIndex = 0;
        }
        private void loadCbSql()
        {
            var scient = _context.Scientists.FromSqlRaw(@"SELECT s.""fio_scient"" FROM ""scientists"" s").Select(s=>s.FioScient).ToList();
            scient.Insert(0, "Все");
            SQLfioScientCb.ItemsSource = scient;
            SQLfioScientCb.SelectedIndex = 0;
            var direct = _context.Directions.FromSqlRaw(@"SELECT d.""name_direction"" FROM ""directions"" d").Select(d=>d.NameDirection).ToList();
            direct.Insert(0, "Все");
            SQLnameDirectCb.ItemsSource = direct;
            SQLnameDirectCb.SelectedIndex = 0;
        }
        private void sqlBt_Click(object sender, RoutedEventArgs e)
        {
            sqlQuery.Visibility = Visibility.Visible;
            linqQuery.Visibility = Visibility.Collapsed;
            fioScientCb.SelectedIndex = 0;
            nameDirectCb.SelectedIndex = 0;
            SQLfioScientCb.SelectedIndex = 0;
            SQLnameDirectCb.SelectedIndex = 0;
        }
        private void linqBt_Click(object sender, RoutedEventArgs e)
        {
            sqlQuery.Visibility = Visibility.Collapsed;
            linqQuery.Visibility = Visibility.Visible;
            fioScientCb.SelectedIndex = 0;
            nameDirectCb.SelectedIndex = 0;
            SQLfioScientCb.SelectedIndex = 0;
            SQLnameDirectCb.SelectedIndex = 0;
        }
        private void exitBt_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
        private void LoadGridLinq()
        {
            string selectedFio = fioScientCb.SelectedItem as string;
            string selectdeDirect = nameDirectCb.SelectedItem as string;

            var query = from grant in _context.Grants
                        join scientist in _context.Scientists on grant.IdScient equals scientist.IdScient
                        join direction in _context.Directions on grant.IdDirection equals direction.IdDirection
                        select new InfoGrantsPostgre
                        {
                            Id = grant.IdGrants,
                            Name = grant.NameTheme,
                            DateStart = grant.DateStart,
                            DateEnd = grant.DateEnd,
                            Organization = grant.Organization,
                            Summa = grant.Summa,
                            FioScient = grant.IdScientNavigation.FioScient,
                            NameDirect = grant.IdDirectionNavigation.NameDirection
                        };
            if (selectedFio != null && selectedFio != "Все")
                query = query.Where(q => q.FioScient.Contains(selectedFio));
            if (selectdeDirect != null && selectdeDirect != "Все")
                query = query.Where(q => q.NameDirect.Contains(selectdeDirect));
            var result = query.ToList();
            linqGrid.ItemsSource = result;
        }
        private void LoadGridSql()
        {
            string selectedFio = fioScientCb.SelectedItem as string;
            string selectdeDirect = nameDirectCb.SelectedItem as string;

            var query = _context.Grants.FromSqlRaw(@"SELECT g.""id_grants"", g.""name_theme"", g.""date_start"", g.""date_end"",
                    g.""organization"", g.""summa"", s.""id_scient"", d.""id_direction""
                    FROM ""grants"" g
                    JOIN ""scientists"" s ON s.""id_scient"" = g.""id_scient""
                    JOIN ""directions"" d ON d.""id_direction"" = g.""id_direction""");
            var result = query.Select(r => new InfoGrantsPostgre
            {
                Id = r.IdGrants,
                Name = r.NameTheme,
                DateStart = r.DateStart,
                DateEnd = r.DateEnd,
                Organization = r.Organization,
                Summa = r.Summa,
                FioScient = r.IdScientNavigation.FioScient,
                NameDirect = r.IdDirectionNavigation.NameDirection
            }).ToList();
            if (selectedFio != null && selectedFio != "Все")
                result = result.Where(r => r.FioScient.Contains(selectedFio)).ToList();
            if (selectdeDirect != null && selectdeDirect != "Все")
                result = result.Where(r => r.NameDirect.Contains(selectdeDirect)).ToList();
            sqlGrid.ItemsSource = result;
        }
        private void fioScientCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadGridLinq();
        }
        private void SQLfioScientCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadGridSql();
        }
    }

    public class InfoGrantsPostgre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly? DateStart { get; set; }
        public DateOnly? DateEnd { get; set; }
        public string Organization { get; set; }
        public int? Summa { get; set; }
        public string FioScient { get; set; }
        public string NameDirect { get; set; }
    }

}
