Imports System.Drawing.Drawing2D
Public Class SupportBoundaryConditionWindow
    Dim TempEL As SelectedElement
    Dim SNsupport As SupportDetails
    Dim ENsupport As SupportDetails
    Dim _DrgFlg As Boolean = False

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
        Dim _PJ As Boolean = True '-- Pin Joint
        Dim _RJ As Boolean '-- Rigid Joint
        Dim _PS As Boolean '-- Pin Support
        Dim _PRS As Boolean '-- Pin Roller Support
        Dim _FRS As Boolean '-- Fixed Roller Support
        Dim _FS As Boolean '-- Fixed Support
        Dim _SS As Boolean '-- Spring Support
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
            Me.Height = (520 * (MDIMain.Height / MDIMain.Width)) + 235 + 25
        Else
            Me.Height = 520 + 235 + 25 '(MDIMain.Width / 3)
            Me.Width = (520 * (MDIMain.Width / MDIMain.Height))
        End If
        _NodeTab.ItemSize = New Size((Me.Width / 2) - 11, 25)

        CoordFix(El)
        For Each itm In WA2dt.Mem
            _memberBindingSource.Add(itm)
            If itm.Equals(El) Then
                _memberBindingSource.Position = WA2dt.Mem.IndexOf(itm) + 1
            End If
        Next
        SNSupportOptionchange()
        ENSupportOptionchange()
    End Sub

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
        _NodeTab.TabPages(0).Text = "Node - " & TempEL.SNode
        _NodeTab.TabPages(1).Text = "Node - " & TempEL.ENode

        SNsupport = New SupportDetails(El.SN.Support.PJ, El.SN.Support.RJ, El.SN.Support.PS, El.SN.Support.PRS, El.SN.Support.FRS, El.SN.Support.FS, El.SN.Support.SS, El.SN.Support.supportinclination, El.SN.Support.settlementdx, El.SN.Support.settlementdy, El.SN.Support.stiffnessK)
        ENsupport = New SupportDetails(El.EN.Support.PJ, El.EN.Support.RJ, El.EN.Support.PS, El.EN.Support.PRS, El.EN.Support.FRS, El.EN.Support.FS, El.EN.Support.SS, El.EN.Support.supportinclination, El.EN.Support.settlementdx, El.EN.Support.settlementdy, El.EN.Support.stiffnessK)

        _SNPJoption.Checked = El.SN.Support.PJ
        _SNRJoption.Checked = El.SN.Support.RJ
        _SNPSoption.Checked = El.SN.Support.PS
        _SNPRSoption.Checked = El.SN.Support.PRS
        _SNFRSoption.Checked = El.SN.Support.FRS
        _SNFSoption.Checked = El.SN.Support.FS
        _SNSSoption.Checked = El.SN.Support.SS
        _SNdxtxtbox.Text = El.SN.Support.settlementdx
        _SNdytxtbox.Text = El.SN.Support.settlementdy
        _SNKtxtbox.Text = El.SN.Support.stiffnessK

        _ENPJoption.Checked = El.EN.Support.PJ
        _ENRJoption.Checked = El.EN.Support.RJ
        _ENPSoption.Checked = El.EN.Support.PS
        _ENPRSoption.Checked = El.EN.Support.PRS
        _ENFRSoption.Checked = El.EN.Support.FRS
        _ENFSoption.Checked = El.EN.Support.FS
        _ENSSoption.Checked = El.EN.Support.SS
        _ENdxtxtbox.Text = El.EN.Support.settlementdx
        _ENdytxtbox.Text = El.EN.Support.settlementdy
        _ENKtxtbox.Text = El.EN.Support.stiffnessK

        _SupportPic.Refresh()
        _SNSupportPic.Refresh()
        _ENSupportPic.Refresh()

    End Sub

    Private Sub SupportBoundaryConditionWindow_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        WA2dt.selLine.Clear()
        MDIMain._DMCMenable()
        WA2dt.MainPic.Refresh()
    End Sub

#Region "Correct Node Support Error"
    Private Sub SupportBoundaryConditionWindow_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        CorrectNodeSupportError()
    End Sub

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

    Private Sub SupportBoundaryConditionWindow_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AddHandler _SNPJoption.CheckedChanged, AddressOf SNSupportOptionchange
        AddHandler _SNRJoption.CheckedChanged, AddressOf SNSupportOptionchange
        AddHandler _SNPSoption.CheckedChanged, AddressOf SNSupportOptionchange
        AddHandler _SNPRSoption.CheckedChanged, AddressOf SNSupportOptionchange
        AddHandler _SNFSoption.CheckedChanged, AddressOf SNSupportOptionchange
        AddHandler _SNSSoption.CheckedChanged, AddressOf SNSupportOptionchange

        AddHandler _ENPJoption.CheckedChanged, AddressOf ENSupportOptionchange
        AddHandler _ENRJoption.CheckedChanged, AddressOf ENSupportOptionchange
        AddHandler _ENPSoption.CheckedChanged, AddressOf ENSupportOptionchange
        AddHandler _ENPRSoption.CheckedChanged, AddressOf ENSupportOptionchange
        AddHandler _ENFSoption.CheckedChanged, AddressOf ENSupportOptionchange
        AddHandler _ENSSoption.CheckedChanged, AddressOf ENSupportOptionchange
    End Sub

    Private Sub _memberNavigator_RefreshItems(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _memberBindingSource.CurrentChanged
        If _memberBindingSource.Count <> 0 Then
            CoordFix(_memberBindingSource.Current)
            WA2dt.selLine(0) = _memberBindingSource.IndexOf(_memberBindingSource.Current)
            _SupportPic.Refresh()
            WA2dt.MainPic.Refresh()
        End If
    End Sub

    Private Sub _NodeTab_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _NodeTab.SelectedIndexChanged
        _SupportPic.Refresh()
    End Sub

    Private Sub SNSupportOptionchange()
        If _SNPJoption.Checked = True Then
            SNsupport.PJ = True
            SNsupport.RJ = False
            SNsupport.PS = False
            SNsupport.PRS = False
            SNsupport.FRS = False
            SNsupport.FS = False
            SNsupport.SS = False
            Label2.Visible = False
            Label3.Visible = False
            Label7.Visible = False
            _SNdxtxtbox.Visible = False
            _SNdytxtbox.Visible = False
            _SNKtxtbox.Visible = False
            _SNDescriptionLabel.Text = " Pin Joint restricts displacement on all directions but it wont restraint moment"
        ElseIf _SNRJoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.RJ = True
            SNsupport.PS = False
            SNsupport.PRS = False
            SNsupport.FRS = False
            SNsupport.FS = False
            SNsupport.SS = False
            Label2.Visible = False
            Label3.Visible = False
            Label7.Visible = False
            _SNdxtxtbox.Visible = False
            _SNdytxtbox.Visible = False
            _SNKtxtbox.Visible = False
            _SNDescriptionLabel.Text = " Rigid Joint restricts displacement & moment on all directions"
        ElseIf _SNPSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.RJ = False
            SNsupport.PS = True
            SNsupport.PRS = False
            SNsupport.FRS = False
            SNsupport.FS = False
            SNsupport.SS = False
            Label2.Visible = True
            Label3.Visible = True
            Label7.Visible = False
            _SNdxtxtbox.Visible = True
            _SNdytxtbox.Visible = True
            _SNKtxtbox.Visible = False
            _SNDescriptionLabel.Text = " Pin Support restricts displacement in all directions but it will not restricts moment"
        ElseIf _SNPRSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.RJ = False
            SNsupport.PS = False
            SNsupport.PRS = True
            SNsupport.FRS = False
            SNsupport.FS = False
            SNsupport.SS = False
            Label2.Visible = False
            Label3.Visible = True
            Label7.Visible = False
            _SNdxtxtbox.Visible = False
            _SNdytxtbox.Visible = True
            _SNKtxtbox.Visible = False
            _SNDescriptionLabel.Text = "Pin Roller Support restricts displacement in Double plane but it will not restricts moment"
        ElseIf _SNFRSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.RJ = False
            SNsupport.PS = False
            SNsupport.PRS = False
            SNsupport.FRS = True
            SNsupport.FS = False
            SNsupport.SS = False
            Label2.Visible = False
            Label3.Visible = True
            Label7.Visible = False
            _SNdxtxtbox.Visible = False
            _SNdytxtbox.Visible = True
            _SNKtxtbox.Visible = False
            _SNDescriptionLabel.Text = "Fixed Roller Support restricts displacement in Double plane and restricts moment"
        ElseIf _SNFSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.RJ = False
            SNsupport.PS = False
            SNsupport.PRS = False
            SNsupport.FRS = False
            SNsupport.FS = True
            SNsupport.SS = False
            Label2.Visible = True
            Label3.Visible = True
            Label7.Visible = False
            _SNdxtxtbox.Visible = True
            _SNdytxtbox.Visible = True
            _SNKtxtbox.Visible = False
            _SNDescriptionLabel.Text = " Fixed Support restricts displacement & moment on all directions"
        ElseIf _SNSSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.RJ = False
            SNsupport.PS = False
            SNsupport.PRS = False
            SNsupport.FRS = False
            SNsupport.FS = False
            SNsupport.SS = True
            Label2.Visible = False
            Label3.Visible = False
            Label7.Visible = True
            _SNdxtxtbox.Visible = False
            _SNdytxtbox.Visible = False
            _SNKtxtbox.Visible = True
            _SNDescriptionLabel.Text = " Spring Support restricts displacement in Double plane with stiffness K but it will not restricts moment"
        End If
        _SNSupportPic.Refresh()
        _SupportPic.Refresh()
        WA2dt.MainPic.Refresh()
    End Sub

    Private Sub ENSupportOptionchange()
        If _ENPJoption.Checked = True Then
            ENsupport.PJ = True
            ENsupport.RJ = False
            ENsupport.PS = False
            ENsupport.PRS = False
            ENsupport.FRS = False
            ENsupport.FS = False
            ENsupport.SS = False
            Label4.Visible = False
            Label5.Visible = False
            Label8.Visible = False
            _ENdxtxtbox.Visible = False
            _ENdytxtbox.Visible = False
            _ENKtxtbox.Visible = False
            _ENDescriptionLabel.Text = " Pin Joint restricts displacement on all directions but it wont restraint moment"
        ElseIf _ENRJoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.RJ = True
            ENsupport.PS = False
            ENsupport.PRS = False
            ENsupport.FRS = False
            ENsupport.FS = False
            ENsupport.SS = False
            Label4.Visible = False
            Label5.Visible = False
            Label8.Visible = False
            _ENdxtxtbox.Visible = False
            _ENdytxtbox.Visible = False
            _ENKtxtbox.Visible = False
            _ENDescriptionLabel.Text = " Rigid Joint restricts displacement & moment on all directions"
        ElseIf _ENPSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.RJ = False
            ENsupport.PS = True
            ENsupport.PRS = False
            ENsupport.FRS = False
            ENsupport.FS = False
            ENsupport.SS = False
            Label4.Visible = True
            Label5.Visible = True
            Label8.Visible = False
            _ENdxtxtbox.Visible = True
            _ENdytxtbox.Visible = True
            _ENKtxtbox.Visible = False
            _ENDescriptionLabel.Text = " Pin Support restricts displacement in all directions but it will not restricts moment"
        ElseIf _ENPRSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.RJ = False
            ENsupport.PS = False
            ENsupport.PRS = True
            ENsupport.FRS = False
            ENsupport.FS = False
            ENsupport.SS = False
            Label4.Visible = True
            Label5.Visible = False
            Label8.Visible = False
            _ENdxtxtbox.Visible = False
            _ENdytxtbox.Visible = True
            _ENKtxtbox.Visible = False
            _ENDescriptionLabel.Text = "Pin Roller Support restricts displacement in Double plane but it will not restricts moment"
        ElseIf _ENFRSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.RJ = False
            ENsupport.PS = False
            ENsupport.PRS = False
            ENsupport.FRS = True
            ENsupport.FS = False
            ENsupport.SS = False
            Label4.Visible = True
            Label5.Visible = False
            Label8.Visible = False
            _ENdxtxtbox.Visible = False
            _ENdytxtbox.Visible = True
            _ENKtxtbox.Visible = False
            _ENDescriptionLabel.Text = "Fixed Roller Support restricts displacement in Double plane and restricts moment"
        ElseIf _ENFSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.RJ = False
            ENsupport.PS = False
            ENsupport.PRS = False
            ENsupport.FRS = False
            ENsupport.FS = True
            ENsupport.SS = False
            Label4.Visible = True
            Label5.Visible = True
            Label8.Visible = False
            _ENdxtxtbox.Visible = True
            _ENdytxtbox.Visible = True
            _ENKtxtbox.Visible = False
            _ENDescriptionLabel.Text = " Fixed Support restricts displacement & moment on all directions"
        ElseIf _ENSSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.RJ = False
            ENsupport.PS = False
            ENsupport.PRS = False
            ENsupport.FRS = False
            ENsupport.FS = False
            ENsupport.SS = True
            Label4.Visible = False
            Label5.Visible = False
            Label8.Visible = True
            _ENdxtxtbox.Visible = False
            _ENdytxtbox.Visible = False
            _ENKtxtbox.Visible = True
            _ENDescriptionLabel.Text = " Spring Support restricts displacement in Double plane with stiffness K but it will not restricts moment"
        End If
        _ENSupportPic.Refresh()
        _SupportPic.Refresh()
        WA2dt.MainPic.Refresh()
    End Sub

#Region "Paint Support Pic"
    Private Sub _SupportPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles _SupportPic.Paint
        Try
            SupportAreaPaintMember(e)
            SupportAreaPaintNode(e)
            SupportAreaPaintLoad(e)
            tempSUPPORTPaint(e)
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

    Private Sub tempSUPPORTPaint(ByRef e As System.Windows.Forms.PaintEventArgs)
        If _NodeTab.SelectedIndex = 0 Then
            e.Graphics.DrawRectangle(Pens.Black, TempEL.Spoint.X - 30, TempEL.Spoint.Y - 30, 60, 60)

        ElseIf _NodeTab.SelectedIndex = 1 Then
            e.Graphics.DrawRectangle(Pens.Black, TempEL.Epoint.X - 30, TempEL.Epoint.Y - 30, 60, 60)
        End If
        '--- Start Node
        If WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.PJ = True Then
            SupportPicPaintPinJoint(e, TempEL.Spoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.supportinclination)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.RJ = True Then
            SupportPicPaintRigidJoint(e, TempEL.Spoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.supportinclination)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.PS = True Then
            SupportPicPaintPinSupport(e, TempEL.Spoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.supportinclination, 0.5)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.PRS = True Then
            SupportPicPaintPinRollerSupport(e, TempEL.Spoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.supportinclination, 0.5)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.FRS = True Then
            SupportPicPaintFixedRollerSupport(e, TempEL.Spoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.supportinclination, 0.5)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.FS = True Then
            SupportPicPaintFixedSupport(e, TempEL.Spoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.supportinclination, 0.5)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.SS = True Then
            SupportPicPaintSpringSupport(e, TempEL.Spoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.supportinclination, 0.5)
        End If

        '--- End Node
        If WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.PJ = True Then
            SupportPicPaintPinJoint(e, TempEL.Epoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.supportinclination)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.RJ = True Then
            SupportPicPaintRigidJoint(e, TempEL.Epoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.supportinclination)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.PS = True Then
            SupportPicPaintPinSupport(e, TempEL.Epoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.supportinclination, 0.5)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.PRS = True Then
            SupportPicPaintPinRollerSupport(e, TempEL.Epoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.supportinclination, 0.5)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.FRS = True Then
            SupportPicPaintFixedRollerSupport(e, TempEL.Epoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.supportinclination, 0.5)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.FS = True Then
            SupportPicPaintFixedSupport(e, TempEL.Epoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.supportinclination, 0.5)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.SS = True Then
            SupportPicPaintSpringSupport(e, TempEL.Epoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.supportinclination, 0.5)
        End If
    End Sub
#End Region

#Region "Paint Start Node Support Pic Events"
    Private Sub _SNSupportPic_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _SNSupportPic.MouseClick
        'If SNsupport.Supportinclination = (45 * (Math.PI / 180)) Then
        '    SNsupport.Supportinclination = (90 * (Math.PI / 180))
        'ElseIf SNsupport.Supportinclination = (90 * (Math.PI / 180)) Then
        '    SNsupport.Supportinclination = (135 * (Math.PI / 180))
        'ElseIf SNsupport.Supportinclination = (135 * (Math.PI / 180)) Then
        '    SNsupport.Supportinclination = (180 * (Math.PI / 180))
        'Else
        '    SNsupport.Supportinclination = (45 * (Math.PI / 180))
        'End If
        _SNSupportPic.Refresh()
    End Sub

    Private Sub _SNSupportPic_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _SNSupportPic.MouseDown
        _DrgFlg = True
        _SNSupportPic.Refresh()
        _SupportPic.Refresh()
    End Sub

    Private Sub _SNSupportPic_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _SNSupportPic.MouseMove
        If _DrgFlg = True And (_SNPSoption.Checked = True Or _SNFSoption.Checked = True Or _SNPRSoption.Checked = True Or _SNFRSoption.Checked = True Or _SNSSoption.Checked = True) Then
            Dim Inclination As Double = If((e.Y - 80) < 0, -1 * Math.Acos((e.X - 80) / (Math.Sqrt(Math.Pow((e.X - 80), 2) + Math.Pow((e.Y - 80), 2)))), Math.Acos((e.X - 80) / (Math.Sqrt(Math.Pow((e.X - 80), 2) + Math.Pow((e.Y - 80), 2)))))
            Dim a As Integer = (Inclination) * (180 / Math.PI)
            SNsupport.Supportinclination = a * (Math.PI / 180)
            _SNSupportPic.Refresh()
            _SupportPic.Refresh()
            SNSupportOptionchange()
        End If
    End Sub

    Private Sub _SNSupportPic_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _SNSupportPic.MouseUp
        _DrgFlg = False
        _SNSupportPic.Refresh()
        _SupportPic.Refresh()
    End Sub

    Private Sub _SNSupportPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles _SNSupportPic.Paint
        e.Graphics.DrawLine(Pens.BurlyWood, 80, 0, 80, 160)
        e.Graphics.DrawLine(Pens.BurlyWood, 0, 80, 160, 80)

        e.Graphics.DrawLine(New Pen(OptionD.Mc, 4), New Point(80, 80), New Point(80 + (TempEL.Epoint.X - TempEL.Spoint.X), 80 + (TempEL.Epoint.Y - TempEL.Spoint.Y)))
        e.Graphics.DrawString(TempEL.SNode, New Font("Verdana", 10), New Pen(OptionD.Nc, 4).Brush, New Point(50, 50))

        If SNsupport.PJ = True Then
            SupportPicPaintPinJoint(e, New Point(80, 80), SNsupport.Supportinclination)
        ElseIf SNsupport.RJ = True Then
            SupportPicPaintRigidJoint(e, New Point(80, 80), SNsupport.Supportinclination)
        ElseIf SNsupport.PS = True Then
            SupportPicPaintPinSupport(e, New Point(80, 80), SNsupport.Supportinclination, 1)
        ElseIf SNsupport.PRS = True Then
            SupportPicPaintPinRollerSupport(e, New Point(80, 80), SNsupport.Supportinclination, 1)
        ElseIf SNsupport.FRS = True Then
            SupportPicPaintFixedRollerSupport(e, New Point(80, 80), SNsupport.Supportinclination, 1)
        ElseIf SNsupport.FS = True Then
            SupportPicPaintFixedSupport(e, New Point(80, 80), SNsupport.Supportinclination, 1)
        ElseIf SNsupport.SS = True Then
            SupportPicPaintSpringSupport(e, New Point(80, 80), SNsupport.Supportinclination, 1)
        End If
    End Sub
#End Region

#Region "Paint End Node Support Pic Events"
    Private Sub _ENSupportPic_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _ENSupportPic.MouseClick
        'If SNsupport.Supportinclination = (45 * (Math.PI / 180)) Then
        '    SNsupport.Supportinclination = (90 * (Math.PI / 180))
        'ElseIf SNsupport.Supportinclination = (90 * (Math.PI / 180)) Then
        '    SNsupport.Supportinclination = (135 * (Math.PI / 180))
        'ElseIf SNsupport.Supportinclination = (135 * (Math.PI / 180)) Then
        '    SNsupport.Supportinclination = (180 * (Math.PI / 180))
        'Else
        '    SNsupport.Supportinclination = (45 * (Math.PI / 180))
        'End If
        _ENSupportPic.Refresh()
    End Sub

    Private Sub _ENSupportPic_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _ENSupportPic.MouseDown
        _DrgFlg = True
        _ENSupportPic.Refresh()
        _SupportPic.Refresh()
    End Sub

    Private Sub _ENSupportPic_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _ENSupportPic.MouseMove
        If _DrgFlg = True And (_ENPSoption.Checked = True Or _ENFSoption.Checked = True Or _ENPRSoption.Checked = True Or _ENFRSoption.Checked = True Or _ENSSoption.Checked = True) Then
            Dim Inclination As Double = If((e.Y - 80) < 0, -1 * Math.Acos((e.X - 80) / (Math.Sqrt(Math.Pow((e.X - 80), 2) + Math.Pow((e.Y - 80), 2)))), Math.Acos((e.X - 80) / (Math.Sqrt(Math.Pow((e.X - 80), 2) + Math.Pow((e.Y - 80), 2)))))
            Dim a As Integer = (Inclination) * (180 / Math.PI)
            ENsupport.Supportinclination = a * (Math.PI / 180)
            _ENSupportPic.Refresh()
            _SupportPic.Refresh()
            ENSupportOptionchange()
        End If
    End Sub

    Private Sub _ENSupportPic_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _ENSupportPic.MouseUp
        _DrgFlg = False
        _ENSupportPic.Refresh()
        _SupportPic.Refresh()
    End Sub

    Private Sub _ENSupportPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles _ENSupportPic.Paint
        e.Graphics.DrawLine(Pens.BurlyWood, 80, 0, 80, 160)
        e.Graphics.DrawLine(Pens.BurlyWood, 0, 80, 160, 80)

        e.Graphics.DrawLine(New Pen(OptionD.Mc, 4), New Point(80, 80), New Point((80 + (TempEL.Spoint.X - TempEL.Epoint.X)), (80 + (TempEL.Spoint.Y - TempEL.Epoint.Y))))
        e.Graphics.DrawString(TempEL.ENode, New Font("Verdana", 10), New Pen(OptionD.Nc, 4).Brush, New Point(50, 50))

        If ENsupport.PJ = True Then
            SupportPicPaintPinJoint(e, New Point(80, 80), ENsupport.Supportinclination)
        ElseIf ENsupport.RJ = True Then
            SupportPicPaintRigidJoint(e, New Point(80, 80), ENsupport.Supportinclination)
        ElseIf ENsupport.PS = True Then
            SupportPicPaintPinSupport(e, New Point(80, 80), ENsupport.Supportinclination, 1)
        ElseIf ENsupport.PRS = True Then
            SupportPicPaintPinRollerSupport(e, New Point(80, 80), ENsupport.Supportinclination, 1)
        ElseIf ENsupport.FRS = True Then
            SupportPicPaintFixedRollerSupport(e, New Point(80, 80), ENsupport.Supportinclination, 1)
        ElseIf ENsupport.FS = True Then
            SupportPicPaintFixedSupport(e, New Point(80, 80), ENsupport.Supportinclination, 1)
        ElseIf ENsupport.SS = True Then
            SupportPicPaintSpringSupport(e, New Point(80, 80), ENsupport.Supportinclination, 1)
        End If
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

        If _DrgFlg = True Then
            e.Graphics.DrawLine(Pens.DarkViolet, New Point(M.X, M.Y), New Point(M.X + (TempEL.CurrentLength * Math.Cos(Inclination)), M.Y + (TempEL.CurrentLength * Math.Sin(Inclination))))
            Dim a As Integer = (Inclination) * (180 / Math.PI)
            e.Graphics.DrawArc(New Pen(Color.DarkViolet, 2), M.X - 40, M.Y - 40, 80, 80, 0, a)
            e.Graphics.DrawString(a, New Font("Verdana", 8), Brushes.DarkViolet, New Point(M.X + (40 * Math.Cos((a / 2) * (Math.PI / 180))), M.Y + (40 * Math.Sin((a / 2) * (Math.PI / 180)))))
        End If
    End Sub

    Private Sub SupportPicPaintPinRollerSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))

        If _DrgFlg = True Then
            e.Graphics.DrawLine(Pens.DarkViolet, New Point(M.X, M.Y), New Point(M.X + (TempEL.CurrentLength * Math.Cos(Inclination)), M.Y + (TempEL.CurrentLength * Math.Sin(Inclination))))
            Dim a As Integer = (Inclination) * (180 / Math.PI)
            e.Graphics.DrawArc(New Pen(Color.DarkViolet, 2), M.X - 40, M.Y - 40, 80, 80, 0, a)
            e.Graphics.DrawString(a, New Font("Verdana", 8), Brushes.DarkViolet, New Point(M.X + (40 * Math.Cos((a / 2) * (Math.PI / 180))), M.Y + (40 * Math.Sin((a / 2) * (Math.PI / 180)))))
        End If
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

        If _DrgFlg = True Then
            e.Graphics.DrawLine(Pens.DarkViolet, New Point(M.X, M.Y), New Point(M.X + (TempEL.CurrentLength * Math.Cos(Inclination)), M.Y + (TempEL.CurrentLength * Math.Sin(Inclination))))
            Dim a As Integer = (Inclination) * (180 / Math.PI)
            e.Graphics.DrawArc(New Pen(Color.DarkViolet, 2), M.X - 40, M.Y - 40, 80, 80, 0, a)
            e.Graphics.DrawString(a, New Font("Verdana", 8), Brushes.DarkViolet, New Point(M.X + (40 * Math.Cos((a / 2) * (Math.PI / 180))), M.Y + (40 * Math.Sin((a / 2) * (Math.PI / 180)))))
        End If
    End Sub

    Private Sub SupportPicPaintFixedSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
        '---- Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((0 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((0 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((0 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (25 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (25 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((0 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((0 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-25 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-25 * Math.Cos(-Inclination))) * ZMfactor)))

        If _DrgFlg = True Then
            e.Graphics.DrawLine(Pens.DarkViolet, New Point(M.X, M.Y), New Point(M.X + (TempEL.CurrentLength * Math.Cos(Inclination)), M.Y + (TempEL.CurrentLength * Math.Sin(Inclination))))
            Dim a As Integer = (Inclination) * (180 / Math.PI)
            e.Graphics.DrawArc(New Pen(Color.DarkViolet, 2), M.X - 40, M.Y - 40, 80, 80, 0, a)
            e.Graphics.DrawString(a, New Font("Verdana", 8), Brushes.DarkViolet, New Point(M.X + (40 * Math.Cos((a / 2) * (Math.PI / 180))), M.Y + (40 * Math.Sin((a / 2) * (Math.PI / 180)))))
        End If
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
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZMfactor)))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZMfactor)))

        If _DrgFlg = True Then
            e.Graphics.DrawLine(Pens.DarkViolet, New Point(M.X, M.Y), New Point(M.X + (TempEL.CurrentLength * Math.Cos(Inclination)), M.Y + (TempEL.CurrentLength * Math.Sin(Inclination))))
            Dim a As Integer = (Inclination) * (180 / Math.PI)
            e.Graphics.DrawArc(New Pen(Color.DarkViolet, 2), M.X - 40, M.Y - 40, 80, 80, 0, a)
            e.Graphics.DrawString(a, New Font("Verdana", 8), Brushes.DarkViolet, New Point(M.X + (40 * Math.Cos((a / 2) * (Math.PI / 180))), M.Y + (40 * Math.Sin((a / 2) * (Math.PI / 180)))))
        End If
    End Sub
#End Region

    Private Sub _SNAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _SNAdd.Click
        If _SNPJoption.Checked = True Then
            SNsupport.PJ = True
            SNsupport.RJ = False
            SNsupport.PS = False
            SNsupport.PRS = False
            SNsupport.FRS = False
            SNsupport.FS = False
            SNsupport.SS = False
        ElseIf _SNRJoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.RJ = True
            SNsupport.PS = False
            SNsupport.PRS = False
            SNsupport.FRS = False
            SNsupport.FS = False
            SNsupport.SS = False
        ElseIf _SNPSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.RJ = False
            SNsupport.PS = True
            SNsupport.PRS = False
            SNsupport.FRS = False
            SNsupport.FS = False
            SNsupport.SS = False
        ElseIf _SNPRSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.RJ = False
            SNsupport.PS = False
            SNsupport.PRS = True
            SNsupport.FRS = False
            SNsupport.FS = False
            SNsupport.SS = False
        ElseIf _SNFRSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.RJ = False
            SNsupport.PS = False
            SNsupport.PRS = False
            SNsupport.FRS = True
            SNsupport.FS = False
            SNsupport.SS = False
        ElseIf _SNFSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.RJ = False
            SNsupport.PS = False
            SNsupport.PRS = False
            SNsupport.FRS = False
            SNsupport.FS = True
            SNsupport.SS = False
        ElseIf _SNSSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.RJ = False
            SNsupport.PS = False
            SNsupport.PRS = False
            SNsupport.FRS = False
            SNsupport.FS = False
            SNsupport.SS = True
        End If

        Dim NoteIndex As Integer = WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.index
        WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support = _
                                          New Line2DT.Node.SupportCondition(SNsupport.PJ, _
                                                                            SNsupport.RJ, _
                                                                            SNsupport.PS, _
                                                                            SNsupport.PRS, _
                                                                            SNsupport.FRS, _
                                                                            SNsupport.FS, _
                                                                            SNsupport.SS, _
                                                                            SNsupport.Supportinclination, _
                                                                            dx:=If(Val(_SNdxtxtbox.Text) <> 0, Val(_SNdxtxtbox.Text), Nothing), _
                                                                            dy:=If(Val(_SNdytxtbox.Text) <> 0, Val(_SNdytxtbox.Text), Nothing), _
                                                                            k:=If(Val(_SNKtxtbox.Text) <> 0, Val(_SNKtxtbox.Text), Nothing))
        For Each El In WA2dt.Mem
            If El.SN.index = NoteIndex Then
                WA2dt.Mem(WA2dt.Mem.IndexOf(El)).SN.Support = _
                                          New Line2DT.Node.SupportCondition(SNsupport.PJ, _
                                                                            SNsupport.RJ, _
                                                                            SNsupport.PS, _
                                                                            SNsupport.PRS, _
                                                                            SNsupport.FRS, _
                                                                            SNsupport.FS, _
                                                                            SNsupport.SS, _
                                                                            SNsupport.Supportinclination, _
                                                                            dx:=If(Val(_SNdxtxtbox.Text) <> 0, Val(_SNdxtxtbox.Text), Nothing), _
                                                                            dy:=If(Val(_SNdytxtbox.Text) <> 0, Val(_SNdytxtbox.Text), Nothing), _
                                                                            k:=If(Val(_SNKtxtbox.Text) <> 0, Val(_SNKtxtbox.Text), Nothing))
            ElseIf El.SN.index = NoteIndex Then
                WA2dt.Mem(WA2dt.Mem.IndexOf(El)).SN.Support = _
                                          New Line2DT.Node.SupportCondition(SNsupport.PJ, _
                                                                            SNsupport.RJ, _
                                                                            SNsupport.PS, _
                                                                            SNsupport.PRS, _
                                                                            SNsupport.FRS, _
                                                                            SNsupport.FS, _
                                                                            SNsupport.SS, _
                                                                            SNsupport.Supportinclination, _
                                                                            dx:=If(Val(_SNdxtxtbox.Text) <> 0, Val(_SNdxtxtbox.Text), Nothing), _
                                                                            dy:=If(Val(_SNdytxtbox.Text) <> 0, Val(_SNdytxtbox.Text), Nothing), _
                                                                            k:=If(Val(_SNKtxtbox.Text) <> 0, Val(_SNKtxtbox.Text), Nothing))
            End If
        Next
        For Each Nd In WA2dt.Bob
            If Nd.index = NoteIndex Then
                WA2dt.Bob(WA2dt.Bob.IndexOf(Nd)).Support = _
                                          New Line2DT.Node.SupportCondition(SNsupport.PJ, _
                                                                            SNsupport.RJ, _
                                                                            SNsupport.PS, _
                                                                            SNsupport.PRS, _
                                                                            SNsupport.FRS, _
                                                                            SNsupport.FS, _
                                                                            SNsupport.SS, _
                                                                            SNsupport.Supportinclination, _
                                                                            dx:=If(Val(_SNdxtxtbox.Text) <> 0, Val(_SNdxtxtbox.Text), Nothing), _
                                                                            dy:=If(Val(_SNdytxtbox.Text) <> 0, Val(_SNdytxtbox.Text), Nothing), _
                                                                            k:=If(Val(_SNKtxtbox.Text) <> 0, Val(_SNKtxtbox.Text), Nothing))
            End If
        Next
        _SNSupportPic.Refresh()
        _SupportPic.Refresh()
        WA2dt.MainPic.Refresh()
    End Sub

    Private Sub _ENAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ENAdd.Click
        If _ENPJoption.Checked = True Then
            ENsupport.PJ = True
            ENsupport.RJ = False
            ENsupport.PS = False
            ENsupport.PRS = False
            ENsupport.FRS = False
            ENsupport.FS = False
            ENsupport.SS = False
        ElseIf _ENRJoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.RJ = True
            ENsupport.PS = False
            ENsupport.PRS = False
            ENsupport.FRS = False
            ENsupport.FS = False
            ENsupport.SS = False
        ElseIf _ENPSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.RJ = False
            ENsupport.PS = True
            ENsupport.PRS = False
            ENsupport.FRS = False
            ENsupport.FS = False
            ENsupport.SS = False
        ElseIf _ENPRSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.RJ = False
            ENsupport.PS = False
            ENsupport.PRS = True
            ENsupport.FRS = False
            ENsupport.FS = False
            ENsupport.SS = False
        ElseIf _ENFRSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.RJ = False
            ENsupport.PS = False
            ENsupport.PRS = False
            ENsupport.FRS = True
            ENsupport.FS = False
            ENsupport.SS = False
        ElseIf _ENFSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.RJ = False
            ENsupport.PS = False
            ENsupport.PRS = False
            ENsupport.FRS = False
            ENsupport.FS = True
            ENsupport.SS = False
        ElseIf _ENSSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.RJ = False
            ENsupport.PS = False
            ENsupport.PRS = False
            ENsupport.FRS = False
            ENsupport.FS = False
            ENsupport.SS = True
        End If

        Dim NoteIndex As Integer = WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.index
        WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support = _
                                          New Line2DT.Node.SupportCondition(ENsupport.PJ, _
                                                                            ENsupport.RJ, _
                                                                            ENsupport.PS, _
                                                                            ENsupport.PRS, _
                                                                            ENsupport.FRS, _
                                                                            ENsupport.FS, _
                                                                            ENsupport.SS, _
                                                                            ENsupport.Supportinclination, _
                                                                            dx:=If(Val(_ENdxtxtbox.Text) <> 0, Val(_ENdxtxtbox.Text), Nothing), _
                                                                            dy:=If(Val(_ENdytxtbox.Text) <> 0, Val(_ENdytxtbox.Text), Nothing), _
                                                                            k:=If(Val(_SNKtxtbox.Text) <> 0, Val(_ENKtxtbox.Text), Nothing))
        For Each El In WA2dt.Mem
            If El.SN.index = NoteIndex Then
                WA2dt.Mem(WA2dt.Mem.IndexOf(El)).SN.Support = _
                                          New Line2DT.Node.SupportCondition(ENsupport.PJ, _
                                                                            ENsupport.RJ, _
                                                                            ENsupport.PS, _
                                                                            ENsupport.PRS, _
                                                                            ENsupport.FRS, _
                                                                            ENsupport.FS, _
                                                                            ENsupport.SS, _
                                                                            ENsupport.Supportinclination, _
                                                                            dx:=If(Val(_ENdxtxtbox.Text) <> 0, Val(_ENdxtxtbox.Text), Nothing), _
                                                                            dy:=If(Val(_ENdytxtbox.Text) <> 0, Val(_ENdytxtbox.Text), Nothing), _
                                                                            k:=If(Val(_ENKtxtbox.Text) <> 0, Val(_ENKtxtbox.Text), Nothing))
            ElseIf El.EN.index = NoteIndex Then
                WA2dt.Mem(WA2dt.Mem.IndexOf(El)).EN.Support = _
                                          New Line2DT.Node.SupportCondition(ENsupport.PJ, _
                                                                            ENsupport.RJ, _
                                                                            ENsupport.PS, _
                                                                            ENsupport.PRS, _
                                                                            ENsupport.FRS, _
                                                                            ENsupport.FS, _
                                                                            ENsupport.SS, _
                                                                            ENsupport.Supportinclination, _
                                                                            dx:=If(Val(_ENdxtxtbox.Text) <> 0, Val(_ENdxtxtbox.Text), Nothing), _
                                                                            dy:=If(Val(_ENdytxtbox.Text) <> 0, Val(_ENdytxtbox.Text), Nothing), _
                                                                            k:=If(Val(_ENKtxtbox.Text) <> 0, Val(_ENKtxtbox.Text), Nothing))
            End If
        Next
        For Each Nd In WA2dt.Bob
            If Nd.index = NoteIndex Then
                WA2dt.Bob(WA2dt.Bob.IndexOf(Nd)).Support = _
                                          New Line2DT.Node.SupportCondition(ENsupport.PJ, _
                                                                            ENsupport.RJ, _
                                                                            ENsupport.PS, _
                                                                            ENsupport.PRS, _
                                                                            ENsupport.FRS, _
                                                                            ENsupport.FS, _
                                                                            ENsupport.SS, _
                                                                            ENsupport.Supportinclination, _
                                                                            dx:=If(Val(_ENdxtxtbox.Text) <> 0, Val(_ENdxtxtbox.Text), Nothing), _
                                                                            dy:=If(Val(_ENdytxtbox.Text) <> 0, Val(_ENdytxtbox.Text), Nothing), _
                                                                            k:=If(Val(_ENKtxtbox.Text) <> 0, Val(_ENKtxtbox.Text), Nothing))
            End If
        Next
        _ENSupportPic.Refresh()
        _SupportPic.Refresh()
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