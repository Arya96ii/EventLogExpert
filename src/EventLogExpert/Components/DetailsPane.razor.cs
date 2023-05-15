﻿// // Copyright (c) Microsoft Corporation.
// // Licensed under the MIT License.

using EventLogExpert.Library.Models;
using EventLogExpert.Store.EventLog;
using System.Text;

namespace EventLogExpert.Components;

public partial class DetailsPane
{
    private bool _expandMenu = false;
    private bool _expandXml = false;

    private DisplayEventModel? Event { get; set; }

    protected override void OnInitialized()
    {
        SubscribeToAction<EventLogAction.SelectEvent>(UpdateDetails);
        base.OnInitialized();
    }

    private void CopyEvent()
    {
        StringBuilder stringToCopy = new();

        stringToCopy.AppendLine($"Log Name: {EventLogState.Value.ActiveLog.Name}");
        stringToCopy.AppendLine($"Source: {Event?.ProviderName}");
        stringToCopy.AppendLine($"Date: {Event?.TimeCreated?.ConvertTimeZone(SettingsState.Value.TimeZone)}");
        stringToCopy.AppendLine($"Event ID: {Event?.Id}");
        stringToCopy.AppendLine($"Task Category: {Event?.TaskDisplayName}");
        stringToCopy.AppendLine($"Level: {Event?.Level}");
        stringToCopy.AppendLine("Keywords:");
        stringToCopy.AppendLine("User:");  // TODO: Update after DisplayEventModel is updated
        stringToCopy.AppendLine($"Computer: {Event?.MachineName}");
        stringToCopy.AppendLine("Description:");
        stringToCopy.AppendLine(Event?.Description);
        stringToCopy.AppendLine("Event Xml:");
        stringToCopy.AppendLine(Event?.Xml);

        Clipboard.SetTextAsync(stringToCopy.ToString());
    }

    private void ToggleMenu() => _expandMenu = !_expandMenu;

    private void ToggleXml() => _expandXml = !_expandXml;

    private void UpdateDetails(EventLogAction.SelectEvent action)
    {
        Event = action.SelectedEvent;
        _expandMenu = true;
        _expandXml = false;
        StateHasChanged();
    }
}
