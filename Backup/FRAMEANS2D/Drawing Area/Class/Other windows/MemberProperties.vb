Imports System.Drawing.Drawing2D

Public Class Member_Properties
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
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        '---------Fix Size of Window
        If MDIMain.Width > MDIMain.Height Then
            Me.Width = 520 '(MDIMain.Width / 3)
            Me.Height = (520 * (MDIMain.Height / MDIMain.Width)) + 280 + 25
        Else
            Me.Height = 520 + 280 + 25 '(MDIMain.Width / 3)
            Me.Width = (520 * (MDIMain.Width / MDIMain.Height))
        End If


        CoordFix(El)
        For Each itm In WA2dt.Mem
            _memberBindingSource.Add(itm)
            If itm.Equals(El) Then
                _memberBindingSource.Position = WA2dt.Mem.IndexOf(itm) + 1
            End If
        Next
    End Sub

#Region "Navigate Member Events"
    Private Sub CoordFix(ByVal El As Line2DT)
        Dim Tx As Double
        Dim Ty As Double
        Dim theta As Double
        Dim MinSide As Double

        If _SupportPic.Width <= _SupportPic.Height Then
            MinSide = _SupportPic.Width - 40
        Else
            MinSide = _SupportPic.Height - 40
        End If
        TempEL = New SelectedElement
        FixMAXLoad()

        RadioButton1.Checked = MDIMain.Nappdefaults.defaultPJ
        RadioButton2.Checked = MDIMain.Nappdefaults.defaultRJ

        TextBox1.Text = El.Length
        TextBox2.Text = El.E
        TextBox3.Text = El.A
        TextBox4.Text = El.I


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

        TempEL.Mpoint = New Point((Me._SupportPic.Width / 2), (Me._SupportPic.Height / 2))
        TempEL.Spoint = New Point(TempEL.Mpoint.X - (Tx / 2), TempEL.Mpoint.Y - (Ty / 2))
        TempEL.Epoint = New Point(TempEL.Mpoint.X + (Tx / 2), TempEL.Mpoint.Y + (Ty / 2))
        TempEL.ActualLength = El.Length
        TempEL.CurrentLength = Math.Sqrt(Math.Pow((TempEL.Spoint.X - TempEL.Epoint.X), 2) + Math.Pow((TempEL.Spoint.Y - TempEL.Epoint.Y), 2))
        For Each D In WA2dt.Bob
            If D.Coord.X = El.SN.Coord.X And D.Coord.Y = El.SN.Coord.Y Then
                TempEL.SNode = WA2dt.Bob.IndexOf(D)
            End If
            If D.Coord.X = El.EN.Coord.X And D.Coord.Y = El.EN.Coord.Y Then
                TempEL.ENode = WA2dt.Bob.IndexOf(D)
            End If
        Next

        SNsupport = New SupportDetails(El.SN.Support.PJ, El.SN.Support.RJ, El.SN.Support.PS, El.SN.Support.PRS, El.SN.Support.FRS, El.SN.Support.FS, El.SN.Support.SS, El.SN.Support.supportinclination, El.SN.Support.settlementdx, El.SN.Support.settlementdy, El.SN.Support.stiffnessK)
        ENsupport = New SupportDetails(El.EN.Support.PJ, El.EN.Support.RJ, El.EN.Support.PS, El.EN.Support.PRS, El.SN.Support.FRS, El.EN.Support.FS, El.EN.Support.SS, El.EN.Support.supportinclination, El.EN.Support.settlementdx, El.EN.Support.settlementdy, El.EN.Support.stiffnessK)

        _SupportPic.Refresh()
    End Sub

    Private Sub _memberNavigator_RefreshItems(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _memberBindingSource.CurrentChanged
        If _memberBindingSource.Count <> 0 Then
            CoordFix(_memberBindingSource.Current)
            WA2dt.selLine(0) = _memberBindingSource.IndexOf(_memberBindingSource.Current)
            _SupportPic.Refresh()
            WA2dt.MainPic.Refresh()
        End If
    End Sub
#End Region

#Region "Paint Support Pic"
    Private Sub _SupportPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles _SupportPic.Paint
        Try
            SupportAreaPaintMember(e)
            SupportAreaPaintNode(e)
            SupportAreaPaintLoad(e)
        Catch ex As Exception
            'MsgBox("Zzzzzzzzsad")
        End Try
    End Sub

    Private Sub SupportAreaPaintMember(ByRef e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.DrawLine(New Pen(OptionD.Mc, 2), TempEL.Spoint, TempEL.Epoint)
        e.Graphics.DrawString(Math.Round(TempEL.ActualLength, OptionD.Prec), New Font("Verdana", (8)), New Pen(OptionD.Lc, 2 / WA2dt.Zm).Brush, TempEL.Mpoint.X, TempEL.Mpoint.Y)
    End Sub

    Private Sub SupportAreaPaintNode(ByRef e As System.Windows.Forms.PaintEventArgs)
        ' Start Node
        'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4).Brush, TempEL.Spoint.X - (2), TempEL.Spoint.Y - (2), 4, 4)
        e.Graphics.DrawString(TempEL.SNode, New Font("Verdana", (8)), New Pen(OptionD.NNc, 2).Brush, TempEL.Spoint.X - (14), TempEL.Spoint.Y - (14))
        ' End Node
        'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4).Brush, TempEL.Epoint.X - (2), TempEL.Epoint.Y - (2), 4, 4)
        e.Graphics.DrawString(TempEL.ENode, New Font("Verdana", (8)), New Pen(OptionD.NNc, 2).Brush, TempEL.Epoint.X - (14), TempEL.Epoint.Y - (14))

        '--- Start Node
        If SNsupport.PJ = True Then
            SupportPicPaintPinJoint(e, TempEL.Spoint, SNsupport.Supportinclination)
        ElseIf SNsupport.RJ = True Then
            SupportPicPaintRigidJoint(e, TempEL.Spoint, SNsupport.Supportinclination)
        ElseIf SNsupport.PS = True Then
            SupportPicPaintPinSupport(e, TempEL.Spoint, SNsupport.Supportinclination, 0.5)
        ElseIf SNsupport.PRS = True Then
            SupportPicPaintPinRollerSupport(e, TempEL.Spoint, SNsupport.Supportinclination, 0.5)
        ElseIf SNsupport.FRS = True Then
            SupportPicPaintFixedRollerSupport(e, TempEL.Spoint, SNsupport.Supportinclination, 0.5)
        ElseIf SNsupport.FS = True Then
            SupportPicPaintFixedSupport(e, TempEL.Spoint, SNsupport.Supportinclination, 0.5)
        ElseIf SNsupport.SS = True Then
            SupportPicPaintSpringSupport(e, TempEL.Spoint, SNsupport.Supportinclination, 0.5)
        End If

        '--- End Node
        If ENsupport.PJ = True Then
            SupportPicPaintPinJoint(e, TempEL.Epoint, ENsupport.Supportinclination)
        ElseIf ENsupport.RJ = True Then
            SupportPicPaintRigidJoint(e, TempEL.Epoint, ENsupport.Supportinclination)
        ElseIf ENsupport.PS = True Then
            SupportPicPaintPinSupport(e, TempEL.Epoint, ENsupport.Supportinclination, 0.5)
        ElseIf ENsupport.PRS = True Then
            SupportPicPaintPinRollerSupport(e, TempEL.Epoint, ENsupport.Supportinclination, 0.5)
        ElseIf ENsupport.FRS = True Then
            SupportPicPaintFixedRollerSupport(e, TempEL.Epoint, ENsupport.Supportinclination, 0.5)
        ElseIf ENsupport.FS = True Then
            SupportPicPaintFixedSupport(e, TempEL.Epoint, ENsupport.Supportinclination, 0.5)
        ElseIf ENsupport.SS = True Then
            SupportPicPaintSpringSupport(e, TempEL.Epoint, ENsupport.Supportinclination, 0.5)
        End If
    End Sub

    Private Sub SupportAreaPaintLoad(ByRef e As System.Windows.Forms.PaintEventArgs)
        Try
            For Each itm In WA2dt.Mem(_memberBindingSource.IndexOf(_memberBindingSource.Current)).Pload
                Dim PPt1 As New Point(((Math.Cos(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.plocation) * (TempEL.CurrentLength / TempEL.ActualLength)))), ((Math.Sin(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.plocation) * (TempEL.CurrentLength / TempEL.ActualLength)))))
                Dim PPt2 As New Point
                PPt2 = If(itm.MAlign = True, New Point(PPt1.X - (Math.Sin(TempEL.inclination) * (40 * (-itm.pload / TempEL.maxload))), PPt1.Y + (Math.Cos(TempEL.inclination) * (40 * (-itm.pload / TempEL.maxload)))), If(itm.HAlign = True, New Point(PPt1.X + (40 * (itm.pload / TempEL.maxload)), PPt1.Y), New Point(PPt1.X, PPt1.Y + (40 * (-itm.pload / TempEL.maxload)))))
                Dim loadpen As New System.Drawing.Pen(Color.Green, 2)
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)

                e.Graphics.DrawLine(loadpen, TempEL.Mpoint.X - PPt1.X, TempEL.Mpoint.Y - PPt1.Y, TempEL.Mpoint.X - PPt2.X, TempEL.Mpoint.Y - PPt2.Y)
                e.Graphics.DrawString(Math.Abs(itm.pload), New Font("Verdana", (8)), New Pen(Color.Green, 2).Brush, TempEL.Mpoint.X - PPt2.X + (2), TempEL.Mpoint.Y - PPt2.Y + (2))
            Next
            For Each itm In WA2dt.Mem(_memberBindingSource.IndexOf(_memberBindingSource.Current)).Uload
                Dim u1Pt1 As New Point(((Math.Cos(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.ulocation1) * (TempEL.CurrentLength / TempEL.ActualLength)))), ((Math.Sin(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.ulocation1) * (TempEL.CurrentLength / TempEL.ActualLength)))))
                Dim u1Pt2 As New Point
                Dim u2Pt1 As New Point(((Math.Cos(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.ulocation2) * (TempEL.CurrentLength / TempEL.ActualLength)))), ((Math.Sin(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.ulocation2) * (TempEL.CurrentLength / TempEL.ActualLength)))))
                Dim u2Pt2 As New Point
                u1Pt2 = If(itm.MAlign = True, New Point(u1Pt1.X - (Math.Sin(TempEL.inclination) * (40 * (-itm.uload1 / TempEL.maxload))), u1Pt1.Y + (Math.Cos(TempEL.inclination) * (40 * (-itm.uload1 / TempEL.maxload)))), If(itm.HAlign = True, New Point(u1Pt1.X + (40 * (itm.uload1 / TempEL.maxload)), u1Pt1.Y), New Point(u1Pt1.X, u1Pt1.Y + (40 * (-itm.uload1 / TempEL.maxload)))))
                u2Pt2 = If(itm.MAlign = True, New Point(u2Pt1.X - (Math.Sin(TempEL.inclination) * (40 * (-itm.uload2 / TempEL.maxload))), u2Pt1.Y + (Math.Cos(TempEL.inclination) * (40 * (-itm.uload2 / TempEL.maxload)))), If(itm.HAlign = True, New Point(u2Pt1.X + (40 * (itm.uload2 / TempEL.maxload)), u2Pt1.Y), New Point(u2Pt1.X, u2Pt1.Y + (40 * (-itm.uload2 / TempEL.maxload)))))
                Dim loadpen As New System.Drawing.Pen(Color.DeepPink, 2)
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)

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
            For Each itm In WA2dt.Mem(_memberBindingSource.IndexOf(_memberBindingSource.Current)).Mload
                Dim MPt1 As New Point(((Math.Cos(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.mlocation) * (TempEL.CurrentLength / TempEL.ActualLength)))), ((Math.Sin(TempEL.inclination) * (((TempEL.ActualLength / 2) - itm.mlocation) * (TempEL.CurrentLength / TempEL.ActualLength)))))
                Dim loadpen As New System.Drawing.Pen(Color.Orange, 2)
                loadpen.CustomStartCap = If(itm.mAnticlockwise = True, New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5), New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0))
                loadpen.CustomEndCap = If(itm.mClockwise = True, New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5), New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0))
                e.Graphics.DrawArc(loadpen, TempEL.Mpoint.X - MPt1.X - 15, TempEL.Mpoint.Y - MPt1.Y - 15, 30, 30, 360, 270)
                e.Graphics.DrawString(Math.Abs(itm.mload), New Font("Verdana", (8)), loadpen.Brush, TempEL.Mpoint.X - MPt1.X - 15, TempEL.Mpoint.Y - MPt1.Y + 15)
            Next
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Support Pic Paint Events"
    Private Sub SupportPicPaintPinJoint(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double)
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), M.X - 2, M.Y - 2, 4, 4)
    End Sub

    Private Sub SupportPicPaintRigidJoint(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double)
        e.Graphics.DrawRectangle(New Pen(OptionD.Nc, 2), M.X - 2, M.Y - 2, 4, 4)
    End Sub

    Private Sub SupportPicPaintPinSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
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

    Private Sub SupportPicPaintPinRollerSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))

    End Sub

    Private Sub SupportPicPaintFixedRollerSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
        '---- Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((0 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((0 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((0 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((0 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZMfactor)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))

    End Sub

    Private Sub SupportPicPaintFixedSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '---- Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)))

    End Sub

    Private Sub SupportPicPaintSpringSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
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

#Region "Modifying E A & I Events"
    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked = True Then
            MDIMain.Nappdefaults.defaultPJ = True
            MDIMain.Nappdefaults.defaultRJ = False
        ElseIf RadioButton2.Checked = True Then
            MDIMain.Nappdefaults.defaultRJ = True
            MDIMain.Nappdefaults.defaultPJ = False
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        '---- Set Default
        If Val(TextBox2.Text) <> 0 And Val(TextBox3.Text) <> 0 And Val(TextBox4.Text) <> 0 Then
            MDIMain.Nappdefaults.defaultE = Val(TextBox2.Text)
            MDIMain.Nappdefaults.defaultA = Val(TextBox3.Text)
            MDIMain.Nappdefaults.defaultI = Val(TextBox4.Text)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Val(TextBox2.Text) <> 0 And Val(TextBox3.Text) <> 0 And Val(TextBox4.Text) <> 0 Then
            WA2dt.Mem(_memberBindingSource.IndexOf(_memberBindingSource.Current)).E = Val(TextBox2.Text)
            WA2dt.Mem(_memberBindingSource.IndexOf(_memberBindingSource.Current)).A = Val(TextBox3.Text)
            WA2dt.Mem(_memberBindingSource.IndexOf(_memberBindingSource.Current)).I = Val(TextBox4.Text)
            _memberBindingSource.ResetBindings(True)
        End If
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If IsNumeric(TextBox2.Text) Then

        Else
            TextBox2.Text = ""
        End If
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        If IsNumeric(TextBox3.Text) Then

        Else
            TextBox3.Text = ""
        End If
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        If IsNumeric(TextBox4.Text) Then

        Else
            TextBox4.Text = ""
        End If
    End Sub
#End Region

    Private Sub Member_Properties_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        WA2dt.selLine.Clear()
        MDIMain._DMCMenable()
        WA2dt.MainPic.Refresh()
    End Sub

    Private Sub FixMAXLoad()
        Try
            TempEL.maxload = 0
            For Each itm In WA2dt.Mem(_memberBindingSource.IndexOf(_memberBindingSource.Current)).Pload
                If Math.Abs(itm.pload) > TempEL.maxload Then
                    TempEL.maxload = Math.Abs(itm.pload)
                End If
            Next
            For Each itm In WA2dt.Mem(_memberBindingSource.IndexOf(_memberBindingSource.Current)).Uload
                If Math.Abs(itm.uload1) > TempEL.maxload Then
                    TempEL.maxload = Math.Abs(itm.uload1)
                End If
                If Math.Abs(itm.uload2) > TempEL.maxload Then
                    TempEL.maxload = Math.Abs(itm.uload2)
                End If
            Next
            _SupportPic.Refresh()
        Catch ex As Exception

        End Try
    End Sub

End Class