<Serializable()> _
Public Class Line2DT
    Dim _SN As Node
    Dim _EN As Node
    Dim _E As Double
    Dim _I As Double
    Dim _A As Double
    Dim _Length As Double
    Dim _Theta As Double
    Dim _Inclination As Double
    Dim _Lcos As Double
    Dim _Msin As Double
    Dim _pload As New List(Of P)
    Dim _uload As New List(Of U)
    Dim _mload As New List(Of M)


#Region "Get Set Property"
    Public Property Length() As Double
        Get
            Return _Length
        End Get
        Set(ByVal value As Double)
            _Length = value
        End Set
    End Property

    '-Theta is the property used for analysis purpose - Normal cartetian coordinate system
    Public ReadOnly Property Theta() As Double
        Get
            Return _Theta
        End Get
    End Property

    Public ReadOnly Property Inclination() As Double
        Get
            Return _Inclination
        End Get
    End Property

    Public Property SN() As Node '---Have to change to Readonly property
        Get
            Return _SN
        End Get
        Set(ByVal value As Node)
            '_SN = value
        End Set
    End Property

    Public Property EN() As Node '---Have to change to Readonly property
        Get
            Return _EN
        End Get
        Set(ByVal value As Node)
            '_EN = value
        End Set
    End Property

    Public Property Lcos() As Double
        Get
            Return _Lcos
        End Get
        Set(ByVal value As Double)
            '----
        End Set
    End Property '---Have to change to Readonly property

    Public Property Msin() As Double
        Get
            Return _Msin
        End Get
        Set(ByVal value As Double)
            '----
        End Set
    End Property

    Public Property Pload() As List(Of P)
        Get
            Return _pload
        End Get
        Set(ByVal value As List(Of P))
            value = _pload
        End Set
    End Property

    Public Property Uload() As List(Of U)
        Get
            Return _uload
        End Get
        Set(ByVal value As List(Of U))
            value = _uload
        End Set
    End Property

    Public Property Mload() As List(Of M)
        Get
            Return _mload
        End Get
        Set(ByVal value As List(Of M))
            value = _mload
        End Set
    End Property

    Public Property E() As Double
        Get
            Return _E
        End Get
        Set(ByVal value As Double)
            _E = value
        End Set
    End Property

    Public Property A() As Double
        Get
            Return _A
        End Get
        Set(ByVal value As Double)
            _A = value
        End Set
    End Property

    Public Property I() As Double
        Get
            Return _I
        End Get
        Set(ByVal value As Double)
            _I = value
        End Set
    End Property
#End Region

#Region "Structure"
    <Serializable()> _
    Public Class Node
        Private _Coord As System.Drawing.Point
        Private _Support As SupportCondition
        Private _index As Integer

        Public Property index() As Integer
            Get
                Return _index
            End Get
            Set(ByVal value As Integer)
                '_index = value
            End Set
        End Property

        Public Property Coord() As Point
            Get
                Return _Coord
            End Get
            Set(ByVal value As Point)

            End Set
        End Property

        Public Property Support() As SupportCondition
            Get
                Return _Support
            End Get
            Set(ByVal value As SupportCondition)
                _Support = value
            End Set
        End Property

        <Serializable()> _
        Public Structure SupportCondition
            Private _PJ As Boolean
            Private _RJ As Boolean
            Private _PS As Boolean
            Private _PRS As Boolean
            Private _FRS As Boolean
            Private _FS As Boolean
            Private _SS As Boolean
            Private _settlementdx As Double
            Private _settlementdy As Double
            Private _stiffnessK As Double
            Private _supportinclination As Double

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

            Public Property settlementdx() As Double
                Get
                    Return _settlementdx
                End Get
                Set(ByVal value As Double)
                    _settlementdx = value
                End Set
            End Property

            Public Property settlementdy() As Double
                Get
                    Return _settlementdy
                End Get
                Set(ByVal value As Double)
                    _settlementdy = value
                End Set
            End Property

            Public Property stiffnessK() As Double
                Get
                    Return _stiffnessK
                End Get
                Set(ByVal value As Double)
                    _stiffnessK = value
                End Set
            End Property

            Public Property supportinclination() As Double
                Get
                    Return _supportinclination
                End Get
                Set(ByVal value As Double)
                    _supportinclination = value
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
                    _supportinclination = supincln
                    _settlementdx = If(dx <> 0, dx, Nothing)
                    _settlementdy = If(dy <> 0, dy, Nothing)
                ElseIf _SS = True Then
                    _supportinclination = supincln
                    _stiffnessK = If(K <> 0, K, Nothing)
                End If
            End Sub
        End Structure

        Public Sub New(ByVal LCoord As Point, Optional ByVal ind As Integer = -1)
            _Coord = New Point(LCoord.X, LCoord.Y)
            _index = ind
        End Sub

        Public Sub ModifyIndex(ByVal I As Integer)
            _index = I
        End Sub

        'Public Sub New(ByVal LCoord As Point, ByVal TSupp As SupportCondition)
        '    _Coord = New Point(LCoord.X, LCoord.Y)
        '    _Support = TSupp
        'End Sub
    End Class

    <Serializable()> _
    Public Structure P
        Private _pload As Double
        Private _plocation As Double
        Private _HAlign As Boolean
        Private _VAlign As Boolean
        Private _MAlign As Boolean
        Private _pcoordS As Point
        Private _pcoordE As Point
        Private _rect As System.Drawing.Rectangle

        Public Property pload() As Double
            Get
                Return _pload
            End Get
            Set(ByVal value As Double)
                _pload = value
            End Set
        End Property

        Public Property plocation() As Double
            Get
                Return _plocation
            End Get
            Set(ByVal value As Double)
                _plocation = value
            End Set
        End Property

        Public Property PCoordS() As Point
            Get
                Return _pcoordS
            End Get
            Set(ByVal value As Point)
                _pcoordS = value
            End Set
        End Property

        Public Property PCoordE() As Point
            Get
                Return _pcoordE
            End Get
            Set(ByVal value As Point)
                _pcoordE = value
            End Set
        End Property

        Public Property MAlign() As Boolean
            Get
                Return _MAlign
            End Get
            Set(ByVal value As Boolean)
                _MAlign = value
            End Set
        End Property

        Public Property HAlign() As Boolean
            Get
                Return _HAlign
            End Get
            Set(ByVal value As Boolean)
                _HAlign = value
            End Set
        End Property

        Public Property VAlign() As Boolean
            Get
                Return _VAlign
            End Get
            Set(ByVal value As Boolean)
                _VAlign = value
            End Set
        End Property

        Public Property rect() As System.Drawing.Rectangle
            Get
                Return _rect
            End Get
            Set(ByVal value As System.Drawing.Rectangle)
                _rect = value
            End Set
        End Property

        Public Sub New(ByVal load As Double, ByVal location As Double, ByVal PS As Point, ByVal PE As Point, ByVal PAlign As Boolean, ByVal PHoriz As Boolean, ByVal PVert As Boolean)
            _pload = load
            _plocation = location
            _pcoordS = PS
            _pcoordE = PE
            _MAlign = PAlign
            _HAlign = PHoriz
            _VAlign = PVert
        End Sub
    End Structure

    <Serializable()> _
    Public Structure U
        Private _uload1 As Double
        Private _uload2 As Double
        Private _ulocation1 As Double
        Private _ulocation2 As Double
        Private _uCoordS1 As Point
        Private _uCoordE1 As Point
        Private _uCoordS2 As Point
        Private _uCoordE2 As Point
        Private _HAlign As Boolean
        Private _VAlign As Boolean
        Private _MAlign As Boolean
        Private _rect As System.Drawing.Rectangle

        Public Property uload1() As Double
            Get
                Return _uload1
            End Get
            Set(ByVal value As Double)
                _uload1 = value
            End Set
        End Property

        Public Property uload2() As Double
            Get
                Return _uload2
            End Get
            Set(ByVal value As Double)
                _uload2 = value
            End Set
        End Property

        Public Property ulocation1() As Double
            Get
                Return _ulocation1
            End Get
            Set(ByVal value As Double)
                _ulocation1 = value
            End Set
        End Property

        Public Property ulocation2() As Double
            Get
                Return _ulocation2
            End Get
            Set(ByVal value As Double)
                _ulocation2 = value
            End Set
        End Property

        Public Property uCoordS1() As Point
            Get
                Return _uCoordS1
            End Get
            Set(ByVal value As Point)
                _uCoordS1 = value
            End Set
        End Property

        Public Property uCoordE1() As Point
            Get
                Return _uCoordE1
            End Get
            Set(ByVal value As Point)
                _uCoordE1 = value
            End Set
        End Property

        Public Property uCoordS2() As Point
            Get
                Return _uCoordS2
            End Get
            Set(ByVal value As Point)
                _uCoordS2 = value
            End Set
        End Property

        Public Property uCoordE2() As Point
            Get
                Return _uCoordE2
            End Get
            Set(ByVal value As Point)
                _uCoordE2 = value
            End Set
        End Property

        Public Property MAlign() As Boolean
            Get
                Return _MAlign
            End Get
            Set(ByVal value As Boolean)
                _MAlign = value
            End Set
        End Property

        Public Property HAlign() As Boolean
            Get
                Return _HAlign
            End Get
            Set(ByVal value As Boolean)
                _HAlign = value
            End Set
        End Property

        Public Property VAlign() As Boolean
            Get
                Return _VAlign
            End Get
            Set(ByVal value As Boolean)
                _VAlign = value
            End Set
        End Property

        Public Property rect() As System.Drawing.Rectangle
            Get
                Return _rect
            End Get
            Set(ByVal value As System.Drawing.Rectangle)
                _rect = value
            End Set
        End Property

        Public Sub New(ByVal load1 As Double, ByVal load2 As Double, ByVal location1 As Double, ByVal location2 As Double, ByVal US1 As Point, ByVal UE1 As Point, ByVal US2 As Point, ByVal UE2 As Point, ByVal UAlign As Boolean, ByVal UHoriz As Boolean, ByVal UVert As Boolean)
            _uload1 = load1
            _uload2 = load2
            _ulocation1 = location1
            _ulocation2 = location2
            _uCoordS1 = US1
            _uCoordE1 = UE1
            _uCoordS2 = US2
            _uCoordE2 = UE2
            _MAlign = UAlign
            _HAlign = UHoriz
            _VAlign = UVert
        End Sub
    End Structure

    <Serializable()> _
    Public Structure M
        Private _mload As Double
        Private _mlocation As Double
        Private _mCoord As Point
        Private _mClockwise As Boolean
        Private _mAnticlockwise As Boolean
        Private _rect As System.Drawing.Rectangle

        Public Property mload() As Double
            Get
                Return _mload
            End Get
            Set(ByVal value As Double)
                _mload = value
            End Set
        End Property

        Public Property mlocation() As Double
            Get
                Return _mlocation
            End Get
            Set(ByVal value As Double)
                _mlocation = value
            End Set
        End Property

        Public Property mCoord() As Point
            Get
                Return _mCoord
            End Get
            Set(ByVal value As Point)
                _mCoord = value
            End Set
        End Property

        Public Property mClockwise() As Boolean
            Get
                Return _mClockwise
            End Get
            Set(ByVal value As Boolean)
                _mClockwise = value
            End Set
        End Property

        Public Property mAnticlockwise() As Boolean
            Get
                Return _mAnticlockwise
            End Get
            Set(ByVal value As Boolean)
                _mAnticlockwise = value
            End Set
        End Property

        Public Property rect() As System.Drawing.Rectangle
            Get
                Return _rect
            End Get
            Set(ByVal value As System.Drawing.Rectangle)
                _rect = value
            End Set
        End Property

        Public Sub New(ByVal load As Double, ByVal location As Double, ByVal S As Point, ByVal clockwise As Boolean, ByVal anticlockwise As Boolean)
            _mload = load
            _mlocation = location
            _mCoord = S
            _mClockwise = clockwise
            _mAnticlockwise = anticlockwise
        End Sub
    End Structure

#End Region

#Region "Constructor"
    Public Sub New(ByVal S As Node, ByVal E As Node, ByVal PL As List(Of P), ByVal UL As List(Of U), ByVal ML As List(Of M))
        AttachNode(S, _SN)
        AttachNode(E, _EN)

        '--- To make the allignment relative to the load
        Dim _TN As Node
        If _SN.index > _EN.index Then
            _TN = _SN
            _SN = _EN
            _EN = _TN
        End If


        _Length = Math.Round( _
                        Math.Sqrt( _
                            Math.Pow((SN.Coord.X - EN.Coord.X), 2) + _
                            Math.Pow((SN.Coord.Y - EN.Coord.Y), 2)), 1) / _
                            MDIMain.Nappdefaults.defaultScaleFactor
        _Lcos = (EN.Coord.X - SN.Coord.X) _
                        / Math.Round( _
                                Math.Sqrt( _
                                    Math.Pow((SN.Coord.X - EN.Coord.X), 2) + _
                                    Math.Pow((SN.Coord.Y - EN.Coord.Y), 2)))
        _Msin = (SN.Coord.Y - EN.Coord.Y) _
                        / Math.Round( _
                                Math.Sqrt( _
                                    Math.Pow((SN.Coord.X - EN.Coord.X), 2) + _
                                    Math.Pow((SN.Coord.Y - EN.Coord.Y), 2)))
        FixInclination(_Theta, _Inclination)
        FixLoad(PL, UL, ML)
        'FixSupport(_SN, _EN)
        _E = MDIMain.Nappdefaults.defaultE
        _A = MDIMain.Nappdefaults.defaultA
        _I = MDIMain.Nappdefaults.defaultI
    End Sub

    Private Sub FixInclination(ByRef _TH As Double, ByRef _Incln As Double)
        'Dim LC, MS As Double
        'If Math.Abs(EN.Coord.X - SN.Coord.X) <= Math.Abs(EN.Coord.Y - SN.Coord.Y) Then
        '    LC = (EN.Coord.X - SN.Coord.X) / _
        '            (Math.Round( _
        '                Math.Sqrt( _
        '                    Math.Pow((SN.Coord.X - EN.Coord.X), 2) + _
        '                    Math.Pow((SN.Coord.Y - EN.Coord.Y), 2))))
        '    _Incln = If((EN.Coord.Y - SN.Coord.Y) < 1, Math.Acos(LC) * -1, Math.Acos(LC))
        '    _TH = (_Incln * 180) / Math.PI
        'Else
        '    MS = (EN.Coord.Y - SN.Coord.Y) / _
        '            (Math.Round( _
        '                Math.Sqrt( _
        '                    Math.Pow((SN.Coord.X - EN.Coord.X), 2) + _
        '                    Math.Pow((SN.Coord.Y - EN.Coord.Y), 2))))
        '    _Incln = If((EN.Coord.X - SN.Coord.X) < 1, Math.Asin(MS) * -1, Math.Asin(MS))
        '    _TH = (_Incln * 180) / Math.PI
        'End If

        Dim b As Double = SN.Coord.Y - EN.Coord.Y
        Dim a As Double = EN.Coord.X - SN.Coord.X
        _Incln = -1 * Math.Atan2(b, a)
        _TH = (_Incln * 180) / Math.PI
    End Sub

    Private Sub FixLoad(ByVal PL As List(Of P), ByVal UL As List(Of U), ByVal ML As List(Of M))
        Dim Mpt As New Point((SN.Coord.X + EN.Coord.X) / 2, (SN.Coord.Y + EN.Coord.Y) / 2)
        '-------------Point Load
        For Each itm In PL
            Dim PPt1 As New Point( _
                            Mpt.X + _
                                (Math.Cos(_Inclination) * _
                                ((itm.plocation - (_Length / 2)) * _
                                MDIMain.Nappdefaults.defaultScaleFactor)), _
                            Mpt.Y + _
                                (Math.Sin(_Inclination) * _
                                ((itm.plocation - (_Length / 2)) * _
                                 MDIMain.Nappdefaults.defaultScaleFactor)))
            Dim PPt2 As New Point
            PPt2 = If(itm.MAlign = True, _
                        New Point( _
                                    PPt1.X - _
                                        (Math.Sin(_Inclination) * _
                                        (WA2dt.Loadhtfactor * _
                                        (itm.pload / WA2dt.Maxload))), _
                                    PPt1.Y + _
                                    (Math.Cos(_Inclination) * _
                                    (WA2dt.Loadhtfactor * _
                                    (itm.pload / WA2dt.Maxload)))), _
                        If(itm.HAlign = True, _
                                    New Point( _
                                        PPt1.X + _
                                            (WA2dt.Loadhtfactor * _
                                            (-itm.pload / WA2dt.Maxload)), _
                                        PPt1.Y), _
                                    New Point( _
                                        PPt1.X, _
                                        PPt1.Y + _
                                            (WA2dt.Loadhtfactor * _
                                            (itm.pload / WA2dt.Maxload)))))
            Pload.Add( _
                New FRAMEANS2D.Line2DT.P( _
                    itm.pload, _
                    itm.plocation, _
                    PPt1, _
                    PPt2, _
                    itm.MAlign, _
                    itm.HAlign, _
                    itm.VAlign))
        Next
        '-----------Uniformly Varrying Load
        For Each itm In UL
            Dim U1Pt1 As New Point( _
                                Mpt.X + _
                                    (Math.Cos(_Inclination) * _
                                    ((itm.ulocation1 - (_Length / 2)) * _
                                    MDIMain.Nappdefaults.defaultScaleFactor)), _
                                Mpt.Y + _
                                    (Math.Sin(_Inclination) * _
                                    ((itm.ulocation1 - (_Length / 2)) * _
                                     MDIMain.Nappdefaults.defaultScaleFactor)))
            Dim U2Pt1 As New Point( _
                                Mpt.X + _
                                    (Math.Cos(_Inclination) * _
                                    ((itm.ulocation2 - (_Length / 2)) * _
                                     MDIMain.Nappdefaults.defaultScaleFactor)), _
                                Mpt.Y + _
                                    (Math.Sin(_Inclination) * _
                                    ((itm.ulocation2 - (_Length / 2)) * _
                                     MDIMain.Nappdefaults.defaultScaleFactor)))

            Dim U1Pt2 As Point
            Dim U2Pt2 As Point
            U1Pt2 = If(itm.MAlign = True, _
                            New Point( _
                                U1Pt1.X - _
                                    (Math.Sin(_Inclination) * _
                                    (WA2dt.Loadhtfactor * _
                                    (itm.uload1 / WA2dt.Maxload))), _
                                U1Pt1.Y + _
                                    (Math.Cos(_Inclination) * _
                                    (WA2dt.Loadhtfactor * _
                                    (itm.uload1 / WA2dt.Maxload)))), _
                            If(itm.HAlign = True, _
                                    New Point(U1Pt1.X + _
                                                (WA2dt.Loadhtfactor * _
                                                (-itm.uload1 / WA2dt.Maxload)), _
                                              U1Pt1.Y), _
                                    New Point(U1Pt1.X, _
                                              U1Pt1.Y + _
                                                (WA2dt.Loadhtfactor * _
                                                (itm.uload1 / WA2dt.Maxload)))))
            U2Pt2 = If(itm.MAlign = True, _
                            New Point( _
                                U2Pt1.X - _
                                    (Math.Sin(_Inclination) * _
                                    (WA2dt.Loadhtfactor * _
                                    (itm.uload2 / WA2dt.Maxload))), _
                                U2Pt1.Y + _
                                    (Math.Cos(_Inclination) * _
                                    (WA2dt.Loadhtfactor * _
                                    (itm.uload2 / WA2dt.Maxload)))), _
                            If(itm.HAlign = True, _
                                    New Point( _
                                        U2Pt1.X + _
                                            (WA2dt.Loadhtfactor * _
                                            (-itm.uload2 / WA2dt.Maxload)), _
                                        U2Pt1.Y), _
                                    New Point( _
                                        U2Pt1.X, _
                                        U2Pt1.Y + _
                                            (WA2dt.Loadhtfactor * _
                                            (itm.uload2 / WA2dt.Maxload)))))

            Uload.Add( _
                    New FRAMEANS2D.Line2DT.U( _
                            itm.uload1, _
                            itm.uload2, _
                            itm.ulocation1, _
                            itm.ulocation2, _
                            U1Pt1, _
                            U1Pt2, _
                            U2Pt1, _
                            U2Pt2, _
                            itm.MAlign, _
                            itm.HAlign, _
                            itm.VAlign))
        Next
        '------------------Moment
        For Each itm In ML
            Dim MPt1 As New Point( _
                                Mpt.X + _
                                    (Math.Cos(_Inclination) * _
                                    ((itm.mlocation - (_Length / 2)) * _
                                     MDIMain.Nappdefaults.defaultScaleFactor)), _
                                Mpt.Y + _
                                    (Math.Sin(_Inclination) * _
                                    ((itm.mlocation - (_Length / 2)) * _
                                     MDIMain.Nappdefaults.defaultScaleFactor)))
            Mload.Add( _
                    New FRAMEANS2D.Line2DT.M( _
                            itm.mload, _
                            itm.mlocation, _
                            MPt1, _
                            itm.mClockwise, _
                            itm.mAnticlockwise))
        Next
    End Sub

    Private Sub AttachNode(ByRef Pt As Node, ByRef Nd As Node)
        WA2dt.PD.Her = New Node(Pt.Coord)
        If WA2dt.Bob.Exists(WA2dt.PD.ExistingNodePD) = True Then
            Nd = WA2dt.Bob.Find(WA2dt.PD.ExistingNodePD)
            For Each mem In WA2dt.Mem
                If Nd.index = mem.SN.index Then
                    Nd.Support = mem.SN.Support
                    Exit For
                End If
                If Nd.index = mem.EN.index Then
                    Nd.Support = mem.EN.Support
                    Exit For
                End If
            Next
        Else
            WA2dt.Bob.Add(New Node(Pt.Coord, WA2dt.Bob.Count))
            Nd = WA2dt.Bob(WA2dt.Bob.Count - 1)
            Nd.Support = New Line2DT.Node.SupportCondition( _
                                MDIMain.Nappdefaults.defaultPJ, _
                                MDIMain.Nappdefaults.defaultRJ, _
                                False, _
                                False, _
                                False, _
                                False, _
                                False, _
                                Nothing, _
                                Nothing, _
                                Nothing, _
                                Nothing)
        End If

    End Sub

    Private Sub FixSupport(ByRef SN As Node, ByRef EN As Node)
        WA2dt.PD.Her = New Node(SN.Coord)
        If WA2dt.Bob.Exists(WA2dt.PD.ExistingNodePD) = True Then
            Dim E As Line2DT.Node = WA2dt.Bob.Find(WA2dt.PD.ExistingNodePD)
            SN.Support = New Line2DT.Node.SupportCondition( _
                                E.Support.PJ, _
                                E.Support.RJ, _
                                E.Support.PS, _
                                E.Support.PRS, _
                                E.Support.FRS, _
                                E.Support.FS, _
                                E.Support.SS, _
                                E.Support.supportinclination, _
                                E.Support.settlementdx, _
                                E.Support.settlementdy, _
                                E.Support.stiffnessK)
        Else
            SN.Support = New Line2DT.Node.SupportCondition( _
                                MDIMain.Nappdefaults.defaultPJ, _
                                MDIMain.Nappdefaults.defaultRJ, _
                                False, _
                                False, _
                                False, _
                                False, _
                                False, _
                                Nothing, _
                                Nothing, _
                                Nothing, _
                                Nothing)
        End If

        WA2dt.PD.Her = New Node(EN.Coord)
        If WA2dt.Bob.Exists(WA2dt.PD.ExistingNodePD) = True Then
            Dim E As Line2DT.Node = WA2dt.Bob.Find(WA2dt.PD.ExistingNodePD)
            EN.Support = New Line2DT.Node.SupportCondition( _
                                E.Support.PJ, _
                                E.Support.RJ, _
                                E.Support.PS, _
                                E.Support.PRS, _
                                E.Support.FRS, _
                                E.Support.FS, _
                                E.Support.SS, _
                                E.Support.supportinclination, _
                                E.Support.settlementdx, _
                                E.Support.settlementdy, _
                                E.Support.stiffnessK)
        Else
            EN.Support = New Line2DT.Node.SupportCondition( _
                                MDIMain.Nappdefaults.defaultPJ, _
                                MDIMain.Nappdefaults.defaultRJ, _
                                False, _
                                False, _
                                False, _
                                False, _
                                False, _
                                Nothing, _
                                Nothing, _
                                Nothing, _
                                Nothing)
        End If

        '---- Modifying Other Bob
        For Each Nodes In WA2dt.Bob
            If Nodes.Coord.Equals(SN) Then
                WA2dt.Bob(WA2dt.Bob.IndexOf(Nodes)).Support = SN.Support
                Exit For
            End If
            If Nodes.Coord.Equals(EN) Then
                WA2dt.Bob(WA2dt.Bob.IndexOf(Nodes)).Support = EN.Support
                Exit For
            End If
        Next

    End Sub '-----Have to Delete this

    Public Sub ReviseNodes()

    End Sub
#End Region
End Class
