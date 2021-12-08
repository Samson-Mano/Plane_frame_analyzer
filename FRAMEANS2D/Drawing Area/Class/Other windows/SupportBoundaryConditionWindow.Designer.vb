<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SupportBoundaryConditionWindow
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SupportBoundaryConditionWindow))
        Me._memberBindingNavigator = New System.Windows.Forms.BindingNavigator(Me.components)
        Me._memberBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me._NodeTab = New System.Windows.Forms.TabControl
        Me.SNodeTabpage = New System.Windows.Forms.TabPage
        Me._SNKtxtbox = New System.Windows.Forms.TextBox
        Me._SNdxtxtbox = New System.Windows.Forms.TextBox
        Me._SNdytxtbox = New System.Windows.Forms.TextBox
        Me._SNFRSoption = New System.Windows.Forms.RadioButton
        Me._SNAdd = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me._SNDescriptionLabel = New System.Windows.Forms.Label
        Me._SNSupportPic = New System.Windows.Forms.PictureBox
        Me._SNSSoption = New System.Windows.Forms.RadioButton
        Me._SNPRSoption = New System.Windows.Forms.RadioButton
        Me._SNPSoption = New System.Windows.Forms.RadioButton
        Me._SNFSoption = New System.Windows.Forms.RadioButton
        Me._SNRJoption = New System.Windows.Forms.RadioButton
        Me._SNPJoption = New System.Windows.Forms.RadioButton
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ENodeTabpage = New System.Windows.Forms.TabPage
        Me._ENKtxtbox = New System.Windows.Forms.TextBox
        Me._ENdytxtbox = New System.Windows.Forms.TextBox
        Me._ENdxtxtbox = New System.Windows.Forms.TextBox
        Me._ENFRSoption = New System.Windows.Forms.RadioButton
        Me._ENAdd = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me._ENDescriptionLabel = New System.Windows.Forms.Label
        Me._ENSupportPic = New System.Windows.Forms.PictureBox
        Me._ENSSoption = New System.Windows.Forms.RadioButton
        Me._ENPRSoption = New System.Windows.Forms.RadioButton
        Me._ENPSoption = New System.Windows.Forms.RadioButton
        Me._ENFSoption = New System.Windows.Forms.RadioButton
        Me._ENRJoption = New System.Windows.Forms.RadioButton
        Me._ENPJoption = New System.Windows.Forms.RadioButton
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me._SupportPic = New System.Windows.Forms.PictureBox
        CType(Me._memberBindingNavigator, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._memberBindingNavigator.SuspendLayout()
        CType(Me._memberBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._NodeTab.SuspendLayout()
        Me.SNodeTabpage.SuspendLayout()
        CType(Me._SNSupportPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ENodeTabpage.SuspendLayout()
        CType(Me._ENSupportPic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._SupportPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_memberBindingNavigator
        '
        Me._memberBindingNavigator.AddNewItem = Nothing
        Me._memberBindingNavigator.BindingSource = Me._memberBindingSource
        Me._memberBindingNavigator.CountItem = Me.BindingNavigatorCountItem
        Me._memberBindingNavigator.DeleteItem = Nothing
        Me._memberBindingNavigator.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2})
        Me._memberBindingNavigator.Location = New System.Drawing.Point(0, 0)
        Me._memberBindingNavigator.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me._memberBindingNavigator.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me._memberBindingNavigator.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me._memberBindingNavigator.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me._memberBindingNavigator.Name = "_memberBindingNavigator"
        Me._memberBindingNavigator.PositionItem = Me.BindingNavigatorPositionItem
        Me._memberBindingNavigator.Size = New System.Drawing.Size(557, 27)
        Me._memberBindingNavigator.TabIndex = 3
        Me._memberBindingNavigator.Text = "BindingNavigator1"
        '
        '_memberBindingSource
        '
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(45, 24)
        Me.BindingNavigatorCountItem.Text = "of {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("BindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(23, 24)
        Me.BindingNavigatorMoveFirstItem.Text = "Move first"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("BindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
        Me.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(23, 24)
        Me.BindingNavigatorMovePreviousItem.Text = "Move previous"
        '
        'BindingNavigatorSeparator
        '
        Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
        Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 27)
        '
        'BindingNavigatorPositionItem
        '
        Me.BindingNavigatorPositionItem.AccessibleName = "Position"
        Me.BindingNavigatorPositionItem.AutoSize = False
        Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
        Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(50, 27)
        Me.BindingNavigatorPositionItem.Text = "0"
        Me.BindingNavigatorPositionItem.ToolTipText = "Current position"
        '
        'BindingNavigatorSeparator1
        '
        Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
        Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 27)
        '
        'BindingNavigatorMoveNextItem
        '
        Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveNextItem.Image = CType(resources.GetObject("BindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(23, 24)
        Me.BindingNavigatorMoveNextItem.Text = "Move next"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = CType(resources.GetObject("BindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(23, 24)
        Me.BindingNavigatorMoveLastItem.Text = "Move last"
        '
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 27)
        '
        '_NodeTab
        '
        Me._NodeTab.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._NodeTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me._NodeTab.Controls.Add(Me.SNodeTabpage)
        Me._NodeTab.Controls.Add(Me.ENodeTabpage)
        Me._NodeTab.ItemSize = New System.Drawing.Size(230, 24)
        Me._NodeTab.Location = New System.Drawing.Point(3, 238)
        Me._NodeTab.Multiline = True
        Me._NodeTab.Name = "_NodeTab"
        Me._NodeTab.SelectedIndex = 0
        Me._NodeTab.Size = New System.Drawing.Size(554, 209)
        Me._NodeTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me._NodeTab.TabIndex = 4
        '
        'SNodeTabpage
        '
        Me.SNodeTabpage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SNodeTabpage.Controls.Add(Me._SNKtxtbox)
        Me.SNodeTabpage.Controls.Add(Me._SNdxtxtbox)
        Me.SNodeTabpage.Controls.Add(Me._SNdytxtbox)
        Me.SNodeTabpage.Controls.Add(Me._SNFRSoption)
        Me.SNodeTabpage.Controls.Add(Me._SNAdd)
        Me.SNodeTabpage.Controls.Add(Me.Label1)
        Me.SNodeTabpage.Controls.Add(Me._SNDescriptionLabel)
        Me.SNodeTabpage.Controls.Add(Me._SNSupportPic)
        Me.SNodeTabpage.Controls.Add(Me._SNSSoption)
        Me.SNodeTabpage.Controls.Add(Me._SNPRSoption)
        Me.SNodeTabpage.Controls.Add(Me._SNPSoption)
        Me.SNodeTabpage.Controls.Add(Me._SNFSoption)
        Me.SNodeTabpage.Controls.Add(Me._SNRJoption)
        Me.SNodeTabpage.Controls.Add(Me._SNPJoption)
        Me.SNodeTabpage.Controls.Add(Me.Label7)
        Me.SNodeTabpage.Controls.Add(Me.Label2)
        Me.SNodeTabpage.Controls.Add(Me.Label3)
        Me.SNodeTabpage.Location = New System.Drawing.Point(4, 28)
        Me.SNodeTabpage.Name = "SNodeTabpage"
        Me.SNodeTabpage.Padding = New System.Windows.Forms.Padding(3)
        Me.SNodeTabpage.Size = New System.Drawing.Size(546, 177)
        Me.SNodeTabpage.TabIndex = 0
        Me.SNodeTabpage.Text = "Node - 1"
        Me.SNodeTabpage.UseVisualStyleBackColor = True
        '
        '_SNKtxtbox
        '
        Me._SNKtxtbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SNKtxtbox.Location = New System.Drawing.Point(481, 119)
        Me._SNKtxtbox.Name = "_SNKtxtbox"
        Me._SNKtxtbox.Size = New System.Drawing.Size(58, 22)
        Me._SNKtxtbox.TabIndex = 22
        Me._SNKtxtbox.Text = "1000"
        '
        '_SNdxtxtbox
        '
        Me._SNdxtxtbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SNdxtxtbox.Location = New System.Drawing.Point(481, 102)
        Me._SNdxtxtbox.Name = "_SNdxtxtbox"
        Me._SNdxtxtbox.Size = New System.Drawing.Size(58, 22)
        Me._SNdxtxtbox.TabIndex = 19
        Me._SNdxtxtbox.Text = "0"
        '
        '_SNdytxtbox
        '
        Me._SNdytxtbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SNdytxtbox.Location = New System.Drawing.Point(481, 129)
        Me._SNdytxtbox.Name = "_SNdytxtbox"
        Me._SNdytxtbox.Size = New System.Drawing.Size(58, 22)
        Me._SNdytxtbox.TabIndex = 20
        Me._SNdytxtbox.Text = "0"
        '
        '_SNFRSoption
        '
        Me._SNFRSoption.AutoSize = True
        Me._SNFRSoption.Location = New System.Drawing.Point(6, 120)
        Me._SNFRSoption.Name = "_SNFRSoption"
        Me._SNFRSoption.Size = New System.Drawing.Size(103, 21)
        Me._SNFRSoption.TabIndex = 24
        Me._SNFRSoption.Text = "Fixed Roller"
        Me._SNFRSoption.UseVisualStyleBackColor = True
        '
        '_SNAdd
        '
        Me._SNAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SNAdd.Image = Global.FRAMEANS2D.My.Resources.Resources.Add
        Me._SNAdd.Location = New System.Drawing.Point(152, 6)
        Me._SNAdd.Name = "_SNAdd"
        Me._SNAdd.Size = New System.Drawing.Size(30, 30)
        Me._SNAdd.TabIndex = 23
        Me._SNAdd.Text = "   "
        Me._SNAdd.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(350, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 17)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Description:"
        '
        '_SNDescriptionLabel
        '
        Me._SNDescriptionLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SNDescriptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._SNDescriptionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._SNDescriptionLabel.Location = New System.Drawing.Point(351, 23)
        Me._SNDescriptionLabel.Name = "_SNDescriptionLabel"
        Me._SNDescriptionLabel.Size = New System.Drawing.Size(191, 72)
        Me._SNDescriptionLabel.TabIndex = 7
        Me._SNDescriptionLabel.Text = "Pin Joint allows displacement and moment"
        '
        '_SNSupportPic
        '
        Me._SNSupportPic.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SNSupportPic.BackColor = System.Drawing.Color.White
        Me._SNSupportPic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._SNSupportPic.Location = New System.Drawing.Point(184, 6)
        Me._SNSupportPic.Name = "_SNSupportPic"
        Me._SNSupportPic.Size = New System.Drawing.Size(160, 160)
        Me._SNSupportPic.TabIndex = 6
        Me._SNSupportPic.TabStop = False
        '
        '_SNSSoption
        '
        Me._SNSSoption.AutoSize = True
        Me._SNSSoption.Location = New System.Drawing.Point(6, 143)
        Me._SNSSoption.Name = "_SNSSoption"
        Me._SNSSoption.Size = New System.Drawing.Size(124, 21)
        Me._SNSSoption.TabIndex = 5
        Me._SNSSoption.Text = "Spring Support"
        Me._SNSSoption.UseVisualStyleBackColor = True
        '
        '_SNPRSoption
        '
        Me._SNPRSoption.AutoSize = True
        Me._SNPRSoption.Location = New System.Drawing.Point(6, 97)
        Me._SNPRSoption.Name = "_SNPRSoption"
        Me._SNPRSoption.Size = New System.Drawing.Size(90, 21)
        Me._SNPRSoption.TabIndex = 4
        Me._SNPRSoption.Text = "Pin Roller"
        Me._SNPRSoption.UseVisualStyleBackColor = True
        '
        '_SNPSoption
        '
        Me._SNPSoption.AutoSize = True
        Me._SNPSoption.Location = New System.Drawing.Point(6, 75)
        Me._SNPSoption.Name = "_SNPSoption"
        Me._SNPSoption.Size = New System.Drawing.Size(103, 21)
        Me._SNPSoption.TabIndex = 3
        Me._SNPSoption.Text = "Pin Support"
        Me._SNPSoption.UseVisualStyleBackColor = True
        '
        '_SNFSoption
        '
        Me._SNFSoption.AutoSize = True
        Me._SNFSoption.Location = New System.Drawing.Point(6, 52)
        Me._SNFSoption.Name = "_SNFSoption"
        Me._SNFSoption.Size = New System.Drawing.Size(116, 21)
        Me._SNFSoption.TabIndex = 2
        Me._SNFSoption.Text = "Fixed Support"
        Me._SNFSoption.UseVisualStyleBackColor = True
        '
        '_SNRJoption
        '
        Me._SNRJoption.AutoSize = True
        Me._SNRJoption.Location = New System.Drawing.Point(6, 29)
        Me._SNRJoption.Name = "_SNRJoption"
        Me._SNRJoption.Size = New System.Drawing.Size(95, 21)
        Me._SNRJoption.TabIndex = 1
        Me._SNRJoption.Text = "Rigid Joint"
        Me._SNRJoption.UseVisualStyleBackColor = True
        '
        '_SNPJoption
        '
        Me._SNPJoption.AutoSize = True
        Me._SNPJoption.Enabled = False
        Me._SNPJoption.ForeColor = System.Drawing.SystemColors.InactiveCaptionText
        Me._SNPJoption.Location = New System.Drawing.Point(6, 6)
        Me._SNPJoption.Name = "_SNPJoption"
        Me._SNPJoption.Size = New System.Drawing.Size(83, 21)
        Me._SNPJoption.TabIndex = 0
        Me._SNPJoption.Text = "Pin Joint"
        Me._SNPJoption.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(383, 122)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(87, 17)
        Me.Label7.TabIndex = 21
        Me.Label7.Text = "Stiffness K ="
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(349, 105)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(134, 17)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Sliding dx (10^-3) = "
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(348, 132)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(143, 17)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Sinking dy (10^-3) =  "
        '
        'ENodeTabpage
        '
        Me.ENodeTabpage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ENodeTabpage.Controls.Add(Me._ENKtxtbox)
        Me.ENodeTabpage.Controls.Add(Me._ENdytxtbox)
        Me.ENodeTabpage.Controls.Add(Me._ENdxtxtbox)
        Me.ENodeTabpage.Controls.Add(Me._ENFRSoption)
        Me.ENodeTabpage.Controls.Add(Me._ENAdd)
        Me.ENodeTabpage.Controls.Add(Me.Label6)
        Me.ENodeTabpage.Controls.Add(Me._ENDescriptionLabel)
        Me.ENodeTabpage.Controls.Add(Me._ENSupportPic)
        Me.ENodeTabpage.Controls.Add(Me._ENSSoption)
        Me.ENodeTabpage.Controls.Add(Me._ENPRSoption)
        Me.ENodeTabpage.Controls.Add(Me._ENPSoption)
        Me.ENodeTabpage.Controls.Add(Me._ENFSoption)
        Me.ENodeTabpage.Controls.Add(Me._ENRJoption)
        Me.ENodeTabpage.Controls.Add(Me._ENPJoption)
        Me.ENodeTabpage.Controls.Add(Me.Label4)
        Me.ENodeTabpage.Controls.Add(Me.Label5)
        Me.ENodeTabpage.Controls.Add(Me.Label8)
        Me.ENodeTabpage.Location = New System.Drawing.Point(4, 28)
        Me.ENodeTabpage.Name = "ENodeTabpage"
        Me.ENodeTabpage.Padding = New System.Windows.Forms.Padding(3)
        Me.ENodeTabpage.Size = New System.Drawing.Size(546, 177)
        Me.ENodeTabpage.TabIndex = 1
        Me.ENodeTabpage.Text = "Node - 2"
        Me.ENodeTabpage.UseVisualStyleBackColor = True
        '
        '_ENKtxtbox
        '
        Me._ENKtxtbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ENKtxtbox.Location = New System.Drawing.Point(481, 119)
        Me._ENKtxtbox.Name = "_ENKtxtbox"
        Me._ENKtxtbox.Size = New System.Drawing.Size(58, 22)
        Me._ENKtxtbox.TabIndex = 24
        Me._ENKtxtbox.Text = "1000"
        '
        '_ENdytxtbox
        '
        Me._ENdytxtbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ENdytxtbox.Location = New System.Drawing.Point(481, 129)
        Me._ENdytxtbox.Name = "_ENdytxtbox"
        Me._ENdytxtbox.Size = New System.Drawing.Size(58, 22)
        Me._ENdytxtbox.TabIndex = 18
        Me._ENdytxtbox.Text = "0"
        '
        '_ENdxtxtbox
        '
        Me._ENdxtxtbox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ENdxtxtbox.Location = New System.Drawing.Point(481, 102)
        Me._ENdxtxtbox.Name = "_ENdxtxtbox"
        Me._ENdxtxtbox.Size = New System.Drawing.Size(58, 22)
        Me._ENdxtxtbox.TabIndex = 17
        Me._ENdxtxtbox.Text = "0"
        '
        '_ENFRSoption
        '
        Me._ENFRSoption.AutoSize = True
        Me._ENFRSoption.Location = New System.Drawing.Point(6, 120)
        Me._ENFRSoption.Name = "_ENFRSoption"
        Me._ENFRSoption.Size = New System.Drawing.Size(103, 21)
        Me._ENFRSoption.TabIndex = 26
        Me._ENFRSoption.Text = "Fixed Roller"
        Me._ENFRSoption.UseVisualStyleBackColor = True
        '
        '_ENAdd
        '
        Me._ENAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ENAdd.Image = Global.FRAMEANS2D.My.Resources.Resources.Add
        Me._ENAdd.Location = New System.Drawing.Point(152, 6)
        Me._ENAdd.Name = "_ENAdd"
        Me._ENAdd.Size = New System.Drawing.Size(30, 30)
        Me._ENAdd.TabIndex = 25
        Me._ENAdd.Text = "   "
        Me._ENAdd.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(350, 3)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(83, 17)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Description:"
        '
        '_ENDescriptionLabel
        '
        Me._ENDescriptionLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ENDescriptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._ENDescriptionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._ENDescriptionLabel.Location = New System.Drawing.Point(351, 23)
        Me._ENDescriptionLabel.Name = "_ENDescriptionLabel"
        Me._ENDescriptionLabel.Size = New System.Drawing.Size(191, 72)
        Me._ENDescriptionLabel.TabIndex = 13
        Me._ENDescriptionLabel.Text = "Pin Joint allows displacement and moment"
        '
        '_ENSupportPic
        '
        Me._ENSupportPic.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._ENSupportPic.BackColor = System.Drawing.Color.White
        Me._ENSupportPic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._ENSupportPic.Location = New System.Drawing.Point(184, 6)
        Me._ENSupportPic.Name = "_ENSupportPic"
        Me._ENSupportPic.Size = New System.Drawing.Size(160, 160)
        Me._ENSupportPic.TabIndex = 12
        Me._ENSupportPic.TabStop = False
        '
        '_ENSSoption
        '
        Me._ENSSoption.AutoSize = True
        Me._ENSSoption.Location = New System.Drawing.Point(6, 143)
        Me._ENSSoption.Name = "_ENSSoption"
        Me._ENSSoption.Size = New System.Drawing.Size(124, 21)
        Me._ENSSoption.TabIndex = 11
        Me._ENSSoption.Text = "Spring Support"
        Me._ENSSoption.UseVisualStyleBackColor = True
        '
        '_ENPRSoption
        '
        Me._ENPRSoption.AutoSize = True
        Me._ENPRSoption.Location = New System.Drawing.Point(6, 97)
        Me._ENPRSoption.Name = "_ENPRSoption"
        Me._ENPRSoption.Size = New System.Drawing.Size(90, 21)
        Me._ENPRSoption.TabIndex = 10
        Me._ENPRSoption.Text = "Pin Roller"
        Me._ENPRSoption.UseVisualStyleBackColor = True
        '
        '_ENPSoption
        '
        Me._ENPSoption.AutoSize = True
        Me._ENPSoption.Location = New System.Drawing.Point(6, 75)
        Me._ENPSoption.Name = "_ENPSoption"
        Me._ENPSoption.Size = New System.Drawing.Size(103, 21)
        Me._ENPSoption.TabIndex = 9
        Me._ENPSoption.Text = "Pin Support"
        Me._ENPSoption.UseVisualStyleBackColor = True
        '
        '_ENFSoption
        '
        Me._ENFSoption.AutoSize = True
        Me._ENFSoption.Location = New System.Drawing.Point(6, 52)
        Me._ENFSoption.Name = "_ENFSoption"
        Me._ENFSoption.Size = New System.Drawing.Size(116, 21)
        Me._ENFSoption.TabIndex = 8
        Me._ENFSoption.Text = "Fixed Support"
        Me._ENFSoption.UseVisualStyleBackColor = True
        '
        '_ENRJoption
        '
        Me._ENRJoption.AutoSize = True
        Me._ENRJoption.Location = New System.Drawing.Point(6, 29)
        Me._ENRJoption.Name = "_ENRJoption"
        Me._ENRJoption.Size = New System.Drawing.Size(95, 21)
        Me._ENRJoption.TabIndex = 7
        Me._ENRJoption.Text = "Rigid Joint"
        Me._ENRJoption.UseVisualStyleBackColor = True
        '
        '_ENPJoption
        '
        Me._ENPJoption.AutoSize = True
        Me._ENPJoption.Enabled = False
        Me._ENPJoption.ForeColor = System.Drawing.SystemColors.InactiveCaptionText
        Me._ENPJoption.Location = New System.Drawing.Point(6, 6)
        Me._ENPJoption.Name = "_ENPJoption"
        Me._ENPJoption.Size = New System.Drawing.Size(83, 21)
        Me._ENPJoption.TabIndex = 6
        Me._ENPJoption.Text = "Pin Joint"
        Me._ENPJoption.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(350, 132)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(143, 17)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Sinking dy (10^-3) =  "
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(350, 105)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(134, 17)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Sliding dx (10^-3) = "
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(348, 117)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(87, 17)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "Stiffness K ="
        '
        '_SupportPic
        '
        Me._SupportPic.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SupportPic.BackColor = System.Drawing.Color.White
        Me._SupportPic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._SupportPic.Location = New System.Drawing.Point(3, 28)
        Me._SupportPic.Name = "_SupportPic"
        Me._SupportPic.Size = New System.Drawing.Size(554, 204)
        Me._SupportPic.TabIndex = 2
        Me._SupportPic.TabStop = False
        '
        'SupportBoundaryConditionWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(557, 447)
        Me.Controls.Add(Me._NodeTab)
        Me.Controls.Add(Me._memberBindingNavigator)
        Me.Controls.Add(Me._SupportPic)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SupportBoundaryConditionWindow"
        Me.Opacity = 0.8
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Support - Boundary Condition "
        CType(Me._memberBindingNavigator, System.ComponentModel.ISupportInitialize).EndInit()
        Me._memberBindingNavigator.ResumeLayout(False)
        Me._memberBindingNavigator.PerformLayout()
        CType(Me._memberBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me._NodeTab.ResumeLayout(False)
        Me.SNodeTabpage.ResumeLayout(False)
        Me.SNodeTabpage.PerformLayout()
        CType(Me._SNSupportPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ENodeTabpage.ResumeLayout(False)
        Me.ENodeTabpage.PerformLayout()
        CType(Me._ENSupportPic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._SupportPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents _SupportPic As System.Windows.Forms.PictureBox
    Friend WithEvents _memberBindingNavigator As System.Windows.Forms.BindingNavigator
    Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents _memberBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents _NodeTab As System.Windows.Forms.TabControl
    Friend WithEvents SNodeTabpage As System.Windows.Forms.TabPage
    Friend WithEvents ENodeTabpage As System.Windows.Forms.TabPage
    Friend WithEvents _SNSSoption As System.Windows.Forms.RadioButton
    Friend WithEvents _SNPRSoption As System.Windows.Forms.RadioButton
    Friend WithEvents _SNPSoption As System.Windows.Forms.RadioButton
    Friend WithEvents _SNFSoption As System.Windows.Forms.RadioButton
    Friend WithEvents _SNRJoption As System.Windows.Forms.RadioButton
    Friend WithEvents _SNPJoption As System.Windows.Forms.RadioButton
    Friend WithEvents _ENSSoption As System.Windows.Forms.RadioButton
    Friend WithEvents _ENPRSoption As System.Windows.Forms.RadioButton
    Friend WithEvents _ENPSoption As System.Windows.Forms.RadioButton
    Friend WithEvents _ENFSoption As System.Windows.Forms.RadioButton
    Friend WithEvents _ENRJoption As System.Windows.Forms.RadioButton
    Friend WithEvents _ENPJoption As System.Windows.Forms.RadioButton
    Friend WithEvents _SNDescriptionLabel As System.Windows.Forms.Label
    Friend WithEvents _SNSupportPic As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents _ENDescriptionLabel As System.Windows.Forms.Label
    Friend WithEvents _ENSupportPic As System.Windows.Forms.PictureBox
    Friend WithEvents _ENdytxtbox As System.Windows.Forms.TextBox
    Friend WithEvents _ENdxtxtbox As System.Windows.Forms.TextBox
    Friend WithEvents _SNdytxtbox As System.Windows.Forms.TextBox
    Friend WithEvents _SNdxtxtbox As System.Windows.Forms.TextBox
    Friend WithEvents _SNKtxtbox As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents _ENKtxtbox As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents _SNAdd As System.Windows.Forms.Button
    Friend WithEvents _ENAdd As System.Windows.Forms.Button
    Friend WithEvents _SNFRSoption As System.Windows.Forms.RadioButton
    Friend WithEvents _ENFRSoption As System.Windows.Forms.RadioButton
End Class
