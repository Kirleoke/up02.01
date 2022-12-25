﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace passenger_transportation
{
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext();
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        // при загрузке окна
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // гарантируем, что база данных создана
            try
            {
                db.Database.EnsureCreated();
                db.Staff.Load();
            }
            catch { }
            // загружаем данные из БД
            // и устанавливаем данные в качестве контекста
            DataContext = db.Staff.Local.ToObservableCollection();
        }

        // добавление
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            StaffWindow StaffWindow = new StaffWindow(new Staff());
            if (StaffWindow.ShowDialog() == true)
            {
                Staff User = StaffWindow.Staff;
                db.Staff.Add(User);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException exception) { }
                catch(ArgumentException exception) { }
               
            }
        }
        // редактирование
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                // получаем выделенный объект
                Staff? staff = staffList.SelectedItem as Staff;
                // если ни одного объекта не выделено, выходим
                if (staff is null) return;

                StaffWindow StaffWindow = new StaffWindow(new Staff
                {
                    Id = staff.Id,
                    ShortId = staff.ShortId,
                    LastName = staff.LastName,
                    Name = staff.Name,
                    Patronymic = staff.Patronymic,
                    BirthdayDate = staff.BirthdayDate,
                    ContactPhone = staff.ContactPhone,
                    Department = staff.Department
                });

                if (StaffWindow.ShowDialog() == true)
                {
                    // получаем измененный объект
                    staff = db.Staff.Find(StaffWindow.Staff.Id);
                    if (staff != null)
                    {
                        staff.ShortId = StaffWindow.Staff.ShortId;
                        staff.LastName = StaffWindow.Staff.LastName;
                        staff.Name = StaffWindow.Staff.Name;
                        staff.Patronymic = StaffWindow.Staff.Patronymic;
                        staff.BirthdayDate = StaffWindow.Staff.BirthdayDate;
                        staff.ContactPhone = StaffWindow.Staff.ContactPhone;
                        staff.Department = StaffWindow.Staff.Department;

                        db.SaveChanges();
                        staffList.Items.Refresh();
                    }
                }
            }
        }
        // удаление
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            Staff? user = staffList.SelectedItem as Staff;
            // если ни одного объекта не выделено, выходим
            if (user is null) return;
            db.Staff.Remove(user);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException exception) { }
        }
        // Экспорт в json
        private void Export_Json_Click(object sender, RoutedEventArgs e)
        {
            StaffListJsonReport JsonExport = new StaffListJsonReport();
            JsonExport.Export_Json(db.Staff.Local.ToObservableCollection());
            
        }
        // Экспорт в xlsx
        private void Export_Xlsx_Click(object sender, RoutedEventArgs e)
        {
            StaffListExcelReport ExcelExport = new StaffListExcelReport();
            ExcelExport.Export_Excel(db.Staff.Local.ToObservableCollection());
        }
        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            SelectSortMethodWindow selectSortMethodWindow= new SelectSortMethodWindow();
            staffList.Items.SortDescriptions.Clear();
            if (selectSortMethodWindow.ShowDialog() == true)
            {
                ListBoxItem lbi = (ListBoxItem)(selectSortMethodWindow.selectSortAttribute.ItemContainerGenerator.ContainerFromIndex(selectSortMethodWindow.selectSortAttribute.SelectedIndex));
                staffList.Items.SortDescriptions.Add(new SortDescription(lbi.Name, selectSortMethodWindow.ascending.IsChecked == true ? ListSortDirection.Ascending : ListSortDirection.Descending));
            }
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            DataContext = db.Staff.Local.ToObservableCollection();
            StaffWindow StaffWindow = new StaffWindow(new Staff());
            if (StaffWindow.ShowDialog() == true)
            {
                DataContext = db.Staff.Local.Where(staff => returnTrueIfFind(StaffWindow, staff.ShortId, staff.LastName, staff.Name, staff.Patronymic, staff.BirthdayDate, staff.ContactPhone, staff.Department)).ToList();
            }
        }
        private bool returnTrueIfFind(StaffWindow StaffWindow, int shortId, string lastName, string name, string patronymic, string birthdateDate, long contactPhone, string department)
        {
            if (StaffWindow.Staff.ShortId == 0) shortId = 0;
            if (StaffWindow.Staff.LastName == null) lastName = null;
            if (StaffWindow.Staff.Name == null) name = null;
            if (StaffWindow.Staff.Patronymic == null) patronymic = null;
            if (StaffWindow.Staff.BirthdayDate == null) birthdateDate = null;
            if (StaffWindow.Staff.ContactPhone == 0) contactPhone = 0;
            if (StaffWindow.Staff.Department == null) department = null;
            return shortId == StaffWindow.Staff.ShortId && lastName == StaffWindow.Staff.LastName && name == StaffWindow.Staff.Name && patronymic == StaffWindow.Staff.Patronymic && birthdateDate == StaffWindow.Staff.BirthdayDate && contactPhone == StaffWindow.Staff.ContactPhone && department == StaffWindow.Staff.Department;
        }
    }
}