Option Explicit

'============================================================
' CONSTANTES GLOBALES
'============================================================
Const TITLE_FONT As String = "Calibri"
Const HEADER_COLOR_CREDIT As Long = &H794F1F   ' Azul corporativo (RGB(31, 78, 121))
Const HEADER_COLOR_DEBIT As Long = &H6B7300     ' Verde corporativo (RGB(0, 115, 107))
Const HEADER_COLOR_WALLET As Long = &H8B475D    ' Púrpura corporativo (RGB(93, 71, 139))
Const SUMMARY_SHEET As String = "Resumen"

'============================================================
' PROCEDIMIENTO PRINCIPAL
'============================================================
Sub SepararPorTipoYProveedorFinal_Pro()
    Dim startTime As Double
    startTime = Timer
    
    ' Configuración inicial para optimización
    With Application
        .ScreenUpdating = False
        .Calculation = xlCalculationManual
        .EnableEvents = False
    End With
    
    On Error GoTo ErrorHandler
    
    ' 1. DECLARACIÓN DE VARIABLES
    Dim ws As Worksheet, creditSheet As Worksheet, debitSheet As Worksheet, walletSheet As Worksheet, summarySheet As Worksheet
    Dim lastRow As Long, i As Long, lastCol As Long
    Dim data As Variant, headers As Variant
    Dim creditData As Object, debitData As Object, walletData As Object
    Dim creditOutput() As Variant, debitOutput() As Variant, walletOutput() As Variant
    Dim creditCount As Long, debitCount As Long, walletCount As Long
    Dim summaryData As Object
    
    ' Configuración de columnas y encabezados
    Dim config As Object
    Set config = GetConfigurationSettings()
    
    ' 2. INICIALIZACIÓN
    Set creditData = CreateObject("Scripting.Dictionary")
    Set debitData = CreateObject("Scripting.Dictionary")
    Set walletData = CreateObject("Scripting.Dictionary")
    Set summaryData = CreateObject("Scripting.Dictionary")
    
    Set ws = ThisWorkbook.sheets("Datos")
    lastRow = GetLastRow(ws)
    lastCol = GetLastColumn(ws)
    
    ' 3. LEER Y CLASIFICAR DATOS
    data = ws.Range("A5:K" & lastRow).value ' Incluye todas las columnas relevantes
    
    ' 4. CLASIFICAR LOS DATOS POR TIPO DE PAGO Y RECOLECTAR DATOS PARA RESUMEN
    ClassifyData data, creditData, debitData, walletData, creditCount, debitCount, walletCount, summaryData
    
    ' 5. PREPARAR HOJAS DE DESTINO
    PrepareStyledSheet "Tarjetas de Crédito", creditSheet, "CRÉDITO"
    PrepareStyledSheet "Tarjetas de Débito", debitSheet, "DÉBITO"
    PrepareStyledSheet "Monederos Virtuales", walletSheet, "MONEDEROS"
    PrepareSummarySheet SUMMARY_SHEET, summarySheet
    
    ' 6. OBTENER Y ESCRIBIR ENCABEZADOS PERSONALIZADOS
    headers = GetCustomHeaders(config("nombresColumnas"))
    
    WriteHeaders creditSheet, headers, config("numColumnas"), HEADER_COLOR_CREDIT
    WriteHeaders debitSheet, headers, config("numColumnas"), HEADER_COLOR_DEBIT
    WriteHeaders walletSheet, headers, config("numColumnas"), HEADER_COLOR_WALLET
    
    ' 7. PROCESAR Y MOSTRAR DATOS
    ProcessAndDisplayData creditData, ws, config("columnasACopiar"), creditOutput, creditSheet, creditCount, config("numColumnas"), RGB(234, 244, 255)
    ProcessAndDisplayData debitData, ws, config("columnasACopiar"), debitOutput, debitSheet, debitCount, config("numColumnas"), RGB(230, 245, 241)
    ProcessAndDisplayData walletData, ws, config("columnasACopiar"), walletOutput, walletSheet, walletCount, config("numColumnas"), RGB(242, 240, 247)
    
    ' 8. GENERAR HOJA DE RESUMEN Y GRÁFICOS
    GenerateSummaryReport summarySheet, summaryData, creditCount, debitCount, walletCount
    
    ' 9. FINALIZAR FORMATO Y CONFIGURACIÓN
    FinalizeSheets config("numColumnas"), creditSheet, debitSheet, walletSheet, summarySheet
    
    ' 10. MOSTRAR RESULTADO
    summarySheet.Activate
    MsgBox "Reportes generados en " & Round(Timer - startTime, 2) & " segundos", vbInformation, "Proceso Completado"
    
CleanUp:
    ' Restaurar configuración de Excel
    With Application
        .ScreenUpdating = True
        .Calculation = xlCalculationAutomatic
        .EnableEvents = True
    End With
    Exit Sub
    
ErrorHandler:
    DisplayError
    Resume CleanUp
End Sub

'============================================================
' FUNCIONES Y PROCEDIMIENTOS AUXILIARES
'============================================================

Private Function GetConfigurationSettings() As Object
    ' Configuración centralizada para fácil mantenimiento
    Dim config As Object
    Set config = CreateObject("Scripting.Dictionary")
    
    ' Columnas a conservar en el orden deseado
    config.Add "columnasACopiar", Array("H", "I", "G", "F")
    
    ' Nombres personalizados para los encabezados
    config.Add "nombresColumnas", Array("FECHA", "SECUENCIA", "CLIENTE", "IMPORTE (S/)")
    
    ' Número de columnas
    config.Add "numColumnas", UBound(config("columnasACopiar")) - LBound(config("columnasACopiar")) + 1
    
    Set GetConfigurationSettings = config
End Function

Private Function GetLastRow(ws As Worksheet) As Long
    GetLastRow = ws.Cells(ws.Rows.Count, "A").End(xlUp).row
End Function

Private Function GetLastColumn(ws As Worksheet) As Long
    GetLastColumn = ws.Cells(4, ws.Columns.Count).End(xlToLeft).Column
End Function

Private Sub ClassifyData(data As Variant, ByRef creditData As Object, ByRef debitData As Object, _
                        ByRef walletData As Object, ByRef creditCount As Long, _
                        ByRef debitCount As Long, ByRef walletCount As Long, ByRef summaryData As Object)
    ' Clasifica los datos por tipo de pago y recolecta datos para resumen
    Dim i As Long
    Dim tipoPago As String, proveedor As String, fecha As String, importe As Double
    
    For i = 1 To UBound(data, 1)
        tipoPago = UCase(Trim(data(i, 2)))
        proveedor = UCase(Trim(data(i, 3)))
        fecha = data(i, 8)
        importe = data(i, 6)
        
        Select Case tipoPago
            Case "TARJETA DE CREDITO"
                AddToDictionary creditData, proveedor, i
                creditCount = creditCount + 1
                AddToSummary summaryData, "CREDITO", proveedor, fecha, importe
                
            Case "TARJETA DE DEBITO"
                AddToDictionary debitData, proveedor, i
                debitCount = debitCount + 1
                AddToSummary summaryData, "DEBITO", proveedor, fecha, importe
                
            Case Else
                AddToDictionary walletData, tipoPago, i
                walletCount = walletCount + 1
                AddToSummary summaryData, "MONEDERO", tipoPago, fecha, importe
        End Select
    Next i
End Sub

Private Sub AddToDictionary(dict As Object, key As String, value As Long)
    ' Agrega un valor a un diccionario, creando la colección si no existe
    If Not dict.Exists(key) Then dict.Add key, New Collection
    dict(key).Add value
End Sub

Private Sub AddToSummary(summaryData As Object, tipo As String, proveedor As String, fecha As String, importe As Double)
    ' Agrega datos para el resumen
    Dim key As String, monthKey As String
    monthKey = Format(CDate(fecha), "YYYY-MM")
    
    ' Totales por tipo
    key = "TOTAL_" & tipo
    If Not summaryData.Exists(key) Then summaryData.Add key, 0
    summaryData(key) = summaryData(key) + importe
    
    ' Totales por proveedor
    key = "PROV_" & tipo & "_" & proveedor
    If Not summaryData.Exists(key) Then summaryData.Add key, 0
    summaryData(key) = summaryData(key) + importe
    
    ' Totales por mes
    key = "MES_" & monthKey
    If Not summaryData.Exists(key) Then summaryData.Add key, 0
    summaryData(key) = summaryData(key) + importe
End Sub

Private Sub ProcessAndDisplayData(dataDict As Object, sourceWs As Worksheet, columnsToCopy As Variant, _
                                ByRef outputArray As Variant, targetSheet As Worksheet, _
                                dataCount As Long, numColumns As Long, rowColor As Long)
    ' Procesa y muestra los datos en la hoja destino
    If dataCount > 0 Then
        ReDim outputArray(1 To dataCount + dataDict.Count, 1 To numColumns)
        ProcessData_Optimo dataDict, sourceWs, columnsToCopy, outputArray
        targetSheet.Range("A4").Resize(UBound(outputArray, 1), numColumns).value = outputArray
        FormatDataRows targetSheet, UBound(outputArray, 1), numColumns, rowColor
        ApplyAccountingFormat targetSheet, "D"
    End If
End Sub

Private Sub FinalizeSheets(numColumns As Long, ParamArray sheets() As Variant)
    ' Aplica formato final a todas las hojas
    Dim sheet As Variant
    Dim ws As Worksheet
    
    For Each sheet In sheets
        If Not IsEmpty(sheet) Then
            If TypeName(sheet) = "Worksheet" Then
                Set ws = sheet
                FinalizeSheetFormat ws, numColumns
                ConfigurarImpresionA4 ws
            End If
        End If
    Next sheet
End Sub

Private Sub DisplayError()
    ' Muestra mensajes de error estandarizados
    MsgBox "Error " & Err.Number & ": " & Err.Description & vbCrLf & _
           "Ocurrió en la línea: " & Erl, vbCritical, "Error"
End Sub

'============================================================
' FUNCIONES DE FORMATO Y CONFIGURACIÓN
'============================================================

Sub ApplyAccountingFormat(ws As Worksheet, colLetter As String)
    ' Aplica formato contable peruano a la columna especificada
    Dim lastRow As Long
    Dim rng As Range
    Dim cell As Range
    
    lastRow = ws.Cells(ws.Rows.Count, colLetter).End(xlUp).row
    
    If lastRow >= 4 Then
        Set rng = ws.Range(colLetter & "4:" & colLetter & lastRow)
        
        With rng
            .NumberFormat = "_([$S/] * #,##0.00_);_([$S/] * (#,##0.00);_([$S/] * ""-""??_);_(@_)"
            .HorizontalAlignment = xlRight
        End With
        
        For Each cell In rng
            If Left(ws.Cells(cell.row, "A").value, 3) = ">> " Then
                cell.NumberFormat = "General"
                cell.HorizontalAlignment = xlLeft
            End If
        Next cell
    End If
End Sub

Sub ConfigurarImpresionA4(ws As Worksheet)
    ' Configura la hoja para impresión en formato A4
    With ws.PageSetup
        .PaperSize = xlPaperA4
        .Orientation = xlPortrait
        .FitToPagesWide = 1
        .FitToPagesTall = False
        .Zoom = False
        .LeftMargin = Application.InchesToPoints(0.25)
        .RightMargin = Application.InchesToPoints(0.25)
        .TopMargin = Application.InchesToPoints(0.75)
        .BottomMargin = Application.InchesToPoints(0.75)
        .HeaderMargin = Application.InchesToPoints(0.3)
        .FooterMargin = Application.InchesToPoints(0.3)
        .CenterFooter = "&P de &N"
        .DifferentFirstPageHeaderFooter = False
    End With
End Sub

'============================================================
' FUNCIONES DE MANEJO DE DATOS
'============================================================

Function GetCustomHeaders(nombresColumnas As Variant) As Variant
    ' Crea encabezados personalizados
    Dim headers() As Variant
    Dim i As Long
    
    ReDim headers(1 To 1, 1 To UBound(nombresColumnas) - LBound(nombresColumnas) + 1)
    
    For i = LBound(nombresColumnas) To UBound(nombresColumnas)
        headers(1, i - LBound(nombresColumnas) + 1) = nombresColumnas(i)
    Next i
    
    GetCustomHeaders = headers
End Function

Sub ProcessData_Optimo(dataDict As Object, ws As Worksheet, columnasACopiar As Variant, ByRef outputArray As Variant)
    ' Procesa datos de manera óptima copiando solo columnas necesarias
    Dim rowIndex As Long: rowIndex = 1
    Dim key As Variant, item As Variant, i As Long, colDest As Long
    Dim colIndex As Long
    
    For Each key In dataDict.Keys
        outputArray(rowIndex, 1) = ">> " & key
        rowIndex = rowIndex + 1
        
        For Each item In dataDict(key)
            colDest = 1
            For i = LBound(columnasACopiar) To UBound(columnasACopiar)
                colIndex = ws.Range(columnasACopiar(i) & "1").Column
                outputArray(rowIndex, colDest) = ws.Cells(4 + item, colIndex).value
                colDest = colDest + 1
            Next i
            rowIndex = rowIndex + 1
        Next item
    Next key
End Sub

'============================================================
' PROCEDIMIENTOS DE FORMATO DE HOJAS
'============================================================

Sub PrepareStyledSheet(sheetName As String, ByRef targetSheet As Worksheet, reportType As String)
    ' Prepara una hoja con formato profesional
    On Error Resume Next
    Set targetSheet = ThisWorkbook.sheets(sheetName)
    If targetSheet Is Nothing Then
        Set targetSheet = ThisWorkbook.sheets.Add(After:=ThisWorkbook.sheets(ThisWorkbook.sheets.Count))
        targetSheet.Name = sheetName
    Else
        targetSheet.Cells.Clear
    End If
    On Error GoTo 0
    
    With targetSheet.Range("A1:D1")
        .Merge
        .value = "REPORTE DE " & UCase(reportType)
        With .Font
            .Name = TITLE_FONT
            .Bold = True
            .Size = 16
            .Color = RGB(30, 30, 30)
        End With
        With .Interior
            .Color = RGB(245, 245, 245)
            .Pattern = xlSolid
        End With
        .HorizontalAlignment = xlCenter
        .VerticalAlignment = xlCenter
        .RowHeight = 40
        With .Borders(xlEdgeBottom)
            .LineStyle = xlContinuous
            .Weight = xlThick
            .Color = RGB(100, 100, 100)
        End With
    End With
    
    targetSheet.Range("A2:D2").RowHeight = 10
End Sub

Sub WriteHeaders(targetSheet As Worksheet, headers As Variant, colCount As Long, headerColor As Long)
    ' Escribe y formatea los encabezados
    Dim originalSheet As Worksheet
    Set originalSheet = ActiveSheet
    
    Application.ScreenUpdating = True
    
    With targetSheet
        .Range("A3").Resize(1, colCount).value = headers
        
        With .Range("A3").Resize(1, colCount)
            .Font.Name = TITLE_FONT
            .Font.Bold = True
            .Font.Size = 12
            .Font.Color = vbWhite
            .Interior.Color = headerColor
            .HorizontalAlignment = xlCenter
            .VerticalAlignment = xlCenter
            .RowHeight = 25
            With .Borders
                .LineStyle = xlContinuous
                .Color = RGB(180, 180, 180)
            End With
        End With
        
        .Parent.Activate
        .Activate
        .Range("A4").Select
        ActiveWindow.FreezePanes = True
        originalSheet.Activate
    End With
    
    Application.ScreenUpdating = False
End Sub

Sub FormatDataRows(targetSheet As Worksheet, dataRows As Long, colCount As Long, rowColor As Long)
    ' Formatea las filas de datos con colores alternados
    Dim dataRange As Range
    Dim i As Long
    Dim categoryColor As Long
    
    Select Case targetSheet.Name
        Case "Tarjetas de Crédito": categoryColor = RGB(200, 220, 255)
        Case "Tarjetas de Débito": categoryColor = RGB(190, 235, 225)
        Case "Monederos Virtuales": categoryColor = RGB(215, 205, 235)
        Case Else: categoryColor = RGB(220, 220, 220)
    End Select
    
    With targetSheet
        Set dataRange = .Range("A4").Resize(dataRows, colCount)
        
        With dataRange
            .Font.Name = TITLE_FONT
            .Font.Size = 11
            With .Borders
                .LineStyle = xlContinuous
                .Color = RGB(200, 200, 200)
            End With
            .HorizontalAlignment = xlLeft
            .VerticalAlignment = xlCenter
        End With
        
        For i = 1 To dataRows Step 2
            With .Range(.Cells(4 + i - 1, 1), .Cells(4 + i - 1, colCount))
                If Not Left(.Cells(1, 1).value, 3) = ">> " Then
                    .Interior.Color = rowColor
                End If
            End With
        Next i
        
        FormatCategoryRows targetSheet, dataRows, colCount, categoryColor
    End With
End Sub

Private Sub FormatCategoryRows(ws As Worksheet, dataRows As Long, colCount As Long, categoryColor As Long)
    ' Formatea específicamente las filas de categoría
    Dim cell As Range
    
    For Each cell In ws.Range("A4:A" & (4 + dataRows - 1))
        If Left(cell.value, 3) = ">> " Then
            With ws.Range(cell, cell.Offset(0, colCount - 1))
                With .Font
                    .Bold = True
                    .Size = 12
                    .Color = RGB(20, 20, 20)
                    .Name = TITLE_FONT
                End With
                .Interior.Color = categoryColor
                With .Borders(xlEdgeTop)
                    .LineStyle = xlContinuous
                    .Weight = xlMedium
                    .Color = RGB(120, 120, 120)
                End With
                With .Borders(xlEdgeBottom)
                    .LineStyle = xlDouble
                    .Weight = xlThick
                    .Color = RGB(80, 80, 80)
                End With
                cell.value = "   " & cell.value
            End With
        End If
    Next cell
End Sub

Sub FinalizeSheetFormat(targetSheet As Worksheet, colCount As Long)
    ' Aplica formato final a la hoja
    Dim i As Long
    
    With targetSheet
        .Columns("A").ColumnWidth = 35
        For i = 2 To colCount
            .Columns(i).AutoFit
            If .Columns(i).ColumnWidth > 20 Then .Columns(i).ColumnWidth = 20
        Next i
        
        With .Cells(4 + .UsedRange.Rows.Count - 1, 1).Resize(1, colCount)
            With .Borders(xlEdgeBottom)
                .LineStyle = xlDouble
                .Weight = xlThick
                .Color = RGB(120, 120, 120)
            End With
        End With
    End With
End Sub

'============================================================
' HOJA DE RESUMEN Y GRÁFICOS
'============================================================

Sub PrepareSummarySheet(sheetName As String, ByRef summarySheet As Worksheet)
    ' Prepara la hoja de resumen
    On Error Resume Next
    Set summarySheet = ThisWorkbook.sheets(sheetName)
    If summarySheet Is Nothing Then
        Set summarySheet = ThisWorkbook.sheets.Add(Before:=ThisWorkbook.sheets(1))
        summarySheet.Name = sheetName
    Else
        summarySheet.Cells.Clear
    End If
    On Error GoTo 0
    
    With summarySheet.Range("A1:F1")
        .Merge
        .value = "RESUMEN DE TRANSACCIONES"
        With .Font
            .Name = TITLE_FONT
            .Bold = True
            .Size = 18
            .Color = RGB(30, 30, 30)
        End With
        .Interior.Color = RGB(245, 245, 245)
        .HorizontalAlignment = xlCenter
        .VerticalAlignment = xlCenter
        .RowHeight = 45
        With .Borders(xlEdgeBottom)
            .LineStyle = xlContinuous
            .Weight = xlThick
            .Color = RGB(100, 100, 100)
        End With
    End With
End Sub

Sub GenerateSummaryReport(summarySheet As Worksheet, summaryData As Object, creditCount As Long, debitCount As Long, walletCount As Long)
    ' Genera el reporte de resumen con estadísticas y gráficos
    Dim key As Variant
    Dim row As Long: row = 3
    Dim total As Double
    
    With summarySheet
        ' Encabezados de resumen
        .Range("A" & row).value = "CATEGORÍA"
        .Range("B" & row).value = "TOTAL (S/)"
        .Range("C" & row).value = "TRANSACCIONES"
        With .Range("A" & row & ":C" & row)
            .Font.Name = TITLE_FONT
            .Font.Bold = True
            .Font.Size = 12
            .Interior.Color = RGB(31, 78, 121)
            .Font.Color = vbWhite
            .HorizontalAlignment = xlCenter
        End With
        row = row + 1
        
        ' Totales por tipo
        total = 0
        If summaryData.Exists("TOTAL_CREDITO") Then
            .Range("A" & row).value = "Tarjetas de Crédito"
            .Range("B" & row).value = summaryData("TOTAL_CREDITO")
            .Range("C" & row).value = creditCount
            total = total + summaryData("TOTAL_CREDITO")
            row = row + 1
        End If
        If summaryData.Exists("TOTAL_DEBITO") Then
            .Range("A" & row).value = "Tarjetas de Débito"
            .Range("B" & row).value = summaryData("TOTAL_DEBITO")
            .Range("C" & row).value = debitCount
            total = total + summaryData("TOTAL_DEBITO")
            row = row + 1
        End If
        If summaryData.Exists("TOTAL_MONEDERO") Then
            .Range("A" & row).value = "Monederos Virtuales"
            .Range("B" & row).value = summaryData("TOTAL_MONEDERO")
            .Range("C" & row).value = walletCount
            total = total + summaryData("TOTAL_MONEDERO")
            row = row + 1
        End If
        
        ' Total general
        .Range("A" & row).value = "TOTAL GENERAL"
        .Range("B" & row).value = total
        .Range("C" & row).value = creditCount + debitCount + walletCount
        With .Range("A" & row & ":C" & row)
            .Font.Bold = True
            .Interior.Color = RGB(200, 200, 200)
        End With
        row = row + 2
        
        ' Formato de totales
        .Range("B4:B" & row).NumberFormat = "_([$S/] * #,##0.00_);_([$S/] * (#,##0.00);_([$S/] * ""-""??_);_(@_)"
        .Columns("A:C").AutoFit
        
        ' Resumen por proveedor
        .Range("A" & row).value = "RESUMEN POR PROVEEDOR"
        With .Range("A" & row & ":B" & row)
            .Font.Bold = True
            .Font.Size = 14
            .Interior.Color = RGB(31, 78, 121)
            .Font.Color = vbWhite
        End With
        row = row + 1
        .Range("A" & row).value = "PROVEEDOR"
        .Range("B" & row).value = "TOTAL (S/)"
        With .Range("A" & row & ":B" & row)
            .Font.Bold = True
            .Interior.Color = RGB(50, 50, 50)
            .Font.Color = vbWhite
        End With
        row = row + 1
        
        For Each key In summaryData.Keys
            If InStr(key, "PROV_") > 0 Then
                .Range("A" & row).value = Mid(key, InStr(key, "_") + 1)
                .Range("B" & row).value = summaryData(key)
                row = row + 1
            End If
        Next key
        .Range("B" & (row - UBound(summaryData.Keys) + 1) & ":B" & row).NumberFormat = "_([$S/] * #,##0.00_);_([$S/] * (#,##0.00);_([$S/] * ""-""??_);_(@_)"
        
        ' Resumen mensual
        row = row + 2
        .Range("A" & row).value = "RESUMEN MENSUAL"
        With .Range("A" & row & ":B" & row)
            .Font.Bold = True
            .Font.Size = 14
            .Interior.Color = RGB(31, 78, 121)
            .Font.Color = vbWhite
        End With
        row = row + 1
        .Range("A" & row).value = "MES"
        .Range("B" & row).value = "TOTAL (S/)"
        With .Range("A" & row & ":B" & row)
            .Font.Bold = True
            .Interior.Color = RGB(50, 50, 50)
            .Font.Color = vbWhite
        End With
        row = row + 1
        
        For Each key In summaryData.Keys
            If InStr(key, "MES_") > 0 Then
                .Range("A" & row).value = Format(CDate(Mid(key, 5)), "mmmm yyyy")
                .Range("B" & row).value = summaryData(key)
                row = row + 1
            End If
        Next key
        .Range("B" & (row - UBound(summaryData.Keys) + 1) & ":B" & row).NumberFormat = "_([$S/] * #,##0.00_);_([$S/] * (#,##0.00);_([$S/] * ""-""??_);_(@_)"
        
        ' Crear gráficos
        CreateSummaryCharts summarySheet
    End With
End Sub

Sub CreateSummaryCharts(summarySheet As Worksheet)
    ' Crea gráficos para el resumen
    Dim chartObj As ChartObject
    Dim lastRow As Long
    
    With summarySheet
        lastRow = .Cells(.Rows.Count, "A").End(xlUp).row
        
        ' Gráfico de barras para totales por tipo
        Set chartObj = .ChartObjects.Add(Left:=300, Top:=20, Width:=400, Height:=250)
        With chartObj.Chart
            .SetSourceData Source:=summarySheet.Range("A4:B6")
            .ChartType = xlColumnClustered
            .HasTitle = True
            .ChartTitle.Text = "Distribución por Tipo de Pago"
            .ChartTitle.Font.Size = 14
            .ChartTitle.Font.Name = TITLE_FONT
            .Axes(xlValue).HasTitle = True
            .Axes(xlValue).AxisTitle.Text = "Total (S/)"
            .Axes(xlCategory).HasTitle = True
            .Axes(xlCategory).AxisTitle.Text = "Categoría"
            .Legend.Position = xlBottom
            .ApplyDataLabels
        End With
        
        ' Gráfico circular para distribución por proveedor
        Set chartObj = .ChartObjects.Add(Left:=300, Top:=300, Width:=400, Height:=250)
        With chartObj.Chart
            .SetSourceData Source:=summarySheet.Range("A9:B" & summarySheet.Cells(summarySheet.Rows.Count, "A").End(xlUp).row)
            .ChartType = xlPie
            .HasTitle = True
            .ChartTitle.Text = "Distribución por Proveedor"
            .ChartTitle.Font.Size = 14
            .ChartTitle.Font.Name = TITLE_FONT
            .Legend.Position = xlRight
            ' Corrección: Aplicar etiquetas de datos con tipo explícito
            .ApplyDataLabels Type:=xlDataLabelsShowLabelAndPercent
            With .SeriesCollection(1).DataLabels
                .ShowPercentage = True
                .ShowValue = False
                .Font.Size = 10
                .Font.Name = TITLE_FONT
            End With
        End With
    End With
End Sub
Option Explicit

'============================================================
' CONSTANTES GLOBALES
'============================================================
Const TITLE_FONT As String = "Calibri"
Const HEADER_COLOR_CREDIT As Long = &H794F1F   ' Azul corporativo (RGB(31, 78, 121))
Const HEADER_COLOR_DEBIT As Long = &H6B7300     ' Verde corporativo (RGB(0, 115, 107))
Const HEADER_COLOR_WALLET As Long = &H8B475D    ' Púrpura corporativo (RGB(93, 71, 139))
Const SUMMARY_SHEET As String = "Resumen"

'============================================================
' PROCEDIMIENTO PRINCIPAL
'============================================================
Sub SepararPorTipoYProveedorFinal_Pro()
    Dim startTime As Double
    startTime = Timer
    
    ' Configuración inicial para optimización
    With Application
        .ScreenUpdating = False
        .Calculation = xlCalculationManual
        .EnableEvents = False
    End With
    
    On Error GoTo ErrorHandler
    
    ' 1. DECLARACIÓN DE VARIABLES
    Dim ws As Worksheet, creditSheet As Worksheet, debitSheet As Worksheet, walletSheet As Worksheet, summarySheet As Worksheet
    Dim lastRow As Long, i As Long, lastCol As Long
    Dim data As Variant, headers As Variant
    Dim creditData As Object, debitData As Object, walletData As Object
    Dim creditOutput() As Variant, debitOutput() As Variant, walletOutput() As Variant
    Dim creditCount As Long, debitCount As Long, walletCount As Long
    Dim summaryData As Object
    
    ' Configuración de columnas y encabezados
    Dim config As Object
    Set config = GetConfigurationSettings()
    
    ' 2. INICIALIZACIÓN
    Set creditData = CreateObject("Scripting.Dictionary")
    Set debitData = CreateObject("Scripting.Dictionary")
    Set walletData = CreateObject("Scripting.Dictionary")
    Set summaryData = CreateObject("Scripting.Dictionary")
    
    Set ws = ThisWorkbook.sheets("Datos")
    lastRow = GetLastRow(ws)
    lastCol = GetLastColumn(ws)
    
    ' 3. LEER Y CLASIFICAR DATOS
    data = ws.Range("A5:K" & lastRow).value ' Incluye todas las columnas relevantes
    
    ' 4. CLASIFICAR LOS DATOS POR TIPO DE PAGO Y RECOLECTAR DATOS PARA RESUMEN
    ClassifyData data, creditData, debitData, walletData, creditCount, debitCount, walletCount, summaryData
    
    ' 5. PREPARAR HOJAS DE DESTINO
    PrepareStyledSheet "Tarjetas de Crédito", creditSheet, "CRÉDITO"
    PrepareStyledSheet "Tarjetas de Débito", debitSheet, "DÉBITO"
    PrepareStyledSheet "Monederos Virtuales", walletSheet, "MONEDEROS"
    PrepareSummarySheet SUMMARY_SHEET, summarySheet
    
    ' 6. OBTENER Y ESCRIBIR ENCABEZADOS PERSONALIZADOS
    headers = GetCustomHeaders(config("nombresColumnas"))
    
    WriteHeaders creditSheet, headers, config("numColumnas"), HEADER_COLOR_CREDIT
    WriteHeaders debitSheet, headers, config("numColumnas"), HEADER_COLOR_DEBIT
    WriteHeaders walletSheet, headers, config("numColumnas"), HEADER_COLOR_WALLET
    
    ' 7. PROCESAR Y MOSTRAR DATOS
    ProcessAndDisplayData creditData, ws, config("columnasACopiar"), creditOutput, creditSheet, creditCount, config("numColumnas"), RGB(234, 244, 255)
    ProcessAndDisplayData debitData, ws, config("columnasACopiar"), debitOutput, debitSheet, debitCount, config("numColumnas"), RGB(230, 245, 241)
    ProcessAndDisplayData walletData, ws, config("columnasACopiar"), walletOutput, walletSheet, walletCount, config("numColumnas"), RGB(242, 240, 247)
    
    ' 8. GENERAR HOJA DE RESUMEN Y GRÁFICOS
    GenerateSummaryReport summarySheet, summaryData, creditCount, debitCount, walletCount
    
    ' 9. FINALIZAR FORMATO Y CONFIGURACIÓN
    FinalizeSheets config("numColumnas"), creditSheet, debitSheet, walletSheet, summarySheet
    
    ' 10. MOSTRAR RESULTADO
    summarySheet.Activate
    MsgBox "Reportes generados en " & Round(Timer - startTime, 2) & " segundos", vbInformation, "Proceso Completado"
    
CleanUp:
    ' Restaurar configuración de Excel
    With Application
        .ScreenUpdating = True
        .Calculation = xlCalculationAutomatic
        .EnableEvents = True
    End With
    Exit Sub
    
ErrorHandler:
    DisplayError
    Resume CleanUp
End Sub

'============================================================
' FUNCIONES Y PROCEDIMIENTOS AUXILIARES
'============================================================

Private Function GetConfigurationSettings() As Object
    ' Configuración centralizada para fácil mantenimiento
    Dim config As Object
    Set config = CreateObject("Scripting.Dictionary")
    
    ' Columnas a conservar en el orden deseado
    config.Add "columnasACopiar", Array("H", "I", "G", "F")
    
    ' Nombres personalizados para los encabezados
    config.Add "nombresColumnas", Array("FECHA", "SECUENCIA", "CLIENTE", "IMPORTE (S/)")
    
    ' Número de columnas
    config.Add "numColumnas", UBound(config("columnasACopiar")) - LBound(config("columnasACopiar")) + 1
    
    Set GetConfigurationSettings = config
End Function

Private Function GetLastRow(ws As Worksheet) As Long
    GetLastRow = ws.Cells(ws.Rows.Count, "A").End(xlUp).row
End Function

Private Function GetLastColumn(ws As Worksheet) As Long
    GetLastColumn = ws.Cells(4, ws.Columns.Count).End(xlToLeft).Column
End Function

Private Sub ClassifyData(data As Variant, ByRef creditData As Object, ByRef debitData As Object, _
                        ByRef walletData As Object, ByRef creditCount As Long, _
                        ByRef debitCount As Long, ByRef walletCount As Long, ByRef summaryData As Object)
    ' Clasifica los datos por tipo de pago y recolecta datos para resumen
    Dim i As Long
    Dim tipoPago As String, proveedor As String, fecha As String, importe As Double
    
    For i = 1 To UBound(data, 1)
        tipoPago = UCase(Trim(data(i, 2)))
        proveedor = UCase(Trim(data(i, 3)))
        fecha = data(i, 8)
        importe = data(i, 6)
        
        Select Case tipoPago
            Case "TARJETA DE CREDITO"
                AddToDictionary creditData, proveedor, i
                creditCount = creditCount + 1
                AddToSummary summaryData, "CREDITO", proveedor, fecha, importe
                
            Case "TARJETA DE DEBITO"
                AddToDictionary debitData, proveedor, i
                debitCount = debitCount + 1
                AddToSummary summaryData, "DEBITO", proveedor, fecha, importe
                
            Case Else
                AddToDictionary walletData, tipoPago, i
                walletCount = walletCount + 1
                AddToSummary summaryData, "MONEDERO", tipoPago, fecha, importe
        End Select
    Next i
End Sub

Private Sub AddToDictionary(dict As Object, key As String, value As Long)
    ' Agrega un valor a un diccionario, creando la colección si no existe
    If Not dict.Exists(key) Then dict.Add key, New Collection
    dict(key).Add value
End Sub

Private Sub AddToSummary(summaryData As Object, tipo As String, proveedor As String, fecha As String, importe As Double)
    ' Agrega datos para el resumen
    Dim key As String, monthKey As String
    monthKey = Format(CDate(fecha), "YYYY-MM")
    
    ' Totales por tipo
    key = "TOTAL_" & tipo
    If Not summaryData.Exists(key) Then summaryData.Add key, 0
    summaryData(key) = summaryData(key) + importe
    
    ' Totales por proveedor
    key = "PROV_" & tipo & "_" & proveedor
    If Not summaryData.Exists(key) Then summaryData.Add key, 0
    summaryData(key) = summaryData(key) + importe
    
    ' Totales por mes
    key = "MES_" & monthKey
    If Not summaryData.Exists(key) Then summaryData.Add key, 0
    summaryData(key) = summaryData(key) + importe
End Sub

Private Sub ProcessAndDisplayData(dataDict As Object, sourceWs As Worksheet, columnsToCopy As Variant, _
                                ByRef outputArray As Variant, targetSheet As Worksheet, _
                                dataCount As Long, numColumns As Long, rowColor As Long)
    ' Procesa y muestra los datos en la hoja destino
    If dataCount > 0 Then
        ReDim outputArray(1 To dataCount + dataDict.Count, 1 To numColumns)
        ProcessData_Optimo dataDict, sourceWs, columnsToCopy, outputArray
        targetSheet.Range("A4").Resize(UBound(outputArray, 1), numColumns).value = outputArray
        FormatDataRows targetSheet, UBound(outputArray, 1), numColumns, rowColor
        ApplyAccountingFormat targetSheet, "D"
    End If
End Sub

Private Sub FinalizeSheets(numColumns As Long, ParamArray sheets() As Variant)
    ' Aplica formato final a todas las hojas
    Dim sheet As Variant
    Dim ws As Worksheet
    
    For Each sheet In sheets
        If Not IsEmpty(sheet) Then
            If TypeName(sheet) = "Worksheet" Then
                Set ws = sheet
                FinalizeSheetFormat ws, numColumns
                ConfigurarImpresionA4 ws
            End If
        End If
    Next sheet
End Sub

Private Sub DisplayError()
    ' Muestra mensajes de error estandarizados
    MsgBox "Error " & Err.Number & ": " & Err.Description & vbCrLf & _
           "Ocurrió en la línea: " & Erl, vbCritical, "Error"
End Sub

'============================================================
' FUNCIONES DE FORMATO Y CONFIGURACIÓN
'============================================================

Sub ApplyAccountingFormat(ws As Worksheet, colLetter As String)
    ' Aplica formato contable peruano a la columna especificada
    Dim lastRow As Long
    Dim rng As Range
    Dim cell As Range
    
    lastRow = ws.Cells(ws.Rows.Count, colLetter).End(xlUp).row
    
    If lastRow >= 4 Then
        Set rng = ws.Range(colLetter & "4:" & colLetter & lastRow)
        
        With rng
            .NumberFormat = "_([$S/] * #,##0.00_);_([$S/] * (#,##0.00);_([$S/] * ""-""??_);_(@_)"
            .HorizontalAlignment = xlRight
        End With
        
        For Each cell In rng
            If Left(ws.Cells(cell.row, "A").value, 3) = ">> " Then
                cell.NumberFormat = "General"
                cell.HorizontalAlignment = xlLeft
            End If
        Next cell
    End If
End Sub

Sub ConfigurarImpresionA4(ws As Worksheet)
    ' Configura la hoja para impresión en formato A4
    With ws.PageSetup
        .PaperSize = xlPaperA4
        .Orientation = xlPortrait
        .FitToPagesWide = 1
        .FitToPagesTall = False
        .Zoom = False
        .LeftMargin = Application.InchesToPoints(0.25)
        .RightMargin = Application.InchesToPoints(0.25)
        .TopMargin = Application.InchesToPoints(0.75)
        .BottomMargin = Application.InchesToPoints(0.75)
        .HeaderMargin = Application.InchesToPoints(0.3)
        .FooterMargin = Application.InchesToPoints(0.3)
        .CenterFooter = "&P de &N"
        .DifferentFirstPageHeaderFooter = False
    End With
End Sub

'============================================================
' FUNCIONES DE MANEJO DE DATOS
'============================================================

Function GetCustomHeaders(nombresColumnas As Variant) As Variant
    ' Crea encabezados personalizados
    Dim headers() As Variant
    Dim i As Long
    
    ReDim headers(1 To 1, 1 To UBound(nombresColumnas) - LBound(nombresColumnas) + 1)
    
    For i = LBound(nombresColumnas) To UBound(nombresColumnas)
        headers(1, i - LBound(nombresColumnas) + 1) = nombresColumnas(i)
    Next i
    
    GetCustomHeaders = headers
End Function

Sub ProcessData_Optimo(dataDict As Object, ws As Worksheet, columnasACopiar As Variant, ByRef outputArray As Variant)
    ' Procesa datos de manera óptima copiando solo columnas necesarias
    Dim rowIndex As Long: rowIndex = 1
    Dim key As Variant, item As Variant, i As Long, colDest As Long
    Dim colIndex As Long
    
    For Each key In dataDict.Keys
        outputArray(rowIndex, 1) = ">> " & key
        rowIndex = rowIndex + 1
        
        For Each item In dataDict(key)
            colDest = 1
            For i = LBound(columnasACopiar) To UBound(columnasACopiar)
                colIndex = ws.Range(columnasACopiar(i) & "1").Column
                outputArray(rowIndex, colDest) = ws.Cells(4 + item, colIndex).value
                colDest = colDest + 1
            Next i
            rowIndex = rowIndex + 1
        Next item
    Next key
End Sub

'============================================================
' PROCEDIMIENTOS DE FORMATO DE HOJAS
'============================================================

Sub PrepareStyledSheet(sheetName As String, ByRef targetSheet As Worksheet, reportType As String)
    ' Prepara una hoja con formato profesional
    On Error Resume Next
    Set targetSheet = ThisWorkbook.sheets(sheetName)
    If targetSheet Is Nothing Then
        Set targetSheet = ThisWorkbook.sheets.Add(After:=ThisWorkbook.sheets(ThisWorkbook.sheets.Count))
        targetSheet.Name = sheetName
    Else
        targetSheet.Cells.Clear
    End If
    On Error GoTo 0
    
    With targetSheet.Range("A1:D1")
        .Merge
        .value = "REPORTE DE " & UCase(reportType)
        With .Font
            .Name = TITLE_FONT
            .Bold = True
            .Size = 16
            .Color = RGB(30, 30, 30)
        End With
        With .Interior
            .Color = RGB(245, 245, 245)
            .Pattern = xlSolid
        End With
        .HorizontalAlignment = xlCenter
        .VerticalAlignment = xlCenter
        .RowHeight = 40
        With .Borders(xlEdgeBottom)
            .LineStyle = xlContinuous
            .Weight = xlThick
            .Color = RGB(100, 100, 100)
        End With
    End With
    
    targetSheet.Range("A2:D2").RowHeight = 10
End Sub

Sub WriteHeaders(targetSheet As Worksheet, headers As Variant, colCount As Long, headerColor As Long)
    ' Escribe y formatea los encabezados
    Dim originalSheet As Worksheet
    Set originalSheet = ActiveSheet
    
    Application.ScreenUpdating = True
    
    With targetSheet
        .Range("A3").Resize(1, colCount).value = headers
        
        With .Range("A3").Resize(1, colCount)
            .Font.Name = TITLE_FONT
            .Font.Bold = True
            .Font.Size = 12
            .Font.Color = vbWhite
            .Interior.Color = headerColor
            .HorizontalAlignment = xlCenter
            .VerticalAlignment = xlCenter
            .RowHeight = 25
            With .Borders
                .LineStyle = xlContinuous
                .Color = RGB(180, 180, 180)
            End With
        End With
        
        .Parent.Activate
        .Activate
        .Range("A4").Select
        ActiveWindow.FreezePanes = True
        originalSheet.Activate
    End With
    
    Application.ScreenUpdating = False
End Sub

Sub FormatDataRows(targetSheet As Worksheet, dataRows As Long, colCount As Long, rowColor As Long)
    ' Formatea las filas de datos con colores alternados
    Dim dataRange As Range
    Dim i As Long
    Dim categoryColor As Long
    
    Select Case targetSheet.Name
        Case "Tarjetas de Crédito": categoryColor = RGB(200, 220, 255)
        Case "Tarjetas de Débito": categoryColor = RGB(190, 235, 225)
        Case "Monederos Virtuales": categoryColor = RGB(215, 205, 235)
        Case Else: categoryColor = RGB(220, 220, 220)
    End Select
    
    With targetSheet
        Set dataRange = .Range("A4").Resize(dataRows, colCount)
        
        With dataRange
            .Font.Name = TITLE_FONT
            .Font.Size = 11
            With .Borders
                .LineStyle = xlContinuous
                .Color = RGB(200, 200, 200)
            End With
            .HorizontalAlignment = xlLeft
            .VerticalAlignment = xlCenter
        End With
        
        For i = 1 To dataRows Step 2
            With .Range(.Cells(4 + i - 1, 1), .Cells(4 + i - 1, colCount))
                If Not Left(.Cells(1, 1).value, 3) = ">> " Then
                    .Interior.Color = rowColor
                End If
            End With
        Next i
        
        FormatCategoryRows targetSheet, dataRows, colCount, categoryColor
    End With
End Sub

Private Sub FormatCategoryRows(ws As Worksheet, dataRows As Long, colCount As Long, categoryColor As Long)
    ' Formatea específicamente las filas de categoría
    Dim cell As Range
    
    For Each cell In ws.Range("A4:A" & (4 + dataRows - 1))
        If Left(cell.value, 3) = ">> " Then
            With ws.Range(cell, cell.Offset(0, colCount - 1))
                With .Font
                    .Bold = True
                    .Size = 12
                    .Color = RGB(20, 20, 20)
                    .Name = TITLE_FONT
                End With
                .Interior.Color = categoryColor
                With .Borders(xlEdgeTop)
                    .LineStyle = xlContinuous
                    .Weight = xlMedium
                    .Color = RGB(120, 120, 120)
                End With
                With .Borders(xlEdgeBottom)
                    .LineStyle = xlDouble
                    .Weight = xlThick
                    .Color = RGB(80, 80, 80)
                End With
                cell.value = "   " & cell.value
            End With
        End If
    Next cell
End Sub

Sub FinalizeSheetFormat(targetSheet As Worksheet, colCount As Long)
    ' Aplica formato final a la hoja
    Dim i As Long
    
    With targetSheet
        .Columns("A").ColumnWidth = 35
        For i = 2 To colCount
            .Columns(i).AutoFit
            If .Columns(i).ColumnWidth > 20 Then .Columns(i).ColumnWidth = 20
        Next i
        
        With .Cells(4 + .UsedRange.Rows.Count - 1, 1).Resize(1, colCount)
            With .Borders(xlEdgeBottom)
                .LineStyle = xlDouble
                .Weight = xlThick
                .Color = RGB(120, 120, 120)
            End With
        End With
    End With
End Sub

'============================================================
' HOJA DE RESUMEN Y GRÁFICOS
'============================================================

Sub PrepareSummarySheet(sheetName As String, ByRef summarySheet As Worksheet)
    ' Prepara la hoja de resumen
    On Error Resume Next
    Set summarySheet = ThisWorkbook.sheets(sheetName)
    If summarySheet Is Nothing Then
        Set summarySheet = ThisWorkbook.sheets.Add(Before:=ThisWorkbook.sheets(1))
        summarySheet.Name = sheetName
    Else
        summarySheet.Cells.Clear
    End If
    On Error GoTo 0
    
    With summarySheet.Range("A1:F1")
        .Merge
        .value = "RESUMEN DE TRANSACCIONES"
        With .Font
            .Name = TITLE_FONT
            .Bold = True
            .Size = 18
            .Color = RGB(30, 30, 30)
        End With
        .Interior.Color = RGB(245, 245, 245)
        .HorizontalAlignment = xlCenter
        .VerticalAlignment = xlCenter
        .RowHeight = 45
        With .Borders(xlEdgeBottom)
            .LineStyle = xlContinuous
            .Weight = xlThick
            .Color = RGB(100, 100, 100)
        End With
    End With
End Sub

Sub GenerateSummaryReport(summarySheet As Worksheet, summaryData As Object, creditCount As Long, debitCount As Long, walletCount As Long)
    ' Genera el reporte de resumen con estadísticas y gráficos
    Dim key As Variant
    Dim row As Long: row = 3
    Dim total As Double
    
    With summarySheet
        ' Encabezados de resumen
        .Range("A" & row).value = "CATEGORÍA"
        .Range("B" & row).value = "TOTAL (S/)"
        .Range("C" & row).value = "TRANSACCIONES"
        With .Range("A" & row & ":C" & row)
            .Font.Name = TITLE_FONT
            .Font.Bold = True
            .Font.Size = 12
            .Interior.Color = RGB(31, 78, 121)
            .Font.Color = vbWhite
            .HorizontalAlignment = xlCenter
        End With
        row = row + 1
        
        ' Totales por tipo
        total = 0
        If summaryData.Exists("TOTAL_CREDITO") Then
            .Range("A" & row).value = "Tarjetas de Crédito"
            .Range("B" & row).value = summaryData("TOTAL_CREDITO")
            .Range("C" & row).value = creditCount
            total = total + summaryData("TOTAL_CREDITO")
            row = row + 1
        End If
        If summaryData.Exists("TOTAL_DEBITO") Then
            .Range("A" & row).value = "Tarjetas de Débito"
            .Range("B" & row).value = summaryData("TOTAL_DEBITO")
            .Range("C" & row).value = debitCount
            total = total + summaryData("TOTAL_DEBITO")
            row = row + 1
        End If
        If summaryData.Exists("TOTAL_MONEDERO") Then
            .Range("A" & row).value = "Monederos Virtuales"
            .Range("B" & row).value = summaryData("TOTAL_MONEDERO")
            .Range("C" & row).value = walletCount
            total = total + summaryData("TOTAL_MONEDERO")
            row = row + 1
        End If
        
        ' Total general
        .Range("A" & row).value = "TOTAL GENERAL"
        .Range("B" & row).value = total
        .Range("C" & row).value = creditCount + debitCount + walletCount
        With .Range("A" & row & ":C" & row)
            .Font.Bold = True
            .Interior.Color = RGB(200, 200, 200)
        End With
        row = row + 2
        
        ' Formato de totales
        .Range("B4:B" & row).NumberFormat = "_([$S/] * #,##0.00_);_([$S/] * (#,##0.00);_([$S/] * ""-""??_);_(@_)"
        .Columns("A:C").AutoFit
        
        ' Resumen por proveedor
        .Range("A" & row).value = "RESUMEN POR PROVEEDOR"
        With .Range("A" & row & ":B" & row)
            .Font.Bold = True
            .Font.Size = 14
            .Interior.Color = RGB(31, 78, 121)
            .Font.Color = vbWhite
        End With
        row = row + 1
        .Range("A" & row).value = "PROVEEDOR"
        .Range("B" & row).value = "TOTAL (S/)"
        With .Range("A" & row & ":B" & row)
            .Font.Bold = True
            .Interior.Color = RGB(50, 50, 50)
            .Font.Color = vbWhite
        End With
        row = row + 1
        
        For Each key In summaryData.Keys
            If InStr(key, "PROV_") > 0 Then
                .Range("A" & row).value = Mid(key, InStr(key, "_") + 1)
                .Range("B" & row).value = summaryData(key)
                row = row + 1
            End If
        Next key
        .Range("B" & (row - UBound(summaryData.Keys) + 1) & ":B" & row).NumberFormat = "_([$S/] * #,##0.00_);_([$S/] * (#,##0.00);_([$S/] * ""-""??_);_(@_)"
        
        ' Resumen mensual
        row = row + 2
        .Range("A" & row).value = "RESUMEN MENSUAL"
        With .Range("A" & row & ":B" & row)
            .Font.Bold = True
            .Font.Size = 14
            .Interior.Color = RGB(31, 78, 121)
            .Font.Color = vbWhite
        End With
        row = row + 1
        .Range("A" & row).value = "MES"
        .Range("B" & row).value = "TOTAL (S/)"
        With .Range("A" & row & ":B" & row)
            .Font.Bold = True
            .Interior.Color = RGB(50, 50, 50)
            .Font.Color = vbWhite
        End With
        row = row + 1
        
        For Each key In summaryData.Keys
            If InStr(key, "MES_") > 0 Then
                .Range("A" & row).value = Format(CDate(Mid(key, 5)), "mmmm yyyy")
                .Range("B" & row).value = summaryData(key)
                row = row + 1
            End If
        Next key
        .Range("B" & (row - UBound(summaryData.Keys) + 1) & ":B" & row).NumberFormat = "_([$S/] * #,##0.00_);_([$S/] * (#,##0.00);_([$S/] * ""-""??_);_(@_)"
        
        ' Crear gráficos
        CreateSummaryCharts summarySheet
    End With
End Sub

Sub CreateSummaryCharts(summarySheet As Worksheet)
    ' Crea gráficos para el resumen
    Dim chartObj As ChartObject
    Dim lastRow As Long
    
    With summarySheet
        lastRow = .Cells(.Rows.Count, "A").End(xlUp).row
        
        ' Gráfico de barras para totales por tipo
        Set chartObj = .ChartObjects.Add(Left:=300, Top:=20, Width:=400, Height:=250)
        With chartObj.Chart
            .SetSourceData Source:=summarySheet.Range("A4:B6")
            .ChartType = xlColumnClustered
            .HasTitle = True
            .ChartTitle.Text = "Distribución por Tipo de Pago"
            .ChartTitle.Font.Size = 14
            .ChartTitle.Font.Name = TITLE_FONT
            .Axes(xlValue).HasTitle = True
            .Axes(xlValue).AxisTitle.Text = "Total (S/)"
            .Axes(xlCategory).HasTitle = True
            .Axes(xlCategory).AxisTitle.Text = "Categoría"
            .Legend.Position = xlBottom
            .ApplyDataLabels
        End With
        
        ' Gráfico circular para distribución por proveedor
        Set chartObj = .ChartObjects.Add(Left:=300, Top:=300, Width:=400, Height:=250)
        With chartObj.Chart
            .SetSourceData Source:=summarySheet.Range("A9:B" & summarySheet.Cells(summarySheet.Rows.Count, "A").End(xlUp).row)
            .ChartType = xlPie
            .HasTitle = True
            .ChartTitle.Text = "Distribución por Proveedor"
            .ChartTitle.Font.Size = 14
            .ChartTitle.Font.Name = TITLE_FONT
            .Legend.Position = xlRight
            ' Corrección: Aplicar etiquetas de datos con tipo explícito
            .ApplyDataLabels Type:=xlDataLabelsShowLabelAndPercent
            With .SeriesCollection(1).DataLabels
                .ShowPercentage = True
                .ShowValue = False
                .Font.Size = 10
                .Font.Name = TITLE_FONT
            End With
        End With
    End With
End Sub
