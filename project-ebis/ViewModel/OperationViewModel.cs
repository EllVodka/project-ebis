using CommunityToolkit.Mvvm.ComponentModel;
using MySqlConnector;
using project_ebis.Model;
using project_ebis.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
