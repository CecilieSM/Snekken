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

        public ObservableCollection<Booking> Bookings { get; set; }

        private int _bookingId;
        public int BookingId { get; set; }

        private int _personId;
        public int PersonId { get; set; }

        private int _resourceId;
        public int ResourceId { get; set; }

        private string _name;
        public string Name { get; set; }

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

        public MainViewModel(IRepository<Booking> bookingRepository) 
        {
            _bookingRepository = bookingRepository;

            try
            {
                Bookings = new ObservableCollection<Booking>(_bookingRepository.GetAll());
            }
            catch (Exception)
            {
                MessageService.Show("Der opstod en fejl ved hentning af bookinger?");
            }

            AddBooking = new RelayCommand(ExecuteAddBooking, CanAddBooking);
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

    }
}
