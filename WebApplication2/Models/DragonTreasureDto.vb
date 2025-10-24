''' <summary>
''' Data Transfer Object for DragonTreasure API responses.
''' </summary>
Public Class DragonTreasureDto
    ''' <summary>
    ''' The unique identifier for the treasure.
    ''' </summary>
    Public Property Id As Integer

    ''' <summary>
 ''' The name of the treasure.
    ''' </summary>
    Public Property Name As String

  ''' <summary>
    ''' A detailed description of the treasure.
    ''' </summary>
    Public Property Description As String

    ''' <summary>
''' A shortened version of the description.
    ''' </summary>
    Public Property ShortDescription As String

    ''' <summary>
    ''' The estimated value of this treasure, in gold coins.
    ''' </summary>
    Public Property Value As Integer

    ''' <summary>
    ''' A rating of how cool this treasure is (0-10).
    ''' </summary>
    Public Property CoolFactor As Integer

    ''' <summary>
    ''' The date and time when this treasure was plundered.
    ''' </summary>
    Public Property PlunderedAt As DateTime

    ''' <summary>
    ''' A human-readable representation of when this treasure was plundered.
    ''' </summary>
    Public Property PlunderedAtAgo As String

    ''' <summary>
  ''' Whether this treasure is published and visible to the public.
    ''' </summary>
    Public Property IsPublished As Boolean

    ''' <summary>
    ''' The username of the dragon who owns this treasure.
    ''' </summary>
    Public Property Owner As String
End Class