Imports System.Drawing.Drawing2D

Public Class PermPaint2DT
    Private Zmfactor As Double

    Public Sub New(ByRef e As System.Windows.Forms.PaintEventArgs)
        If WA2dt.PaintNULL = False Then
            Zmfactor = WA2dt.Zm
            PaintAxis(e)
            PaintMember(e)
            PaintLoad(e)
            PaintNode(e)
            PaintSupport(e)
            If WA2dt.selLine.Count <> 0 Then
                PaintSelecedMember(e)
            End If
        End If
    End Sub

    Private Sub PaintSelecedMember(ByRef e As System.Windows.Forms.PaintEventArgs)
        For Each Ind In WA2dt.selLine
            Dim P As New Pen(OptionD.Bc, 2 / Zmfactor)
            P.DashStyle = Drawing2D.DashStyle.Dot
            e.Graphics.DrawLine(P, WA2dt.Mem(Ind).SN.Coord.X, WA2dt.Mem(Ind).SN.Coord.Y, WA2dt.Mem(Ind).EN.Coord.X, WA2dt.Mem(Ind).EN.Coord.Y)
        Next
    End Sub

    Private Sub PaintAxis(ByRef e As System.Windows.Forms.PaintEventArgs)
        'Dim Y As Integer = (WA2dt.MainPic.Width / Zmfactor)
        'Dim X As Integer = (WA2dt.MainPic.Height / Zmfactor)
        'e.Graphics.DrawLine(New Pen(Color.FromArgb(100, Color.Red), 1 / Zmfactor), 0, -X, 0, X)
        'e.Graphics.DrawLine(New Pen(Color.FromArgb(100, Color.Blue), 1 / Zmfactor), -Y, 0, Y, 0)
    End Sub

    Private Sub PaintMember(ByRef e As System.Windows.Forms.PaintEventArgs)
        For Each El In WA2dt.Mem
            e.Graphics.DrawLine(New Pen(OptionD.Mc, 2 / Zmfactor), El.SN.Coord.X, El.SN.Coord.Y, El.EN.Coord.X, El.EN.Coord.Y)
            If MDIMain.LengthviewoptionToolStripMenuItem.Checked = True Then
                e.Graphics.DrawString(Math.Round(El.Length, OptionD.Prec), New Font("Verdana", (8 / Zmfactor)), New Pen(OptionD.Lc, 2 / Zmfactor).Brush, ((El.SN.Coord.X + El.EN.Coord.X) / 2), ((El.SN.Coord.Y + El.EN.Coord.Y) / 2))
            End If
        Next
    End Sub

    Private Sub PaintLoad(ByRef e As System.Windows.Forms.PaintEventArgs)
        For Each El In WA2dt.Mem
            For Each itm In El.Pload
                Dim loadpen As New System.Drawing.Pen(Color.Green, 2 / Zmfactor)
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                e.Graphics.DrawLine(loadpen, itm.PCoordS.X, itm.PCoordS.Y, itm.PCoordE.X, itm.PCoordE.Y)
                If MDIMain.LoadviewoptionToolStripMenuItem.Checked = True Then
                    e.Graphics.DrawString(Math.Abs(itm.pload), New Font("Verdana", (8 / Zmfactor)), New Pen(Color.Green, 2 / Zmfactor).Brush, itm.PCoordE.X + (2 / Zmfactor), itm.PCoordE.Y + (2 / Zmfactor))
                End If
            Next
            For Each itm In El.Uload
                Dim loadpen As New System.Drawing.Pen(Color.DeepPink, 2 / Zmfactor)
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                e.Graphics.DrawLine(loadpen, itm.uCoordS1.X, itm.uCoordS1.Y, itm.uCoordE1.X, itm.uCoordE1.Y)
                e.Graphics.DrawLine(loadpen, itm.uCoordS2.X, itm.uCoordS2.Y, itm.uCoordE2.X, itm.uCoordE2.Y)
                e.Graphics.DrawLine(New Pen(Color.DeepPink, 2 / Zmfactor), itm.uCoordE1.X, itm.uCoordE1.Y, itm.uCoordE2.X, itm.uCoordE2.Y)
                If MDIMain.LoadviewoptionToolStripMenuItem.Checked = True Then
                    e.Graphics.DrawString(Math.Abs(itm.uload1), New Font("Verdana", (8 / Zmfactor)), New Pen(Color.DeepPink, 2 / Zmfactor).Brush, itm.uCoordE1.X + (2 / Zmfactor), itm.uCoordE1.Y + (2 / Zmfactor))
                    e.Graphics.DrawString(Math.Abs(itm.uload2), New Font("Verdana", (8 / Zmfactor)), New Pen(Color.DeepPink, 2 / Zmfactor).Brush, itm.uCoordE2.X + (2 / Zmfactor), itm.uCoordE2.Y + (2 / Zmfactor))
                End If

                Dim hBrush3 As HatchBrush = New HatchBrush(HatchStyle.DiagonalBrick, Color.DeepPink, Color.Transparent)
                Dim uStream() As Point = { _
                                            New Point(itm.uCoordS1.X, itm.uCoordS1.Y), _
                                            New Point(itm.uCoordE1.X, itm.uCoordE1.Y), _
                                            New Point(itm.uCoordE2.X, itm.uCoordE2.Y), _
                                            New Point(itm.uCoordS2.X, itm.uCoordS2.Y), _
                                            New Point(itm.uCoordS1.X, itm.uCoordS1.Y)}
                Dim Stream_path As New GraphicsPath
                Stream_path.AddClosedCurve(uStream, 0)
                e.Graphics.FillPath(hBrush3, Stream_path)
            Next
            For Each itm In El.Mload
                Dim loadpen As New System.Drawing.Pen(Color.Orange, 2 / Zmfactor)
                loadpen.CustomStartCap = If(itm.mAnticlockwise = True, New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5), New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0))
                loadpen.CustomEndCap = If(itm.mClockwise = True, New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5), New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0))
                e.Graphics.DrawArc(loadpen, itm.mCoord.X - 30, itm.mCoord.Y - 30, 60, 60, 360, 270)
                If MDIMain.LoadviewoptionToolStripMenuItem.Checked = True Then
                    e.Graphics.DrawString(Math.Abs(itm.mload), New Font("Verdana", (8 / Zmfactor)), New Pen(Color.Orange, 2 / Zmfactor).Brush, itm.mCoord.X - 30, itm.mCoord.Y + 30)
                End If
            Next
        Next
    End Sub

    Private Sub PaintNode(ByRef e As System.Windows.Forms.PaintEventArgs)
        For Each Nd In WA2dt.Bob
            'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4 / WA2dt.Zm).Brush, Nd.Coord.X  - (2 / WA2dt.Zm), Nd.Coord.Y  - (2 / WA2dt.Zm), 4 / WA2dt.Zm, 4 / WA2dt.Zm)
            If MDIMain.NodeNoviewoptionToolStripMenuItem.Checked = True Then
                e.Graphics.DrawString(Nd.index, New Font("Verdana", (8 / WA2dt.Zm)), New Pen(OptionD.NNc, 2 / WA2dt.Zm).Brush, Nd.Coord.X - (14 / WA2dt.Zm), Nd.Coord.Y - (14 / WA2dt.Zm))
            End If
        Next
        'For Each El In WA2dt.Mem
        '    'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4 / WA2dt.Zm).Brush, Nd.Coord.X  - (2 / WA2dt.Zm), Nd.Coord.Y  - (2 / WA2dt.Zm), 4 / WA2dt.Zm, 4 / WA2dt.Zm)
        '    e.Graphics.DrawString(El.SN.index, New Font("Verdana", (8 / WA2dt.Zm)), New Pen(Color.DarkOrange, 2 / WA2dt.Zm).Brush, El.SN.Coord.X - (20 / WA2dt.Zm), El.SN.Coord.Y - (20 / WA2dt.Zm))
        '    e.Graphics.DrawString(El.EN.index, New Font("Verdana", (8 / WA2dt.Zm)), New Pen(Color.DarkRed, 2 / WA2dt.Zm).Brush, El.EN.Coord.X - (20 / WA2dt.Zm), El.EN.Coord.Y - (20 / WA2dt.Zm))
        'Next
    End Sub

    Private Sub PaintSupport(ByRef e As System.Windows.Forms.PaintEventArgs)
        '--- Start Node
        For Each EL In WA2dt.Mem
            If EL.SN.Support.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(EL.SN.Coord.X, EL.SN.Coord.Y))
            ElseIf EL.SN.Support.RJ = True Then
                SupportPicPaintRigidJoint(e, New Point(EL.SN.Coord.X, EL.SN.Coord.Y))
            ElseIf EL.SN.Support.PS = True Then
                SupportPicPaintPinSupport(e, New Point(EL.SN.Coord.X, EL.SN.Coord.Y), EL.SN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.SN.Support.PRS = True Then
                SupportPicPaintPinRollerSupport(e, New Point(EL.SN.Coord.X, EL.SN.Coord.Y), EL.SN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.SN.Support.FRS = True Then
                SupportPicPaintFixedRollerSupport(e, New Point(EL.SN.Coord.X, EL.SN.Coord.Y), EL.SN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.SN.Support.FS = True Then
                SupportPicPaintFixedSupport(e, New Point(EL.SN.Coord.X, EL.SN.Coord.Y), EL.SN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.SN.Support.SS = True Then
                SupportPicPaintSpringSupport(e, New Point(EL.SN.Coord.X, EL.SN.Coord.Y), EL.SN.Support.supportinclination, 0.5 / Zmfactor)
            End If

            '--- End Node
            If EL.EN.Support.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(EL.EN.Coord.X, EL.EN.Coord.Y))
            ElseIf EL.EN.Support.RJ = True Then
                SupportPicPaintRigidJoint(e, New Point(EL.EN.Coord.X, EL.EN.Coord.Y))
            ElseIf EL.EN.Support.PS = True Then
                SupportPicPaintPinSupport(e, New Point(EL.EN.Coord.X, EL.EN.Coord.Y), EL.EN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.EN.Support.PRS = True Then
                SupportPicPaintPinRollerSupport(e, New Point(EL.EN.Coord.X, EL.EN.Coord.Y), EL.EN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.EN.Support.FRS = True Then
                SupportPicPaintFixedRollerSupport(e, New Point(EL.EN.Coord.X, EL.EN.Coord.Y), EL.EN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.EN.Support.FS = True Then
                SupportPicPaintFixedSupport(e, New Point(EL.EN.Coord.X, EL.EN.Coord.Y), EL.EN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.EN.Support.SS = True Then
                SupportPicPaintSpringSupport(e, New Point(EL.EN.Coord.X, EL.EN.Coord.Y), EL.EN.Support.supportinclination, 0.5 / Zmfactor)
            End If
        Next

    End Sub

#Region "Support Pic Paint Events"
    Private Sub SupportPicPaintPinJoint(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point)
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2 / Zmfactor), CInt(M.X - (2 / Zmfactor)), CInt(M.Y - (2 / Zmfactor)), CInt(4 / Zmfactor), CInt(4 / Zmfactor))
        'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4 / Zmfactor).Brush, M.X - (3 / Zmfactor), M.Y - (3 / Zmfactor), (6 / Zmfactor), (6 / Zmfactor))
    End Sub

    Private Sub SupportPicPaintRigidJoint(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point)
        e.Graphics.DrawRectangle(New Pen(OptionD.Nc, 2 / Zmfactor), CInt(M.X - (2 / Zmfactor)), CInt(M.Y - (2 / Zmfactor)), CInt(4 / Zmfactor), CInt(4 / Zmfactor))
        'e.Graphics.FillRectangle(New Pen(OptionD.Nc, 4 / Zmfactor).Brush, M.X - (3 / Zmfactor), M.Y - (3 / Zmfactor), (6 / Zmfactor), (6 / Zmfactor))
    End Sub

    Private Sub SupportPicPaintPinSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)))

        '----Bottom Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + ((20 * -Math.Sin(-Inclination) + (-20 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZM))

    End Sub

    Private Sub SupportPicPaintPinRollerSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2 / Zmfactor), New Rectangle(New Point((M.X - (5 * ZM)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZM), (M.Y - (5 * ZM)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZM)), New Size((10 * ZM), (10 * ZM))))
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2 / Zmfactor), New Rectangle(New Point((M.X - (5 * ZM)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZM), (M.Y - (5 * ZM)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZM)), New Size((10 * ZM), (10 * ZM))))

    End Sub

    Private Sub SupportPicPaintFixedRollerSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '---- Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZM)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2 / Zmfactor), New Rectangle(New Point((M.X - (5 * ZM)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZM), (M.Y - (5 * ZM)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZM)), New Size((10 * ZM), (10 * ZM))))
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2 / Zmfactor), New Rectangle(New Point((M.X - (5 * ZM)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZM), (M.Y - (5 * ZM)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZM)), New Size((10 * ZM), (10 * ZM))))

    End Sub

    Private Sub SupportPicPaintFixedSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '---- Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)))

    End Sub

    Private Sub SupportPicPaintSpringSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '----SPRING
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((Math.Cos(-Inclination)) + (Math.Sin(-Inclination))) * ZM), M.Y + (((-Math.Sin(-Inclination)) + (Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((4 * Math.Cos(-Inclination)) + (Math.Sin(-Inclination))) * ZM), M.Y + (((4 * -Math.Sin(-Inclination)) + (Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((4 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((4 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((4 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((4 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((10 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((10 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((10 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((10 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM)))
        'e.Graphics.DrawLine(New Pen(Color.DarkSlateGray, 2), New Point(M.X + (((12 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * zm), M.Y + (((12 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * zm)), New Point(M.X + (((12 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * zm), M.Y + (((12 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * zm)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((16 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((16 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((16 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((16 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((16 * Math.Cos(-Inclination)) + (Math.Sin(-Inclination))) * ZM), M.Y + (((16 * -Math.Sin(-Inclination)) + (Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (Math.Sin(-Inclination))) * ZM), M.Y + (((-20 * Math.Sin(-Inclination)) + (Math.Cos(-Inclination))) * ZM)))

        '----Bottom Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / Zmfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZM)))

    End Sub
#End Region
End Class
