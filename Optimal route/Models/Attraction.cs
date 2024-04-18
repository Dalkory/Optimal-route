using Newtonsoft.Json;
using System.ComponentModel;
using System.Windows;

namespace Optimal_route.Models
{
    public class Attraction : INotifyPropertyChanged
    {
        private string _name;
        private double _time;
        private int _importance;

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                {
                    return;
                }
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        [JsonProperty(PropertyName = "time")]
        public double Time
        {
            get => _time;
            set
            {
                if (value <= 0)
                {
                    MessageBox.Show("Время должно быть положительным числом!");
                    return;
                }
                if (_time == value)
                {
                    return;
                }
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        [JsonProperty(PropertyName = "importance")]
        public int Importance
        {
            get => _importance;
            set
            {
                if (value <= 0)
                {
                    MessageBox.Show("Важность должна быть положительным числом!");
                    return;
                }
                if (_importance == value)
                {
                    return;
                }
                _importance = value;
                OnPropertyChanged(nameof(Importance));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
