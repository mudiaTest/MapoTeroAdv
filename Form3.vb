﻿Imports System
Imports System.IO
Imports System.Windows.Forms

Public Class Form3
    Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
    Dim folderScalania As String = ""

    Dim styl_numeracji As String
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load






        folderScalania = myPath & "\download\"
        TextBox1.Text = folderScalania
        TextBox1.Enabled = False
        GroupBox1.Enabled = False
        CheckBox2.Checked = Module1.georef_scalanie_qgis
        CheckBox3.Checked = Module1.georef_scalanie_kml
        CheckBox4.Checked = Module1.georef_scalanie_map
        CheckBox5.Checked = Module1.georef_scalanie_tab
        Dim zm_jpg As String = ""

        'procedura wczytywania parametrów macierzy z pliku conf.txt
        On Error GoTo errorhandler
        FileClose(1) 'w razie gyby był otwarty

        FileOpen(1, folderScalania & "\conf.txt", OpenMode.Input)
        Input(1, "folderSegmentow")
        Input(1, "rodzaj mapy")
        Input(1, TextBox4.Text)
        Input(1, TextBox5.Text)
        Input(1, TextBox2.Text)
        Input(1, TextBox3.Text)
        Input(1, TextBox6.Text)
        Input(1, TextBox7.Text) ' rozdzielczosc 1 segmentu
        Input(1, "TextBox10.Text")
        Input(1, "null") 'Input(1, warstwy(0))
        Input(1, "null") 'Input(1, warstwy(1))
        Input(1, "null") 'Input(1, warstwy(2))
        Input(1, "null") 'Input(1, warstwy(3))
        Input(1, "null") 'Input(1, warstwy(4))
        Input(1, "null") 'Input(1, warstwy(5))
        Input(1, "null") 'Input(1, warstwy(6))
        Input(1, "null") 'Input(1, warstwy(7))
        Input(1, "null") 'Input(1, warstwy(8))
        Input(1, "null") 'Input(1, warstwy(9))
        Input(1, "null") 'Input(1, warstwy(10))
        Input(1, "null") 'Input(1, warstwy(11))

        Input(1, zm_jpg) 'format

        If wspolnaNazwaKwadratu = "" Then
            Input(1, "null")
            TextBox11.Text = "\"

        Else
            Input(1, TextBox11.Text)
        End If
        Input(1, "null") 'Input(1, pobierajPowyzejOstatniego)
        Input(1, "null") 'Input(1, Label35.Text)
        Input(1, "null") 'Input(1, Label63.Text)
        Input(1, "null") 'Input(1, Label65.Text)
        Input(1, ComboBox2.Text)
        FileClose(1)

errorhandler:
        If zm_jpg = "jpeg" Then
            ComboBox1.Text = "jpg"

        Else
            If zm_jpg = "tiff" Then
                ComboBox1.Text = "tif"

            Else
                ComboBox1.Text = zm_jpg

            End If

        End If

        TextBox8.Text = Math.Ceiling(((Val(TextBox3.Text) - Val(TextBox5.Text)) / Val(TextBox7.Text)) / Val(TextBox6.Text))  'ile seg horiz
        TextBox9.Text = Math.Ceiling(((Val(TextBox2.Text) - Val(TextBox4.Text)) / Val(TextBox7.Text)) / Val(TextBox6.Text))  'ile seg wert

        If ComboBox2.Text = "01_02_03" Then
            styl_numeracji = "0"
        ElseIf ComboBox2.Text = "1_2_3" Then
            styl_numeracji = "1"
        ElseIf ComboBox2.Text = "NrWiersza_NrKolumny" Then
            styl_numeracji = "2"
        End If

        If File.Exists(folderScalania & "\error.txt") = False Then

            If File.Exists(folderScalania & "\conf.txt") = False Then
                MsgBox("Brak pliku konfiguracji 'conf.txt' w podanej lokalizacji.")
                CheckBox1.Enabled = True
                RichTextBox1.ForeColor = System.Drawing.Color.Red
                RichTextBox1.Text = "We wskazanym katalogu segmentów nie odnaleziono pliku konfiguracyjnego conf.txt. Jesli został on bezpowrotnie utracony, istnieje możliwość samodzielnego zdefiniowania parametrów segmentów. W tym celu zaznacz opcję 'ręczne wprowadzanie parametrów'"
            Else
                CheckBox1.Enabled = False
                RichTextBox1.Text = "Segmenty gotowe do złączenia. Wszelkie parametry scalania zostały załadowane automatycznie z pliku conf.txt"
            End If
        Else
            RichTextBox1.ForeColor = System.Drawing.Color.Red
            RichTextBox1.Text = "W katalogu segmentów wykryto obecność pliku error.txt co świadczy o niekompletnym zestawie segmentów. Uzupełnij je i usuń plik error.txt"
        End If


    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.FolderBrowserDialog1.SelectedPath = myPath & "\download\"
        If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            folderScalania = FolderBrowserDialog1.SelectedPath & "\"
            If File.Exists(folderScalania & "\conf.txt") = False Then
                MsgBox("Brak pliku konfiguracji 'conf.txt' w podanej lokalizacji.")
            Else
                RichTextBox1.ForeColor = System.Drawing.Color.Green
                RichTextBox1.Text = "Plik conf.txt został pomyślnie załadowany z nowej lokalizacji. Segmenty gotowe do złączenia"
            End If
            TextBox1.Text = folderScalania
            TextBox1.Enabled = True
        End If

    End Sub

    Public Sub WykonajScalanie(ByVal amyPath As String, ByVal atb8 As String, ByVal atb9 As String, ByVal atb6 As String, ByVal astylNumeracji As String, ByVal atb1 As Integer, ByVal afolderscalania As String, ByVal atb11 As String, ByVal anazwasklejka As String, ByVal acb1 As String, ByVal afolderSegmentow As String)
        'Procedura skleja kafle w mapę
        'Wywolanie: ..\NoToCONS.exe [Npoziom] [Npion] [Px] [TypNazwy] [Qjpg] [Path] [Prefix] [NazwaMapy] [Rozszerzenie]

        If File.Exists(folderScalania & "\error.txt") = True Then
            RichTextBox1.ForeColor = System.Drawing.Color.Red
            RichTextBox1.Text = "W katalogu segmentów wykryto obecność pliku error.txt co świadczy o niekompletnym zestawie segmentów. Uzupełnij je i usuń plik error.txt"
        Else
            Dim procID As Integer
            procID = Shell(amyPath & "\skrypty\NoToCONS.exe" & " " & atb8 & " " & atb9 & " " & atb6 & " " & astylNumeracji & " " & atb1 & " " & Chr(34) & afolderscalania & Chr(34) & " " & atb11 & " " & anazwasklejka & " " & acb1, AppWinStyle.NormalFocus)


            'sklejka = Shell(myPath & "\skrypty\NoToCONS.exe" & " " & TextBox8.Text & " " & TextBox9.Text & " " & TextBox6.Text & " " & styl_numeracji & " " & TrackBar1.Value & " " & Chr(34) & folderScalania & Chr(34) & " " & TextBox11.Text & " " & nazwa_sklejka & " " & ComboBox1.Text, AppWinStyle.NormalFocus)
            'Sleep(1000 * 1)
            'If File.Exists(folderScalania & nazwa_sklejka & "." & ComboBox1.Text) = True Then
            If procID <> 0 Then
                Module1.folderSegmentow = afolderSegmentow

                Form1.RichTextBox1.ForeColor = System.Drawing.Color.Green
                Form1.RichTextBox1.Text = "Segmenty zostały prawidłowo stalone i zapisane do pliku o nazwie" & " " & anazwasklejka
                If Module1.georef_scalanie_qgis = True Then Module1.plikGeoreferencyjny_jpgw()
                If Module1.georef_scalanie_kml = True Then Module1.plikGeoreferencyjny_kml()
                If Module1.georef_scalanie_map = True Then Module1.plikGeoreferencyjny_map()
                If Module1.georef_scalanie_tab = True Then Module1.plikGeoreferencyjny_tab()
                Me.Close()
            Else
                RichTextBox1.ForeColor = System.Drawing.Color.Red
                RichTextBox1.Text = "Błąd. Segmenty nie zostały poprawnie scalone. Prawdopodobnie przygotowane wcześniej segmenty obszaru nie są kompletne, bądź po ich skompletowaniu nie został usunięty plik errot.txt"
            End If
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        'Dim sklejka As String
        Dim nazwa_sklejka As String = ""
        nazwa_sklejka = "_scalone_segmenty_" & TextBox8.Text & "x" & TextBox9.Text
        WykonajScalanie(myPath, TextBox8.Text, TextBox9.Text, TextBox6.Text, styl_numeracji, TrackBar1.Value, folderScalania, TextBox11.Text, nazwa_sklejka, ComboBox1.Text, Module1.folderSegmentow)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            GroupBox1.Enabled = True
            TextBox2.Enabled = False
            TextBox3.Enabled = False
            TextBox4.Enabled = False
            TextBox5.Enabled = False

        Else
            GroupBox1.Enabled = False
        End If
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Label20.Text = TrackBar1.Value & "%"
    End Sub


    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged

        Module1.georef_scalanie_qgis = CheckBox2.CheckState



    End Sub

    Private Sub CheckBox2_Click(sender As Object, e As EventArgs) Handles CheckBox2.Click
        Select Case CheckBox2.CheckState
            Case CheckState.Checked
                Module1.georef_scalanie_qgis = True

            Case CheckState.Unchecked
                Module1.georef_scalanie_qgis = False
        End Select
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged

        Module1.georef_scalanie_kml = CheckBox3.CheckState



    End Sub

    Private Sub CheckBox3_Click(sender As Object, e As EventArgs) Handles CheckBox3.Click
        Select Case CheckBox3.CheckState
            Case CheckState.Checked
                Module1.georef_scalanie_kml = True

            Case CheckState.Unchecked
                Module1.georef_scalanie_kml = False
        End Select
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged

        Module1.georef_scalanie_map = CheckBox4.CheckState



    End Sub

    Private Sub CheckBox4_Click(sender As Object, e As EventArgs) Handles CheckBox4.Click
        Select Case CheckBox4.CheckState
            Case CheckState.Checked
                Module1.georef_scalanie_map = True

            Case CheckState.Unchecked
                Module1.georef_scalanie_map = False
        End Select
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        Module1.georef_scalanie_tab = CheckBox5.CheckState
    End Sub

    Private Sub CheckBox5_Click(sender As Object, e As EventArgs) Handles CheckBox5.Click
        Select Case CheckBox5.CheckState
            Case CheckState.Checked
                Module1.georef_scalanie_tab = True

            Case CheckState.Unchecked
                Module1.georef_scalanie_tab = False
        End Select
    End Sub

    Private Sub Form3_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Module1.georef_scalanie_qgis = False
        Module1.georef_scalanie_kml = False
        Module1.georef_scalanie_map = False
        Module1.georef_scalanie_tab = False

    End Sub
End Class