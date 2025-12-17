using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Models;
using Models.Repository;
using RentalKiosk.View;
using WPFLib.Services;
using WPFLib.Utility;
using WPFLib.ViewModel;

namespace RentalKiosk.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<ResourceType> _resourceTypeRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Resource> _resourceRepository;

        public ObservableCollection<Booking> Bookings { get; set; }
        public ObservableCollection<ResourceType> ResourceTypes { get; set; }
        public ObservableCollection<DateTime> WeekDays { get; } = new();
        public ObservableCollection<Resource> ResourcesForSelectedType { get; } = new();
        public ObservableCollection<Resource> AllResources { get; set; }
        public ObservableCollection<TimeSlot> TimeSlots { get; } = new();
        public ObservableCollection<TimeSlot> AvailableTimeSlots { get; } = new ObservableCollection<TimeSlot>();
        public ObservableCollection<TimeSlot> SelectedTimeSlots { get; } = new ObservableCollection<TimeSlot>();

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

        private double _price;
        public double Price { get; set; }

        private double _calculatedPrice;
        public double CalculatedPrice { get; set; }

        private bool _requirementFulfilled;
        public bool RequirementFulfilled { get; set; }

        private bool _isPaid;
        public bool IsPaid { get; set; }

        private int _selectedPersonId;
        public int SelectedPersonId { get; set; }

        private int _selectedResourceId;
        public int SelectedResourceId { get; set; }

        private ResourceType _selectedResourceType;
        public ResourceType SelectedResourceType
        {
            get => _selectedResourceType;
            set
            {
                _selectedResourceType = value;
                FilterResourcesByType();
                OnPropertyChanged();
            }
        }

        private Resource _selectedResource;
        public Resource SelectedResource
        {
            get => _selectedResource;
            set
            {
                _selectedResource = value;
                PopulateTimeSlots();
                OnPropertyChanged();

                if (_selectedResource != null)
                    LoadAvailableSlots(_selectedResource.Id, SelectedDate);
            }
        }

        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();

                if (SelectedResource != null)
                {
                    PopulateTimeSlots();
                    LoadAvailableSlots(SelectedResource.Id, _selectedDate);
                }
            }
        }

        private TimeSlot _selectedTimeSlot;
        public TimeSlot SelectedTimeSlot
        {
            get => _selectedTimeSlot;
            set
            {
                _selectedTimeSlot = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedBookingHeader));
                UpdateCalculatedPrice();
            }
        }

        //_startSlot + _endSlot used in ExecuteSelectTimeSlotCommand
        private TimeSlot? _startSlot;
        public TimeSlot? StartSlot 
        {
            get => _startSlot;
            private set 
            {
                _startSlot = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedBookingHeader));
            }
        }
        private TimeSlot? _endSlot;
        public TimeSlot? EndSlot
        {
            get => _endSlot;
            private set
            {
                _endSlot = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedBookingHeader));
            }
        }



        public string SelectedBookingHeader
        {
            get
            {
                if (StartSlot == null)
                    return "Select a time range";

                if (EndSlot == null)
                    return $"From {StartSlot.StartTime:HH:mm}";

                //var hours = SelectedTimeSlots.Count;
                var duration = EndSlot.StartTime - StartSlot.StartTime;
                var hours = (int)Math.Ceiling(duration.TotalHours) + 1;

                return $"{StartSlot.StartTime:HH:mm} – {EndSlot.StartTime:HH:mm} ({hours} hour{(hours > 1 ? "s" : "")})";

                //if (SelectedResource == null || SelectedTimeSlots == null || SelectedTimeSlots.Count == 0)
                //    return "Valg ressource og tidspunkt";

                //// Earliest and latest selected times
                //var start = SelectedTimeSlots.Min(s => s.StartTime);
                //var end = SelectedTimeSlots.Max(s => s.StartTime).AddHours(1);

                //return $"{SelectedResource.Title} " +
                //       $"{SelectedDate:dd.MM.yyyy} kl. {start:HH:mm}-{end:HH:mm}";
            }
        }

        public DateTime? BookingStart =>
            SelectedTimeSlots.Any()
                ? SelectedTimeSlots.Min(s => s.StartTime)
                : null;

        public DateTime? BookingEnd =>
            SelectedTimeSlots.Any()
                ? SelectedTimeSlots.Max(s => s.StartTime).AddHours(1)
                : null;

        public ICommand AddBookingCommand { get; }
        public ICommand AddPersonCommand { get; }
        public ICommand NextWeekCommand { get; }
        public ICommand PreviousWeekCommand { get; }
        public ICommand SelectDateAndResourceCommand { get; }
        public ICommand SelectTimeSlotCommand { get; }
        public ICommand ClearAllCommand { get; }

        public MainViewModel(IRepository<Booking> bookingRepository, IRepository<ResourceType> resourceTypeRepository, IRepository<Resource> resourceRepository, IRepository<Person> personRepository)
        {
            _bookingRepository = bookingRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _resourceRepository = resourceRepository;
            _personRepository = personRepository;


            try
            {
                Bookings = new ObservableCollection<Booking>(_bookingRepository.GetAll());
            }
            catch (Exception)
            {
                MessageService.Show("Der opstod en fejl ved hentning af bookinger?");
            }

            try
            {
                ResourceTypes = new ObservableCollection<ResourceType>(_resourceTypeRepository.GetAll());
            }
            catch (Exception)
            {
                MessageService.Show("Der opstod en fejl ved hentning af resourcetypes?");
            }

            try
            {
                AllResources = new ObservableCollection<Resource>(_resourceRepository.GetAll());
            }
            catch (Exception)
            {
                MessageService.Show("Der opstod en fejl ved hentning af allresources?");
            }

            AddBookingCommand = new RelayCommand(ExecuteAddBooking, CanAddBooking);
            AddPersonCommand = new RelayCommand(ExecuteAddPerson, CanAddPerson);
            NextWeekCommand = new RelayCommand(ExecuteNextWeek);
            PreviousWeekCommand = new RelayCommand(ExecutePreviousWeek);
            SelectDateAndResourceCommand = new RelayCommand(ExecuteSelectDateAndResourceCommand);
            SelectTimeSlotCommand = new RelayCommand(ExecuteSelectTimeSlotCommand);

            // Clear All command
            ClearAllCommand = new RelayCommand(_ => ExecuteClearAll());

            // Start på ugen = mandag i denne uge
            var today = DateTime.Today;
            int diff = (7 + (int)today.DayOfWeek - (int)DayOfWeek.Monday) % 7;
            CurrentWeekStart = today.AddDays(-diff);
        }


        // Clear All logik - nulstiller hele kiosken
        private void ExecuteClearAll()
        {
            // Formularfelter
            Name = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            CalculatedPrice = 0;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Phone));
            OnPropertyChanged(nameof(CalculatedPrice));

            // Valgte tider
            StartSlot = null;
            EndSlot = null;
            SelectedTimeSlots.Clear();
            OnPropertyChanged(nameof(SelectedTimeSlots));
            OnPropertyChanged(nameof(SelectedBookingHeader));

            // Tidsoversigt
            TimeSlots.Clear();
            AvailableTimeSlots.Clear();
            OnPropertyChanged(nameof(TimeSlots));
            OnPropertyChanged(nameof(AvailableTimeSlots));

            // Valg i flow
            SelectedResource = null;
            OnPropertyChanged(nameof(SelectedResource));

            SelectedDate = DateTime.Today; // eller behold hvis I vil
            OnPropertyChanged(nameof(SelectedDate));

            ResourcesForSelectedType.Clear();
            OnPropertyChanged(nameof(ResourcesForSelectedType));

            SelectedResourceType = null;
            OnPropertyChanged(nameof(SelectedResourceType));
        }

        

        public void ExecuteAddBooking(object parameter)
        {
            if (SelectedResource == null || SelectedTimeSlots == null || SelectedTimeSlots.Count == 0)
            {
                MessageService.Show("Vælg venligst ressource og tidspunkt.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageService.Show("Indtast venligst navn.");
                return;
            }

            // check if email is valid format with regular expression
            if (!Validator.IsValidEmail(Email))
            {
                MessageService.Show("Indtast venligst en gyldig email.");
                return;
            }

            try
            {
                Person person = new Person(Name, Email, Phone);
                int personId = _personRepository.Add(person);

                DateTime start = SelectedTimeSlots.Min(s => s.StartTime);
                DateTime end = SelectedTimeSlots.Max(s => s.StartTime).AddHours(1); // assuming 1-hour slots

                decimal price = (decimal)CalculatedPrice;

                Booking newBooking = new Booking(
                        resourceId: SelectedResource.Id,
                        personId: personId,
                        startTime: start,
                        endTime: end,
                        totalPrice: price,
                        requirementFulfilled: false,
                        isPaid: false
                    );


                int newId = _bookingRepository.Add(newBooking);

                Bookings.Add(newBooking);

                MessageService.Show($"Booking #{newId} til {Name} oprettet!");

                // Auto-clear after success
                ExecuteClearAll();
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

        public void ExecuteNextWeek(object parameter)
        {
            CurrentWeekStart = CurrentWeekStart.AddDays(7);
        }

        public void ExecutePreviousWeek(object parameter)
        {
            CurrentWeekStart = CurrentWeekStart.AddDays(-7);
        }

        private void FilterResourcesByType()
        {
            ResourcesForSelectedType.Clear();
            if (SelectedResourceType != null)
            {
                foreach (var resource in AllResources)
                {
                    if (resource.ResourceTypeId == SelectedResourceType.Id)
                    {
                        ResourcesForSelectedType.Add(resource);
                    }
                }
            }
        }

        private void ExecuteSelectDateAndResourceCommand(object parameter)
        {
            var paramString = parameter as string;
            if (string.IsNullOrWhiteSpace(paramString)) return;

            var parts = paramString.Split('|');
            if (parts.Length != 2) return;

            SelectedDate = DateTime.Parse(parts[0]);

            int id = int.Parse(parts[1]);
            SelectedResource = ResourcesForSelectedType.First(r => r.Id == id);
        }

        private void PopulateTimeSlots()

        {

            if (SelectedResource == null)

                return;

            TimeSlots.Clear();

            var date = SelectedDate;

            var bookingsForResourceAndDate = Bookings.Where(b => b.ResourceId == SelectedResource.Id && b.StartTime.Date == date.Date).ToList();


            for (int hour = 7; hour <= 22; hour++)

            {

                bool availability = !bookingsForResourceAndDate.Any(b => (b.StartTime.Hour <= (hour + 1) && b.EndTime.Hour > hour));

                TimeSlots.Add(new TimeSlot
                {

                    ResourceId = SelectedResource.Id,

                    StartTime = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0),

                    IsAvailable = availability

                });

            }

        }

        public void LoadAvailableSlots(int resourceId, DateTime date) // Bliver denne metode brugt til noget?

        {

            AvailableTimeSlots.Clear();

            //foreach (var slot in TimeSlots)

            //{

            //    slot.IsAvailable = slot.ResourceId == resourceId && slot.StartTime.Date == date.Date;

            //}

            OnPropertyChanged(nameof(TimeSlots));

        }


        public void ExecuteSelectTimeSlotCommand(object parameter)
        {
            if (parameter is not TimeSlot clicked)
                return;

            //No start yet
            if (_startSlot == null) 
            {
                StartSlot = clicked;
                EndSlot = null;
                RebuildRange();
                return;
            }

            // Start is set men ikke sluttid
            if (_endSlot == null) 
            {
                EndSlot = clicked;
                NormalizeAnchors();
                RebuildRange();
                return;
            }

            // Range Exists
            if (clicked.StartTime < StartSlot.StartTime)
            {
                StartSlot = clicked;
            }
            else if (clicked.StartTime > EndSlot.StartTime)
            {
                EndSlot = clicked;
            }
            else
            {
            var distToStart = Math.Abs((clicked.StartTime - StartSlot.StartTime).TotalMinutes);
            var distToEnd = Math.Abs((clicked.StartTime - EndSlot.StartTime).TotalMinutes);

                //Inside range -> move nearest edge
                if (distToStart <= distToEnd)
                    StartSlot = clicked;
                else
                    EndSlot = clicked;
            }

            NormalizeAnchors();
            RebuildRange();

        }

        //helpermetoder til ExecuteSelectTimeSlotCommand
        private void NormalizeAnchors() 
        {
            if (_startSlot == null || _endSlot == null)
            return;

            if (_startSlot.StartTime > _endSlot.StartTime)
                (_startSlot, _endSlot) = (_endSlot, _startSlot);
        }

        private void RebuildRange() 
        {
            SelectedTimeSlots.Clear();

            if (_startSlot == null)
                return;

            if (_endSlot == null) 
            {
                SelectedTimeSlots.Add(_startSlot);
                UpdateCalculatedPrice();
                return;
            }

            var min = _startSlot.StartTime;
            var max = _endSlot.StartTime;

            var range = TimeSlots
                .Where(ts =>
                    ts.IsAvailable &&
                    ts.StartTime >= min &&
                    ts.StartTime <= max)
                .OrderBy(ts => ts.StartTime);

            foreach (var slot in range)
                SelectedTimeSlots.Add(slot);

            UpdateCalculatedPrice();
        }

        private void UpdateCalculatedPrice()
        {
            if (SelectedResource == null || SelectedTimeSlots == null || SelectedTimeSlots.Count == 0)
            {
                CalculatedPrice = 0;
            }
            else
            {
                // Assuming each slot is 1 hour
                CalculatedPrice = SelectedTimeSlots.Count * SelectedResource.Price;
            }

            OnPropertyChanged(nameof(CalculatedPrice));
        }
    }
}
