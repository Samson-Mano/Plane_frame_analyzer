<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MDIMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MDIMain))
        Me.StatusStrip = New System.Windows.Forms.StatusStrip
        Me._HelpStatusLabel = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.___ToolBarPanel = New System.Windows.Forms.Panel
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me._filemenu = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me._line = New System.Windows.Forms.ToolStripButton
        Me._arc = New System.Windows.Forms.ToolStripButton
        Me._selectM = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me._delete = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me._move = New System.Windows.Forms.ToolStripButton
        Me._clone = New System.Windows.Forms.ToolStripButton
        Me._mirror = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me._memberproperties = New System.Windows.Forms.ToolStripButton
        Me._AddLoad = New System.Windows.Forms.ToolStripButton
        Me._Addsupport = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
        Me._analyze = New System.Windows.Forms.ToolStripButton
        Me._shearforce = New System.Windows.Forms.ToolStripButton
        Me._bendingmoment = New System.Windows.Forms.ToolStripButton
        Me._deflection = New System.Windows.Forms.ToolStripButton
        Me._reaction = New System.Windows.Forms.ToolStripButton
        Me._matrix = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me._viewOption = New System.Windows.Forms.ToolStripDropDownButton
        Me.LengthviewoptionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NodeNoviewoptionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LoadviewoptionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ResultviewoptionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me._pan = New System.Windows.Forms.ToolStripButton
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.LineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ArcToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SelectMToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator
        Me.MoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CloneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MirrorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator
        Me.MemberpropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddloadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddsupportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator
        Me.AnalyzeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ShearForceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.BendingMomentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DeflectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ReactionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me._Commandtxtbox = New System.Windows.Forms.TextBox
        Me._commandtxtlabel = New System.Windows.Forms.Label
        Me.StatusStrip.SuspendLayout()
        Me.___ToolBarPanel.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip
        '
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._HelpStatusLabel})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 467)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(1028, 22)
        Me.StatusStrip.TabIndex = 7
        Me.StatusStrip.Text = "StatusStrip"
        '
        '_HelpStatusLabel
        '
        Me._HelpStatusLabel.Name = "_HelpStatusLabel"
        Me._HelpStatusLabel.Size = New System.Drawing.Size(39, 17)
        Me._HelpStatusLabel.Text = "Status"
        '
        '___ToolBarPanel
        '
        Me.___ToolBarPanel.Controls.Add(Me.ToolStrip1)
        Me.___ToolBarPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.___ToolBarPanel.Location = New System.Drawing.Point(0, 0)
        Me.___ToolBarPanel.Name = "___ToolBarPanel"
        Me.___ToolBarPanel.Size = New System.Drawing.Size(1028, 52)
        Me.___ToolBarPanel.TabIndex = 11
        '
        'ToolStrip1
        '
        Me.ToolStrip1.AutoSize = False
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._filemenu, Me.ToolStripSeparator4, Me._line, Me._arc, Me._selectM, Me.ToolStripSeparator6, Me._delete, Me.ToolStripSeparator2, Me._move, Me._clone, Me._mirror, Me.ToolStripSeparator1, Me._memberproperties, Me._AddLoad, Me._Addsupport, Me.ToolStripSeparator8, Me._analyze, Me._shearforce, Me._bendingmoment, Me._deflection, Me._reaction, Me._matrix, Me.ToolStripSeparator7, Me._viewOption, Me.ToolStripSeparator5, Me._pan})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1028, 52)
        Me.ToolStrip1.Stretch = True
        Me.ToolStrip1.TabIndex = 9
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        '_filemenu
        '
        Me._filemenu.AutoSize = False
        Me._filemenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._filemenu.Image = Global.FRAMEANS2D.My.Resources.Resources.File
        Me._filemenu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._filemenu.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._filemenu.Name = "_filemenu"
        Me._filemenu.Size = New System.Drawing.Size(50, 50)
        Me._filemenu.Text = "File"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 52)
        '
        '_line
        '
        Me._line.AutoSize = False
        Me._line.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._line.Image = Global.FRAMEANS2D.My.Resources.Resources.line
        Me._line.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._line.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._line.Name = "_line"
        Me._line.Size = New System.Drawing.Size(40, 40)
        Me._line.Text = "Draw Line (Ctrl + L)"
        '
        '_arc
        '
        Me._arc.AutoSize = False
        Me._arc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._arc.Image = Global.FRAMEANS2D.My.Resources.Resources.arc
        Me._arc.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._arc.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._arc.Name = "_arc"
        Me._arc.Size = New System.Drawing.Size(40, 40)
        Me._arc.Text = "Draw Arc"
        '
        '_selectM
        '
        Me._selectM.AutoSize = False
        Me._selectM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._selectM.Image = Global.FRAMEANS2D.My.Resources.Resources.selectmember
        Me._selectM.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._selectM.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._selectM.Name = "_selectM"
        Me._selectM.Size = New System.Drawing.Size(40, 40)
        Me._selectM.Text = "Select"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 52)
        '
        '_delete
        '
        Me._delete.AutoSize = False
        Me._delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._delete.Enabled = False
        Me._delete.Image = Global.FRAMEANS2D.My.Resources.Resources.del
        Me._delete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._delete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._delete.Name = "_delete"
        Me._delete.Size = New System.Drawing.Size(40, 40)
        Me._delete.Text = "Delete (Del)"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 52)
        '
        '_move
        '
        Me._move.AutoSize = False
        Me._move.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._move.Enabled = False
        Me._move.Image = Global.FRAMEANS2D.My.Resources.Resources.move
        Me._move.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._move.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._move.Name = "_move"
        Me._move.Size = New System.Drawing.Size(40, 40)
        Me._move.Text = "Move (Ctrl + X)"
        '
        '_clone
        '
        Me._clone.AutoSize = False
        Me._clone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._clone.Enabled = False
        Me._clone.Image = Global.FRAMEANS2D.My.Resources.Resources.clone
        Me._clone.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._clone.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._clone.Name = "_clone"
        Me._clone.Size = New System.Drawing.Size(40, 40)
        Me._clone.Text = "Clone (Ctrl + C)"
        '
        '_mirror
        '
        Me._mirror.AutoSize = False
        Me._mirror.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._mirror.Enabled = False
        Me._mirror.Image = Global.FRAMEANS2D.My.Resources.Resources.Mirror
        Me._mirror.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._mirror.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._mirror.Name = "_mirror"
        Me._mirror.Size = New System.Drawing.Size(40, 40)
        Me._mirror.Text = "Flip (Ctrl + V)"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 52)
        '
        '_memberproperties
        '
        Me._memberproperties.AutoSize = False
        Me._memberproperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._memberproperties.Enabled = False
        Me._memberproperties.Image = Global.FRAMEANS2D.My.Resources.Resources.propert
        Me._memberproperties.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._memberproperties.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._memberproperties.Name = "_memberproperties"
        Me._memberproperties.Size = New System.Drawing.Size(40, 40)
        Me._memberproperties.Text = "Member Properties (Ctrl + D)"
        '
        '_AddLoad
        '
        Me._AddLoad.AutoSize = False
        Me._AddLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._AddLoad.Enabled = False
        Me._AddLoad.Image = Global.FRAMEANS2D.My.Resources.Resources.Load
        Me._AddLoad.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._AddLoad.Name = "_AddLoad"
        Me._AddLoad.Size = New System.Drawing.Size(40, 40)
        Me._AddLoad.Text = "Add Load (Ctrl + A)"
        '
        '_Addsupport
        '
        Me._Addsupport.AutoSize = False
        Me._Addsupport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._Addsupport.Enabled = False
        Me._Addsupport.Image = Global.FRAMEANS2D.My.Resources.Resources.support
        Me._Addsupport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._Addsupport.Name = "_Addsupport"
        Me._Addsupport.Size = New System.Drawing.Size(40, 40)
        Me._Addsupport.Text = "Add Support (Ctrl + S)"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 52)
        '
        '_analyze
        '
        Me._analyze.AutoSize = False
        Me._analyze.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._analyze.Image = Global.FRAMEANS2D.My.Resources.Resources.analyze
        Me._analyze.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._analyze.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._analyze.Name = "_analyze"
        Me._analyze.Size = New System.Drawing.Size(40, 40)
        Me._analyze.Text = "Analyze (F5)"
        '
        '_shearforce
        '
        Me._shearforce.AutoSize = False
        Me._shearforce.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._shearforce.Enabled = False
        Me._shearforce.Image = Global.FRAMEANS2D.My.Resources.Resources.shearforce
        Me._shearforce.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._shearforce.Name = "_shearforce"
        Me._shearforce.Size = New System.Drawing.Size(40, 40)
        Me._shearforce.Text = "Shear Force"
        '
        '_bendingmoment
        '
        Me._bendingmoment.AutoSize = False
        Me._bendingmoment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._bendingmoment.Enabled = False
        Me._bendingmoment.Image = Global.FRAMEANS2D.My.Resources.Resources.BendingMoment
        Me._bendingmoment.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._bendingmoment.Name = "_bendingmoment"
        Me._bendingmoment.Size = New System.Drawing.Size(40, 40)
        Me._bendingmoment.Text = "Bending Moment"
        '
        '_deflection
        '
        Me._deflection.AutoSize = False
        Me._deflection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._deflection.Enabled = False
        Me._deflection.Image = Global.FRAMEANS2D.My.Resources.Resources.Deflection
        Me._deflection.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._deflection.Name = "_deflection"
        Me._deflection.Size = New System.Drawing.Size(40, 40)
        Me._deflection.Text = "Deflection"
        '
        '_reaction
        '
        Me._reaction.AutoSize = False
        Me._reaction.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._reaction.Enabled = False
        Me._reaction.Image = Global.FRAMEANS2D.My.Resources.Resources.reaction
        Me._reaction.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._reaction.Name = "_reaction"
        Me._reaction.Size = New System.Drawing.Size(40, 40)
        Me._reaction.Text = "Reaction"
        '
        '_matrix
        '
        Me._matrix.AutoSize = False
        Me._matrix.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._matrix.Image = Global.FRAMEANS2D.My.Resources.Resources.RRMatrixDetails
        Me._matrix.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._matrix.Name = "_matrix"
        Me._matrix.Size = New System.Drawing.Size(40, 40)
        Me._matrix.Text = "Matrix_Details"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 52)
        '
        '_viewOption
        '
        Me._viewOption.AutoSize = False
        Me._viewOption.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._viewOption.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LengthviewoptionToolStripMenuItem, Me.NodeNoviewoptionToolStripMenuItem, Me.LoadviewoptionToolStripMenuItem, Me.ResultviewoptionToolStripMenuItem})
        Me._viewOption.Image = Global.FRAMEANS2D.My.Resources.Resources.option_i
        Me._viewOption.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._viewOption.Name = "_viewOption"
        Me._viewOption.Size = New System.Drawing.Size(40, 40)
        Me._viewOption.Text = "View Option"
        '
        'LengthviewoptionToolStripMenuItem
        '
        Me.LengthviewoptionToolStripMenuItem.Checked = True
        Me.LengthviewoptionToolStripMenuItem.CheckOnClick = True
        Me.LengthviewoptionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.LengthviewoptionToolStripMenuItem.Name = "LengthviewoptionToolStripMenuItem"
        Me.LengthviewoptionToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.LengthviewoptionToolStripMenuItem.Text = "Length"
        '
        'NodeNoviewoptionToolStripMenuItem
        '
        Me.NodeNoviewoptionToolStripMenuItem.Checked = True
        Me.NodeNoviewoptionToolStripMenuItem.CheckOnClick = True
        Me.NodeNoviewoptionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.NodeNoviewoptionToolStripMenuItem.Name = "NodeNoviewoptionToolStripMenuItem"
        Me.NodeNoviewoptionToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NodeNoviewoptionToolStripMenuItem.Text = "Node No"
        '
        'LoadviewoptionToolStripMenuItem
        '
        Me.LoadviewoptionToolStripMenuItem.Checked = True
        Me.LoadviewoptionToolStripMenuItem.CheckOnClick = True
        Me.LoadviewoptionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.LoadviewoptionToolStripMenuItem.Name = "LoadviewoptionToolStripMenuItem"
        Me.LoadviewoptionToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.LoadviewoptionToolStripMenuItem.Text = "Load"
        '
        'ResultviewoptionToolStripMenuItem
        '
        Me.ResultviewoptionToolStripMenuItem.Checked = True
        Me.ResultviewoptionToolStripMenuItem.CheckOnClick = True
        Me.ResultviewoptionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ResultviewoptionToolStripMenuItem.Name = "ResultviewoptionToolStripMenuItem"
        Me.ResultviewoptionToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ResultviewoptionToolStripMenuItem.Text = "Result"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 52)
        '
        '_pan
        '
        Me._pan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me._pan.Image = CType(resources.GetObject("_pan.Image"), System.Drawing.Image)
        Me._pan.ImageTransparentColor = System.Drawing.Color.Magenta
        Me._pan.Name = "_pan"
        Me._pan.Size = New System.Drawing.Size(23, 49)
        Me._pan.Text = "ToolStripButton1"
        Me._pan.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.OpenToolStripMenuItem, Me.SaveToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 180)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.NewM
        Me.NewToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(122, 44)
        Me.NewToolStripMenuItem.Text = "New "
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.OpenM
        Me.OpenToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 44)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.saveM
        Me.SaveToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(122, 44)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.CloseM
        Me.ExitToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 44)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LineToolStripMenuItem, Me.ArcToolStripMenuItem, Me.SelectMToolStripMenuItem, Me.ToolStripSeparator3, Me.DeleteToolStripMenuItem, Me.ToolStripSeparator10, Me.MoveToolStripMenuItem, Me.CloneToolStripMenuItem, Me.MirrorToolStripMenuItem, Me.ToolStripSeparator11, Me.MemberpropertiesToolStripMenuItem, Me.AddloadToolStripMenuItem, Me.AddsupportToolStripMenuItem, Me.ToolStripSeparator9, Me.AnalyzeToolStripMenuItem, Me.ShearForceToolStripMenuItem, Me.BendingMomentToolStripMenuItem, Me.DeflectionToolStripMenuItem, Me.ReactionToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(180, 418)
        '
        'LineToolStripMenuItem
        '
        Me.LineToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.line
        Me.LineToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.LineToolStripMenuItem.Name = "LineToolStripMenuItem"
        Me.LineToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.LineToolStripMenuItem.Text = "Line"
        '
        'ArcToolStripMenuItem
        '
        Me.ArcToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.arc
        Me.ArcToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ArcToolStripMenuItem.Name = "ArcToolStripMenuItem"
        Me.ArcToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.ArcToolStripMenuItem.Text = "Arc"
        '
        'SelectMToolStripMenuItem
        '
        Me.SelectMToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.selectmember
        Me.SelectMToolStripMenuItem.Name = "SelectMToolStripMenuItem"
        Me.SelectMToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.SelectMToolStripMenuItem.Text = "Select Member"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(176, 6)
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Enabled = False
        Me.DeleteToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.del
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(176, 6)
        '
        'MoveToolStripMenuItem
        '
        Me.MoveToolStripMenuItem.Enabled = False
        Me.MoveToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.move
        Me.MoveToolStripMenuItem.Name = "MoveToolStripMenuItem"
        Me.MoveToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.MoveToolStripMenuItem.Text = "Move"
        '
        'CloneToolStripMenuItem
        '
        Me.CloneToolStripMenuItem.Enabled = False
        Me.CloneToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.clone
        Me.CloneToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.CloneToolStripMenuItem.Name = "CloneToolStripMenuItem"
        Me.CloneToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.CloneToolStripMenuItem.Text = "Clone"
        '
        'MirrorToolStripMenuItem
        '
        Me.MirrorToolStripMenuItem.Enabled = False
        Me.MirrorToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.Mirror
        Me.MirrorToolStripMenuItem.Name = "MirrorToolStripMenuItem"
        Me.MirrorToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.MirrorToolStripMenuItem.Text = "Flip"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(176, 6)
        '
        'MemberpropertiesToolStripMenuItem
        '
        Me.MemberpropertiesToolStripMenuItem.Enabled = False
        Me.MemberpropertiesToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.propert
        Me.MemberpropertiesToolStripMenuItem.Name = "MemberpropertiesToolStripMenuItem"
        Me.MemberpropertiesToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.MemberpropertiesToolStripMenuItem.Text = "Member Properties"
        '
        'AddloadToolStripMenuItem
        '
        Me.AddloadToolStripMenuItem.Enabled = False
        Me.AddloadToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.Load
        Me.AddloadToolStripMenuItem.Name = "AddloadToolStripMenuItem"
        Me.AddloadToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.AddloadToolStripMenuItem.Text = "Add Load"
        '
        'AddsupportToolStripMenuItem
        '
        Me.AddsupportToolStripMenuItem.Enabled = False
        Me.AddsupportToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.support
        Me.AddsupportToolStripMenuItem.Name = "AddsupportToolStripMenuItem"
        Me.AddsupportToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.AddsupportToolStripMenuItem.Text = "Add Support"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(176, 6)
        '
        'AnalyzeToolStripMenuItem
        '
        Me.AnalyzeToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.analyze
        Me.AnalyzeToolStripMenuItem.Name = "AnalyzeToolStripMenuItem"
        Me.AnalyzeToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.AnalyzeToolStripMenuItem.Text = "Analyze"
        '
        'ShearForceToolStripMenuItem
        '
        Me.ShearForceToolStripMenuItem.Enabled = False
        Me.ShearForceToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.shearforce
        Me.ShearForceToolStripMenuItem.Name = "ShearForceToolStripMenuItem"
        Me.ShearForceToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.ShearForceToolStripMenuItem.Text = "Shear Force"
        '
        'BendingMomentToolStripMenuItem
        '
        Me.BendingMomentToolStripMenuItem.Enabled = False
        Me.BendingMomentToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.BendingMoment
        Me.BendingMomentToolStripMenuItem.Name = "BendingMomentToolStripMenuItem"
        Me.BendingMomentToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.BendingMomentToolStripMenuItem.Text = "Bending Moment"
        '
        'DeflectionToolStripMenuItem
        '
        Me.DeflectionToolStripMenuItem.Enabled = False
        Me.DeflectionToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.Deflection
        Me.DeflectionToolStripMenuItem.Name = "DeflectionToolStripMenuItem"
        Me.DeflectionToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.DeflectionToolStripMenuItem.Text = "Deflection"
        '
        'ReactionToolStripMenuItem
        '
        Me.ReactionToolStripMenuItem.Enabled = False
        Me.ReactionToolStripMenuItem.Image = Global.FRAMEANS2D.My.Resources.Resources.reaction
        Me.ReactionToolStripMenuItem.Name = "ReactionToolStripMenuItem"
        Me.ReactionToolStripMenuItem.Size = New System.Drawing.Size(179, 26)
        Me.ReactionToolStripMenuItem.Text = "Reaction"
        '
        '_Commandtxtbox
        '
        Me._Commandtxtbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._Commandtxtbox.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Commandtxtbox.ForeColor = System.Drawing.Color.Blue
        Me._Commandtxtbox.Location = New System.Drawing.Point(880, 469)
        Me._Commandtxtbox.Name = "_Commandtxtbox"
        Me._Commandtxtbox.Size = New System.Drawing.Size(146, 22)
        Me._Commandtxtbox.TabIndex = 13
        Me._Commandtxtbox.Visible = False
        '
        '_commandtxtlabel
        '
        Me._commandtxtlabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._commandtxtlabel.AutoSize = True
        Me._commandtxtlabel.Font = New System.Drawing.Font("Verdana", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._commandtxtlabel.Location = New System.Drawing.Point(734, 473)
        Me._commandtxtlabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me._commandtxtlabel.Name = "_commandtxtlabel"
        Me._commandtxtlabel.Size = New System.Drawing.Size(152, 13)
        Me._commandtxtlabel.TabIndex = 15
        Me._commandtxtlabel.Text = "<Length>   <Inclination>"
        Me._commandtxtlabel.Visible = False
        '
        'MDIMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 489)
        Me.Controls.Add(Me._commandtxtlabel)
        Me.Controls.Add(Me._Commandtxtbox)
        Me.Controls.Add(Me.___ToolBarPanel)
        Me.Controls.Add(Me.StatusStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Name = "MDIMain"
        Me.Text = "Plane Frame Analyzer (Static) ----- Developed by Samson Mano <https://sites.googl" & _
            "e.com/site/samsoninfinite/>"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.___ToolBarPanel.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents _HelpStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents ___ToolBarPanel As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents LineToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ArcToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectMToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MoveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MirrorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MemberpropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddloadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddsupportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AnalyzeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShearForceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BendingMomentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeflectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReactionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents _Commandtxtbox As System.Windows.Forms.TextBox
    Friend WithEvents _commandtxtlabel As System.Windows.Forms.Label
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents _filemenu As System.Windows.Forms.ToolStripButton
    Friend WithEvents _line As System.Windows.Forms.ToolStripButton
    Friend WithEvents _arc As System.Windows.Forms.ToolStripButton
    Friend WithEvents _selectM As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents _delete As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents _move As System.Windows.Forms.ToolStripButton
    Friend WithEvents _clone As System.Windows.Forms.ToolStripButton
    Friend WithEvents _mirror As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents _memberproperties As System.Windows.Forms.ToolStripButton
    Friend WithEvents _AddLoad As System.Windows.Forms.ToolStripButton
    Friend WithEvents _Addsupport As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents _analyze As System.Windows.Forms.ToolStripButton
    Friend WithEvents _shearforce As System.Windows.Forms.ToolStripButton
    Friend WithEvents _bendingmoment As System.Windows.Forms.ToolStripButton
    Friend WithEvents _deflection As System.Windows.Forms.ToolStripButton
    Friend WithEvents _reaction As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents _pan As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents _viewOption As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents LengthviewoptionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NodeNoviewoptionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadviewoptionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ResultviewoptionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents _matrix As System.Windows.Forms.ToolStripButton

End Class
