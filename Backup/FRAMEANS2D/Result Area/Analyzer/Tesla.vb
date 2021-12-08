Imports System.Math
Public Class Tesla
    Shared _Failed As Boolean = False
    Dim _updater As AnalyzerUpdate
    Dim _updateStr As String = ""
    Dim _ResultMem As List(Of AnalysisResultMem)
    Dim _ResultNode As List(Of AnalysisResultNode)
    Shared LoadScale(3) As Double '0 - Max Shear, 1 - Max BM, 2 - Max Def, 3 - Max Axial Load
    Shared _dx As Double

    Public ReadOnly Property MatrixUpdater() As AnalyzerUpdate
        Get
            Return _updater
        End Get
    End Property

    Public ReadOnly Property ResultMem() As List(Of AnalysisResultMem)
        Get
            Return _ResultMem
        End Get
    End Property

    Public ReadOnly Property ResultNode() As List(Of AnalysisResultNode)
        Get
            Return _ResultNode
        End Get
    End Property

    Public ReadOnly Property Failed() As Boolean
        Get
            Return _Failed
        End Get
    End Property

#Region "Material Holding Class"
    Public Class AnalysisResultNode
        Public _NodeNo As Integer
        Public HForce As Double
        Public VForce As Double
        Public Moment As Double
        Public GlobalX As Double
        Public GlobalY As Double
        Public GlobalRotation As Double
    End Class

    Public Class AnalysisResultMem
        Public _DegreeOFFreedomMatrix(5) As Double
        Public _SettlementMatrix(5) As Double
        Public _LLoadVectorMatrix(5) As Double
        Public _GLoadVectorMatrix(5) As Double
        Public _LElementStiffnessMatrix(5, 5) As Double
        Public _GElementStiffnessMatrix(5, 5) As Double
        Public _LResultMatrix(5) As Double
        Public _GResultMatrix(5) As Double
        Dim _StartNodeNo As Integer
        Dim _EndNodeNo As Integer
        Dim _ActLCos As Double
        Dim _ActMsin As Double
        Dim _ResulCoords As New ResultDrawingCoords
        Public _inclination As Double
        Dim _Transformation_Mat(5, 5) As Double '-- Transformation matrix for support condition
        Dim _Transformation_MatT(5, 5) As Double
        'Dim _R As New ResultDrawingCoords

        Public ReadOnly Property ActLcos() As Double
            Get
                Return _ActLCos
            End Get
        End Property

        Public ReadOnly Property ActMsin() As Double
            Get
                Return _ActMsin
            End Get
        End Property

        Public Structure ResultDrawingCoords
            Public _BMPoints As List(Of Point)
            Public _BMMcPointsindex As List(Of Integer)
            Public _MemPoints As List(Of Point)
            Public _DEPoints As List(Of Point)
            Public _DEMcPointsindex As List(Of Integer)
            Public _SFPoints As List(Of Point)
            Public _SFMcPointsindex As List(Of Integer)
            Public _AXPoints As List(Of Point)
            Public _AXMcPointsindex As List(Of Integer)
            Public shearValues As List(Of Double)
            Public bendingValues As List(Of Double)
            Public deflectionValuesX As List(Of Double)
            Public deflectionValuesY As List(Of Double)
            Public beamValues As List(Of Double)
            Public axialForcevalues As List(Of Double)
        End Structure

        Public ReadOnly Property ResulCoords() As ResultDrawingCoords
            Get
                Return _ResulCoords
            End Get
        End Property

        Private ElDetails As Line2DT

        Public Property StartNodeNo() As Integer
            Get
                Return _StartNodeNo
            End Get
            Set(ByVal value As Integer)
                '-----------
            End Set
        End Property

        Public Property EndNodeNo() As Integer
            Get
                Return _EndNodeNo
            End Get
            Set(ByVal value As Integer)
                '-----------
            End Set
        End Property

        Public Sub New(ByVal EL As Line2DT)
            ElDetails = EL
            _ActLCos = (EL.EN.Coord.X - EL.SN.Coord.X) _
                        / Math.Round( _
                                Math.Sqrt( _
                                    Math.Pow((EL.SN.Coord.X - EL.EN.Coord.X), 2) + _
                                    Math.Pow((EL.SN.Coord.Y - EL.EN.Coord.Y), 2)))
            _ActMsin = (EL.SN.Coord.Y - EL.EN.Coord.Y) _
                        / Math.Round( _
                                Math.Sqrt( _
                                    Math.Pow((EL.SN.Coord.X - EL.EN.Coord.X), 2) + _
                                    Math.Pow((EL.SN.Coord.Y - EL.EN.Coord.Y), 2)))
            FixDegreeOfFreedom(EL)
            FixSupportInclnTransformationMatrix(EL)
            FixFER(EL)
            FixElementStiffness(EL)
            For Each Node In WA2dt.Bob
                If EL.SN.Coord.Equals(Node.Coord) Then
                    _StartNodeNo = WA2dt.Bob.IndexOf(Node)
                End If
                If EL.EN.Coord.Equals(Node.Coord) Then
                    _EndNodeNo = WA2dt.Bob.IndexOf(Node)
                End If
            Next
        End Sub

        Private Sub FixDegreeOfFreedom(ByRef EL As Line2DT)
            '-----------START NODE BOUNDARY CONDITION Free - 1; Fixed - 0
            If EL.SN.Support.PJ = True Then
                _DegreeOFFreedomMatrix(0) = 1
                _DegreeOFFreedomMatrix(1) = 1
                _DegreeOFFreedomMatrix(2) = 1

                _SettlementMatrix(0) = 0
                _SettlementMatrix(1) = 0
                _SettlementMatrix(2) = 0
            ElseIf EL.SN.Support.RJ = True Then
                _DegreeOFFreedomMatrix(0) = 1
                _DegreeOFFreedomMatrix(1) = 1
                _DegreeOFFreedomMatrix(2) = 1

                _SettlementMatrix(0) = 0
                _SettlementMatrix(1) = 0
                _SettlementMatrix(2) = 0
            ElseIf EL.SN.Support.PS = True Then
                _DegreeOFFreedomMatrix(0) = 0
                _DegreeOFFreedomMatrix(1) = 0
                _DegreeOFFreedomMatrix(2) = 1

                If EL.SN.Support.settlementdx <> 0 Then
                    _SettlementMatrix(0) = EL.SN.Support.settlementdx * 10 ^ -3
                Else
                    _SettlementMatrix(0) = 0
                End If
                If EL.SN.Support.settlementdy <> 0 Then
                    _SettlementMatrix(1) = EL.SN.Support.settlementdy * 10 ^ -3
                Else
                    _SettlementMatrix(1) = 0
                End If
                _SettlementMatrix(2) = 0
            ElseIf EL.SN.Support.PRS = True Then
                _DegreeOFFreedomMatrix(0) = 1
                _DegreeOFFreedomMatrix(1) = 0
                _DegreeOFFreedomMatrix(2) = 1

                _SettlementMatrix(0) = 0
                If EL.SN.Support.settlementdy <> 0 Then
                    _SettlementMatrix(1) = EL.SN.Support.settlementdy * 10 ^ -3
                Else
                    _SettlementMatrix(1) = 0
                End If
                _SettlementMatrix(2) = 0
            ElseIf EL.SN.Support.FRS = True Then
                _DegreeOFFreedomMatrix(0) = 1
                _DegreeOFFreedomMatrix(1) = 0
                _DegreeOFFreedomMatrix(2) = 0

                _SettlementMatrix(0) = 0
                If EL.SN.Support.settlementdy <> 0 Then
                    _SettlementMatrix(1) = EL.SN.Support.settlementdy * 10 ^ -3
                Else
                    _SettlementMatrix(1) = 0
                End If
                _SettlementMatrix(2) = 0
            ElseIf EL.SN.Support.FS = True Then
                _DegreeOFFreedomMatrix(0) = 0
                _DegreeOFFreedomMatrix(1) = 0
                _DegreeOFFreedomMatrix(2) = 0

                If EL.SN.Support.settlementdx <> 0 Then
                    _SettlementMatrix(0) = EL.SN.Support.settlementdx * 10 ^ -3
                Else
                    _SettlementMatrix(0) = 0
                End If
                If EL.SN.Support.settlementdy <> 0 Then
                    _SettlementMatrix(1) = EL.SN.Support.settlementdy * 10 ^ -3
                Else
                    _SettlementMatrix(1) = 0
                End If
                _SettlementMatrix(2) = 0
            ElseIf EL.SN.Support.SS = True Then
                _DegreeOFFreedomMatrix(0) = 1
                _DegreeOFFreedomMatrix(1) = 1
                _DegreeOFFreedomMatrix(2) = 1
            End If

            '-------------END NODE BOUNDARY CONDITION Free - 1; Fixed - 0
            If EL.EN.Support.PJ = True Then
                _DegreeOFFreedomMatrix(3) = 1
                _DegreeOFFreedomMatrix(4) = 1
                _DegreeOFFreedomMatrix(5) = 1

                _SettlementMatrix(3) = 0
                _SettlementMatrix(4) = 0
                _SettlementMatrix(5) = 0
            ElseIf EL.EN.Support.RJ = True Then
                _DegreeOFFreedomMatrix(3) = 1
                _DegreeOFFreedomMatrix(4) = 1
                _DegreeOFFreedomMatrix(5) = 1

                _SettlementMatrix(3) = 0
                _SettlementMatrix(4) = 0
                _SettlementMatrix(5) = 0
            ElseIf EL.EN.Support.PS = True Then
                _DegreeOFFreedomMatrix(3) = 0
                _DegreeOFFreedomMatrix(4) = 0
                _DegreeOFFreedomMatrix(5) = 1

                If EL.EN.Support.settlementdx <> 0 Then
                    _SettlementMatrix(3) = EL.EN.Support.settlementdx * 10 ^ -3
                Else
                    _SettlementMatrix(3) = 0
                End If
                If EL.EN.Support.settlementdy <> 0 Then
                    _SettlementMatrix(4) = EL.EN.Support.settlementdy * 10 ^ -3
                Else
                    _SettlementMatrix(4) = 0
                End If
                _SettlementMatrix(5) = 0
            ElseIf EL.EN.Support.PRS = True Then
                _DegreeOFFreedomMatrix(3) = 1
                _DegreeOFFreedomMatrix(4) = 0
                _DegreeOFFreedomMatrix(5) = 1

                _SettlementMatrix(3) = 0
                If EL.EN.Support.settlementdy <> 0 Then
                    _SettlementMatrix(4) = EL.EN.Support.settlementdy * 10 ^ -3
                Else
                    _SettlementMatrix(4) = 0
                End If
                _SettlementMatrix(5) = 0
            ElseIf EL.EN.Support.FRS = True Then
                _DegreeOFFreedomMatrix(3) = 1
                _DegreeOFFreedomMatrix(4) = 0
                _DegreeOFFreedomMatrix(5) = 0

                _SettlementMatrix(3) = 0
                If EL.EN.Support.settlementdy <> 0 Then
                    _SettlementMatrix(4) = EL.EN.Support.settlementdy * 10 ^ -3
                Else
                    _SettlementMatrix(4) = 0
                End If
                _SettlementMatrix(5) = 0
            ElseIf EL.EN.Support.FS = True Then
                _DegreeOFFreedomMatrix(3) = 0
                _DegreeOFFreedomMatrix(4) = 0
                _DegreeOFFreedomMatrix(5) = 0

                If EL.EN.Support.settlementdx <> 0 Then
                    _SettlementMatrix(3) = EL.EN.Support.settlementdx * 10 ^ -3
                Else
                    _SettlementMatrix(3) = 0
                End If
                If EL.EN.Support.settlementdy <> 0 Then
                    _SettlementMatrix(4) = EL.EN.Support.settlementdy * 10 ^ -3
                Else
                    _SettlementMatrix(4) = 0
                End If
                _SettlementMatrix(5) = 0
            ElseIf EL.EN.Support.SS = True Then
                _DegreeOFFreedomMatrix(3) = 1
                _DegreeOFFreedomMatrix(4) = 1
                _DegreeOFFreedomMatrix(5) = 1
            End If
        End Sub

        Private Sub FixSupportInclnTransformationMatrix(ByRef EL As Line2DT)
            '-----------START NODE BOUNDARY CONDITION _ TRANSFORMATION 
            If EL.SN.Support.PJ = True Or EL.SN.Support.RJ = True Then
                '-Transformation Matrix
                '-Row 1
                _Transformation_Mat(0, 0) = 1
                _Transformation_Mat(0, 1) = 0
                _Transformation_Mat(0, 2) = 0
                '-Row 2
                _Transformation_Mat(1, 0) = 0
                _Transformation_Mat(1, 1) = 1
                _Transformation_Mat(1, 2) = 0
                '-Row 3
                _Transformation_Mat(2, 0) = 0
                _Transformation_Mat(2, 1) = 0
                _Transformation_Mat(2, 2) = 1

                '-Transformation Matrix Transpose
                '-Row 1
                _Transformation_MatT(0, 0) = 1
                _Transformation_MatT(0, 1) = 0
                _Transformation_MatT(0, 2) = 0
                '-Row 2
                _Transformation_MatT(1, 0) = 0
                _Transformation_MatT(1, 1) = 1
                _Transformation_MatT(1, 2) = 0
                '-Row 3
                _Transformation_MatT(2, 0) = 0
                _Transformation_MatT(2, 1) = 0
                _Transformation_MatT(2, 2) = 1
            ElseIf EL.SN.Support.PS = True Or EL.SN.Support.PRS = True Or EL.SN.Support.SS = True Or EL.SN.Support.FRS = True Or EL.SN.Support.FS = True Then
                '-Transformation Matrix
                '-Row 1
                _Transformation_Mat(0, 0) = Math.Cos(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(0, 1) = Math.Sin(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(0, 2) = 0
                '-Row 2
                _Transformation_Mat(1, 0) = -Math.Sin(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(1, 1) = Math.Cos(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(1, 2) = 0
                '-Row 3
                _Transformation_Mat(2, 0) = 0
                _Transformation_Mat(2, 1) = 0
                _Transformation_Mat(2, 2) = 1

                '-Transformation Matrix Transpose
                '-Row 1
                _Transformation_MatT(0, 0) = Math.Cos(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(0, 1) = -Math.Sin(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(0, 2) = 0
                '-Row 2
                _Transformation_MatT(1, 0) = Math.Sin(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(1, 1) = Math.Cos(EL.SN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(1, 2) = 0
                '-Row 3
                _Transformation_MatT(2, 0) = 0
                _Transformation_MatT(2, 1) = 0
                _Transformation_MatT(2, 2) = 1
            End If
            '-------------END NODE BOUNDARY CONDITION _ TRANSFORMATION 
            If EL.EN.Support.PJ = True Or EL.EN.Support.RJ = True Then
                '-Transformation Matrix
                '-Row 1
                _Transformation_Mat(3, 3) = 1
                _Transformation_Mat(3, 4) = 0
                _Transformation_Mat(3, 5) = 0
                '-Row 2
                _Transformation_Mat(4, 3) = 0
                _Transformation_Mat(4, 4) = 1
                _Transformation_Mat(4, 5) = 0
                '-Row 3
                _Transformation_Mat(5, 3) = 0
                _Transformation_Mat(5, 4) = 0
                _Transformation_Mat(5, 5) = 1

                '-Transformation Matrix Transpose
                '-Row 1
                _Transformation_MatT(3, 3) = 1
                _Transformation_MatT(3, 4) = 0
                _Transformation_MatT(3, 5) = 0
                '-Row 2
                _Transformation_MatT(4, 3) = 0
                _Transformation_MatT(4, 4) = 1
                _Transformation_MatT(4, 5) = 0
                '-Row 3
                _Transformation_MatT(5, 3) = 0
                _Transformation_MatT(5, 4) = 0
                _Transformation_MatT(5, 5) = 1
            ElseIf EL.EN.Support.PS = True Or EL.EN.Support.PRS = True Or EL.EN.Support.SS = True Or EL.EN.Support.FRS = True Or EL.EN.Support.FS = True Then
                '-Transformation Matrix
                '-Row 1
                _Transformation_Mat(3, 3) = Math.Cos(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(3, 4) = Math.Sin(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(3, 5) = 0
                '-Row 2
                _Transformation_Mat(4, 3) = -Math.Sin(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(4, 4) = Math.Cos(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_Mat(4, 5) = 0
                '-Row 3
                _Transformation_Mat(5, 3) = 0
                _Transformation_Mat(5, 4) = 0
                _Transformation_Mat(5, 5) = 1

                '-Transformation Matrix Transpose
                '-Row 1
                _Transformation_MatT(3, 3) = Math.Cos(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(3, 4) = -Math.Sin(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(3, 5) = 0
                '-Row 2
                _Transformation_MatT(4, 3) = Math.Sin(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(4, 4) = Math.Cos(EL.EN.Support.supportinclination - (Math.PI / 2))
                _Transformation_MatT(4, 5) = 0
                '-Row 3
                _Transformation_MatT(5, 3) = 0
                _Transformation_MatT(5, 4) = 0
                _Transformation_MatT(5, 5) = 1
            End If
        End Sub

#Region " Fixed End Reaction - Force & Moment"
        Private Sub FixFER(ByVal El As Line2DT)
            '<------Finding reaction--------->
            Dim Ai, Fi, Mi, Aj, Fj, Mj As Double
            FER_Total(Ai, Fi, Mi, Aj, Fj, Mj, El)

            _LLoadVectorMatrix(0) = Ai
            _LLoadVectorMatrix(1) = -Fi 'Upward - ive
            _LLoadVectorMatrix(2) = -Mi 'Anti clockwise - ive
            _LLoadVectorMatrix(3) = Aj
            _LLoadVectorMatrix(4) = -Fj 'downward + ive
            _LLoadVectorMatrix(5) = -Mj 'clockwise + ive

            '--- Transformation for support inclination (Roller Supports)
            Dim GLoadTemp(5, 0) As Double
            GLoadTemp(0, 0) = (_ActLCos * _LLoadVectorMatrix(0)) - (_ActMsin * _LLoadVectorMatrix(1))
            GLoadTemp(1, 0) = (_ActMsin * _LLoadVectorMatrix(0)) + (_ActLCos * _LLoadVectorMatrix(1))
            GLoadTemp(2, 0) = _LLoadVectorMatrix(2)
            GLoadTemp(3, 0) = (_ActLCos * _LLoadVectorMatrix(3)) - (_ActMsin * _LLoadVectorMatrix(4))
            GLoadTemp(4, 0) = (_ActMsin * _LLoadVectorMatrix(3)) + (_ActLCos * _LLoadVectorMatrix(4))
            GLoadTemp(5, 0) = _LLoadVectorMatrix(5)

            Dim GLoadRslt(5, 0) As Double
            Matrix_Multiplication(_Transformation_MatT, 5, 5, GLoadTemp, 5, 0, GLoadRslt)

            _GLoadVectorMatrix(0) = GLoadRslt(0, 0)
            _GLoadVectorMatrix(1) = GLoadRslt(1, 0)
            _GLoadVectorMatrix(2) = GLoadRslt(2, 0)
            _GLoadVectorMatrix(3) = GLoadRslt(3, 0)
            _GLoadVectorMatrix(4) = GLoadRslt(4, 0)
            _GLoadVectorMatrix(5) = GLoadRslt(5, 0)
        End Sub

        Private Sub FER_Total(ByRef Ai As Double, ByRef Fi As Double, ByRef Mi As Double, ByRef Aj As Double, ByRef Fj As Double, ByRef Mj As Double, ByVal Fmem As Line2DT)
            Ai = 0
            Fi = 0
            Mi = 0
            Aj = 0
            Fj = 0
            Mj = 0

            Dim Tai, Taj, Tmi, Tmj, Tfi, Tfj As Double
            Dim Length As Double = Fmem.Length
            Dim K, a, b, c As Double
            For Each PL In Fmem.Pload
                Dim LoadVertical As Double
                Dim LoadHorizontal As Double
                If PL.MAlign = True Then
                    LoadVertical = PL.pload
                    LoadHorizontal = 0
                ElseIf PL.HAlign = True Then
                    LoadVertical = (PL.pload * Sin(Fmem.Inclination))
                    LoadHorizontal = (-PL.pload * Cos(Fmem.Inclination)) '--From Left to right
                ElseIf PL.VAlign = True Then
                    LoadVertical = (PL.pload * Cos(Fmem.Inclination))
                    LoadHorizontal = (PL.pload * Sin(Fmem.Inclination))
                End If

                K = LoadVertical
                a = PL.plocation
                b = Length - PL.plocation

                Tai = (LoadHorizontal * PL.plocation) / Length
                Taj = (LoadHorizontal * (Length - PL.plocation)) / Length
                Tmi = ((4 * deltaI_PointLoad(LoadVertical, b, Length) - 2 * deltaJ_PointLoad(LoadVertical, a, Length)) / Length)
                Tmj = ((2 * deltaI_PointLoad(LoadVertical, b, Length) - 4 * deltaJ_PointLoad(LoadVertical, a, Length)) / Length)
                Tfi = (((K * b) + Tmi + Tmj) / Length)
                Tfj = (((K * a) - Tmi - Tmj) / Length)

                Ai = Ai + Tai
                Mi = Mi + Tmi
                Fi = Fi + Tfi
                Aj = Aj + Taj
                Mj = Mj + Tmj
                Fj = Fj + Tfj
            Next
            For Each ML In Fmem.Mload
                K = ML.mload
                a = ML.mlocation
                b = Length - ML.mlocation

                Tmi = ((4 * deltaI_Momentoad(ML.mload, b, Length) - 2 * deltaJ_Momentoad(ML.mload, a, Length)) / Length)
                Tmj = ((2 * deltaI_Momentoad(ML.mload, b, Length) - 4 * deltaJ_Momentoad(ML.mload, a, Length)) / Length)
                Tfi = (((K * b) + Tmi + Tmj) / Length)
                Tfj = (((K * a) - Tmi - Tmj) / Length)

                Mi = Mi + ((((ML.mload) * b * ((3 * a) - Length)) / (Length ^ 2)))  ' + Tmi
                Mj = Mj + ((((ML.mload) * a * ((3 * b) - Length)) / (Length ^ 2))) ' + Tmj
                Fi = Fi + (((6 * (ML.mload) * a * b) / (Length ^ 3))) ' + Tfi
                Fj = Fj - (((6 * (ML.mload) * a * b) / (Length ^ 3))) '+ Tfj
            Next
            For Each UL In Fmem.Uload
                Dim LoadVertical1 As Double
                Dim LoadVertical2 As Double
                Dim LoadHorizontal1 As Double
                Dim LoadHorizontal2 As Double
                If UL.MAlign = True Then
                    LoadVertical1 = UL.uload1
                    LoadVertical2 = UL.uload2
                    LoadHorizontal1 = 0
                    LoadHorizontal2 = 0
                ElseIf UL.HAlign = True Then
                    LoadVertical1 = (UL.uload1 * Sin(Fmem.Inclination))
                    LoadVertical2 = (UL.uload2 * Sin(Fmem.Inclination))
                    LoadHorizontal1 = (-UL.uload1 * Cos(Fmem.Inclination)) '--From Left to right
                    LoadHorizontal2 = (-UL.uload2 * Cos(Fmem.Inclination)) '--From Left to right
                ElseIf UL.VAlign = True Then
                    LoadVertical1 = (UL.uload1 * Cos(Fmem.Inclination))
                    LoadVertical2 = (UL.uload2 * Cos(Fmem.Inclination))
                    LoadHorizontal1 = (UL.uload1 * Sin(Fmem.Inclination))
                    LoadHorizontal2 = (UL.uload2 * Sin(Fmem.Inclination))
                End If
                If LoadVertical1 = LoadVertical2 Then
                    c = UL.ulocation2 - UL.ulocation1
                    K = LoadVertical1 * c
                    a = UL.ulocation1 + (c / 2)
                    b = Length - (UL.ulocation2 - (c / 2))

                    Tai = axialI_UVLoadCase1(LoadHorizontal1, UL.ulocation1, UL.ulocation2, Length)
                    Taj = axialJ_UVLoadCase1(LoadHorizontal1, UL.ulocation1, UL.ulocation2, Length)
                    Tmi = ((4 * deltaI_UVLoadCase1(LoadVertical1, a, b, c, Length) - 2 * deltaJ_UVLoadCase1(LoadVertical1, a, b, c, Length)) / Length)
                    Tmj = ((2 * deltaI_UVLoadCase1(LoadVertical1, a, b, c, Length) - 4 * deltaJ_UVLoadCase1(LoadVertical1, a, b, c, Length)) / Length)
                    Tfi = (((K * b) + Tmi + Tmj) / Length)
                    Tfj = (((K * a) - Tmi - Tmj) / Length)

                    Ai = Ai + Tai
                    Mi = Mi + Tmi
                    Fi = Fi + Tfi
                    Aj = Aj + Taj
                    Mj = Mj + Tmj
                    Fj = Fj + Tfj
                ElseIf LoadVertical2 > LoadVertical1 Then
                    '---------For the smallest of load - ie., uniformly distributed
                    c = UL.ulocation2 - UL.ulocation1
                    K = LoadVertical1 * c
                    a = UL.ulocation1 + (c / 2)
                    b = Length - (UL.ulocation2 - (c / 2))

                    Tai = axialI_UVLoadCase1(LoadHorizontal1, UL.ulocation1, UL.ulocation2, Length)
                    Taj = axialJ_UVLoadCase1(LoadHorizontal1, UL.ulocation1, UL.ulocation2, Length)
                    Tmi = ((4 * deltaI_UVLoadCase1(LoadVertical1, a, b, c, Length) - 2 * deltaJ_UVLoadCase1(LoadVertical1, a, b, c, Length)) / Length)
                    Tmj = ((2 * deltaI_UVLoadCase1(LoadVertical1, a, b, c, Length) - 4 * deltaJ_UVLoadCase1(LoadVertical1, a, b, c, Length)) / Length)
                    Tfi = (((K * b) + Tmi + Tmj) / Length)
                    Tfj = (((K * a) - Tmi - Tmj) / Length)

                    Ai = Ai + Tai
                    Mi = Mi + Tmi
                    Fi = Fi + Tfi
                    Aj = Aj + Taj
                    Mj = Mj + Tmj
                    Fj = Fj + Tfj

                    '---------For the varrying load
                    c = UL.ulocation2 - UL.ulocation1
                    K = (LoadVertical2 - LoadVertical1) * (c / 2)
                    a = UL.ulocation1 + ((2 * c) / 3)
                    b = Length - (UL.ulocation2 - (c / 3))

                    Tai = axialI_UVLoadCase2(LoadHorizontal1, LoadHorizontal2, UL.ulocation1, UL.ulocation2, Length)
                    Taj = axialJ_UVLoadCase2(LoadHorizontal1, LoadHorizontal2, UL.ulocation1, UL.ulocation2, Length)
                    Tmi = ((4 * deltaI_UVLoadCase2((LoadVertical2 - LoadVertical1), a, c, Length) - 2 * deltaJ_UVLoadCase2((LoadVertical2 - LoadVertical1), b, c, Length)) / Length)
                    Tmj = ((2 * deltaI_UVLoadCase2((LoadVertical2 - LoadVertical1), a, c, Length) - 4 * deltaJ_UVLoadCase2((LoadVertical2 - LoadVertical1), b, c, Length)) / Length)
                    Tfi = (((K * b) + Tmi + Tmj) / Length)
                    Tfj = (((K * a) - Tmi - Tmj) / Length)

                    Ai = Ai + Tai
                    Mi = Mi + Tmi
                    Fi = Fi + Tfi
                    Aj = Aj + Taj
                    Mj = Mj + Tmj
                    Fj = Fj + Tfj
                ElseIf LoadVertical2 < LoadVertical1 Then
                    '---------For the smallest of load - ie., uniformly distributed
                    c = UL.ulocation2 - UL.ulocation1
                    K = LoadVertical2 * c
                    a = UL.ulocation1 + (c / 2)
                    b = Length - (UL.ulocation2 - (c / 2))

                    Tai = axialI_UVLoadCase1(LoadHorizontal2, UL.ulocation1, UL.ulocation2, Length)
                    Taj = axialJ_UVLoadCase1(LoadHorizontal2, UL.ulocation1, UL.ulocation2, Length)
                    Tmi = ((4 * deltaI_UVLoadCase1(LoadVertical2, a, b, c, Length) - 2 * deltaJ_UVLoadCase1(LoadVertical2, a, b, c, Length)) / Length)
                    Tmj = ((2 * deltaI_UVLoadCase1(LoadVertical2, a, b, c, Length) - 4 * deltaJ_UVLoadCase1(LoadVertical2, a, b, c, Length)) / Length)
                    Tfi = (((K * b) + Tmi + Tmj) / Length)
                    Tfj = (((K * a) - Tmi - Tmj) / Length)

                    Ai = Ai + Tai
                    Mi = Mi + Tmi
                    Fi = Fi + Tfi
                    Aj = Aj + Taj
                    Mj = Mj + Tmj
                    Fj = Fj + Tfj

                    '---------For the varrying load
                    c = UL.ulocation2 - UL.ulocation1
                    K = (LoadVertical1 - LoadVertical2) * (c / 2)
                    a = UL.ulocation1 + (c / 3)
                    b = Length - (UL.ulocation2 - ((2 * c) / 3))

                    Tai = axialI_UVLoadCase3(LoadHorizontal1, LoadHorizontal2, UL.ulocation1, UL.ulocation2, Length)
                    Taj = axialJ_UVLoadCase3(LoadHorizontal1, LoadHorizontal2, UL.ulocation1, UL.ulocation2, Length)
                    Tmi = ((4 * deltaI_UVLoadCase3((LoadVertical1 - LoadVertical2), a, c, Length) - 2 * deltaJ_UVLoadCase3((LoadVertical1 - LoadVertical2), b, c, Length)) / Length)
                    Tmj = ((2 * deltaI_UVLoadCase3((LoadVertical1 - LoadVertical2), a, c, Length) - 4 * deltaJ_UVLoadCase3((LoadVertical1 - LoadVertical2), b, c, Length)) / Length)
                    Tfi = (((K * b) + Tmi + Tmj) / Length)
                    Tfj = (((K * a) - Tmi - Tmj) / Length)

                    Ai = Ai + Tai
                    Mi = Mi + Tmi
                    Fi = Fi + Tfi
                    Aj = Aj + Taj
                    Mj = Mj + Tmj
                    Fj = Fj + Tfj
                End If
            Next
        End Sub

        Private Function deltaI_PointLoad(ByVal W As Double, ByVal b As Double, ByVal L As Double) As Double
            Dim Di As Double
            Di = (W * b * ((L ^ 2) - (b ^ 2))) / (6 * L)
            Return Di
        End Function

        Private Function deltaJ_PointLoad(ByVal W As Double, ByVal a As Double, ByVal L As Double) As Double
            Dim Dj As Double
            Dj = (W * a * ((L ^ 2) - (a ^ 2))) / (6 * L)
            Return Dj
        End Function

        Private Function axialI_UVLoadCase1(ByVal W As Double, ByVal L1 As Double, ByVal L2 As Double, ByVal L As Double) As Double
            '-------- Uload1 = uload2
            Dim Ai As Double
            Ai = ((W * (L2 - L1)) * (L1 + ((L2 - L1) / 2))) / L
            Return Ai
        End Function

        Private Function axialJ_UVLoadCase1(ByVal W As Double, ByVal L1 As Double, ByVal L2 As Double, ByVal L As Double) As Double
            '-------- Uload1 = uload2
            Dim Aj As Double
            Aj = ((W * (L2 - L1)) * (L - (L1 + ((L2 - L1) / 2)))) / L
            Return Aj
        End Function

        Private Function deltaI_UVLoadCase1(ByVal W As Double, ByVal a As Double, ByVal b As Double, ByVal c As Double, ByVal L As Double) As Double
            '-------- Uload1 = uload2
            Dim Di As Double
            Di = (W * b * c * ((4 * a * (b + L)) - (c ^ 2))) / (24 * L)
            Return Di
        End Function

        Private Function deltaJ_UVLoadCase1(ByVal W As Double, ByVal a As Double, ByVal b As Double, ByVal c As Double, ByVal L As Double) As Double
            '-------- Uload1 = uload2
            Dim Dj As Double
            Dj = (W * a * c * ((4 * b * (a + L)) - (c ^ 2))) / (24 * L)
            Return Dj
        End Function

        Private Function axialI_UVLoadCase2(ByVal W1 As Double, ByVal W2 As Double, ByVal L1 As Double, ByVal L2 As Double, ByVal L As Double) As Double
            '-------- Uload1 < uload2
            Dim Ai As Double
            Ai = (((W2 - W1) * (L2 - L1) * 0.5) * (L1 + ((L2 - L1) * (2 / 3)))) / L
            Return Ai
        End Function

        Private Function axialJ_UVLoadCase2(ByVal W1 As Double, ByVal W2 As Double, ByVal L1 As Double, ByVal L2 As Double, ByVal L As Double) As Double
            '-------- Uload1 < uload2
            Dim Aj As Double
            Aj = (((W2 - W1) * (L2 - L1) * 0.5) * (L - (L1 + ((L2 - L1) * (2 / 3))))) / L
            Return Aj
        End Function

        Private Function deltaI_UVLoadCase2(ByVal W As Double, ByVal a As Double, ByVal c As Double, ByVal L As Double) As Double
            '-------- Uload1 < uload2
            Dim Di, alpha, gamma As Double
            alpha = a / L
            gamma = c / L
            Di = ((W * (L ^ 3) * gamma) * _
                 ((270 * (alpha - (alpha ^ 3))) - ((gamma ^ 2) * ((45 * alpha) - (2 * gamma))))) / _
                 (3240)
            Return Di
        End Function

        Private Function deltaJ_UVLoadCase2(ByVal W As Double, ByVal b As Double, ByVal c As Double, ByVal L As Double) As Double
            '-------- Uload1 < uload2
            Dim Dj, beta, gamma As Double
            beta = b / L
            gamma = c / L
            Dj = ((W * (L ^ 3) * gamma) * _
                 ((270 * (beta - (beta ^ 3))) - ((gamma ^ 2) * ((45 * beta) + (2 * gamma))))) / _
                 (3240)
            Return Dj
        End Function

        Private Function axialI_UVLoadCase3(ByVal W1 As Double, ByVal W2 As Double, ByVal L1 As Double, ByVal L2 As Double, ByVal L As Double) As Double
            '-------- Uload1 < uload2
            Dim Ai As Double
            Ai = (((W1 - W2) * (L2 - L1) * 0.5) * (L1 + ((L2 - L1) * (1 / 3)))) / L
            Return Ai
        End Function

        Private Function axialJ_UVLoadCase3(ByVal W1 As Double, ByVal W2 As Double, ByVal L1 As Double, ByVal L2 As Double, ByVal L As Double) As Double
            '-------- Uload1 < uload2
            Dim Aj As Double
            Aj = (((W1 - W2) * (L2 - L1) * 0.5) * (L - (L1 + ((L2 - L1) * (1 / 3))))) / L
            Return Aj
        End Function

        Private Function deltaI_UVLoadCase3(ByVal W As Double, ByVal a As Double, ByVal c As Double, ByVal L As Double) As Double
            '-------- Uload1 > uload2
            Dim Di, alpha, gamma As Double
            alpha = a / L
            gamma = c / L
            Di = ((W * (L ^ 3) * gamma) * _
                 ((270 * (alpha - (alpha ^ 3))) - ((gamma ^ 2) * ((45 * alpha) + (2 * gamma))))) / _
                 (3240)
            Return Di
        End Function

        Private Function deltaJ_UVLoadCase3(ByVal W As Double, ByVal b As Double, ByVal c As Double, ByVal L As Double) As Double
            '-------- Uload1 > uload2
            Dim Dj, beta, gamma As Double
            beta = b / L
            gamma = c / L
            Dj = ((W * (L ^ 3) * gamma) * _
                 ((270 * (beta - (beta ^ 3))) - ((gamma ^ 2) * ((45 * beta) - (2 * gamma))))) / _
                 (3240)
            Return Dj
        End Function

        Private Function deltaI_Momentoad(ByVal W As Double, ByVal b As Double, ByVal L As Double) As Double
            Dim Di As Double
            Di = ((W) * ((3 * (b ^ 2)) - (L ^ 2))) / (6 * L)
            Return Di
        End Function

        Private Function deltaJ_Momentoad(ByVal W As Double, ByVal a As Double, ByVal L As Double) As Double
            Dim Dj As Double
            Dj = ((W) * ((L ^ 2) - (3 * (a ^ 2)))) / (6 * L)
            Return Dj
        End Function
#End Region

        Private Sub FixElementStiffness(ByRef EL As Line2DT)
            '-----------Element Stiffness matrix - Global Coordinates --------------
            Dim K1 As Double = EL.E / EL.Length
            Dim V1 As Double = K1 * ((EL.A * (_ActLCos ^ 2)) + (12 * EL.I * ((_ActMsin ^ 2) / (EL.Length ^ 2))))
            Dim V2 As Double = K1 * ((EL.A * (_ActMsin ^ 2)) + (12 * EL.I * ((_ActLCos ^ 2) / (EL.Length ^ 2))))
            Dim V3 As Double = K1 * ((EL.A - ((12 * EL.I) / (EL.Length ^ 2))) * (_ActMsin * _ActLCos))
            Dim V4 As Double = K1 * ((6 * EL.I * _ActLCos) / EL.Length)
            Dim V5 As Double = K1 * ((6 * EL.I * _ActMsin) / EL.Length)
            Dim V6 As Double = K1 * (4 * EL.I)

            '--- Row1
            _GElementStiffnessMatrix(0, 0) = V1
            _GElementStiffnessMatrix(0, 1) = V3
            _GElementStiffnessMatrix(0, 2) = -V5
            _GElementStiffnessMatrix(0, 3) = -V1
            _GElementStiffnessMatrix(0, 4) = -V3
            _GElementStiffnessMatrix(0, 5) = -V5

            '--- Row2
            _GElementStiffnessMatrix(1, 0) = V3
            _GElementStiffnessMatrix(1, 1) = V2
            _GElementStiffnessMatrix(1, 2) = V4
            _GElementStiffnessMatrix(1, 3) = -V3
            _GElementStiffnessMatrix(1, 4) = -V2
            _GElementStiffnessMatrix(1, 5) = V4

            '--- Row3
            _GElementStiffnessMatrix(2, 0) = -V5
            _GElementStiffnessMatrix(2, 1) = V4
            _GElementStiffnessMatrix(2, 2) = V6
            _GElementStiffnessMatrix(2, 3) = V5
            _GElementStiffnessMatrix(2, 4) = -V4
            _GElementStiffnessMatrix(2, 5) = V6 / 2

            '--- Row4
            _GElementStiffnessMatrix(3, 0) = -V1
            _GElementStiffnessMatrix(3, 1) = -V3
            _GElementStiffnessMatrix(3, 2) = V5
            _GElementStiffnessMatrix(3, 3) = V1
            _GElementStiffnessMatrix(3, 4) = V3
            _GElementStiffnessMatrix(3, 5) = V5

            '--- Row5
            _GElementStiffnessMatrix(4, 0) = -V3
            _GElementStiffnessMatrix(4, 1) = -V2
            _GElementStiffnessMatrix(4, 2) = -V4
            _GElementStiffnessMatrix(4, 3) = V3
            _GElementStiffnessMatrix(4, 4) = V2
            _GElementStiffnessMatrix(4, 5) = -V4

            '--- Row6
            _GElementStiffnessMatrix(5, 0) = -V5
            _GElementStiffnessMatrix(5, 1) = V4
            _GElementStiffnessMatrix(5, 2) = V6 / 2
            _GElementStiffnessMatrix(5, 3) = V5
            _GElementStiffnessMatrix(5, 4) = -V4
            _GElementStiffnessMatrix(5, 5) = V6

            '-----------Element Stiffness matrix - Local Coordinates --------------
            Dim C1 As Double = (EL.A * EL.E) / EL.Length
            Dim C2 As Double = (EL.E * EL.I) / EL.Length ^ 3

            '--- Row1
            _LElementStiffnessMatrix(0, 0) = C1
            _LElementStiffnessMatrix(0, 1) = 0
            _LElementStiffnessMatrix(0, 2) = 0
            _LElementStiffnessMatrix(0, 3) = -C1
            _LElementStiffnessMatrix(0, 4) = 0
            _LElementStiffnessMatrix(0, 5) = 0

            '--- Row2
            _LElementStiffnessMatrix(1, 0) = 0
            _LElementStiffnessMatrix(1, 1) = 12 * C2
            _LElementStiffnessMatrix(1, 2) = 6 * C2 * EL.Length
            _LElementStiffnessMatrix(1, 3) = 0
            _LElementStiffnessMatrix(1, 4) = -12 * C2
            _LElementStiffnessMatrix(1, 5) = 6 * C2 * EL.Length

            '--- Row3
            _LElementStiffnessMatrix(2, 0) = 0
            _LElementStiffnessMatrix(2, 1) = 6 * C2 * EL.Length
            _LElementStiffnessMatrix(2, 2) = 4 * C2 * (EL.Length ^ 2)
            _LElementStiffnessMatrix(2, 3) = 0
            _LElementStiffnessMatrix(2, 4) = -6 * C2 * EL.Length
            _LElementStiffnessMatrix(2, 5) = 2 * C2 * (EL.Length ^ 2)

            '--- Row4
            _LElementStiffnessMatrix(3, 0) = -C1
            _LElementStiffnessMatrix(3, 1) = 0
            _LElementStiffnessMatrix(3, 2) = 0
            _LElementStiffnessMatrix(3, 3) = C1
            _LElementStiffnessMatrix(3, 4) = 0
            _LElementStiffnessMatrix(3, 5) = 0

            '--- Row5
            _LElementStiffnessMatrix(4, 0) = 0
            _LElementStiffnessMatrix(4, 1) = -12 * C2
            _LElementStiffnessMatrix(4, 2) = -6 * C2 * EL.Length
            _LElementStiffnessMatrix(4, 3) = 0
            _LElementStiffnessMatrix(4, 4) = 12 * C2
            _LElementStiffnessMatrix(4, 5) = -6 * C2 * EL.Length

            '--- Row6
            _LElementStiffnessMatrix(5, 0) = 0
            _LElementStiffnessMatrix(5, 1) = 6 * C2 * EL.Length
            _LElementStiffnessMatrix(5, 2) = 2 * C2 * (EL.Length ^ 2)
            _LElementStiffnessMatrix(5, 3) = 0
            _LElementStiffnessMatrix(5, 4) = -6 * C2 * EL.Length
            _LElementStiffnessMatrix(5, 5) = 4 * C2 * (EL.Length ^ 2)

            '---Matrix Multiplication KT
            Dim KTmatrix(5, 5) As Double
            Matrix_Multiplication(_GElementStiffnessMatrix, 5, 5, _Transformation_Mat, 5, 5, KTmatrix)
            '---Matrix Multiplication TtKT
            Matrix_Multiplication(_Transformation_MatT, 5, 5, KTmatrix, 5, 5, _GElementStiffnessMatrix)
        End Sub

        Public Sub FixGlobalMatrices(ByRef DOFmatrix() As Double, ByRef FERmatrix() As Double, ByRef Settlementmatrix() As Double, ByRef GSmatrix(,) As Double)
            '-----------Degree of Freedom Matrix
            DOFmatrix((_StartNodeNo * 3) + 0) = _DegreeOFFreedomMatrix(0)
            DOFmatrix((_StartNodeNo * 3) + 1) = _DegreeOFFreedomMatrix(1)
            DOFmatrix((_StartNodeNo * 3) + 2) = _DegreeOFFreedomMatrix(2)
            DOFmatrix((_EndNodeNo * 3) + 0) = _DegreeOFFreedomMatrix(3)
            DOFmatrix((_EndNodeNo * 3) + 1) = _DegreeOFFreedomMatrix(4)
            DOFmatrix((_EndNodeNo * 3) + 2) = _DegreeOFFreedomMatrix(5)

            '-----------Fixed End Reaction Matrix
            FERmatrix((_StartNodeNo * 3) + 0) = FERmatrix((_StartNodeNo * 3) + 0) + _GLoadVectorMatrix(0) '
            FERmatrix((_StartNodeNo * 3) + 1) = FERmatrix((_StartNodeNo * 3) + 1) + _GLoadVectorMatrix(1) '
            FERmatrix((_StartNodeNo * 3) + 2) = FERmatrix((_StartNodeNo * 3) + 2) + _GLoadVectorMatrix(2) ' 
            FERmatrix((_EndNodeNo * 3) + 0) = FERmatrix((_EndNodeNo * 3) + 0) + _GLoadVectorMatrix(3) '
            FERmatrix((_EndNodeNo * 3) + 1) = FERmatrix((_EndNodeNo * 3) + 1) + _GLoadVectorMatrix(4) '
            FERmatrix((_EndNodeNo * 3) + 2) = FERmatrix((_EndNodeNo * 3) + 2) + _GLoadVectorMatrix(5) '

            '-----------Settlement Matrix
            Settlementmatrix((_StartNodeNo * 3) + 0) = _SettlementMatrix(0)
            Settlementmatrix((_StartNodeNo * 3) + 1) = _SettlementMatrix(1)
            Settlementmatrix((_StartNodeNo * 3) + 2) = _SettlementMatrix(2)
            Settlementmatrix((_EndNodeNo * 3) + 0) = _SettlementMatrix(3)
            Settlementmatrix((_EndNodeNo * 3) + 1) = _SettlementMatrix(4)
            Settlementmatrix((_EndNodeNo * 3) + 2) = _SettlementMatrix(5)


            '-----------Global Stiffness matrix --------------
            '--- Column1
            GSmatrix((_StartNodeNo * 3) + 0, (_StartNodeNo * 3) + 0) = GSmatrix((_StartNodeNo * 3) + 0, (_StartNodeNo * 3) + 0) + _GElementStiffnessMatrix(0, 0)
            GSmatrix((_StartNodeNo * 3) + 0, (_StartNodeNo * 3) + 1) = GSmatrix((_StartNodeNo * 3) + 0, (_StartNodeNo * 3) + 1) + _GElementStiffnessMatrix(0, 1)
            GSmatrix((_StartNodeNo * 3) + 0, (_StartNodeNo * 3) + 2) = GSmatrix((_StartNodeNo * 3) + 0, (_StartNodeNo * 3) + 2) + _GElementStiffnessMatrix(0, 2)
            GSmatrix((_StartNodeNo * 3) + 0, (_EndNodeNo * 3) + 0) = GSmatrix((_StartNodeNo * 3) + 0, (_EndNodeNo * 3) + 0) + _GElementStiffnessMatrix(0, 3)
            GSmatrix((_StartNodeNo * 3) + 0, (_EndNodeNo * 3) + 1) = GSmatrix((_StartNodeNo * 3) + 0, (_EndNodeNo * 3) + 1) + _GElementStiffnessMatrix(0, 4)
            GSmatrix((_StartNodeNo * 3) + 0, (_EndNodeNo * 3) + 2) = GSmatrix((_StartNodeNo * 3) + 0, (_EndNodeNo * 3) + 2) + _GElementStiffnessMatrix(0, 5)

            '--- Column2
            GSmatrix((_StartNodeNo * 3) + 1, (_StartNodeNo * 3) + 0) = GSmatrix((_StartNodeNo * 3) + 1, (_StartNodeNo * 3) + 0) + _GElementStiffnessMatrix(1, 0)
            GSmatrix((_StartNodeNo * 3) + 1, (_StartNodeNo * 3) + 1) = GSmatrix((_StartNodeNo * 3) + 1, (_StartNodeNo * 3) + 1) + _GElementStiffnessMatrix(1, 1)
            GSmatrix((_StartNodeNo * 3) + 1, (_StartNodeNo * 3) + 2) = GSmatrix((_StartNodeNo * 3) + 1, (_StartNodeNo * 3) + 2) + _GElementStiffnessMatrix(1, 2)
            GSmatrix((_StartNodeNo * 3) + 1, (_EndNodeNo * 3) + 0) = GSmatrix((_StartNodeNo * 3) + 1, (_EndNodeNo * 3) + 0) + _GElementStiffnessMatrix(1, 3)
            GSmatrix((_StartNodeNo * 3) + 1, (_EndNodeNo * 3) + 1) = GSmatrix((_StartNodeNo * 3) + 1, (_EndNodeNo * 3) + 1) + _GElementStiffnessMatrix(1, 4)
            GSmatrix((_StartNodeNo * 3) + 1, (_EndNodeNo * 3) + 2) = GSmatrix((_StartNodeNo * 3) + 1, (_EndNodeNo * 3) + 2) + _GElementStiffnessMatrix(1, 5)

            '--- Column3
            GSmatrix((_StartNodeNo * 3) + 2, (_StartNodeNo * 3) + 0) = GSmatrix((_StartNodeNo * 3) + 2, (_StartNodeNo * 3) + 0) + _GElementStiffnessMatrix(2, 0)
            GSmatrix((_StartNodeNo * 3) + 2, (_StartNodeNo * 3) + 1) = GSmatrix((_StartNodeNo * 3) + 2, (_StartNodeNo * 3) + 1) + _GElementStiffnessMatrix(2, 1)
            GSmatrix((_StartNodeNo * 3) + 2, (_StartNodeNo * 3) + 2) = GSmatrix((_StartNodeNo * 3) + 2, (_StartNodeNo * 3) + 2) + _GElementStiffnessMatrix(2, 2)
            GSmatrix((_StartNodeNo * 3) + 2, (_EndNodeNo * 3) + 0) = GSmatrix((_StartNodeNo * 3) + 2, (_EndNodeNo * 3) + 0) + _GElementStiffnessMatrix(2, 3)
            GSmatrix((_StartNodeNo * 3) + 2, (_EndNodeNo * 3) + 1) = GSmatrix((_StartNodeNo * 3) + 2, (_EndNodeNo * 3) + 1) + _GElementStiffnessMatrix(2, 4)
            GSmatrix((_StartNodeNo * 3) + 2, (_EndNodeNo * 3) + 2) = GSmatrix((_StartNodeNo * 3) + 2, (_EndNodeNo * 3) + 2) + _GElementStiffnessMatrix(2, 5)

            '--- Column4
            GSmatrix((_EndNodeNo * 3) + 0, (_StartNodeNo * 3) + 0) = GSmatrix((_EndNodeNo * 3) + 0, (_StartNodeNo * 3) + 0) + _GElementStiffnessMatrix(3, 0)
            GSmatrix((_EndNodeNo * 3) + 0, (_StartNodeNo * 3) + 1) = GSmatrix((_EndNodeNo * 3) + 0, (_StartNodeNo * 3) + 1) + _GElementStiffnessMatrix(3, 1)
            GSmatrix((_EndNodeNo * 3) + 0, (_StartNodeNo * 3) + 2) = GSmatrix((_EndNodeNo * 3) + 0, (_StartNodeNo * 3) + 2) + _GElementStiffnessMatrix(3, 2)
            GSmatrix((_EndNodeNo * 3) + 0, (_EndNodeNo * 3) + 0) = GSmatrix((_EndNodeNo * 3) + 0, (_EndNodeNo * 3) + 0) + _GElementStiffnessMatrix(3, 3)
            GSmatrix((_EndNodeNo * 3) + 0, (_EndNodeNo * 3) + 1) = GSmatrix((_EndNodeNo * 3) + 0, (_EndNodeNo * 3) + 1) + _GElementStiffnessMatrix(3, 4)
            GSmatrix((_EndNodeNo * 3) + 0, (_EndNodeNo * 3) + 2) = GSmatrix((_EndNodeNo * 3) + 0, (_EndNodeNo * 3) + 2) + _GElementStiffnessMatrix(3, 5)

            '--- Column5
            GSmatrix((_EndNodeNo * 3) + 1, (_StartNodeNo * 3) + 0) = GSmatrix((_EndNodeNo * 3) + 1, (_StartNodeNo * 3) + 0) + _GElementStiffnessMatrix(4, 0)
            GSmatrix((_EndNodeNo * 3) + 1, (_StartNodeNo * 3) + 1) = GSmatrix((_EndNodeNo * 3) + 1, (_StartNodeNo * 3) + 1) + _GElementStiffnessMatrix(4, 1)
            GSmatrix((_EndNodeNo * 3) + 1, (_StartNodeNo * 3) + 2) = GSmatrix((_EndNodeNo * 3) + 1, (_StartNodeNo * 3) + 2) + _GElementStiffnessMatrix(4, 2)
            GSmatrix((_EndNodeNo * 3) + 1, (_EndNodeNo * 3) + 0) = GSmatrix((_EndNodeNo * 3) + 1, (_EndNodeNo * 3) + 0) + _GElementStiffnessMatrix(4, 3)
            GSmatrix((_EndNodeNo * 3) + 1, (_EndNodeNo * 3) + 1) = GSmatrix((_EndNodeNo * 3) + 1, (_EndNodeNo * 3) + 1) + _GElementStiffnessMatrix(4, 4)
            GSmatrix((_EndNodeNo * 3) + 1, (_EndNodeNo * 3) + 2) = GSmatrix((_EndNodeNo * 3) + 1, (_EndNodeNo * 3) + 2) + _GElementStiffnessMatrix(4, 5)

            '--- Column6
            GSmatrix((_EndNodeNo * 3) + 2, (_StartNodeNo * 3) + 0) = GSmatrix((_EndNodeNo * 3) + 2, (_StartNodeNo * 3) + 0) + _GElementStiffnessMatrix(5, 0)
            GSmatrix((_EndNodeNo * 3) + 2, (_StartNodeNo * 3) + 1) = GSmatrix((_EndNodeNo * 3) + 2, (_StartNodeNo * 3) + 1) + _GElementStiffnessMatrix(5, 1)
            GSmatrix((_EndNodeNo * 3) + 2, (_StartNodeNo * 3) + 2) = GSmatrix((_EndNodeNo * 3) + 2, (_StartNodeNo * 3) + 2) + _GElementStiffnessMatrix(5, 2)
            GSmatrix((_EndNodeNo * 3) + 2, (_EndNodeNo * 3) + 0) = GSmatrix((_EndNodeNo * 3) + 2, (_EndNodeNo * 3) + 0) + _GElementStiffnessMatrix(5, 3)
            GSmatrix((_EndNodeNo * 3) + 2, (_EndNodeNo * 3) + 1) = GSmatrix((_EndNodeNo * 3) + 2, (_EndNodeNo * 3) + 1) + _GElementStiffnessMatrix(5, 4)
            GSmatrix((_EndNodeNo * 3) + 2, (_EndNodeNo * 3) + 2) = GSmatrix((_EndNodeNo * 3) + 2, (_EndNodeNo * 3) + 2) + _GElementStiffnessMatrix(5, 5)
        End Sub

        Public Sub FixResultMatrix(ByRef ResMatrix() As Double)
            Dim _TRslt(5) As Double
            _TRslt(0) = ResMatrix((_StartNodeNo * 3) + 0)
            _TRslt(1) = ResMatrix((_StartNodeNo * 3) + 1)
            _TRslt(2) = ResMatrix((_StartNodeNo * 3) + 2)
            _TRslt(3) = ResMatrix((_EndNodeNo * 3) + 0)
            _TRslt(4) = ResMatrix((_EndNodeNo * 3) + 1)
            _TRslt(5) = ResMatrix((_EndNodeNo * 3) + 2)

            '-----------START NODE BOUNDARY CONDITION _ TRANSFORMATION 
            If ElDetails.SN.Support.PJ = True Or ElDetails.SN.Support.RJ = True Then
                _GResultMatrix(0) = _TRslt(0)
                _GResultMatrix(1) = _TRslt(1)
                _GResultMatrix(2) = _TRslt(2)
            ElseIf ElDetails.SN.Support.PS = True Or ElDetails.SN.Support.PRS = True Or ElDetails.SN.Support.SS = True Or ElDetails.SN.Support.FRS = True Or ElDetails.SN.Support.FS = True Then
                _GResultMatrix(0) = (_TRslt(0) * Math.Cos((ElDetails.SN.Support.supportinclination - (Math.PI / 2)))) + (_TRslt(1) * Math.Sin((ElDetails.SN.Support.supportinclination - (Math.PI / 2))))
                _GResultMatrix(1) = (_TRslt(0) * -Math.Sin((ElDetails.SN.Support.supportinclination - (Math.PI / 2)))) + (_TRslt(1) * Math.Cos((ElDetails.SN.Support.supportinclination - (Math.PI / 2))))
                _GResultMatrix(2) = _TRslt(2)
            End If
            '-------------END NODE BOUNDARY CONDITION _ TRANSFORMATION 
            If ElDetails.EN.Support.PJ = True Or ElDetails.EN.Support.RJ = True Then
                _GResultMatrix(3) = _TRslt(3)
                _GResultMatrix(4) = _TRslt(4)
                _GResultMatrix(5) = _TRslt(5)
            ElseIf ElDetails.EN.Support.PS = True Or ElDetails.EN.Support.PRS = True Or ElDetails.EN.Support.SS = True Or ElDetails.EN.Support.FRS = True Or ElDetails.EN.Support.FS = True Then
                _GResultMatrix(3) = (_TRslt(3) * Math.Cos((ElDetails.EN.Support.supportinclination - (Math.PI / 2)))) + (_TRslt(4) * Math.Sin((ElDetails.EN.Support.supportinclination - (Math.PI / 2))))
                _GResultMatrix(4) = (_TRslt(3) * -Math.Sin((ElDetails.EN.Support.supportinclination - (Math.PI / 2)))) + (_TRslt(4) * Math.Cos((ElDetails.EN.Support.supportinclination - (Math.PI / 2))))
                _GResultMatrix(5) = _TRslt(5)
            End If


            Dim _tempG(5, 0) As Double
            Dim _tempR(5, 0) As Double
            ' Inverse Transforming Globale Result matrix to Local Result matrix
            _tempG(0, 0) = (_ActLCos * _GResultMatrix(0)) + (_ActMsin * _GResultMatrix(1))
            _tempG(1, 0) = (-_ActMsin * _GResultMatrix(0)) + (_ActLCos * _GResultMatrix(1))
            _tempG(2, 0) = _GResultMatrix(2)
            _tempG(3, 0) = (_ActLCos * _GResultMatrix(3)) + (_ActMsin * _GResultMatrix(4))
            _tempG(4, 0) = (-_ActMsin * _GResultMatrix(3)) + (_ActLCos * _GResultMatrix(4))
            _tempG(5, 0) = _GResultMatrix(5)

            Matrix_Multiplication(_LElementStiffnessMatrix, 5, 5, _tempG, 5, 0, _tempR)


            _LResultMatrix(0) = (_tempR(0, 0) - _LLoadVectorMatrix(0))
            _LResultMatrix(1) = (_tempR(1, 0) - _LLoadVectorMatrix(1))
            _LResultMatrix(2) = (_tempR(2, 0) - _LLoadVectorMatrix(2))
            _LResultMatrix(3) = (_tempR(3, 0) - _LLoadVectorMatrix(3))
            _LResultMatrix(4) = (_tempR(4, 0) - _LLoadVectorMatrix(4))
            _LResultMatrix(5) = (_tempR(5, 0) - _LLoadVectorMatrix(5))

        End Sub

        Private Sub Matrix_Multiplication(ByVal A(,) As Double, ByVal arow As Integer, ByVal acolumn As Integer, ByVal B(,) As Double, ByVal brow As Integer, ByVal bcolumn As Integer, ByRef C(,) As Double)
            ReDim C(arow, bcolumn)
            For i = 0 To arow
                For j = 0 To bcolumn
                    C(i, j) = 0
                    For k = 0 To brow
                        C(i, j) = C(i, j) + (A(i, k) * B(k, j))
                    Next
                Next
            Next
        End Sub

#Region "Finding Shear Force and Bending Moment for set of points"
        Private Function Fixinterval(ByVal Ellength As Double) As Double
            Return (Ellength / 1000)
        End Function

#Region "Shear Force Calculation"
        Private Function Total_ShearForce_L(ByVal _curDx As Double) As Double
            '-----Function Returns Total Shear Force in a point from left to right
            Dim SF_L As Double
            SF_L = SF_EndReaction_L(_curDx) + SF_PointLoad_L(_curDx) + SF_UVL_L(_curDx)
            Return SF_L
        End Function

        Private Function SF_PointLoad_L(ByVal _curDx As Double) As Double
            '----Shear Force due to point load from left to right
            Dim SF, T_SF As Double
            Dim intensity As Double
            For Each PL In ElDetails.Pload
                SF = 0
                If PL.MAlign = True Then
                    intensity = PL.pload
                ElseIf PL.HAlign = True Then
                    intensity = (PL.pload * Sin(ElDetails.Inclination))
                ElseIf PL.VAlign = True Then
                    intensity = (PL.pload * Cos(ElDetails.Inclination))
                End If
                If _curDx >= PL.plocation Then
                    SF = intensity
                End If
                T_SF = T_SF + SF
            Next
            Return T_SF
        End Function

        Private Function SF_UVL_L(ByVal _curDx As Double) As Double
            '----Shear force due to UVL from left to right
            Dim _RectF, _TriF, _SecF, SF, T_SF As Double
            Dim intensity1, intensity2 As Double
            For Each UL In ElDetails.Uload
                If UL.MAlign = True Then
                    intensity1 = UL.uload1
                    intensity2 = UL.uload2
                ElseIf UL.HAlign = True Then
                    intensity1 = (UL.uload1 * Sin(ElDetails.Inclination))
                    intensity2 = (UL.uload2 * Sin(ElDetails.Inclination))
                ElseIf UL.VAlign = True Then
                    intensity1 = (UL.uload1 * Cos(ElDetails.Inclination))
                    intensity2 = (UL.uload2 * Cos(ElDetails.Inclination))
                End If

                If intensity1 <= intensity2 Then
                    SF = 0
                    If (_curDx >= UL.ulocation2) Then
                        _RectF = intensity1 * (UL.ulocation2 - UL.ulocation1)
                        _TriF = 0.5 * (intensity2 - intensity1) * (UL.ulocation2 - UL.ulocation1)
                        SF = _RectF + _TriF
                    ElseIf ((_curDx >= UL.ulocation1) And (_curDx < UL.ulocation2)) Then
                        _RectF = intensity1 * (_curDx - UL.ulocation1)
                        _SecF = ((intensity2 - intensity1) / (UL.ulocation2 - UL.ulocation1)) * _
                                (_curDx - UL.ulocation1)
                        _TriF = 0.5 * _SecF * (_curDx - UL.ulocation1)
                        SF = _RectF + _TriF
                    End If
                    T_SF = T_SF + SF
                Else
                    SF = 0
                    If (_curDx >= UL.ulocation2) Then
                        _RectF = intensity2 * (UL.ulocation2 - UL.ulocation1)
                        _TriF = 0.5 * (intensity1 - intensity2) * (UL.ulocation2 - UL.ulocation1)
                        SF = _RectF + _TriF
                    ElseIf ((_curDx >= UL.ulocation1) And (_curDx < UL.ulocation2)) Then
                        _SecF = intensity2 + _
                                                           (((intensity1 - intensity2) / (UL.ulocation2 - UL.ulocation1)) * (UL.ulocation2 - _curDx))
                        _RectF = _SecF * (_curDx - UL.ulocation1)
                        _TriF = 0.5 * (intensity1 - _SecF) * (_curDx - UL.ulocation1)
                        SF = _RectF + _TriF
                    End If
                    T_SF = T_SF + SF
                End If
            Next
            Return T_SF
        End Function

        Private Function SF_EndReaction_L(ByVal _curDx As Double) As Double
            '-----Shear force due to end reaction force from Left to Right
            Dim SF As Double
            SF = -_LResultMatrix(1)
            If _curDx = ElDetails.Length Then
                SF = SF - _LResultMatrix(4)
            End If
            Return SF
        End Function
#End Region

#Region "Bending Moment Calculation"
        Private Function Total_BendingMoment_L(ByVal _curDx As Double) As Double
            '-----Function Returns Total Bending Moment in a point from left to right
            Dim BM As Double
            BM = BM_EndMoment_L(_curDx) + _
                 BM_EndReaction_L(_curDx) + _
                 BM_PointLoad_L(_curDx) + _
                 BM_UVL_L(_curDx) + _
                 BM_moment_L(_curDx)
            Return BM
        End Function

        Private Function BM_PointLoad_L(ByVal _curDx As Double) As Double
            '----Bending moment due to point load from left to right
            Dim BM, T_BM As Double
            Dim intensity As Double
            For Each PL In ElDetails.Pload
                BM = 0
                If PL.MAlign = True Then
                    intensity = PL.pload
                ElseIf PL.HAlign = True Then
                    intensity = (PL.pload * Sin(ElDetails.Inclination))
                ElseIf PL.VAlign = True Then
                    intensity = (PL.pload * Cos(ElDetails.Inclination))
                End If
                If _curDx >= PL.plocation Then
                    BM = intensity * (_curDx - PL.plocation)
                End If
                T_BM = T_BM + BM
            Next
            Return T_BM
        End Function

        Private Function BM_UVL_L(ByVal _curDx As Double) As Double
            '----Bending moment due to UVL from left to right
            Dim _RectF, _TriF, _SecF, BM, T_BM As Double
            Dim intensity1, intensity2 As Double
            For Each UL In ElDetails.Uload
                BM = 0
                If UL.MAlign = True Then
                    intensity1 = UL.uload1
                    intensity2 = UL.uload2
                ElseIf UL.HAlign = True Then
                    intensity1 = (UL.uload1 * Sin(ElDetails.Inclination))
                    intensity2 = (UL.uload2 * Sin(ElDetails.Inclination))
                ElseIf UL.VAlign = True Then
                    intensity1 = (UL.uload1 * Cos(ElDetails.Inclination))
                    intensity2 = (UL.uload2 * Cos(ElDetails.Inclination))
                End If

                If intensity1 <= intensity2 Then
                    If (_curDx > UL.ulocation2) Then
                        _RectF = intensity1 * (UL.ulocation2 - UL.ulocation1)
                        _TriF = 0.5 * (intensity2 - intensity1) * (UL.ulocation2 - UL.ulocation1)
                        BM = (_RectF * (_curDx - (UL.ulocation1 + ((UL.ulocation2 - UL.ulocation1) * 0.5)))) + _
                             (_TriF * (_curDx - (UL.ulocation1 + ((UL.ulocation2 - UL.ulocation1) * (2 / 3)))))
                    ElseIf ((_curDx >= UL.ulocation1) And (_curDx <= UL.ulocation2)) Then
                        _RectF = intensity1 * (_curDx - UL.ulocation1)
                        _SecF = ((intensity2 - intensity1) / (UL.ulocation2 - UL.ulocation1)) * _
                                (_curDx - UL.ulocation1)
                        _TriF = 0.5 * _SecF * (_curDx - UL.ulocation1)
                        BM = (_RectF * (((_curDx - UL.ulocation1) * 0.5))) + _
                             (_TriF * (((_curDx - UL.ulocation1) * (1 / 3))))
                    End If
                    T_BM = T_BM + BM
                Else
                    If (_curDx > UL.ulocation2) Then
                        _RectF = intensity2 * (UL.ulocation2 - UL.ulocation1)
                        _TriF = 0.5 * (intensity1 - intensity2) * (UL.ulocation2 - UL.ulocation1)
                        BM = (_RectF * (_curDx - (UL.ulocation1 + ((UL.ulocation2 - UL.ulocation1) * 0.5)))) + _
                             (_TriF * (_curDx - (UL.ulocation1 + ((UL.ulocation2 - UL.ulocation1) * (1 / 3)))))
                    ElseIf ((_curDx >= UL.ulocation1) And (_curDx <= UL.ulocation2)) Then
                        _SecF = intensity2 + _
                                (((intensity1 - intensity2) / (UL.ulocation2 - UL.ulocation1)) * (UL.ulocation2 - _curDx))
                        _RectF = _SecF * (_curDx - UL.ulocation1)
                        _TriF = 0.5 * (intensity1 - _SecF) * (_curDx - UL.ulocation1)
                        BM = (_RectF * (((_curDx - UL.ulocation1) * 0.5))) + _
                             (_TriF * (((_curDx - UL.ulocation1) * (2 / 3))))
                    End If
                    T_BM = T_BM + BM
                End If
            Next
            Return T_BM
        End Function

        Private Function BM_moment_L(ByVal _curDx As Double) As Double
            '----- Bending moment due to moment from left to right
            Dim BM, T_BM As Double
            For Each ML In ElDetails.Mload
                BM = 0
                If _curDx >= ML.mlocation Then
                    BM = ML.mload
                End If
                T_BM = T_BM + BM
            Next
            Return T_BM
        End Function

        Private Function BM_EndReaction_L(ByVal _curDx As Double) As Double
            '-----Bending moment due to end reaction force from Left to Right
            Dim BM As Double
            BM = -_LResultMatrix(1) * _curDx
            If _curDx = ElDetails.Length Then
                BM = BM - (_LResultMatrix(4) * 0)
            End If
            Return BM
        End Function

        Private Function BM_EndMoment_L(ByVal _curDx As Double) As Double
            '-----Bending moment due to end reaction moment from left to right
            Dim BM As Double
            BM = (_LResultMatrix(2))
            If _curDx = ElDetails.Length Then
                BM = BM + (_LResultMatrix(5))
            End If
            Return BM
        End Function
#End Region

#Region "Deflection Calculation"
        Private Function GaussQuadrature_3Point(ByVal b As Double, ByVal a As Double, ByVal _Curx As Double)
            Dim delta As Double = (b - a) / 2
            Const C1 = 0.555555556
            Const C2 = 0.888888889
            Const C3 = 0.555555556

            Const X1 = -0.774596669
            Const X2 = 0.0
            Const X3 = 0.774596669

            Dim FX1 As Double = Total_BendingMoment_L((delta * X1) + ((b + a) / 2))
            Dim FX2 As Double = Total_BendingMoment_L((delta * X2) + ((b + a) / 2))
            Dim FX3 As Double = Total_BendingMoment_L((delta * X3) + ((b + a) / 2))

            Dim Integration As Double
            Integration = delta * ((C1 * FX1) + (C2 * FX2) + (C3 * FX3))
            Return Integration
        End Function

        Private Function GaussQuadrature_3Point_AxialForce(ByVal b As Double, ByVal a As Double, ByVal _Curx As Double)
            Dim delta As Double = (b - a) / 2
            Const C1 = 0.555555556
            Const C2 = 0.888888889
            Const C3 = 0.555555556

            Const X1 = -0.774596669
            Const X2 = 0.0
            Const X3 = 0.774596669

            Dim FX1 As Double = Total_AxialForce_L((delta * X1) + ((b + a) / 2))
            Dim FX2 As Double = Total_AxialForce_L((delta * X2) + ((b + a) / 2))
            Dim FX3 As Double = Total_AxialForce_L((delta * X3) + ((b + a) / 2))

            Dim Integration As Double
            Integration = delta * ((C1 * FX1) + (C2 * FX2) + (C3 * FX3))
            Return Integration
        End Function

        Private Sub FixDefelection(ByRef R As ResultDrawingCoords, ByVal CalibratedX As List(Of Double))
            '------ Calculating Slope & Deflection
            '--------First Integration
            Dim _FirstIntegration As New List(Of Double)
            Dim LowerLimit As Double = 0
            Dim UpperLimit As Double = 0
            Dim CummulativeBM As Double = 0
            LowerLimit = CalibratedX(0)
            UpperLimit = (CalibratedX(0) + CalibratedX(1)) / 2
            CummulativeBM = (GaussQuadrature_3Point(UpperLimit, LowerLimit, CalibratedX(0)))
            _FirstIntegration.Add(CummulativeBM)
            For i = 1 To CalibratedX.Count - 2
                LowerLimit = (CalibratedX(i - 1) + CalibratedX(i)) / 2
                UpperLimit = (CalibratedX(i) + CalibratedX(i + 1)) / 2
                CummulativeBM = CummulativeBM + (GaussQuadrature_3Point(UpperLimit, LowerLimit, CalibratedX(i)))
                _FirstIntegration.Add(CummulativeBM)
            Next
            LowerLimit = (CalibratedX(CalibratedX.Count - 2) + CalibratedX(CalibratedX.Count - 1)) / 2
            UpperLimit = CalibratedX(CalibratedX.Count - 1)
            CummulativeBM = (CummulativeBM + GaussQuadrature_3Point(UpperLimit, LowerLimit, CalibratedX(CalibratedX.Count - 1)))
            _FirstIntegration.Add(CummulativeBM)


            '--------Second Integration
            Dim _SecondIntegration As New List(Of Double)
            LowerLimit = 0
            UpperLimit = 0
            Dim CummulativeSL As Double = 0
            LowerLimit = CalibratedX(0)
            UpperLimit = (CalibratedX(0) + CalibratedX(1)) / 2
            CummulativeSL = (((UpperLimit - LowerLimit) / 2) * (_FirstIntegration(0)))
            _SecondIntegration.Add(CummulativeSL)
            For i = 1 To CalibratedX.Count - 2
                LowerLimit = (CalibratedX(i - 1) + CalibratedX(i)) / 2
                UpperLimit = (CalibratedX(i) + CalibratedX(i + 1)) / 2
                CummulativeSL = CummulativeSL + (((UpperLimit - LowerLimit) / 2) * (_FirstIntegration(i - 1) + _FirstIntegration(i + 1)))
                _SecondIntegration.Add(CummulativeSL)
            Next
            LowerLimit = (CalibratedX(CalibratedX.Count - 2) + CalibratedX(CalibratedX.Count - 1)) / 2
            UpperLimit = CalibratedX(CalibratedX.Count - 1)
            CummulativeSL = CummulativeSL + (((UpperLimit - LowerLimit) / 2) * (_FirstIntegration(CalibratedX.Count - 1)))
            _SecondIntegration.Add(CummulativeSL)

            '--------Finding Slope and deflection incorporating the integration constants
            Dim DisplacementMatrix(5) As Double
            DisplacementMatrix(0) = (_ActLCos * _GResultMatrix(0)) + (_ActMsin * _GResultMatrix(1))
            DisplacementMatrix(1) = (-_ActMsin * _GResultMatrix(0)) + (_ActLCos * _GResultMatrix(1))
            DisplacementMatrix(2) = _GResultMatrix(2)
            DisplacementMatrix(3) = (_ActLCos * _GResultMatrix(3)) + (_ActMsin * _GResultMatrix(4))
            DisplacementMatrix(4) = (-_ActMsin * _GResultMatrix(3)) + (_ActLCos * _GResultMatrix(4))
            DisplacementMatrix(5) = _GResultMatrix(5)

            Dim c1 As Double = -DisplacementMatrix(0) 'DisplacementX1
            Dim c2 As Double = -DisplacementMatrix(1) 'DisplacementY1
            Dim R1 As Double = -DisplacementMatrix(2) 'Rotation1
            Dim c3 As Double = -DisplacementMatrix(3) 'DisplacementX2
            Dim c4 As Double = -DisplacementMatrix(4) 'DisplacementY2
            Dim ConstX As Double = 0
            Dim ConstY As Double = 0

            Dim XDef As New List(Of Double)
            Dim YDef As New List(Of Double)
            Dim J As Integer = 0
            For Each Cx In CalibratedX
                If J < _FirstIntegration.Count - 1 And J < _SecondIntegration.Count - 1 Then
                    ConstX = c1 + ((c3 - c1) * (Cx / CalibratedX(CalibratedX.Count - 1)))
                    ConstY = c2 '+ ((c4 - c2) * (Cx / CalibratedX(CalibratedX.Count - 1)))
                    XDef.Add(ConstX)
                    YDef.Add(-((_SecondIntegration(J) / (ElDetails.I * ElDetails.E)) + (R1 * Cx) + ConstY))
                End If
                J = J + 1
            Next
            For i = 0 To (YDef.Count - 1)
                R.deflectionValuesX.Add(Math.Round(XDef(i), 10))
                R.deflectionValuesY.Add(Math.Round(YDef(i), 10))
            Next
        End Sub
#End Region

#Region "Axial Force Calculation"
        Private Function Total_AxialForce_L(ByVal _curDx As Double) As Double
            '-----Function Returns Total Axial Force in a point from left to right
            Dim AX As Double
            AX = AX_PointLoad_L(_curDx) + _
                 AX_UVL_L(_curDx) + _
                 AX_EndReaction_L(_curDx)
            Return AX
        End Function

        Private Function AX_PointLoad_L(ByVal _curDx As Double) As Double
            '----Axial Force due to point load from left to right
            Dim AX, T_AX As Double
            Dim intensity As Double
            For Each PL In ElDetails.Pload
                AX = 0
                If PL.MAlign = True Then
                    intensity = 0
                ElseIf PL.HAlign = True Then
                    intensity = (-PL.pload * Cos(ElDetails.Inclination)) '--From Left to right
                ElseIf PL.VAlign = True Then
                    intensity = (PL.pload * Sin(ElDetails.Inclination))
                End If
                If _curDx >= PL.plocation Then
                    AX = -intensity
                End If
                T_AX = T_AX + AX
            Next
            Return T_AX
        End Function

        Private Function AX_UVL_L(ByVal _curDx As Double) As Double
            '----Axial force due to UVL from left to right
            Dim _RectF, _TriF, _SecF, AX, T_AX As Double
            Dim intensity1, intensity2 As Double
            For Each UL In ElDetails.Uload
                If UL.MAlign = True Then
                    intensity1 = 0
                    intensity2 = 0
                ElseIf UL.HAlign = True Then
                    intensity1 = (-UL.uload1 * Cos(ElDetails.Inclination)) '--From Left to right
                    intensity2 = (-UL.uload2 * Cos(ElDetails.Inclination)) '--From Left to right
                ElseIf UL.VAlign = True Then
                    intensity1 = (UL.uload1 * Sin(ElDetails.Inclination))
                    intensity2 = (UL.uload2 * Sin(ElDetails.Inclination))
                End If

                If intensity1 <= intensity2 Then
                    AX = 0
                    If (_curDx >= UL.ulocation2) Then
                        _RectF = intensity1 * (UL.ulocation2 - UL.ulocation1)
                        _TriF = 0.5 * (intensity2 - intensity1) * (UL.ulocation2 - UL.ulocation1)
                        AX = _RectF + _TriF
                    ElseIf ((_curDx >= UL.ulocation1) And (_curDx < UL.ulocation2)) Then
                        _RectF = intensity1 * (_curDx - UL.ulocation1)
                        _SecF = ((intensity2 - intensity1) / (UL.ulocation2 - UL.ulocation1)) * _
                                (_curDx - UL.ulocation1)
                        _TriF = 0.5 * _SecF * (_curDx - UL.ulocation1)
                        AX = _RectF + _TriF
                    End If
                    T_AX = T_AX - AX
                Else
                    AX = 0
                    If (_curDx >= UL.ulocation2) Then
                        _RectF = intensity2 * (UL.ulocation2 - UL.ulocation1)
                        _TriF = 0.5 * (intensity1 - intensity2) * (UL.ulocation2 - UL.ulocation1)
                        AX = _RectF + _TriF
                    ElseIf ((_curDx >= UL.ulocation1) And (_curDx < UL.ulocation2)) Then
                        _SecF = intensity2 + _
                                                           (((intensity1 - intensity2) / (UL.ulocation2 - UL.ulocation1)) * (UL.ulocation2 - _curDx))
                        _RectF = _SecF * (_curDx - UL.ulocation1)
                        _TriF = 0.5 * (intensity1 - _SecF) * (_curDx - UL.ulocation1)
                        AX = _RectF + _TriF
                    End If
                    T_AX = T_AX - AX
                End If
            Next
            Return T_AX
        End Function

        Private Function AX_EndReaction_L(ByVal _curDx As Double) As Double
            '-----Bending moment due to end reaction force from Left to Right
            Dim AX As Double
            AX = -_LResultMatrix(0)
            If _curDx = ElDetails.Length Then
                AX = AX - (_LResultMatrix(3))
            End If
            Return AX
        End Function
#End Region

        Private Sub SetResult(ByRef R As ResultDrawingCoords)
            '----Calculating Shear
            R.shearValues = New List(Of Double)
            R.deflectionValuesX = New List(Of Double)
            R.deflectionValuesY = New List(Of Double)
            R.bendingValues = New List(Of Double)
            R.axialForcevalues = New List(Of Double)


            Dim i As Integer = 0
            Dim _dx As Double = Fixinterval(ElDetails.Length)
            Dim Cx As Double
            Dim CalibratedX As New List(Of Double)
            For Cx = 0 To ElDetails.Length Step _dx
                CalibratedX.Add(Cx)
            Next
            CalibratedX(CalibratedX.Count - 1) = ElDetails.Length


            For Each Cx In CalibratedX
                R.shearValues.Add(Math.Round(Total_ShearForce_L(Cx), 4))
                R.bendingValues.Add(Math.Round(Total_BendingMoment_L(Cx), 4))
                R.axialForcevalues.Add(Math.Round(Total_AxialForce_L(Cx), 4))
            Next
            FixDefelection(R, CalibratedX)
        End Sub
#End Region

        Public Sub FixResultValues()
            _ResulCoords._MemPoints = New List(Of Point)
            _ResulCoords._SFPoints = New List(Of Point)
            _ResulCoords._BMPoints = New List(Of Point)
            _ResulCoords._DEPoints = New List(Of Point)
            _ResulCoords._AXPoints = New List(Of Point)
            _ResulCoords._SFMcPointsindex = New List(Of Integer)
            _ResulCoords._BMMcPointsindex = New List(Of Integer)
            _ResulCoords._DEMcPointsindex = New List(Of Integer)
            _ResulCoords._AXMcPointsindex = New List(Of Integer)
            _ResulCoords.shearValues = New List(Of Double)
            _ResulCoords.bendingValues = New List(Of Double)
            _ResulCoords.deflectionValuesX = New List(Of Double)
            _ResulCoords.deflectionValuesY = New List(Of Double)
            _ResulCoords.beamValues = New List(Of Double)
            _ResulCoords.axialForcevalues = New List(Of Double)
            SetResult(_ResulCoords)
        End Sub

        Public Sub SetCoordinates()
            Dim ResultDispMax As Double = WA2dt.Loadhtfactor * 1.5
            If LoadScale(0) = 0 And _
               LoadScale(1) = 0 And _
               LoadScale(2) = 0 And _
               LoadScale(3) = 0 Then
                MsgBox("Something went wrong!!!!!!! InValid external load", MsgBoxStyle.Critical, "FrameANS")
                _Failed = True
                Exit Sub
            End If

            Dim _dx As Double = Fixinterval(ElDetails.Length * MDIMain.Nappdefaults.defaultScaleFactor)
            Dim _scaledLength As Double = (ElDetails.Length * MDIMain.Nappdefaults.defaultScaleFactor)
            Dim _incl As Double = (ElDetails.Inclination) '--- Corrected Bug
            Dim _origin As Point = New Point((ElDetails.SN.Coord.X + ElDetails.EN.Coord.X) / 2, _
                                             (ElDetails.SN.Coord.Y + ElDetails.EN.Coord.Y) / 2)
            Dim CDSindex As New List(Of Integer)
            Dim n As Integer
            Dim _Slopepositivedirection As Boolean


            '-------------------------------------------------------------------
            '-------Member Values - For referenece
            _ResulCoords._MemPoints.Add( _
                        New Point( _
                            (_origin.X) + (-(_scaledLength / 2) * Math.Cos(_incl) - 0 * Math.Sin(_incl)), _
                            (_origin.Y) + (-(_scaledLength / 2) * Math.Sin(_incl) + 0 * Math.Cos(_incl))))
            For i = -(_scaledLength / 2) To (_scaledLength / 2) Step _dx
                _ResulCoords._MemPoints.Add( _
                        New Point( _
                            (_origin.X) + (i * Math.Cos(_incl) - 0 * Math.Sin(_incl)), _
                            (_origin.Y) + (i * Math.Sin(_incl) + 0 * Math.Cos(_incl))))
            Next
            _ResulCoords._MemPoints.Add( _
                        New Point( _
                            (_origin.X) + ((_scaledLength / 2) * Math.Cos(_incl) - 0 * Math.Sin(_incl)), _
                            (_origin.Y) + ((_scaledLength / 2) * Math.Sin(_incl) + 0 * Math.Cos(_incl))))


            '-------------------------------------------------------------------
            '-------Shear Force Points
            Dim IndexV As Integer
            If LoadScale(0) <> 0 Then
                IndexV = 0
                _ResulCoords._SFPoints.Add( _
                            New Point( _
                                (_origin.X) + (-(_scaledLength / 2) * Math.Cos(_incl) - (0 * (ResultDispMax / LoadScale(0)) * Math.Sin(_incl))), _
                                (_origin.Y) + (-(_scaledLength / 2) * Math.Sin(_incl) + (0 * (ResultDispMax / LoadScale(0)) * Math.Cos(_incl)))))
                For i = -((_scaledLength / 2)) To ((_scaledLength / 2)) Step _dx
                    If IndexV = _ResulCoords.shearValues.Count Then Exit For
                    _ResulCoords._SFPoints.Add( _
                            New Point( _
                                (_origin.X) + (i * Math.Cos(_incl) - (_ResulCoords.shearValues(IndexV) * (ResultDispMax / LoadScale(0)) * Math.Sin(_incl))), _
                                (_origin.Y) + (i * Math.Sin(_incl) + (_ResulCoords.shearValues(IndexV) * (ResultDispMax / LoadScale(0)) * Math.Cos(_incl)))))
                    IndexV = IndexV + 1
                Next
                _ResulCoords._SFPoints.Add( _
                            New Point( _
                                (_origin.X) + ((_scaledLength / 2) * Math.Cos(_incl) - (0 * (ResultDispMax / LoadScale(0)) * Math.Sin(_incl))), _
                                (_origin.Y) + ((_scaledLength / 2) * Math.Sin(_incl) + (0 * (ResultDispMax / LoadScale(0)) * Math.Cos(_incl)))))
            End If
            '------Capturing the Shear Force indexes for displaying
            _ResulCoords._SFMcPointsindex.Add(1) 'Start Point (+1)
            _ResulCoords._SFMcPointsindex.Add(_ResulCoords.shearValues.Count - 2) 'End Point (-1)
            Dim _SFdx As Double = Fixinterval(ElDetails.Length)
            For Each pl In ElDetails.Pload
                If (pl.plocation / _SFdx) > 1 And (pl.plocation / _SFdx) < (_ResulCoords.shearValues.Count - 2) Then
                    If ((pl.plocation / _SFdx) - 2) < _ResulCoords._SFPoints.Count Then
                        _ResulCoords._SFMcPointsindex.Add((pl.plocation / _SFdx) - 2) ' Point Load (-2)
                    End If
                    If Ceiling((pl.plocation / _SFdx) + 2) < _ResulCoords._SFPoints.Count And Ceiling((pl.plocation / _SFdx) + 2) < _ResulCoords.shearValues.Count Then
                        _ResulCoords._SFMcPointsindex.Add(Ceiling((pl.plocation / _SFdx) + 2)) ' Point Load (+2)
                    End If
                End If
            Next
            For Each ul In ElDetails.Uload
                If ((ul.ulocation1 / _SFdx)) > 1 Then
                    If ((ul.ulocation1 / _SFdx) + 1) < _ResulCoords._SFPoints.Count Then
                        _ResulCoords._SFMcPointsindex.Add((ul.ulocation1 / _SFdx) + 1)  ' UL location1 (+1)
                    End If
                End If
                If ((ul.ulocation2 / _SFdx)) < (_ResulCoords.shearValues.Count - 2) And ((ul.ulocation2 / _SFdx)) < (_ResulCoords._SFPoints.Count - 2) Then
                    If ((ul.ulocation2 / _SFdx) - 1) < _ResulCoords._SFPoints.Count Then
                        _ResulCoords._SFMcPointsindex.Add((ul.ulocation2 / _SFdx) - 1) ' UL location2 (-1)
                    End If
                End If
            Next


            '-------------------------------------------------------------------
            '-------Bending Moment Points
            If LoadScale(1) <> 0 Then
                IndexV = 0
                _ResulCoords._BMPoints.Add( _
                            New Point( _
                                (_origin.X) + (-(_scaledLength / 2) * Math.Cos(_incl) - (0 * (ResultDispMax / LoadScale(1)) * Math.Sin(_incl))), _
                                (_origin.Y) + (-(_scaledLength / 2) * Math.Sin(_incl) + (0 * (ResultDispMax / LoadScale(1)) * Math.Cos(_incl)))))
                For i = -(_scaledLength / 2) To ((_scaledLength / 2)) Step _dx
                    If IndexV = _ResulCoords.bendingValues.Count Or IndexV = -1 Then Exit For
                    _ResulCoords._BMPoints.Add( _
                            New Point( _
                                (_origin.X) + ((i * Math.Cos(_incl)) - (_ResulCoords.bendingValues(IndexV) * (ResultDispMax / LoadScale(1)) * Math.Sin(_incl))), _
                                (_origin.Y) + ((i * Math.Sin(_incl)) + (_ResulCoords.bendingValues(IndexV) * (ResultDispMax / LoadScale(1)) * Math.Cos(_incl)))))
                    IndexV = IndexV + 1
                Next
                _ResulCoords._BMPoints.Add( _
                            New Point( _
                                (_origin.X) + ((_scaledLength / 2) * Math.Cos(_incl) - (0 * (ResultDispMax / LoadScale(1)) * Math.Sin(_incl))), _
                                (_origin.Y) + ((_scaledLength / 2) * Math.Sin(_incl) + (0 * (ResultDispMax / LoadScale(1)) * Math.Cos(_incl)))))

            End If
            '------Capturing the Bending Moment indexes for displaying
            n = _ResulCoords.bendingValues.Count - 1
            _Slopepositivedirection = If(_ResulCoords.shearValues(1) > 0, True, False)
            If Math.Round(_ResulCoords.bendingValues(0), 3) <> 0 Then
                _ResulCoords._BMMcPointsindex.Add(1)
            End If
            For i = 1 To n - 2
                If _ResulCoords.shearValues(i) > 0 And _Slopepositivedirection = False Then
                    _Slopepositivedirection = True
                    If Math.Round(_ResulCoords.bendingValues(i), 3) <> 0 Then
                        _ResulCoords._BMMcPointsindex.Add(i)
                    End If
                End If
                If _ResulCoords.shearValues(i) < 0 And _Slopepositivedirection = True Then
                    _Slopepositivedirection = False
                    If Math.Round(_ResulCoords.bendingValues(i), 3) <> 0 Then
                        _ResulCoords._BMMcPointsindex.Add(i)
                    End If
                End If
            Next
            If Math.Round(_ResulCoords.bendingValues(n - 1), 3) <> 0 Then
                _ResulCoords._BMMcPointsindex.Add(n - 2)
            End If



            '-------------------------------------------------------------------
            '-------Deflection values points
            If LoadScale(2) <> 0 Then
                IndexV = 0
                For i = -(_scaledLength / 2) To ((_scaledLength / 2)) Step _dx
                    If IndexV = _ResulCoords.deflectionValuesY.Count Then Exit For
                    _ResulCoords._DEPoints.Add( _
                            New Point( _
                                (_origin.X) + ((i + (_ResulCoords.deflectionValuesX(IndexV) * (ResultDispMax / LoadScale(2)))) * Math.Cos(_incl) - (_ResulCoords.deflectionValuesY(IndexV) * (ResultDispMax / LoadScale(2)) * Math.Sin(_incl))), _
                                (_origin.Y) + ((i + (_ResulCoords.deflectionValuesX(IndexV) * (ResultDispMax / LoadScale(2)))) * Math.Sin(_incl) + (_ResulCoords.deflectionValuesY(IndexV) * (ResultDispMax / LoadScale(2)) * Math.Cos(_incl)))))
                    IndexV = IndexV + 1
                Next
            End If
            '------Capturing the deflection indexes for displaying
            Dim MaxPositiveDeflection As Double = 0
            Dim MaxNegativeDeflection As Double = 0
            For Each DE In _ResulCoords.deflectionValuesY
                If DE < 0 Then
                    If DE < MaxNegativeDeflection Then
                        MaxNegativeDeflection = DE
                    End If
                End If
                If DE > 0 Then
                    If DE > MaxPositiveDeflection Then
                        MaxPositiveDeflection = DE
                    End If
                End If
            Next
            If MaxPositiveDeflection > Math.Abs(MaxNegativeDeflection) Then
                If MaxNegativeDeflection <> 0 Then
                    If MaxPositiveDeflection / Math.Abs(MaxNegativeDeflection) > 10 Then
                        MaxNegativeDeflection = 0
                    End If
                End If
            Else
                If MaxPositiveDeflection <> 0 Then
                    If Math.Abs(MaxNegativeDeflection) / MaxPositiveDeflection > 10 Then
                        MaxPositiveDeflection = 0
                    End If
                End If
            End If
            If MaxPositiveDeflection <> 0 Then
                If _ResulCoords.deflectionValuesY.IndexOf(MaxPositiveDeflection) < (_ResulCoords._DEPoints.Count) Then
                    _ResulCoords._DEMcPointsindex.Add(_ResulCoords.deflectionValuesY.IndexOf(MaxPositiveDeflection)) 'Maximum Positive 
                End If
            End If
            If MaxNegativeDeflection <> 0 Then
                If _ResulCoords.deflectionValuesY.IndexOf(MaxNegativeDeflection) < (_ResulCoords._DEPoints.Count) Then
                    _ResulCoords._DEMcPointsindex.Add(_ResulCoords.deflectionValuesY.IndexOf(MaxNegativeDeflection)) 'Maximum Negative 
                End If
            End If


            '-------------------------------------------------------------------
            '-------Axial force points
            If LoadScale(3) <> 0 Then
                IndexV = 0
                _ResulCoords._AXPoints.Add( _
                                       New Point( _
                                           (_origin.X) + (-(_scaledLength / 2) * Math.Cos(_incl) - (0 * (ResultDispMax / LoadScale(3)) * Math.Sin(_incl))), _
                                           (_origin.Y) + (-(_scaledLength / 2) * Math.Sin(_incl) + (0 * (ResultDispMax / LoadScale(3)) * Math.Cos(_incl)))))
                For i = -(_scaledLength / 2) To ((_scaledLength / 2)) Step _dx
                    If IndexV = _ResulCoords.axialForcevalues.Count Then Exit For
                    _ResulCoords._AXPoints.Add( _
                            New Point( _
                                (_origin.X) + (i * Math.Cos(_incl) - (_ResulCoords.axialForcevalues(IndexV) * (ResultDispMax / LoadScale(3)) * Math.Sin(_incl))), _
                                (_origin.Y) + (i * Math.Sin(_incl) + (_ResulCoords.axialForcevalues(IndexV) * (ResultDispMax / LoadScale(3)) * Math.Cos(_incl)))))
                    IndexV = IndexV + 1
                Next
                _ResulCoords._AXPoints.Add( _
                           New Point( _
                               (_origin.X) + ((_scaledLength / 2) * Math.Cos(_incl) - (0 * (ResultDispMax / LoadScale(3)) * Math.Sin(_incl))), _
                               (_origin.Y) + ((_scaledLength / 2) * Math.Sin(_incl) + (0 * (ResultDispMax / LoadScale(3)) * Math.Cos(_incl)))))
            End If
            '------Capturing the Axial Force indexes for displaying
            _ResulCoords._AXMcPointsindex.Add(1) 'Start Point (+1)
            _ResulCoords._AXMcPointsindex.Add(_ResulCoords.axialForcevalues.Count - 2) 'End Point (-1)
        End Sub
    End Class
#End Region

    Public Sub New(Optional ByVal Tcpass As Boolean = False)
        _updater = New AnalyzerUpdate
        _updater.Button1.Visible = False
        _updater.Button2.Visible = False
        _updater.Button3.Visible = False

        _updateStr = "------------- Finite Element Analyzer ----------------" & vbNewLine & _
                    "----------------- Programmed by ----------------------" & vbNewLine & _
                    "------------------ Samson Mano -----------------------" & vbNewLine & _
                    "------------------------------------------------------" & vbNewLine & _
                    "--- The horse is made ready for the day of battle, but victory" & vbNewLine & _
                    "--- rests with the LORD <Proverb 21:31>" & vbNewLine & _
                    "--- Unless the LORD builds the house, its builders labor in vain. Unless the" & vbNewLine & _
                    "--- LORD watches over the city, the watchmen stand guard in vain. <Psalm 127:1>" & vbNewLine & _
                    vbNewLine & _
                    vbNewLine
        '---------Check Structure Feasibility for Analysis
        _Failed = PreliminaryCheck()
        _updater._ResultTxtBox.Text = _updateStr
        If Failed = True Then
            _updater.Button1.Visible = False
            _updater.Button2.Text = "Back"
            _updater.Button2.Visible = True
            _updater.Button3.Visible = False
            _updater.ShowDialog()
            Exit Sub
        End If

        '----Starting Analysis

        _ResultMem = New List(Of AnalysisResultMem)
        _ResultNode = New List(Of AnalysisResultNode)
        For Each E1 In WA2dt.Mem
            _ResultMem.Add(New AnalysisResultMem(E1))
        Next
        _updater._ResultTxtBox.Text = ""

        _updateStr = "------------- Finite Element Analyzer ----------------" & vbNewLine & _
                          "----------------- Programmed by ----------------------" & vbNewLine & _
                          "------------------ Samson Mano -----------------------" & vbNewLine & _
                          "------------------------------------------------------" & vbNewLine & _
                          "------------------------------------------------------" & vbNewLine & _
                          "------------------------------------------------------" & vbNewLine & _
                          "------------------------------------------------------" & vbNewLine & _
                          "------------------------------------------------------" & vbNewLine & _
                          vbNewLine & _
                          vbNewLine

        '_____________________________________________________________________________________________
        _updateStr = _updateStr & "----> Element - Degree Of Freedom Matrix <----" & vbNewLine & vbNewLine
        For Each itm In _ResultMem
            _updateStr = _updateStr & "Member :" & (_ResultMem.IndexOf(itm) + 1) _
                                        & "( " & itm.StartNodeNo & " ---> " & itm.EndNodeNo & " )" _
                                        & vbTab & " =  |   " & vbTab & itm._DegreeOFFreedomMatrix(0) _
                                        & vbTab & vbTab
            _updateStr = _updateStr & itm._DegreeOFFreedomMatrix(1) & vbTab & vbTab
            _updateStr = _updateStr & itm._DegreeOFFreedomMatrix(2) & vbTab & vbTab
            _updateStr = _updateStr & itm._DegreeOFFreedomMatrix(3) & vbTab & vbTab
            _updateStr = _updateStr & itm._DegreeOFFreedomMatrix(4) & vbTab & vbTab
            _updateStr = _updateStr & itm._DegreeOFFreedomMatrix(5) & vbTab & "   |" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr
        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Element - Direction Cosine Matrix <----" & vbNewLine & vbNewLine
        For Each itm In _ResultMem
            _updateStr = _updateStr & "Member :" & (_ResultMem.IndexOf(itm) + 1) _
                                        & "( " & itm.StartNodeNo & " ---> " & itm.EndNodeNo & " )" _
                                        & vbTab & " =  |   " & vbTab & itm.ActLcos _
                                        & vbTab & vbTab
            _updateStr = _updateStr & itm.ActMsin & vbTab & "   |" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr
        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Element Stiffness Matrix <----" & vbNewLine
        For Each itm In _ResultMem
            _updateStr = _updateStr & vbNewLine & "Member " & (_ResultMem.IndexOf(itm) + 1) _
                                         & "( " & itm.StartNodeNo & " ---> " & itm.EndNodeNo & " )" _
                                         & vbNewLine
            For p = 0 To 5
                _updateStr = _updateStr & "|"
                For t = 0 To 5
                    _updateStr = _updateStr & vbTab & Math.Round(itm._GElementStiffnessMatrix(p, t), 2)
                Next
                _updateStr = _updateStr & vbTab & "|" & vbNewLine
            Next
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Element - Fixed End Reaction Matrix <----" & vbNewLine & vbNewLine
        For Each itm In _ResultMem
            _updateStr = _updateStr & "Member " & (_ResultMem.IndexOf(itm) + 1) _
                                           & "( " & itm.StartNodeNo & " ---> " & itm.EndNodeNo & " )" _
                                           & vbTab & " =  |   " & vbTab & itm._GLoadVectorMatrix(0) & vbTab & vbTab
            _updateStr = _updateStr & itm._GLoadVectorMatrix(1) & vbTab & vbTab
            _updateStr = _updateStr & itm._GLoadVectorMatrix(2) & vbTab & vbTab
            _updateStr = _updateStr & itm._GLoadVectorMatrix(3) & vbTab & vbTab
            _updateStr = _updateStr & itm._GLoadVectorMatrix(4) & vbTab & vbTab
            _updateStr = _updateStr & itm._GLoadVectorMatrix(5) & vbTab & "   |" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        Dim GlobalStiffnessMatrix((WA2dt.Bob.Count * 3) - 1, (WA2dt.Bob.Count * 3) - 1) As Double
        Dim GlobalDOFMatrix((WA2dt.Bob.Count * 3) - 1) As Double
        Dim GlobalReactionMatrix((WA2dt.Bob.Count * 3) - 1) As Double
        Dim GlobalSettlementMatrix((WA2dt.Bob.Count * 3) - 1) As Double
        For Each resultM In _ResultMem
            resultM.FixGlobalMatrices(GlobalDOFMatrix, GlobalReactionMatrix, GlobalSettlementMatrix, GlobalStiffnessMatrix)
        Next
        '____Spring Stiffness - redistribution to global stiffness matrix_____
        Call SpringStiffnessRedistribution(GlobalStiffnessMatrix)
        '____Settlement and Sliding of support - redistribution to global force matrix______
        Call SlidingRedistribution(GlobalStiffnessMatrix, GlobalSettlementMatrix, GlobalReactionMatrix)


        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Global - Degree Of Freedom Matrix <----" _
                                                     & vbTab & vbTab _
                                                     & "----> Global - Fixed End Reaction Matrix <----" _
                                                     & vbNewLine & vbNewLine
        For p = 0 To ((WA2dt.Bob.Count * 3) - 1)
            _updateStr = _updateStr & vbTab & "|  " & vbTab & GlobalDOFMatrix(p) & vbTab & "  |" & vbTab & vbTab & vbTab
            _updateStr = _updateStr & vbTab & "|  " & vbTab & GlobalReactionMatrix(p) & vbTab & "  |" & vbTab & vbTab & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Global Stiffness Matrix <----" & vbNewLine
        For p = 0 To ((WA2dt.Bob.Count * 3) - 1)
            _updateStr = _updateStr & "|"
            For t = 0 To ((WA2dt.Bob.Count * 3) - 1)
                _updateStr = _updateStr & vbTab & Math.Round(GlobalStiffnessMatrix(p, t), 2)
            Next
            _updateStr = _updateStr & vbTab & "|" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr


        Dim CBound As Integer
        Call Curtailment(GlobalDOFMatrix, GlobalReactionMatrix, GlobalStiffnessMatrix, CBound)

        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Global Stiffness Matrix After Curtailment <----" & vbNewLine
        For p = 0 To (CBound - 1)
            _updateStr = _updateStr & "|"
            For t = 0 To (CBound - 1)
                _updateStr = _updateStr & vbTab & Math.Round(GlobalStiffnessMatrix(p, t), 2)
            Next
            _updateStr = _updateStr & vbTab & "|" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        Dim ResultMatrix(CBound - 1) As Double
        Call Gauss(GlobalStiffnessMatrix, GlobalReactionMatrix, ResultMatrix, CBound - 1)

        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Result Matrix <----" & vbNewLine & vbNewLine
        For p = 0 To (CBound - 1)
            _updateStr = _updateStr & "|  " & vbTab & ResultMatrix(p) & vbTab & "  |" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr




        ReDim GlobalStiffnessMatrix((WA2dt.Bob.Count * 3) - 1, (WA2dt.Bob.Count * 3) - 1)
        ReDim GlobalDOFMatrix((WA2dt.Bob.Count * 3) - 1)
        ReDim GlobalReactionMatrix((WA2dt.Bob.Count * 3) - 1)
        ReDim GlobalSettlementMatrix((WA2dt.Bob.Count * 3) - 1)
        For Each resultM In _ResultMem
            resultM.FixGlobalMatrices(GlobalDOFMatrix, GlobalReactionMatrix, GlobalSettlementMatrix, GlobalStiffnessMatrix)
        Next
        '____Spring Stiffness - redistribution to global stiffness matrix_____
        Call SpringStiffnessRedistribution(GlobalStiffnessMatrix)
        Call Welding(ResultMatrix, GlobalSettlementMatrix, GlobalDOFMatrix)

        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Result Matrix After Welding <----" & vbNewLine & vbNewLine
        For p = 0 To ((WA2dt.Bob.Count * 3) - 1)
            _updateStr = _updateStr & "|  " & vbTab & ResultMatrix(p) & vbTab & "  |" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        Dim GlobalNodalResultantForce((WA2dt.Bob.Count * 3) - 1) As Double
        Call GMultiplier(GlobalNodalResultantForce, ResultMatrix, GlobalStiffnessMatrix, GlobalReactionMatrix)

        For p = 0 To ((WA2dt.Bob.Count) - 1)
            Dim TempNoderesultant As New AnalysisResultNode
            If WA2dt.Bob(p).Support.SS = True Then
                TempNoderesultant.HForce = GlobalNodalResultantForce((p * 3) + 0)
                TempNoderesultant.VForce = GlobalNodalResultantForce((p * 3) + 1) + (-WA2dt.Bob(p).Support.stiffnessK * ResultMatrix((p * 3) + 1))
                TempNoderesultant.Moment = GlobalNodalResultantForce((p * 3) + 2)
                TempNoderesultant.GlobalX = ResultMatrix((p * 3) + 0)
                TempNoderesultant.GlobalY = ResultMatrix((p * 3) + 1)
                TempNoderesultant.GlobalRotation = ResultMatrix((p * 3) + 2)
                TempNoderesultant._NodeNo = p
            Else
                TempNoderesultant.HForce = GlobalNodalResultantForce((p * 3) + 0) '* Math.Cos((WA2dt.Bob(p).Support.supportinclination - (Math.PI / 2))) + GlobalNodalResultantForce((p * 3) + 1) * Math.Sin((WA2dt.Bob(p).Support.supportinclination - (Math.PI / 2)))
                TempNoderesultant.VForce = GlobalNodalResultantForce((p * 3) + 1) '* -Math.Sin((WA2dt.Bob(p).Support.supportinclination - (Math.PI / 2))) + GlobalNodalResultantForce((p * 3) + 1) * Math.Cos((WA2dt.Bob(p).Support.supportinclination - (Math.PI / 2)))
                TempNoderesultant.Moment = GlobalNodalResultantForce((p * 3) + 2)
                TempNoderesultant.GlobalX = ResultMatrix((p * 3) + 0)
                TempNoderesultant.GlobalY = ResultMatrix((p * 3) + 1)
                TempNoderesultant.GlobalRotation = ResultMatrix((p * 3) + 2)
                TempNoderesultant._NodeNo = p
            End If
            _ResultNode.Add(TempNoderesultant)
        Next

        For Each resultM In _ResultMem
            resultM.FixResultMatrix(ResultMatrix)
        Next
        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Element - Result Matrix <----" & vbNewLine & vbNewLine
        _updateStr = _updateStr & vbTab & vbTab _
                                & vbTab & vbTab & " RA " & vbTab & vbTab & " FA " & vbTab & vbTab & " MA " _
                                & vbTab & vbTab & " RB " & vbTab & vbTab & " FB " & vbTab & vbTab & " MB " & vbNewLine


        For Each itm In _ResultMem
            _updateStr = _updateStr & "Member :" & (_ResultMem.IndexOf(itm) + 1) _
                                        & "( " & itm.StartNodeNo & " ---> " & itm.EndNodeNo & " )" _
                                        & vbTab & " =  |   " & vbTab & itm._LResultMatrix(0) _
                                        & vbTab & vbTab
            _updateStr = _updateStr & itm._LResultMatrix(1) & vbTab & vbTab
            _updateStr = _updateStr & itm._LResultMatrix(2) & vbTab & vbTab
            _updateStr = _updateStr & itm._LResultMatrix(3) & vbTab & vbTab
            _updateStr = _updateStr & itm._LResultMatrix(4) & vbTab & vbTab
            _updateStr = _updateStr & itm._LResultMatrix(5) & vbTab & "   |" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        '_____________________________________________________________________________________________
        _updateStr = vbNewLine & vbNewLine
        _updateStr = _updateStr & "----> Nodal Resultant Matrix <----" & vbNewLine & vbNewLine
        For p = 0 To ((WA2dt.Bob.Count * 3) - 1)
            _updateStr = _updateStr & "|  " & vbTab & Round(GlobalNodalResultantForce(p), 2) & vbTab & "  |" & vbNewLine
        Next
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr


        _updater.ResultStatus.Text = "Status: Success"
        For i = 0 To ((WA2dt.Bob.Count * 3) - 1)
            If GlobalDOFMatrix(i) = 1 Then
                If Round(GlobalNodalResultantForce(i), 1) <> 0 Then
                    _updater.ResultStatus.Text = "Status: Nodal Resultant Higher than 0.1"
                    Exit For
                End If
            End If
        Next


        '------------------------------- Analysis Complete
        Call BeamCoordinates()


        _updater.Button1.Visible = True
        _updater.Button2.Text = "View Result"
        _updater.Button2.Visible = True
        _updater.Button3.Visible = True
        _updater.ShowDialog()
    End Sub

#Region "Preliminary Checking For Structure, Load & Support"
    Private Function PreliminaryCheck()
        Dim ChkResult As Boolean = False
        CorrectNodeSupportError()
        CheckStructure(ChkResult)
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        CheckSupport(ChkResult)
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        CheckLoad(ChkResult)
        _updater._ResultTxtBox.Text = _updater._ResultTxtBox.Text & _updateStr

        Return ChkResult
    End Function

#Region "Correct Node Support Error"
    Private Structure Repetation
        Dim NdNumber As Integer
        Dim ReOccurMember As List(Of Integer)
    End Structure

    Private Sub CorrectNodeSupportError()
        '----Sort nodes based on Repetation count
        Dim RCount As New List(Of Repetation)
        For Each Nd In WA2dt.Bob
            Dim TReap As New Repetation
            TReap.ReOccurMember = New List(Of Integer)
            For Each El In WA2dt.Mem
                If Nd.index = El.SN.index Then
                    TReap.ReOccurMember.Add(WA2dt.Mem.IndexOf(El))
                    Continue For
                End If
                If Nd.index = El.EN.index Then
                    TReap.ReOccurMember.Add(WA2dt.Mem.IndexOf(El))
                    Continue For
                End If
            Next
            TReap.NdNumber = WA2dt.Bob.IndexOf(Nd)
            RCount.Add(TReap)
        Next
        Dim PooF As New List(Of Integer)
        For Each Rc In RCount
            Dim SupportRevised As Line2DT.Node.SupportCondition
            For Each ReO In Rc.ReOccurMember
                If WA2dt.Mem(ReO).SN.index = WA2dt.Bob(Rc.NdNumber).index Then
                    If WA2dt.Mem(ReO).SN.Support.PS = True Or _
                        WA2dt.Mem(ReO).SN.Support.PRS = True Or _
                        WA2dt.Mem(ReO).SN.Support.FRS = True Or _
                        WA2dt.Mem(ReO).SN.Support.FS = True Or _
                        WA2dt.Mem(ReO).SN.Support.SS = True Then
                        SupportRevised = New  _
                                    Line2DT.Node.SupportCondition(False, _
                                                False, _
                                                WA2dt.Mem(ReO).SN.Support.PS, _
                                                WA2dt.Mem(ReO).SN.Support.PRS, _
                                                WA2dt.Mem(ReO).SN.Support.FRS, _
                                                WA2dt.Mem(ReO).SN.Support.FS, _
                                                WA2dt.Mem(ReO).SN.Support.SS, _
                                                WA2dt.Mem(ReO).SN.Support.supportinclination, _
                                                WA2dt.Mem(ReO).SN.Support.settlementdx, _
                                                WA2dt.Mem(ReO).SN.Support.settlementdy, _
                                                WA2dt.Mem(ReO).SN.Support.stiffnessK)
                        Exit For
                    ElseIf WA2dt.Mem(ReO).SN.Support.PJ = True Or _
                            WA2dt.Mem(ReO).SN.Support.RJ = True Then
                        SupportRevised = New  _
                                    Line2DT.Node.SupportCondition( _
                                                WA2dt.Mem(ReO).SN.Support.PJ, _
                                                WA2dt.Mem(ReO).SN.Support.RJ, _
                                                False, _
                                                False, _
                                                False, _
                                                False, _
                                                False, _
                                                Nothing, _
                                                Nothing, _
                                                Nothing, _
                                                Nothing)
                        Continue For
                    Else
                        PooF.Add(ReO)
                        Continue For
                    End If
                ElseIf WA2dt.Mem(ReO).EN.index = WA2dt.Bob(Rc.NdNumber).index Then
                    If WA2dt.Mem(ReO).EN.Support.PS = True Or _
                        WA2dt.Mem(ReO).EN.Support.PRS = True Or _
                        WA2dt.Mem(ReO).EN.Support.FRS = True Or _
                        WA2dt.Mem(ReO).EN.Support.FS = True Or _
                        WA2dt.Mem(ReO).EN.Support.SS = True Then
                        SupportRevised = New  _
                                    Line2DT.Node.SupportCondition(False, _
                                                False, _
                                                WA2dt.Mem(ReO).EN.Support.PS, _
                                                WA2dt.Mem(ReO).EN.Support.PRS, _
                                                WA2dt.Mem(ReO).EN.Support.FRS, _
                                                WA2dt.Mem(ReO).EN.Support.FS, _
                                                WA2dt.Mem(ReO).EN.Support.SS, _
                                                WA2dt.Mem(ReO).EN.Support.supportinclination, _
                                                WA2dt.Mem(ReO).EN.Support.settlementdx, _
                                                WA2dt.Mem(ReO).EN.Support.settlementdy, _
                                                WA2dt.Mem(ReO).EN.Support.stiffnessK)
                        Exit For
                    ElseIf WA2dt.Mem(ReO).EN.Support.PJ = True Or _
                            WA2dt.Mem(ReO).EN.Support.RJ = True Then
                        SupportRevised = New  _
                                    Line2DT.Node.SupportCondition( _
                                                WA2dt.Mem(ReO).EN.Support.PJ, _
                                                WA2dt.Mem(ReO).EN.Support.RJ, _
                                                False, _
                                                False, _
                                                False, _
                                                False, _
                                                False, _
                                                Nothing, _
                                                Nothing, _
                                                Nothing, _
                                                Nothing)
                        Continue For
                    Else
                        PooF.Add(ReO)
                        Continue For
                    End If
                End If
            Next

            For Each ReO In Rc.ReOccurMember
                If WA2dt.Mem(ReO).SN.index = WA2dt.Bob(Rc.NdNumber).index Then
                    WA2dt.Mem(ReO).SN.Support = SupportRevised
                ElseIf WA2dt.Mem(ReO).EN.index = WA2dt.Bob(Rc.NdNumber).index Then
                    WA2dt.Mem(ReO).EN.Support = SupportRevised
                End If
            Next
            WA2dt.Bob(Rc.NdNumber).Support = SupportRevised
        Next

        For Each leftOut In PooF
            If WA2dt.Mem(leftOut).SN.Support.PJ = False And _
                WA2dt.Mem(leftOut).SN.Support.RJ = False And _
                WA2dt.Mem(leftOut).SN.Support.PS = False And _
                WA2dt.Mem(leftOut).SN.Support.PRS = False And _
                WA2dt.Mem(leftOut).SN.Support.FRS = False And _
                WA2dt.Mem(leftOut).SN.Support.FS = False And _
                WA2dt.Mem(leftOut).SN.Support.SS = False Then

                MsgBox("Something gone Wrong !!!!! Its My Fault", MsgBoxStyle.Critical, "FrameANS")
            ElseIf WA2dt.Mem(leftOut).EN.Support.PJ = False And _
                WA2dt.Mem(leftOut).EN.Support.RJ = False And _
                WA2dt.Mem(leftOut).EN.Support.PS = False And _
                WA2dt.Mem(leftOut).EN.Support.PRS = False And _
                WA2dt.Mem(leftOut).EN.Support.FRS = False And _
                WA2dt.Mem(leftOut).EN.Support.FS = False And _
                WA2dt.Mem(leftOut).EN.Support.SS = False Then

                MsgBox("Something gone Wrong !!!!! Its My Fault", MsgBoxStyle.Critical, "FrameANS")
            End If
        Next
    End Sub
#End Region

    Private Sub CheckStructure(ByRef ChkFlg As Boolean)
        '        Exit Sub

        '        '---------Structure Stability Over Connectivity
        '        Dim Sl As New List(Of Integer)
        '        Dim I As Integer = 0
        '        ' ------ Finding the isolated nodes
        '        For Each node In WA2dt.Bob
        '            I = 0
        '            For Each EL In WA2dt.Mem
        '                If EL.SN.Equals(node) = True Or EL.EN.Equals(node) = True Then
        '                    I = I + 1
        '                    If I = 2 Then
        '                        GoTo S1
        '                    End If
        '                End If
        '            Next
        '            Sl.Add(WA2dt.Bob.IndexOf(node))
        'S1:
        '        Next

        '        ' ------- Checking the boundary conditions of the isolated nodes
        '        Dim ErrNodenos As New List(Of Integer)
        '        For Each I In Sl
        '            If (WA2dt.Bob(I).Support.PJ = True Or WA2dt.Bob(I).Support.RJ = True) Then
        '                ErrNodenos.Add(I)
        '            End If
        '        Next

        '        Sl.Clear()
        '        ' ------- Finding the other node of the isolated node to chk whether it is rigid or with support
        '        Dim OtherNode As New List(Of Line2DT.Node)
        '        For Each El In WA2dt.Mem
        '            For Each I In ErrNodenos
        '                If WA2dt.Bob(I).Coord.Equals(El.SN.Coord) Then
        '                    If El.EN.Support.PJ = True Or El.EN.Support.PS = True Or El.EN.Support.RS = True Or El.EN.Support.SS = True Then
        '                        Sl.Add(I)
        '                        Exit For
        '                    End If
        '                End If
        '                If WA2dt.Bob(I).Coord.Equals(El.EN.Coord) Then
        '                    If El.SN.Support.PJ = True Or El.SN.Support.PS = True Or El.SN.Support.RS = True Or El.SN.Support.SS = True Then
        '                        Sl.Add(I)
        '                        Exit For
        '                    End If
        '                End If
        '            Next
        '        Next
        '        If Sl.Count <> 0 Or WA2dt.Mem.Count = 0 Then
        '            _updateStr = " Checking Structure" & _
        '                                          " ........................" & _
        '                                          " Failed"
        '            For Each itm In Sl
        '                _updateStr = _updateStr & vbNewLine & " Problem with Node" & _
        '             " - " & _
        '             (itm)
        '            Next
        '            _updateStr = _updateStr & vbNewLine
        '            ChkFlg = True
        '            Exit Sub
        '        End If

        '        _updateStr = vbNewLine & " Checking Structure" & _
        '             " ........................" & _
        '             " Ok" & _
        '                vbNewLine
    End Sub

    Private Sub CheckSupport(ByRef ChkFlg As Boolean)
        Dim I As Integer = 0
        For Each node In WA2dt.Bob
            If node.Support.FS = True Or node.Support.RJ = True Then
                I = 2
            End If
            If node.Support.PS = True Or node.Support.PRS = True Or node.Support.FRS = True Or node.Support.SS = True Then
                I = I + 1
            End If
        Next

        If I < 2 Then
            _updateStr = vbNewLine & " Checking Support" & _
             " ........................" & _
             " Failed" & _
                vbNewLine
            ChkFlg = True
            Exit Sub
        End If

        _updateStr = vbNewLine & " Checking Support" & _
        " ........................" & _
        " Ok" & _
           vbNewLine
    End Sub

    Private Sub CheckLoad(ByRef ChkFlg As Boolean)
        Dim I As Integer = 0

        For Each itm In WA2dt.Mem
            If itm.Pload.Count <> 0 Or itm.Uload.Count <> 0 Or itm.Mload.Count <> 0 Then
                I = I + 1
            End If
        Next

        If I = 0 Then
            _updateStr = vbNewLine & " Checking Load" & _
             " ........................" & _
             " Failed" & _
                vbNewLine
            ChkFlg = True
            Exit Sub
        End If

        _updateStr = vbNewLine & " Checking Load" & _
        " ........................" & _
        " Ok" & _
           vbNewLine
    End Sub
#End Region

#Region "Gauss Elimination Method"
    '-----Redefined Gauss Elimination Procedure
    Private Sub Gauss(ByVal A(,) As Double, ByVal B() As Double, ByRef X() As Double, ByVal Bound As Integer)
        Dim Triangular_A(Bound, Bound + 1) As Double
        Dim soln(Bound) As Double 'Solution matrix
        For m = 0 To Bound
            For n = 0 To Bound
                Triangular_A(m, n) = A(m, n)
            Next
        Next
        '.... substituting the force to triangularmatrics....
        For n = 0 To Bound
            Triangular_A(n, Bound + 1) = B(n)
        Next
        ForwardSubstitution(Triangular_A, Bound)
        ReverseElimination(Triangular_A, X, Bound)
    End Sub

    Private Sub ForwardSubstitution(ByRef _triang(,) As Double, ByVal bound As Integer)
        'Forward Elimination
        'Dim _fraction As Double
        For k = 0 To bound - 1
            For i = k + 1 To bound
                If _triang(k, k) = 0 Then
                    Continue For
                End If
                _triang(i, k) = _triang(i, k) / _triang(k, k)
                For j = k + 1 To bound + 1
                    _triang(i, j) = _triang(i, j) - (_triang(i, k) * _triang(k, j))
                Next
            Next
        Next
    End Sub

    Private Sub ReverseElimination(ByRef _triang(,) As Double, ByRef X() As Double, ByVal bound As Integer)
        'Back Substitution
        For i = 0 To bound
            X(i) = _triang(i, bound + 1)
        Next

        For i = bound To 0 Step -1
            For j = i + 1 To bound
                X(i) = X(i) - (_triang(i, j) * X(j))
            Next
            X(i) = X(i) / _triang(i, i)
        Next
    End Sub
#End Region

#Region "Gauss Elimination Method Outdated"
    Private Sub GElimination(ByRef A(,) As Double, ByRef B() As Double, ByRef re() As Double, ByVal cb As Integer)
        '----Check For Uncertainity :)
        If WA2dt.Mem.Count - 1 <= 0 Then
            Exit Sub
        End If

        Dim Triangular_A(cb, cb + 1), line_1, temporary_1, multiplier_1, sum_1 As Double
        Dim soln(cb + 1) As Double
        For n = 0 To cb
            For m = 0 To cb
                Triangular_A(m, n) = A(m, n)
            Next
        Next

        '.... substituting the force to triangularmatrics....
        For n = 0 To cb
            Triangular_A(n, cb + 1) = B(n)
        Next

        '...............soving the triangular matrics.............
        For k = 0 To cb
            '......Bring a non-zero element first by changes lines if necessary
            If Triangular_A(k, k) = 0 Then
                For n = k To cb
                    If Triangular_A(n, k) <> 0 Then line_1 = n : Exit For 'Finds line_1 with non-zero element
                Next n
                '..........Change line k with line_1
                For m = k To cb
                    temporary_1 = Triangular_A(k, m)
                    Triangular_A(k, m) = Triangular_A(line_1, m)
                    Triangular_A(line_1, m) = temporary_1
                Next m
            End If
            '....For other lines, make a zero element by using:
            '.........Ai1=Aij-A11*(Aij/A11)
            '.....and change all the line using the same formula for other elements
            For n = k + 1 To cb
                If Triangular_A(n, k) <> 0 Then 'if it is zero, stays as it is
                    multiplier_1 = Triangular_A(n, k) / Triangular_A(k, k)
                    For m = k To cb + 1
                        Triangular_A(n, m) = Triangular_A(n, m) - Triangular_A(k, m) * multiplier_1
                    Next m
                End If
            Next n
        Next k


        '..... calculating the dof value..........

        'First, calculate last xi (for i = System_DIM)
        soln(cb + 1) = Triangular_A(cb, cb + 1) / Triangular_A(cb, cb)

        '................
        For n = 1 To cb
            sum_1 = 0
            For m = 1 To n
                sum_1 = sum_1 + soln(cb + 2 - m) * Triangular_A(cb - n, cb + 1 - m)
            Next m
            soln(cb + 1 - n) = (Triangular_A(cb - n, cb + 1) - sum_1) / Triangular_A(cb - n, cb - n)

        Next n

        For n = 0 To cb
            re(n) = soln(n + 1)
        Next
    End Sub
#End Region

#Region "Curtailment & Welding of Matrices"
    Public Sub Curtailment(ByRef DOFmatrix() As Double, ByRef FERmatrix() As Double, ByRef GSmatrix(,) As Double, ByRef CBound As Integer)
        Dim tgm((WA2dt.Bob.Count * 3) - 1, (WA2dt.Bob.Count * 3) - 1) As Double
        Dim tdofm((WA2dt.Bob.Count * 3) - 1) As Integer
        Dim tferm((WA2dt.Bob.Count * 3) - 1) As Double

        Dim r, s As Integer
        For p = 0 To ((WA2dt.Bob.Count * 3) - 1)
            If DOFmatrix(p) = 0 Then
                Continue For
            Else
                s = 0
                For t = 0 To ((WA2dt.Bob.Count * 3) - 1)
                    If DOFmatrix(t) = 0 Then
                        Continue For
                    Else
                        tferm(s) = FERmatrix(t)
                        tdofm(s) = DOFmatrix(t)
                        tgm(r, s) = GSmatrix(p, t)
                        s = s + 1
                    End If
                Next
                r = r + 1
            End If
        Next

        ReDim GSmatrix(r - 1, r - 1)
        ReDim DOFmatrix(r - 1)
        ReDim FERmatrix(r - 1)

        For p = 0 To r - 1
            DOFmatrix(p) = tdofm(p)
            FERmatrix(p) = tferm(p)
            For t = 0 To r - 1
                GSmatrix(p, t) = tgm(p, t)
            Next
        Next
        CBound = r
    End Sub

    Public Sub Welding(ByRef re() As Double, ByVal SettlementMatrix() As Double, ByRef DOFmatrix() As Double)
        Dim tres((WA2dt.Bob.Count * 3) - 1) As Double
        Dim j As Integer
        For i = 0 To ((WA2dt.Bob.Count * 3) - 1)
            If DOFmatrix(i) = 0 Then
                If SettlementMatrix(i) <> 0 Then
                    tres(i) = SettlementMatrix(i)
                End If
                Continue For
            End If
            tres(i) = tres(i) + re(j)
            j = j + 1
        Next

        ReDim re((WA2dt.Bob.Count * 3) - 1)
        'For i = 0 To ((WA2dt.Bob.Count * 3) - 1)
        '    re(i) = tres(i)
        'Next

        '------ Converting Result Matrix to the Support Inclination
        For i = 0 To (WA2dt.Bob.Count - 1)
            re((i * 3) + 0) = tres((i * 3) + 0)
            re((i * 3) + 1) = tres((i * 3) + 1)
            re((i * 3) + 2) = tres((i * 3) + 2)
        Next

    End Sub

    Public Sub GMultiplier(ByRef Nre() As Double, ByRef re() As Double, ByRef GSmatrix(,) As Double, ByRef FERmatrix() As Double)
        Dim teR((WA2dt.Bob.Count * 3) - 1) As Double

        For i = 0 To ((WA2dt.Bob.Count * 3) - 1)
            teR(i) = 0
            For j = 0 To ((WA2dt.Bob.Count * 3) - 1)
                teR(i) = teR(i) + (GSmatrix(i, j) * re(j))
            Next
        Next

        For i = 0 To ((WA2dt.Bob.Count * 3) - 1)
            Nre(i) = teR(i) - FERmatrix(i)
        Next
    End Sub

    Public Sub SpringStiffnessRedistribution(ByRef GStiffnessMatrix(,) As Double)
        For i = 0 To (WA2dt.Bob.Count - 1)
            If WA2dt.Bob(i).Support.SS = True Then
                GStiffnessMatrix((WA2dt.Bob(i).index * 3) + 0, (WA2dt.Bob(i).index * 3) + 0) = GStiffnessMatrix((WA2dt.Bob(i).index * 3) + 0, (WA2dt.Bob(i).index * 3) + 0) + 0
                GStiffnessMatrix((WA2dt.Bob(i).index * 3) + 1, (WA2dt.Bob(i).index * 3) + 1) = GStiffnessMatrix((WA2dt.Bob(i).index * 3) + 1, (WA2dt.Bob(i).index * 3) + 1) + (WA2dt.Bob(i).Support.stiffnessK)
                GStiffnessMatrix((WA2dt.Bob(i).index * 3) + 2, (WA2dt.Bob(i).index * 3) + 2) = GStiffnessMatrix((WA2dt.Bob(i).index * 3) + 2, (WA2dt.Bob(i).index * 3) + 2) + 0
            End If
        Next
    End Sub

    Public Sub SlidingRedistribution(ByVal GStiffnessMatrix(,) As Double, ByVal SettlementMatrix() As Double, ByRef ForceMatrix() As Double)
        Dim MatrixCount As Integer = -1 '---Note the number of force redistribution matrix required
        Dim SettlementValue As New List(Of Double)
        Dim Ivalue As New List(Of Integer)
        For i = 0 To ((WA2dt.Bob.Count * 3) - 1)
            If SettlementMatrix(i) <> 0 Then
                Ivalue.Add(i)
                SettlementValue.Add(SettlementMatrix(i))
                MatrixCount = MatrixCount + 1
            End If
        Next
        If MatrixCount = -1 Then
            Exit Sub
        End If
        Dim RedistributionMatrix(MatrixCount, (WA2dt.Bob.Count * 3) - 1)
        For i = 0 To MatrixCount
            For p = 0 To ((WA2dt.Bob.Count * 3) - 1)
                If Ivalue(i) = p Then
                    RedistributionMatrix(i, p) = SettlementValue(i)
                    Continue For
                End If
                RedistributionMatrix(i, p) = SettlementValue(i) * GStiffnessMatrix(Ivalue(i), p)
            Next
        Next
        '-------Force Redistribution Matrix
        Dim CummulativeForceReDistribution((WA2dt.Bob.Count * 3) - 1) As Double
        For i = 0 To MatrixCount
            For p = 0 To ((WA2dt.Bob.Count * 3) - 1)
                CummulativeForceReDistribution(p) = CummulativeForceReDistribution(p) + RedistributionMatrix(i, p)
            Next
        Next
        For p = 0 To ((WA2dt.Bob.Count * 3) - 1)
            ForceMatrix(p) = ForceMatrix(p) - CummulativeForceReDistribution(p)
        Next
    End Sub
#End Region

#Region "Coordinate Fixing"
    Public Sub BeamCoordinates()
        For Each R In _ResultMem
            R.FixResultValues()
        Next
        FixLoadMaximum()
        For Each R In _ResultMem
            R.SetCoordinates()
        Next
    End Sub

    Private Sub FixLoadMaximum()
        LoadScale(0) = 0
        LoadScale(1) = 0
        LoadScale(2) = 0
        LoadScale(3) = 0

        For Each R In _ResultMem
            For Each V In R.ResulCoords.shearValues
                If Math.Abs(V) > LoadScale(0) Then
                    LoadScale(0) = Math.Abs(V)
                End If
            Next
            For Each V In R.ResulCoords.bendingValues
                If Math.Abs(V) > LoadScale(1) Then
                    LoadScale(1) = Math.Abs(V)
                End If
            Next
            For Each V In R.ResulCoords.deflectionValuesY
                If Math.Abs(V) > LoadScale(2) Then
                    LoadScale(2) = Math.Abs(V)
                End If
            Next
            For Each V In R.ResulCoords.deflectionValuesX
                If Math.Abs(V) > LoadScale(2) Then
                    LoadScale(2) = Math.Abs(V)
                End If
            Next
            For Each V In R.ResulCoords.axialForcevalues
                If Math.Abs(V) > LoadScale(3) Then
                    LoadScale(3) = Math.Abs(V)
                End If
            Next
        Next

    End Sub
#End Region
End Class
