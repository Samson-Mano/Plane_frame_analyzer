Imports System.Drawing.Drawing2D

Public Class ResultPaint
    Private Zmfactor As Double
    Dim hBrush3 As HatchBrush

    Public Sub New(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal PaintNull As Boolean, ByVal Mpt As Point)
        If PaintNull = False Then
            Zmfactor = RA2dt.Zm
            e.Graphics.TranslateTransform(Mpt.X, Mpt.Y)
            If MDIMain._bendingmoment.Checked = True Then
                PaintBendingMoment(e)
            ElseIf MDIMain._shearforce.Checked = True Then
                PaintShear(e)
            ElseIf MDIMain._deflection.Checked = True Then
                PaintDeflection(e)
            ElseIf MDIMain._reaction.Checked = True Then
                PaintAxialForce(e)
            End If
        End If
    End Sub

    Private Sub PaintShear(ByRef e As System.Windows.Forms.PaintEventArgs)
        hBrush3 = New HatchBrush(HatchStyle.Percent30, Color.Green, Color.Transparent)
        Dim SFPen As New Pen(Color.Green, 1 / Zmfactor)
        For Each Coords In RA2dt.Analyzer.ResultMem
            If Coords.ResulCoords._SFPoints.Count = 0 Then Continue For
            Dim ClosedHashPt As New List(Of Point)
            ClosedHashPt.AddRange(Coords.ResulCoords._SFPoints)
            ClosedHashPt.AddRange(Coords.ResulCoords._MemPoints)

            Dim curve_path As New GraphicsPath()
            curve_path.AddClosedCurve(ClosedHashPt.ToArray, 0)
            e.Graphics.DrawLines(SFPen, Coords.ResulCoords._SFPoints.ToArray)
            e.Graphics.FillPath(hBrush3, curve_path)

            '------- Paint Maximum Shear Force Caption
            If MDIMain.ResultviewoptionToolStripMenuItem.Checked = True Then
                For Each CDIndex In Coords.ResulCoords._SFMcPointsindex
                    e.Graphics.DrawString(Coords.ResulCoords.shearValues(CDIndex), New Font("Verdana", (8 / Zmfactor)), New Pen(Color.Green, 2 / Zmfactor).Brush, _
                                                                               Coords.ResulCoords._SFPoints(CDIndex).X + (4 / Zmfactor), _
                                                                               Coords.ResulCoords._SFPoints(CDIndex).Y + (4 / Zmfactor))
                Next
            End If

        Next
    End Sub

    Private Sub PaintBendingMoment(ByRef e As System.Windows.Forms.PaintEventArgs)
        hBrush3 = New HatchBrush(HatchStyle.Percent30, Color.Maroon, Color.Transparent)
        Dim BMPen As New Pen(Color.Maroon, 1 / Zmfactor)
        For Each Coords In RA2dt.Analyzer.ResultMem
            If Coords.ResulCoords._BMPoints.Count = 0 Then Continue For
            Dim ClosedHashPt As New List(Of Point)
            ClosedHashPt.AddRange(Coords.ResulCoords._BMPoints)
            ClosedHashPt.AddRange(Coords.ResulCoords._MemPoints)

            Dim curve_path As New GraphicsPath()
            curve_path.AddClosedCurve(ClosedHashPt.ToArray, 0)
            e.Graphics.DrawLines(BMPen, Coords.ResulCoords._BMPoints.ToArray)
            e.Graphics.FillPath(hBrush3, curve_path)

            '------- Paint Maximum Bending Moment Caption
            If MDIMain.ResultviewoptionToolStripMenuItem.Checked = True Then
                For Each CDIndex In Coords.ResulCoords._BMMcPointsindex
                    e.Graphics.DrawString(Coords.ResulCoords.bendingValues(CDIndex), New Font("Verdana", (8 / Zmfactor)), New Pen(Color.Maroon, 2 / Zmfactor).Brush, _
                                                                               Coords.ResulCoords._BMPoints(CDIndex).X + (4 / Zmfactor), _
                                                                               Coords.ResulCoords._BMPoints(CDIndex).Y + (4 / Zmfactor))
                Next
            End If
        Next
    End Sub

    Private Sub PaintDeflection(ByRef e As System.Windows.Forms.PaintEventArgs)
        Dim DFPen As New Pen(Color.Red, 1 / Zmfactor)
        For Each Coords In RA2dt.Analyzer.ResultMem
            If Coords.ResulCoords._DEPoints.Count = 0 Then Continue For
            e.Graphics.DrawLines(DFPen, Coords.ResulCoords._DEPoints.ToArray)
            '------- Paint Maximum Deflection Caption
            If MDIMain.ResultviewoptionToolStripMenuItem.Checked = True Then
                For Each CDIndex In Coords.ResulCoords._DEMcPointsindex
                    e.Graphics.DrawString(Coords.ResulCoords.deflectionValuesY(CDIndex), New Font("Verdana", (8 / Zmfactor)), New Pen(Color.Red, 2 / Zmfactor).Brush, _
                                                                               Coords.ResulCoords._DEPoints(CDIndex).X + (4 / Zmfactor), _
                                                                               Coords.ResulCoords._DEPoints(CDIndex).Y + (4 / Zmfactor))
                Next
            End If
        Next
    End Sub

    Private Sub PaintAxialForce(ByRef e As System.Windows.Forms.PaintEventArgs)
        hBrush3 = New HatchBrush(HatchStyle.Percent30, Color.Blue, Color.Transparent)
        Dim AFPen As New Pen(Color.Blue, 1 / Zmfactor)
        For Each Coords In RA2dt.Analyzer.ResultMem
            If Coords.ResulCoords._AXPoints.Count = 0 Then Continue For
            Dim ClosedHashPt As New List(Of Point)
            ClosedHashPt.AddRange(Coords.ResulCoords._AXPoints)
            ClosedHashPt.AddRange(Coords.ResulCoords._MemPoints)

            Dim curve_path As New GraphicsPath()
            curve_path.AddClosedCurve(ClosedHashPt.ToArray, 0)
            e.Graphics.DrawLines(AFPen, Coords.ResulCoords._AXPoints.ToArray)
            e.Graphics.FillPath(hBrush3, curve_path)
            '------- Paint Maximum Axial Force Caption
            If MDIMain.ResultviewoptionToolStripMenuItem.Checked = True Then
                For Each CDIndex In Coords.ResulCoords._AXMcPointsindex
                    e.Graphics.DrawString(Coords.ResulCoords.axialForcevalues(CDIndex), New Font("Verdana", (8 / Zmfactor)), New Pen(Color.Blue, 2 / Zmfactor).Brush, _
                                                                               Coords.ResulCoords._AXPoints(CDIndex).X + (4 / Zmfactor), _
                                                                               Coords.ResulCoords._AXPoints(CDIndex).Y + (4 / Zmfactor))
                Next
            End If
        Next
     

        '-----Reaction paint
        For Each NdResult In RA2dt.Analyzer.ResultNode
            '---- Vertical Force
            If WA2dt.Bob(NdResult._NodeNo).Support.FS = True Or _
                WA2dt.Bob(NdResult._NodeNo).Support.PS = True Or _
                WA2dt.Bob(NdResult._NodeNo).Support.PRS = True Or _
                WA2dt.Bob(NdResult._NodeNo).Support.FRS = True Or _
                WA2dt.Bob(NdResult._NodeNo).Support.SS = True Then
                If Math.Round(NdResult.VForce, 2) <> 0 Then
                    Dim loadpen As New System.Drawing.Pen(Color.Green, 2 / Zmfactor)
                    loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                    e.Graphics.DrawLine(loadpen, New Point(WA2dt.Bob(NdResult._NodeNo).Coord.X + (100 * Math.Cos(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination)), _
                                                           WA2dt.Bob(NdResult._NodeNo).Coord.Y + (100 * -Math.Sin(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination))), _
                                                 New Point(WA2dt.Bob(NdResult._NodeNo).Coord.X + (50 * Math.Cos(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination)), _
                                                           WA2dt.Bob(NdResult._NodeNo).Coord.Y + (50 * -Math.Sin(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination))))
                    If MDIMain.ResultviewoptionToolStripMenuItem.Checked = True Then
                        e.Graphics.DrawString(NdResult.VForce, New Font("Verdana", (8 / Zmfactor)), New Pen(Color.Green, 2 / Zmfactor).Brush, _
                                                                              WA2dt.Bob(NdResult._NodeNo).Coord.X + (110 * Math.Cos(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination)) + (2 / Zmfactor), _
                                                                              WA2dt.Bob(NdResult._NodeNo).Coord.Y + (110 * -Math.Sin(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination)) + (2 / Zmfactor))
                    End If
                End If
            End If
          
            '---- Horizontal Force
            If WA2dt.Bob(NdResult._NodeNo).Support.FS = True Or _
               WA2dt.Bob(NdResult._NodeNo).Support.PS = True  Then
                If Math.Round(NdResult.HForce, 2) <> 0 Then
                    Dim loadpen As New System.Drawing.Pen(Color.Green, 2 / Zmfactor)
                    loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                    e.Graphics.DrawLine(loadpen, _
                                                 New Point(WA2dt.Bob(NdResult._NodeNo).Coord.X + ((50 * Math.Cos(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination)) + (55 * Math.Sin(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination))), _
                                                           WA2dt.Bob(NdResult._NodeNo).Coord.Y + ((50 * -Math.Sin(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination)) + (55 * Math.Cos(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination)))), _
                                                 New Point(WA2dt.Bob(NdResult._NodeNo).Coord.X + (50 * Math.Cos(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination)), _
                                                           WA2dt.Bob(NdResult._NodeNo).Coord.Y + (50 * -Math.Sin(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination))))
                    If MDIMain.ResultviewoptionToolStripMenuItem.Checked = True Then
                        e.Graphics.DrawString(NdResult.HForce, New Font("Verdana", (8 / Zmfactor)), New Pen(Color.Green, 2 / Zmfactor).Brush, _
                                                                              WA2dt.Bob(NdResult._NodeNo).Coord.X + ((50 * Math.Cos(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination)) + (65 * Math.Sin(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination))) + (2 / Zmfactor), _
                                                                              WA2dt.Bob(NdResult._NodeNo).Coord.Y + ((50 * -Math.Sin(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination)) + (65 * Math.Cos(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination))) + (2 / Zmfactor))
                    End If
                End If
            End If

            '---- Moment
            If WA2dt.Bob(NdResult._NodeNo).Support.FS = True Or WA2dt.Bob(NdResult._NodeNo).Support.FRS = True Then
                If Math.Round((NdResult.Moment), 2) <> 0 Then
                    Dim loadpen As New System.Drawing.Pen(Color.Green, 2 / Zmfactor)
                    loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                    loadpen.CustomEndCap = New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0)
                    e.Graphics.DrawArc(loadpen, Convert.ToInt32(WA2dt.Bob(NdResult._NodeNo).Coord.X + (50 * Math.Cos(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination))) - 20, _
                                                Convert.ToInt32(WA2dt.Bob(NdResult._NodeNo).Coord.Y + (50 * -Math.Sin(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination))) - 20, 40, 40, 360, 270)
                    If MDIMain.ResultviewoptionToolStripMenuItem.Checked = True Then
                        e.Graphics.DrawString((-NdResult.Moment), New Font("Verdana", (8 / Zmfactor)), New Pen(Color.Green, 2 / Zmfactor).Brush, Convert.ToInt64(WA2dt.Bob(NdResult._NodeNo).Coord.X + (50 * Math.Cos(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination))) - 20, _
                                                                                                                                         Convert.ToInt64(WA2dt.Bob(NdResult._NodeNo).Coord.Y + (50 * -Math.Sin(-WA2dt.Bob(NdResult._NodeNo).Support.supportinclination))) + 20)
                    End If
                End If
            End If
        Next
    End Sub
End Class
