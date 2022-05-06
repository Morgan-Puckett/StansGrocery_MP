'Morgan Puckett
'RCET 0265
'May 6th 2022
'Stans Grocery
'


Option Strict On
Option Explicit On

'I cant rename any forms without major issues, is this a computer problem?
Public Class Form1
    Dim food(300, 2) As String
    Dim usable(300, 2) As String
    Dim filename As String = "C:\Users\morga\OneDrive\VB\StansGrocery_MP\Grocery.txt" ' set up own path on machine

    'Searchs file for keywords
    Private Sub SearchButton_Click() Handles SearchButton.Click
        FilterByCategoryRadioButton.Checked = True
        If SearchTextBox.Text = "zzz" Then
            Me.Close()
        Else
            CheckFile(SearchTextBox.Text)
        End If
        Select Case DisplayListBox.Items.Count
            Case < 1
                DisplayLabel.Text = $"Sorry no matches for '{SearchTextBox.Text}'"
            Case = 1
                DisplayLabel.Text = $"{DisplayListBox.Items.Count} product"
            Case >= 2
                DisplayLabel.Text = $"{DisplayListBox.Items.Count} products"
        End Select

    End Sub

    'Shows file in listed format with names only
    Private Sub DisplayListBox_Click() Handles DisplayListBox.Click

        Try
            If DisplayListBox.SelectedItem.ToString() <> Nothing Then
                DisplayLabel.Text = ($" You will find {DisplayListBox.SelectedItem.ToString()} on aisle {CheckLocation(SearchTextBox.Text)} ")

            End If
        Catch ex As Exception

        End Try
    End Sub

    'Loads and resets form/file
    Private Sub StansGroceryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReadFile()
        Reset()
    End Sub

    'Filters
    Private Sub FilterSelect() Handles FilterByAisleRadioButton.CheckedChanged, FilterByCategoryRadioButton.CheckedChanged
        Dim er() As String
        Dim re As String
        Dim i As Integer
        Select Case True
            Case FilterByAisleRadioButton.Checked
                re = "##LOC"
                i = 1

            Case FilterByCategoryRadioButton.Checked
                re = "%%CAT"
                i = 2
        End Select
        FilterComboBox.Items.Clear()
        FilterComboBox.Items.Add(" Show All")
        FilterComboBox.SelectedItem = " Show All"

        For i = LBound(food) To UBound(food)
            Try
                er = Split(food(i, i), re)
                If FilterComboBox.Items.Contains(er(1)) = False And er(1) <> "" Then   '.PadLeft(2)) = False And er(1) <> "" Then
                    FilterComboBox.Items.Add(er(1))

                End If
            Catch ex As Exception

            End Try
        Next
        FilterComboBox.Sorted = True
    End Sub

    'Applies filters necessary
    Private Sub FilterComboBox_SelectedindexChanged() Handles FilterComboBox.SelectedIndexChanged
        '
        If FilterByCategoryRadioButton.Checked = True Then
            DisplayLabel.Text = $"{FilterComboBox.Text} products"
        End If
        If FilterByAisleRadioButton.Checked = True And FilterComboBox.Text <> "Show All" Then
            DisplayLabel.Text = $"{FilterComboBox.Text} Aisle products  "
        End If

        If FilterComboBox.Text = " Show All" Then
            CheckFile(" ")
        Else
            CheckFile(FilterComboBox.Text)
        End If

    End Sub
    Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click
        Reset()
    End Sub

    'Finds Location of the desired item
    'Fair warning it doesn't retreive the right one- I cant figure out why
    Function CheckLocation(desired As String) As String
        Dim itemLocation() As String
        Dim i As Integer = 0

        i = DisplayListBox.SelectedIndex + 1
        itemLocation = Split(food(i, 1), "##LOC")

        Label1.Text = itemLocation(0)
        Label2.Text = itemLocation(1)
        Label3.Text = itemLocation(2)

        If itemLocation(1) = "" Then
            itemLocation(1) = "N/A"
        End If

        Return itemLocation(1)
        itemLocation(1) = ""
        itemLocation(2) = ""
        itemLocation(0) = ""
    End Function

    'Sets defaults of program
    Sub Reset()
        SearchTextBox.Text = Nothing
        FilterByCategoryRadioButton.Checked = True
        FilterComboBox.Text = (" Show All")
        SearchButton_Click()
    End Sub

    'Searches File for KeyWords
    Sub CheckFile(desired As String)

        Dim current() As String
        Dim loc() As String
        Dim cat() As String
        Dim tempRec As String
        DisplayListBox.Items.Clear()

        For i = LBound(food) To UBound(food)
            tempRec = ($"{food(i, 0)} {food(i, 1)} {food(i, 2)} ")

            current = Split(food(i, 0), "$$ITM")
            loc = Split(food(i, 1), "##LOC")
            cat = Split(food(i, 2), "%%CAT")
            Try
                Console.WriteLine(current(1))
                If InStr(tempRec, desired, CompareMethod.Text) > 0 And InStr(tempRec, desired, CompareMethod.Text) <> 0 Then
                    DisplayListBox.Items.Add($"{current(1)}")
                    usable(i, 0) = current(1)
                    usable(i, 1) = loc(1)
                    usable(i, 2) = cat(1)

                End If
            Catch ex As Exception
                Console.WriteLine("oops")
            End Try
        Next
        DisplayListBox.Sorted = True
        DisplayListBox.Items.Remove("")

    End Sub

    'Opens and Reads File
    Sub ReadFile()
        'Clear listbox each time called for neatness
        DisplayListBox.Items.Clear()
        Dim er As String
        Dim itemNumber As Integer
        Try
            FileOpen(1, filename, OpenMode.Input)
            Do Until EOF(1)
                For lineNumber = 0 To 2

                    Input(1, er)
                    food(itemNumber, lineNumber) = er

                Next
                itemNumber += 1
            Loop
            FileClose(1)
        Catch ex As Exception

        End Try
    End Sub

End Class
