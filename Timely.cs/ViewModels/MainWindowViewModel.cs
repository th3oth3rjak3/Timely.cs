using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Timely.cs.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] 
    private bool _isPaneOpen;

    [ObservableProperty] 
    private int _selectedIndex;

    [ObservableProperty] 
    private ViewModelBase _currentPage = new HomePageViewModel();
    
    partial void OnSelectedIndexChanged(int value)
    {
        var instance = Activator.CreateInstance(ListItems[value].ModelType);
        if (instance is null) return;
        CurrentPage = (ViewModelBase)instance;
    }

    [RelayCommand]
    private void TogglePane() => IsPaneOpen = !IsPaneOpen;
    
    public ObservableCollection<ListItemTemplate> ListItems { get; } = [
        new(typeof(HomePageViewModel), "HomeRegular"),
        new(typeof(ButtonsPageViewModel), "CursorHoverRegular"),
    ];
}

public class ListItemTemplate
{
    public Type ModelType { get; }
    public string Label { get; }
    public StreamGeometry Icon { get; }

    public ListItemTemplate(Type type, string icon)
    {
        ModelType = type;
        Label = type.Name.Replace("PageViewModel", "");
        var resource = Application.Current!.FindResource(icon);
        Icon = (StreamGeometry)resource!;
    }
}