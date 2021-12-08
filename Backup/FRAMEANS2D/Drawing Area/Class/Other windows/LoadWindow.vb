Imports System.Drawing.Drawing2D
Public Class LoadWindow
    Dim TempEL As SelectedElement
    Dim SNsupport As SupportDetails
    Dim ENsupport As SupportDetails

    Private Class SelectedElement
        Public Spoint As Point
        Public Epoint As Point
        Public Mpoint As Point
        Public inclination As Double
        Public ActualLength As Double
        Public CurrentLength As Double
        Public SNode As Integer
        Public ENode As Integer
        Public maxload As Double = 10
    End Class

    Private Class SupportDetails
        Dim _Supportinclination As Double = 90 * (Math.PI / 180)
        Dim _PJ As Boolean = True
        Dim _RJ As Boolean
        Dim _PS As Boolean
        Dim _PRS As Boolean
        Dim _FRS As Boolean
        Dim _FS As Boolean
        Dim _SS As Boolean
        Dim _dxsettlement As Double
        Dim _dysettlement As Double
        Dim _kstiffness As Double

        Public Property Supportinclination() As Double
            Get
                Return _Supportinclination
            End Get
            Set(ByVal value As Double)
                _Supportinclination = value
            End Set
        End Property

        Public Property PJ() As Boolean
            Get
                Return _PJ
            End Get
            Set(ByVal value As Boolean)
                _PJ = value
            End Set
        End Property

        Public Property RJ() As Boolean
            Get
                Return _RJ
            End Get
            Set(ByVal value As Boolean)
                _RJ = value
            End Set
        End Property

        Public Property PS() As Boolean
            Get
                Return _PS
            End Get
            Set(ByVal value As Boolean)
                _PS = value
            End Set
        End Property

        Public Property PRS() As Boolean
            Get
                Return _PRS
            End Get
            Set(ByVal value As Boolean)
                _PRS = value
            End Set
        End Property

        Public Property FRS() As Boolean
            Get
                Return _FRS
            End Get
            Set(ByVal value As Boolean)
                _FRS = value
            End Set
        End Property

        Public Property FS() As Boolean
            Get
                Return _FS
            End Get
            Set(ByVal value As Boolean)
                _FS = value
            End Set
        End Property

        Public Property SS() As Boolean
            Get
                Return _SS
            End Get
            Set(ByVal value As Boolean)
                _SS = value
            End Set
        End Property

        Public Property dxsettlement() As Double
            Get
                Return _dxsettlement
            End Get
            Set(ByVal value As Double)
                _dxsettlement = value
            End Set
        End Property

        Public Property dysettlement() As Double
            Get
                Return _dysettlement
            End Get
            Set(ByVal value As Double)
                _dysettlement = value
            End Set
        End Property

        Public Property kstiffness() As Double
            Get
                Return _kstiffness
            End Get
            Set(ByVal value As Double)
                _kstiffness = value
            End Set
        End Property

        Public Sub New(ByVal Pjoint As Boolean, ByVal Rjoint As Boolean, ByVal Psupport As Boolean, ByVal PRsupport As Boolean, ByVal FRsupport As Boolean, ByVal Fsupport As Boolean, ByVal Ssupport As Boolean, ByVal supincln As Double, ByVal dx As Double, ByVal dy As Double, ByVal K As Double)
            _PJ = Pjoint
            _RJ = Rjoint
            _PS = Psupport
            _PRS = PRsupport
            _FRS = FRsupport
            _FS = Fsupport
            _SS = Ssupport

            If _PS = True Or _PRS = True Or _FRS = True Or _FS = True Then
                _Supportinclination = supincln
                _dxsettlement = If(dx <> 0, dx, Nothing)
                _dysettlement = If(dy <> 0, dy, Nothing)
            ElseIf _SS = True Then
                _Supportinclination = supincln
                _kstiffness = If(K <> 0, K, Nothing)
            End If
        End Sub
    End Class

    Public Sub New(ByVal El As Line2DT)
        'TempEL = New SelectedElement
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        '---------Fix Size of Window
        If MDIMain.Width > MDIMain.Height Then
            Me.Width = 475 '(MDIMain.Width / 3)
            Me.Height = (475 * (MDIMain.Height / MDIMain.Width)) + 218 + 25
        Else
            Me.Height = 475 + 218 + 25 '(MDIMain.Width / 3)
            Me.Width = (475 * (MDIMain.Width / MDIMain.Height))
        End If
        CoordFix(El)
        BindLoadtoDatasource(El)
        For Each itm In WA2dt.Mem
            _memberBindingsource.Add(itm)
            If itm.Equals(El) Then
                _memberBindingsource.Position = WA2dt.Mem.IndexOf(itm) + 1
            End If
        Next
    End Sub

#Region "Element Binding"
    Private Sub BindLoadtoDatasource(ByVal El As Line2DT)
        _pointloadBindingSource.Clear()
        For Each pl In El.Pload
            _pointloadBindingSource.Add(pl)
        Next
        _UVLBindingSource.Clear()
        For Each ul In El.Uload
            _UVLBindingSource.Add(ul)
        Next
        _momentBindingsource.Clear()
        For Each ml In El.Mload
            _momentBindingsource.Add(ml)
        Next
        AddDeleteCheck()
    End Sub

    Private Sub CoordFix(ByVal El As Line2DT)
        Dim Tx As Double
        Dim Ty As Double
        Dim theta As Double
        Dim MinSide As Double

        If _LoadPic.Width <= _LoadPic.Height Then
            MinSide = _LoadPic.Width - 40
        Else
            MinSide = _LoadPic.Height - 40
        End If
        TempEL = New SelectedElement

        Tx = El.EN.Coord.X - El.SN.Coord.X
        Ty = El.EN.Coord.Y - El.SN.Coord.Y

        TempEL.inclination = -1 * Math.Atan2((El.SN.Coord.Y - El.EN.Coord.Y), (El.EN.Coord.X - El.SN.Coord.X))
        If Math.Abs(Tx) <= Math.Abs(Ty) Then
            theta = Math.Acos((El.EN.Coord.X - El.SN.Coord.X) / (Math.Round(Math.Sqrt(Math.Pow((El.SN.Coord.X - El.EN.Coord.X), 2) + Math.Pow((El.SN.Coord.Y - El.EN.Coord.Y), 2)))))
            Tx = MinSide / (Math.Tan(theta))
            Ty = If((El.EN.Coord.Y - El.SN.Coord.Y) < 1, MinSide * -1, MinSide)
        Else
            theta = Math.Asin((El.EN.Coord.Y - El.SN.Coord.Y) / (Math.Round(Math.Sqrt(Math.Pow((El.SN.Coord.X - El.EN.Coord.X), 2) + Math.Pow((El.SN.Coord.Y - El.EN.Coord.Y), 2)))))
            Tx = If((El.EN.Coord.X - El.SN.Coord.X) < 1, MinSide * -1, MinSide)
            Ty = MinSide * (Math.Tan(theta))
        End If

        TempEL.Mpoint = New Point((Me._LoadPic.Width / 2), (Me._LoadPic.Height / 2))
        TempEL.Spoint = New Point(TempEL.Mpoint.X - (Tx / 2), TempEL.Mpoint.Y - (Ty / 2))
        TempEL.Epoint = New Point(TempEL.Mpoint.X + (Tx / 2), TempEL.Mpoint.Y + (Ty / 2))
        TempEL.ActualLength = El.Length
        TempEL.CurrentLength = Math.Sqrt(Math.Pow((TempEL.Spoint.X - TempEL.Epoint.X), 2) + Math.Pow((TempEL.Spoint.Y - TempEL.Epoint.Y), 2))
        PloadTabpageInitialize()
        UloadTabpageInitialize()
        MloadTabpageInitialize()
        For Each D In WA2dt.Bob
            If D.Coord.X = El.SN.Coord.X And D.Coord.Y = El.SN.Coord.Y Then
                TempEL.SNode = WA2dt.Bob.IndexOf(D)
            End If
            If D.Coord.X = El.EN.Coord.X And D.Coord.Y = El.EN.Coord.Y Then
                TempEL.ENode = WA2dt.Bob.IndexOf(D)
            End If
        Next

        SNsupport = New SupportDetails(El.SN.Support.PJ, El.SN.Support.RJ, El.SN.Support.PS, El.SN.Support.PRS, El.SN.Support.FRS, El.SN.Support.FS, El.SN.Support.SS, El.SN.Support.supportinclination, El.SN.Support.settlementdx, El.SN.Support.settlementdy, El.SN.Support.stiffnessK)
        ENsupport = New SupportDetails(El.EN.Support.PJ, El.EN.Support.RJ, El.EN.Support.PS, El.EN.Support.PRS, El.EN.Support.FRS, El.EN.Support.FS, El.EN.Support.SS, El.EN.Support.supportinclination, El.EN.Support.settlementdx, El.EN.Support.settlementdy, El.EN.Support.stiffnessK)

        _LoadPic.Refresh()
    End Sub

    Private Sub LoadWindow_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        WA2dt.selLine.Clear()
        MDIMain._DMCMenable()
        WA2dt.MainPic.Refresh()
    End Sub

    Private Sub LoadWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AddHandler _PloadAlign.CheckedChanged, AddressOf AddDeleteCheck
        AddHandler _PloadHorizontal.CheckedChanged, AddressOf AddDeleteCheck
        AddHandler _PloadVertical.CheckedChanged, AddressOf AddDeleteCheck
        AddHandler _pointloadBindingSource.CurrentChanged, AddressOf AddDeleteCheck
        AddHandler _Ploadintensity.TextChanged, AddressOf AddDeleteCheck
        AddHandler _UloadAlign.CheckedChanged, AddressOf AddDeleteCheck
        AddHandler _UloadHorizontal.CheckedChanged, AddressOf AddDeleteCheck
        AddHandler _UloadVertical.CheckedChanged, AddressOf AddDeleteCheck
        AddHandler _UVLBindingSource.CurrentChanged, AddressOf AddDeleteCheck
        AddHandler _ULoadSintensity.TextChanged, AddressOf AddDeleteCheck
        AddHandler _ULoadEintensity.TextChanged, AddressOf AddDeleteCheck
        AddHandler _Mloadintensity.TextChanged, AddressOf AddDeleteCheck
        AddHandler _MloadAnticlockwise.CheckedChanged, AddressOf AddDeleteCheck
        AddHandler _Mloadclockwise.CheckedChanged, AddressOf AddDeleteCheck
        AddHandler _momentBindingsource.CurrentChanged, AddressOf AddDeleteCheck
    End Sub

    Private Sub _memberNavigator_RefreshItems(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _memberBindingsource.CurrentChanged
        If _memberBindingsource.Count <> 0 Then
            CoordFix(_memberBindingsource.Current)
            BindLoadtoDatasource(_memberBindingsource.Current)
            WA2dt.selLine(0) = _memberBindingsource.IndexOf(_memberBindingsource.Current)
            WA2dt.MainPic.Refresh()
        End If
    End Sub
#End Region

    Private Sub TabControl1_SelectedIndexChanged() Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedIndex = 0 Then
            PloadTabpageInitialize()
        ElseIf TabControl1.SelectedIndex = 2 Then
            UloadTabpageInitialize()
        ElseIf TabControl1.SelectedIndex = 1 Then
            MloadTabpageInitialize()
        End If
        _LoadPic.Refresh()
    End Sub

#Region "Point Load Binding"
    Private Sub PloadTabpageInitialize()
        _Ploadlocation.Maximum = TempEL.ActualLength * 100
    End Sub

    Private Sub _Ploadlocation_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _Ploadlocation.Scroll
        _ploadlocationlabel.Text = _Ploadlocation.Value / 100
        _LoadPic.Refresh()
    End Sub

    Public Overridable Function PointLoadAddnew() Handles _pointloadBindingSource.AddingNew
        Dim Mpt As New Point((WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).SN.Coord.X + WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).EN.Coord.X) / 2, (WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).SN.Coord.Y + WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).EN.Coord.Y) / 2)
        Dim PPt1 As New Point(Mpt.X + (Math.Cos(WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Inclination) * (((_Ploadlocation.Value / 100) - (WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Length / 2)) * MDIMain.Nappdefaults.defaultScaleFactor)), Mpt.Y + (Math.Sin(WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Inclination) * (((_Ploadlocation.Value / 100) - (WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Length / 2)) * MDIMain.Nappdefaults.defaultScaleFactor)))
        Dim PPt2 As New Point
        If Val(_Ploadintensity.Text) = 0 Then
            MsgBox("Load value not valid", MsgBoxStyle.Information, "FrameANS")
            _pointloadBindingSource.RemoveCurrent()
            Return Nothing
            Exit Function
        End If
        For Each mem In WA2dt.Mem
            For Each L In mem.Pload
                If (PPt1.X = L.PCoordS.X And PPt1.Y = L.PCoordS.Y) Then
                    MsgBox("Load Location identical - Point load already declared in current location", MsgBoxStyle.Information, "FrameANS")
                    _pointloadBindingSource.RemoveCurrent()
                    Return Nothing
                    Exit Function
                End If
            Next
        Next
        PPt2 = If(_PloadAlign.Checked = True, _
                  New Point(PPt1.X - (Math.Sin(TempEL.inclination) * (60 * (Val(_Ploadintensity.Text) / TempEL.maxload))), _
                            PPt1.Y + (Math.Cos(TempEL.inclination) * (60 * (Val(_Ploadintensity.Text) / TempEL.maxload)))), _
               If(_PloadHorizontal.Checked = True, _
                  New Point(PPt1.X - (Math.Sin(0) * (60 * (-Val(_Ploadintensity.Text) / TempEL.maxload))), _
                            PPt1.Y + (Math.Cos(0) * (60 * (-Val(_Ploadintensity.Text) / TempEL.maxload)))), _
                  New Point(PPt1.X - (Math.Sin(Math.PI / 2) * (60 * (Val(_Ploadintensity.Text) / TempEL.maxload))), _
                            PPt1.Y + (Math.Cos(Math.PI / 2) * (60 * (Val(_Ploadintensity.Text) / TempEL.maxload))))))  '----Else
        WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Pload.Add(New FRAMEANS2D.Line2DT.P(Val(_Ploadintensity.Text), (_Ploadlocation.Value / 100), PPt1, PPt2, _PloadAlign.Checked, _PloadHorizontal.Checked, _PloadVertical.Checked))
        Dim A As New AddPortal2DT(0)
        AddDeleteCheck()
        Return 0
    End Function

    Private Sub _PLBindingNavigatorDeleteItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _PLBindingNavigatorDeleteItem.Click
        WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Pload.RemoveAt(_pointloadBindingSource.Position)
        Dim A As New AddPortal2DT(0)
        AddDeleteCheck()
    End Sub

    Private Sub _Ploadintensity_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Ploadintensity.TextChanged
        If IsNumeric(_Ploadintensity.Text) Then
            If Val(_Ploadintensity.Text) = 0 Then
                _Ploadintensity.Text = "0.1"
            End If
        ElseIf _Ploadintensity.Text = "-" Then
            _Ploadintensity.Text = "-1"
        Else
            _Ploadintensity.Text = ""
        End If
    End Sub
#End Region

#Region "UVL Binding"
    Private Sub UloadTabpageInitialize()
        _ULoadSlocation.Maximum = TempEL.ActualLength * 100
        _ULoadElocation.Maximum = TempEL.ActualLength * 100
        _ULoadElocation.Value = _ULoadElocation.Maximum
        _ULoadElocationLabel.Text = _ULoadElocation.Value / 100
    End Sub

    Private Sub _ULoadSlocation_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ULoadSlocation.Scroll
        If _ULoadSlocation.Value >= _ULoadElocation.Value Then
            _ULoadSlocation.Value = _ULoadElocation.Value - 1
        End If
        _ULoadSlocationLabel.Text = _ULoadSlocation.Value / 100
        _LoadPic.Refresh()
    End Sub

    Private Sub _ULoadElocation_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ULoadElocation.Scroll
        If _ULoadElocation.Value <= _ULoadSlocation.Value Then
            _ULoadElocation.Value = _ULoadSlocation.Value + 1
        End If
        _ULoadElocationLabel.Text = _ULoadElocation.Value / 100
        _LoadPic.Refresh()
    End Sub

    Public Overridable Function UVLAddnew() Handles _UVLBindingSource.AddingNew
        Dim Mpt As New Point((WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).SN.Coord.X + WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).EN.Coord.X) / 2, (WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).SN.Coord.Y + WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).EN.Coord.Y) / 2)
        Dim U1Pt1 As New Point(Mpt.X + (Math.Cos(WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Inclination) * (((_ULoadSlocation.Value / 100) - (WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Length / 2)) * MDIMain.Nappdefaults.defaultScaleFactor)), Mpt.Y + (Math.Sin(WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Inclination) * (((_ULoadSlocation.Value / 100) - (WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Length / 2)) * MDIMain.Nappdefaults.defaultScaleFactor)))
        Dim U2Pt1 As New Point(Mpt.X + (Math.Cos(WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Inclination) * (((_ULoadElocation.Value / 100) - (WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Length / 2)) * MDIMain.Nappdefaults.defaultScaleFactor)), Mpt.Y + (Math.Sin(WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Inclination) * (((_ULoadElocation.Value / 100) - (WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Length / 2)) * MDIMain.Nappdefaults.defaultScaleFactor)))
        Dim U1Pt2 As Point
        Dim U2Pt2 As Point
        If Val(_ULoadSintensity.Text) = 0 And Val(_ULoadEintensity.Text) = 0 Then
            MsgBox("Load value not valid", MsgBoxStyle.Information, "FrameANS")
            _UVLBindingSource.RemoveCurrent()
            Return Nothing
            Exit Function
        End If
        For Each mem In WA2dt.Mem
            For Each L In mem.Uload
                If (U1Pt1.X = L.uCoordS1.X And U1Pt1.Y = L.uCoordS1.Y) And (U2Pt1.X = L.uCoordS2.X And U2Pt1.Y = L.uCoordS2.Y) Then
                    MsgBox("Load Location identical - Point load already declared in current location", MsgBoxStyle.Information, "FrameANS")
                    _UVLBindingSource.RemoveCurrent()
                    Return Nothing
                    Exit Function
                End If
            Next
        Next
        U1Pt2 = If(_UloadAlign.Checked = True, _
                    New Point(U1Pt1.X - (Math.Sin(TempEL.inclination) * (60 * (Val(_ULoadSintensity.Text) / TempEL.maxload))), _
                              U1Pt1.Y + (Math.Cos(TempEL.inclination) * (60 * (Val(_ULoadSintensity.Text) / TempEL.maxload)))), _
                If(_UloadHorizontal.Checked = True, _
                     New Point(U1Pt1.X - (Math.Sin(0) * (60 * (-Val(_ULoadSintensity.Text) / TempEL.maxload))), _
                               U1Pt1.Y + (Math.Cos(0) * (60 * (-Val(_ULoadSintensity.Text) / TempEL.maxload)))), _
                     New Point(U1Pt1.X - (Math.Sin(Math.PI / 2) * (60 * (Val(_ULoadSintensity.Text) / TempEL.maxload))), _
                               U1Pt1.Y + (Math.Cos(Math.PI / 2) * (60 * (Val(_ULoadSintensity.Text) / TempEL.maxload))))))
        U2Pt2 = If(_UloadAlign.Checked = True, _
                    New Point(U2Pt1.X - (Math.Sin(TempEL.inclination) * (60 * (Val(_ULoadEintensity.Text) / TempEL.maxload))), _
                              U2Pt1.Y + (Math.Cos(TempEL.inclination) * (60 * (Val(_ULoadEintensity.Text) / TempEL.maxload)))), _
                If(_UloadHorizontal.Checked = True, _
                    New Point(U2Pt1.X - (Math.Sin(0) * (60 * (-Val(_ULoadEintensity.Text) / TempEL.maxload))), _
                              U2Pt1.Y + (Math.Cos(0) * (60 * (-Val(_ULoadEintensity.Text) / TempEL.maxload)))), _
                    New Point(U2Pt1.X - (Math.Sin(Math.PI / 2) * (60 * (Val(_ULoadEintensity.Text) / TempEL.maxload))), _
                              U2Pt1.Y + (Math.Cos(Math.PI / 2) * (60 * (Val(_ULoadEintensity.Text) / TempEL.maxload))))))
        WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Uload.Add(New FRAMEANS2D.Line2DT.U(Val(_ULoadSintensity.Text), Val(_ULoadEintensity.Text), (_ULoadSlocation.Value / 100), (_ULoadElocation.Value / 100), U1Pt1, U1Pt2, U2Pt1, U2Pt2, _UloadAlign.Checked, _UloadHorizontal.Checked, _UloadVertical.Checked))
        Dim A As New AddPortal2DT(0)
        AddDeleteCheck()
        Return 0
    End Function

    Private Sub _UVLBindingNavigatorDeleteItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _UVLBindingNavigatorDeleteItem.Click
        WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Uload.RemoveAt(_UVLBindingSource.Position)
        Dim A As New AddPortal2DT(0)
        AddDeleteCheck()
    End Sub

    Private Sub _ULoadEintensity_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ULoadEintensity.TextChanged
        If IsNumeric(_ULoadEintensity.Text) Then
            If Val(_ULoadEintensity.Text) = 0 Then
                _ULoadEintensity.Text = "0.1"
            End If
        ElseIf _ULoadEintensity.Text = "-" Then
            _ULoadEintensity.Text = "-1"
        Else
            _ULoadEintensity.Text = ""
        End If
    End Sub

    Private Sub _ULoadSintensity_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ULoadSintensity.TextChanged
        If IsNumeric(_ULoadSintensity.Text) Then
            If Val(_ULoadSintensity.Text) = 0 Then
                _ULoadSintensity.Text = "0.1"
            End If
        ElseIf _ULoadSintensity.Text = "-" Then
            _ULoadSintensity.Text = "-1"
        Else
            _ULoadSintensity.Text = ""
        End If
    End Sub
#End Region

#Region "Moment Binding"
    Private Sub MloadTabpageInitialize()
        _Mloadlocation.Maximum = TempEL.ActualLength * 100
    End Sub

    Private Sub _Mloadlocation_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _Mloadlocation.Scroll
        _mloadlocationlabel.Text = _Mloadlocation.Value / 100
        _LoadPic.Refresh()
    End Sub

    Public Overridable Function MLoadAddnew() Handles _momentBindingsource.AddingNew
        Dim Mpt As New Point((WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).SN.Coord.X + WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).EN.Coord.X) / 2, (WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).SN.Coord.Y + WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).EN.Coord.Y) / 2)
        Dim MPt1 As New Point(Mpt.X + (Math.Cos(WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Inclination) * (((_Mloadlocation.Value / 100) - (WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Length / 2)) * MDIMain.Nappdefaults.defaultScaleFactor)), Mpt.Y + (Math.Sin(WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Inclination) * (((_Mloadlocation.Value / 100) - (WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Length / 2)) * MDIMain.Nappdefaults.defaultScaleFactor)))
        If Val(_Mloadintensity.Text) = 0 Then
            MsgBox("Load value not valid", MsgBoxStyle.Information, "FrameANS")
            _momentBindingsource.RemoveCurrent()
            Return Nothing
            Exit Function
        End If
        For Each mem In WA2dt.Mem
            For Each L In mem.Mload
                If (MPt1.X = L.mCoord.X And MPt1.Y = L.mCoord.Y) Then
                    MsgBox("Load Location identical - moment load already declared in current location", MsgBoxStyle.Information, "FrameANS")
                    _momentBindingsource.RemoveCurrent()
                    Return Nothing
                    Exit Function
                End If
            Next
        Next
        Dim MLoad As Double = If(_Mloadclockwise.Checked = True, Val(_Mloadintensity.Text), -1 * Val(_Mloadintensity.Text))
        WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Mload.Add(New FRAMEANS2D.Line2DT.M(MLoad, (_Mloadlocation.Value / 100), MPt1, _Mloadclockwise.Checked, _MloadAnticlockwise.Checked))
        Dim A As New AddPortal2DT(0)
        AddDeleteCheck()
        Return 0
    End Function

    Private Sub _MBindingNavigatorDeleteItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _momentBindingNavigatorDeleteItem.Click
        WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Mload.RemoveAt(_momentBindingsource.Position)
        Dim A As New AddPortal2DT(0)
        AddDeleteCheck()
    End Sub

    Private Sub _Mloadintensity_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Mloadintensity.TextChanged
        If IsNumeric(_Mloadintensity.Text) Then
            If Val(_Mloadintensity.Text) = 0 Then
                _Mloadintensity.Text = "0.1"
            End If
        ElseIf _Mloadintensity.Text = "-" Then
            _Mloadintensity.Text = "-1"
        Else
            _Mloadintensity.Text = ""
        End If
    End Sub
#End Region

#Region "Paint Load Pic"
    Private Sub _LoadPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles _LoadPic.Paint
        Try
            LoadAreaPaintMember(e)
            LoadAreaPaintLoad(e)
            LoadAreaPaintNode(e)
            tempLOADPaint(e)
        Catch ex As Exception
            'MsgBox("Zzzzzzzzsad")
        End Try
    End Sub

    Private Sub LoadAreaPaintMember(ByRef e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.DrawLine(New Pen(OptionD.Mc, 2), TempEL.Spoint, TempEL.Epoint)
        e.Graphics.DrawString(Math.Round(TempEL.ActualLength, OptionD.Prec), New Font("Verdana", (8)), New Pen(OptionD.Lc, 2 / WA2dt.Zm).Brush, TempEL.Mpoint.X, TempEL.Mpoint.Y)
    End Sub

    Private Sub LoadAreaPaintNode(ByRef e As System.Windows.Forms.PaintEventArgs)
        ' Start Node
        'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4).Brush, TempEL.Spoint.X - (2), TempEL.Spoint.Y - (2), 4, 4)
        e.Graphics.DrawString(TempEL.SNode, New Font("Verdana", (8)), New Pen(OptionD.NNc, 2).Brush, TempEL.Spoint.X - (14), TempEL.Spoint.Y - (14))
        ' End Node
        'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4).Brush, TempEL.Epoint.X - (2), TempEL.Epoint.Y - (2), 4, 4)
        e.Graphics.DrawString(TempEL.ENode, New Font("Verdana", (8)), New Pen(OptionD.NNc, 2).Brush, TempEL.Epoint.X - (14), TempEL.Epoint.Y - (14))

        '--- Start Node
        If SNsupport.PJ = True Then
            LoadAreaPaintPinJoint(e, TempEL.Spoint, SNsupport.Supportinclination)
        ElseIf SNsupport.RJ = True Then
            LoadAreaPaintRigidJoint(e, TempEL.Spoint, SNsupport.Supportinclination)
        ElseIf SNsupport.PS = True Then
            LoadAreaPaintPinSupport(e, TempEL.Spoint, SNsupport.Supportinclination, 0.5)
        ElseIf SNsupport.PRS = True Then
            LoadAreaPaintPinRollerSupport(e, TempEL.Spoint, SNsupport.Supportinclination, 0.5)
        ElseIf SNsupport.FRS = True Then
            LoadAreaPaintFixedRollerSupport(e, TempEL.Spoint, SNsupport.Supportinclination, 0.5)
        ElseIf SNsupport.FS = True Then
            LoadAreaPaintFixedSupport(e, TempEL.Spoint, SNsupport.Supportinclination, 0.5)
        ElseIf SNsupport.SS = True Then
            LoadAreaPaintSpringSupport(e, TempEL.Spoint, SNsupport.Supportinclination, 0.5)
        End If

        '--- End Node
        If ENsupport.PJ = True Then
            LoadAreaPaintPinJoint(e, TempEL.Epoint, ENsupport.Supportinclination)
        ElseIf ENsupport.RJ = True Then
            LoadAreaPaintRigidJoint(e, TempEL.Epoint, ENsupport.Supportinclination)
        ElseIf ENsupport.PS = True Then
            LoadAreaPaintPinSupport(e, TempEL.Epoint, ENsupport.Supportinclination, 0.5)
        ElseIf ENsupport.PRS = True Then
            LoadAreaPaintPinRollerSupport(e, TempEL.Epoint, ENsupport.Supportinclination, 0.5)
        ElseIf ENsupport.FRS = True Then
            LoadAreaPaintFixedRollerSupport(e, TempEL.Epoint, ENsupport.Supportinclination, 0.5)
        ElseIf ENsupport.FS = True Then
            LoadAreaPaintFixedSupport(e, TempEL.Epoint, ENsupport.Supportinclination, 0.5)
        ElseIf ENsupport.SS = True Then
            LoadAreaPaintSpringSupport(e, TempEL.Epoint, ENsupport.Supportinclination, 0.5)
        End If
    End Sub

    Private Sub LoadAreaPaintLoad(ByRef e As System.Windows.Forms.PaintEventArgs)
        Try
            For Each itm In WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Pload
                Dim PPt1 As New Point(((Math.Cos(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.plocation) * (TempEL.CurrentLength / TempEL.ActualLength)))), ((Math.Sin(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.plocation) * (TempEL.CurrentLength / TempEL.ActualLength)))))
                Dim PPt2 As New Point
                PPt2 = If(itm.MAlign = True, New Point(PPt1.X - (Math.Sin(TempEL.inclination) * (40 * (-itm.pload / TempEL.maxload))), PPt1.Y + (Math.Cos(TempEL.inclination) * (40 * (-itm.pload / TempEL.maxload)))), If(itm.HAlign = True, New Point(PPt1.X + (40 * (itm.pload / TempEL.maxload)), PPt1.Y), New Point(PPt1.X, PPt1.Y + (40 * (-itm.pload / TempEL.maxload)))))
                Dim loadpen As New System.Drawing.Pen(Color.Green, 2)
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                If itm.Equals(WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Pload(_pointloadBindingSource.Position)) And TabControl1.SelectedIndex = 0 Then
                    Dim loadpen1 As New System.Drawing.Pen(Color.Yellow, 3)
                    loadpen1.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(4, 5)
                    e.Graphics.DrawLine(loadpen1, TempEL.Mpoint.X - PPt1.X, TempEL.Mpoint.Y - PPt1.Y, TempEL.Mpoint.X - PPt2.X, TempEL.Mpoint.Y - PPt2.Y)
                End If

                e.Graphics.DrawLine(loadpen, TempEL.Mpoint.X - PPt1.X, TempEL.Mpoint.Y - PPt1.Y, TempEL.Mpoint.X - PPt2.X, TempEL.Mpoint.Y - PPt2.Y)
                e.Graphics.DrawString(Math.Abs(itm.pload), New Font("Verdana", (8)), New Pen(Color.Green, 2).Brush, TempEL.Mpoint.X - PPt2.X + (2), TempEL.Mpoint.Y - PPt2.Y + (2))
            Next
            For Each itm In WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Uload
                Dim u1Pt1 As New Point(((Math.Cos(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.ulocation1) * (TempEL.CurrentLength / TempEL.ActualLength)))), ((Math.Sin(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.ulocation1) * (TempEL.CurrentLength / TempEL.ActualLength)))))
                Dim u1Pt2 As New Point
                Dim u2Pt1 As New Point(((Math.Cos(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.ulocation2) * (TempEL.CurrentLength / TempEL.ActualLength)))), ((Math.Sin(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.ulocation2) * (TempEL.CurrentLength / TempEL.ActualLength)))))
                Dim u2Pt2 As New Point
                u1Pt2 = If(itm.MAlign = True, New Point(u1Pt1.X - (Math.Sin(TempEL.inclination) * (40 * (-itm.uload1 / TempEL.maxload))), u1Pt1.Y + (Math.Cos(TempEL.inclination) * (40 * (-itm.uload1 / TempEL.maxload)))), If(itm.HAlign = True, New Point(u1Pt1.X + (40 * (itm.uload1 / TempEL.maxload)), u1Pt1.Y), New Point(u1Pt1.X, u1Pt1.Y + (40 * (-itm.uload1 / TempEL.maxload)))))
                u2Pt2 = If(itm.MAlign = True, New Point(u2Pt1.X - (Math.Sin(TempEL.inclination) * (40 * (-itm.uload2 / TempEL.maxload))), u2Pt1.Y + (Math.Cos(TempEL.inclination) * (40 * (-itm.uload2 / TempEL.maxload)))), If(itm.HAlign = True, New Point(u2Pt1.X + (40 * (itm.uload2 / TempEL.maxload)), u2Pt1.Y), New Point(u2Pt1.X, u2Pt1.Y + (40 * (-itm.uload2 / TempEL.maxload)))))
                Dim loadpen As New System.Drawing.Pen(Color.DeepPink, 2)
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                If itm.Equals(WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Uload(_UVLBindingSource.Position)) And TabControl1.SelectedIndex = 2 Then
                    Dim loadpen1 As New System.Drawing.Pen(Color.Yellow, 3)
                    loadpen1.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(4, 5)
                    e.Graphics.DrawLine(loadpen1, TempEL.Mpoint.X - u1Pt1.X, TempEL.Mpoint.Y - u1Pt1.Y, TempEL.Mpoint.X - u1Pt2.X, TempEL.Mpoint.Y - u1Pt2.Y)
                    e.Graphics.DrawLine(loadpen1, TempEL.Mpoint.X - u2Pt1.X, TempEL.Mpoint.Y - u2Pt1.Y, TempEL.Mpoint.X - u2Pt2.X, TempEL.Mpoint.Y - u2Pt2.Y)
                End If

                Dim hBrush3 As HatchBrush = New HatchBrush(HatchStyle.DiagonalBrick, Color.DeepPink, Color.Transparent)
                Dim uStream() As Point = { _
                                            New Point(TempEL.Mpoint.X - u1Pt1.X, TempEL.Mpoint.Y - u1Pt1.Y), _
                                            New Point(TempEL.Mpoint.X - u1Pt2.X, TempEL.Mpoint.Y - u1Pt2.Y), _
                                            New Point(TempEL.Mpoint.X - u2Pt2.X, TempEL.Mpoint.Y - u2Pt2.Y), _
                                            New Point(TempEL.Mpoint.X - u2Pt1.X, TempEL.Mpoint.Y - u2Pt1.Y), _
                                            New Point(TempEL.Mpoint.X - u1Pt1.X, TempEL.Mpoint.Y - u1Pt1.Y)}
                Dim Stream_path As New GraphicsPath
                Stream_path.AddClosedCurve(uStream, 0)
                e.Graphics.FillPath(hBrush3, Stream_path)

                e.Graphics.DrawLine(loadpen, TempEL.Mpoint.X - u1Pt1.X, TempEL.Mpoint.Y - u1Pt1.Y, TempEL.Mpoint.X - u1Pt2.X, TempEL.Mpoint.Y - u1Pt2.Y)
                e.Graphics.DrawString(Math.Abs(itm.uload1), New Font("Verdana", (8)), loadpen.Brush, TempEL.Mpoint.X - u1Pt2.X + (2), TempEL.Mpoint.Y - u1Pt2.Y + (2))
                e.Graphics.DrawLine(loadpen, TempEL.Mpoint.X - u2Pt1.X, TempEL.Mpoint.Y - u2Pt1.Y, TempEL.Mpoint.X - u2Pt2.X, TempEL.Mpoint.Y - u2Pt2.Y)
                e.Graphics.DrawString(Math.Abs(itm.uload2), New Font("Verdana", (8)), loadpen.Brush, TempEL.Mpoint.X - u2Pt2.X + (2), TempEL.Mpoint.Y - u2Pt2.Y + (2))
                e.Graphics.DrawLine(New Pen(Color.DeepPink, 2), TempEL.Mpoint.X - u1Pt2.X, TempEL.Mpoint.Y - u1Pt2.Y, TempEL.Mpoint.X - u2Pt2.X, TempEL.Mpoint.Y - u2Pt2.Y)
            Next
            For Each itm In WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Mload
                Dim MPt1 As New Point(((Math.Cos(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.mlocation) * (TempEL.CurrentLength / TempEL.ActualLength)))), ((Math.Sin(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.mlocation) * (TempEL.CurrentLength / TempEL.ActualLength)))))
                Dim loadpen As New System.Drawing.Pen(Color.Orange, 2)
                loadpen.CustomStartCap = If(itm.mAnticlockwise = True, New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5), New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0))
                loadpen.CustomEndCap = If(itm.mClockwise = True, New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5), New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0))
                If itm.Equals(WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Mload(_momentBindingsource.Position)) And TabControl1.SelectedIndex = 1 Then
                    Dim loadpen1 As New System.Drawing.Pen(Color.Yellow, 3)
                    loadpen1.CustomStartCap = If(itm.mAnticlockwise = True, New System.Drawing.Drawing2D.AdjustableArrowCap(4, 5), New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0))
                    loadpen1.CustomEndCap = If(itm.mClockwise = True, New System.Drawing.Drawing2D.AdjustableArrowCap(4, 5), New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0))
                    e.Graphics.DrawArc(loadpen1, TempEL.Mpoint.X - MPt1.X - 15, TempEL.Mpoint.Y - MPt1.Y - 15, 30, 30, 360, 270)
                End If
                e.Graphics.DrawArc(loadpen, TempEL.Mpoint.X - MPt1.X - 15, TempEL.Mpoint.Y - MPt1.Y - 15, 30, 30, 360, 270)
                e.Graphics.DrawString(Math.Abs(itm.mload), New Font("Verdana", (8)), loadpen.Brush, TempEL.Mpoint.X - MPt1.X - 15, TempEL.Mpoint.Y - MPt1.Y + 15)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tempLOADPaint(ByRef e As System.Windows.Forms.PaintEventArgs)
        Try
            If TabControl1.SelectedIndex = 0 Then
                Dim PPt1 As New Point(((Math.Cos(TempEL.inclination) * (((TempEL.ActualLength / 2) - (_Ploadlocation.Value / 100)) * (TempEL.CurrentLength / TempEL.ActualLength)))), ((Math.Sin(TempEL.inclination) * (((TempEL.ActualLength / 2) - (_Ploadlocation.Value / 100)) * (TempEL.CurrentLength / TempEL.ActualLength)))))
                Dim PPt2 As New Point
                PPt2 = If(_PloadAlign.Checked = True, New Point(PPt1.X - (Math.Sin(TempEL.inclination) * (40 * (-Val(_Ploadintensity.Text) / TempEL.maxload))), _
                                                                PPt1.Y + (Math.Cos(TempEL.inclination) * (40 * (-Val(_Ploadintensity.Text) / TempEL.maxload)))), _
                          If(_PloadHorizontal.Checked = True, New Point(PPt1.X + (40 * (Val(_Ploadintensity.Text) / TempEL.maxload)), PPt1.Y), _
                                                              New Point(PPt1.X, PPt1.Y + (40 * (-Val(_Ploadintensity.Text) / TempEL.maxload)))))
                Dim loadpen As New System.Drawing.Pen(Color.Green, 2)
                loadpen.DashStyle = Drawing2D.DashStyle.Dot
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                e.Graphics.DrawLine(loadpen, TempEL.Mpoint.X - PPt1.X, TempEL.Mpoint.Y - PPt1.Y, TempEL.Mpoint.X - PPt2.X, TempEL.Mpoint.Y - PPt2.Y)
                e.Graphics.DrawString(Math.Abs(Val(_Ploadintensity.Text)), New Font("Verdana", (8)), New Pen(Color.Green, 2).Brush, TempEL.Mpoint.X - PPt2.X + (2), TempEL.Mpoint.Y - PPt2.Y + (2))
            ElseIf TabControl1.SelectedIndex = 2 Then
                Dim U1Pt1 As New Point(((Math.Cos(TempEL.inclination) * (((TempEL.ActualLength / 2) - (_ULoadSlocation.Value / 100)) * (TempEL.CurrentLength / TempEL.ActualLength)))), ((Math.Sin(TempEL.inclination) * (((TempEL.ActualLength / 2) - (_ULoadSlocation.Value / 100)) * (TempEL.CurrentLength / TempEL.ActualLength)))))
                Dim U2Pt1 As New Point(((Math.Cos(TempEL.inclination) * (((TempEL.ActualLength / 2) - (_ULoadElocation.Value / 100)) * (TempEL.CurrentLength / TempEL.ActualLength)))), ((Math.Sin(TempEL.inclination) * (((TempEL.ActualLength / 2) - (_ULoadElocation.Value / 100)) * (TempEL.CurrentLength / TempEL.ActualLength)))))
                Dim U1Pt2 As New Point
                Dim U2Pt2 As New Point

                U1Pt2 = If(_UloadAlign.Checked = True, New Point(U1Pt1.X - (Math.Sin(TempEL.inclination) * (40 * (-Val(_ULoadSintensity.Text) / TempEL.maxload))), U1Pt1.Y + (Math.Cos(TempEL.inclination) * (40 * (-Val(_ULoadSintensity.Text) / TempEL.maxload)))), If(_UloadHorizontal.Checked = True, New Point(U1Pt1.X + (40 * (Val(_ULoadSintensity.Text) / TempEL.maxload)), U1Pt1.Y), New Point(U1Pt1.X, U1Pt1.Y + (40 * (-Val(_ULoadSintensity.Text) / TempEL.maxload)))))
                U2Pt2 = If(_UloadAlign.Checked = True, New Point(U2Pt1.X - (Math.Sin(TempEL.inclination) * (40 * (-Val(_ULoadEintensity.Text) / TempEL.maxload))), U2Pt1.Y + (Math.Cos(TempEL.inclination) * (40 * (-Val(_ULoadEintensity.Text) / TempEL.maxload)))), If(_UloadHorizontal.Checked = True, New Point(U2Pt1.X + (40 * (Val(_ULoadEintensity.Text) / TempEL.maxload)), U2Pt1.Y), New Point(U2Pt1.X, U2Pt1.Y + (40 * (-Val(_ULoadEintensity.Text) / TempEL.maxload)))))

                Dim hBrush3 As HatchBrush = New HatchBrush(HatchStyle.DiagonalBrick, Color.DeepPink, Color.Transparent)
                Dim uStream() As Point = { _
                                            New Point(TempEL.Mpoint.X - U1Pt1.X, TempEL.Mpoint.Y - U1Pt1.Y), _
                                            New Point(TempEL.Mpoint.X - U1Pt2.X, TempEL.Mpoint.Y - U1Pt2.Y), _
                                            New Point(TempEL.Mpoint.X - U2Pt2.X, TempEL.Mpoint.Y - U2Pt2.Y), _
                                            New Point(TempEL.Mpoint.X - U2Pt1.X, TempEL.Mpoint.Y - U2Pt1.Y), _
                                            New Point(TempEL.Mpoint.X - U1Pt1.X, TempEL.Mpoint.Y - U1Pt1.Y)}
                Dim Stream_path As New GraphicsPath
                Stream_path.AddClosedCurve(uStream, 0)
                e.Graphics.FillPath(hBrush3, Stream_path)

                Dim loadpen As New System.Drawing.Pen(Color.DeepPink, 2)
                loadpen.DashStyle = Drawing2D.DashStyle.Dot
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                e.Graphics.DrawLine(loadpen, TempEL.Mpoint.X - U1Pt1.X, TempEL.Mpoint.Y - U1Pt1.Y, TempEL.Mpoint.X - U1Pt2.X, TempEL.Mpoint.Y - U1Pt2.Y)
                e.Graphics.DrawString(Math.Abs(Val(_ULoadSintensity.Text)), New Font("Verdana", (8)), New Pen(Color.DeepPink, 2).Brush, TempEL.Mpoint.X - U1Pt2.X + (2), TempEL.Mpoint.Y - U1Pt2.Y + (2))
                e.Graphics.DrawLine(loadpen, TempEL.Mpoint.X - U2Pt1.X, TempEL.Mpoint.Y - U2Pt1.Y, TempEL.Mpoint.X - U2Pt2.X, TempEL.Mpoint.Y - U2Pt2.Y)
                e.Graphics.DrawString(Math.Abs(Val(_ULoadEintensity.Text)), New Font("Verdana", (8)), New Pen(Color.DeepPink, 2).Brush, TempEL.Mpoint.X - U2Pt2.X + (2), TempEL.Mpoint.Y - U2Pt2.Y + (2))
                e.Graphics.DrawLine(New Pen(Color.DeepPink, 2), TempEL.Mpoint.X - U1Pt2.X, TempEL.Mpoint.Y - U1Pt2.Y, TempEL.Mpoint.X - U2Pt2.X, TempEL.Mpoint.Y - U2Pt2.Y)
            ElseIf TabControl1.SelectedIndex = 1 Then
                Dim MPt1 As New Point(((Math.Cos(TempEL.inclination) * (((TempEL.ActualLength / 2) - (_Mloadlocation.Value / 100)) * (TempEL.CurrentLength / TempEL.ActualLength)))), ((Math.Sin(TempEL.inclination) * (((TempEL.ActualLength / 2) - (_Mloadlocation.Value / 100)) * (TempEL.CurrentLength / TempEL.ActualLength)))))
                Dim loadpen As New System.Drawing.Pen(Color.Orange, 2)
                loadpen.DashStyle = Drawing2D.DashStyle.Dot
                loadpen.CustomStartCap = If(_MloadAnticlockwise.Checked = True, New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5), New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0))
                loadpen.CustomEndCap = If(_Mloadclockwise.Checked = True, New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5), New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0))
                e.Graphics.DrawArc(loadpen, TempEL.Mpoint.X - MPt1.X - 15, TempEL.Mpoint.Y - MPt1.Y - 15, 30, 30, 360, 270)
                e.Graphics.DrawString(Math.Abs(Val(_Mloadintensity.Text)), New Font("Verdana", (8)), loadpen.Brush, TempEL.Mpoint.X - MPt1.X - 15, TempEL.Mpoint.Y - MPt1.Y + 15)
            End If
        Catch ex As Exception

        End Try
    End Sub

#Region "Support Pic Paint Events"
    Private Sub LoadAreaPaintPinJoint(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double)
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), M.X - 2, M.Y - 2, 4, 4)
    End Sub

    Private Sub LoadAreaPaintRigidJoint(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double)
        e.Graphics.DrawRectangle(New Pen(OptionD.Nc, 2), M.X - 2, M.Y - 2, 4, 4)
    End Sub

    Private Sub LoadAreaPaintPinSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)))

        '----Bottom Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + ((20 * -Math.Sin(-Inclination) + (-20 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZMfactor))

    End Sub

    Private Sub LoadAreaPaintPinRollerSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))

    End Sub

    Private Sub LoadAreaPaintFixedRollerSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
        '---- Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((0 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((0 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((0 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((0 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZMfactor)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))

    End Sub

    Private Sub LoadAreaPaintFixedSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '---- Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)))

    End Sub

    Private Sub LoadAreaPaintSpringSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
        '----SPRING
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((Math.Cos(-Inclination)) + (Math.Sin(-Inclination))) * ZMfactor), M.Y + (((-Math.Sin(-Inclination)) + (Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((4 * Math.Cos(-Inclination)) + (Math.Sin(-Inclination))) * ZMfactor), M.Y + (((4 * -Math.Sin(-Inclination)) + (Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((4 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((4 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((4 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((4 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((10 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((10 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((10 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((10 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor)))
        'e.Graphics.DrawLine(New Pen(Color.DarkSlateGray, 2), New Point(M.X + (((12 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((12 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((12 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((12 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((16 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((16 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((16 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((16 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((16 * Math.Cos(-Inclination)) + (Math.Sin(-Inclination))) * ZMfactor), M.Y + (((16 * -Math.Sin(-Inclination)) + (Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (Math.Sin(-Inclination))) * ZMfactor), M.Y + (((-20 * Math.Sin(-Inclination)) + (Math.Cos(-Inclination))) * ZMfactor)))

        '----Bottom Rectangle
        e.Graphics.DrawLine(New Pen(Color.DarkSlateGray, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(Color.DarkSlateGray, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(Color.DarkSlateGray, 2), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(Color.DarkSlateGray, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZMfactor)))

    End Sub
#End Region
#End Region

    Private Sub AddDeleteCheck()
        Try
            TempEL.maxload = If(TabControl1.SelectedIndex = 0, Math.Abs(Val(_Ploadintensity.Text)), If(Math.Abs(Val(_ULoadSintensity.Text)) > Math.Abs(Val(_ULoadEintensity.Text)), Math.Abs(Val(_ULoadSintensity.Text)), Math.Abs(Val(_ULoadEintensity.Text))))
            For Each itm In WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Pload
                If Math.Abs(itm.pload) > TempEL.maxload Then
                    TempEL.maxload = Math.Abs(itm.pload)
                End If
            Next
            For Each itm In WA2dt.Mem(_memberBindingsource.IndexOf(_memberBindingsource.Current)).Uload
                If Math.Abs(itm.uload1) > TempEL.maxload Then
                    TempEL.maxload = Math.Abs(itm.uload1)
                End If
                If Math.Abs(itm.uload2) > TempEL.maxload Then
                    TempEL.maxload = Math.Abs(itm.uload2)
                End If
            Next
            WA2dt.MainPic.Refresh()
            _LoadPic.Refresh()
        Catch ex As Exception

        End Try
    End Sub
End Class

