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
using Snekken.View;
using WPFLib.Services;
using WPFLib.Utility;
using WPFLib.ViewModel;

namespace Snekken.ViewModel;

public class ResourceViewModel : BaseViewModel
{
    //DATABASEKODE
    private readonly IRepository<Resource> _resourceRepository;
    private readonly IRepository<ResourceType> _resourceTypeRepository;

  

    //PROPERTIES RESOURCE FORM
    private string _resourceFormTitle;  
    public string ResourceFormTitle { get => _resourceFormTitle; set { _resourceFormTitle = value; OnPropertyChanged(); } }

    private ResourceType _resourceFormType;
    public ResourceType ResourceFormType { get => _resourceFormType; set { _resourceFormType = value; OnPropertyChanged(); } }

    private double _resourceFormUnitPrice = 0;
    public double ResourceFormUnitPrice { get => _resourceFormUnitPrice; set { _resourceFormUnitPrice = value; OnPropertyChanged(); } }

    private string _resourceFormDescription;
    public string ResourceFormDescription { get => _resourceFormDescription; set { _resourceFormDescription = value; OnPropertyChanged(); } }     

    private Resource? _selectedResource;
    public Resource? SelectedResource
    {
        get => _selectedResource;
        set
        {
            if (_selectedResource == value) return;
            _selectedResource = value;
            OnPropertyChanged(); 
        }
    }

    //PROPERTIES RESOURCE-TYPE FORM
    private string _typeFormTitle;
    public string TypeFormTitle { get => _typeFormTitle; set { _typeFormTitle = value; OnPropertyChanged(); } }

    private TimeUnit _typeFormUnit; //_typeFormUnit = privat felt, der indeholder værdien.

    public TimeUnit TypeFormUnit //TypeFormUnit er den offentlige property (med get/set og OnPropertyChanged)
    {
        get => _typeFormUnit;
        set
        {
            _typeFormUnit = value;
            OnPropertyChanged();
        }
    }

    public Array TimeUnitOptions => Enum.GetValues(typeof(TimeUnit));

    private string? _typeFormRequirement;
    public string? TypeFormRequirement { get => _typeFormRequirement; set { _typeFormRequirement = value; OnPropertyChanged(); } }

    private ResourceType? _selectedResourceType;
    public ResourceType? SelectedResourceType
    {
        get => _selectedResourceType;
        set
        {
            if (_selectedResourceType == value) return;
            _selectedResourceType = value;
            OnPropertyChanged();
        }
    }
  
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
    public ResourceViewModel(IRepository<Resource> resourceRepository, IRepository<ResourceType> resourceTypeRepository)
    {
        _resourceRepository = resourceRepository;
        _resourceTypeRepository = resourceTypeRepository;
        
        try
        {
            ResourceTypes = new ObservableCollection<ResourceType>(_resourceTypeRepository.GetAll());
        }
        catch (Exception)
        {
            MessageService.Show("Der opstod en fejl ved hentning af ressource-typer?");
        }

        AddResourceCommand = new RelayCommand(AddResource, CanAddResource);

        AddResourceTypeCommand = new RelayCommand(AddResourceType, CanAddResourceType);
    }


    //METODER
    private void AddResource(object? parameter)
    {
        try
        {
            Resource newResource = new Resource(this.ResourceFormTitle, this.ResourceFormUnitPrice, this.ResourceFormType.Id, this.ResourceFormDescription);
            int newId = _resourceRepository.Add(newResource);
            ClearResourceForm();
            CloseWindowRequested("AddResource");
            //RequestClose?.Invoke(this, "AddResource");
        }
        catch (Exception)
        {
            MessageService.Show("Der opstod en fejl ved oprettelse af ressource? Måske findes der allerede en ressource med samme navn.");
        }
        //newResource.ResourceId = newId;
        //Resources.Add(newResource);
        
    }

    private bool CanAddResource()
    {
        // Tjek, at alle felter er gyldige
        if (string.IsNullOrWhiteSpace(ResourceFormTitle))
            return false;

        if (ResourceFormType == null) // enum eller objekt
            return false;


        //if (ResourceFormUnitPrice < 0)
        //    return false;

        // Hvis alle checks er OK
        return true;
    }

    private void AddResourceType(object? parameter)
    {
        try
        {
            ResourceType newResourceType = new ResourceType(this.TypeFormTitle, this.TypeFormUnit, this.TypeFormRequirement);
            int newId = _resourceTypeRepository.Add(newResourceType); // Insert to DB and recive newId
            ClearResourceTypeForm();
            newResourceType.Id = newId;
            this.ResourceTypes.Add(newResourceType);
            CloseWindowRequested("AddResourceType");
            //this.RequestClose?.Invoke(this, "AddResourceType");
        }
        catch (Exception)
        {
            MessageBox.Show("Der opstod en fejl ved oprettelse af ressource-type? Måske findes der allerede en ressource-type med samme navn.");
        }    
    }

    private bool CanAddResourceType()
    {
        // Tjek, at alle felter er gyldige
        if (string.IsNullOrWhiteSpace(TypeFormTitle))
            return false;

        //    if (TypeFormUnit == TimeUnit.None)
        //        return false;


        //    // Hvis alle checks er OK
        return true;
    }

    private void ClearResourceForm()
    {
        // Ryd Resource-form
        ResourceFormTitle = string.Empty;
        ResourceFormType = null;
        ResourceFormUnitPrice = 0;
        ResourceFormDescription = string.Empty;
        SelectedResource = null;

    }

    private void ClearResourceTypeForm()
    {
        // Ryd ResourceType-form
        TypeFormTitle = string.Empty;
        TypeFormUnit = TimeUnit.None;
        TypeFormRequirement = string.Empty;
        SelectedResourceType = null;
    }
}
