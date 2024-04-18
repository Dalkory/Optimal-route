using Optimal_route.Models;
using Optimal_route.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Optimal_route
{
    public partial class MainWindow : Window
    {
        private readonly string _filePath = $"{Environment.CurrentDirectory}\\attractionsDataList.json";
        private BindingList<Attraction> _attractionsDataList;
        private FileIOService _fileIOService;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIOService = new FileIOService(_filePath);

            try
            {
                _attractionsDataList = await _fileIOService.LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            attractionsDataGrid.ItemsSource = _attractionsDataList;
            _attractionsDataList.ListChanged += _attractionsDataList_ListChanged;
        }

        private async void _attractionsDataList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    await _fileIOService.SaveDataAsync(sender);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }
            }
        }

        private void CalculateRoute_Click(object sender, RoutedEventArgs e)
        {
            List<Attraction> sortedAttractions = _attractionsDataList.OrderByDescending(a => a.Importance).ToList();
            if (string.IsNullOrWhiteSpace(daysTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите количество дней!");
                return;
            }
            if (!int.TryParse(daysTextBox.Text, out int numberOfDays) || numberOfDays <= 0)
            {
                MessageBox.Show("Количество дней должно быть положительным числом!");
                return;
            }
            double totalHours = numberOfDays * 24;
            totalHours -= numberOfDays * 8;

            List<Attraction> selectedAttractions = new List<Attraction>();
            double remainingHours = totalHours;

            foreach (Attraction attraction in sortedAttractions)
            {
                if (remainingHours >= attraction.Time)
                {
                    selectedAttractions.Add(attraction);
                    remainingHours -= attraction.Time;
                }
            }

            string result = "Оптимальный маршрут:\n";
            foreach (Attraction attraction in selectedAttractions)
            {
                string hoursText = attraction.Time == 1 ? "час" : attraction.Time >= 2 && attraction.Time <= 4 ? "часа" : "часов";

                result += $"{attraction.Name} ({attraction.Time} {hoursText})\n";
            }

            resultTextBox.Text = result;
            resultTextBox.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            resultTextBox.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Collapsed;
        }
    }
}
