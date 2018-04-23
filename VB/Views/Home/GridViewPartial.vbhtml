@functions
    Protected Function GetChecked(ByVal container As GridViewGroupRowTemplateContainer) As Boolean
        For i As Integer = 0 To container.Grid.GetChildRowCount(container.VisibleIndex) - 1
            Dim isRowSelected As Boolean = container.Grid.Selection.IsRowSelectedByKey(container.Grid.GetChildDataRow(container.VisibleIndex, i)("ProductID"))
            If Not isRowSelected Then
                Return False
            End If
        Next i
        Return True
    End Function

    Protected Function GetCaptionText(ByVal container As GridViewGroupRowTemplateContainer) As String
        Dim captionText As String = If((Not String.IsNullOrEmpty(container.Column.Caption)), container.Column.Caption, container.Column.FieldName)
        Return String.Format("{0} : {1} {2}", captionText, container.GroupText, container.SummaryText)
    End Function
End Functions

@Html.DevExpress().GridView( _
    Sub(settings)
            settings.Name = "GridView"
            settings.KeyFieldName = "ProductID"
            settings.CallbackRouteValues = New With {.Controller = "Home", .Action = "GridViewPartial"}
            settings.CustomActionRouteValues = New With {.Controller = "Home", .Action = "GridViewCustomActionPartial"}

            settings.Settings.ShowGroupPanel = True
    
            settings.CommandColumn.Visible = True
            settings.CommandColumn.ShowSelectCheckbox = True

            settings.Columns.Add("ProductID")
            settings.Columns.Add("ProductName")
            settings.Columns.Add("UnitPrice")
            settings.Columns.Add("UnitsOnOrder").GroupIndex = 0

            settings.SetGroupRowContentTemplateContent( _
                Sub(c)
                        ViewContext.Writer.Write("<table><tr><td>")
                        Html.DevExpress().CheckBox( _
                            Sub(checkBox)
                                    checkBox.Name = "checkBox" + c.VisibleIndex.ToString()
                                    checkBox.Properties.ClientSideEvents.CheckedChanged = String.Format("function(s, e){{ {0}.PerformCallback({{parameters: '{1};' + s.GetChecked()}}); }}", settings.Name, c.VisibleIndex)
                                    checkBox.Checked = GetChecked(c)
                            End Sub).GetHtml()
                        ViewContext.Writer.Write("</td><td>")
                        Html.DevExpress().Label( _
                            Sub(label)
                                    label.Name = "label" + c.VisibleIndex.ToString()
                                    label.Text = GetCaptionText(c)
                            End Sub).GetHtml()
                        ViewContext.Writer.Write("</td></tr></table>")
                End Sub)

            settings.BeforeGetCallbackResult = _
                Sub(s, e)
                        Dim gridView As ASPxGridView = CType(s, ASPxGridView)

                        If ViewData("parameters") Is Nothing Then
                            Return
                        End If

                        Dim parameters() As String = ViewData("parameters").ToString().Split(";"c)
                        Dim index As Integer = Integer.Parse(parameters(0))
                        Dim isGroupRowSelected As Boolean = Boolean.Parse(parameters(1))
                        For i As Integer = 0 To gridView.GetChildRowCount(index) - 1
                            Dim row As System.Data.DataRow = gridView.GetChildDataRow(index, i)
                            gridView.Selection.SetSelectionByKey(row("ProductID"), isGroupRowSelected)
                        Next i
                End Sub

            'settings.ClientSideEvents.SelectionChanged = "function(s, e) { e.processOnServer = true; }"
    End Sub).Bind(Model).GetHtml()