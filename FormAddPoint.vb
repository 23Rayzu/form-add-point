Imports DotSpatial.Controls
Imports DotSpatial.Data
Imports DotSpatial.Symbology
Imports System.IO
Public Class formAddPoint

    Private asetFeatureSet As Object
    Private lyrAdministrasi As Object
    Public AppPath As String = Application.ExecutablePath
    Public ResourcesPath As String = AppPath.ToUpper.Replace("\Creating_Geospatial_Information_System.exe", "\Resources")

    Private Sub cmdBrowse_Click(sender As Object, e As EventArgs) Handles cmd_Browse.Click
        Dim ofd As OpenFileDialog = New OpenFileDialog()
        ofd.Title = “Browse Photo”
        ofd.InitialDirectory = “C:\”
        ofd.Filter = “JPG (*.jpg)|*.jpg|JPEG (*.jpeg)|*.jpeg|PNG (*.png)|*.png|All files (*.*)|*.*”
        ofd.FilterIndex = 1
        ofd.RestoreDirectory = True

        If (ofd.ShowDialog() = DialogResult.OK) Then
            Dim fileName As String = Path.GetFileName(ofd.FileName)
            Dim sourcePath As String = Path.GetDirectoryName(ofd.FileName)
            Dim targetPath As String = Path.Combine(Form1.ResourcesPath, "\Spatial\ADMINISTRASIKECAMATAN_AR_50K.shp")
            Dim sourceFile As String = Path.Combine(sourcePath, fileName)
            Dim destFile As String = Path.Combine(targetPath, fileName)
            File.Copy(sourceFile, destFile, True)
            txtFoto.Text = fileName
            Map1.ClearLayers()
            Map1.AddLayer(destFile)
        Else
            MessageBox.Show("BELUM MEMILIH FOTO!!", "Report", MessageBoxButtons.OK)
        End If
    End Sub

    Private Sub CmdZoomOut_Click(sender As Object, e As EventArgs) Handles cmd_ZoomOut.Click
        Map1.FunctionMode = DotSpatial.Controls.FunctionMode.ZoomOut
    End Sub

    Private Sub cmdPan_Click(sender As Object, e As EventArgs) Handles cmd_Pan.Click
        Map1.FunctionMode = DotSpatial.Controls.FunctionMode.Pan
    End Sub

    Private Sub CmdZoomIn_Click(sender As Object, e As EventArgs) Handles cmd_ZoomIn.Click
        Map1.FunctionMode = DotSpatial.Controls.FunctionMode.ZoomIn
    End Sub

    Private Sub CmdFullExtent_Click(sender As Object, e As EventArgs) Handles cmd_FullExtent.Click
        Map1.ZoomToMaxExtent()
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmd_Save.Click
        Dim PemerintahFeatureSet As FeatureSet = Form1.lyrPemerintah.FeatureSet
        Dim PemerintahPoint As New Point(CDbl(txtTitikX.Text), CDbl(txtTitikY.Text))
        Dim featureInserted As IFeature = asetFeatureSet.AddFeature(asetFeatureSet)
        featureInserted.DataRow.BeginEdit()
        featureInserted.DataRow("kode") = txtKode.Text
        featureInserted.DataRow("nama_aset") = txtNamaAset.Text
        featureInserted.DataRow("jenis_aset") = txtJenisAset.Text
        featureInserted.DataRow("atas_nama") = txtAtasNama.Text
        featureInserted.DataRow("foto") = txtFoto.Text
        featureInserted.DataRow.EndEdit()

        PemerintahFeatureSet.InitializeVertices()
        PemerintahFeatureSet.UpdateExtent()
        PemerintahFeatureSet.Save()

        Form1.lyrPemerintah.DataSet.InitializeVertices()
        Form1.lyrPemerintah.AssignFastDrawnStates()
        Form1.lyrPemerintah.DataSet.UpdateExtent()

        Dim dt As DataTable
        dt = Form1.lyrPemerintah.DataSet.DataTable
        dt.Columns.RemoveAt((dt.Columns.Count - 1))
        dt.AcceptChanges()
        Form1.lyrPemerintah.DataSet.Save()
        Form1.lyrPemerintah.FeatureSet.AddFid()
        Form1.lyrPemerintah.FeatureSet.Save()

        Form1.pointLayerTemplate.SelectAll()
        Form1.pointLayerTemplate.RemoveSelectedFeatures()

        Form1.pointFeatureTemplate.InitializeVertices()
        Form1.pointFeatureTemplate.UpdateExtent()
        Form1.pointLayerTemplate.DataSet.InitializeVertices()
        Form1.pointLayerTemplate.AssignFastDrawnStates()
        Form1.pointLayerTemplate.DataSet.UpdateExtent()

        Map1.Refresh()
        Map1.ResetBuffer()

        MessageBox.Show("Data tersimpan")
    End Sub
    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click

    End Sub

    Private Sub Map1_Load(sender As Object, e As EventArgs) Handles Map1.Load
        lyrAdministrasi = Map1.Layers.Add(ResourcesPath & "\Resources\Database\Spatial\ADMINISTRASIKECAMATAN_AR_50K.shp")
    End Sub

    Private Sub Map1_Click(sender As Object, e As EventArgs) Handles Map1.Click
        lyrAdministrasi = Map1.Layers.Add(ResourcesPath & "\Resources\Database\Spatial\ADMINISTRASIKECAMATAN_AR_50K.shp")
    End Sub

    Private Sub formAddPoint_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class