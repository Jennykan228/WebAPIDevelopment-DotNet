Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports Newtonsoft.Json

''' <summary>
''' A rare and valuable treasure owned by a dragon.
''' </summary>
Public Class DragonTreasure
    ''' <summary>
    ''' The unique identifier for the treasure.
    ''' </summary>
    <Key>
    Public Property Id As Integer

    ''' <summary>
    ''' The name of the treasure (2-50 characters).
    ''' </summary>
    <Required>
    <StringLength(50, MinimumLength:=2, ErrorMessage:="Describe your loot in 50 chars or less")>
    Public Property Name As String

    ''' <summary>
    ''' A detailed description of the treasure.
    ''' </summary>
    <Required>
    Public Property Description As String

    ''' <summary>
    ''' The estimated value of this treasure, in gold coins.
    ''' </summary>
    <Range(0, Integer.MaxValue, ErrorMessage:="Value must be greater than or equal to 0")>
    Public Property Value As Integer = 0

    ''' <summary>
    ''' A rating of how cool this treasure is (0-10).
    ''' </summary>
    <Range(0, 10, ErrorMessage:="Cool factor must be between 0 and 10")>
    Public Property CoolFactor As Integer = 0

    ''' <summary>
    ''' The date and time when this treasure was plundered.
    ''' </summary>
    Public Property PlunderedAt As DateTime

    ''' <summary>
    ''' Whether this treasure is published and visible to the public.
''' </summary>
    Public Property IsPublished As Boolean = False

  ''' <summary>
    ''' The username of the dragon who owns this treasure.
    ''' </summary>
    <Required>
    <StringLength(100, ErrorMessage:="Owner name cannot exceed 100 characters")>
    Public Property Owner As String

    ''' <summary>
    ''' Gets a shortened version of the description (max 40 characters).
    ''' </summary>
    <JsonIgnore>
    Public ReadOnly Property ShortDescription As String
        Get
        If String.IsNullOrEmpty(Description) Then
           Return ""
          End If
            Return If(Description.Length <= 40, Description, Description.Substring(0, 37) + "...")
        End Get
    End Property

    ''' <summary>
    ''' Gets a human-readable representation of when this treasure was plundered.
    ''' </summary>
    <JsonIgnore>
    Public ReadOnly Property PlunderedAtAgo As String
        Get
   Dim timeSpan = DateTime.Now - PlunderedAt
            If timeSpan.TotalDays > 365 Then
              Dim years = Math.Floor(timeSpan.TotalDays / 365)
Return $"{years} year{If(years = 1, "", "s")} ago"
         ElseIf timeSpan.TotalDays > 30 Then
          Dim months = Math.Floor(timeSpan.TotalDays / 30)
         Return $"{months} month{If(months = 1, "", "s")} ago"
            ElseIf timeSpan.TotalDays >= 1 Then
     Dim days = Math.Floor(timeSpan.TotalDays)
                Return $"{days} day{If(days = 1, "", "s")} ago"
         ElseIf timeSpan.TotalHours >= 1 Then
 Dim hours = Math.Floor(timeSpan.TotalHours)
       Return $"{hours} hour{If(hours = 1, "", "s")} ago"
            Else
                Dim minutes = Math.Floor(timeSpan.TotalMinutes)
       Return $"{minutes} minute{If(minutes = 1, "", "s")} ago"
 End If
      End Get
    End Property

    ''' <summary>
    ''' Initializes a new instance of the DragonTreasure class.
    ''' </summary>
    Public Sub New()
        PlunderedAt = DateTime.Now
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the DragonTreasure class with a name.
    ''' </summary>
    ''' <param name="name">The name of the treasure.</param>
    Public Sub New(name As String)
     Me.New()
    Me.Name = name
    End Sub
End Class