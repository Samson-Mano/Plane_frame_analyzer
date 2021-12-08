Imports System.Drawing.Drawing2D

Public Class RTempPaint
    Dim Pt As Point
    Dim Spoint As Point
    Dim Epoint As Point
    Dim Zmfactor As Double

    Public Sub New(ByRef Sp As Point, ByRef Ep As Point)
        Zmfactor = RA2dt.Zm
        Spoint = Sp
        Epoint = Ep
        Pt = New Point(RA2dt.Cpoint.X / Zmfactor, RA2dt.Cpoint.Y / Zmfactor)
        PanH()
    End Sub

    Public Sub New(ByRef e As System.Windows.Forms.PaintEventArgs)
        Zmfactor = RA2dt.Zm
        Pt = New Point(RA2dt.Cpoint.X / Zmfactor, RA2dt.Cpoint.Y / Zmfactor)
        If RA2dt.Linflg = True Then
            If MDIMain._pan.Checked = True Then
                PaintTempPan(e)
            End If
        End If
    End Sub

#Region "PanH"
    Private Sub PanH()
        RA2dt.Mpoint = New Point _
                            (RA2dt.Mpoint.X + (Epoint.X - Spoint.X), _
                             RA2dt.Mpoint.Y + (Epoint.Y - Spoint.Y))
    End Sub
#End Region

#Region "Pan Draw"
    Private Sub PaintTempPan(ByRef e As System.Windows.Forms.PaintEventArgs)
        Dim ext As New Point(-RA2dt.Spoint.X - RA2dt.Mpoint.X + Pt.X, -RA2dt.Spoint.Y - RA2dt.Mpoint.Y + Pt.Y)
        PaintPanAxis(e, ext)
        PaintPanMember(e, ext)
        PaintPanNode(e, ext)
        PaintPanLoad(e, ext)
        PaintPanSupport(e, ext)
    End Sub

    Private Sub PaintPanAxis(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        'e.Graphics.DrawLine(New Pen(Color.FromArgb(100, Color.Red), 1 / Zmfactor), RA2dt.Mpoint.X + Extent.X, 0, RA2dt.Mpoint.X + Extent.X, RA2dt.MainPic.Height / Zmfactor)
        'e.Graphics.DrawLine(New Pen(Color.FromArgb(100, Color.Blue), 1 / Zmfactor), 0, RA2dt.Mpoint.Y + Extent.Y, RA2dt.MainPic.Width / Zmfactor, RA2dt.Mpoint.Y + Extent.Y)
    End Sub

    Private Sub PaintPanMember(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        For Each El In WA2dt.Mem
            e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / Zmfactor), El.SN.Coord.X + RA2dt.Mpoint.X + Extent.X, El.SN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y, El.EN.Coord.X + RA2dt.Mpoint.X + Extent.X, El.EN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y)
            If MDIMain.LengthviewoptionToolStripMenuItem.Checked = True Then
                e.Graphics.DrawString(Math.Round(El.Length, OptionD.Prec), New Font("Verdana", (8 / Zmfactor)), New Pen(Color.LightGray, 2 / Zmfactor).Brush, ((El.SN.Coord.X + El.EN.Coord.X) / 2) + RA2dt.Mpoint.X + Extent.X, ((El.SN.Coord.Y + El.EN.Coord.Y) / 2) + RA2dt.Mpoint.Y + Extent.Y)
            End If
        Next
    End Sub

    Private Sub PaintPanNode(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        For Each Nd In WA2dt.Bob
            'e.Graphics.FillEllipse(New Pen(Color.lightgray, 4 / Ra2dt.Zm).Brush, Nd.Coord.X + Ra2dt.Mpoint.X - (2 / Ra2dt.Zm) + Extent.X, Nd.Coord.Y + Ra2dt.Mpoint.Y - (2 / Ra2dt.Zm) + Extent.Y, 4 / Ra2dt.Zm, 4 / Ra2dt.Zm)
            If MDIMain.NodeNoviewoptionToolStripMenuItem.Checked = True Then
                e.Graphics.DrawString(WA2dt.Bob.IndexOf(Nd), New Font("Verdana", (8 / RA2dt.Zm)), New Pen(Color.LightGray, 2 / RA2dt.Zm).Brush, Nd.Coord.X + RA2dt.Mpoint.X - (14 / RA2dt.Zm) + Extent.X, Nd.Coord.Y + RA2dt.Mpoint.Y - (14 / RA2dt.Zm) + Extent.Y)
            End If
        Next
    End Sub

    Private Sub PaintPanLoad(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        For Each El In WA2dt.Mem
            For Each itm In El.Pload
                Dim loadpen As New System.Drawing.Pen(Color.LightGray, 2 / RA2dt.Zm)
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                e.Graphics.DrawLine(loadpen, itm.PCoordS.X + RA2dt.Mpoint.X + Extent.X, itm.PCoordS.Y + RA2dt.Mpoint.Y + Extent.Y, itm.PCoordE.X + RA2dt.Mpoint.X + Extent.X, itm.PCoordE.Y + RA2dt.Mpoint.Y + Extent.Y)
                If MDIMain.LoadviewoptionToolStripMenuItem.Checked = True Then
                    e.Graphics.DrawString(Math.Abs(itm.pload), New Font("Verdana", (8 / RA2dt.Zm)), New Pen(Color.LightGray, 2 / RA2dt.Zm).Brush, itm.PCoordE.X + RA2dt.Mpoint.X + Extent.X + (2 / RA2dt.Zm), itm.PCoordE.Y + RA2dt.Mpoint.Y + Extent.Y + (2 / RA2dt.Zm))
                End If
            Next
            For Each itm In El.Uload
                Dim hBrush3 As HatchBrush = New HatchBrush(HatchStyle.DiagonalBrick, Color.LightGray, Color.Transparent)
                Dim uStream() As Point = { _
                                            New Point(itm.uCoordS1.X + RA2dt.Mpoint.X + Extent.X, itm.uCoordS1.Y + RA2dt.Mpoint.Y + Extent.Y), _
                                            New Point(itm.uCoordE1.X + RA2dt.Mpoint.X + Extent.X, itm.uCoordE1.Y + RA2dt.Mpoint.Y + Extent.Y), _
                                            New Point(itm.uCoordE2.X + RA2dt.Mpoint.X + Extent.X, itm.uCoordE2.Y + RA2dt.Mpoint.Y + Extent.Y), _
                                            New Point(itm.uCoordS2.X + RA2dt.Mpoint.X + Extent.X, itm.uCoordS2.Y + RA2dt.Mpoint.Y + Extent.Y), _
                                            New Point(itm.uCoordS1.X + RA2dt.Mpoint.X + Extent.X, itm.uCoordS1.Y + RA2dt.Mpoint.Y + Extent.Y)}
                Dim Stream_path As New GraphicsPath
                Stream_path.AddClosedCurve(uStream, 0)
                e.Graphics.FillPath(hBrush3, Stream_path)

                Dim loadpen As New System.Drawing.Pen(Color.LightGray, 2 / RA2dt.Zm)
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                e.Graphics.DrawLine(loadpen, itm.uCoordS1.X + RA2dt.Mpoint.X + Extent.X, itm.uCoordS1.Y + RA2dt.Mpoint.Y + Extent.Y, itm.uCoordE1.X + RA2dt.Mpoint.X + Extent.X, itm.uCoordE1.Y + RA2dt.Mpoint.Y + Extent.Y)
                e.Graphics.DrawLine(loadpen, itm.uCoordS2.X + RA2dt.Mpoint.X + Extent.X, itm.uCoordS2.Y + RA2dt.Mpoint.Y + Extent.Y, itm.uCoordE2.X + RA2dt.Mpoint.X + Extent.X, itm.uCoordE2.Y + RA2dt.Mpoint.Y + Extent.Y)
                e.Graphics.DrawLine(New Pen(Color.LightGray, (2 / RA2dt.Zm)), itm.uCoordE1.X + RA2dt.Mpoint.X + Extent.X, itm.uCoordE1.Y + RA2dt.Mpoint.Y + Extent.Y, itm.uCoordE2.X + RA2dt.Mpoint.X + Extent.X, itm.uCoordE2.Y + RA2dt.Mpoint.Y + Extent.Y)
                If MDIMain.LoadviewoptionToolStripMenuItem.Checked = True Then
                    e.Graphics.DrawString(Math.Abs(itm.uload1), New Font("Verdana", (8 / RA2dt.Zm)), New Pen(Color.LightGray, 2 / RA2dt.Zm).Brush, itm.uCoordE1.X + RA2dt.Mpoint.X + Extent.X + (2 / RA2dt.Zm), itm.uCoordE1.Y + RA2dt.Mpoint.Y + Extent.Y + (2 / RA2dt.Zm))
                    e.Graphics.DrawString(Math.Abs(itm.uload2), New Font("Verdana", (8 / RA2dt.Zm)), New Pen(Color.LightGray, 2 / RA2dt.Zm).Brush, itm.uCoordE2.X + RA2dt.Mpoint.X + Extent.X + (2 / RA2dt.Zm), itm.uCoordE2.Y + RA2dt.Mpoint.Y + Extent.Y + (2 / RA2dt.Zm))
                End If
            Next
            For Each itm In El.Mload
                Dim loadpen As New System.Drawing.Pen(Color.LightGray, 2 / RA2dt.Zm)
                loadpen.CustomStartCap = If(itm.mAnticlockwise = True, New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5), New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0))
                loadpen.CustomEndCap = If(itm.mClockwise = True, New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5), New System.Drawing.Drawing2D.AdjustableArrowCap(0, 0))
                e.Graphics.DrawArc(loadpen, itm.mCoord.X + RA2dt.Mpoint.X + Extent.X - 30, itm.mCoord.Y + RA2dt.Mpoint.Y + Extent.Y - 30, 60, 60, 360, 270)
                If MDIMain.LoadviewoptionToolStripMenuItem.Checked = True Then
                    e.Graphics.DrawString(Math.Abs(itm.mload), New Font("Verdana", (8 / RA2dt.Zm)), New Pen(Color.LightGray, 2 / RA2dt.Zm).Brush, itm.mCoord.X + RA2dt.Mpoint.X + Extent.X - 30, itm.mCoord.Y + RA2dt.Mpoint.Y + Extent.Y + 30)
                End If
            Next
        Next
    End Sub

    Private Sub PaintPanSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        '--- Start Node
        For Each EL In WA2dt.Mem
            If EL.SN.Support.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(EL.SN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.SN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y))
            ElseIf EL.SN.Support.RJ = True Then
                SupportPicPaintRigidJoint(e, New Point(EL.SN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.SN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y))
            ElseIf EL.SN.Support.PS = True Then
                SupportPicPaintPinSupport(e, New Point(EL.SN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.SN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.SN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.SN.Support.PRS = True Then
                SupportPicPaintPinRollerSupport(e, New Point(EL.SN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.SN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.SN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.SN.Support.FRS = True Then
                SupportPicPaintFixedRollerSupport(e, New Point(EL.SN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.SN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.SN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.SN.Support.FS = True Then
                SupportPicPaintFixedSupport(e, New Point(EL.SN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.SN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.SN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.SN.Support.SS = True Then
                SupportPicPaintSpringSupport(e, New Point(EL.SN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.SN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.SN.Support.supportinclination, 0.5 / Zmfactor)
            End If

            '--- End Node
            If EL.EN.Support.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(EL.EN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.EN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y))
            ElseIf EL.EN.Support.RJ = True Then
                SupportPicPaintRigidJoint(e, New Point(EL.EN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.EN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y))
            ElseIf EL.EN.Support.PS = True Then
                SupportPicPaintPinSupport(e, New Point(EL.EN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.EN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.EN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.EN.Support.PRS = True Then
                SupportPicPaintPinRollerSupport(e, New Point(EL.EN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.EN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.EN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.EN.Support.FRS = True Then
                SupportPicPaintFixedRollerSupport(e, New Point(EL.EN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.EN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.EN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.EN.Support.FS = True Then
                SupportPicPaintFixedSupport(e, New Point(EL.EN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.EN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.EN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.EN.Support.SS = True Then
                SupportPicPaintSpringSupport(e, New Point(EL.EN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.EN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.EN.Support.supportinclination, 0.5 / Zmfactor)
            End If
        Next
    End Sub
#End Region

#Region "Support Pic Paint Events"
    Private Sub SupportPicPaintPinJoint(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point)
        e.Graphics.DrawEllipse(New Pen(Color.LightGray, 2 / RA2dt.Zm), CInt(M.X - (2 / RA2dt.Zm)), CInt(M.Y - (2 / RA2dt.Zm)), CInt(4 / RA2dt.Zm), CInt(4 / RA2dt.Zm))
        'e.Graphics.FillEllipse(New Pen(Color.lightgray, 4 / Ra2dt.Zm).Brush, M.X - (3 / Ra2dt.Zm), M.Y - (3 / Ra2dt.Zm), (6 / Ra2dt.Zm), (6 / Ra2dt.Zm))
    End Sub

    Private Sub SupportPicPaintRigidJoint(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point)
        e.Graphics.DrawRectangle(New Pen(Color.LightGray, 2 / RA2dt.Zm), CInt(M.X - (2 / RA2dt.Zm)), CInt(M.Y - (2 / RA2dt.Zm)), CInt(4 / RA2dt.Zm), CInt(4 / RA2dt.Zm))
        'e.Graphics.FillRectangle(New Pen(Color.lightgray, 4 / Ra2dt.Zm).Brush, M.X - (3 / Ra2dt.Zm), M.Y - (3 / Ra2dt.Zm), (6 / Ra2dt.Zm), (6 / Ra2dt.Zm))
    End Sub

    Private Sub SupportPicPaintPinSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)))

        '----Bottom Rectangle
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + ((20 * -Math.Sin(-Inclination) + (-20 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZM))

    End Sub

    Private Sub SupportPicPaintPinRollerSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Rectangle(New Point((M.X - (5 * ZM)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZM), (M.Y - (5 * ZM)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZM)), New Size((10 * ZM), (10 * ZM))))
        e.Graphics.DrawEllipse(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Rectangle(New Point((M.X - (5 * ZM)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZM), (M.Y - (5 * ZM)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZM)), New Size((10 * ZM), (10 * ZM))))

    End Sub

    Private Sub SupportPicPaintFixedRollerSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '---- Rectangle
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (15 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (15 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-15 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-15 * Math.Cos(-Inclination))) * ZM)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Rectangle(New Point((M.X - (5 * ZM)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZM), (M.Y - (5 * ZM)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZM)), New Size((10 * ZM), (10 * ZM))))
        e.Graphics.DrawEllipse(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Rectangle(New Point((M.X - (5 * ZM)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZM), (M.Y - (5 * ZM)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZM)), New Size((10 * ZM), (10 * ZM))))

    End Sub

    Private Sub SupportPicPaintFixedSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '---- Rectangle
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((0 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZM)))

    End Sub

    Private Sub SupportPicPaintSpringSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '----SPRING
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((Math.Cos(-Inclination)) + (Math.Sin(-Inclination))) * ZM), M.Y + (((-Math.Sin(-Inclination)) + (Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((4 * Math.Cos(-Inclination)) + (Math.Sin(-Inclination))) * ZM), M.Y + (((4 * -Math.Sin(-Inclination)) + (Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((4 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((4 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((4 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((4 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((10 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((10 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((10 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((10 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM)))
        'e.Graphics.DrawLine(New Pen(Color.DarkSlateGray, 2), New Point(M.X + (((12 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * zm), M.Y + (((12 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * zm)), New Point(M.X + (((12 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * zm), M.Y + (((12 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * zm)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((16 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((16 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((16 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((16 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((16 * Math.Cos(-Inclination)) + (Math.Sin(-Inclination))) * ZM), M.Y + (((16 * -Math.Sin(-Inclination)) + (Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (Math.Sin(-Inclination))) * ZM), M.Y + (((-20 * Math.Sin(-Inclination)) + (Math.Cos(-Inclination))) * ZM)))

        '----Bottom Rectangle
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZM)))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZM)))

    End Sub
#End Region
End Class
