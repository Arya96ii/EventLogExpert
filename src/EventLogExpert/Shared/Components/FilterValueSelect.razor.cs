using EventLogExpert.Eventing.Helpers;
using EventLogExpert.UI;
using Microsoft.AspNetCore.Components;

namespace EventLogExpert.Shared.Components;

public partial class FilterValueSelect
{
    private bool _isDropDownVisible;
    private List<string> _items = new();
    private UI.FilterType _type;

    [Parameter]
    public UI.FilterType Type
    {
        get => _type;
        set
        {
            _type = value;
            ResetItems();
        }
    }

    [Parameter]
    public string Value { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    private List<string> FilteredItems => _items.Where(x => x.ToLower().Contains(Value.ToLower())).ToList();

    private string IsDropDownVisible => _isDropDownVisible.ToString().ToLower();

    private async void OnInputChange(ChangeEventArgs args)
    {
        Value = args.Value?.ToString() ?? string.Empty;
        await ValueChanged.InvokeAsync(Value);
    }

    private void ResetItems()
    {
        switch (Type)
        {
            case FilterType.EventId :
                _items = EventLogState.Value.ActiveLogs.Values.SelectMany(log => log.EventIds)
                    .Distinct().OrderBy(id => id).Select(id => id.ToString()).ToList();
                break;
            case FilterType.Level :
                _items = new List<string>();

                foreach (SeverityLevel item in Enum.GetValues(typeof(SeverityLevel)))
                {
                    _items.Add(item.ToString());
                }

                break;
            case FilterType.Keywords :
                _items = EventLogState.Value.ActiveLogs.Values.SelectMany(log => log.KeywordNames)
                    .Distinct().OrderBy(name => name).Select(name => name.ToString()).ToList();

                break;
            case FilterType.Source :
                _items = EventLogState.Value.ActiveLogs.Values.SelectMany(log => log.EventProviderNames)
                    .Distinct().OrderBy(name => name).Select(name => name.ToString()).ToList();

                break;
            case FilterType.Task :
                _items = EventLogState.Value.ActiveLogs.Values.SelectMany(log => log.TaskNames)
                    .Distinct().OrderBy(name => name).Select(name => name.ToString()).ToList();

                break;
            case FilterType.Description :
            default :
                break;
        }
    }

    private void SetDropDownVisibility(bool visible) => _isDropDownVisible = visible;

    private async Task UpdateValue(string value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(Value);
    }
}
