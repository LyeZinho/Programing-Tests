Imports System.Data.SQLite


Public Module Hash
    Public Function Sha1(_imput) As String
        Dim sha1Obj As New Security.Cryptography.SHA1CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(_imput)

        bytesToHash = sha1Obj.ComputeHash(bytesToHash)

        Dim strResult As String = ""

        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2")
        Next
        Return strResult
    End Function
End Module

Public Module SqlInjection

    Public Function FindSQLI(_input) As Integer
        Dim Filter As String = "insert INSERT where WHERE if IF select SELECT update UPDATE delete DELETE create CREATE"
        Dim result As Integer = Filter.IndexOf(_input)
        '0 = confirmed sql injection
        '1 = not confimed sql injection
        If result = -1 Then
            Return 0
        ElseIf Not (result = -1) Then
            Return 1
        End If
        Return 3
    End Function
End Module

Public Class Cliente
    Private inome As String
    Private iidade As Integer
    Private inif As Integer
    Private ReadOnly isaldo As Double
    Private ReadOnly ihashPassword As String
    Private userIsValid As Boolean

    Property Nome() As String
        Get
            Nome = inome
        End Get
        Set(ByVal Vaule As String)
            Dim result As Integer = SqlInjection.FindSQLI(Vaule)
            If result = 0 Then
                userIsValid = False
            ElseIf result = 1 Then
                userIsValid = True
                inome = Vaule
            End If
        End Set
    End Property

    Property Idade() As Integer
        Get
            Idade = iidade
        End Get
        Set(ByVal Vaule As Integer)
            If Idade < 18 Then
                userIsValid = False
            ElseIf Idade < 0 Then
                userIsValid = False
            Else
                userIsValid = True
                iidade = Vaule
            End If
        End Set
    End Property


    Property Nif() As Integer
        Get
            Nif = inif
        End Get
        Set(ByVal Vaule As Integer)
            If Idade < 18 Then
                userIsValid = False
            ElseIf Idade < 0 Then
                userIsValid = False
            Else
                userIsValid = True
                inif = Vaule
            End If
        End Set
    End Property


    '[SQL]
    Private Shared Sub NewClientTable()
        Dim sqlite_conn As SQLiteConnection
        Dim sqlite_cmd As SQLiteCommand
        sqlite_conn = New SQLiteConnection("Data Source=clients.sqlite;Version=3;")
        sqlite_conn.Open()
        sqlite_cmd = sqlite_conn.CreateCommand()
        sqlite_cmd.CommandText = "CREATE TABLE IF NOT EXISTS Clientes (nome CHAR(255), idade INT, nif INT, saldo DOUBLE PRECISION, hashpass CHAR(255));"
        sqlite_cmd.ExecuteNonQuery()
    End Sub

    Private Sub InsertIntoClientes()
        Dim sqlite_conn As SQLiteConnection
        Dim sqlite_cmd As SQLiteCommand
        sqlite_conn = New SQLiteConnection("Data Source=clients.sqlite;Version=3;")
        sqlite_conn.Open()
        sqlite_cmd = sqlite_conn.CreateCommand()
        sqlite_cmd.CommandText = "INSERT INTO Clientes (nome, idade, nif, saldo, hashpass) VALUES ('" + inome + "'," + iidade.ToString + "," + inif.ToString + "," + isaldo.ToString + ",'" + ihashPassword + "');"
        sqlite_cmd.ExecuteNonQuery()
    End Sub

    Private Function ReadSaldoClientes(_nome As String) As String
        Dim sqlite_conn As SQLiteConnection
        Dim sqlite_cmd As SQLiteCommand
        Dim sqlite_datareader As SQLiteDataReader

        sqlite_conn = New SQLiteConnection("Data Source=database.sqlite;Version=3;New=True;")
        sqlite_conn.Open()
        sqlite_cmd = sqlite_conn.CreateCommand()

        sqlite_cmd.CommandText = "SELECT saldo FROM Clients WHERE nome =" + _nome + ";"
        sqlite_datareader = sqlite_cmd.ExecuteReader()

        Dim saldoReader As Double

        ' The SQLiteDataReader allows us to run through each row per loop
        While (sqlite_datareader.Read())
            saldoReader = sqlite_datareader.GetDouble(0)
        End While

        Return saldoReader
    End Function
End Class

Public Class Dados


End Class