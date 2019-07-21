Imports System.IO

Public Class Form1

    Dim route As String = "C:\Octave\Octave-4.4.0\bin\octave-cli.exe"
    'Dim route As String = "C:\Octave\Octave-5.1.0.0\octave.vbs"
    Dim pInfo As New ProcessStartInfo
    Dim p As Process

    Dim vector1(10) As Double
    Dim vector2(10) As Double

    Dim vectort(10) As Double

    Dim posxi1, tami1 As Integer
    Dim posxi2, tami2 As Integer

    Dim elementos As Integer

    Dim auxCarro1 As Integer = 0
    Dim auxCarro2 As Integer = 0

    Dim grafico As Graphics '2do Animación


    Sub sendO(cadena As String)
        AppActivate(route)
        SendKeys.SendWait(cadena & Chr(13))
        System.Threading.Thread.Sleep(200)
        progressBar1.Value += 1
    End Sub

    Sub Cargar()
        ReDim vector1(elementos)
        ReDim vector2(elementos)
        ReDim vectort(elementos)

        Dim datosix1, datosix2, datost As StreamReader
        datosix1 = New StreamReader(Application.StartupPath & "\datosix1.dat")
        For j As Integer = 0 To elementos
            vector1(j) = Val(datosix1.ReadLine) * Val(textBoxG.Text)
        Next
        datosix1.Close()

        datosix2 = New StreamReader(Application.StartupPath & "\datosix2.dat")
        For k As Integer = 0 To elementos
            vector2(k) = Val(datosix2.ReadLine) * Val(textBoxG.Text)
        Next
        datosix2.Close()

        datost = New StreamReader(Application.StartupPath & "\datost.dat")
        For i As Integer = 0 To elementos
            vectort(i) = Val(datost.ReadLine)
        Next

        datost.Close()
        labelt.Text = vectort(elementos)
        Timer1.Enabled = True

    End Sub

    Private Sub button1_Click(sender As Object, e As EventArgs) Handles button1.Click

        elementos = Val(TextBoxE.Text)
        labele.Text = textBoxM.Text & "s^2+" &
        textBoxB.Text & "s+" & textBoxK.Text &
        textBoxM2.Text & "s^2+" & textBoxB2.Text & "s+" & textBoxK2.Text
        labele.Text = "0/" & elementos
        sendO("clear")
        sendO("clc")
        sendO("pkg load control")

        sendO("s=tf{(}'s'{)};")

        sendO("b1=" & textBoxB.Text & ";")
        sendO("m1=" & textBoxM.Text & ";")
        sendO("k1=" & textBoxK.Text & ";")

        sendO("m2=" & textBoxM2.Text & ";")
        sendO("b2=" & textBoxB2.Text & ";")
        sendO("k2=" & textBoxK2.Text & ";")

        sendO("g1={(}m2*s*s{+}k2{+}b2*s{)}/{(}{(}m1*s*s{+}k1{+}b1*s{+}k2{)}*{(}m2*s*s{+}k2{+}b2*s{)}{+}{(}k2*k2{)}{)};")
        sendO("[x1,t]=step{(}g1{)};")

        sendO("c=length{(}t{)};")
        sendO("tiempo=t{(}c{)}*1.1;")

        sendO("[x1, t]=step{(}g1, tiempo, tiempo/" & elementos & "{)};")
        sendO("dlmwrite{(}'" & Application.StartupPath & "\datosix1.dat', x1, '\n'{)};")
        sendO("dlmwrite{(}'" & Application.StartupPath & "\datost.dat', x1, '\n'{)};")

        sendO("g2={(}k2{)}/{(}{(}m1*s*s{+}k1{+}b1*s{+}k2{)}*{(}m2*s*s{+}k2{+}b2*s{)}*{(}k2{)}{)};")
        sendO("[x2,t]=step{(}g2{)};")

        sendO("c=length{(}t{)};")
        sendO("tiempo=t{(}c{)}*1.1;")

        sendO("[x2, t]=step{(}g2, tiempo, tiempo/" & elementos & "{)};")
        sendO("dlmwrite{(}'" & Application.StartupPath & "\datosix2.dat', x2, '\n'{)};")
        sendO("dlmwrite{(}'" & Application.StartupPath & "\datost.dat', x2, '\n'{)};")

        Cargar()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        pictureBoxCarro1.Location = New Point(posxi1 + vector1(auxCarro1), pictureBoxCarro1.Location.Y)
        pictureBoxResorte1.Height = tami1 + vector1(auxCarro1)
        Chart1.Series(0).Points.AddXY(vectort(auxCarro1), vector1(auxCarro1))

        pictureBoxCarro2.Location = New Point(posxi2 + vector2(auxCarro2), pictureBoxCarro2.Location.Y)
        pictureBoxResorte3.Location = New Point(pictureBoxCarro2.Location.X + pictureBoxCarro2.Width, pictureBoxResorte3.Location.Y)
        pictureBoxResorte3.Width = tami2 + vector2(auxCarro2)
        Chart2.Series(0).Points.AddXY(vectort(auxCarro2), vector2(auxCarro2))

        pictureBoxResorte2.Location = New Point(pictureBoxCarro1.Location.X + pictureBoxCarro1.Width, pictureBoxResorte2.Location.Y)
        pictureBoxResorte2.Width = pictureBoxCarro2.Location.X

        auxCarro1 += 1
        labele.Text = auxCarro1 & "/" & elementos
        If auxCarro1 = elementos Then
            Timer1.Enabled = False
        End If

    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pInfo.FileName = route
        pInfo.WindowStyle = ProcessWindowStyle.Minimized
        p = Process.Start(pInfo)
    End Sub


End Class
