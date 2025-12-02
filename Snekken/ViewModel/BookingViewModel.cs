using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Models;
using Models.Repository;
using WPFLib.Utility;
using WPFLib.ViewModel;

namespace Snekken.ViewModel;

public class BookingViewModel : BaseViewModel
{
    // Properties for booking details
    
    private readonly IRepository<Booking> _bookingRepository;
    public ObservableCollection<Booking> Bookings;
    private Booking _selectedBooking;
    public Booking SelectedBooking
    {
        get => _selectedBooking;
        set
        {
            if (_selectedBooking == value) return;
            _selectedBooking = value;
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


    #region formfields
    // bruger fields
    private string _formName;
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
    private string _formEmail;
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
    private string _formPhone;
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
    private DateTime _formStartDate;
    public DateTime FormStartDate
    {
        get => _formStartDate;
        set
        {
            if (_formStartDate == value) return;
            _formStartDate = value;
            OnPropertyChanged();
        }
    }
    private DateTime _formEndDate;
    public DateTime FormEndDate
    {
        get => _formEndDate;
        set
        {
            if (_formEndDate == value) return;
            _formEndDate = value;
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

    #endregion

    public ICommand UpdateBookingCommand { get; }
    public ICommand DeleteBookingCommand { get; }
    public BookingViewModel(IRepository<Booking> BookingRepository)
    {
        _bookingRepository = BookingRepository;
        Bookings = new ObservableCollection<Booking>(_bookingRepository.GetAll());
        // need bookings with users and types included


        // add relay commands
        UpdateBookingCommand = new RelayCommand(UpdateBooking, CanUpdateBooking);
        DeleteBookingCommand = new RelayCommand(DeleteBooking, CanDeleteBooking);
    }

    #region Command methods
    private void UpdateBooking(object? parameter) 
    {   
        _bookingRepository.Update(SelectedBooking); // Vil helt sikkert ikke virke endnu
    }

    public bool CanUpdateBooking()
    {
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
}
