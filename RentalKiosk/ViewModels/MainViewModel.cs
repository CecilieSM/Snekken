using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLib.Services;
using WPFLib.Utility;
using WPFLib.ViewModel;
using System.Windows.Input;
using System.Windows;
using Models.Repository;
using Models;
using System.Collections.ObjectModel;

namespace RentalKiosk.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<ResourceType> _resourceTypeRepository;
        private readonly IRepository<Person> _personRepository;

        public ObservableCollection<Booking> Bookings { get; set; }
        public ObservableCollection<ResourceType> ResourceTypes { get; set; }
        public ObservableCollection<DateTime> WeekDays { get; } = new();

        private DateTime _currentWeekStart;
        public DateTime CurrentWeekStart 
        {
            get => _currentWeekStart;
            set
            {
                if (_currentWeekStart != value)
                {
                    _currentWeekStart = value;
                    OnPropertyChanged();
                    UpdateWeekDays();
                    OnPropertyChanged(nameof(CurrentWeekText));
                }
            }
        }

        public string CurrentWeekText =>
            $"{CurrentWeekStart:dd.MM.yyyy} - {CurrentWeekStart.AddDays(6):dd.MM.yyyy}";

        private int _bookingId;
        public int BookingId { get; set; }

        private int _personId;
        public int PersonId { get; set; }

        private int _resourceId;
        public int ResourceId { get; set; }

        private string _name;
        public string Name { get; set; }

        private string _email;
        public string Email { get; set; }

        private string _phone;
        public string Phone { get; set; }

        private DateTime _startTime;
        public DateTime StartTime { get; set; }

        private DateTime _endTime;
        public DateTime EndTime { get; set; }

        private bool _requirementFulfilled;
        public bool RequirementFulfilled { get; set; }

        private bool _isPaid;
        public bool IsPaid { get; set; }

        private int _selectedPersonId;
        public int SelectedPersonId { get; set; }

        private int _selectedResourceId;
        public int SelectedResourceId { get; set; }

        public ICommand AddBooking { get; }
        public ICommand AddPerson { get; }

        public MainViewModel(IRepository<Booking> bookingRepository, IRepository<ResourceType> resourceTypeRepository)
        {
            _bookingRepository = bookingRepository;
            _resourceTypeRepository = resourceTypeRepository;

            try
            {
                Bookings = new ObservableCollection<Booking>(_bookingRepository.GetAll());
                ResourceTypes = new ObservableCollection<ResourceType>(_resourceTypeRepository.GetAll());
            }
            catch (Exception)
            {
                MessageService.Show("Der opstod en fejl ved hentning af bookinger?");
            }

            AddBooking = new RelayCommand(ExecuteAddBooking, CanAddBooking);
            AddPerson = new RelayCommand(ExecuteAddPerson, CanAddPerson);

            // Start på ugen = mandag i denne uge
            var today = DateTime.Today;
            int diff = (7 + (int)today.DayOfWeek - (int)DayOfWeek.Monday) % 7;
            CurrentWeekStart = today.AddDays(-diff);
        }


        public void ExecuteAddBooking(object parameter)
        {
            try
            {
                Booking newBooking = new Booking(this.ResourceId, this.PersonId, this.StartTime, this.EndTime, this.RequirementFulfilled, this.IsPaid);

                int newId = _bookingRepository.Add(newBooking);

                MessageService.Show($"Booking #{newId} added successfully!");
            }

            catch (Exception ex)
            {
                MessageService.Show("An error occurred while adding a booking: " + ex.Message);
                MessageService.Log("Error in ExecuteAddBooking: " + ex.ToString());
            }
        }

        public bool CanAddBooking()
        {


            return true;
        }

        public void ExecuteAddPerson(object parameter)
        {
            try
            {
                Person newPerson = new Person(this.Name, this.Email, this.Phone);

                int newId = _personRepository.Add(newPerson);

                MessageService.Show($"Person #{newId} added successfully!");
            }

            catch (Exception ex)
            {
                MessageService.Show("An error occurred while adding a person: " + ex.Message);
                MessageService.Log("Error in ExecuteAddPerson: " + ex.ToString());
            }

        }

        public bool CanAddPerson()
        {
            return true;
        }

        private void UpdateWeekDays() 
        {
            WeekDays.Clear();
            for (int i = 0; i < 7; i++) 
            {
                WeekDays.Add(CurrentWeekStart.AddDays(i));
            }
        }

    }
}
