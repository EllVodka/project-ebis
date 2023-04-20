using CommunityToolkit.Mvvm.ComponentModel;
using project_ebis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_ebis.ViewModel;

[QueryProperty("Operation", "Operation")]
public partial class OperationViewModel : BaseViewModel 
{
    [ObservableProperty]
    Operation operation;
}
