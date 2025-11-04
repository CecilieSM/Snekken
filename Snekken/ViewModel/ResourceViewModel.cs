using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snekken.ViewModel;

public class ResourceViewModel : BaseViewModel
{
    //DATA

    //ResourceRepository
    //ResourceTypeRepository
    //ObservableCollection Resources
    //ObservableCollection Types

    //Datafelter Ressource
    private string _resourceFormTitle;
    public string ResourceFormTitle { get => _resourceFormTitle ; set { _resourceFormTitle = value; OnPropertyChanged(); } }


    private string resourceFormType;
    public string ResourceFormType { get => resourceFormType; set { resourceFormType = value; OnPropertyChanged(); } }


    private double _resourceFormUnitPrice;
    public double ResourceFormUnitPrice { get => _resourceFormUnitPrice; set { _resourceFormUnitPrice = value; OnPropertyChanged(); } }


    private int _resourceFormCapacity;
    public int ResourceFormCapacity { get => _resourceFormCapacity; set { _resourceFormCapacity = value; OnPropertyChanged(); } }


    private string _resourceFormDescription;
    public string ResourceFormDescription { get => _resourceFormDescription; set { _resourceFormDescription = value; OnPropertyChanged(); } }


    //Datafelter type
    private string _typeFormTitle;
    public string TypeFormTitle { get => _typeFormTitle; set { _typeFormTitle = value; OnPropertyChanged(); } }


    private TimeUnit _typeFormUnit; //TimeUnit???
    public TimeUnit TypeFormUnit { get => _typeFormUnit; set { _typeFormUnit = value; OnPropertyChanged(); } }


    private string _typeFormDemands;
    public string TypeFormDemands { get => _typeFormDemands; set { _typeFormDemands = value; OnPropertyChanged(); } }


    //Relaycommands Ressource
    //Relaycommands type

  

    //CONSTRUCTOR

    public ResourceViewModel() 
  
    {
       
    }

    //METODER
    //Execute og CanExecute 

}
