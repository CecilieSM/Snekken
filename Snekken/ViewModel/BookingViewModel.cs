using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Models;
using Models.Repository;
using WPFLib.Services;
using WPFLib.Utility;
using WPFLib.ViewModel;

namespace Snekken.ViewModel;

public class BookingViewModel : BaseViewModel
{
    private readonly IRepository<Booking> _bookingRepository;
    private readonly IRepository<Person> _personRepository;
    private readonly IRepository<Resource> _resourceRepository;
    private readonly IRepository<ResourceType> _resourceTypeRepository;

    public ObservableCollection<Booking> Bookings;
    public ObservableCollection<Person> Persons;
    public ObservableCollection<Resource> Resources;
    public ObservableCollection<ResourceType> ResourceTypes;

    private Booking? _selectedBooking;
    public Booking? SelectedBooking
    {
        get => _selectedBooking ;
        set
        {
            if (_selectedBooking == value) return;
            _selectedBooking = value;
            if(_selectedBooking != null)
            {
                setFields(_selectedBooking);
            }
            OnPropertyChanged();
        }
    }
    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText == value) return;
            _searchText = value;
            OnPropertyChanged();
            // Optionally, implement search filtering logic here
        }
    }

    public ObservableCollection<Booking> FilteredBookings
    {
        get
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return Bookings;
            var filtered = Bookings.Where(b => 
                b.Id.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) // This does not search the correct fields, adjust as necessary
                                                                                         // Add other properties to search through as needed
            );
            return new ObservableCollection<Booking>(filtered);
        }
    }

    #region formfields
    // bruger fields
    private string _formName = "";
    public string FormName
    {
        get => _formName;
        set
        {
            if (_formName == value) return;
            _formName = value;
            OnPropertyChanged();
        }
    }
    private string _formEmail = "";
    public string FormEmail
    {
        get => _formEmail;
        set
        {
            if (_formEmail == value) return;
            _formEmail = value;
            OnPropertyChanged();
        }
    }
    private string _formPhone = "";
    public string FormPhone
    {
        get => _formPhone;
        set
        {
            if (_formPhone == value) return;
            _formPhone = value;
            OnPropertyChanged();
        }
    }

    // booking fields
    private DateTime _formStart;
    public DateTime FormStart
    {
        get => _formStart;
        set
        {
            if (_formStart == value) return;
            _formStart = value;
            OnPropertyChanged();
        }
    }
    private DateTime _formEnd;
    public DateTime FormEnd
    {
        get => _formEnd;
        set
        {
            if (_formEnd == value) return;
            _formEnd = value;
            OnPropertyChanged();
        }
    }

    private bool _formIsPaid;
    public bool FormIsPaid
    {
        get => _formIsPaid;
        set
        {
            if (_formIsPaid == value) return;
            _formIsPaid = value;
            OnPropertyChanged();
        }
    }
    private bool _formRequirementFulfilled;
    public bool FormRequirementFulfilled
    {
        get => _formRequirementFulfilled;
        set
        {
            if (_formRequirementFulfilled == value) return;
            _formRequirementFulfilled = value;
            OnPropertyChanged();
        }
    }
    
    private bool _isCheckedOut;
    public bool IsCheckedOut
    {
        get => _isCheckedOut;
        set
        {
            if (_isCheckedOut == value) return;
            _isCheckedOut = value;
            OnPropertyChanged();
        }
    }
    private bool _isReturned;
    public bool IsReturned
    {
        get => _isReturned;
        set
        {
            if (_isReturned == value) return;
            _isReturned = value;
            OnPropertyChanged();
        }
    }

    private string _resourceTitle = "";
    public string ResourceTitle
    {
        get => _resourceTitle;
        set
        {
            if (_resourceTitle == value) return;
            _resourceTitle = value;
            OnPropertyChanged();
        }
    }
    private string _requirements = "";
    public string Requirements
    {
        get => _requirements;
        set
        {
            if (_requirements == value) return;
            _requirements = value;
            OnPropertyChanged();
        }
    }
    private decimal _totalPrice;
    public decimal TotalPrice
    {
        get => _totalPrice;
        set
        {
            if (_totalPrice == value) return;
            _totalPrice = value;
            OnPropertyChanged();
        }
    }
    #endregion

    public ICommand AddBookingCommand { get; }
    public ICommand UpdateBookingCommand { get; }
    public ICommand DeleteBookingCommand { get; }

    public BookingViewModel(
        IRepository<Booking> BookingRepository, 
        IRepository<Person> PersonRepository, 
        IRepository<Resource> ResourceRepository, 
        IRepository<ResourceType> ResourceTypeRepository
        )
    {
        _bookingRepository = BookingRepository;
        Bookings = new ObservableCollection<Booking>(_bookingRepository.GetAll());
        _personRepository = PersonRepository;
        Persons = new ObservableCollection<Person>(_personRepository.GetAll());
        _resourceRepository = ResourceRepository;
        Resources = new ObservableCollection<Resource>(_resourceRepository.GetAll());
        _resourceTypeRepository = ResourceTypeRepository;
        ResourceTypes = new ObservableCollection<ResourceType>(_resourceTypeRepository.GetAll());

        // add relay commands
        AddBookingCommand = new RelayCommand(AddBooking);
        UpdateBookingCommand = new RelayCommand(UpdateBooking, CanUpdateBooking);
        DeleteBookingCommand = new RelayCommand(DeleteBooking, CanDeleteBooking);
    }

    #region Command methods

    private void AddBooking(object? parameter)
    {
        MessageService.Show("BookingViewModel linje 241 skal starte en kioskproces");
    }
    private void UpdateBooking(object? parameter) 
    {
        //tjek if finalized, then delete booking
        if (IsReturned && FormIsPaid)
        {
            DeleteBooking(SelectedBooking);
            clearFields();
            return;
        }

        // update selected booking and person from form fields
        if (SelectedBooking == null) return;
        Person person = Persons.FirstOrDefault(p => p.Id == SelectedBooking.PersonId)!;
        person.Name = FormName;
        person.Email = FormEmail;
        person.Phone = FormPhone;
        SelectedBooking.StartTime = FormStart;
        SelectedBooking.EndTime = FormEnd;
        SelectedBooking.IsPaid = FormIsPaid;
        SelectedBooking.RequirementFulfilled = FormRequirementFulfilled;
        SelectedBooking.HandedOutAt = IsCheckedOut ? DateTime.Now : null;
        SelectedBooking.ReturnedAt = IsReturned ? DateTime.Now : null;

        _bookingRepository.Update(SelectedBooking!);
        _personRepository.Update(person!);
    }

    public bool CanUpdateBooking()
    {
        if (SelectedBooking == null) return false;
        return true;
    }

    public void DeleteBooking(object? parameter)
    {
        if (SelectedBooking == null) return;
        _bookingRepository.Delete(SelectedBooking.Id);
        Bookings.Remove(SelectedBooking);
    }

    public bool CanDeleteBooking()
    {
        return SelectedBooking != null;
    }
    #endregion

    private void setFields(Booking b)
    {
        Person person = Persons.FirstOrDefault(p => p.Id == b.PersonId)!;
        FormName = person?.Name ?? string.Empty;
        FormEmail = person?.Email ?? string.Empty;
        FormPhone = person?.Phone ?? string.Empty;
        
        FormStart = b.StartTime;
        FormEnd = b.EndTime;
        var Resource = Resources.FirstOrDefault(r => r.Id == b.Id);
        ResourceTitle = Resource?.Title ?? string.Empty;
        Requirements = ResourceTypes.FirstOrDefault(rt => rt.Id == Resource?.ResourceTypeId)?.Requirement ?? string.Empty;
        TotalPrice = (decimal)(Resource?.Price ?? 0);

        FormIsPaid = b.IsPaid;
        FormRequirementFulfilled = b.RequirementFulfilled;
        IsCheckedOut = b.HandedOutAt.HasValue;
        IsReturned = b.ReturnedAt.HasValue;
    }

    private void clearFields()
    {
        _selectedBooking = null;
        FormName = string.Empty;
        FormEmail = string.Empty;
        FormPhone = string.Empty;

        FormStart = DateTime.Now;
        FormEnd = DateTime.Now;
        ResourceTitle = string.Empty;
        Requirements = string.Empty;
        TotalPrice = 0;

        FormIsPaid = false;
        FormRequirementFulfilled = false;
        IsCheckedOut = false;
        IsReturned = false;
    }
}
