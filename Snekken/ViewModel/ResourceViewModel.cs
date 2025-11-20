using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration.Internal;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Models;
using Models.Repository;
using Snekken.Services;
using Snekken.Utility;
using Snekken.View;

namespace Snekken.ViewModel;

public class ResourceViewModel : BaseViewModel
{
    //DATABASEKODE
    //private string _connectionString;
    //private readonly ResourceRepository _resourceRepository;
    //private readonly ResourceTypeRepository _resourceTypeRepository;

    //new fields for resourceviewmodel refactor
    private readonly IResourceRepository _resourceRepository;
    private readonly IResourceTypeRepository _resourceTypeRepository;
    


    //PROPERTIES RESOURCE
    private string _resourceFormTitle;  
    public string ResourceFormTitle { get => _resourceFormTitle; set { _resourceFormTitle = value; OnPropertyChanged(); } }

    private ResourceType _resourceFormType;
    public ResourceType ResourceFormType { get => _resourceFormType; set { _resourceFormType = value; OnPropertyChanged(); } }

    private double _resourceFormUnitPrice;
    public double ResourceFormUnitPrice { get => _resourceFormUnitPrice; set { _resourceFormUnitPrice = value; OnPropertyChanged(); } }

    private string _resourceFormDescription;
    public string ResourceFormDescription { get => _resourceFormDescription; set { _resourceFormDescription = value; OnPropertyChanged(); } }
         
    private bool _isActive;
    public bool IsActive { get => _isActive; set { _isActive = value; OnPropertyChanged(); } }


    //PROPERTIES RESOURCETYPE
    private string _typeFormTitle;
    public string TypeFormTitle { get => _typeFormTitle; set { _typeFormTitle = value; OnPropertyChanged(); } }


    private TimeUnit _typeFormUnit; //_typeFormUnit = privat felt, der indeholder værdien.

    public TimeUnit TypeFormUnit //TypeFormUnit er den offentlige property (med get/set og OnPropertyChanged)
    {
        get => _typeFormUnit;
        set
        {
            _typeFormUnit = value;
            OnPropertyChanged(nameof(TypeFormUnit));
        }
    }

    public Array TimeUnitOptions => Enum.GetValues(typeof(TimeUnit));

    private string? _typeFormRequirement;
    public string? TypeFormRequirement { get => _typeFormRequirement; set { _typeFormRequirement = value; OnPropertyChanged(); } }



    ////PROPERTIES PERSON
    //private string _newName;
    //public string NewName { get => _newName; set { _newName = value; OnPropertyChanged(); } }


    //private int _newPhone;
    //public int NewPhone { get => _newPhone; set { _newPhone = value; OnPropertyChanged(); } }


    //private string _newEmail;
    //public string NewEmail { get => _newEmail; set { _newEmail = value; OnPropertyChanged(); } }

    ////PROPERTIES BOOKING
    //private DateTime _startDate;
    //public DateTime StartDate { get => _startDate; set {  _startDate = value; OnPropertyChanged(); } }

    //private DateTime _endDate; 
    //public DateTime EndDate { get => EndDate; set {  _endDate = value; OnPropertyChanged(); } } 


    //ObservableCollection Resource
    public ObservableCollection<Resource> Resources { get; set; }

    //ObservableCollection ResourceType
    public ObservableCollection<ResourceType> ResourceTypes { get; set; }



//Relaycommands Ressource
public ICommand AddResourceCommand { get; }
    public ICommand UpdateResourceCommand { get; }
    public ICommand DeleteResourceCommand { get; }
    public ICommand DeselectResourceCommand { get; }


    //Relaycommands ResourceType
    public ICommand AddResourceTypeCommand { get; }
    public ICommand UpdateResourceTypeCommand { get; }
    public ICommand DeleteResourceTypeCommand { get; }
    public ICommand DeselectResourceTypeCommand { get; }


    //CONSTRUCTOR

    public ResourceViewModel(IResourceRepository resourceRepository, IResourceTypeRepository resourceTypeRepository)
    {
        _resourceRepository = resourceRepository;
        _resourceTypeRepository = resourceTypeRepository;

        ////SKAL VI BRUGE DEM?
        //this._connectionString = ConfigHelper.GetConnectionString();
        //_resourceRepository = new ResourceRepository(this._connectionString);
        //_resourceTypeRepository = new ResourceTypeRepository(this._connectionString);

        // ObservableCollection til alle ressourcer (til ListView fx)
        //Resources = new ObservableCollection<Resource>(_resourceRepository.GetAll());

        // ObservableCollection til resource-typer (til ComboBox fx)

        try
        {
            ResourceTypes = new ObservableCollection<ResourceType>(_resourceTypeRepository.GetAll());
        }
        catch (Exception)
        {
            _messageService.Show("Der opstod en fejl ved hentning af ressource-typer?");
        }


        AddResourceCommand = new RelayCommand(AddResource, CanAddResource);
        //UpdateResourceCommand = new RelayCommand(UpdateResource, CanUpdateResource);
        //DeleteResourceCommand = new RelayCommand(DeleteResource, CanDeleteResouce);
        //DeselectResourceCommand = new RelayCommand(DeselectResource, CanDeselectResource);

        AddResourceTypeCommand = new RelayCommand(AddResourceType, CanAddResourceType);
        //UpdateResourceTypeCommand = new RelayCommand(UpdateResourceType, CanUpdateResourceType);
        //DeleteResourceTypeCommand = new RelayCommand(DeleteResourceType, CanDeleteResouceType);
        //DeselectResourceTypeCommand = new RelayCommand(DeselectResourceType, CanDeselectResourceType);

    }

    //METODER
    private void AddResource(object? parameter)
    {
        try
        {
            Resource newResource = new Resource(this.ResourceFormTitle, this.ResourceFormUnitPrice, this.ResourceFormType.Id, this.ResourceFormDescription, this.IsActive);
            int newId = _resourceRepository.Add(newResource);                
        }
        catch (Exception)
        {
            _messageService.Show("Der opstod en fejl ved oprettelse af ressource?");
        }
        //newResource.ResourceId = newId;
        //Resources.Add(newResource);
        //ClearResourceForm();
    }

    private bool CanAddResource()
    {
        // Hvis der allerede er valgt en resource, må vi ikke oprette ny
        //if (SelectedResource != null)
        //    return false;

        //// Tjek, at alle felter er gyldige
        //if (string.IsNullOrWhiteSpace(ResourceFormTitle))
        //    return false;

        //if (ResourceFormType == null) // enum eller objekt
        //    return false;

        // if (ResourceFormDescription 

        //if (ResourceFormUnitPrice < 0)
        //    return false;

        //// Hvis alle checks er OK
        return true;
    }

    private void AddResourceType(object? parameter)
    {
        try
        {
            ResourceType newResourceType = new ResourceType(this.TypeFormTitle, this.TypeFormUnit, this.TypeFormRequirement);
            int newId = _resourceTypeRepository.Add(newResourceType);
        }
        catch (Exception)
        {
            MessageBox.Show("Der opstod en fejl ved oprettelse af ressource-type?");
        }

        //newResourceType.ResourceTypeId = newId;
        //ResourceTypes.Add(newResourceType);
        //ClearResourceForm();
    }

    private bool CanAddResourceType()
    {
        //    // Hvis der allerede er valgt en resource, må vi ikke oprette ny
        //    if (SelectedResourceType != null)
        //        return false;

        //    // Tjek, at alle felter er gyldige
        //    if (string.IsNullOrWhiteSpace(TypeFormTitle))
        //        return false;

        //    if (TypeFormUnit == TimeUnit.None)
        //        return false;

        //    if (string.IsNullOrWhiteSpace(TypeFormRequirement))
        //        return false;

        //    // Hvis alle checks er OK
        return true;
    }


    private void ClearResourceForm()
    {
        ResourceFormTitle = string.Empty;
        //ResourceFormType = string.Empty;
        ResourceFormUnitPrice = 0;   
        IsActive = true; 
    }



    //private void UpdateResource(object? parameter)
    //{
        //   try 
        //{	        

        //}
        //catch (Exception)
        //{
        // MessageBox.Show("Der opstod en fejl ved opdatering af ressource?");
        //}
    //    if (SelectedResource == null)
    //        return;
    //    SelectedResource.Title = ResourceFormTitle;
    //    SelectedResource.Type = ResourceFormType;
    //    SelectedResource.Description = ResourceFormDescription;
    //    SelectedResource.UnitPrice = ResourceFormUnitPrice;
    //    SelectedResource.Capacity = ResourceFormCapacity;
    //    SelectedResource.IsActive = IsActive;

    //    _resourceRepository.Update(SelectedResource);

    //    ResourcesView.Refresh();
    //}

    //private bool CanUpdateResource(object? parameter)
    //{
    //    // Hvis der allerede er valgt en resource, må vi ikke oprette ny
    //    if (SelectedResource != null)

    //        return false;

    //        // Tjek, at alle felter er gyldige
    //        if (string.IsNullOrWhiteSpace(ResourceFormTitle))
    //        { return false; }

    //        if (string.IsNullOrWhiteSpace(ResourceFormType))
    //            return false;

    //        if (string.IsNullOrWhiteSpace(ResourceFormDescription))
    //            return false;

    //        if (ResourceFormUnitPrice < 0)
    //        { return false; }


    //        if (ResourceFormCapacity <= 0)
    //            return false;

    //        // Hvis alle checks er OK
    //        return true;

    //}




}
