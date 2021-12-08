Public Class ResultArea2DT

#Region "Main Variables"
    Dim _Zm As Double = 1
    Dim _mpoint As Point
    Dim _Spoint As Point
    Dim _Epoint As Point
    Dim _Cpoint As Point
    Dim Tpaint As RTempPaint
    Dim Ppaint As BackgroundMemberPaint
    Dim Rpaint As ResultPaint
    Dim _Linflg As Boolean = False
    Dim _AP As RTempPaint
    Dim _PaintNULL As Boolean = False
    Dim _Analyser As Tesla
#End Region

#Region "Get Set Property"
    Public Property Mpoint() As Point
        Get
            Return _mpoint
        End Get
        Set(ByVal value As Point)
            _mpoint = value
        End Set
    End Property

    Public Property Spoint() As Point
        Get
            Return _Spoint
        End Get
        Set(ByVal value As Point)

        End Set
    End Property

    Public Property Cpoint() As Point
        Get
            Return _Cpoint
        End Get
        Set(ByVal value As Point)

        End Set
    End Property

    Public Property Epoint() As Point
        Get
            Return _Epoint
        End Get
        Set(ByVal value As Point)

        End Set
    End Property

    Public Property Zm() As Double
        Get
            Return _Zm
        End Get
        Set(ByVal value As Double)
            _Zm = value
        End Set
    End Property

    Public Property Linflg() As Boolean
        Get
            Return _Linflg
        End Get
        Set(ByVal value As Boolean)
            _Linflg = value
        End Set
    End Property

    Public Property PaintNULL() As Boolean
        Get
            Return _PaintNULL
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property Analyzer() As Tesla
        Get
            Return _Analyser
        End Get
        Set(ByVal value As Tesla)
            _Analyser = value
        End Set
    End Property
#End Region

    Private Sub ResultArea2DT_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _mpoint = New Point(WA2dt.Mpoint.X, WA2dt.Mpoint.Y)
        _Zm = WA2dt.Zm
    End Sub

#Region "Zoom Handler"
    Private Sub MainPic_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainPic.MouseEnter
        MainPic.Focus()
        '_PD = New SnapPredicate2DT
        Windows.Forms.Cursor.Show()
    End Sub

    Private Sub MainPic_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainPic.MouseLeave
        '_PD = New SnapPredicate2DT
        Windows.Forms.Cursor.Show()
    End Sub

    Private Sub MainPic_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseWheel
        Dim xw, yw As Double
        MainPic.Focus()
        xw = (e.X / Zm) - _mpoint.X
        yw = (e.Y / Zm) - _mpoint.Y
        If e.Delta > 0 Then
            If Zm < 100 Then
                Zm = Zm + 0.1
            End If
        ElseIf e.Delta < 0 Then
            If Zm > 0.101 Then
                Zm = Zm - 0.1
            End If
        End If
        _mpoint.X = (e.X / Zm) - xw
        _mpoint.Y = (e.Y / Zm) - yw
        MTPic.Refresh()
        '_PD = New SnapPredicate2DT
    End Sub
#End Region

#Region "Move Handler"
    Private Sub MainPic_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseMove
        _Cpoint = e.Location
        MTPic.Refresh()
    End Sub
#End Region

#Region "Click Handler"
    Private Sub MainPic_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseClick
        MainPic.Focus()
        If e.Button = Windows.Forms.MouseButtons.Right And Linflg = False Then
            MDIMain.ContextMenuStrip2.Show(MainPic.PointToScreen(e.Location))
        End If
        If e.Button = Windows.Forms.MouseButtons.Left Then
            For Each EL In WA2dt.Mem
                If SelectMember((e.X / Zm) - RA2dt.Mpoint.X, (e.Y / Zm) - RA2dt.Mpoint.Y, EL.SN.Coord, EL.EN.Coord, EL.Inclination) = True Then
                    Try
                        Dim A As New MemberResultView(EL)
                        A.ShowDialog()
                        Exit Sub
                    Catch ex As Exception

                    End Try
                End If
            Next
        End If
        MTPic.Refresh()
    End Sub

    Private Sub MainPic_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseDoubleClick
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            MainPic.Refresh()
        End If
    End Sub

    Private Function SelectMember(ByVal Ix1 As Double, ByVal Iy1 As Double, ByVal Spoint As Point, ByVal Epoint As Point, ByVal theta As Double) As Integer
        Dim th As Double
        Dim sm1 As Double
        Dim sm2 As Double
        Dim mx1 As Double = Spoint.X
        Dim my1 As Double = Spoint.Y
        Dim mx2 As Double = Epoint.X
        Dim my2 As Double = Epoint.Y
        Dim seltheta As Double = (180 / Math.PI) * Math.Atan(Math.Abs(my1 - my2) / Math.Abs(mx1 - mx2))


        sm1 = ((180 / Math.PI) * Math.Atan(Math.Abs(Iy1 - my2) / Math.Abs(Ix1 - mx2)))
        sm2 = ((180 / Math.PI) * Math.Atan(Math.Abs(Iy1 - my1) / Math.Abs(Ix1 - mx1)))

        If Ix1 < mx1 And Ix1 > mx2 And Iy1 > my1 And Iy1 < my2 Then
            If (sm1 < (seltheta + 8) And sm1 > (seltheta - 8)) And (sm2 < (seltheta + 8) And sm2 > (seltheta - 8)) Then
                Return True
                Exit Function
            End If
        ElseIf Ix1 > mx1 And Ix1 < mx2 And Iy1 < my1 And Iy1 > my2 Then
            If (sm1 < (seltheta + 8) And sm1 > (seltheta - 8)) And (sm2 < (seltheta + 8) And sm2 > (seltheta - 8)) Then
                Return True
                Exit Function
            End If
        ElseIf Ix1 > mx1 And Ix1 < mx2 And Iy1 > my1 And Iy1 < my2 Then
            If (sm1 < (seltheta + 8) And sm1 > (seltheta - 8)) And (sm2 < (seltheta + 8) And sm2 > (seltheta - 8)) Then
                Return True
                Exit Function
            End If
        ElseIf Ix1 < mx1 And Ix1 > mx2 And Iy1 < my1 And Iy1 > my2 Then
            If (sm1 < (seltheta + 8) And sm1 > (seltheta - 8)) And (sm2 < (seltheta + 8) And sm2 > (seltheta - 8)) Then
                Return True
                Exit Function
            End If
        Else
            th = Math.Abs((180 / Math.PI) * (Math.Atan((my2 - Iy1) / (Ix1 - mx2))))
            If (th > -4 And th < 42) Or (th > 86 And th < 94) Then
                If (Ix1 < mx1 And Ix1 > mx2) Or (Ix1 > mx1 And Ix1 < mx2) Then
                    If (sm1 < (seltheta + 8) And sm1 > (seltheta - 8)) And (sm2 < (seltheta + 8) And sm2 > (seltheta - 8)) Then
                        Return True
                        Exit Function
                    End If
                End If
                If (Iy1 < my1 And Iy1 > my2) Or (Iy1 > my1 And Iy1 < my2) Then
                    If (sm1 < (seltheta + 8) And sm1 > (seltheta - 8)) And (sm2 < (seltheta + 8) And sm2 > (seltheta - 8)) Then
                        Return True
                        Exit Function
                    End If
                End If
            End If
            Return False
            Exit Function
        End If
        Return False
    End Function
#End Region

#Region "Drag Handler"
    Private Sub MainPic_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            _Spoint = New Point((e.X / _Zm) - Mpoint.X, (e.Y / _Zm) - Mpoint.Y)
            _PaintNULL = True
            _Linflg = True
            MDIMain._pan.Checked = True
        End If
    End Sub

    Private Sub MainPic_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            _Epoint = New Point((e.X / _Zm) - Mpoint.X, (e.Y / _Zm) - Mpoint.Y)
            _AP = New RTempPaint(_Spoint, _Epoint)
            _PaintNULL = False
            _Linflg = False
            MDIMain._pan.Checked = False
        End If
    End Sub
#End Region

#Region "Paint Events"
    Private Sub MainPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MainPic.Paint
        e.Graphics.ScaleTransform(_Zm, _Zm)
        Ppaint = New BackgroundMemberPaint(e)
        Rpaint = New ResultPaint(e, PaintNULL, RA2dt.Mpoint)
    End Sub

    Private Sub MTPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MTPic.Paint
        e.Graphics.DrawImage(MDIMain.Cursorimg, New Point(Cpoint.X + 10, Cpoint.Y + 10))
        e.Graphics.ScaleTransform(_Zm, _Zm)
        Tpaint = New RTempPaint(e)
        Rpaint = New ResultPaint(e, If(PaintNULL = True, False, True), New Point(-RA2dt.Spoint.X + (Cpoint.X / Zm), -RA2dt.Spoint.Y + (Cpoint.Y / Zm)))
    End Sub
#End Region
End Class