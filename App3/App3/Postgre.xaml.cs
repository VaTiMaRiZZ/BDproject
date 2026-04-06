using App3.ModelsPostgre;
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

namespace App3
{
    /// <summary>
    /// Логика взаимодействия для Postgre.xaml
    /// </summary>
    public partial class Postgre : Window
    {
        private ScienceDbContext _context;
        private int? _editGrantId = null;
        public Postgre()
        {
            InitializeComponent();
            _context = new ScienceDbContext();
            LoadDataLINQ();
            LoadDataSQL();
            LoadGridSQL();
            LoadDataToGrid();
        }
        private void LoadDataLINQ()
        {
            var scient = _context.Scientists
                .Select(s => s.FioScient)
                .ToList();
            scient.Insert(0, "Все");
            oneTableLinq.Items.Clear();
            oneTableLinq.ItemsSource = scient;
            var direct = _context.Directions
                .Select(d => d.NameDirection)
                .ToList();
            twoTableLinq.Items.Clear();
            twoTableLinq.ItemsSource = direct;
            direct.Insert(0, "Все");
            oneTableLinq.SelectedIndex = 0;
            twoTableLinq.SelectedIndex = 0;
        }
        private void LoadDataSQL()
        {
            var scient = _context.Scientists.FromSqlRaw(@"SELECT s.""fio_scient"" FROM ""scientists"" s").Select(s => s.FioScient).ToList();
            scient.Insert(0, "Все");
            oneSqlTable.Items.Clear();
            oneSqlTable.ItemsSource = scient;
            var direct = _context.Directions.FromSqlRaw(@"SELECT d.""name_direction"" FROM ""directions"" d").Select(d => d.NameDirection).ToList();
            direct.Insert(0, "Все");
            twoSqlTable.Items.Clear();
            twoSqlTable.ItemsSource = direct;
            oneSqlTable.SelectedIndex = 0;
            twoSqlTable.SelectedIndex = 0;
        }
        private void LoadDataToGrid()
        {
            string selectedScient = oneTableLinq.SelectedItem as string;
            string selectedDirect = twoTableLinq.SelectedItem as string;
            var query = from Grant in _context.Grants
                        join Scientist in _context.Scientists on Grant.IdScient equals Scientist.IdScient
                        join Direction in _context.Directions on Grant.IdDirection equals Direction.IdDirection
                        select new InfoGrantsPostgre
                        {
                            Id = Grant.IdGrants,
                            Name = Grant.NameTheme,
                            Summa = Grant.Summa,
                            DateStart = Grant.DateStart,
                            DateEnd = Grant.DateEnd,
                            Organization = Grant.Organization,
                            FioScient = Scientist.FioScient,
                            NameDirect = Direction.NameDirection
                        };
            if (selectedScient != null && selectedScient != "Все")
            {
                query = query.Where(g => g.FioScient.Contains(selectedScient));
            }
            if (selectedDirect != null && selectedDirect != "Все")
            {
                query = query.Where(g => g.NameDirect.Contains(selectedDirect));
            }
            var result = query.ToList();
            gridLinq.ItemsSource = result;
        }
        private void LoadGridSQL()
        {
            string selectedScient = oneSqlTable.SelectedItem as string;
            string selectedDirect = twoSqlTable.SelectedItem as string;

            var query = _context.Grants.FromSqlRaw(
                @"SELECT g.""id_grants"", g.""name_theme"", g.""summa"", g.""date_start"", g.""date_end"", g.""organization"", g.""id_scient"", g.""id_direction"",
                 s.""fio_scient"", d.""name_direction""
          FROM grants g
          INNER JOIN ""scientists"" s ON g.""id_scient"" = s.""id_scient""
          INNER JOIN ""directions"" d ON g.""id_direction"" = d.""id_direction""");

            var result = query.Select(s => new InfoGrantsPostgre
            {
                Id = s.IdGrants,
                Name = s.NameTheme,
                Summa = s.Summa,
                DateStart = s.DateStart,
                DateEnd = s.DateEnd,
                Organization = s.Organization,
                FioScient = s.IdScientNavigation.FioScient,
                NameDirect = s.IdDirectionNavigation.NameDirection
            }).ToList();

            if (selectedScient != null && selectedScient != "Все")
            {
                result = result.Where(g => g.FioScient.Contains(selectedScient)).ToList();
            }
            if (selectedDirect != null && selectedDirect != "Все")
            {
                result = result.Where(g => g.NameDirect.Contains(selectedDirect)).ToList();
            }
            gridSql.ItemsSource = result;
        }

        private void oneSqlTablePostgre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadGridSQL();
        }
        private void twoSqlTablePostgre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadGridSQL();
        }

        private void sql_Click(object sender, RoutedEventArgs e)
        {
            sqlQuery.Visibility = Visibility.Visible;
            linqQuery.Visibility = Visibility.Collapsed;
            oneSqlTable.SelectedIndex = 0;
            twoSqlTable.SelectedIndex = 0;
            oneTableLinq.SelectedIndex = 0;
            twoTableLinq.SelectedIndex = 0;
        }

        private void linq_Click(object sender, RoutedEventArgs e)
        {
            sqlQuery.Visibility = Visibility.Collapsed;
            linqQuery.Visibility = Visibility.Visible;
            oneSqlTable.SelectedIndex = 0;
            twoSqlTable.SelectedIndex = 0;
            oneTableLinq.SelectedIndex = 0;
            twoTableLinq.SelectedIndex = 0;
        }

        private void oneTableLinq_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDataToGrid();
        }

        private void twoTableLinq_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDataToGrid();
        }
        private void saveBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(grantTb.Text) || string.IsNullOrEmpty(dateStartTb.Text) || string.IsNullOrEmpty(dateEndTb.Text)
                    || string.IsNullOrEmpty(organizationTb.Text) || string.IsNullOrEmpty(summaTb.Text))
                {
                    MessageBox.Show("Заполните поля!");
                    return;
                }
                string selectedScientName = (linqQuery.Visibility == Visibility.Visible) ?
                    oneTableLinq.SelectedItem as string : oneSqlTable.SelectedItem as string;
                string selectedDirectionName = (linqQuery.Visibility == Visibility.Visible) ?
                    twoTableLinq.SelectedItem as string : twoSqlTable.SelectedItem as string;

                if (selectedScientName == "Все" || string.IsNullOrEmpty(selectedScientName))
                {
                    MessageBox.Show("Выберите конкретного ученого!", "Ошибка");
                    return;
                }

                if (selectedDirectionName == "Все" || string.IsNullOrEmpty(selectedDirectionName))
                {
                    MessageBox.Show("Выберите конкретное направление!", "Ошибка");
                    return;
                }
                var scientist = _context.Scientists.FirstOrDefault(s => s.FioScient == selectedScientName);
                var direction = _context.Directions.FirstOrDefault(d => d.NameDirection == selectedDirectionName);
                if (scientist == null || direction == null)
                {
                    MessageBox.Show("Ученый или направление не найдены!", "Ошибка");
                    return;
                }
                DateOnly? startDate = null;
                if (!string.IsNullOrWhiteSpace(dateStartTb.Text))
                {
                    startDate = DateOnly.Parse(dateStartTb.Text);
                }
                DateOnly? endDate = null;
                if (!string.IsNullOrWhiteSpace(dateEndTb.Text))
                {
                    endDate = DateOnly.Parse(dateEndTb.Text);
                }
                int? summa = null;
                if (!string.IsNullOrWhiteSpace(summaTb.Text))
                {
                    summa = int.Parse(summaTb.Text);
                }
                if (_editGrantId.HasValue)
                {
                    var grantToUpdate = _context.Grants.FirstOrDefault(g => g.IdGrants == _editGrantId.Value);
                    if (grantToUpdate != null)
                    {
                        grantToUpdate.NameTheme = grantTb.Text;
                        grantToUpdate.DateStart = startDate;
                        grantToUpdate.DateEnd = endDate;
                        grantToUpdate.Organization = organizationTb.Text;
                        grantToUpdate.Summa = summa;
                        grantToUpdate.IdScient = scientist.IdScient;
                        grantToUpdate.IdDirection = direction.IdDirection;
                        _context.Entry(grantToUpdate).State = EntityState.Modified;
                        MessageBox.Show("Запись успешно обновлена!", "Успех");
                    }
                }
                else
                {
                    var newGrant = new Grant
                    {
                        NameTheme = grantTb.Text,
                        DateStart = startDate,
                        DateEnd = endDate,
                        Organization = organizationTb.Text,
                        Summa = summa,
                        IdScient = scientist.IdScient,
                        IdDirection = direction.IdDirection
                    };
                    _context.Grants.Add(newGrant);
                    MessageBox.Show("Запись успешно добавлена!", "Успех");
                }
                _context.SaveChanges();
                ClearTextBoxes();
                LoadDataToGrid();
                LoadGridSQL();
                _editGrantId = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }
        private void ClearTextBoxes()
        {
            grantTb.Text = "";
            dateStartTb.Text = "";
            dateEndTb.Text = "";
            organizationTb.Text = "";
            summaTb.Text = "";
            _editGrantId = null;
        }
        private void deleteBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGrid activeGrid = (linqQuery.Visibility == Visibility.Visible) ? gridLinq : gridSql;

                if (activeGrid.SelectedItem == null)
                {
                    MessageBox.Show("Выберите запись для удаления!", "Система");
                    return;
                }
                InfoGrantsPostgre selectedGrant = activeGrid.SelectedItem as InfoGrantsPostgre;

                if (selectedGrant == null)
                {
                    MessageBox.Show("Ошибка выбора записи!", "Ошибка");
                    return;
                }
                var grantToDelete = _context.Grants.FirstOrDefault(g => g.IdGrants == selectedGrant.Id);
                if (grantToDelete != null)
                {
                    _context.Grants.Remove(grantToDelete);
                    _context.SaveChanges();
                    MessageBox.Show("Запись успешно удалена!", "Успех");
                    LoadDataToGrid();
                    LoadGridSQL();
                    ClearTextBoxes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка");
            }
        }

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid send = sender as DataGrid;
            if (send.SelectedItem == null) return;

            InfoGrantsPostgre selectGrant = send.SelectedItem as InfoGrantsPostgre;
            if (selectGrant != null)
            {
                grantTb.Text = selectGrant.Name;
                dateStartTb.Text = selectGrant.DateStart?.ToString("yyyy-MM-dd");
                dateEndTb.Text = selectGrant.DateEnd?.ToString("yyyy-MM-dd");
                organizationTb.Text = selectGrant.Organization;
                summaTb.Text = selectGrant.Summa.ToString();
                _editGrantId = selectGrant.Id;
            }

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
