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

        public string SelectedBookingHeader
        {
            get
            {
                if (SelectedResource == null || SelectedTimeSlots == null || SelectedTimeSlots.Count == 0)
                    return "Valg ressource og tidspunkt";

                // Earliest and latest selected times
                var start = SelectedTimeSlots.Min(s => s.StartTime);
                var end = SelectedTimeSlots.Max(s => s.StartTime).AddHours(1);

                return $"{SelectedResource.Title} " +
                       $"{SelectedDate:dd.MM.yyyy} kl. {start:HH:mm}-{end:HH:mm}";
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

            // Start på ugen = mandag i denne uge
            var today = DateTime.Today;
            int diff = (7 + (int)today.DayOfWeek - (int)DayOfWeek.Monday) % 7;
            CurrentWeekStart = today.AddDays(-diff);
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
                //Booking newBooking = new Booking(this.ResourceId, this.PersonId, this.StartTime, this.EndTime, this.RequirementFulfilled, this.IsPaid);

                //int newId = _bookingRepository.Add(newBooking);

                //MessageService.Show($"Booking #{newId} added successfully!");

                Person person = new Person(Name, Email, Phone);
                int personId = _personRepository.Add(person);

                DateTime start = SelectedTimeSlots.Min(s => s.StartTime);
                DateTime end = SelectedTimeSlots.Max(s => s.StartTime).AddHours(1); // assuming 1-hour slots

                Booking newBooking = new Booking(
                        resourceId: SelectedResource.Id,
                        personId: personId,
                        startTime: start,
                        endTime: end,
                        requirementFulfilled: false,
                        isPaid: false
                    );

                int newId = _bookingRepository.Add(newBooking);

                MessageService.Show($"Booking #{newId} til {Name} oprettet!");
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

              // <-- IMPORTANT: populate TimeSlots

            //if (parameter == null)
            //{
            //    MessageService.Show("Parameter is null");
            //    return;
            //}

            //// Show the runtime type
            //MessageService.Show($"Parameter type: {parameter.GetType().FullName}");

            //// If it's an array, dump the contents
            //if (parameter is object[] arr)
            //{
            //    var values = string.Join(", ", arr.Select(v => v?.ToString() ?? "null"));
            //    MessageService.Show($"Array values: {values}");
            //}
            //else
            //{
            //    // Otherwise just show ToString()
            //    MessageService.Show($"Parameter value: {parameter}");
            //}

            //OPRINDELIG KODE:
            //var paramString = parameter as string;
            //if (string.IsNullOrEmpty(paramString)) { MessageService.Show("Null or empty"); return; }

            //var parts = paramString.Split('|');
            //var date = DateTime.Parse(parts[0]);
            //var resourceId = parts[1];

            //MessageService.Show($"Selected date: {date.ToShortDateString()}, Resource ID: {resourceId}");
        }

        //public ICommand SelectTimeAndResourceCommand(object parameter)
        //{
        //    var paramString = parameter as string;
        //    var parts = paramString.Split('|');
        //    var time = TimeSpan.Parse(parts[0]);
        //    var resourceId = int.Parse(parts[1]);

        //    // Handle the booking of this timeslot
        //    BookTimeSlot(resourceId, SelectedDate, time);
        //});


        private void PopulateTimeSlots()
        {
            var random = new Random();//kan slettes efter random test
            if (SelectedResource == null) 
                return;

            TimeSlots.Clear();
            var date = SelectedDate;

            for (int hour = 7; hour <= 22; hour++)
                TimeSlots.Add(new TimeSlot 
                {
                    ResourceId = SelectedResource.Id,
                    StartTime = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0),
                    //Laver random IsAvailable = falsk til test
                    IsAvailable = random.Next(0, 5) != 0 // 20% chance of being unavailable
                });
            
            //MessageService.Show("Populating TimeSlots...");
        }

        public void LoadAvailableSlots(int resourceId, DateTime date)
        {
            AvailableTimeSlots.Clear();

            foreach (var slot in TimeSlots)
            {
                slot.IsAvailable = slot.ResourceId == resourceId && slot.StartTime.Date == date.Date;
            }

            OnPropertyChanged(nameof(TimeSlots));

            //foreach (var ts in TimeSlots)
            //{
            //    Debug.WriteLine($"Resource: {ts.ResourceId}, StartTime: {ts.StartTime}, IsAvailable: {ts.IsAvailable}");
            //}

            //MessageService.Show("Loading available slots...");
        }


        public void ExecuteSelectTimeSlotCommand(object parameter)
        {
            if (parameter is not TimeSlot slot)
                return;

            if (SelectedTimeSlots.Contains(slot))
                SelectedTimeSlots.Remove(slot);
            else
                SelectedTimeSlots.Add(slot);

            OnPropertyChanged(nameof(SelectedBookingHeader));
            UpdateCalculatedPrice();


            //nedunder til enkel time selecttion
            //if (parameter is not TimeSlot slot || !slot.IsAvailable)
            //    return;

            //foreach (var s in TimeSlots)
            //{
            //    s.IsSelected = false;
            //}

            //slot.IsSelected = true;

            //SelectedTimeSlot = slot;
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
        //private void BookTimeSlot(DateTime date, TimeSpan startTime, int resourceId)
        //{
        //    var booking = new Booking
        //    {
        //        ResourceId = resourceId,
        //        Date = date,
        //        Start = startTime,
        //        End = startTime + TimeSpan.FromMinutes(30) // or your interval
        //    };

        //    Bookings.Add(booking);

        //    // Optionally update UI (mark timeslot as booked)
        //    MarkSlotAsBooked(startTime);
        //}

    }
}
