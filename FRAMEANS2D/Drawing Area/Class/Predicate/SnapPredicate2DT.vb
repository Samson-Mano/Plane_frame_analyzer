Public Class SnapPredicate2DT
    Private _Her As Line2DT.Node
    Private Mx As Double
    Public ExistingNodePD As System.Predicate(Of Line2DT.Node)
    Public NodeSnapPD As System.Predicate(Of Line2DT.Node)
    Public HorizSnapPD As System.Predicate(Of Line2DT.Node)
    Public VertSnapPD As System.Predicate(Of Line2DT.Node)
    Public MidptSnapPD As System.Predicate(Of Line2DT)
    Public LineSnapPD As System.Predicate(Of Line2DT)
    Public SelectPD As System.Predicate(Of Line2DT)

    Public Property Her() As Line2DT.Node
        Get
            Return _Her
        End Get
        Set(ByVal value As Line2DT.Node)
            _Her = value
        End Set
    End Property

    Public Sub New() '---General Predicate
        ExistingNodePD = New System.Predicate(Of Line2DT.Node)(AddressOf ExistingNodes)
        NodeSnapPD = New System.Predicate(Of Line2DT.Node)(AddressOf NodeSnap)
        HorizSnapPD = New System.Predicate(Of Line2DT.Node)(AddressOf HorizSnap)
        VertSnapPD = New System.Predicate(Of Line2DT.Node)(AddressOf VertSnap)
        MidptSnapPD = New System.Predicate(Of Line2DT)(AddressOf MidPtSnap)
        LineSnapPD = New System.Predicate(Of Line2DT)(AddressOf LineSnap)
    End Sub

    Public Sub New(ByVal B As Boolean)
        LineSnapPD = New System.Predicate(Of Line2DT)(AddressOf SplitSnap)
    End Sub

    Public Sub New(ByVal B As Double, ByVal c As Boolean)
        SelectPD = New System.Predicate(Of Line2DT)(AddressOf SelectSnap)
    End Sub

#Region "Existing Node Finder"
    Private Function ExistingNodes(ByVal His As Line2DT.Node) As Boolean
        ExistingNodes = If(((Her.Coord.X < His.Coord.X + (1 / WA2dt.Zm) _
                             And Her.Coord.X > His.Coord.X - (1 / WA2dt.Zm)) _
                             And (Her.Coord.Y < His.Coord.Y + (1 / WA2dt.Zm) _
                             And Her.Coord.Y > His.Coord.Y - (1 / WA2dt.Zm))), True, False)
    End Function
#End Region

#Region "Node Snap"
    Private Function NodeSnap(ByVal His As Line2DT.Node) As Boolean
        NodeSnap = If((Her.Coord.X < (His.Coord.X + (OptionD.NSint / WA2dt.Zm)) _
                       And Her.Coord.X > (His.Coord.X - (OptionD.NSint / WA2dt.Zm))) _
                       And (Her.Coord.Y < (His.Coord.Y + (OptionD.NSint / WA2dt.Zm)) _
                       And Her.Coord.Y > (His.Coord.Y - (OptionD.NSint / WA2dt.Zm))), True, False)
    End Function
#End Region

#Region "HV Snap"
    Private Function HorizSnap(ByVal His As Line2DT.Node) As Boolean
        HorizSnap = If((Her.Coord.Y < (His.Coord.Y + (OptionD.NSint / WA2dt.Zm)) _
                        And Her.Coord.Y > (His.Coord.Y - (OptionD.NSint / WA2dt.Zm))), True, False)
    End Function

    Private Function VertSnap(ByVal His As Line2DT.Node) As Boolean
        VertSnap = If((Her.Coord.X < (His.Coord.X + (OptionD.NSint / WA2dt.Zm)) _
                       And Her.Coord.X > (His.Coord.X - (OptionD.NSint / WA2dt.Zm))), True, False)
    End Function
#End Region

#Region "MidPt Snap"
    Private Function MidPtSnap(ByVal His As Line2DT) As Boolean
        MidPtSnap = If( _
        (Her.Coord.X < (((His.SN.Coord.X + His.EN.Coord.X) / 2) + (OptionD.MSint / WA2dt.Zm)) _
         And Her.Coord.X > (((His.SN.Coord.X + His.EN.Coord.X) / 2) - (OptionD.MSint / WA2dt.Zm)) _
         And (Her.Coord.Y < (((His.SN.Coord.Y + His.EN.Coord.Y) / 2) + (OptionD.MSint / WA2dt.Zm)) _
         And Her.Coord.Y > (((His.SN.Coord.Y + His.EN.Coord.Y) / 2) - (OptionD.MSint / WA2dt.Zm)))), True, False)
    End Function
#End Region

#Region "Line Snap"
    Private Function LineSnap(ByVal His As Line2DT) As Boolean
        Dim tx, ty, elx1, ely1, elx2, ely2 As Double
        tx = ((Her.Coord.X * (His.Lcos)) + (Her.Coord.Y * (-1 * His.Msin)))
        ty = ((Her.Coord.Y * (His.Lcos)) + (Her.Coord.X * (His.Msin)))
        elx1 = ((His.SN.Coord.X * (His.Lcos)) + (His.SN.Coord.Y * (-1 * His.Msin)))
        ely1 = ((His.SN.Coord.Y * (His.Lcos)) + (His.SN.Coord.X * (His.Msin)))
        elx2 = ((His.EN.Coord.X * (His.Lcos)) + (His.EN.Coord.Y * (-1 * His.Msin)))
        ely2 = ((His.EN.Coord.Y * (His.Lcos)) + (His.EN.Coord.X * (His.Msin)))
        If (tx > elx1 And tx < elx2) Then
            LineSnap = If(((ty < ely1 + (2 / WA2dt.Zm) _
                            And ty > ely1 - (2 / WA2dt.Zm)) _
                            And (ty < ely2 + (2 / WA2dt.Zm) _
                            And ty > ely2 - (2 / WA2dt.Zm))), True, False)
            'LineSnap = If((ty < ely1 + (OptionD.MSint) And ty > ely1 - (OptionD.MSint)), True, False)
        End If
    End Function
#End Region

#Region "Select Predicate"
    Private Function SelectSnap(ByVal His As Line2DT) As Boolean
        Dim tx, ty, elx1, ely1, elx2, ely2 As Double
        tx = ((Her.Coord.X * (His.Lcos)) + (Her.Coord.Y * (-1 * His.Msin)))
        ty = ((Her.Coord.Y * (His.Lcos)) + (Her.Coord.X * (His.Msin)))
        elx1 = ((His.SN.Coord.X * (His.Lcos)) + (His.SN.Coord.Y * (-1 * His.Msin)))
        ely1 = ((His.SN.Coord.Y * (His.Lcos)) + (His.SN.Coord.X * (His.Msin)))
        elx2 = ((His.EN.Coord.X * (His.Lcos)) + (His.EN.Coord.Y * (-1 * His.Msin)))
        ely2 = ((His.EN.Coord.Y * (His.Lcos)) + (His.EN.Coord.X * (His.Msin)))
        If (tx > elx1 And tx < elx2) Then
            SelectSnap = If(((ty < ely1 + (12 / WA2dt.Zm) _
                              And ty > ely1 - (12 / WA2dt.Zm)) _
                              And (ty < ely2 + (12 / WA2dt.Zm) _
                              And ty > ely2 - (12 / WA2dt.Zm))), True, False)
            'LineSnap = If((ty < ely1 + (OptionD.MSint) And ty > ely1 - (OptionD.MSint)), True, False)
        End If
    End Function
#End Region

#Region "Split Predicate"
    Private Function SplitSnap(ByVal His As Line2DT) As Boolean
        Dim tx, ty, elx1, ely1, elx2, ely2 As Double
        tx = ((Her.Coord.X * (His.Lcos)) + (Her.Coord.Y * (-1 * His.Msin)))
        ty = ((Her.Coord.Y * (His.Lcos)) + (Her.Coord.X * (His.Msin)))
        elx1 = ((His.SN.Coord.X * (His.Lcos)) + (His.SN.Coord.Y * (-1 * His.Msin)))
        ely1 = ((His.SN.Coord.Y * (His.Lcos)) + (His.SN.Coord.X * (His.Msin)))
        elx2 = ((His.EN.Coord.X * (His.Lcos)) + (His.EN.Coord.Y * (-1 * His.Msin)))
        ely2 = ((His.EN.Coord.Y * (His.Lcos)) + (His.EN.Coord.X * (His.Msin)))
        If (tx > elx1 And tx < elx2) Then
            SplitSnap = If(((ty < ely1 + (2 / WA2dt.Zm) _
                             And ty > ely1 - (2 / WA2dt.Zm)) _
                             And (ty < ely2 + (2 / WA2dt.Zm) _
                             And ty > ely2 - (2 / WA2dt.Zm))), True, False)
            'LineSnap = If((ty < ely1 + (OptionD.MSint) And ty > ely1 - (OptionD.MSint)), True, False)
        End If
    End Function
#End Region
End Class
