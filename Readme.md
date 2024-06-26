<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128550402/22.1.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T362032)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# Grid View for ASP.NET MVC - How to select/deselect all rows in a group when data is grouped by one column

This example demonstrates how to allow users to add/remove all rows in a group to/from selection. Note that this technique works only when grid data is grouped by one column.

## Overview

Call the grid's [SetGroupRowContentTemplateContent](https://docs.devexpress.com/AspNetMvc/DevExpress.Web.Mvc.GridViewSettings.SetGroupRowContentTemplateContent.overloads) method and place a check box and a label to the group row content template. Implement two-way data binding between the label's `Text` property and group row markup.

```cshtml
settings.SetGroupRowContentTemplateContent(c => {
    ViewContext.Writer.Write("<table><tr><td>");
    Html.DevExpress().CheckBox(checkBox => {
        checkBox.Name = "checkBox" + c.VisibleIndex.ToString();
        <!-- ... -->
    }).GetHtml();
    ViewContext.Writer.Write("</td><td>");
    Html.DevExpress().Label(label => {
        label.Name = "label" + c.VisibleIndex.ToString();
        label.Text = GetCaptionText(c);
    }).GetHtml();
    ViewContext.Writer.Write("</td></tr></table>");
});
```

```cs
protected string GetCaptionText(GridViewGroupRowTemplateContainer container) {
    string captionText = !string.IsNullOrEmpty(container.Column.Caption) ? container.Column.Caption : container.Column.FieldName;
    return string.Format("{0} : {1} {2}", captionText, container.GroupText, container.SummaryText);
}
```

Set the check box's [CheckedChanged](https://docs.devexpress.com/AspNet/DevExpress.Web.CheckEditClientSideEvents.CheckedChanged) event to a function that sends the visible index of the group row and the check state of the check box as callback parameters to the server.

```cshtml
Html.DevExpress().CheckBox(checkBox => {
    checkBox.Name = "checkBox" + c.VisibleIndex.ToString();
    checkBox.Properties.ClientSideEvents.CheckedChanged = string.Format("function(s, e){{ {0}.PerformCallback({{parameters: '{1};' + s.GetCheckState()}}); }}", settings.Name, c.VisibleIndex);
    checkBox.Init += (s, e) => {
        var checkEditor = (MVCxCheckBox)s;

        var checkState = GetCheckState(c);
        checkEditor.AllowGrayed = true;

        checkEditor.CheckState = GetCheckState(c);
    };
}).GetHtml();
```

```cs
protected CheckState GetCheckState(GridViewGroupRowTemplateContainer container) {
    var grid = (MVCxGridView)container.Grid;
    var groupRowKey = GetGroupRowKey(grid, container.VisibleIndex);
    var groupKeysCache = GetGroupKeysCache(grid);
    var dataRowsKeys = groupKeysCache[groupRowKey];
    var selectedCount = dataRowsKeys.Count(key => grid.Selection.IsRowSelectedByKey(key));
    if (selectedCount == 0)
        return CheckState.Unchecked;
    if (dataRowsKeys.Count == selectedCount)
        return CheckState.Checked;
    return CheckState.Indeterminate;
}
```

Handle the grid's [BeforeGetCallbackResult](https://docs.devexpress.com/AspNetMvc/DevExpress.Web.Mvc.GridSettingsBase.BeforeGetCallbackResult) event to process the callback. Pass the data row's key and the check state of the checkbox to the [SetSelectionByKey](https://docs.devexpress.com/AspNet/DevExpress.Web.Data.WebDataSelection.SetSelectionByKey(System.Object-System.Boolean)) method to select or deselect this row.

```cshtml
settings.BeforeGetCallbackResult += (sender, e) => {
    MVCxGridView gridView = (MVCxGridView)sender;
    if (Session["Grouped"] != null && Session["Grouped"].ToString() == "clear") {
        if (ViewBag.ClearGrouping == true) {
            foreach (var columns in gridView.DataColumns) {
                columns.UnGroup();
            }
            Session["Grouped"] = "";
        }
        return;
    }
    if (ViewData["data"] != null) {
        string[] parameters = ViewData["data"].ToString().Split(';');
        int visibleIndex = int.Parse(parameters[0]);
        bool isGroupRowSelected = false;
        CheckState currentState = (CheckState)Enum.Parse(typeof(CheckState), parameters[1]);
        switch(currentState) {
            case CheckState.Indeterminate:
            case CheckState.Checked:
                isGroupRowSelected = true;
                break;
            case CheckState.Unchecked:
                isGroupRowSelected = false;
                break;
        }
        var groupRowKey = GetGroupRowKey(gridView, visibleIndex);
        var groupKeysCache = GetGroupKeysCache(gridView);
        var dataRowsKeys = groupKeysCache[groupRowKey];
        foreach (var dataRowKey in dataRowsKeys) {
            gridView.Selection.SetSelectionByKey(dataRowKey, isGroupRowSelected);
        }
    }
    if(ViewBag.GroupedColumns == null)
        return;
    string[] columnNames = ViewBag.GroupedColumns.Split(';');
    gridView.ClearSort();
    foreach(string name in columnNames) {
        gridView.GroupBy(gridView.Columns[name]);
    }
    Session["Grouped"] = "";
    gridView.CollapseAll();
};
```

## Files to Review

* [HomeController.cs](./CS/Controllers/HomeController.cs)
* [_GridViewPartial.cshtml](./CS/Views/Home/_GridViewPartial.cshtml)
* [Index.cshtml](./CS/Views/Home/Index.cshtml)

## Documentation

* [Group Data](https://docs.devexpress.com/AspNet/3715/components/grid-view/concepts/group-data)
* [Callbacks](https://docs.devexpress.com/AspNet/402559/common-concepts/callbacks)

## More Examples

* [Grid View for ASP.NET Web Forms - How to select/deselect all rows in a group when data is grouped by one column](https://github.com/DevExpress-Examples/asp-net-web-forms-gridview-select-deselect-all-rows-in-a-group)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-mvc-grid-select-deselect-all-rows-in-a-group&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-mvc-grid-select-deselect-all-rows-in-a-group&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
