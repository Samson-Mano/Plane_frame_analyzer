Module Indexes
    Public WA2dt As WorkArea2DT
    Public RA2dt As ResultArea2DT

    Public Sub CreateElement2DT()
        WA2dt = New WorkArea2DT
        WA2dt.MdiParent = MDIMain
        WA2dt.Dock = DockStyle.Fill
    End Sub

    Public Sub StartAnalysis()
        If MDIMain._analyze.Checked = True Then
            MDIMain._analyze.Checked = False
            '--Tool Bar
            MDIMain._line.Enabled = True
            MDIMain._arc.Enabled = True
            MDIMain._selectM.Enabled = True

            MDIMain._line.Checked = False
            MDIMain._arc.Checked = False
            MDIMain._selectM.Checked = False

            MDIMain._delete.Enabled = False

            MDIMain._move.Enabled = False
            MDIMain._clone.Enabled = False
            MDIMain._mirror.Enabled = False

            MDIMain._move.Checked = False
            MDIMain._clone.Checked = False
            MDIMain._mirror.Checked = False


            '--Context Menu
            MDIMain._memberproperties.Enabled = False
            MDIMain._AddLoad.Enabled = False
            MDIMain._Addsupport.Enabled = False

            MDIMain.LineToolStripMenuItem.Enabled = True
            MDIMain.ArcToolStripMenuItem.Enabled = True
            MDIMain.SelectMToolStripMenuItem.Enabled = True

            MDIMain.LineToolStripMenuItem.Checked = True
            MDIMain.ArcToolStripMenuItem.Checked = False
            MDIMain.SelectMToolStripMenuItem.Checked = False


            MDIMain.DeleteToolStripMenuItem.Enabled = False
            MDIMain.MoveToolStripMenuItem.Enabled = False
            MDIMain.CloneToolStripMenuItem.Enabled = False
            MDIMain.MirrorToolStripMenuItem.Enabled = False
            MDIMain.MemberpropertiesToolStripMenuItem.Enabled = False
            MDIMain.AddloadToolStripMenuItem.Enabled = False
            MDIMain.AddsupportToolStripMenuItem.Enabled = False

            MDIMain.ShearForceToolStripMenuItem.Enabled = False
            MDIMain.BendingMomentToolStripMenuItem.Enabled = False
            MDIMain.ReactionToolStripMenuItem.Enabled = False
            MDIMain.DeflectionToolStripMenuItem.Enabled = False

            MDIMain.ShearForceToolStripMenuItem.Checked = False
            MDIMain.BendingMomentToolStripMenuItem.Checked = False
            MDIMain.ReactionToolStripMenuItem.Checked = False
            MDIMain.DeflectionToolStripMenuItem.Checked = False

            MDIMain._shearforce.Enabled = False
            MDIMain._bendingmoment.Enabled = False
            MDIMain._reaction.Enabled = False
            MDIMain._deflection.Enabled = False
            MDIMain._matrix.Enabled = False

            MDIMain.ShearForceToolStripMenuItem.Checked = False
            MDIMain._shearforce.Checked = False
            MDIMain._bendingmoment.Checked = False
            MDIMain._reaction.Checked = False
            MDIMain._deflection.Checked = False

            MDIMain.Cursorimg = My.Resources.line

            MDIMain._line.Checked = True

            MDIMain._analyze.Image = My.Resources.analyze
            MDIMain.AnalyzeToolStripMenuItem.Image = My.Resources.analyze

            RA2dt.Dispose()
            RA2dt.Close()
            WA2dt.Show()
        Else
 


            MDIMain._analyze.Checked = True

            WA2dt.selLine.Clear()
            WA2dt.Linflg = False
            WA2dt.Arcflg = False

            '--Tool Bar
            MDIMain._line.Enabled = False
            MDIMain._arc.Enabled = False
            MDIMain._selectM.Enabled = False

            MDIMain._line.Checked = False
            MDIMain._arc.Checked = False
            MDIMain._selectM.Checked = False

            MDIMain._delete.Enabled = False

            MDIMain._move.Enabled = False
            MDIMain._clone.Enabled = False
            MDIMain._mirror.Enabled = False

            MDIMain._move.Checked = False
            MDIMain._clone.Checked = False
            MDIMain._mirror.Checked = False


            '--Context Menu
            MDIMain._memberproperties.Enabled = False
            MDIMain._AddLoad.Enabled = False
            MDIMain._Addsupport.Enabled = False

            MDIMain.LineToolStripMenuItem.Enabled = False
            MDIMain.ArcToolStripMenuItem.Enabled = False
            MDIMain.SelectMToolStripMenuItem.Enabled = False

            MDIMain.LineToolStripMenuItem.Checked = False
            MDIMain.ArcToolStripMenuItem.Checked = False
            MDIMain.SelectMToolStripMenuItem.Checked = False

            MDIMain.DeleteToolStripMenuItem.Enabled = False
            MDIMain.MoveToolStripMenuItem.Enabled = False
            MDIMain.CloneToolStripMenuItem.Enabled = False
            MDIMain.MirrorToolStripMenuItem.Enabled = False
            MDIMain.MemberpropertiesToolStripMenuItem.Enabled = False
            MDIMain.AddloadToolStripMenuItem.Enabled = False
            MDIMain.AddsupportToolStripMenuItem.Enabled = False

            MDIMain.Cursorimg = My.Resources.shearforce

            MDIMain.ShearForceToolStripMenuItem.Enabled = True
            MDIMain.BendingMomentToolStripMenuItem.Enabled = True
            MDIMain.ReactionToolStripMenuItem.Enabled = True
            MDIMain.DeflectionToolStripMenuItem.Enabled = True

            MDIMain._shearforce.Enabled = True
            MDIMain._bendingmoment.Enabled = True
            MDIMain._reaction.Enabled = True
            MDIMain._deflection.Enabled = True
            MDIMain._matrix.Enabled = True

            MDIMain.ShearForceToolStripMenuItem.Checked = True
            MDIMain._shearforce.Checked = True


            MDIMain._analyze.Image = My.Resources.create
            MDIMain.AnalyzeToolStripMenuItem.Image = My.Resources.create
            WA2dt.Hide()

            '-----Main Analysis Call

            CreateResult2DT()
            RA2dt.Analyzer = New Frame_FE_Analyzer
            RA2dt.Show()

            If RA2dt.Analyzer.Failed = True Then
                MDIMain._analyze.Checked = True
                StartAnalysis()
            End If


        End If
    End Sub

    Private Sub CreateResult2DT()
        RA2dt = New ResultArea2DT
        RA2dt.MdiParent = MDIMain
        RA2dt.Dock = DockStyle.Fill
    End Sub
End Module
